using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;

namespace MonoTouch.Dialog
{
	public abstract class OwnerDrawnElement : Element, IElementSizing
	{		
		public string CellReuseIdentifier
		{
			get;set;	
		}
		
		public UITableViewCellStyle Style
		{
			get;set;	
		}
		
		public OwnerDrawnElement (UITableViewCellStyle style, string cellIdentifier) : base(null)
		{
			this.CellReuseIdentifier = cellIdentifier;
			this.Style = style;
		}
		
		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return Height(tableView.Bounds);
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			OwnerDrawnCell cell = tv.DequeueReusableCell(this.CellReuseIdentifier) as OwnerDrawnCell;
			
			if (cell == null)
			{
				cell = new OwnerDrawnCell(this.Style, this.CellReuseIdentifier);
			}
			
			cell.Element = this;
			return cell;
		}	
		
		public abstract void Draw(RectangleF bounds, CGContext context, UIView view);
		
		public abstract float Height(RectangleF bounds);
		
		class OwnerDrawnCell : UITableViewCell
		{
			OwnerDrawnCellView view;
			
			public OwnerDrawnCell(UITableViewCellStyle style, string cellReuseIdentifier) : base(style, cellReuseIdentifier){
				
			}
			
			public OwnerDrawnElement Element
			{
				get {
					return view.Element;
				}
				set {
					if (view == null)
					{
						view = new OwnerDrawnCellView (value);
						ContentView.Add (view);
					}
					else
					{
						view.Element = value;
					}
					
					
				}
			}
	
			public override void LayoutSubviews()
			{
				base.LayoutSubviews();
				
				view.Frame = ContentView.Bounds;
			}
			
			public override void PrepareForReuse ()
			{
				base.PrepareForReuse ();
				view.Element = null;
			}
		}
		
		class OwnerDrawnCellView : UIView
		{				
			OwnerDrawnElement element;
			
			public OwnerDrawnCellView(OwnerDrawnElement element)
			{
				this.element = element;
			}
			
			public OwnerDrawnElement Element
			{
				get { return element; }
				set {
					element = value; 
					if (element != null) 
						SetNeedsDisplay();
				}
			}
			
			public override void Draw (RectangleF rect)
			{
				if (element==null)
					return;
				
				CGContext context = UIGraphics.GetCurrentContext();
				element.Draw(rect, context, this);
			}
		}
	}
}

