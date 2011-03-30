//
// UIViewElement.cs
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
	public class UIViewElement : Element, IElementSizing {
		static int count;
		NSString key;
		protected UIView View;
		public CellFlags Flags;
		
		public enum CellFlags {
			Transparent = 1,
			DisableSelection = 2
		}
		
		public UIViewElement (string caption, UIView view, bool transparent) : base (caption) 
		{
			this.View = view;
			this.Flags = transparent ? CellFlags.Transparent : 0;
			key = new NSString ("UIViewElement" + count++);
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, key);
				if ((Flags & CellFlags.Transparent) != 0){
					cell.BackgroundColor = UIColor.Clear;
					
					// 
					// This trick is necessary to keep the background clear, otherwise
					// it gets painted as black
					//
					cell.BackgroundView = new UIView (RectangleF.Empty) { 
						BackgroundColor = UIColor.Clear 
					};
				}
				if ((Flags & CellFlags.DisableSelection) != 0)
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				
				cell.ContentView.AddSubview (View);
			} 
			return cell;
		}
		
		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return View.Bounds.Height;
		}
		
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (disposing){
				View.Dispose ();
				View = null;
			}
		}
	}
}
