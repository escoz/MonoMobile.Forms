//
// ImageStringElement.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using Escoz.Forms.Util;
namespace Escoz.Forms.Elements
{
	
	public class ImageStringElement : StringElement, IElementSizing {
			
		static NSString skey = new NSString ("ImageStringElement");
		UIImage image;
		public UITableViewCellAccessory Accessory { get; set; }
		string imgUrl;
		public UIFont TitleFont = UIFont.BoldSystemFontOfSize(15);
		
		public ImageStringElement (string caption, UIImage image) : base (caption)
		{
			this.image = image;
		}

		public ImageStringElement (string caption, string value, UIImage image) : base (caption, value)
		{
			this.image = image;
		}
		
		public ImageStringElement (string caption, string value, UIImage image, Action tapped) : base (caption, value, tapped)
		{
			this.image = image;
		}
		
		public ImageStringElement (string caption,  Action tapped, UIImage image) : base (caption, tapped)
		{
			this.image = image;
		}
		
		public ImageStringElement (string caption, string value, Action tapped, UIImage defaultImage, string imageurl) : this(caption, value, defaultImage, tapped) {
			imgUrl = imageurl;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (skey) as WebImageTableViewCell;
			if (cell == null){
				cell = new WebImageTableViewCell (UITableViewCellStyle.Subtitle, skey);
			}
			
			cell.SelectionStyle = HasTappedEvent()? UITableViewCellSelectionStyle.Blue : UITableViewCellSelectionStyle.None;
			cell.Accessory = HasTappedEvent() ? UITableViewCellAccessory.DisclosureIndicator : UITableViewCellAccessory.None;
			cell.TextLabel.Text = Caption;
			cell.TextLabel.Font = TitleFont;
			cell.TextLabel.TextAlignment = Alignment;
			cell.ImageView.Image = image;
			cell.DetailTextLabel.Lines = DetailLines;
			cell.SetImageUrl(imgUrl);
			if (cell.DetailTextLabel!=null)
				cell.DetailTextLabel.Text = Value == null ? "" : Value;
			
			cell.SetNeedsLayout();
			return cell;
		}
		
		#region IElementSizing implementation
		float IElementSizing.GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return Height + (Value==null? 0 : DetailLines*16);
		}
		#endregion
		
		public int Height = 42;
		public int DetailLines = 1;
	}
	
	class WebImageTableViewCell : UITableViewCell, IImageUpdated {
		
		string url;
		
		public void SetImageUrl(string url){
			this.url = url;
			if (!string.IsNullOrEmpty(url)) {
				SimpleImageStore.Current.RequestImage(url, this);
			}
		}
		
		public WebImageTableViewCell(UITableViewCellStyle style, NSString key) : base(style, key){
		}
		
		public void UpdatedImage (string url, UIImage image)
		{
			if (this.url != url) return;
			
			this.ImageView.Image = image;
			this.SetNeedsLayout();
		}
		
		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			url = null;
		}
	}
}
