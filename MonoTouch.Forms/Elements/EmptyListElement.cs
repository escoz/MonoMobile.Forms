
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
namespace MonoTouch.Dialog
{
	public class EmptyListElement : ActionElement {
		public EmptyListElement(string message):base(message, null){}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell= base.GetCell (tv);
			cell.TextLabel.TextColor = UIColor.LightGray;
			return cell;
		}
	}
}
