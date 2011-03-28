
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
namespace MonoTouch.Dialog
{
	public class RangeAttribute : Attribute {
		public RangeAttribute (float low, float high)
		{
			Low = low;
			High = high;
		}
		public float Low, High;
		public bool ShowCaption;
	}
}
