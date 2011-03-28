
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
	public class StringElement : Element {

		public UITextAlignment Alignment = UITextAlignment.Left;
		public string Value;
		
		public StringElement (string caption) : base(caption) {}
		
		public StringElement (string caption, string value) :  this(caption, value, null){}
		
		public StringElement (string caption,  Action tapped) : this (caption, null, tapped){}
		
		public StringElement (string caption, string value, UIImage image, Action tapped) : base(caption)
		{
			Tapped += tapped;
			this.Value = value;
			this.Image = image;
			CellId = new NSString(Value==null ? "dialog.StringElement" : "dialog.StringElementValue");
		
		}
		
		public StringElement (string caption, string value, Action tapped) : this(caption, value, null, tapped){}
		
		public event Action Tapped;
		public UIImage Image;
		
		protected bool HasTappedEvent(){
			return Tapped != null;
		}
				
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (CellId);
			if (cell == null){
				if (Value==null) 
					cell = new UITableViewCell (UITableViewCellStyle.Default, CellId);
				else
					cell = new UITableViewCell (UITableViewCellStyle.Value1, CellId);
			}
			
			cell.SelectionStyle = (Tapped != null) ? UITableViewCellSelectionStyle.Blue : UITableViewCellSelectionStyle.None;
			
			cell.Accessory = Tapped == null? UITableViewCellAccessory.None : UITableViewCellAccessory.DisclosureIndicator;
			
			cell.ImageView.Image = Image;
			
			cell.TextLabel.Text = Caption;
			cell.TextLabel.TextAlignment = Alignment;
			
			if (cell.DetailTextLabel!=null)
				cell.DetailTextLabel.Text = Value == null ? "" : Value;
			
			return cell;
		}

		public override string Summary ()
		{
			return Caption;
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			base.Selected(dvc, tableView, indexPath);
			
			if (Tapped != null)
				Tapped ();
			tableView.DeselectRow (indexPath, true);
		}
		
		public override bool Matches (string text)
		{
			return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
		}
	}
}
