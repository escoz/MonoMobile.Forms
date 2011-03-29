using System;
using MonoTouch.Forms.Activities;
using MonoTouch.Dialog;
using MonoTouch.Forms;
using MonoTouch.Foundation;

namespace Sample.Activities
{
	
	[Preserve(AllMembers=true)]
	public class ShowValuesInConsole : Activity {
		
		public override void Execute (ActivityElement element, JsonDialogViewController controller, Action completed)
		{
			Console.WriteLine("ShowValuesInConsoleActivity");
			var values = controller.GetAllValues();
			foreach (var v in values){
				Console.WriteLine("Value => {0} - {1}", v.Key, v.Value);	
			}
			System.Threading.Thread.Sleep(2000);
			element.Caption = "Action completed!";
			completed();
		}	
	}
}
