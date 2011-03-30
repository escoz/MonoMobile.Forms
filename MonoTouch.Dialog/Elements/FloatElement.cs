//
// FloatElement.cs
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
	public class FloatElement : Element {
		public float Value;
		public float MinValue, MaxValue;
		static NSString skey = new NSString ("FloatElement");
		UISlider slider;
		
		public FloatElement (string caption, float value) : base (null)
		{
			Caption = caption;
			MinValue = 0;
			MaxValue = 1;
			Value = value;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (skey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, skey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else
				RemoveTag (cell, 1);

			SizeF captionSize = new SizeF (0, 0);
			if (Caption != null){
				cell.TextLabel.Text = Caption;
				captionSize = cell.TextLabel.StringSize (Caption, UIFont.FromName (cell.TextLabel.Font.Name, UIFont.LabelFontSize));
				captionSize.Width += 10; // Spacing
			}

			if (slider == null){
				slider = new UISlider (new RectangleF (10f + captionSize.Width, 12f, 280f - captionSize.Width, 7f)){
					BackgroundColor = UIColor.Clear,
					MinValue = this.MinValue,
					MaxValue = this.MaxValue,
					Continuous = true,
					Value = this.Value,
					Tag = 1
				};
				slider.ValueChanged += delegate {
					Value = slider.Value;
				};
			}
			
			cell.ContentView.AddSubview (slider);
			return cell;
		}

		public override string Summary ()
		{
			return Value.ToString ();
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing){
				slider.Dispose ();
				slider = null;
			}
		}		
	}
}
