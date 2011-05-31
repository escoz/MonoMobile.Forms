//
// FormObjectExtensions.cs
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

namespace MonoMobile.Forms
{
	public static class FormObjectExtensions {
	
		public static List<string> asStringList(this JsonObject json, string name){
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
		
		public static string Clean(this string str){
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
				return UIImage.FromBundle(json[name]);
			
			return null;
		}
		
		public static string asString(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return Clean(json[name]);
			
			return null;
		}	
		
		public static JsonObject asJsonObject(this JsonObject json, string name){
			if (json != null && json.ContainsKey(name))
				return (JsonObject)json[name];
			return null;
		}
		
		public static JsonValue asJsonValue(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return json[name];
			
			return null;
		}	
		
		public static Action asAction(this JsonObject json, FormDialogViewController dvc){
			
			if (json.ContainsKey(Constants.Action)) {
				string actionName = json[Constants.Action];
				return ()=>{
					ControllerAction act;
					if (json.ContainsKey(Constants.NavigateTo))
						act = new ControllerAction(actionName, json[Constants.NavigateTo]);
					else
						act = new ControllerAction(actionName);
					dvc.Execute(act, null, ()=>{});
				};
			}
			
			if (json.ContainsKey(Constants.NavigateTo)) {
				string file = json[Constants.NavigateTo];
				return ()=>{
					dvc.NavigateTo(file);
				};
			}
			
			return null;
		}
		
		public static DateTime asDateTime(this JsonObject json, string name){
			if (json!=null && json.ContainsKey(name))
				return DateTime.Parse(json.asString(name));
			
			return DateTime.MinValue;
		}
	}
}
