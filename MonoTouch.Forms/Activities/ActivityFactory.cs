
using System;
using System.Collections.Generic;
using System.Reflection;
namespace MonoTouch.Forms.Activities
{
	public static class ActivityFactory {
		private static Dictionary<string, Type> typeCache = new Dictionary<string, Type>();
		
		private static bool tryFindType(string typeName, out Type t) {
		    lock (typeCache) {
		        if (!typeCache.TryGetValue(typeName, out t)) {
		            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
		                t = a.GetType(typeName);
		                if (t != null)
		                    break;
		            }
		            typeCache[typeName] = t;
		        }
		    }
		    return t != null;
		}
	
		public static Activity Create(string name){
			Type t;
			if (tryFindType(name, out t)){
				return Activator.CreateInstance(t) as Activity;
			}
			throw new Exception(string.Format("Activity type not found. Type {0} does not exist in the application", name));
		}
		
	}
}
