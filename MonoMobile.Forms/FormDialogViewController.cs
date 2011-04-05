//
// FormDialogViewController.cs
//
// Author:
//   Eduardo Scoz (contact@escoz.com)
//
// Copyright 2010, ESCOZ, Inc.
//
// Code licensed under the MIT X11 license
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoMobile.Forms;
using System.Linq;
using MonoMobile.Util;
using MonoMobile.Forms.Activities;

namespace MonoMobile.Forms
{
	
	public partial class FormDialogViewController : DialogViewController 
	{
		protected FormBindingContext Context;
		Dictionary<string, string> _elementValues = new Dictionary<string, string>();
		private string _url;
		string _file, _values;
		public string DataRootName;
		
		public string Url {get {return _url;}}
		
		public FormDialogViewController(IntPtr ptr):base(ptr){}
		
		public FormDialogViewController(string file, bool pushing) : this(file, null, pushing) {}
		
		public FormDialogViewController(string file, string values, bool pushing) : base(pushing) {
			_file = file;
			_values = values;
		}
		
		public FormDialogViewController(RootElement root):base(root) {
			Loading = false;
		}
		
		public virtual void NetworkFailed(NSError error){
			Loading = false;
			InvokeOnMainThread(()=>{
				using (var popup = new UIAlertView("Error", error.LocalizedDescription, null, "OK")){
					popup.Show();
				}
			});
		}
		
		public virtual bool Loading {
			set {
				InvokeOnMainThread(()=>{
					_shouldbeLoading = value;
					if (value) {
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
		}
		
		public override void ViewDidLoad ()
		{
			if (_file != null)
				_processFile(_file, _values);
		}
		
		public void SetValue(string key, string value){
			if (_elementValues.ContainsKey(key))
				_elementValues.Remove(key);
			_elementValues.Add(key, value);
		}
		
		public string GetValue(string key){
			if (_elementValues.ContainsKey(key))
				return _elementValues[key];
			
			return null;
		}
		
		public Dictionary<string, string> GetAllValues(){
			_fetch();
			return new Dictionary<string, string>(_elementValues);
		}
		
		private void _fetch(){
			var formValues = Context.Fetch();
			foreach (var v in formValues) {
				if (_elementValues.ContainsKey(v.Key))
					_elementValues.Remove(v.Key);
			
				_elementValues.Add(v.Key, v.Value);
			}
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
				Loading = true;
				var req = CreateRequestForUrl(file);
				new UrlConnection(Constants.UrlConnectionNameForm, req, (string result)=>{
					InvokeOnMainThread(()=>{
						_processContentOfFile(result, values);	
						LoadView();
						Loading = false;
						
					});
				}, (error)=>{ NetworkFailed(error); });
			} else {
				_processContentOfFile(File.ReadAllText(file), values);
			}
		}
		
		private void _processContentOfFile(string file, string values){
			
			var json = _parse(file);
			JsonObject valueJson = null;
			
			if (!string.IsNullOrEmpty(values)){
				if (values.StartsWith(Constants.Http)){
					Context = new FormBindingContext(this, json, Title);
					Loading = true;
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
							
							Context = new FormBindingContext(this, json,  resultValue, Title);
							_configureDialog(json, valueJson);
							PrepareRoot(Context.Root);
							ReloadData();
							Loading = false;
							
						});
					}, (error)=>{ NetworkFailed(error); });
					
				} else {
					var datavalue = JsonObject.Parse(File.ReadAllText(values));
					Context = new FormBindingContext(this, json, datavalue, Title);
				}
					
			} else {
				Context = new FormBindingContext(this, json, Title);
				Loading = false;
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
			var item = (JsonObject)json[property];
			string datavalue = null, id = null;
			id = item.asString(Constants.Id);
			Activity action = null;
			UIBarButtonItem button;
			if (valuesJson!=null && !string.IsNullOrEmpty(id)){
				datavalue = valuesJson.asString(id);
			}
				
			if (item.ContainsKey(Constants.Action)) {
				action = new ControllerAction(datavalue ?? item.asString(Constants.Action));
			} else if (item.ContainsKey(Constants.Activity)) {
				action = ActivityFactory.Create(item.asString(Constants.Activity));
			}
			
			if (action==null)
				return null;
			
			if (item.ContainsKey(Constants.Image)){
				button = new UIBarButtonItem(UIImage.FromBundle(item.asString(Constants.Image)), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
					action.Execute(this, null, null);
				});
			} else {
				button = new UIBarButtonItem(item.asString(Constants.Caption), UIBarButtonItemStyle.Plain, (object o, EventArgs a)=>{
					action.Execute(this, null, null);
				});
			}	
			return button;
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
			Loading = _shouldbeLoading;
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
