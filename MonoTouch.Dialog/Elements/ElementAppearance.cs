using System;
using MonoTouch.UIKit;

namespace MonoTouch.Dialog
{
	public class ElementAppearance
	{
		public ElementAppearance ()
		{
			LabelFont = UIFont.BoldSystemFontOfSize(18);
			LabelTextColor = UIColor.Black;
			DetailLabelFont = UIFont.BoldSystemFontOfSize(18);
			DetailLabelTextColor = UIColor.FromRGB(0.1653f, 0.2532f, 0.4543f);
			BackgroundColorDisabled= UIColor.FromRGB(0.9606f, 0.9605f, 0.9605f);
			BackgroundColorEditable = UIColor.White;
		}

		public UIFont DetailLabelFont {
			get;
			set;
		}
		
		public UIColor DetailLabelTextColor {
			get;
			set;
		}
		public UIFont LabelFont {
			get;
			set;
		}

		public UIColor LabelTextColor {
			get;
			set;
		}

		public UIColor BackgroundColorDisabled {
			get;
			set;
		}

		public UIColor BackgroundColorEditable {
			get;
			set;
		}
	}
}

