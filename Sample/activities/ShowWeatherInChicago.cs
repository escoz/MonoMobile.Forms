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
		public override void Execute (MonoTouch.Forms.ActivityElement element, MonoTouch.Forms.JsonDialogViewController controller, Action completed)
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

