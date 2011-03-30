//
// SampleFormController.cs
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
using MonoTouch.Forms;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Collections.Generic;

namespace Sample
{
	public class SampleFormController : FormDialogViewController
	{
		public SampleFormController () : base("js/sample.js", true){}
		public SampleFormController(RootElement root):base(root){}
		public SampleFormController(string url, bool pushing):base(url, pushing){}
		
		public void ShowPopup(Element element){
			InvokeOnMainThread(()=>{
				using (var popup = new UIAlertView("Hello", "This is a popup created from the action.", null, "OK")){
					popup.Show();
				}
			});
		}
		
		public void LoginFormPopulated(Element element){
			var values = new Dictionary<string, string>(){{"login", "escoz"}, {"password","mypass"},{"remember_me", "true"}};
			this.NavigateTo("js/login.js", values);	
		}
		
		public void ShowValues(Element element){
			Console.WriteLine("========>>> Form Values");
			foreach (var v in GetAllValues())
				Console.WriteLine("kv: "+v.Key+" - " +v.Value);
		}
		
	}
}

