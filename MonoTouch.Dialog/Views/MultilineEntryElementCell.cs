
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
			var topspace = string.IsNullOrEmpty(_element.Caption)? 0 : 20;
			_entry = new UITextView(new RectangleF(0,topspace,size.Width, size.Height-topspace));
			
			TextLabel.BackgroundColor = UIColor.Clear;
			TextLabel.TextColor = UIColor.Gray;
			TextLabel.Font = UIFont.SystemFontOfSize(14);
			_entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
				UIViewAutoresizing.FlexibleLeftMargin;
			_entry.Editable = true;
			_entry.Font = Font;
			_entry.ScrollEnabled = false;
			_entry.Bounces = false;
			_entry.ContentMode = UIViewContentMode.TopLeft;
			_entry.EnablesReturnKeyAutomatically = true;
			_entry.Changed += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
				
				tableview.BeginUpdates();
				tableview.EndUpdates();
				_entry.Frame = new RectangleF(0,topspace,Frame.Width, Frame.Height-topspace);
			};
			_entry.Ended += delegate {
				if (_element != null)
					_element.Value = _entry.Text;
				
				tableview.BeginUpdates();
				tableview.EndUpdates();
				_entry.Frame = new RectangleF(0,topspace,Frame.Width, Frame.Height-topspace);
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
			ContentView.BringSubviewToFront(TextLabel);
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			
			TextLabel.Frame = new RectangleF(8,0,300,30);
		}
		
		public UIFont Font = UIFont.SystemFontOfSize(UIFont.LabelFontSize);
	}
}
