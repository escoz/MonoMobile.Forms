
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
	public class SectionAttribute : Attribute {
		public SectionAttribute () {}
		
		public SectionAttribute (string caption)
		{
			Caption = caption;
		}
			
		public SectionAttribute (string caption, string footer)
		{
			Caption = caption;
			Footer = footer;
		}
		public string Caption, Footer;
	}
}
