
using System;
using System.Net;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Json;
using System.IO;
using MonoTouch.Foundation;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using MonoTouch.Forms.Elements;

namespace MonoTouch.Forms
{
	
	public partial class JsonDialogViewController : DialogViewController 
	{
		public JsonBindingContext Context;
		private ActionElement rightBarItem, leftBarItem;
		private string _url;
		public string DataRootName;
		
		public string Url {get {return _url;}}
		
		//public JsonDialogViewController(IntPtr ptr):base(ptr){}
		
		public JsonDialogViewController(string file, bool pushing) : this(file, null, pushing) {}
		
		public JsonDialogViewController(string file, string values, bool pushing) : base(pushing) {
			_file = file;
			_values = values;
		}
		
		string _file, _values;
		
		public JsonDialogViewController(IRoot root):base(root) {
			Loading(false);
			
		}
		
		private Dictionary<string, string> BindingValues = new Dictionary<string, string>();
		
		public void ShowBindingValues(){
			Console.WriteLine("========== binding values");
			foreach (var v in BindingValues)
				Console.WriteLine("kv: "+v.Key+" - " +v.Value);
		}
		
		public void SetValue(string name, string v){
			if (BindingValues.ContainsKey(name))
				BindingValues.Remove(name);
			
			BindingValues.Add(name, v);
		}
		
		public string GetValue(string name){
			return BindingValues[name];
		}
		
		public override void ViewDidLoad ()
		{
			if (_file != null)
				_processFile(_file, _values);
		}
		
		public virtual NSMutableUrlRequest CreateRequestForUrl(string url) {
			return new NSMutableUrlRequest(new NSUrl(url), NSUrlRequestCachePolicy.UseProtocolCachePolicy, 15);
		}
		
		private void _processFile(string file, string values) {
			if (string.IsNullOrEmpty(values) && file.Contains(' ')) {
				var temp = file.Split(' ');
				file = temp[0];
				values = temp[1];
			}
			
			_url = file + " " + values;
			
			if (file.StartsWith("http")){
				Loading(true);
				var req = CreateRequestForUrl(file);
				new UrlConnection("dialog", req, (string result)=>{
					InvokeOnMainThread(()=>{
						_processContentOfFile(result, values);	
						LoadView();
						Loading(false);
						
					});
				}, (error)=>{ NetworkFailed(); });
			} else {
				_processContentOfFile(File.ReadAllText(file), values);
				
			}
		}
		
		private void _processContentOfFile(string file, string values){
			Loading(true);
			var json = _parse(file);
			JsonObject valueJson = null;
			
			if (!string.IsNullOrEmpty(values)){
				if (values.StartsWith("http")){
					Context = new JsonBindingContext(this, json, Title);
					Loading(true);
					var req = CreateRequestForUrl(values);
					new UrlConnection("dialogValues", req, (string result)=>{
						InvokeOnMainThread(()=>{
							JsonValue resultValue = null;
							try {
								resultValue = JsonObject.Parse(result);
							
							} catch (Exception e){
								Console.WriteLine("Exception while parsing result: " +e.ToString() +" Values: " + values);
								return;
								
							}
							if (resultValue.JsonType==JsonType.Object)
								valueJson = (JsonObject)resultValue;
						
							if (!string.IsNullOrEmpty(DataRootName)) {
								resultValue = ((JsonObject)resultValue).asJsonValue(DataRootName);
							}
							
							Context = new JsonBindingContext(this, json,  resultValue, Title);
							_configureDialog(json, valueJson);
							PrepareRoot(Context.Root);
							ReloadData();
							Loading(false);
							
						});
					}, (error)=>{ NetworkFailed(); });
					
				} else {
					var datavalue = JsonObject.Parse(File.ReadAllText(values));
					Context = new JsonBindingContext(this, json, datavalue, Title);
				}
					
			} else {
				Context = new JsonBindingContext(this, json, Title);
				Loading(false);
			}
			_configureDialog(json, valueJson);
			PrepareRoot(Context.Root);
			LoadView();
			ReloadData();
		}
		
