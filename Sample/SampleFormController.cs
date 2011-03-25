using System;
using MonoTouch.Forms;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
namespace Sample
{
	public class SampleFormController : JsonDialogViewController
	{
		public SampleFormController () : base("js/sample.js", false)
		{
		}
		
		public SampleFormController(string url, bool pushing):base(url, pushing){}
		
		public void ShowPopup(Element element){
			InvokeOnMainThread(()=>{
				using (var popup = new UIAlertView("Hello", "This is a popup created from the action.", null, "OK")){
					popup.Show();
				}
			});
		}
	}
}

