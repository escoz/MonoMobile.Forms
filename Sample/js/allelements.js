{
	"grouped":true,
	"title":"All Elements",
	
	"root": [
		
		{ "caption":"Sample of all elements", "elements":[
				{ "type":"StringElement", "caption":"StringElement", "value":"Some Value" },
				{ "type":"EntryElement", "caption":"EntryElement", "value":"Some Value" },
				{ "type":"BooleanElement", "caption":"BooleanElement", "value":true },
				{ "type":"FloatElement", "caption":"FloatElement", "value":0.33 },
				{ "type":"FloatElement", "value":80, "minvalue":0, "maxvalue":100 },
				{ "type":"CheckboxElement", "caption":"CheckboxElement", "value":true },
				{ "type":"DateElement", "caption":"DateElement", "value":"4/10/2011" },
				{ "type":"TimeElement", "caption":"TimeElement", "value":"22:30" },
				{ "type":"DateTimeElement", "caption":"DateTimeElement", "value":"4/10/2011 22:40" },
				{ "type":"RadioElement", "caption":"RadioElement", "pop":true, "selected":2, "items":["item 0", "item 1", "item 2", "item 3", "item 4"] },
				{ "type":"MapElement", "caption":"MapElement", "value":"Sears", "lat":"41.878924", "lng":"-87.635987" },
				{ "type":"WebElement", "caption":"WebElement", "url":"http://google.com"},
				{ "type":"ImagePickerElement", "caption":"ImagePickerElement" },
				{ "type":"ImageStringElement", "caption":"ImageStringElement", "image":"", "imageurl":"http://a3.mzstatic.com/us/r1000/052/Purple/31/db/c2/mzl.bshvyykk.100x100-75.jpg" },
				{ "type":"MultilineElement", "caption":"MultilineElement is a type of element which is a type of object", "value":"Some Value" }
			]
		}, 
		{ "elements":[
				{ "type":"ButtonElement", "caption":"ButtonElement", "action":"ShowPopup" }
			]
		}
				
	]
}