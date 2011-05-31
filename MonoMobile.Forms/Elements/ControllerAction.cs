
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using System.Reflection;
using MonoMobile.Forms.Activities;
namespace MonoMobile.Forms
{
	public class ControllerAction : Activity {
		
		public string ActionName;
		public string Parameter;
		
		public ControllerAction(string actionName) : base(){
			ActionName = actionName;
		}
		
		public ControllerAction(string actionName, string param) : base(){
			ActionName = actionName;	
			Parameter = param;
		}
		
		public override void Execute (FormDialogViewController controller, Element element,  Action completed)
		{
			try {
				controller.GetType().InvokeMember(this.ActionName,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
				    null, controller, Parameter == null ? new object[]{element} : new object[]{Parameter});
				
			} catch (Exception e){
				Console.WriteLine("Could not invoke action '{0}' on dialog '{1}'. {2}", ActionName, controller.GetType().Name, e.ToString());
			}
		}
	}
}
