
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Json;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;
namespace MonoTouch.Forms
{
	public static class JsonObjectExtensions {
	
		public static List<string> sl(this JsonObject json, string name){
			var result = new List<string>();
			if (json!=null && json.ContainsKey(name)) {
				foreach (var e in json[name]){
					result.Add(e.ToString().Clean()); // TODO: is this a bug with JsonObject?
				}
			}
			return result;
		}
		
		public static string CleanString(this JsonObject json){
			return json.ToString().Clean();
		}
		
		private static string Clean(this string str){
			if (str==null)
				return "";
			if (str.Length>2)
				return (str[0]=='\"' ? str.Substring(1, str.Length-2) : str);
			else
				return (str=="\"\"" ? "" : str);
		}
		
		public static string CleanString(this JsonValue json){
			var e = json.ToString();
			return e.Clean();
		}
			
		public static bool asBoolean(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return json[name];
			
			return false;
		}
		
		public static Nullable<double> asDouble(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return json[name];
			
			return null;
		}
		
		public static UIImage asUIImage(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return UIImage.FromBundle(json["image"]);
			
			return null;
		}
		
		public static string asString(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return Clean(json[name]);
			
			return null;
		}	
		
		public static JsonValue asJsonValue(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return json[name];
			
			return null;
		}	
		
		public static Action asAction(this JsonObject json, JsonDialogViewController dvc){
			if (json!=null && json.ContainsKey("navigateto") && json.ContainsKey("action")) {
				string file = json["navigateto"];
				string action = json["action"];
				return ()=>{
					dvc.InvokeAction(action, new object[]{file});
				};
			}
			
			if (json.ContainsKey("navigateto")) {
				string file = json["navigateto"];
				return ()=>{
					dvc.NavigateTo(file);
				};
			}
			
			
			if (json.ContainsKey("action")) {
				return ()=>{
					dvc.InvokeAction(json["action"], new Element("adf"));
				};
			}
			
			return null;
			
		}
		
		public static string GravatarUrl(this JsonObject json, string name ){
			//&d=https%3A%2F%2Fgithub.com%2Fimages%2Fgravatars%2Fgravatar-140.png
			return string.Format("https://secure.gravatar.com/avatar/{0}?s=140", json.asString(name));
		}
		
		public static DateTime dt(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return DateTime.Parse(json.asString(name));//json[name];
			
			return DateTime.MinValue;
		}
	}
}
