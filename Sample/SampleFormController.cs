using System;
using MonoTouch.Forms;
namespace Sample
{
	public class SampleFormController : JsonDialogViewController
	{
		public SampleFormController () : base("js/sample.js", false)
		{
		}
	}
}

