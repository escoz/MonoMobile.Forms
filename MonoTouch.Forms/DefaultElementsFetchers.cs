using System;
using System.Collections.Generic;
using MonoTouch.Dialog;
namespace MonoTouch.Forms
{
	public class DefaultElementsFetchers
	{
		public static Dictionary<Type, Func<Element, string>> Create(){
			var result = new Dictionary<Type, Func<Element, string>>(){
					{typeof(EntryElement), (e)=>{return ((EntryElement)e).Value; } },
					{typeof(BooleanElement), (e)=>{return ((BooleanElement)e).Value ? "1" : "0";}},
					{typeof(RadioElement), (e)=>{return ((RadioElement)e).Value;}},
					//{typeof(RootElement), (e)=>{return ((RootElement)e).RadioSelected.ToString();}},
					{typeof(DateElement), (e)=>{return ((DateElement)e).Value; }},
					{typeof(TimeElement), (e)=>{return ((TimeElement)e).Value; }},
					{typeof(DateTimeElement), (e)=>{return ((DateTimeElement)e).Value; }}				
			};	
				
			return result;
		}
	}
}

