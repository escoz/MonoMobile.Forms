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
		{ "elements":[
				{ "id":"login", "type":"MapElement", "caption":"Location", "value":"Sears", "lat":"41.878924", "lng":"-87.635987" },
				{ "type":"ActionElement", "caption":"Shop Popup", "action":"ShowPopup" }
			]
		}
	],
	
	
	"leftbaritem":{ "caption":"Details", "id":"details", "action":"ShowPopup"},
	"rightbaritem":{ "caption":"More Info", "id":"reload", "action":"ShowPopup" }

}