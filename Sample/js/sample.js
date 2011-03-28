{
	"grouped":true,
	"title":"Samples",
	
	"root": [
		{ "elements":[
				{ "id":"login", "type":"StringElement", "caption":"Login Form", "navigateto":"js/login.js" },
				{ "id":"login", "type":"StringElement", "caption":"Another", "navigateto":"js/another.js" },
				{ "id":"login", "type":"StringElement", "caption":"Countries", "navigateto":"js/countries.js" },
				{ "id":"login", "type":"StringElement", "caption":"Person With Data", "navigateto":"js/person.js js/person.json" },
				{ "id":"login", "type":"StringElement", "caption":"Person No Data", "navigateto":"js/personnodata.js" }
			]
		},
		{ "caption":"All Element Types - no data", "elements":[
		
				{ "type":"StringElement", "caption":"StringElement", "value":"Some Value" },
				{ "type":"EntryElement", "caption":"EntryElement", "value":"Some Value" },
				{ "type":"BooleanElement", "caption":"BooleanElement", "value":true },
				{ "type":"FloatElement", "caption":"FloatElement", "value":0.33 },
				{ "type":"CheckboxElement", "caption":"CheckboxElement", "value":true },
				{ "type":"DateElement", "caption":"DateElement", "value":"4/10/2011" },
				{ "type":"TimeElement", "caption":"TimeElement", "value":"22:30" },
				{ "type":"DateTimeElement", "caption":"DateTimeElement", "value":"4/10/2011 22:40" },
				{ "type":"RadioElement", "caption":"RadioElement", "pop":true, "selected":2, "items":["item 0", "item 1", "item 2", "item 3", "item 4"] },
				{ "type":"MapElement", "caption":"MapElement", "value":"Sears", "lat":"41.878924", "lng":"-87.635987" },
				{ "type":"WebElement", "caption":"WebElement", "url":"http://google.com"},
				{ "type":"ActionElement", "caption":"ActionElement", "action":"ShowPopup" },
				{ "type":"ImagePickerElement", "caption":"ImagePickerElement" },
				{ "type":"ImageStringElement", "caption":"ImageStringElement", "image":"img", "imageurl":"http://lalawag.s3.amazonaws.com/wp-content/uploads/iphone-settings-icon.jpg" },
				{ "type":"MultilineElement", "caption":"MultilineElement is a type of element which is a type of object", "value":"Some Value" }
			]
		}
	],
	
	
	"leftbaritem":{ "caption":"Details", "id":"details", "action":"ShowPopup"},
	"rightbaritem":{ "caption":"More Info", "id":"reload", "action":"ShowPopup" }

}