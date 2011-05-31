Json Forms for MonoTouch
========================

MonoMobile.Forms allows you to easily create iOS dialogs using JSON and C#. MonoTouch.Forms also simplifies
the display of data from JSON REST services, by directly binding values in JSON to fields in forms.

This library is not intended to replace C#, MonoTouch, or the Apple SDK. Instead, it decreases
the amount of boilerplate code necessary to display forms of data, letting you focus on real
business logic for your apps.

If you would like to see an application that uses this library in action, download the app GitHubby from the
AppStore. The entire application is written using this library plus a very small portion of C# code. I 
also develop a few other apps using this library, like [Quicklytics](http://itunes.apple.com/us/app/quicklytics-google-analytics/id354890919?mt=8)

I really hope you enjoy this project, and that you helps you develop apps.

Eduardo Scoz

How It Works?
========================

At the heart of MonoMobile.Forms, is the FormDialogViewController class. This is the main UIController class that
you'll be using when creating forms with this control. When defining a new form, we'll need to send the address of
at least one JSON file, which contains the definition of the form to be created. This JSON form follows a strict format
which is defined below; basically, it defines sections for the form, which themselves contains elements. The JSON
file can be either on the device, or on the internet; the controller will automatically download the file if necessary.

These elements are what form each field of the form. MonoMobile.Forms comes with a variety of them, and you can use them
right away from your JSON file. Those include, for example, entry elements, button elements, radio elements, etc. When 
the application runs, the framework automatically parses the JSON form file, and creates all the elements. When
the form appears on the screen, a cell is created for the elements as necessary.

During the creation of the FormDialogViewController, you can also define a second JSON file, which contains simple JSON data.
If you define such a file, the controller will automatically bind the elements in the first file to the values for the forms
in the second one. You can also do things like iterate over arrays and property of those elements automatically.

Finally, its time to read the data back from the form. The FormDialogViewController contains a method GetAllValues() that
returns a Dictionary with all the values by key. Values are always returned as strings, so its easy to
send them to over to the internet. 

For your app, you may need to do more than the default elements do, though. This framework makes it very easy to create your
own custom elements, as well as register them with the ElementParsers class so that those elements get created easily. 
You also might want to register the element with the ElementFetcher class, so that you can extract the value from the element
once editing is done.

This is it really. The system is not very complex; the idea is to make it easy to extend so that you can adapt the framework
to each of your applications.

Using the FormDialogViewController
========================

The recommended way to use the FormDialogViewController is to create a subclass of it on your project. Here's an example 
(make sure you add references to the MonoMobile.Forms and MonoTouch.Dialog projects from your project):

namespace MyApp
{
	public class MyFormController : FormDialogViewController
	{
		public MyFormController (string url) : base(url, true)
		{
		}
		
		public void ShowLogin(Element el){
			var loginForm = new MyFormController("js/login.js");
			this.PresentModalViewController(new UINavigationController(loginForm), true);
		}
		
		public void Dismiss(Element el){
			this.DismissModalViewControllerAnimated(true);
		}
	}
}

In the class above, we're defining what will be the basis for our application. To use this class, you can simply do 
the following on your AppDelegate class:

public override bool FinishedLaunching (UIApplication app, NSDictionary options)
	{		
		window.AddSubview (new UINavigationController(new MyFormController("js/main.js")).View);
		window.MakeKeyAndVisible ();
		return true;
	}
}

This will cause the application to start up with a new MyFormController, displaying the file called js/main.js. In 
that file, we can define the form to be displayed, like the following:

{
	"title":"About",
	
	"root": [
		{ "caption":"Hello", "elements":[
				{  "type":"StringElement", "caption":"Hello World", "value":"hi!" }
			]
		},

		{ "caption":"Visit the website", "elements":[
				{  "type":"WebElement", "caption":"MonoMobile.Forms Website", "url":"http://github.com/escoz/monomobile.forms" }
			]
		}
	]
}

In the example above, we're defining one table with two different sections; the first section (with caption "Hello") will
simply display an element with the caption and values defined, while the second one will display an element that when
clicked, will show a website. The title for the page is also defined. Pretty simple, huh? There's a lot more functionality
in the framework, though, so keep reading!

Be careful when defining the form: the file has to contain only correctly defined JSON data. It's easy to 
mistakenly add an extra comma at the end of an array, or forgetting to add quotes for titles. 
The MonoMobile.Forms framework will throw errors in that case.

Controller Actions
==================
In the first example above, where we defined the MyFormController class, you saw that there were two methods being defined, each
one accepting an Element object. Those are called Actions. Actions are a simple way of executing methods directly on your controller.
So lets define a new section, with a button that calls the ShowLogin() action. To do that, simply add this to the root
array in your main.js file:

{ "elements":[
		{ "type":"ButtonElement", "caption":"Login", "action":"ShowLogin" }
	]
}

Wow, that was easy! When you touch the Button element, the ShowLogin action automatically gets called from the framework. In our case,
we'll be showing another form called js/login.js as a modal page. Actions can also be called from many other elements, like StringElements.

Navigation
==================
The JsonFormDialogController uses the UINavigationController model available in iOS. While you could create custom methods in your
controller to do the push of a new Form for you (and this works great when you have to execute custom logic before doing the push), 
if all you want is to display more data in a new page, you can use the "navigateto" directive, like below:

{ "elements":[
		{ "type":"ButtonElement", "caption":"Detailed Info", "navigateto":"js/details.js" }
	]
}

This will automatically cause the new form to be pushed when you press the button.



