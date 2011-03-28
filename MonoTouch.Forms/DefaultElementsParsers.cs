using System;
using MonoTouch.Foundation;
using System.Json;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using System.Collections.Generic;
using MonoTouch.Forms.Elements;
namespace MonoTouch.Forms
{
	
	[Preserve(AllMembers=true)]
	public class DefaultElementsParsers
	{
		public static Dictionary<string, Func<JsonObject, JsonDialogViewController, JsonValue, Element>> Create()
		{
			var result = new Dictionary<string, Func<JsonObject, JsonDialogViewController, JsonValue, Element>>();
			
			result.Add("StringElement", (json, dvc, data)=>{
					string v = "";
					if (data==null)
						v = json.asString("value");
					else if (string.IsNullOrEmpty(json.asString("bind")))
						v = "";
					else if (data.GetType()==typeof(JsonPrimitive))
						v = data.CleanString();
					else if (data.GetType()==typeof(JsonObject))
						v = ((JsonObject)data).asString(json.asString("bind"));
					
					return new StringElement(json.asString("caption"), v, json.asAction(dvc));
				}
			);
			
			result.Add("MultilineElement", (json, dvc, data)=>{
					string v = "";
					if (data==null)
						v = json.asString("value");
					else if (string.IsNullOrEmpty(json.asString("bind")))
						v = "";
					else if (data.GetType()==typeof(JsonPrimitive))
						v = data.CleanString();
					else if (data.GetType()==typeof(JsonObject))
						v = ((JsonObject)data).asString(json.asString("bind"));
					
					return new MultilineElement(json.asString("caption"), v, json.asAction(dvc));
				}
			);
			
			result.Add("EntryElement", (json, dvc, data)=>{
					return new EntryElement(json.asString("caption"), json.asString("placeholder"), 
				            data==null? json.asString("value") : data.CleanString(), json.asBoolean("ispassword")){
							KeyboardType = (UIKeyboardType)Enum.Parse(typeof(UIKeyboardType), json.asString("keyboard") ?? "Default"),
						}; 
				}
			);
			
			result.Add("ActionElement", (json, dvc, data)=>{ 
					ActionElement el = null;
					el = new ActionElement(json.asString("caption"), json.asString("action"), ()=>{
						dvc.InvokeAction(el);
					});
					return el;
				}
			);
			
			result.Add("BooleanElement", (json, dvc, data)=>{
					return new BooleanElement(json.asString("caption"), json.asBoolean("value")) ;
				}
			);
			
			result.Add("FloatElement", (json, dvc, data)=>{
					return new FloatElement(json.asString("caption"), (float)json.asDouble("value")) ;
				}
			);
				
			result.Add("CheckboxElement", (json, dvc, data)=>{
					return new CheckboxElement(json.asString("caption"), json.asBoolean("value"));
				}
			);
			
			result.Add("MapElement", (json, dvc, data)=>{
					return new MapElement(json.asString("caption"), json.asString("value"), 
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
					return new RootElement(json.asString("caption"), new RadioGroup(null, selected)) {
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
				return new DateElement(json.asString("caption"), json.dt("value"));
			});
			
			result.Add("TimeElement", (json, dvc, data)=>{
				return new TimeElement(json.asString("caption"), json.dt("value"));
			});
			
			result.Add("DateTimeElement", (json, dvc, data)=>{
				return new DateTimeElement(json.asString("caption"), json.dt("value"));
			});
				
			result.Add("ImageStringElement", (json, dvc, data)=>{
				return  new ImageStringElement(json.asString("caption"), json.asString("value"), 
				        json.asAction(dvc), json.asUIImage("image"), json.asString("imageurl"));
			});
			
			result.Add("WebElement", (json, dvc, data)=>{ 
					var url= data==null ? json.asString("url") : data.CleanString();
					if (string.IsNullOrEmpty(url) || (data!=null && data.GetType()!=typeof(JsonPrimitive)))
					    return null;
					return new WebElement(json.asString("caption"), url);
			});
			
				//{"MultilineElement", (json, dvc, data)=>{ return  new MultilineElement(data==null ? json.s("caption") : data.CleanString() ?? "", null, json.a(dvc));}},
				//{"SimpleImageElement", (json, dvc, data)=>{ return  new SimpleImageElement(
				//	                            UIImage.FromBundle(json.s(UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone? "url" : "urlIpad")));
				//}},
				//{"Template", (json, dvc, data)=>{return  null;}},
			
			return result;
		}
	}
}

