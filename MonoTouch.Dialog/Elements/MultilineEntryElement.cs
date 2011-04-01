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
	public class MultilineEntryElement : EntryElement, IElementSizing {
		
		
		public MultilineEntryElement (string placeholder, string value) : base (null, placeholder, value, false)
		{
			Value = value;
			Placeholder = placeholder;
		}
		
		public override string Summary ()
		{
			return Value;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			MultilineEntryElementCell cell = (MultilineEntryElementCell)tv.DequeueReusableCell(MultilineEntryElementCell.KEY);
			if (cell == null){
				cell = new MultilineEntryElementCell();
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
			((MultilineEntryElementCell)tableView.CellAt(path)).BecomeFirstResponder();
		}
		
		
		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return ComputeEntrySize(tableView).Height;
		}
		
		
		public SizeF ComputeEntrySize(UITableView tableView)
		{
			SizeF size = new SizeF (320, float.MaxValue);
			
			if (string.IsNullOrEmpty(Value)) 
				return new SizeF(320, Font.LineHeight+25);
			return new SizeF(320, tableView.StringSize (Value, Font, size, UILineBreakMode.WordWrap).Height + 35);
		}
		
		
		public UIFont Font = UIFont.SystemFontOfSize(UIFont.LabelFontSize);
		
	}
	
}
