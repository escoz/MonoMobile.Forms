//
// ElementParsers.cs
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
using System.Json;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoMobile.Forms;
using MonoMobile.Forms.Elements;

namespace MonoMobile.Forms
{
	
	[Preserve(AllMembers=true)]
	public class ElementParsers
	{
		
		public static Dictionary<string, Func<JsonObject, FormDialogViewController, JsonValue, Element>> Parsers = 
			ElementParsers.CreateDefaultParsers();
		
		
		private static Dictionary<string, Func<JsonObject, FormDialogViewController, JsonValue, Element>> CreateDefaultParsers()
		{
			var result = new Dictionary<string, Func<JsonObject, FormDialogViewController, JsonValue, Element>>();
			
			result.Add("StringElement", (json, dvc, data)=>{
					string v = "";
					if (data==null)
						v = json.asString(Constants.Value);
					else if (string.IsNullOrEmpty(json.asString(Constants.Bind)))
						v = "";
					else if (data.GetType()==typeof(JsonPrimitive))
						v = data.CleanString();
					else if (data.GetType()==typeof(JsonObject))
						v = ((JsonObject)data).asString(json.asString(Constants.Bind));
					
					return new StringElement(json.asString(Constants.Caption), v, json.asAction(dvc));
				}
			);
			
			result.Add("MultilineElement", (json, dvc, data)=>{
					string caption = "";
					if (data==null)
						caption = json.asString(Constants.Caption);
					else if (string.IsNullOrEmpty(json.asString(Constants.Bind)))
						caption = "";
					else if (data.GetType()==typeof(JsonPrimitive))
						caption = data.CleanString();
					else if (data.GetType()==typeof(JsonObject))
						caption = ((JsonObject)data).asString(json.asString(Constants.Bind));
					
					return new MultilineElement(caption, json.asString(Constants.Value), json.asAction(dvc));
				}
			);
			
			
			result.Add("MultilineEntryElement", (json, dvc, data)=>{
					return new MultilineEntryElement(json.asString("caption"), json.asString("placeholder"), 
				            data==null? json.asString(Constants.Value) : data.CleanString()){
							KeyboardType = (UIKeyboardType)Enum.Parse(typeof(UIKeyboardType), json.asString("keyboard") ?? "Default"),
						}; 
				}
			);
			
			result.Add("EntryElement", (json, dvc, data)=>{
					return new EntryElement(json.asString(Constants.Caption), json.asString("placeholder"), 
				            data==null? json.asString(Constants.Value) : data.CleanString(), json.asBoolean("ispassword")){
							KeyboardType = (UIKeyboardType)Enum.Parse(typeof(UIKeyboardType), json.asString("keyboard") ?? "Default"),
						}; 
				}
			);
			
			result.Add("ButtonElement", (json, dvc, data)=>{ 
					ButtonElement el = null;
					el = new ButtonElement(json.asString(Constants.Caption), new ControllerAction(json.asString(Constants.Action)));
					return el;
				}
			);
			
			
			result.Add("ActivityElement", (json, dvc, data)=>{ 
					return new ActivityElement(json.asString(Constants.Caption), json.asString(Constants.Activity), json.asString(Constants.Value));
				}
			);
			
			result.Add("BooleanElement", (json, dvc, data)=>{
					return new iPhoneBooleanElement(json.asString(Constants.Caption), 
				                          string.IsNullOrEmpty(data)? json.asBoolean(Constants.Value) : bool.Parse(data.CleanString())) ;
				}
			);
			
			result.Add("FloatElement", (json, dvc, data)=>{
					var minValue = json.asDouble("minvalue");
					var maxValue = json.asDouble("maxvalue");
					return new FloatElement(json.asString(Constants.Caption), (float)json.asDouble(Constants.Value)) 
						{ MinValue = minValue.HasValue? (float)minValue.Value : 0f, 
						  MaxValue = maxValue.HasValue? (float)maxValue.Value : 1f
					    };
				}
			);
				
			result.Add("CheckboxElement", (json, dvc, data)=>{
					return new CheckboxElement(json.asString(Constants.Caption), json.asBoolean(Constants.Value));
				}
			);
			
			result.Add("MapElement", (json, dvc, data)=>{
					JsonArray position = data==null? null : (JsonArray)data;
					double lat = position==null? json.asDouble("lat").Value : (double)position[0];
					double lng = position==null? json.asDouble("lng").Value : (double)position[1];
					string value = position==null? json.asString(Constants.Value) : position.Count > 2 ? (string)position[2] : "";
					return new MapElement(json.asString(Constants.Caption), value, 
				                      new CLLocationCoordinate2D(lat, lng));
				}
			);

			result.Add("RadioElement", (json, dvc, data)=>{
					var radios = new List<RadioElement>();
					var popAutomatically = json.asBoolean("pop");
					foreach (string item in json.sl("items")){
						radios.Add(new RadioElement(item, popAutomatically));
					}
					int selected = 0;
					if (json.ContainsKey("selected")) {
						selected = (int)json.asDouble("selected");
					} else if (!string.IsNullOrEmpty(data)) {
						selected = int.Parse(data);
					}
					return new RootElement(json.asString(Constants.Caption), new RadioGroup(null, selected)) {
						new Section(){
							radios.ToArray()		
						}
					};
				}
			);
			
			result.Add("ImagePickerElement", (json, dvc, data)=>{
				return new ImagePickerElement(null);
			});
			
			result.Add("DateElement", (json, dvc, data)=>{
				return new DateElement(json.asString(Constants.Caption), json.asDateTime(Constants.Value));
			});
			
			result.Add("TimeElement", (json, dvc, data)=>{
				return new TimeElement(json.asString(Constants.Caption), json.asDateTime(Constants.Value));
			});
			
			result.Add("DateTimeElement", (json, dvc, data)=>{
				return new DateTimeElement(json.asString(Constants.Caption), json.asDateTime(Constants.Value));
			});
				
			result.Add("ImageStringElement", (json, dvc, data)=>{
				return  new ImageStringElement(json.asString(Constants.Caption), json.asString(Constants.Value), 
				        json.asAction(dvc), json.asUIImage(Constants.Image), json.asString("imageurl"));
			});
			
			result.Add("WebElement", (json, dvc, data)=>{ 
					var url= data==null ? json.asString("url") : data.CleanString();
					if (string.IsNullOrEmpty(url) || (data!=null && data.GetType()!=typeof(JsonPrimitive)))
					    return null;
					return new WebElement(json.asString(Constants.Caption), url);
			});
			
			return result;
		}
	}
}

