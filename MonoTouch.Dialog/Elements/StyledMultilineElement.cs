//
// StyledMultilineElement.cs
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
	public class StyledMultilineElement : StyledStringElement, IElementSizing {
		public StyledMultilineElement (string caption) : base (caption) {}
		public StyledMultilineElement (string caption, string value) : base (caption, value) {}
		public StyledMultilineElement (string caption, Action tapped) : base (caption, tapped) {}

		public virtual float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			SizeF size = new SizeF (280, float.MaxValue);
			using (var font = UIFont.FromName ("Helvetica", 17f))
				return tableView.StringSize (Caption, font, size, LineBreakMode).Height;
		}
	}
}
