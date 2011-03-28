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

namespace MonoTouch.Dialog
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