{
	"grouped":true,
	"title":"Samples",
	
	"root": [
		{ "caption":"Simple Elements", "elements":[ 
				{ "id":"login", "type":"StringElement", "caption":"All Elements", "navigateto":"js/allelements.js" },
				{ "id":"login", "type":"StringElement", "caption":"Login Form", "navigateto":"js/login.js" },
				{ "id":"login", "type":"StringElement", "caption":"Login Form Populated", "action":"LoginFormPopulated" }, 
				{ "id":"login", "type":"StringElement", "caption":"Another", "navigateto":"js/another.js" },
				{ "id":"login", "type":"StringElement", "caption":"Countries", "navigateto":"js/countries.js" },
				{ "id":"login", "type":"StringElement", "caption":"Person No Data", "navigateto":"js/personnodata.js" }
			]
		} , 
		{ "caption":"Data Binding", "elements":[ 
				{ "id":"login", "type":"StringElement", "caption":"Person With Data Local", "navigateto":"js/person.js js/person.json" } , 
				{ "id":"login", "type":"StringElement", "caption":"Person With Data Remote", "navigateto":"js/person.js http://escoz.com/samples/person.json" }
			]
		}
	],
	
	
	"leftbaritem":{ "caption":"Details", "id":"details", "action":"ShowPopup"},
	"rightbaritem":{ "caption":"More Info", "id":"reload", "action":"ShowPopup" }

}