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
using Escoz.Forms;
using Escoz.Forms.Elements;

namespace Escoz.Forms
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
					string v = "";
					if (data==null)
						v = json.asString(Constants.Value);
					else if (string.IsNullOrEmpty(json.asString(Constants.Bind)))
						v = "";
					else if (data.GetType()==typeof(JsonPrimitive))
						v = data.CleanString();
					else if (data.GetType()==typeof(JsonObject))
						v = ((JsonObject)data).asString(json.asString(Constants.Bind));
					
					return new MultilineElement(json.asString(Constants.Caption), v, json.asAction(dvc));
				}
			);
			
			result.Add("EntryElement", (json, dvc, data)=>{
					return new EntryElement(json.asString(Constants.Caption), json.asString("placeholder"), 
				            data==null? json.asString(Constants.Value) : data.CleanString(), json.asBoolean("ispassword")){
							KeyboardType = (UIKeyboardType)Enum.Parse(typeof(UIKeyboardType), json.asString("keyboard") ?? "Default"),
						}; 
				}
			);
			
			result.Add("ActionElement", (json, dvc, data)=>{ 
					ActionElement el = null;
					el = new ActionElement(json.asString(Constants.Caption), json.asString(Constants.Action), ()=>{
						dvc.InvokeAction(el);
					});
					return el;
				}
			);
			
			
			result.Add("ActivityElement", (json, dvc, data)=>{ 
					return new ActivityElement(json.asString(Constants.Caption), json.asString(Constants.Activity), json.asString(Constants.Value));
				}
			);
			
			result.Add("BooleanElement", (json, dvc, data)=>{
					return new BooleanElement(json.asString(Constants.Caption), 
				                          string.IsNullOrEmpty(data)? json.asBoolean(Constants.Value) : bool.Parse(data.CleanString())) ;
				}
			);
			
			result.Add("FloatElement", (json, dvc, data)=>{
					return new FloatElement(json.asString(Constants.Caption), (float)json.asDouble(Constants.Value)) ;
				}
			);
				
			result.Add("CheckboxElement", (json, dvc, data)=>{
					return new CheckboxElement(json.asString(Constants.Caption), json.asBoolean(Constants.Value));
				}
			);
			
			result.Add("MapElement", (json, dvc, data)=>{
					return new MapElement(json.asString(Constants.Caption), json.asString(Constants.Value), 
				                      new CLLocationCoordinate2D(json.asDouble("lat").Value, json.asDouble("lng").Value));
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

