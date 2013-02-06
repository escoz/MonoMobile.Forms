using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace MonoTouch.Dialog
{
	public class DialogTextView : UITextView
	{
		string _placeholderText;

		public UILabel PlaceholderLabel {get; private set;}

		public DialogTextView (RectangleF frame) : base(frame)
		{
			this.Changed += HandleChanged;
			PlaceholderLabel = new UILabel() {
				LineBreakMode = UILineBreakMode.WordWrap,
				Lines = 0,
				BackgroundColor = UIColor.Clear,
				TextColor = UIColor.Gray,
				Alpha = 0
			};
			this.BackgroundColor = UIColor.Clear;
			this.AddSubview(PlaceholderLabel);
			this.SendSubviewToBack(PlaceholderLabel);
		}

		
		public string PlaceholderText { 
			get { return _placeholderText;}
			set {
				_placeholderText = value;
				HandleChanged(null, null);
			}
		}

		void HandleChanged (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.PlaceholderText))
				return;
			this.PlaceholderLabel.Text = this.PlaceholderText;
			this.PlaceholderLabel.Font = this.Font;
			this.PlaceholderLabel.Alpha = string.IsNullOrEmpty (this.Text) ? 1 : 0;
			this.SetNeedsLayout();
		}

		public override string Text {
			get {
				return base.Text;
			}
			set {
				this.HandleChanged(null, null);
				base.Text = value;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			PlaceholderLabel.Frame = new RectangleF(8,8,this.Bounds.Size.Width-16, 0);
			PlaceholderLabel.SizeToFit();
		}

	}
}

