//
// DateTimeElement.cs
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
	public class DateTimeElement : EntryElement {
		public DateTime? DateValue;
		public UIDatePicker datePicker;
		protected internal NSDateFormatter fmt = new NSDateFormatter () {
			DateStyle = NSDateFormatterStyle.Short
		};
		
		public DateTimeElement (string caption, DateTime? date) : base (caption, "", "")
		{
			DateValue = date;
			Value = FormatDate (date);
		}	
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			Value = FormatDate (DateValue);
			EntryElementCell cell = (EntryElementCell)tv.DequeueReusableCell("DateTimeElement");
			if (cell == null){
				cell = new EntryElementCell("DateTimeElement");
			} 

			cell.Update(this, tv);
			var picker = CreatePicker();
			picker.ValueChanged += (sender, e) => { 
				this.DateValue = picker.Date;
				cell.DetailTextLabel.Text = FormatDate(picker.Date); 
				this.Value = FormatDate(picker.Date);
				cell.SetNeedsLayout();
			};
			cell.TextField.EditingDidBegin += (sender, e) => {
				this.DateValue = picker.Date;
				cell.DetailTextLabel.Text = FormatDate(picker.Date); 
				this.Value = FormatDate(picker.Date);
				cell.SetNeedsLayout();
			};

			cell.TextField.InputView = picker;
			cell.TextField.Alpha = 0;
			cell.TextLabel.TextColor = Appearance.LabelTextColor;
			
			cell.TextLabel.HighlightedTextColor = Appearance.LabelHighlightedTextColor;
			cell.TextLabel.Font = Appearance.LabelFont;
			
			cell.DetailTextLabel.Text = FormatDate(this.DateValue); 
			cell.DetailTextLabel.TextColor = Appearance.DetailLabelTextColor;
			cell.DetailTextLabel.Font = Appearance.DetailLabelFont;
			cell.DetailTextLabel.HighlightedTextColor = Appearance.DetailLabelHighlightedTextColor;


			cell.BackgroundColor = Appearance.BackgroundColorEditable;

			var toolbar =  new UIToolbar();
			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (e, a)=>{
					cell.TextField.ResignFirstResponder();
				})
			};
			toolbar.SizeToFit();
			cell.TextField.InputAccessoryView = toolbar;				
			return cell;
		}
		
		public virtual string FormatDate (DateTime? dt)
		{
			if (dt.HasValue)
				return fmt.ToString (dt) + " " + dt.Value.ToLocalTime ().ToShortTimeString ();
			return "";
		}
		
		public virtual UIDatePicker CreatePicker ()
		{
			var picker = new UIDatePicker (RectangleF.Empty){
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
				Mode = UIDatePickerMode.Date,
				Date = DateValue.HasValue ? DateValue.Value : DateTime.Now
			};
			return picker;
		}
		               
	}
}
