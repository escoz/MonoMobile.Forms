{
	"grouped":true,
	"title":"Custom Form",
	
	"root": [
		{ "caption":"Other fields", "elements":[
				{ "id":"myInteger", "type":"IntegerElement", "caption":"myInteger", "min":0, "max":50 },
				{ "id":"myEntry", "type":"EntryElement", "caption":"myEntry" },
				{ "id":"myMultiline", "type":"MultilineElement", "caption":"Default value." }
			]
		},
		{ "caption":"Form Data", "elements":[
				{ "type":"StringElement", "caption":"Show with values from web", "navigateto":"js/custom.js http://escoz.com/customvalues.js"},
				{ "type":"ActionElement", "caption":"Show with values", "action":"OpenFormWithValues"},
				{ "type":"ActionElement", "caption":"Reset Form", "action":"ResetForm"}
			]
		},
		{ "caption":"Custom Actions", "elements":[
				{ "type":"ActionElement", "caption":"Execute Simple Action", "action":"ShowTime"},
				{ "type":"ActionElement", "caption":"Show Values", "action":"ShowValues"},
				{ "type":"SubmitElement", "caption":"Submit and Execute Action", "url":"http://escoz.com/loginPostWithAction.js"}
			]
		},
		
		{ "caption":"Radio elements", "elements":[
				{  "type":"RadioElement", "caption":"Comparison", "selected":0, "pop":true, "items":["Previous day", "Previous week", "None"] },				
				{  "type":"RadioElement", "caption":"Default view", "selected":0, "pop":true, "items":["Day", "Yesterday", "Month"] }
			]
		}
		
		
	],
	
	"rightbaritem":{ "caption": "New", "action":"OpenNewForm" }
}