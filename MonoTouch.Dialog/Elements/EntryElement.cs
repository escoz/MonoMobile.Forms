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
		
		static NSString ekey = new NSString ("EntryElement");
		bool isPassword;
		public UITextAutocapitalizationType AutoCapitalize = UITextAutocapitalizationType.Sentences;
		UITextField entry;
		string placeholder;
		static UIFont font = UIFont.BoldSystemFontOfSize (17);

		public event EventHandler Changed;
		
		public EntryElement (string caption, string placeholder, string value) : base (caption)
		{
			Value = value;
			this.placeholder = placeholder;
		}
		
		public EntryElement (string caption, string placeholder, string value, bool isPassword) : base (caption)
		{
			Value = value;
			this.isPassword = isPassword;
			this.placeholder = placeholder;
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
				
				var size = string.IsNullOrEmpty(ee.Caption) ? new SizeF(0,20) : tv.StringSize (ee.Caption, font);
				if (size.Width > max.Width)
					max = size;				
			}
			s.EntryAlignment = new SizeF (16 + Math.Min (max.Width, 160), max.Height);
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
				var _entry = new UITextField (new RectangleF (size.Width+10, (cell.ContentView.Bounds.Height-size.Height)/2-1, 320-size.Width, size.Height)){
					Tag = 1,
					Placeholder = placeholder ?? "",
					SecureTextEntry = isPassword,
					AutocapitalizationType = AutoCapitalize
				};
				_entry.Text = Value ?? "";
				entry = _entry;
				
				entry.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
					UIViewAutoresizing.FlexibleLeftMargin;
				
				entry.ValueChanged += delegate {
					FetchValue ();
				};
				entry.Ended += delegate {
					FetchValue ();
				};
				entry.AddTarget((object o, EventArgs r)=>{
					FetchValue(false);
				}, UIControlEvent.EditingChanged);
				entry.ShouldReturn += delegate {
					EntryElement focus = null;
					foreach (var e in (Parent as Section).Elements){
						if (e == this)
							focus = this;
						else if (focus != null && e is EntryElement)
							focus = e as EntryElement;
					}
					if (focus != this)
						focus.entry.BecomeFirstResponder ();
					else 
						focus.entry.ResignFirstResponder ();
					
					return true;
				};
				entry.Started += delegate {
					EntryElement self = null;
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
			FetchValue(false);
		}
		
		public void FetchValue(bool setEntry)
		{
			if (entry == null)
				return;

			var newValue = entry.Text;
			var diff = newValue != Value;
			if (setEntry)
			Value = newValue;
			else 
				val = newValue;
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
