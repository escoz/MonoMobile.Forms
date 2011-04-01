//
// ActionElement.cs
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
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using System.Reflection;
using MonoMobile.Forms.Activities;

namespace MonoMobile.Forms
{
	public class ControllerAction : Activity {
		
		public string ActionName;
		
		public ControllerAction(string actionName) : base(){
			ActionName = actionName;	
		}
		
		public override void Execute (Element element, FormDialogViewController controller, Action completed)
		{
			try {
				controller.GetType().InvokeMember(this.ActionName,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
				    null, controller, new object[]{element});
				
			} catch (Exception e){
				Console.WriteLine("Could not invoke action '{0}' on dialog '{1}'. {2}", ActionName, controller.GetType().Name, e.ToString());
			}
		}
	}
	
	public class ActionElement : StringElement {
		static NSString skey = new NSString("ActionElement");
		public ControllerAction Action;
		
		static UIColor actionTextColor = UIColor.FromRGB(50.0f/255.0f, 79.0f/255.0f, 133.0f/255.0f);
		
		public ActionElement(string caption, ControllerAction action) : base (caption) {
			Action = action;
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			Action.Execute(this, (FormDialogViewController)dvc, ()=>{}) ;	
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (skey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, skey);
			}
			
			cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			cell.TextLabel.Text = Caption;
			cell.Accessory = UITableViewCellAccessory.None;
			cell.TextLabel.TextAlignment = UITextAlignment.Center;
			cell.TextLabel.TextColor = actionTextColor;
			return cell;
		}
	}

}
