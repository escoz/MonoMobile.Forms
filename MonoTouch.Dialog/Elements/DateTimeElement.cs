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
		public DateTime DateValue;
		public UIDatePicker datePicker;
		protected internal NSDateFormatter fmt = new NSDateFormatter () {
			DateStyle = NSDateFormatterStyle.Short
		};
		
		public DateTimeElement (string caption, DateTime date) : base (caption, "", "")
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
				cell.DetailTextLabel.Text = FormatDate(picker.Date); 
				this.Value = FormatDate(picker.Date);
				this.DateValue = picker.Date;
			};

			cell.TextField.InputView = picker;
			cell.TextField.Alpha = 0;
			cell.TextLabel.TextColor = RootElement.Appearance.LabelTextColor;
			cell.TextLabel.Font = RootElement.Appearance.LabelFont;
			
			cell.DetailTextLabel.Text = FormatDate(picker.Date); 
			cell.DetailTextLabel.TextColor = RootElement.Appearance.DetailLabelTextColor;
			cell.DetailTextLabel.Font = RootElement.Appearance.DetailLabelFont;

			cell.BackgroundColor = RadioElement.Appearance.BackgroundColorEditable;

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
		
		public virtual string FormatDate (DateTime dt)
		{
			return fmt.ToString (dt) + " " + dt.ToLocalTime ().ToShortTimeString ();
		}
		
		public virtual UIDatePicker CreatePicker ()
		{
			var picker = new UIDatePicker (RectangleF.Empty){
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
				Mode = UIDatePickerMode.Date,
				Date = DateValue
			};
			return picker;
		}
		               
	}
}
