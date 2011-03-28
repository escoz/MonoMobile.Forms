
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
	[AttributeUsage (AttributeTargets.Field | AttributeTargets.Property, Inherited=false)]
	public class PasswordAttribute : EntryAttribute {
		public PasswordAttribute (string placeholder) : base (placeholder) {}
	}
}
