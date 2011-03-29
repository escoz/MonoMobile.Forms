using System;
using System.Collections.Generic;
using System.Reflection;
namespace MonoTouch.Forms.Activities
{
	public abstract class Activity
	{
		public Activity () {}
		
		public abstract void Execute(ActivityElement element, JsonDialogViewController controller, Action completed);
	}
}

