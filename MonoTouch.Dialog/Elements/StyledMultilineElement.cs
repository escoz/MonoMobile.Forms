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
		public StyledMultilineElement (NSAttributedString attributedString) : base(null) {
			this.AttributedText = attributedString;
		}
		public StyledMultilineElement (string caption) : base (caption) {}
		public StyledMultilineElement (string caption, string value) : base (caption, value) {}
		public StyledMultilineElement (string caption, Action tapped) : base (caption, tapped) {}

		public NSAttributedString AttributedText { get;set;}

		public virtual float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			if (this.AttributedText!=null && UIDevice.CurrentDevice.CheckSystemVersion(6,0)) {
				var sizeAttr = this.AttributedText.GetBoundingRect(new SizeF(270, int.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, new NSStringDrawingContext());
				return sizeAttr.Height + 20;
			}

			SizeF size = new SizeF (270, float.MaxValue);
			var text = Caption;
			var size2 = tableView.StringSize (text, this.Font!=null? this.Font : UIFont.FromName ("Helvetica", 17f), size, LineBreakMode);
			return size2.Height + 20;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = base.GetCell (tv);
			if (this.AttributedText == null) 
				return cell;

			cell.TextLabel.AttributedText = this.AttributedText;
			return cell;
		}
	}
}
