///
// Reflect.cs: Creates Element classes from an instance
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Json;
using System.Linq;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;

namespace MonoTouch.Forms
{
	public partial class JsonBindingContext : IDisposable {
		public RootElement Root;
		List<Element> _elements;
		public JsonDialogViewController _controller;

		
		public JsonBindingContext (JsonDialogViewController controller, JsonValue json, string title)
		{
			_controller = controller;
			_elements = new List<Element>();
			
			if (json == null)
				throw new ArgumentNullException ("json");
			
			Root = new RootElement(title);
			
			try {
				Populate (json, Root, null);
			} catch (Exception e) {
				Console.WriteLine("Exception on JsonBindingContext " + e.ToString());
			}
		}
		
		public JsonBindingContext(JsonDialogViewController callbacks, JsonValue json, JsonValue data, string title) {
			_controller = callbacks;
			_elements = new List<Element>();
			
			if (json == null)
				throw new ArgumentNullException ("json");
			
			Root = new RootElement(title);
			try {
				Populate (json, Root, data);
			} catch (Exception e) {
				Console.WriteLine("Exception on JsonBindingContext " + e.ToString());
			}
			
		}
			
		void Populate (JsonValue json, RootElement root, JsonValue data)
		{
			if (json.ContainsKey("title"))
				root.Caption = json["title"];
			
			JsonValue jsonRoot = null;
			try {
				jsonRoot = json["root"];
			} catch (Exception){
				Console.WriteLine("Bad JSON: could not find the root element - "+json.ToString()) ;	
				return;
			}
			
			foreach (JsonObject section in jsonRoot){
				var sec = new Section(section.asString("caption"), section.asString("footer"));
				
				if (!string.IsNullOrEmpty(section.asString("captionImage"))) {
					var imgDevice = section.asString("captionImage").Split(' ');
					var img = UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone ? imgDevice[0] : imgDevice[1];
					sec.HeaderView = new UIImageView(UIImage.FromBundle(img));
				}
				
				if (section.ContainsKey("elements")){
					foreach (JsonObject elem in section["elements"]) {
						
						var dataForElement = data;
						var bindExpression = elem.asString("bind");
						if (bindExpression!=null) {
							try {
								if (data!=null && data.JsonType==JsonType.Object){
									var bind = elem.asString("bind");
									if (data != null && !string.IsNullOrEmpty(bind) && data.ContainsKey(bind)) {
										dataForElement = data[bind];
									}
								} else if (bindExpression.StartsWith("#")) {
									dataForElement = _controller.GetValue(bindExpression.Replace("#", "")); 	
								}
							} catch (Exception){
								Console.WriteLine("Exception when binding element " + elem.ToString());	
							}
						}
						
						_parseElement(elem, sec, dataForElement);
					}
					
				} else if (section.ContainsKey("iterate") && data != null){
					string iterationname = section["iterate"];	
					string emptyMessage = section.ContainsKey("empty") ? section["empty"].CleanString() : "Empty";
					var iterationdata = string.IsNullOrEmpty(iterationname) ? (JsonArray)data : (JsonArray)data[iterationname];
					var template = (JsonObject)section["template"];
					
					var items = iterationdata.ToList();
					if (items.Count>0) {
						foreach(JsonValue v in items){
							_parseElement(template, sec, v);
						}
					} else {
						sec.Add(new EmptyListElement(emptyMessage));
						
					}
					
				} else if (section.ContainsKey("iterateproperties") && data != null){
					string iterationname = section["iterateproperties"];	
					string emptyMessage = section.ContainsKey("empty") ? section["empty"].CleanString() : "Empty";
					
					var iterationdata = string.IsNullOrEmpty(iterationname) ? (JsonObject)data : (JsonObject)data[iterationname];
					var template = (JsonObject)section["template"];
					var items =  iterationdata.Keys;
					if (items.Count>0) {
						foreach(string v in items){
							var obj = new JsonObject();
							obj.Add(v, iterationdata[v]);
							_parseElement(template, sec, obj);
						}
					} else {
						sec.Add(new EmptyListElement(emptyMessage));
						
					}
				}
				root.Add(sec);
			}
		}
		
		public Element GetElement(string id){
			return (from e in _elements where e.ID==id select e).FirstOrDefault();	
		}
		
		public Dictionary<string, string> Fetch ()
		{
			var result = new Dictionary<string, string>();
			foreach (var el in _elements){
				if (!string.IsNullOrEmpty(el.ID) && _fetchFunctions.ContainsKey(el.GetType())){
					result.Add(el.ID, _fetchFunctions[el.GetType()](el));
				}
			}
			
			return result;
		}
		
		
		private void _parseElement(JsonObject json, Section section, JsonValue data){
			string type = "Unknown";
			try {
				type = json["type"];
				if (type=="HiddenElement"){
					var name = json.asString("id");
					
					_controller.SetValue(name, data==null? json.asString("value") : data.CleanString());	
				} else {
					string id = (json.ContainsKey("id") ? json["id"] : null);
					var newElement = _parseFunctions[type](json, _controller, data);
					if (newElement!=null) {
						if (!string.IsNullOrEmpty(id))
							newElement.ID = new NSString(id);
						
						_elements.Add(newElement);
						section.Add(newElement);
					}
				}
			} catch (Exception e){
				Console.WriteLine("Problem parsing element. Element was skipped. Type: "+type+" = " + e.ToString());
			}
		}
		
		public void Dispose ()
		{
			Dispose (true);
		}
		
		protected virtual void Dispose (bool disposing)
		{
			if (disposing){
				foreach (var element in _elements){
					element.Dispose ();
				}
			}
		}
		
		public static void RegisterFetcher(Type t, Func<Element, string> f) {
			_fetchFunctions.Add(t, f);	
		}
		
		public static void RegisterParser(string s, Func<JsonObject, JsonDialogViewController, JsonValue , Element> f) {
			_parseFunctions.Add(s, f);	
		}
		
		public static Dictionary<Type, Func<Element, string>> _fetchFunctions = DefaultElementsFetchers.Create();
		
		private static Dictionary<string, Func<JsonObject, JsonDialogViewController, JsonValue, Element>> _parseFunctions = 
			DefaultElementsParsers.Create();
	}
}
	