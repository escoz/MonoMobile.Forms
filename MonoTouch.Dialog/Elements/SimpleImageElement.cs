using System;
using MonoTouch.UIKit;
using System.Drawing;
namespace MonoTouch.Dialog
{
	public class SimpleImageElement : Element, IElementSizing {
		public UIImage Value;
		private string ikey = "simpleimagelement";
		private UIImageView Image;
		
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

