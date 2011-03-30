//
// RangeAttribute.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// Original code created by Miguel de Icaza for the MonoTouch.Dialog library available at
// https://github.com/migueldeicaza/MonoTouch.Dialog
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
namespace MonoTouch.Dialog
{
	public class RangeAttribute : Attribute {
		public RangeAttribute (float low, float high)
		{
			Low = low;
			High = high;
		}
		public float Low, High;
		public bool ShowCaption;
	}
}
