//
// RadioElement.cs
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
	public class RadioElement : StringElement {
		internal bool PopOnSelect;
		string Group;
		internal int RadioIdx;
		
		public RadioElement (string caption, string group) : base (caption)
		{
			Group = group;
		}
		
		public RadioElement (string caption, bool popOnSelect) : base (caption)
		{
			PopOnSelect = popOnSelect;
		}
				
		public RadioElement (string caption) : base (caption)
		{
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = base.GetCell (tv);			
			var root = (RootElement) Parent.Parent;
			
			if (!(root.group is RadioGroup))
				throw new Exception ("The RootElement's Group is null or is not a RadioGroup");
			
			bool selected = RadioIdx == ((RadioGroup)(root.group)).Selected;
			cell.Accessory = selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			RootElement root = (RootElement) Parent.Parent;
			UITableViewCell cell = null;
			if (RadioIdx != root.Value){
				var indexForOldItem = root.PathForRadio (root.Value);
				if (indexForOldItem!=null){
					cell = tableView.CellAt (indexForOldItem);
					if (cell != null)
						cell.Accessory = UITableViewCellAccessory.None;
				}
				cell = tableView.CellAt (indexPath);
				if (cell != null)
					cell.Accessory = UITableViewCellAccessory.Checkmark;
				root.Value = RadioIdx;
			}
			
			base.Selected (dvc, tableView, indexPath);
			
			if (PopOnSelect)
				dvc.NavigationController.PopViewControllerAnimated(true);
		}
	}
}
