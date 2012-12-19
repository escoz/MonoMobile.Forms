//
// Element.cs
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
	public abstract class Element : IDisposable {

		static ElementAppearance _classAppearance = new ElementAppearance();

		private ElementAppearance _appearance;
		public static ElementAppearance GlobalAppearance  {get {return _classAppearance; } }

		public ElementAppearance Appearance {
			get {
				if (_appearance!=null)
					return _appearance;
				if (Parent!=null)
					return Parent.Appearance;

				return _classAppearance;
			} 
			set {
				_appearance = value;
			}
		}



		public Element Parent;
		
		public string ID;
		public string Caption;
		public bool ReadOnly;
		public Object Object { get; set; }

		public Element (string caption)
		{
			this.Caption = caption;
		}	
		
		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
		}
		
		public abstract UITableViewCell GetCell (UITableView tv);
		
		public virtual string Summary ()
		{
			return "";
		}
		
		public virtual void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			tableView.CellAt(path).BecomeFirstResponder();
		}
		
		public NSIndexPath GetIndexPath() { 
			var section = Parent as Section;
			if (section == null)
				return null;
			var root = section.Parent as RootElement;
			if (root == null)
				return null;
			
			int row = 0;
			foreach (var element in section.Elements){
				if (element == this){
					int nsect = 0;
					foreach (var sect in root.Sections){
						if (section == sect){
							return NSIndexPath.FromRowSection (row, nsect);
						}
						nsect++;
					}
				}
				row++;
			}
			return null;
		}
		
		public virtual bool Matches (string text)
		{
			if (Caption == null)
				return false;
			return Caption.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1;
		}
	}
}
