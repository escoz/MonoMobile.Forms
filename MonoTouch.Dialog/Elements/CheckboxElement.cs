//
// CheckboxElement.cs
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
	public class CheckboxElement : StringElement {
		public new bool Value;
		public string Group;
		
		public CheckboxElement (string caption) : base (caption) {}
		public CheckboxElement (string caption, bool value) : base (caption)
		{
			Value = value;
		}
		
		public CheckboxElement (string caption, bool value, string group) : this (caption, value)
		{
			Group = group;
		}
		
		UITableViewCell ConfigCell (UITableViewCell cell)
		{
			cell.Accessory = Value ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
			return cell;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			return  ConfigCell (base.GetCell (tv));
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			Value = !Value;
			var cell = tableView.CellAt (path);
			ConfigCell (cell);
			base.Selected (dvc, tableView, path);
		}

	}
}
