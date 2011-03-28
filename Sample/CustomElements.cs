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
			ElementParsers.Parsers.Add("KeyValueElement", (json, dvc, data)=>{
				var obj = ((JsonObject)data);
				var name = obj.Keys.First();
				return new StringElement(name.ToString(), obj[name].CleanString(), json.asAction(dvc));
			});
		}
	}
}

