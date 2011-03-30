//
// ImageStore.cs
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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch;

namespace MonoTouch.Forms.Elements
{
	public interface IImageUpdated {
		void UpdatedImage (string url, UIImage image);
	}
	
	public class SimpleImageStore {
		
		public static SimpleImageStore Current = new SimpleImageStore();
		
		private SimpleImageStore(){}
		
		static LRUCache<string,UIImage> cache = new LRUCache<string, UIImage>(200);
		static Dictionary<string, List<IImageUpdated>> pendingRequests = new Dictionary<string, List<IImageUpdated>>();
		
		public bool RequestImage(string url, IImageUpdated receiver) {
			
			var local = RequestLocalImage(url);
			
			if (local != null) {
				receiver.UpdatedImage(url, local);
				return true;
			}
			
			if (pendingRequests.ContainsKey(url)){
				pendingRequests[url].Add(receiver);
			} else {
				pendingRequests.Add(url, new List<IImageUpdated>(){receiver});
			}
			NSUrlRequest req = new NSUrlRequest(new NSUrl(url), NSUrlRequestCachePolicy.ReturnCacheDataElseLoad, 10);
			new UrlConnection("img"+url, req, (UIImage img)=>{
				var surl = url;
				cache[surl] = img;
				var imgreq = pendingRequests[surl];
				foreach (var v in imgreq)
					v.UpdatedImage(surl, img);
				pendingRequests.Remove(surl);
				
			});
			return false;
			
		}
		public UIImage RequestLocalImage(string url) {
			
			if (cache[url]!=null){
				
				if (cache[url] != null)
					return cache[url];
			}
			
			return null;
		}
	}
}