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
	public class StringElement : RootElement, IElementSizing {

		public UITextAlignment Alignment = UITextAlignment.Left;
		public string Value;
		public bool ShowValueAsBadge = false;
		public UITableViewCellAccessory CellAccessory = UITableViewCellAccessory.DisclosureIndicator;

		public float RowHeight = 44.0f;

		public UITableViewCellStyle CellStyle = UITableViewCellStyle.Value1;

		public StringElement (string caption) : base(caption) {}
		
		public StringElement (string caption, string value) :  this(caption, value, null){}
		
		public StringElement (string caption,  Action tapped) : this (caption, null, tapped){}
		
		public StringElement (string caption, string value, UIImage image, Action tapped) : base(caption)
		{
			Tapped += tapped;
			this.Value = value;
			this.Image = image;
			this.ReadOnly = true;
		
		}
		
		public StringElement (string caption, string value, Action tapped) : this(caption, value, null, tapped){}
		
		public event Action Tapped;
		public UIImage Image;
		
		protected bool HasTappedEvent(){
			return Tapped != null;
		}

		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return this.RowHeight;
		}

				
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cellid = new NSString(Value==null ? "dialog.StringElement" : "dialog.StringElementValue");
			var cell = (StringElementCell)tv.DequeueReusableCell (cellid);
			if (cell == null){
				if (Value==null) 
					cell = new StringElementCell (UITableViewCellStyle.Default, cellid);
				else
					cell = new StringElementCell (this.CellStyle, cellid);
			}

			cell.SelectionStyle = (Tapped != null || Sections.Count>0) ? UITableViewCellSelectionStyle.Blue : UITableViewCellSelectionStyle.None;
			cell.Accessory = (Tapped != null || Sections.Count>0)? CellAccessory : UITableViewCellAccessory.None;
			cell.ImageView.Image = Image;
			cell.TextLabel.Text = Caption;
			cell.TextLabel.MinimumFontSize = 11;
			cell.TextLabel.AdjustsFontSizeToFitWidth = true;
			cell.TextLabel.TextAlignment = Alignment;
			cell.TextLabel.Font = this.Appearance.LabelFont;
			cell.TextLabel.TextColor = this.Appearance.LabelTextColor; //this.ReadOnly ?  this.Appearance.DisabledLabelTextColor : this.Appearance.LabelTextColor;
			cell.TextLabel.HighlightedTextColor = this.Appearance.LabelHighlightedTextColor;

			cell.BackgroundColor = this.ReadOnly ?  this.Appearance.BackgroundColorDisabled : this.Appearance.BackgroundColorEditable;
			
			if (cell.DetailTextLabel != null && !String.IsNullOrEmpty (Value)) {
				cell.DetailTextLabel.Font = this.Appearance.DetailLabelFont;
				cell.DetailTextLabel.TextColor = this.Appearance.DetailLabelTextColor; //this.ReadOnly ? this.Appearance.DisabledLabelTextColor : 
				cell.DetailTextLabel.MinimumFontSize = 11;
				cell.DetailTextLabel.AdjustsFontSizeToFitWidth = true;
				cell.DetailTextLabel.HighlightedTextColor = this.Appearance.DetailLabelHighlightedTextColor;

				if (!ShowValueAsBadge) {
					cell.DetailTextLabel.Text = Value == null ? "" : Value;
					cell.ShowBadge = false;
				} else {
					var frame = cell.DetailTextLabel.Frame;
					cell.ShowBadge = true;
					cell.BadgeLabel.Text = Value == null ? "" : Value;
				}
			} else {
				if (cell.DetailTextLabel!=null) 
					cell.DetailTextLabel.Text = "";
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

			if (DeselectAutomatically)
				tableView.DeselectRow (indexPath, true);
		}
		
		public override bool Matches (string text)
		{
			return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
		}
	}
}
