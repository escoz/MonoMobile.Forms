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
		public IRoot Root;
		List<Element> _elements;
		public JsonDialogViewController _controller;

		
		public JsonBindingContext (JsonDialogViewController controller, JsonValue json, string title)
		{
			_controller = controller;
			_elements = new List<Element>();
			
			if (json == null)
				throw new ArgumentNullException ("json");
			
			Root = new RootElement<string>(title);
			
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
			
			Root = new RootElement<string>(title);
			try {
				Populate (json, Root, data);
			} catch (Exception e) {
				Console.WriteLine("Exception on JsonBindingContext " + e.ToString());
			}
			
		}
			
		void Populate (JsonValue json, IRoot root, JsonValue data)
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
				var sec = new Section(section.s("caption"), section.s("footer"));
				
				if (!string.IsNullOrEmpty(section.s("captionImage"))) {
					var imgDevice = section.s("captionImage").Split(' ');
					var img = UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone ? imgDevice[0] : imgDevice[1];
					sec.HeaderView = new UIImageView(UIImage.FromBundle(img));
				}
				
				if (section.ContainsKey("elements")){
					foreach (JsonObject elem in section["elements"]) {
						
						var dataForElement = data;
						var bindExpression = elem.s("bind");
						if (bindExpression!=null) {
							try {
								if (data!=null && data.JsonType==JsonType.Object){
									var bind = elem.s("bind");
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
			return (from e in _elements where e.Id==id select e).FirstOrDefault();	
		}
		
		public Dictionary<string, string> Fetch ()
		{
			var result = new Dictionary<string, string>();
			foreach (var el in _elements){
				if (!string.IsNullOrEmpty(el.Id) && _fetchFunctions.ContainsKey(el.GetType())){
					result.Add(el.Id, _fetchFunctions[el.GetType()](el));
				}
			}
			
			return result;
		}
		
		
		private void _parseElement(JsonObject json, Section section, JsonValue data){
			string type = "Unknown";
			try {
				type = json["type"];
				if (type=="HiddenElement"){
					var name = json.s("id");
					
					_controller.SetValue(name, data==null? json.s("value") : data.CleanString());	
				} else {
					string id = (json.ContainsKey("id") ? json["id"] : null);
					var newElement = _parseFunctions[type](json, _controller, data);
					if (newElement!=null) {
						newElement.Id = new NSString(id);
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
		
		public static Dictionary<Type, Func<Element, string>> _fetchFunctions = new Dictionary<Type, Func<Element, string>>(){
				{typeof(EntryElement), (e)=>{return ((EntryElement)e).Value; } },
				{typeof(BooleanElement), (e)=>{return ((BooleanElement)e).Value ? "1" : "0";}},
				{typeof(RadioElement), (e)=>{return ((RadioElement)e).Value.ToString();}},
				//{typeof(RootElement), (e)=>{return ((RootElement)e).RadioSelected.ToString();}},
				{typeof(DateElement), (e)=>{return ((DateElement)e).Value.ToLongDateString(); }},
				{typeof(TimeElement), (e)=>{return ((TimeElement)e).Value.ToLongTimeString(); }},
				{typeof(DateTimeElement), (e)=>{return ((DateTimeElement)e).Value.ToLongDateString(); }}				
		};
		
		private static Dictionary<string, Func<JsonObject, JsonDialogViewController, JsonValue, Element>> _parseFunctions = 
			new Dictionary<string, Func<JsonObject, JsonDialogViewController, JsonValue, Element>>(){
				{"EntryElement", (json, dvc, data)=>{
					return new EntryElement(json.s("caption"), json.s("placeholder"), data==null? json.s("value") : data.CleanString(), json.b("ispassword")){
							KeyboardType = (UIKeyboardType)Enum.Parse(typeof(UIKeyboardType), json.s("keyboard") ?? "Default"),
						}; }},
				{"ActionElement", (json, dvc, data)=>{ 
					ActionElement el = null;
					el = new ActionElement(json.s("caption"), json.s("action"), ()=>{
						dvc.InvokeAction(el);
					});
					return el;
				}},
				{"SubmitElement", (json, dvc, data)=>{
					SubmitElement el = null;
					el = new SubmitElement(json.s("caption"), data==null? json.s("url") : data.CleanString(), json.s("action"), ()=>{
						dvc.InvokeAction(el);
					});
					return el;
				}},
				{"BooleanElement", (json, dvc, data)=>{return new BooleanElement(json.s("caption"), json.b("value")) ;}},
				{"StringElement", (json, dvc, data)=>{
					string v = "";
					if (data==null)
						v = json.s("value");
					else if (string.IsNullOrEmpty(json.s("bind")))
						v = "";
					else if (data.GetType()==typeof(JsonPrimitive))
						v = data.CleanString();
					
					return new StringElement(json.s("caption"), v);
				}},
				//{"ImageStringElement", (json, dvc, data)=>{return  new ImageStringElement(json.s("caption"), json.s("value"), json.a(dvc), json.i("image"), json.s("imageurl"));}},
				{"MapElement", (json, dvc, data)=>{return new MapElement(json.s("caption"), json.s("value"), new CLLocationCoordinate2D(json.d("lat").Value, json.d("lng").Value));}},
				//{"WebElement", (json, dvc, data)=>{ 
				//	var url= data==null ? json.s("url") : data.CleanString();
				//	if (string.IsNullOrEmpty(url) || (data!=null && data.GetType()!=typeof(JsonPrimitive)))
				//	    return null;
				//return new WebElement(json.s("caption"), url);
				//}},
				{"RadioElement", (json, dvc, data)=>{
					var radios = new List<RadioElement>();
					var popAutomatically = json.b("pop");
					foreach (string item in json.sl("items")){
						radios.Add(new RadioElement(item, popAutomatically));
					}
					int selected = 0;
					if (json.ContainsKey("selected")) {
						selected = (int)json.d("selected");
					} else if (!string.IsNullOrEmpty(data)) {
						selected = int.Parse(data);
					}
					return new RootElement<int>(json.s("caption"), new RadioGroup(null, selected)) {
						new Section(){
							radios.ToArray()		
						}
					};
				}},
				{"DateElement", (json, dvc, data)=>{return  new DateElement(json.s("caption"), json.dt("value"));}},
				{"TimeElement", (json, dvc, data)=>{return  new TimeElement(json.s("caption"), json.dt("value"));}},
				{"DateTimeElement", (json, dvc, data)=>{return  new DateTimeElement(json.s("caption"), json.dt("value"));}},
				//{"MultilineElement", (json, dvc, data)=>{ return  new MultilineElement(data==null ? json.s("caption") : data.CleanString() ?? "", null, json.a(dvc));}},
				//{"SimpleImageElement", (json, dvc, data)=>{ return  new SimpleImageElement(
				//	                            UIImage.FromBundle(json.s(UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone? "url" : "urlIpad")));
				//}},
				//{"Template", (json, dvc, data)=>{return  null;}},
		};
		
	}
	

}
	