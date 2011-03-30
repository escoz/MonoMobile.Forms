using System;
using MonoTouch.Forms.Activities;
using MonoTouch.Dialog;
using MonoTouch.Forms;
using MonoTouch.Foundation;
using System.Threading;

namespace Sample.Activities
{
	
	[Preserve(AllMembers=true)]
	public class ShowValuesInConsole : Activity {
		
		public override void Execute (ActivityElement element, JsonDialogViewController controller, Action completed)
		{
			Console.WriteLine("ShowValuesInConsole");
			ThreadPool.QueueUserWorkItem( delegate { 
				
				System.Threading.Thread.Sleep(2000);
				var values = controller.GetAllValues();
				foreach (var v in values){
					Console.WriteLine("Value => {0} - {1}", v.Key, v.Value);	
				}
				
				element.Caption = "Action completed!";
				completed();
			} );
			
		}	
	}
}
