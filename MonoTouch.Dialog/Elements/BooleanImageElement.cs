//
// BoolanImageElement.cs
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
	public class BooleanImageElement : BaseBooleanImageElement {
		UIImage onImage, offImage;
		
		public BooleanImageElement (string caption, bool value, UIImage onImage, UIImage offImage) : base (caption, value)
		{
			this.onImage = onImage;
			this.offImage = offImage;
		}
		
		public BooleanImageElement(string caption, bool value, UIImage onImage, UIImage offImage, NSAction tapped) : this(caption, value, onImage, offImage){
			Tapped += tapped;
		}
		
		protected override UIImage GetImage ()
		{
			if (Value)
				return onImage;
			else
				return offImage;
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			onImage = offImage = null;
		}
	}
}