		public virtual void NetworkFailed(){
			Loading(false);
		}
		
		
		private void _configureDialog(JsonValue json, JsonObject valuesJson){
			if (json.ContainsKey("grouped")) {
				Style = bool.Parse(json["grouped"].ToString()) ? UITableViewStyle.Grouped : UITableViewStyle.Plain;
			}
			if (json.ContainsKey("title"))
				Title = json["title"];
			
			if (json.ContainsKey("dataroot")) 
				DataRootName = json["dataroot"];
			
			if (json.ContainsKey("rightbaritem")){
				var item = (JsonObject)json["rightbaritem"];
				string datavalue = null, id = null;
				id = item.asString("id");
				if (valuesJson!=null && !string.IsNullOrEmpty(id)){
					datavalue = valuesJson.asString(id);
				}
					
				if (item.ContainsKey("action")) {
						rightBarItem = item.ContainsKey("url") ? 
							new SubmitElement(item.asString("caption"), datavalue ?? item.asString("url"), null, null) :
							new ActionElement(item.asString("caption"), datavalue ?? item.asString("action"), null);
						rightBarItem.Id = new NSString(id);
				}	
				if (item.ContainsKey("image")){
					NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIImage.FromBundle(item.asString("image")), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
						InvokeAction(this.rightBarItem);
					});
				} else {
					NavigationItem.RightBarButtonItem = new UIBarButtonItem(item.asString("caption"), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
					InvokeAction(this.rightBarItem);
				});
				}
			}
			if (json.ContainsKey("leftbaritem")){
				var item = (JsonObject)json["leftbaritem"];
				if (item.ContainsKey("action")) {
						leftBarItem = item.ContainsKey("url") ? 
							new SubmitElement(item.asString("caption"), item.asString("url"), null, null) :
							new ActionElement(item.asString("caption"), item.asString("action"), null);
						leftBarItem.Id = new NSString(item.asString("id"));
				}	
				NavigationItem.LeftBarButtonItem = new UIBarButtonItem(item.asString("caption"), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
					InvokeAction(this.leftBarItem);
				});
			}
			
		}
		
		private bool _shouldbeLoading = false;
		
		protected virtual void Loading(bool isLoading){
			InvokeOnMainThread(()=>{
				_shouldbeLoading = isLoading;
				if (isLoading) {
					if (string.IsNullOrEmpty(Title))
						Title = "Loading...";
					indicator.StartAnimating();
					this.indicator.Hidden = false;
					_shouldbeLoading = true;
				} else {
					if (string.IsNullOrEmpty(Title))
						Title = "Form";
					indicator.StopAnimating();
					this.indicator.Hidden = true;
				}
			});
		}
		
		public override void LoadView () {
			base.LoadView();
			this.View.AddSubview(indicator);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			Loading(_shouldbeLoading);
		}
		
		private JsonValue _parse(string str){
			try {
				return JsonValue.Parse(str);
			} catch (Exception e){
				Console.WriteLine("Error while parsing json - " + e.ToString() + "\n" + str);
				return new JsonObject();
			}
		}
		
		private UIIndicatorView indicator = new UIIndicatorView();
		
	}
	
	public class UIIndicatorView : UIView {
		private UIActivityIndicatorView _ind = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray) { Frame = new RectangleF(148,12,24,24) };
		
		public UIIndicatorView(){
			Frame = new RectangleF(0,0,800,800);
			this.UserInteractionEnabled = true;
		}
		
		public override void LayoutSubviews ()
		{
			this.AddSubview(_ind);
		}
		
		public void StartAnimating(){
			_ind.StartAnimating();
		}
		
		public void StopAnimating(){
			_ind.StopAnimating();	
		}
		
	}
}
