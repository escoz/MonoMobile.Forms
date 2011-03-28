
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
	public class RadioGroup : Group {
		public int Selected;
		
		public RadioGroup (string key, int selected) : base (key)
		{
			Selected = selected;
		}
		
		public RadioGroup (int selected) : base (null)
		{
			Selected = selected;
		}
	}
}
