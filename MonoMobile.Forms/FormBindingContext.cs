//
// FormBindingContext.cs
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
using System.Linq;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;

namespace MonoMobile.Forms
{
	public partial class FormBindingContext : IDisposable {
		public RootElement Root;
		Dictionary<string, Element> _elements;
		public FormDialogViewController _controller;

		public FormBindingContext (FormDialogViewController controller, JsonValue json, string title)
		{
			_controller = controller;
			_elements = new Dictionary<string, Element>();
			
			if (json == null)
				throw new ArgumentNullException ("json");
			
			Root = new RootElement(title);
			
			try {
				Populate (json, Root, null);
			} catch (Exception e) {
				Console.WriteLine("Exception on JsonBindingContext " + e.ToString());
			}
		}
		
		public FormBindingContext(FormDialogViewController callbacks, JsonValue json, JsonValue data, string title) {
			_controller = callbacks;
			_elements = new Dictionary<string, Element>();
			
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
			if (json.ContainsKey(Constants.Title))
				root.Caption = json[Constants.Title];
			
			if (json.ContainsKey(Constants.DataRoot)){
				var dataroot = json[Constants.DataRoot].CleanString();
				data = data[dataroot];
			}			
			
			JsonValue jsonRoot = null;
			try {
				jsonRoot = json[Constants.Root];
			} catch (Exception){
				Console.WriteLine("Bad JSON: could not find the root element - "+json.ToString()) ;	
				return;
			}
			
			foreach (JsonObject section in jsonRoot){
				var sec = new FormSectionBuilder(this._controller).Build(section, data);
				foreach (var el in sec.Elements) {
					if (!string.IsNullOrEmpty(el.ID) && !_elements.ContainsKey(el.ID)) _elements.Add(el.ID, el);
				}
				root.Add(sec);
			}
		}
		
		public Element GetElement(string id){
			Element element;
			_elements.TryGetValue(id, out element);	
			return element;
		}
		
		public Dictionary<string, string> Fetch()
		{
			var result = new Dictionary<string, string>();
			foreach (var el in _elements.Values){
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
				foreach (var element in _elements.Values){
					element.Dispose ();
				}
			}
		}
		
	}
}
	