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
	public abstract class BooleanElement : Element {
		bool _value;

		public bool Value {
			get { return _value; }
			set { 
				_value = value;
				if (this.OnValueChanged != null)
					OnValueChanged (this);
			}
		}

		public bool ValueWithoutEvent {
			get { return _value; }
			set { 
				_value = value;
			}
		}
		
		public BooleanElement(string caption):base(caption){}

		public override string Summary ()
		{
			return Value.ToString ();
		}	
	}
	
	public class iPhoneBooleanElement : BooleanElement {
		
		
		public iPhoneBooleanElement (string caption, bool value) : base(caption)
		{
			Value = value;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			BooleanElementCell cell = (BooleanElementCell)tv.DequeueReusableCell(typeof(BooleanElementCell).ToString());
			if (cell == null){
				cell = new BooleanElementCell();
			} 
			cell.Update(this);
			
			return cell;
		}	
	}
	

}