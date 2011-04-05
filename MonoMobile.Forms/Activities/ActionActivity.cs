
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
namespace MonoMobile.Forms.Activities
{
	public class ActionActivity : BaseActivity {
		
		public string ActionName;
		
		public ActionActivity(string actionName) : base(){
			ActionName = actionName;	
		}
		
		public override void Execute (Element element, Action completed)
		{
			try {
				if (element==null)
					Controller.GetType().InvokeMember(this.ActionName,
					    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
					    null, Controller, null);
				
				else
					Controller.GetType().InvokeMember(this.ActionName,
					    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
					    null, Controller, new object[]{element});
				
			} catch (Exception e){
				Console.WriteLine("Could not invoke action '{0}' on dialog '{1}'. {2}", ActionName, Controller.GetType().Name, e.ToString());
			}
		}
		
	}
}
