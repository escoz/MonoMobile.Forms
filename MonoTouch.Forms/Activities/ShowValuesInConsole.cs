using System;
namespace MonoTouch.Forms.Activities
{
	
	public class ShowValuesInConsole : Activity {
		
		public override void Execute (ActivityElement element, JsonDialogViewController controller, Action completed)
		{
			var values = controller.GetAllValues();
			foreach (var v in values){
				Console.WriteLine("Value => {0} - {1}", v.Key, v.Value);	
			}
			completed();
		}	
	}
}
