//
// JsonSectionBuilder.cs
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
using MonoTouch.Dialog;
using System.Json;
using MonoTouch.UIKit;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.Foundation;
namespace MonoTouch.Forms
{
	public class JsonSectionBuilder
	{
		JsonDialogViewController _controller;
		
		public JsonSectionBuilder (JsonDialogViewController controller)
		{
			_controller = controller;
		}
		
		public Section Build(JsonObject section, JsonValue data)
		{
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
							} catch (Exception e){
								Console.WriteLine("Exception when binding element " + elem.ToString()+ " - " + e.ToString());	
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
					
					var iterationdata = string.IsNullOrEmpty(iterationname) ? (JsonObject)data 
						: data.ContainsKey(iterationname) ? (JsonObject)data[iterationname] : null;
					var template = (JsonObject)section["template"];
					var items =  iterationdata == null ? new List<string>() : iterationdata.Keys;
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
			
			
			return sec;
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
					var newElement = ElementParsers.Parsers[type](json, _controller, data);
					if (newElement!=null) {
						if (!string.IsNullOrEmpty(id))
							newElement.ID = new NSString(id);
						
						section.Add(newElement);
					}
				}
			} catch (Exception e){
				Console.WriteLine("Problem parsing element. Element was skipped. Type: "+type+" = " + e.ToString());
			}
		}
		
		
	}
}

