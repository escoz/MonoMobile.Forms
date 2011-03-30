//
// Constants.cs
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
using MonoTouch.Forms.Activities;
using MonoTouch.Foundation;
using MonoTouch;
using System.Json;
using MonoTouch.Forms;
namespace Sample.Activities
{
	
	[Preserve(AllMembers=true)]
	public class ShowWeatherInChicago : Activity
	{
		public override void Execute (MonoTouch.Forms.ActivityElement element, MonoTouch.Forms.FormDialogViewController controller, Action completed)
		{
			controller.SetValue("temperature", "");
			controller.SetValue("humidity", "");
			controller.SetValue("windspeed", "");
			
			var request = new NSMutableUrlRequest(new NSUrl("http://ws.geonames.org/weatherIcaoJSON?ICAO=KORD"), NSUrlRequestCachePolicy.ReloadIgnoringLocalAndRemoteCacheData, 10);
			
			new UrlConnection("weather", request, (result)=>{
				var json = JsonObject.Parse(result);
				var weather = json["weatherObservation"];
				
				controller.SetValue("temperature", weather["temperature"].CleanString()+ " celsius");
				controller.SetValue("humidity", weather["humidity"].CleanString() + "%");
				controller.SetValue("windspeed", weather["windSpeed"].CleanString()+" km/h");
				
				controller.Reload();
				completed();
				
			}, (error)=>{
				controller.NetworkFailed(error);
				completed();
			});
		}
		
	}
}

