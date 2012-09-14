//
// SimpleImageElement.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// Original code created by Miguel de Icaza for the MonoTouch.Dialog library available at
// https://github.com/migueldeicaza/MonoTouch.Dialog
//
using System;
using MonoTouch.UIKit;
using System.Drawing;
namespace MonoTouch.Dialog
{
	public class SimpleImageElement : Element, IElementSizing {
		public UIImage Value;
		private string ikey = "simpleimagelement";
		protected UIImageView Image;
		
		public SimpleImageElement (UIImage image) : base ("")
		{
			Value = image;			
			Image = new UIImageView(image);
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (ikey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, ikey);
				cell.Add(Image);
			}
			
			if (tv.Style == UITableViewStyle.Grouped){
				cell.BackgroundColor = UIColor.Clear;
				cell.BackgroundView = new UIView{BackgroundColor = UIColor.Clear, Frame = new RectangleF(0,0,0,0)};
			}
			Image.Image = Value;
			cell.UserInteractionEnabled =false;
			return cell;
		}
		
		#region IElementSizing implementation
		float IElementSizing.GetHeight (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return Value.Size.Height;
		}
		#endregion
	}
}

