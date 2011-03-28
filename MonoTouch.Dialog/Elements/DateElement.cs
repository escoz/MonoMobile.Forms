
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
	public class DateElement : DateTimeElement {
		public DateElement (string caption, DateTime date) : base (caption, date)
		{
			fmt.DateStyle = NSDateFormatterStyle.Medium;
		}
		
		public override string FormatDate (DateTime dt)
		{
			return dt==DateTime.MinValue ? "" : fmt.ToString (dt);
		}
		
		public override UIDatePicker CreatePicker ()
		{
			var picker = base.CreatePicker ();
			picker.Mode = UIDatePickerMode.Date;
			return picker;
		}
	}
}
