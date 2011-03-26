{
	"grouped":true,
	"title":"MonoTouch.Forms",
	
	"root": [
		{ "elements":[
				{ "id":"login", "type":"StringElement", "caption":"Login Form", "navigateto":"js/login.js" },
				{ "id":"login", "type":"StringElement", "caption":"Another", "navigateto":"js/another.js" },
				{ "id":"login", "type":"StringElement", "caption":"Countries", "navigateto":"js/countries.js" },
				{ "id":"login", "type":"StringElement", "caption":"Person Form", "navigateto":"js/person.js js/person.json" }
			]
		},
		{ "elements":[
				{ "id":"login", "type":"MapElement", "caption":"Location", "value":"Sears", "lat":"41.878924", "lng":"-87.635987" },
				{ "type":"ActionElement", "caption":"Shop Popup", "action":"ShowPopup" }
			]
		}
	]

}