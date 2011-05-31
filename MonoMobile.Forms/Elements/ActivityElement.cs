//
// ActivityElement.cs
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
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using MonoMobile.Forms.Activities;

namespace MonoMobile.Forms
{
	public class ActivityElement : UIViewElement, IElementSizing {
		
		private string _activityName;
		private bool _animating;
		private Activity _activity;
		
		public ActivityElement(string caption, string activity, string value) :base(caption, new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray), false)
		{
			_activityName = activity;
			_activity= ActivityFactory.Create(_activityName);
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			var cell = (AnimatingCell)tableView.CellAt(path);
			tableView.DeselectRow(path, true);
			_animating = true;
			
			cell.Animating = true;
			
			dvc.View.UserInteractionEnabled = false;
			_activity.Execute((FormDialogViewController)dvc, this, ()=>{ 
				View.InvokeOnMainThread(()=>{
					_animating = false;
					cell.Animating = false; 
					updateCell(cell);
					dvc.View.UserInteractionEnabled = true;
				});
			});
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = (AnimatingCell)tv.DequeueReusableCell (typeof(AnimatingCell).Name);
			if (cell == null){
				cell = new AnimatingCell(UITableViewCellStyle.Default, typeof(AnimatingCell).Name);
			}
			updateCell(cell);
			return cell;
		}
		
		private void updateCell(AnimatingCell cell){
			cell.TextLabel.Text = Caption;
			cell.Animating = _animating;
		}
		
		public new float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			return base.GetHeight (tableView, indexPath)+ 24;
		}
	
	}
	
	public class AnimatingCell : UITableViewCell {
		
		static UIColor actionTextColor = UIColor.FromRGB(50.0f/255.0f, 79.0f/255.0f, 133.0f/255.0f);
		private UIActivityIndicatorView _indicator;
		
		public AnimatingCell(UITableViewCellStyle style, string reuse):base(style, reuse){
			Initialize();	
		}
		
		public void Initialize()
		{
			var sbounds = UIScreen.MainScreen.Bounds;			
			_indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
			
			var vbounds = _indicator.Bounds;
			_indicator.Frame = new RectangleF ((sbounds.Width-vbounds.Width)/2, 12, vbounds.Width, vbounds.Height + 0);
			_indicator.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
			this.AddSubview(_indicator);
			
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			Accessory = UITableViewCellAccessory.None;
			TextLabel.TextAlignment = UITextAlignment.Center;
			TextLabel.TextColor = actionTextColor;
			
			
		}
		
		public virtual bool Animating {
			set {
				
				TextLabel.Hidden = value;
				if (value) {
					_indicator.StartAnimating ();
					BringSubviewToFront(_indicator);
				}
				else
					_indicator.StopAnimating ();
			}
		}
		
	}
}
