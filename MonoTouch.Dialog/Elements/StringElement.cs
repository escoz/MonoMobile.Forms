//
// StringElement.cs
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
	public class StringElement : RootElement {

		public UITextAlignment Alignment = UITextAlignment.Left;
		public string Value;
		public bool ShowValueAsBadge = false;
		public UITableViewCellAccessory CellAccessory = UITableViewCellAccessory.DisclosureIndicator;
		
		public StringElement (string caption) : base(caption) {}
		
		public StringElement (string caption, string value) :  this(caption, value, null){}
		
		public StringElement (string caption,  Action tapped) : this (caption, null, tapped){}
		
		public StringElement (string caption, string value, UIImage image, Action tapped) : base(caption)
		{
			Tapped += tapped;
			this.Value = value;
			this.Image = image;
		
		}
		
		public StringElement (string caption, string value, Action tapped) : this(caption, value, null, tapped){}
		
		public event Action Tapped;
		public UIImage Image;
		
		protected bool HasTappedEvent(){
			return Tapped != null;
		}
				
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cellid = new NSString(Value==null ? "dialog.StringElement" : "dialog.StringElementValue");
			var cell = (StringElementCell)tv.DequeueReusableCell (cellid);
			if (cell == null){
				if (Value==null) 
					cell = new StringElementCell (UITableViewCellStyle.Default, cellid);
				else
					cell = new StringElementCell (UITableViewCellStyle.Value1, cellid);
			}
			
			cell.SelectionStyle = (Tapped != null || Sections.Count>0) ? UITableViewCellSelectionStyle.Blue : UITableViewCellSelectionStyle.None;
			cell.Accessory = (Tapped != null || Sections.Count>0)? CellAccessory : UITableViewCellAccessory.None;
			cell.ImageView.Image = Image;
			cell.TextLabel.Text = Caption;
			cell.TextLabel.TextAlignment = Alignment;
			cell.TextLabel.Font = StringElement.Appearance.LabelFont;
			cell.TextLabel.TextColor = StringElement.Appearance.LabelTextColor;
			cell.TextLabel.HighlightedTextColor = StringElement.Appearance.LabelHighlightedTextColor;

			cell.BackgroundColor = StringElement.Appearance.BackgroundColorDisabled;
			
			if (cell.DetailTextLabel!=null && !String.IsNullOrEmpty(Value)) {
				cell.DetailTextLabel.Font = StringElement.Appearance.DetailLabelFont;
				cell.DetailTextLabel.TextColor = StringElement.Appearance.DetailLabelTextColor;
				cell.DetailTextLabel.HighlightedTextColor = StringElement.Appearance.DetailLabelHighlightedTextColor;

				if (!ShowValueAsBadge){
					cell.DetailTextLabel.Text = Value == null ? "" : Value;
					cell.ShowBadge = false;
				} else {
					var frame = cell.DetailTextLabel.Frame;
					cell.ShowBadge = true;
					cell.BadgeLabel.Text = Value == null ? "" : Value;
				}
			}
			return cell;
		}

		public override string Summary ()
		{
			return Caption;
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			if (Tapped != null)
				Tapped ();

			base.Selected(dvc, tableView, indexPath);

			tableView.DeselectRow (indexPath, true);
		}
		
		public override bool Matches (string text)
		{
			return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
		}
	}
}
