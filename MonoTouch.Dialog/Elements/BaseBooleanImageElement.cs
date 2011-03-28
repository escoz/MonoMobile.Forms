
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
	/// <summary>
	///  This class is used to render a string + a state in the form
	/// of an image.  
	/// </summary>
	/// <remarks>
	/// It is abstract to avoid making this element
	/// keep two pointers for the state images, saving 8 bytes per
	/// slot.   The more derived class "BooleanImageElement" shows
	/// one way to implement this by keeping two pointers, a better
	/// implementation would return pointers to images that were 
	/// preloaded and are static.
	/// 
	/// A subclass only needs to implement the GetImage method.
	/// </remarks>
	public abstract class BaseBooleanImageElement : BoolElement {
		static NSString key = new NSString ("BooleanImageElement");
		
		public class TextWithImageCellView : UITableViewCell {
			const int fontSize = 17;
			static UIFont font = UIFont.BoldSystemFontOfSize (fontSize);
			
			BaseBooleanImageElement _element;
			public BaseBooleanImageElement Element {
				get {return _element;}
				set {
					_element = value;
					UpdateElement();
				}
			}
			
			UILabel label;
			UIButton button;
			const int ImageSpace = 32;
			const int Padding = 8;
	
			public TextWithImageCellView () : base (UITableViewCellStyle.Value1, key)
			{
				label = new UILabel () {
					TextAlignment = UITextAlignment.Left,
					Font = font,
				};
				button = UIButton.FromType (UIButtonType.Custom);
				button.TouchDown += delegate {
					Element.Value = !Element.Value;
					UpdateElement();
					if (Element.ImageTapped != null)
						Element.ImageTapped();
				};
				
				ContentView.Add (label);
				ContentView.Add (button);
			}

			void UpdateElement()
			{
				label.Text = _element.Caption;
				button.SetImage (Element.GetImage (), UIControlState.Normal);
				this.SetNeedsDisplay();
			}
			
			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				var full = ContentView.Bounds;
				var frame = full;
				frame.Height = 22;
				frame.X = Padding;
				frame.Y = (full.Height-frame.Height)/2;
				frame.Width -= ImageSpace+Padding;
				label.Frame = frame;
				
				button.Frame = new RectangleF (full.Width-ImageSpace, -3, ImageSpace, 48);
			}
			
			public override void PrepareForReuse ()
			{
				this._element = null;
			}
		}
	
		public BaseBooleanImageElement (string caption, bool value)
			: base (caption, value)
		{
		}
		
		public event NSAction ImageTapped;
		public event NSAction Tapped;
		
		protected abstract UIImage GetImage ();
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key) as TextWithImageCellView;
			if (cell == null)
				cell = new TextWithImageCellView ();
			
			cell.Element = this;
			cell.Accessory = Tapped == null ? UITableViewCellAccessory.None : UITableViewCellAccessory.DisclosureIndicator;
			cell.SelectionStyle = Tapped == null ? UITableViewCellSelectionStyle.None : UITableViewCellSelectionStyle.Blue;
			return cell;
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected (dvc, tableView, path);
			if (Tapped!=null)
				Tapped();
		}
		
	}
}
