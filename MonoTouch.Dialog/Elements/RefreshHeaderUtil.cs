
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace MonoTouch.Dialog
{
	internal static class RefreshHeaderUtil {
		public static UIImage FromResource (Assembly assembly, string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			assembly = Assembly.GetCallingAssembly ();
			var stream = assembly.GetManifestResourceStream (name);
			if (stream == null)
				return null;
			
			IntPtr buffer = Marshal.AllocHGlobal ((int) stream.Length);
			if (buffer == IntPtr.Zero)
				return null;
			
			var copyBuffer = new byte [Math.Min (1024, (int) stream.Length)];
			int n;
			IntPtr target = buffer;
			while ((n = stream.Read (copyBuffer, 0, copyBuffer.Length)) != 0){
				Marshal.Copy (copyBuffer, 0, target, n);
				target = (IntPtr) ((int) target + n);
			}
			try {
				var data = NSData.FromBytes (buffer, (uint) stream.Length);
				return UIImage.LoadFromData (data);
			} finally {
				Marshal.FreeHGlobal (buffer);
				stream.Dispose ();
			}
		}
	
	}
}
