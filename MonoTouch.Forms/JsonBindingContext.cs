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
				var sec = new JsonSectionBuilder(this._controller).Build(section, data);
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
				if (!string.IsNullOrEmpty(el.ID) && ElementFetchers.Fetchers.ContainsKey(el.GetType())){
					result.Add(el.ID, ElementFetchers.Fetchers[el.GetType()](el));
				}
			}
			
			return result;
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
		
	}
}
	