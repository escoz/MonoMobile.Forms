
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using MonoTouch.MVVM;
namespace MonoTouch.Forms.Elements
{
	public class ActionElement : StringElement {
		static NSString skey = new NSString("ActionElement");
		public string Action;
		
		static UIColor actionTextColor = UIColor.FromRGB(50.0f/255.0f, 79.0f/255.0f, 133.0f/255.0f);
		
		public ActionElement(string caption, string action, Action tapped) : base(caption) { 
			Action = action;
			this.Command = new ActionCommand(tapped);
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
	

	

}
