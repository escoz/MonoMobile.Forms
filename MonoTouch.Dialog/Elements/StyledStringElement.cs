//
// StyledStringElement.cs
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
	public class StyledStringElement : StringElement {
		static NSString skey = new NSString ("StyledStringElement");

		public StyledStringElement (string caption) : base (caption) {}
		public StyledStringElement (string caption, string value) : base (caption, value) {}
		public StyledStringElement (string caption, string value, Action tapped) : base (caption, value, tapped) {}
		public StyledStringElement (string caption, Action tapped) : base (caption, tapped) {}
		
		public UIFont Font;
		public UIColor TextColor;
		public UILineBreakMode LineBreakMode = UILineBreakMode.WordWrap;
		public int Lines = 1;
		public UITableViewCellAccessory Accessory = UITableViewCellAccessory.None;
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (skey);
			if (cell == null){
				cell = new UITableViewCell (Value == null ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1, skey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}
			cell.Accessory = Accessory;
			cell.BackgroundColor = RootElement.Appearance.BackgroundColorDisabled;
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			
			var tl = cell.TextLabel;
			tl.Text = Caption;
			tl.TextAlignment = Alignment;
			tl.TextColor = TextColor == null ? UIColor.Black : TextColor;
			tl.BackgroundColor = UIColor.Clear;
			tl.Font = Font == null ? RootElement.Appearance.LabelFont : Font;
			tl.LineBreakMode = LineBreakMode;
			tl.Lines = 0;			
			// The check is needed because the cell might have been recycled.
			if (cell.DetailTextLabel != null)
				cell.DetailTextLabel.Text = Value == null ? "" : Value;
			
			return cell;
		}		
	}
}
