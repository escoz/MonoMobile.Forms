
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
namespace MonoTouch.Forms.Elements
{
	public class EmptyListElement : StringElement {
		public EmptyListElement(string message):base(message){}
		
		public override UITableViewElementCell GetCell (UITableView tv)
		{
			var cell= base.GetCell (tv);
			cell.TextLabel.TextColor = UIColor.LightGray;
			return cell;
		}
	}
}
