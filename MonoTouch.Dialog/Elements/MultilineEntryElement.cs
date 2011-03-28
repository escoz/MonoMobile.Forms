using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
namespace MonoTouch.Dialog
{
	public class MultilineEntryElement : Element
	{
		public string Value { 
			get {
				return val;
			}
			set {
				val = value;
				if (entry != null)
					entry.Text = value;
			}
		}
		string val;
		public UIKeyboardType KeyboardType = UIKeyboardType.Default;
		
		static NSString ekey = new NSString ("MultilineEntryElement");
		UITextView entry;
		static UIFont font = UIFont.BoldSystemFontOfSize (15);

		public event EventHandler Changed;
		
		public MultilineEntryElement (string caption, string value) : base (caption)
		{
			Value = value;
		}

		public override string Summary ()
		{
			return Value;
		}

		SizeF ComputeEntryPosition (UITableView tv, UITableViewCell cell)
		{
			Section s = Parent as Section;
			if (s.EntryAlignment.Width != 0)
				return s.EntryAlignment;
			
			SizeF max = new SizeF (-1, -1);
			foreach (var e in s.Elements){
				var ee = e as EntryElement;
				if (ee == null)
					continue;
				
				var size = tv.StringSize (ee.Caption, font);
				if (size.Width > max.Width)
					max = size;				
			}
			s.EntryAlignment = new SizeF (25 + Math.Min (max.Width, 160), max.Height);
			return s.EntryAlignment;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (ekey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, ekey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else 
				RemoveTag (cell, 1);
			
			if (entry == null){
				SizeF size = ComputeEntryPosition (tv, cell);
				var _entry = new UITextView (new RectangleF (size.Width, (cell.ContentView.Bounds.Height-size.Height)/2-1, 320-size.Width, size.Height)){
					Tag = 1,
					Editable = true
				};
				_entry.Text = Value ?? "";
				entry = _entry;
				
				entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
					UIViewAutoresizing.FlexibleLeftMargin;
				
				//entry.ValueChanged += delegate {
				//	FetchValue ();
				//};
				entry.Ended += delegate {
					FetchValue ();
				};
//				entry.ShouldReturn += delegate {
//					EntryElement focus = null;
//					foreach (var e in (Parent as Section).Elements){
//						if (e == this)
//							focus = this;
//						else if (focus != null && e is EntryElement)
//							focus = e as EntryElement;
//					}
//					if (focus != this)
//						focus.entry.BecomeFirstResponder ();
//					else 
//						focus.entry.ResignFirstResponder ();
//					
//					return true;
//				};
				entry.Started += delegate {
					MultilineEntryElement self = null;
					var returnType = UIReturnKeyType.Default;
					
					foreach (var e in (Parent as Section).Elements){
						if (e == this)
							self = this;
						else if (self != null && e is EntryElement)
							returnType = UIReturnKeyType.Next;
					}
					entry.ReturnKeyType = returnType;
				};
			}
			entry.KeyboardType = KeyboardType;
			
			cell.TextLabel.Text = Caption;
			cell.ContentView.AddSubview (entry);
			return cell;
		}
		
		public void FetchValue ()
		{
			if (entry == null)
				return;

			var newValue = entry.Text;
			var diff = newValue != Value;
			Value = newValue;
			
			if (diff){
				if (Changed != null)
					Changed (this, EventArgs.Empty);
			}
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing){
				entry.Dispose ();
				entry = null;
			}
		}
		
		public override bool Matches (string text)
		{
			return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			entry.BecomeFirstResponder();
		}
	}
}

