
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
			
		protected UITextField _entry;
		protected EntryElement _element;
		
		public EntryElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
				
		}
		
		public void Update(EntryElement element, UITableView tableView){
			_element = element;
			
			if (_entry==null){
				PrepareEntry(tableView);
			}
			
			_entry.Text = element.Value ?? "";
			_entry.Placeholder = element.Placeholder ?? "";
			_entry.SecureTextEntry = element.IsPassword;
			_entry.AutocapitalizationType = element.AutoCapitalize;
			_entry.KeyboardType = element.KeyboardType;
            _entry.ReturnKeyType = element.ReturnKeyType;
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
			
			_entry = new UITextField (new RectangleF (size.Width+10, (ContentView.Bounds.Height-size.Height)/2-1, 320-size.Width, size.Height));
			
			TextLabel.BackgroundColor = UIColor.Clear;
			_entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
				UIViewAutoresizing.FlexibleLeftMargin;
			
			_entry.ValueChanged += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
			};
			_entry.Ended += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
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
				
				var size = string.IsNullOrEmpty(ee.Caption) ? new SizeF(0,20) : tv.StringSize (ee.Caption, Fonts.EntryFont);
				if (size.Width > max.Width)
					max = size;				
			}
			section.EntryAlignment = new SizeF (16 + Math.Min (max.Width, 160), max.Height);
			return section.EntryAlignment;
		}
		
		
	}
	
	
	

}
