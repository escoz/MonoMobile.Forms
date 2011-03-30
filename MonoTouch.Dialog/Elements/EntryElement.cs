//
// EntryElement.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// Original code created by Miguel de Icaza for the MonoTouch.Dialog library available at
// https://github.com/migueldeicaza/MonoTouch.Dialog
//

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

	public class EntryElement : Element {
		
		public string Value, Placeholder;
		public bool IsPassword = false;
		
		public UIKeyboardType KeyboardType = UIKeyboardType.Default;
		public UITextAutocapitalizationType AutoCapitalize = UITextAutocapitalizationType.Sentences;
		
		public EntryElement (string caption, string placeholder, string value) : this (caption, placeholder, value, false){}
		
		public EntryElement (string caption, string placeholder, string value, bool isPassword) : base (caption)
		{
			Value = value;
			IsPassword = isPassword;
			Placeholder = placeholder;
		}
		
		public override string Summary ()
		{
			return Value;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			EntryElementCell cell = (EntryElementCell)tv.DequeueReusableCell(EntryElementCell.KEY);
			if (cell == null){
				cell = new EntryElementCell();
			} 
			
			cell.Update(this, tv);
				
			return cell;
		}
		
		public override bool Matches (string text)
		{
			return (Value != null ? Value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1: false) || base.Matches (text);
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			((EntryElementCell)tableView.CellAt(path)).BecomeFirstResponder();
		}
	}
	
	
	
	public class EntryElementCell : UITableViewCell {
		
		public static NSString KEY = new NSString ("EntryElement");
			
		private UITextField _entry;
		private EntryElement _element;
		
		public EntryElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
				
		}
		
		public void Update(EntryElement element, UITableView tableView){
			_element = element;
			
			if (_entry==null){
				_prepareEntry(tableView);
			}
			
			_entry.Text = element.Value ?? "";
			_entry.Placeholder = element.Placeholder ?? "";
			_entry.SecureTextEntry = element.IsPassword;
			_entry.AutocapitalizationType = element.AutoCapitalize;
			_entry.KeyboardType = element.KeyboardType;
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
			
		private void _prepareEntry(UITableView tableview){
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
				UITableViewCell cellToFocusOn = null;
				
				foreach (var c in tableview.VisibleCells){
					if (c == this)
						cellToFocusOn = c;
					else if (cellToFocusOn != null && c is EntryElementCell)
						cellToFocusOn = c as EntryElementCell;
				}
				if (cellToFocusOn != this && cellToFocusOn!=null)
					cellToFocusOn.BecomeFirstResponder();
				else 
					_entry.ResignFirstResponder();
				
				return true;
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
