
using System;
using System.Net;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Json;
using System.IO;
using MonoTouch.Foundation;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
namespace MonoTouch.Forms
{
	public class UIIndicatorView : UIView {
		private UIActivityIndicatorView _ind = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { Frame = new RectangleF(148,12,24,24) };
		
		public UIIndicatorView(){
			Frame = new RectangleF(0,0,800,800);
			this.UserInteractionEnabled = true;
		}
		
		public override void LayoutSubviews ()
		{
			this.AddSubview(_ind);
		}
		
		public void StartAnimating(){
			_ind.StartAnimating();
		}
		
		public void StopAnimating(){
			_ind.StopAnimating();	
		}
		
	}
}
