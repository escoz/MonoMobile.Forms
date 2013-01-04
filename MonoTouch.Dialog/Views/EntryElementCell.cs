
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
	public class EntryElementCell : UITableViewCell {
		
		public static NSString KEY = new NSString ("EntryElement");
			
		protected CustomTextField _entry;

		public CustomTextField TextField {
			get {
				return _entry;
			}
		}

		protected EntryElement _element;
		
		public EntryElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
				
		}

		public EntryElementCell(string key):base(UITableViewCellStyle.Value1, key){
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}
		
		public void Update(EntryElement element, UITableView tableView){
			_element = element;

			if (_entry==null){
				PrepareEntry(tableView);
			}

			_entry.Font = element.Appearance.TextFieldFont;
			_entry.TextColor = element.Appearance.TextFieldFontTextColor;

			TextLabel.Font = element.Appearance.LabelFont;
			TextLabel.TextColor = element.Appearance.LabelTextColor;
			TextLabel.HighlightedTextColor = element.Appearance.LabelHighlightedTextColor;
			
			_entry.Text = element.Value ?? "";
			_entry.RightText = element.AppendedText;
			_entry.TextAlignment = element.TextAlignment;
			_entry.Placeholder = element.Placeholder ?? "";
			_entry.SecureTextEntry = element.IsPassword;
			if (element.KeyboardType==UIKeyboardType.EmailAddress || element.IsPassword){
				_entry.AutocorrectionType = UITextAutocorrectionType.No;
				_entry.AutocapitalizationType = UITextAutocapitalizationType.None;
			} else {
				_entry.AutocorrectionType = UITextAutocorrectionType.Default;
				_entry.AutocapitalizationType = element.AutoCapitalize;
			}
			
			_entry.KeyboardType = element.KeyboardType;
            _entry.ReturnKeyType = element.ReturnKeyType;
			_entry.AutocorrectionType = element.AutoCorrection;
			TextLabel.Text = element.Caption;

		}
			
		public override bool BecomeFirstResponder ()
		{
			return _entry.BecomeFirstResponder();
		}
		
		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			_element = null;
		}
			
		protected virtual void PrepareEntry(UITableView tableview){
			SizeF size = _computeEntryPosition(tableview);
			
			_entry = new CustomTextField (new RectangleF (size.Width+10, (ContentView.Bounds.Height-size.Height)/2-1, 320-size.Width-20, size.Height));
			TextLabel.BackgroundColor = UIColor.Clear;
			_entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
				UIViewAutoresizing.FlexibleLeftMargin;
			
			_entry.ValueChanged += delegate {
				if (_element != null) {
					_element.Value = _entry.Text;
				}
			};
			_entry.Ended += delegate {
				if (_element != null) {
					_element.Value = _entry.Text;
					
					if (_element.OnValueChanged!=null)
						_element.OnValueChanged(_element);
				}
			};
			
			_entry.AddTarget((object o, EventArgs r)=>{
				if (_element != null)
					_element.Value = _entry.Text;
				}, UIControlEvent.EditingChanged);
				
			_entry.ShouldReturn += delegate {
				Element elementToFocusOn = null;
				
				foreach (var c in ((Section)_element.Parent).Elements){
					if (c == _element)
						elementToFocusOn = c;
					else if (elementToFocusOn != null && c is EntryElement)
						elementToFocusOn = c as EntryElement;
				}
				if (elementToFocusOn != _element && elementToFocusOn!=null) {
					var cell = tableview.CellAt(elementToFocusOn.GetIndexPath());
					cell.BecomeFirstResponder();
				}
				else 
					_entry.ResignFirstResponder();

                if (_entry.ReturnKeyType == UIReturnKeyType.Go) {
                    _element.FireGo(this, EventArgs.Empty);
                }

				return true;
			};
			_entry.Started += delegate {
				EntryElement self = null;
				var returnType = _element.ReturnKeyType;

                if (returnType != UIReturnKeyType.Default) {
                    foreach (var e in (_element.Parent as Section).Elements){
                        if (e == _element)
                            self = _element;
                        else if (self != null && e is EntryElement)
                            returnType = UIReturnKeyType.Next;
                    }
                }
                _entry.ReturnKeyType = returnType;
			};
				
			ContentView.AddSubview (_entry);
		}
		
		private SizeF _computeEntryPosition (UITableView tv)
		{
			var section = _element.Parent as Section;
			if (section.EntryAlignment.Width != 0)
				return section.EntryAlignment;
			
			SizeF max = new SizeF (-1, -1);
			foreach (var e in section.Elements){
				var ee = e as EntryElement;
				if (ee == null)
					continue;
				
				var size = string.IsNullOrEmpty(ee.Caption) ? new SizeF(0,20) : tv.StringSize (ee.Caption, e.Appearance.TextFieldFont);
				if (size.Width > max.Width)
					max = size;				
			}
			section.EntryAlignment = new SizeF (16 + Math.Min (max.Width, 160), max.Height);
			return section.EntryAlignment;
		}
		
		
	}

	public class CustomTextField : UITextField {

		public string LeftText;
		public string RightText;

		public CustomTextField(RectangleF rect) : base(rect) {

		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (!string.IsNullOrEmpty (RightText)) {
				if (this.RightView == null) {
					this.RightView = new UILabel (new RectangleF (0, 0, 50, 50));
				}

				((UILabel)this.RightView).Text = RightText;
				((UILabel)this.RightView).SizeToFit ();
				this.RightViewMode = UITextFieldViewMode.Always;
			} else {
//				this.RightView = null;
			}
		}
	}
	
	
	

}
