
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
	public class WebElement : Element {
		NSUrl nsUrl;
		static NSString hkey = new NSString ("HtmlElement");
		UIWebView web;
		
		public WebElement (string caption, string url) : base (caption)
		{
			Url = url;
		}
		
		public WebElement (string caption, NSUrl url) : base (caption)
		{
			nsUrl = url;
		}
		
		public string Url {
			get {
				return nsUrl.ToString ();
			}
			set {
				if (value!=null)
					nsUrl = new NSUrl (value);
			}
		}
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (hkey);
			if (cell == null){
				cell = new UITableViewCell (UITableViewCellStyle.Default, hkey);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			cell.TextLabel.Text = Caption;
			return cell;
		}

		static bool NetworkActivity {
			set {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = value;
			}
		}
		
		// We use this class to dispose the web control when it is not
		// in use, as it could be a bit of a pig, and we do not want to
		// wait for the GC to kick-in.
		class WebViewController : UIViewController {
			WebElement container;
			
			public WebViewController (WebElement container) : base ()
			{
				this.container = container;
			}
			
			public override void ViewWillDisappear (bool animated)
			{
				base.ViewWillDisappear (animated);
				NetworkActivity = false;
				if (container.web == null)
					return;
				container.web.StopLoading ();
				container.web.Dispose ();
				container.web = null;
			}
			
			public bool Autorotate { get; set; }
			
			public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
			{
				return Autorotate;
			}
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			var vc = new WebViewController (this) {
				Autorotate = dvc.Autorotate
			};
			web = new UIWebView (UIScreen.MainScreen.ApplicationFrame){
				BackgroundColor = UIColor.White,
				ScalesPageToFit = true,
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
			};
			web.LoadStarted += delegate {
				NetworkActivity = true;
				var indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
				vc.NavigationItem.RightBarButtonItem = new UIBarButtonItem(indicator);
				indicator.StartAnimating();
			};
			web.LoadFinished += delegate {
				NetworkActivity = false;
				vc.NavigationItem.RightBarButtonItem = null;
			};
			web.LoadError += (webview, args) => {
				NetworkActivity = false;
				vc.NavigationItem.RightBarButtonItem = null;
				if (web != null)
					web.LoadHtmlString (String.Format ("<html><center><font size=+5 color='red'>An error occurred:<br>{0}</font></center></html>", args.Error.LocalizedDescription), null);
			};
			vc.NavigationItem.Title = Caption;
			vc.View = web;
			
			dvc.ActivateController (vc, dvc);
			web.LoadRequest (NSUrlRequest.FromUrl (nsUrl));
		}
	}
}
