//
// UrlConnection.cs
//
// Author:
//   Eduardo Scoz (contact@escoz.com)
//
// Copyright 2010, ESCOZ, Inc.
//
// Code licensed under the MIT X11 license
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using MonoTouch.Foundation;
using System.Collections.Generic;
using MonoTouch.UIKit;
namespace Escoz.Util
{

	public class UrlConnection:NSUrlConnection {
		
		private static Dictionary<string, UrlConnection> Connections = new Dictionary<string, UrlConnection>();
		
		public static void KillAllConnections() {
			
			foreach (UrlConnection c in Connections.Values) {
				c.Cancel();
			}
			Connections.Clear();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}
		
		public static void KillConnection(string name) {
			if (!Connections.ContainsKey(name))
				return;
			
			Connections[name].Cancel();
			Connections.Remove(name);
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = Connections.Count>0;
		}
		
		public static void ConnectionEnded(string name) {
			Connections.Remove(name);
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = Connections.Count>0;
		}
		
		public static bool IsDownloading(string name) {
			return Connections.ContainsKey(name);	
		}
		
		public static void StartedDownloading(string name, UrlConnection connection){
			Connections.Add(name, connection);
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}
		
		public UrlConnection(string name, NSUrlRequest request, Action<string> c):base(request, new UrlDelegate(name, c), true) {
			if (Connections.ContainsKey(name)) {
				KillConnection(name);
			}
			StartedDownloading(name, this);
		}
		
		public UrlConnection(string name, NSUrlRequest request, Action<string> success, Action<NSError> failure):base(request, new UrlDelegate(name, success, failure), true) {
			if (Connections.ContainsKey(name)) {
				KillConnection(name);
			}
			StartedDownloading(name, this);
		}
		
		public UrlConnection(string name, NSUrlRequest request, Action<UIImage> c):base(request, new UrlDelegate(name, c), true) {
			if (Connections.ContainsKey(name)) {
				KillConnection(name);
			}
			StartedDownloading(name, this);
		}
		
		public UrlConnection(string name, NSUrlRequest request, Action<UIImage> success, Action<NSError> failure):base(request, new UrlDelegate(name, success, failure), true) {
			if (Connections.ContainsKey(name)) {
				KillConnection(name);
			}
			StartedDownloading(name, this);
		}
	}
	
	public class UrlDelegate : NSUrlConnectionDelegate {
		Action<string> strCallback;
		Action<UIImage> imgCallback;
		Action<NSError> _failure;
		NSMutableData data;
		NSUrlCredential _credential;
		string _name;
		
		public UrlDelegate(string name, Action<string> success) {
			_name = name;
			strCallback = success;
			data = new NSMutableData();
		}
		
		public UrlDelegate(string name, Action<UIImage> success) {
			_name = name;
			imgCallback = success;
			data = new NSMutableData();
		}
		
		public UrlDelegate(string name, Action<string> success,  Action<NSError> failure) {
			_name = name;
			strCallback = success;
			_failure = failure;
			data = new NSMutableData();
		}
		
		public UrlDelegate(string name, Action<UIImage> success,  Action<NSError> failure) {
			_name = name;
			imgCallback = success;
			_failure = failure;
			data = new NSMutableData();
		}
		
		public UrlDelegate(string name, Action<UIImage> success, Action<NSError> failure, NSUrlCredential credential) {
			_name = name;
			imgCallback = success;
			_credential = credential;
			_failure = failure;
			data = new NSMutableData();
		}
		
		
		public override void ReceivedData (NSUrlConnection connection, NSData d)
		{
			data.AppendData(d);
		}
		
		public override bool CanAuthenticateAgainstProtectionSpace (NSUrlConnection connection, NSUrlProtectionSpace protectionSpace)
		{
			return true;
		}
		
		bool showError = true;
		
		public override void ReceivedAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge)
		{
			if (challenge.PreviousFailureCount>0){
				showError = false;
				challenge.Sender.CancelAuthenticationChallenge(challenge);
				return;
			}
			
			if (challenge.ProtectionSpace.AuthenticationMethod=="NSURLAuthenticationMethodServerTrust")
				challenge.Sender.UseCredentials(NSUrlCredential.FromTrust(challenge.ProtectionSpace.ServerTrust), challenge);
			
			
			if (challenge.ProtectionSpace.AuthenticationMethod=="NSURLAuthenticationMethodDefault" && _credential!=null) {
				challenge.Sender.UseCredentials(_credential, challenge);
			}
		}
		
		public override void CanceledAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge)
		{
			Console.WriteLine("canceled");
		}
		
		public override void FailedWithError (NSUrlConnection connection, NSError error)
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			if (showError)
				//Application.ShowNetworkError(error.LocalizedDescription);
			
			if (_failure!=null)
				_failure(error);
		}
		
		public override void FinishedLoading (NSUrlConnection connection)
		{
			UrlConnection.ConnectionEnded(_name);
			if (strCallback!=null)
				strCallback(data.ToString());
			
			if (imgCallback!=null)
				imgCallback(UIImage.LoadFromData(data));
		}
	}
}

