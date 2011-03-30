
using System;
//
// BooleanElement.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// Original code created by Miguel de Icaza for the MonoTouch.Dialog library available at
// https://github.com/migueldeicaza/MonoTouch.Dialog
//

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
namespace MonoTouch.Dialog
{
	public class BooleanElement : BoolElement {
		static NSString bkey = new NSString ("BooleanElement");
		UISwitch sw;
		
		public BooleanElement (string caption, bool value) : base (caption, value){  }
		
		public BooleanElement (string caption, bool value, string key) : base (caption, value){  }
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			if (sw == null){
				sw = new UISwitch (){
					BackgroundColor = UIColor.Clear,
					Tag = 1,
					On = Value
				};
				sw.AddTarget (delegate {
					Value = sw.On;
				}, UIControlEvent.ValueChanged);
			}
			
			var cell = tv.DequeueReusableCell (bkey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, bkey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else
				RemoveTag (cell, 1);
		
			cell.TextLabel.Text = Caption;
			cell.AccessoryView = sw;

			return cell;
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing){
				sw.Dispose ();
				sw = null;
			}
		}
	}
}
