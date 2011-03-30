//
// BoolElement.cs
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
	public abstract class BoolElement : Element {
		bool val;
		public bool Value {
			get {
				return val;
			}
			set {
				bool emit = val != value;
				val = value;
				if (emit && ValueChanged != null)
					ValueChanged (this, EventArgs.Empty);
			}
		}
		public event EventHandler ValueChanged;
		
		public BoolElement (string caption, bool value) : base (caption)
		{
			val = value;
		}
		
		public override string Summary ()
		{
			return val ? "On" : "Off";
		}		
	}
}
