
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Sample
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	public partial class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			CustomElements.RegisterAll();
			
			InvokeOnMainThread(()=>{
				
				var sampleForm = new SampleFormController();
				var navigation = new UINavigationController(sampleForm);
				window.AddSubview(navigation.View);
				window.MakeKeyAndVisible ();
			
			});
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}

