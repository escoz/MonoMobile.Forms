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
using Escoz.Forms.Activities;
namespace Escoz.Forms
{
	public class ActivityElement : UIViewElement, IElementSizing {
		
		static UIColor actionTextColor = UIColor.FromRGB(50.0f/255.0f, 79.0f/255.0f, 133.0f/255.0f);
		
		private string _activityName;
		public ActivityElement(string caption, string activity, string value) :base(caption, new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray), false)
		{
			_activityName = activity;
			Initialize();
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			var cell = tableView.CellAt(path);
			cell.TextLabel.Hidden = true;
			tableView.DeselectRow(path, true);
			
			Animating = true;
			
			var activity = ActivityFactory.Create(_activityName);
			dvc.View.UserInteractionEnabled = false;
			activity.Execute(this, (FormDialogViewController)dvc, ()=>{ 
				View.InvokeOnMainThread(()=>{
					Animating = false;	
					cell.TextLabel.Hidden = false;
					updateCell(cell);
					dvc.View.UserInteractionEnabled = true;
				});
			});
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = base.GetCell(tv);
			updateCell(cell);
			return cell;
		}
		
		private void updateCell(UITableViewCell cell){
			
			cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			cell.TextLabel.Text = Caption;
			cell.Accessory = UITableViewCellAccessory.None;
			cell.TextLabel.TextAlignment = UITextAlignment.Center;
			cell.TextLabel.TextColor = actionTextColor;
		}
		
		public void Initialize()
		{
			var sbounds = UIScreen.MainScreen.Bounds;			
			var uia = View as UIActivityIndicatorView;
			
			uia.StartAnimating ();
			
			var vbounds = View.Bounds;
			View.Frame = new RectangleF ((sbounds.Width-vbounds.Width)/2, 12, vbounds.Width, vbounds.Height + 0);
			View.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
		}
		
		public bool Animating {
			get {
				return ((UIActivityIndicatorView) View).IsAnimating;
			}
			set {
				var activity = View as UIActivityIndicatorView;
				if (value) {
					activity.StartAnimating ();
					activity.Superview.BringSubviewToFront(activity);
				}
				else
					activity.StopAnimating ();
			}
		}
		
		public new float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			return base.GetHeight (tableView, indexPath)+ 24;
		}
	
	}
}
