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
		public float MinValue = 0, MaxValue = 1;
		
		public FloatElement (string caption, float value) : base(caption)
		{
			Value = value;
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			FloatElementCell cell = (FloatElementCell)tv.DequeueReusableCell (FloatElementCell.KEY);
			if (cell == null){
				cell = new FloatElementCell();
			} 
			cell.Update(this);
			
			return cell;
		}

		public override string Summary ()
		{
			return Value.ToString ();
		}	
	}
	
	public class FloatElementCell : UITableViewCell {
		public static NSString KEY = new NSString ("FloatElement");
		
		private UISlider _slider;
		private FloatElement _element;
		
		public FloatElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}
		
		public void Update(FloatElement element){
			_element = element;
			if (_slider==null) 
				prepareCell();
			
			TextLabel.BackgroundColor = UIColor.Clear;
			SizeF captionSize = new SizeF (0, 0);
			if (element.Caption != null){
				TextLabel.Text = element.Caption;
				captionSize = TextLabel.StringSize (element.Caption, RootElement.Appearance.TextFieldFont);
				captionSize.Width += 8; // Spacing
			}
			
			_slider.Frame = new RectangleF (10f + captionSize.Width, 20f, 280f - captionSize.Width, 7f);
			_slider.MinValue = element.MinValue;
			_slider.MaxValue = element.MaxValue;
			_slider.Value = element.Value;
		}
		
		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			_element = null;
		}
		
		private void prepareCell(){
			_slider = new UISlider (){
				BackgroundColor = UIColor.Clear,
				Continuous = true,
				Tag = 1
			};
			_slider.ValueChanged += delegate {
				_element.Value = _slider.Value;
			};
			
			ContentView.AddSubview (_slider);
		}
		
	}
}
