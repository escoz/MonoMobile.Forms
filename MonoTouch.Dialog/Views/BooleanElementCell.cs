
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
	public class BooleanElementCell : UITableViewCell {
		public static NSString KEY = new NSString (typeof(FloatElementCell).ToString());
		
		private UISwitch _switch;
		private BooleanElement _element;
		
		public BooleanElementCell():base(UITableViewCellStyle.Default, KEY){
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}
		
		public void Update(BooleanElement element){
			_element = element;
			if (_switch==null) 
				prepareCell();
			
			TextLabel.BackgroundColor = UIColor.Clear;
			TextLabel.Text = _element.Caption;
			_switch.On = _element.Value;
		}
		
		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			_element = null;
		}
		
		private void prepareCell(){
			_switch = new UISwitch (){
				BackgroundColor = UIColor.Clear
			};
			_switch.AddTarget (delegate {
				_element.Value = _switch.On;
			}, UIControlEvent.ValueChanged);
			
			AccessoryView = _switch;
		}
		
	}
}
