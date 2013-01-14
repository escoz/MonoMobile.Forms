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
		public virtual string Value { get; set; }
		public string Placeholder;
		public bool IsPassword = false;
		
		public UIKeyboardType KeyboardType = UIKeyboardType.Default;
		public UITextAutocapitalizationType AutoCapitalize = UITextAutocapitalizationType.Sentences;
        public UIReturnKeyType ReturnKeyType = UIReturnKeyType.Default;
		public UITextAutocorrectionType AutoCorrection = UITextAutocorrectionType.Default;
		public UITextAlignment TextAlignment = UITextAlignment.Left;

		public string AppendedText { get; set;}

        public event EventHandler Go;
		
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

        public void FireGo(object sender, EventArgs e)
        {
            if (Go != null)
            {
                Go(sender, e);
            }
        }
	}
}
