
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using MonoTouch.Forms.Activities;
using System.Threading;
namespace MonoTouch.Forms
{
	public class ActivityElement : UIViewElement, IElementSizing {
		
		static UIColor actionTextColor = UIColor.FromRGB(50.0f/255.0f, 79.0f/255.0f, 133.0f/255.0f);
		
		private string _commandName;
		public ActivityElement(string caption, string commandName, string value) :base(caption, new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray), false)
		{
			_commandName = commandName;
			Initialize();
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			tableView.DeselectRow(path, true);
			var cell = tableView.CellAt(path);
			cell.TextLabel.Hidden = true;
			
			Animating = true;
			
			var activity = ActivityFactory.Create(_commandName);
			dvc.View.UserInteractionEnabled = false;
			activity.Execute(this, (JsonDialogViewController)dvc, ()=>{ 
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
