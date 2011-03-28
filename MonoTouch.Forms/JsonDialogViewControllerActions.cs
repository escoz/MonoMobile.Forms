using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;
using System.Reflection;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
namespace MonoTouch.Forms
{
	public partial class JsonDialogViewController
	{
		public void InvokeAction(string action , params object[] parameters){
			this.GetType().InvokeMember(action,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, 
				    null, this, parameters);
		}
		
		public void InvokeAction(ActionElement action){
			_invokeAction(action.Action, action);
		}
		
		public void InvokeAction(string action, Element element){
			_invokeAction(action, element);
		}
		
		private void _invokeAction(string action, Element element){
			this.GetType().InvokeMember(action,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
				    null, this, new object[]{element});
		}
		
		public void DismissModal(Element el){
			this.DismissModalViewControllerAnimated(true);	
		}
		
		public void NavigateTo(string file, IDictionary<string, string> hiddenfields){
			var dialog = this;
			UrlConnection.KillAllConnections();
			InvokeOnMainThread(()=>{
				var newcontroller = (JsonDialogViewController)Activator.CreateInstance(this.GetType(), file, true);
				foreach (var v in hiddenfields)
					newcontroller.SetValue(v.Key, v.Value);
				
				dialog.NavigationController.PushViewController(newcontroller, true);
			});
		}
		
		public void NavigateTo(string file){
			var dialog = this;
			UrlConnection.KillAllConnections();
			InvokeOnMainThread(()=>{
				var controller = (JsonDialogViewController)Activator.CreateInstance(this.GetType(), file, true);
				dialog.NavigationController.PushViewController(controller, true);
			});
		}
		
		
		private void _invokeSubmissionResultAction(string action, JsonObject json){
			this.GetType().InvokeMember(action,
				    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, 
				    null, this, new object[]{json});
		}
		
		public void Reload(Element el){
			PrepareRoot(new RootElement(Title));
			ReloadData();
			_processFile(Url, null);
		}
//		
//		public void Submit(){
//			_submitForm(null);
//		}
//		
//		
//		private void _submitForm(ActionElement el){
//			
//			var values = Context.Fetch();
//			if (el!=null)
//				values.Add("commit", el.Id);
//			
//			Loading(true);
//			var req = CreateRequestForUrl(el.Url);
//			string parameters = "";
//			foreach (var v in values) {
//				parameters += v.Key+"="+v.Value+"&";	// TODO: ugly hack, do we need to encode chars here?
//			}
//			req.HttpMethod = "POST";
//			
//			req.Body = NSData.FromString(parameters);
//			new UrlConnection("formPost", req, (string value)=>{
//				Loading(false);
//				_processSubmissionResult(value);
//
//			}, (error)=>{
//				Loading(false);	
//				NetworkFailed();
//			});
//		}
//		
//		private void _processSubmissionResult(string value){
//			JsonObject json = ((JsonObject)(_parse(value)["result"]));
//			
//			if (!string.IsNullOrEmpty(json.asString("message"))){
//				using (var v = new UIAlertView(json.asBoolean("success") ? "Success" : "Error", json.asString("message"), null, "OK")) {
//					v.Show();
//				}
//			}
//			var action = json.asString("action");
//			var url = json.asString("navigateto");
//			if (!string.IsNullOrEmpty(action)){
//				_invokeSubmissionResultAction(action, json);
//			} else if (!string.IsNullOrEmpty(url)){
//				NavigateTo(url);
//			} else if (json.asBoolean("success")) {
//				if (NavigationController.ViewControllers[0]==this &&
//				    NavigationController!=null && NavigationController.ParentViewController!=null &&
//				    NavigationController.ParentViewController.ModalViewController==NavigationController){
//					NavigationController.ParentViewController.DismissModalViewControllerAnimated(true);
//				} else {
//					NavigationController.PopViewControllerAnimated(true);
//				}
//			}
//		}
//		
		
		
	}
}

