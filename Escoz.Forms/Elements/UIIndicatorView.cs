//
// UIIndicatorView.cs
//
// Author:
//   Eduardo Scoz (contact@escoz.com)
//
// Copyright 2010, ESCOZ, Inc.
//
// Code licensed under the MIT X11 license
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
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
namespace Escoz.Forms
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
