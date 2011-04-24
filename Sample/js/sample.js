{
	"grouped":true,
	"title":"Samples",
	
	"root": [
		{ "caption":"Simple Elements", "elements":[ 
				{ "type":"StringElement", "caption":"Login Form", "navigateto":"js/login.js" },
				{ "type":"StringElement", "caption":"Login Form Populated", "action":"LoginFormPopulated" },
				{ "type":"StringElement", "caption":"All Elements", "navigateto":"js/allelements.js" }, 
				{ "type":"StringElement", "caption":"Another", "navigateto":"js/another.js" },
				{ "type":"StringElement", "caption":"Countries", "navigateto":"js/countries.js" },
				{ "type":"StringElement", "caption":"Weather With Activity", "navigateto":"js/weather.js" },
				{ "type":"StringElement", "caption":"Weather Binding", "navigateto":"js/weatherbind.js http://ws.geonames.org/weatherIcaoJSON?ICAO=KORD" }
			]
		} , 
		{ "caption":"Data Binding", "elements":[ 
				{ "type":"StringElement", "caption":"Person No Data", "navigateto":"js/personnodata.js" } ,
				{ "type":"StringElement", "caption":"Person With Data Local", "navigateto":"js/person.js js/person.json" } , 
				{ "type":"StringElement", "caption":"Person With Data Remote", "navigateto":"js/person.js http://escoz.com/samples/person.json" },
				{ "type":"StringElement", "caption":"Person With Form Remote", "navigateto":"http://escoz.com/samples/person.js http://escoz.com/samples/person.json" }
			]
		}
	],
	
	"leftbaritem":{ "caption":"Left", "id":"details", "action":"ShowPopup"},
	"rightbaritem":{ "caption":"Right", "id":"reload", "action":"ShowPopup"}

}