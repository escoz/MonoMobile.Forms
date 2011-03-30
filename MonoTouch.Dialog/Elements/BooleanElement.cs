//
// FloatElement.cs
//
// Author:
//   Eduardo Scoz (contact@escoz.com)
//
// Copyright 2010, ESCOZ, Inc.
//
// Code licensed under the MIT X11 license
//
// Original code created by Miguel de Icaza for the MonoTouch.Dialog library available at
// https://github.com/migueldeicaza/MonoTouch.Dialog
//

using System;
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
	public class BooleanElement : Element {
		public bool Value;
		
		public BooleanElement (string caption, bool value) : base(caption)
		{
			Value = value;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			BooleanElementCell cell = (BooleanElementCell)tv.DequeueReusableCell(typeof(FloatElementCell).ToString());
			if (cell == null){
				cell = new BooleanElementCell();
			} 
			cell.Update(this);
			
			return cell;
		}

		public override string Summary ()
		{
			return Value.ToString ();
		}	
	}
	
	public class BooleanElementCell : UITableViewCell {
		public static NSString KEY = new NSString (typeof(FloatElementCell).ToString());
		
		private UISwitch _switch;
		private BooleanElement _element;
		
		public BooleanElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}
		
		public void Update(BooleanElement element){
			_element = element;
			if (_switch==null) 
				prepareCell();
			
			TextLabel.BackgroundColor = UIColor.Clear;
			TextLabel.Text = _element.Caption;
			_switch.On = _element.Value;
		}
		
		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			_element = null;
		}
		
		private void prepareCell(){
			_switch = new UISwitch (){
				BackgroundColor = UIColor.Clear
			};
			_switch.AddTarget (delegate {
				_element.Value = _switch.On;
			}, UIControlEvent.ValueChanged);
			
			AccessoryView = _switch;
		}
		
	}
}