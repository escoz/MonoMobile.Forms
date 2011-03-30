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
	public class DateTimeElement : StringElement {
		public DateTime DateValue;
		public UIDatePicker datePicker;
		protected internal NSDateFormatter fmt = new NSDateFormatter () {
			DateStyle = NSDateFormatterStyle.Short
		};
		
		public DateTimeElement (string caption, DateTime date) : base (caption)
		{
			DateValue = date;
			Value = FormatDate (date);
		}	
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			Value = FormatDate (DateValue);
			return base.GetCell (tv);
		}
 
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (disposing){
				fmt.Dispose ();
				fmt = null;
				if (datePicker != null){
					datePicker.Dispose ();
					datePicker = null;
				}
			}
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
		                                                                                                                                
		static RectangleF PickerFrameWithSize (SizeF size)
		{                                                                                                                                    
			var screenRect = UIScreen.MainScreen.ApplicationFrame;
			float fY = 0, fX = 0;
			
			switch (UIApplication.SharedApplication.StatusBarOrientation){
			case UIInterfaceOrientation.LandscapeLeft:
			case UIInterfaceOrientation.LandscapeRight:
				fX = (screenRect.Height - size.Width) /2;
				fY = (screenRect.Width - size.Height) / 2 -17;
				break;
				
			case UIInterfaceOrientation.Portrait:
			case UIInterfaceOrientation.PortraitUpsideDown:
				fX = (screenRect.Width - size.Width) / 2;
				fY = (screenRect.Height - size.Height) / 2 - 25;
				break;
			}
			
			return new RectangleF (fX, fY, size.Width, size.Height);
		}                                                                                                                                    
		
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			var vc = new DateTimeElementController (this) {
				Autorotate = dvc.Autorotate
			};
			datePicker = CreatePicker ();
			datePicker.Frame = PickerFrameWithSize (datePicker.SizeThatFits (SizeF.Empty));
			                            
			vc.View.BackgroundColor = UIColor.Black;
			vc.View.AddSubview (datePicker);
			dvc.ActivateController (vc, dvc);
		}
		
		
		
		class DateTimeElementController : UIViewController {
			DateTimeElement container;
			
			public DateTimeElementController (DateTimeElement container)
			{
				this.container = container;
			}
			
			public override void ViewWillDisappear (bool animated)
			{
				base.ViewWillDisappear (animated);
				container.DateValue = container.datePicker.Date;
			}
			
			public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
			{
				base.DidRotate (fromInterfaceOrientation);
				container.datePicker.Frame = PickerFrameWithSize (container.datePicker.SizeThatFits (SizeF.Empty));
			}
			
			public bool Autorotate { get; set; }
			
			public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
			{
				return Autorotate;
			}
		}
	}
}
