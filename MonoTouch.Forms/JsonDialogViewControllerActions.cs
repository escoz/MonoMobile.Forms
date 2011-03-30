
//
// JsonDialogViewControllerActions.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Json;
using System.Reflection;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
namespace MonoTouch.Forms
{
	public partial class JsonDialogViewController
	{
		public void InvokeAction(string action , params object[] parameters){
			this.GetType().InvokeMember(action,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, 
				    null, this, parameters);
		}
		
		public void InvokeAction(ActionElement action){
			_invokeAction(action.Action, action);
		}
		
		public void InvokeAction(string action, Element element){
			_invokeAction(action, element);
		}
		
		private void _invokeAction(string action, Element element){
			try {
			this.GetType().InvokeMember(action,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
				    null, this, new object[]{element});
			} catch (Exception e){
				Console.WriteLine("Could not invoke action '{0}' on dialog '{1}'. {2}", action, this.GetType().Name, e.ToString());
			}
		}
		
		public void NavigateTo(string file, IDictionary<string, string> hiddenfields){
			var dialog = this;
			UrlConnection.KillAllConnections();
			InvokeOnMainThread(()=>{
				var newcontroller = (JsonDialogViewController)Activator.CreateInstance(this.GetType(), file, true);
				foreach (var v in hiddenfields)
					newcontroller.SetValue(v.Key, v.Value);
				
				dialog.NavigationController.PushViewController(newcontroller, true);
			});
		}
		
		public void NavigateTo(string file){
			var dialog = this;
			UrlConnection.KillAllConnections();
			InvokeOnMainThread(()=>{
				var controller = (JsonDialogViewController)Activator.CreateInstance(this.GetType(), file, true);
				dialog.NavigationController.PushViewController(controller, true);
			});
		}
		
		public void Reload(Element el){
			Reload();
		}
		public void Reload(){
			PrepareRoot(new RootElement(Title));
			ReloadData();
			_processFile(Url, null);
		}
		
			
//		
//		private void _invokeSubmissionResultAction(string action, JsonObject json){
//			this.GetType().InvokeMember(action,
//				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, 
//				    null, this, new object[]{json});
//		}
//		
//		public void Submit(){
//			_submitForm(null);
//		}
//		
//		
//		private void _submitForm(ActionElement el){
//			
//			var values = Context.Fetch();
//			if (el!=null)
//				values.Add("commit", el.Id);
//			
//			Loading(true);
//			var req = CreateRequestForUrl(el.Url);
//			string parameters = "";
//			foreach (var v in values) {
//				parameters += v.Key+"="+v.Value+"&";	// TODO: ugly hack, do we need to encode chars here?
//			}
//			req.HttpMethod = "POST";
//			
//			req.Body = NSData.FromString(parameters);
//			new UrlConnection("formPost", req, (string value)=>{
//				Loading(false);
//				_processSubmissionResult(value);
//
//			}, (error)=>{
//				Loading(false);	
//				NetworkFailed();
//			});
//		}
//		
//		private void _processSubmissionResult(string value){
//			JsonObject json = ((JsonObject)(_parse(value)["result"]));
//			
//			if (!string.IsNullOrEmpty(json.asString("message"))){
//				using (var v = new UIAlertView(json.asBoolean("success") ? "Success" : "Error", json.asString("message"), null, "OK")) {
//					v.Show();
//				}
//			}
//			var action = json.asString("action");
//			var url = json.asString("navigateto");
//			if (!string.IsNullOrEmpty(action)){
//				_invokeSubmissionResultAction(action, json);
//			} else if (!string.IsNullOrEmpty(url)){
//				NavigateTo(url);
//			} else if (json.asBoolean("success")) {
//				if (NavigationController.ViewControllers[0]==this &&
//				    NavigationController!=null && NavigationController.ParentViewController!=null &&
//				    NavigationController.ParentViewController.ModalViewController==NavigationController){
//					NavigationController.ParentViewController.DismissModalViewControllerAnimated(true);
//				} else {
//					NavigationController.PopViewControllerAnimated(true);
//				}
//			}
//		}
//		
		
		
	}
}

