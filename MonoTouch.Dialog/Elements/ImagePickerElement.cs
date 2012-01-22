//
// ImagePickerElement.cs
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
	public class ImagePickerElement : Element {
		public UIImage Value;
		static RectangleF rect = new RectangleF (0, 0, dimx, dimy);
		static NSString ikey = new NSString ("ImageElement");
		UIImage scaled;
		UIPopoverController popover;
		
		static UIImagePickerController picker;
		
		const int dimx = 48;
		const int dimy = 43;
		
		const int rad = 10;
		
		static UIImage MakeEmpty ()
		{
			using (var cs = CGColorSpace.CreateDeviceRGB ()){
				using (var bit = new CGBitmapContext (IntPtr.Zero, dimx, dimy, 8, 0, cs, CGImageAlphaInfo.PremultipliedFirst)){
					bit.SetStrokeColor (1, 0, 0, 0.5f);
					bit.FillRect (new RectangleF (0, 0, dimx, dimy));
					
					return UIImage.FromImage (bit.ToImage ());
				}
			}
		}
		
		UIImage Scale (UIImage source)
		{
			UIGraphics.BeginImageContext (new SizeF (dimx, dimy));
			var ctx = UIGraphics.GetCurrentContext ();
		
			var img = source.CGImage;
			ctx.TranslateCTM (0, dimy);
			if (img.Width > img.Height)
				ctx.ScaleCTM (1, -img.Width/dimy);
			else
				ctx.ScaleCTM (img.Height/dimx, -1);

			ctx.DrawImage (rect, source.CGImage);
			
			var ret = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return ret;
		}
		
		public ImagePickerElement (UIImage image) : base ("")
		{
			if (image == null){
				Value = MakeEmpty ();
				scaled = Value;
			} else {
				Value = image;			
				scaled = Scale (Value);
			}
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (ikey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, ikey);
			}
			
			if (scaled == null)
				return cell;
			
			Section psection = Parent as Section;
			bool roundTop = psection.Elements [0] == this;
			bool roundBottom = psection.Elements [psection.Elements.Count-1] == this;
			
			using (var cs = CGColorSpace.CreateDeviceRGB ()){
				using (var bit = new CGBitmapContext (IntPtr.Zero, dimx, dimy, 8, 0, cs, CGImageAlphaInfo.PremultipliedFirst)){
					if (roundBottom){
						bit.AddArc (rad, rad, rad, (float) Math.PI, (float) (3*Math.PI/2), false);
					} else {
						bit.MoveTo (0, rad);
						bit.AddLineToPoint (0, 0);
					}
					bit.AddLineToPoint (dimx, 0);
					bit.AddLineToPoint (dimx, dimy);
					
					if (roundTop){
						bit.AddArc (rad, dimy-rad, rad, (float) (Math.PI/2), (float) Math.PI, false);
						bit.AddLineToPoint (0, rad);
					} else {
						bit.AddLineToPoint (0, dimy);
					}
					bit.Clip ();
					bit.DrawImage (rect, scaled.CGImage);
					
					cell.ImageView.Image = UIImage.FromImage (bit.ToImage ());
				}
			}			
			return cell;
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing){
				scaled.Dispose ();
				Value.Dispose ();
			}
			base.Dispose (disposing);
		}

		class ImagePickerControllerDelegate : UIImagePickerControllerDelegate {
			ImagePickerElement container;
			UITableView table;
			NSIndexPath path;
			
			public ImagePickerControllerDelegate (ImagePickerElement container, UITableView table, NSIndexPath path)
			{
				this.container = container;
				this.table = table;
				this.path = path;
			}
			
			public override void FinishedPickingImage (UIImagePickerController picker, UIImage image, NSDictionary editingInfo)
			{
				container.Picked (image);
				table.ReloadRows (new NSIndexPath [] { path }, UITableViewRowAnimation.None);
			}
		}
		
		void Picked (UIImage image)
		{
			Value = image;
			scaled = Scale (image);
			currentController.DismissModalViewControllerAnimated (true);
			
		}
		
		UIViewController currentController;
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (picker == null)
				picker = new UIImagePickerController ();
			picker.Delegate = new ImagePickerControllerDelegate (this, tableView, path);
			
			switch (UIDevice.CurrentDevice.UserInterfaceIdiom){
			case UIUserInterfaceIdiom.Pad:
				popover = new UIPopoverController (picker);
				popover.PresentFromRect (rect, dvc.View, UIPopoverArrowDirection.Any, true);
				break;
				
			default:
			case UIUserInterfaceIdiom.Phone:
				dvc.ActivateController (picker, dvc);
				break;
			}
			currentController = dvc;
		}
	}
}
