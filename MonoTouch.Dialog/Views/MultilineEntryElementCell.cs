
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
namespace MonoTouch.Dialog
{
	public class MultilineEntryElementCell: UITableViewCell {
		
		public static NSString KEY = new NSString ("EntryElement");
			
		protected UITextView _entry;
		protected MultilineEntryElement _element;
		
		public MultilineEntryElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
				
		}
		
		public void Update(MultilineEntryElement element, UITableView tableView){
			_element = element;
			
			if (_entry==null){
				PrepareEntry(tableView);
			}
			
			_entry.Text = element.Value ?? "";
			_entry.SecureTextEntry = element.IsPassword;
			_entry.AutocapitalizationType = element.AutoCapitalize;
			_entry.KeyboardType = element.KeyboardType;
			TextLabel.Text = element.Caption;
			
			tableView.BeginUpdates();
			tableView.EndUpdates();
			
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
			
			var size = _element.ComputeEntrySize(tableview);
			_entry = new UITextView(new RectangleF(0,0,size.Width, size.Height));
			
			TextLabel.BackgroundColor = UIColor.Clear;
			_entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
				UIViewAutoresizing.FlexibleLeftMargin;
			_entry.Editable = true;
			_entry.Font = Font;
			_entry.ScrollEnabled = false;
			_entry.Bounces = false;
			_entry.ContentMode = UIViewContentMode.TopLeft;
			_entry.BackgroundColor = UIColor.Yellow;
			_entry.EnablesReturnKeyAutomatically = true;
			_entry.Changed += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
				
				tableview.BeginUpdates();
				tableview.EndUpdates();
				_entry.Frame = new RectangleF(0,0,Frame.Width, Frame.Height);
			};
			_entry.Ended += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
				
				tableview.BeginUpdates();
				tableview.EndUpdates();
				_entry.Frame = new RectangleF(0,0,Frame.Width, Frame.Height);
			};

			_entry.Started += delegate {
				EntryElement self = null;
				var returnType = UIReturnKeyType.Default;
				
				foreach (var e in (_element.Parent as Section).Elements){
					if (e == _element)
						self = _element;
					else if (self != null && e is EntryElement)
						returnType = UIReturnKeyType.Next;
				}
				_entry.ReturnKeyType = returnType;
			};
				
			ContentView.AddSubview (_entry);
		}
		
		public UIFont Font = UIFont.SystemFontOfSize(UIFont.LabelFontSize);
	}
}
