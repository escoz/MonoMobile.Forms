//
// MultilineElement.cs
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
	public class MultilineElement : StringElement, IElementSizing {
		
		public MultilineElement (string caption) : base (caption){}
		public MultilineElement (string caption, string value) : base (caption, value){}
		public MultilineElement (string caption, Action tapped) : base (caption, tapped){}
		
		public MultilineElement (string caption, string value, Action tapped) : base (caption, value, tapped)
		{
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = base.GetCell (tv);				
			var tl = cell.TextLabel;
			tl.LineBreakMode = UILineBreakMode.WordWrap;
			tl.Font = this.Font;
			tl.Lines = 0;

			return cell;
		}
		
		public virtual float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			SizeF size = new SizeF (280, float.MaxValue);
			if (string.IsNullOrEmpty(Caption)) return Font.LineHeight+10;
			return tableView.StringSize (Caption, Font, size, UILineBreakMode.WordWrap).Height + 10;
		}
		
		public UIFont Font = UIFont.SystemFontOfSize(15f);
	}
}
