
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

namespace MonoTouch.Forms
{
	
	public partial class JsonDialogViewController : DialogViewController 
	{
		protected JsonBindingContext Context;
		protected ActionElement rightBarItem, leftBarItem;
		Dictionary<string, string> _hiddenElements = new Dictionary<string, string>();
		private string _url;
		string _file, _values;
		public string DataRootName;
		
		public string Url {get {return _url;}}
		
		public JsonDialogViewController(IntPtr ptr):base(ptr){}
		
		public JsonDialogViewController(string file, bool pushing) : this(file, null, pushing) {}
		
		public JsonDialogViewController(string file, string values, bool pushing) : base(pushing) {
			_file = file;
			_values = values;
		}
		
		public JsonDialogViewController(RootElement root):base(root) {
			Loading(false);
		}
		
		
		public virtual void NetworkFailed(){
			Loading(false);
		}
		
		protected virtual void Loading(bool isLoading){
			InvokeOnMainThread(()=>{
				_shouldbeLoading = isLoading;
				if (isLoading) {
					if (string.IsNullOrEmpty(Title))
						Title = Constants.LoadingTitle;
					indicator.StartAnimating();
					this.indicator.Hidden = false;
					_shouldbeLoading = true;
				} else {
					if (string.IsNullOrEmpty(Title))
						Title = Constants.DefaultTitle;
					indicator.StopAnimating();
					this.indicator.Hidden = true;
				}
			});
		}
		
		public override void ViewDidLoad ()
		{
			if (_file != null)
				_processFile(_file, _values);
		}
		
		public void SetValue(string key, string value){
			if (_hiddenElements.ContainsKey(key))
				_hiddenElements.Remove(key);
			_hiddenElements.Add(key, value);
		}
		
		public string GetValue(string key){
			if (_hiddenElements.ContainsKey(key))
				return _hiddenElements[key];
			
			return null;
		}
		
		public Dictionary<string, string> GetAllValues(){
			var formValues = Context.Fetch();
			foreach (var v in _hiddenElements)
				formValues.Add(v.Key, v.Value);
			return formValues;
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
			
			if (file.StartsWith(Constants.Http)){
				Loading(true);
				var req = CreateRequestForUrl(file);
				new UrlConnection(Constants.UrlConnectionNameForm, req, (string result)=>{
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
			var json = _parse(file);
			JsonObject valueJson = null;
			
			if (!string.IsNullOrEmpty(values)){
				if (values.StartsWith(Constants.Http)){
					Context = new JsonBindingContext(this, json, Title);
					Loading(true);
					var req = CreateRequestForUrl(values);
					new UrlConnection(Constants.UrlConnectionNameData, req, (string result)=>{
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
		
		
		private void _configureDialog(JsonValue json, JsonObject valuesJson){
			
			if (json.ContainsKey(Constants.Grouped)) {
				Style = bool.Parse(json[Constants.Grouped].ToString()) ? UITableViewStyle.Grouped : UITableViewStyle.Plain;
			}
			if (json.ContainsKey(Constants.Title))
				Title = json[Constants.Title];
			
			if (json.ContainsKey(Constants.DataRoot)) 
				DataRootName = json[Constants.DataRoot];
			
			if (json.ContainsKey(Constants.RightBarItem)){
				NavigationItem.RightBarButtonItem = _createButtonItemFor(Constants.RightBarItem, json, valuesJson);
				
			}
			if (json.ContainsKey(Constants.LeftBarItem)){
				NavigationItem.LeftBarButtonItem = _createButtonItemFor(Constants.LeftBarItem, json, valuesJson);
			}
		}
		
		private UIBarButtonItem _createButtonItemFor(string property, JsonValue json, JsonObject valuesJson){
			UIBarButtonItem result = null;
			var item = (JsonObject)json[property];
			string datavalue = null, id = null;
			id = item.asString(Constants.Id);
			
			if (valuesJson!=null && !string.IsNullOrEmpty(id)){
				datavalue = valuesJson.asString(id);
			}
				
			if (item.ContainsKey(Constants.Action)) {
					rightBarItem = new ActionElement(item.asString(Constants.Caption), datavalue ?? item.asString(Constants.Action), null);
					rightBarItem.ID = new NSString(id);
			}	
			if (item.ContainsKey(Constants.Image)){
				result = new UIBarButtonItem(UIImage.FromBundle(item.asString(Constants.Image)), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
					InvokeAction(this.rightBarItem);
				});
			} else {
				result = new UIBarButtonItem(item.asString(Constants.Caption), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
					InvokeAction(this.rightBarItem);
				});
			}	
			return result;
		}
		
		private bool _shouldbeLoading = false;
		
		
		public override void LoadView () {
			base.LoadView();
			this.View.AddSubview(indicator);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			string forceNavControllerToStaySeemsToBeABug = "getalinktotheTNC" + this.NavigationController;
			forceNavControllerToStaySeemsToBeABug = forceNavControllerToStaySeemsToBeABug+"removeWarning";
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
	
}
