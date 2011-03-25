
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
namespace MonoTouch.Forms
{
	public class ActionElement : StringElement {
		static NSString skey = new NSString("ActionElement");
		public string Action;
		
		static UIColor actionTextColor = UIColor.FromRGB(50.0f/255.0f, 79.0f/255.0f, 133.0f/255.0f);
		
		//public ActionElement(string caption, NSAction tapped) : base (caption, tapped) {}
		
		public ActionElement(string caption, string action, NSAction tapped) : base(caption) { // TODO Handle action tapped
			Action = action;
		}
		
		public override UITableViewElementCell GetCell (UITableView tv)
		{
			var cell = (UITableViewElementCell) tv.DequeueReusableCell (skey);
			if (cell == null){
				cell = new UITableViewElementCell (UITableViewCellStyle.Default, skey);
			}
			
			cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			cell.TextLabel.Text = Caption;
			cell.Accessory = UITableViewCellAccessory.None;
			cell.TextLabel.TextAlignment = UITextAlignment.Center;
			cell.TextLabel.TextColor = actionTextColor;
			return cell;
		}
	}
	
	public class SubmitElement : ActionElement {
		public string Url;
		
		public SubmitElement(string caption, string url, string action, NSAction tapped) : base(caption, action, tapped){
			Url = url;
		}
	}
	
	public class EmptyListElement : StringElement {
		public EmptyListElement(string message):base(message){}
		
		public override UITableViewElementCell GetCell (UITableView tv)
		{
			var cell= base.GetCell (tv);
			cell.TextLabel.TextColor = UIColor.LightGray;
			return cell;
		}
	}
}
