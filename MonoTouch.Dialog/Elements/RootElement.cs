//
// RootElement.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
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
	public class RootElement : Element, IEnumerable {


		public string CaptionPrompt { get;set;}

		static NSString rkey = new NSString ("RootElement");
		int summarySection, summaryElement;
		internal Group group;
		public bool UnevenRows;
		public Func<RootElement, UIViewController> createOnSelected;
		protected UITableView TableView;
        public Type RootControllerType = new DialogViewController(null).GetType();
		public bool DeselectAutomatically;

		
		public RootElement (string caption) : base (caption)
		{
			summarySection = -1;
			Sections = new List<Section> ();
		}

		public RootElement (string caption, Func<RootElement, UIViewController> createOnSelected) : base (caption)
		{
			summarySection = -1;
			this.createOnSelected = createOnSelected;
			Sections = new List<Section> ();
		}
		
		public 	RootElement (string caption, int section, int element) : base (caption)
		{
			summarySection = section;
			summaryElement = element;
		}
		
		public RootElement (string caption, Group group) : base (caption)
		{
			this.group = group;
		}
		
		internal List<Section> Sections = new List<Section> ();

		internal NSIndexPath PathForRadio (int idx)
		{
			RadioGroup radio = group as RadioGroup;
			if (radio == null)
				return null;
			
			uint current = 0, section = 0;
			foreach (Section s in Sections){
				uint row = 0;
				
				foreach (Element e in s.Elements){
					if (!(e is RadioElement))
						continue;
					
					if (current == idx){
						return NSIndexPath.Create(section, row); 
					}
					row++;
					current++;
				}
				section++;
			}
			return null;
		}
		
		public int Count { 
			get {
				return Sections.Count;
			}
		}

		public Section this [int idx] {
			get {
				return Sections [idx];
			}
		}
		
		internal int IndexOf (Section target)
		{
			int idx = 0;
			foreach (Section s in Sections){
				if (s == target)
					return idx;
				idx++;
			}
			return -1;
		}
			
		internal void Prepare ()
		{
			int current = 0;
			foreach (Section s in Sections){				
				foreach (Element e in s.Elements){
					var re = e as RadioElement;
					if (re != null)
						re.RadioIdx = current++;
					if (UnevenRows == false && e is IElementSizing)
						UnevenRows = true;
				}
			}
		}
		
		public void Add (Section section)
		{
			if (section == null)
				return;
			
			Sections.Add (section);
			section.Parent = this;

			if (TableView == null)
				return;
			
			TableView.InsertSections (MakeIndexSet (Sections.Count-1, 1), UITableViewRowAnimation.Fade);
		}

		//
		// This makes things LINQ friendly;  You can now create RootElements
		// with an embedded LINQ expression, like this:
		// new RootElement ("Title") {
		//     from x in names
		//         select new Section (x) { new StringElement ("Sample") }
		//
		public void Add (IEnumerable<Section> sections)
		{
			foreach (var s in sections)
				Add (s);
		}
		
		NSIndexSet MakeIndexSet (int start, int count)
		{
			NSRange range;
			range.Location = start;
			range.Length = count;
			return NSIndexSet.FromNSRange (range);
		}
		
		/// <summary>
		/// Inserts a new section into the RootElement
		/// </summary>
		/// <param name="idx">
		/// The index where the section is added <see cref="System.Int32"/>
		/// </param>
		/// <param name="anim">
		/// The <see cref="UITableViewRowAnimation"/> type.
		/// </param>
		/// <param name="newSections">
		/// A <see cref="Section[]"/> list of sections to insert
		/// </param>
		/// <remarks>
		///    This inserts the specified list of sections (a params argument) into the
		///    root using the specified animation.
		/// </remarks>
		public void Insert (int idx, UITableViewRowAnimation anim, params Section [] newSections)
		{
			if (idx < 0 || idx > Sections.Count)
				return;
			if (newSections == null)
				return;
			
			if (TableView != null)
				TableView.BeginUpdates ();
			
			int pos = idx;
			foreach (var s in newSections){
				s.Parent = this;
				Sections.Insert (pos++, s);
			}
			
			if (TableView == null)
				return;
			
			TableView.InsertSections (MakeIndexSet (idx, newSections.Length), anim);
			TableView.EndUpdates ();
		}
		
		/// <summary>
		/// Inserts a new section into the RootElement
		/// </summary>
		/// <param name="idx">
		/// The index where the section is added <see cref="System.Int32"/>
		/// </param>
		/// <param name="newSections">
		/// A <see cref="Section[]"/> list of sections to insert
		/// </param>
		/// <remarks>
		///    This inserts the specified list of sections (a params argument) into the
		///    root using the Fade animation.
		/// </remarks>
		public void Insert (int idx, Section section)
		{
			Insert (idx, UITableViewRowAnimation.None, section);
		}
		
		/// <summary>
		/// Removes a section at a specified location
		/// </summary>
		public void RemoveAt (int idx)
		{
			RemoveAt (idx, UITableViewRowAnimation.Fade);
		}

		/// <summary>
		/// Removes a section at a specified location using the specified animation
		/// </summary>
		/// <param name="idx">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="anim">
		/// A <see cref="UITableViewRowAnimation"/>
		/// </param>
		public void RemoveAt (int idx, UITableViewRowAnimation anim)
		{
			if (idx < 0 || idx >= Sections.Count)
				return;
			
			Sections.RemoveAt (idx);
			
			if (TableView == null)
				return;
			
			TableView.DeleteSections (NSIndexSet.FromIndex (idx), anim);
		}
			
		public void Remove (Section s)
		{
			if (s == null)
				return;
			int idx = Sections.IndexOf (s);
			if (idx == -1)
				return;
			RemoveAt (idx, UITableViewRowAnimation.Fade);
		}
		
		public void Remove (Section s, UITableViewRowAnimation anim)
		{
			if (s == null)
				return;
			int idx = Sections.IndexOf (s);
			if (idx == -1)
				return;
			RemoveAt (idx, anim);
		}

		public void Clear ()
		{
			foreach (var s in Sections)
				s.Dispose ();
			Sections = new List<Section> ();
			if (TableView != null)
				TableView.ReloadData ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing){
				if (Sections == null)
					return;
				
				TableView = null;
				Clear ();
				Sections = null;
			}
		}
		
		/// <summary>
		/// Enumerator that returns all the sections in the RootElement.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator"/>
		/// </returns>
		public IEnumerator GetEnumerator ()
		{
			foreach (var s in Sections)
				yield return s;
		}

		/// <summary>
		/// The currently selected Radio item in the whole Root.
		/// </summary>
		public int RadioSelected {
			get {
				var radio = group as RadioGroup;
				if (radio != null)
					return radio.Selected;
				return -1;
			}
			set {
				var radio = group as RadioGroup;
				if (radio != null)
					radio.Selected = value;
			}
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (rkey);
			if (cell == null){
				var style = summarySection == -1 ? UITableViewCellStyle.Default : UITableViewCellStyle.Value1;
				
				cell = new UITableViewCell (style, rkey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			} 
		
			cell.TextLabel.Text = Caption;
			cell.TextLabel.Font = Appearance.LabelFont;
			cell.TextLabel.TextColor = this.ReadOnly ? Appearance.DisabledLabelTextColor : Appearance.LabelTextColor;
			cell.TextLabel.HighlightedTextColor = this.ReadOnly ? Appearance.DisabledLabelTextColor : Appearance.LabelHighlightedTextColor;

			cell.DetailTextLabel.Text = "";
			cell.DetailTextLabel.Font = Appearance.DetailLabelFont;
			cell.DetailTextLabel.TextColor = this.ReadOnly ? Appearance.DisabledLabelTextColor : Appearance.DetailLabelTextColor;
			cell.DetailTextLabel.HighlightedTextColor = Appearance.DetailLabelHighlightedTextColor;

			cell.UserInteractionEnabled = !this.ReadOnly;
			cell.BackgroundColor = this.ReadOnly ? Appearance.BackgroundColorDisabled : Appearance.BackgroundColorEditable;

			var radio = group as RadioGroup;
			if (radio != null){
				int selected = radio.Selected;
				int current = 0;
				
				foreach (var s in Sections){
					foreach (var e in s.Elements){
						if (!(e is RadioElement))
							continue;
						
						if (current == selected){
							cell.DetailTextLabel.Text = e.Summary ();

							goto le;
						}
						current++;
					}
				}
			} else if (group != null){
				int count = 0;
				
				foreach (var s in Sections){
					foreach (var e in s.Elements){
						var ce = e as CheckboxElement;
						if (ce != null){
							if (ce.Value)
								count++;
							continue;
						}
						var be = e as BooleanElement;
						if (be != null){
							if (be.Value)
								count++;
							continue;
						}
					}
				}
				cell.DetailTextLabel.Text = count.ToString ();
			} else if (summarySection != -1 && summarySection < Sections.Count){
					var s = Sections [summarySection];
					if (summaryElement < s.Elements.Count)
						cell.DetailTextLabel.Text = s.Elements [summaryElement].Summary ();
			} 
		le:
			cell.Accessory = this.ReadOnly ? UITableViewCellAccessory.None: UITableViewCellAccessory.DisclosureIndicator;
			
			return cell;
		}
		
		/// <summary>
		///    This method does nothing by default, but gives a chance to subclasses to
		///    customize the UIViewController before it is presented
		/// </summary>
		protected virtual void PrepareDialogViewController (UIViewController dvc)
		{
		}
		
		/// <summary>
		/// Creates the UIViewController that will be pushed by this RootElement
		/// </summary>
		protected virtual UIViewController MakeViewController ()
		{
			if (createOnSelected != null)
				return createOnSelected (this);
			
			
			return new DialogViewController (this, true) {
				Autorotate = true
			};
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (this.Sections.Count()>0)
				dvc.PushRootElement(this);
		}
		
		public void Reload (Section section, UITableViewRowAnimation animation)
		{
			if (section == null)
				throw new ArgumentNullException ("section");
			if (section.Parent == null || section.Parent != this)
				throw new ArgumentException ("Section is not attached to this root");
			
			int idx = 0;
			foreach (var sect in Sections){
				if (sect == section){
					TableView.ReloadSections (new NSIndexSet ((uint) idx), animation);
					return;
				}
				idx++;
			}
		}
		
		public void Reload (Element element, UITableViewRowAnimation animation)
		{
			if (element == null)
				throw new ArgumentNullException ("element");
			var section = element.Parent as Section;
			if (section == null)
				throw new ArgumentException ("Element is not attached to this root");
			var root = section.Parent as RootElement;
			if (root == null)
				throw new ArgumentException ("Element is not attached to this root");
			var path = element.GetIndexPath();
			if (path == null)
				return;
			if (TableView!=null)
				TableView.ReloadRows (new NSIndexPath [] { path }, animation);
		}
		
	}
}
