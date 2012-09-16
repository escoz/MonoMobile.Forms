using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace MonoTouch.Dialog
{
	public class StringElementCell : UITableViewCell
	{
		public StringElementCell (UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
		{
			
			BadgeLabel = new UILabel {
				Font = UIFont.BoldSystemFontOfSize(12),
				BackgroundColor = new UIColor(0.530f , 0.600f , 0.738f , 1.000f),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center
			};
			BadgeLabel.Layer.CornerRadius = 6;
			
			this.ContentView.Add(BadgeLabel);
		}

		public UILabel BadgeLabel;

		bool _showBadge;

		public bool ShowBadge {
			set {
				BadgeLabel.Hidden = !value;
				_showBadge = value;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (_showBadge){
				var size = BadgeLabel.StringSize(BadgeLabel.Text, BadgeLabel.Font);
				BadgeLabel.Frame = new RectangleF(this.ContentView.Frame.Width-size.Width-18, this.ContentView.Frame.Height/2-((size.Height+4)/2), size.Width+12, size.Height+4);
			}
		}
	}
}

