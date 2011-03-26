using System;
using MonoTouch.Forms;
using MonoTouch.Dialog;
using System.Json;
using System.Linq;

namespace Sample
{
	public static class CustomElements
	{
		public static void RegisterAll()
		{
			JsonBindingContext.RegisterParser("KeyValueElement", (json, dvc, data)=>{
				var obj = ((JsonObject)data);
				var name = obj.Keys.First();
				return new StringElement(name.ToString(), obj[name].CleanString() ){
					Command = new ActionCommand(json.asAction(dvc))
				};
			});
		}
	}
}

