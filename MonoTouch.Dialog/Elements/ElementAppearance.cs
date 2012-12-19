using System;
using MonoTouch.UIKit;

namespace MonoTouch.Dialog
{
	public class ElementAppearance
	{
		public ElementAppearance Copy ()
		{
			var el = new ElementAppearance ();
			el.LabelFont = LabelFont;
			el.LabelTextColor = LabelTextColor;
			el.DetailLabelFont = DetailLabelFont;
			el.DetailLabelTextColor = DetailLabelTextColor;
			el.BackgroundColorDisabled= BackgroundColorDisabled;
			el.BackgroundColorEditable = BackgroundColorEditable;
			el.GroupedTableBackgroundColor =  GroupedTableBackgroundColor;
			el.UngroupedBackgroundColor = UngroupedBackgroundColor;
			el.TextFieldFont = TextFieldFont;
			el.TextFieldFontTextColor = TextFieldFontTextColor;
			el.DisabledLabelTextColor = DisabledLabelTextColor;
			el.DetailLabelHighlightedTextColor = DetailLabelHighlightedTextColor;
			el.LabelHighlightedTextColor = LabelHighlightedTextColor;
			el.CellHighlightedColor = CellHighlightedColor;
			return el;
		}

		public ElementAppearance ()
		{
			LabelFont = UIFont.BoldSystemFontOfSize(18);
			LabelTextColor = UIColor.Black;
			DetailLabelFont = UIFont.BoldSystemFontOfSize(18);
			DetailLabelTextColor = UIColor.FromRGB(0.1653f, 0.2532f, 0.4543f);
			DisabledLabelTextColor = UIColor.Gray;
			BackgroundColorDisabled= UIColor.FromRGB(0.9606f, 0.9605f, 0.9605f);
			BackgroundColorEditable = UIColor.White;
			GroupedTableBackgroundColor = UIColor.White;
			UngroupedBackgroundColor = UIColor.GroupTableViewBackgroundColor;
			TextFieldFont = LabelFont;
			TextFieldFontTextColor = UIColor.Black;
			DetailLabelHighlightedTextColor = UIColor.White;
			LabelHighlightedTextColor = UIColor.White;}

		public UIView TableBackgroundView {
			get;
			set;
		}

		public UIColor TextFieldFontTextColor {
			get;
			set;
		}

		public UIFont TextFieldFont {
			get;
			set;
		}

		public UIColor UngroupedBackgroundColor {
			get;
			set;
		}

		public UIColor GroupedTableBackgroundColor {
			get;
			set;
		}

		public UIFont DetailLabelFont {
			get;
			set;
		}

		public UIColor CellHighlightedColor {
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
		public UIColor DisabledLabelTextColor {
			get;
			set;
		}


		public UIColor LabelHighlightedTextColor {
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

		public UIColor DetailLabelHighlightedTextColor {
			get;
			set;
		}
	}
}

