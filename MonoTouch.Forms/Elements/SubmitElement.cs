
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
namespace MonoTouch.Forms.Elements
{
	public class SubmitElement : ActionElement {
		public string Url;
		
		public SubmitElement(string caption, string url, string action, Action tapped) : base(caption, action, tapped){
			Url = url;
		}
	}
}
