using System;
using MonoTouch.Forms;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Collections.Generic;

namespace Sample
{
	public class SampleFormController : JsonDialogViewController
	{
		public SampleFormController () : base("js/sample.js", true){}
		public SampleFormController(RootElement root):base(root){}
		public SampleFormController(string url, bool pushing):base(url, pushing){}
		
		
		public void ShowPopup(Element element){
			InvokeOnMainThread(()=>{
				using (var popup = new UIAlertView("Hello", "This is a popup created from the action.", null, "OK")){
					popup.Show();
				}
			});
		}
		
		public void LoginFormPopulated(Element element){
			var values = new Dictionary<string, string>(){{"login", "escoz"}, {"password","mypass"},{"remember_me", "true"}};
			this.NavigateTo("js/login.js", values);	
		}
	}
}

