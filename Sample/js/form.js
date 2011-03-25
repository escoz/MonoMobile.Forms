{

	"grouped":true,
	"title":"JSON Form",
	
	"root": [
		{ "caption":"Elements", "footer":"", "elements": [ 
		
				{ "type":"StringElement", "caption":"This is a string element", "value":"Yeah!" },
				{ "type":"BooleanElement", "caption":"Save Password", "value":true },
				{ "type":"MapElement", "caption":"Some Location", "value":"Floripa", "lat":-27.58333, "lng":-48.56667 },
				{ "type":"WebElement", "caption":"ESCOZ.com", "url":"http://escoz.com" },
				{ "type":"StringElement", "caption":"Open Another file", "navigateto":"js/another.js" },
				{ "type":"StringElement", "caption":"All Countries", "navigateto":"js/countries.js" },
				{ "type":"StringElement", "caption":"Open web file", "navigateto":"http://escoz.com/blog/monotouch.js" },
				{ "type":"RadioElement", "caption":"Select one", "selected":4, "pop":true, 
					"items":["soccer", "football", "baseball", "volleyball", "Formula 1", "rugby" ] 
				},
				{ "type":"DateElement", "caption":"Date:", "value":"12/1/2010" },
				{ "type":"TimeElement", "caption":"Time:", "value":"15:30:00" },
				{ "type":"DateTimeElement", "caption":"Date Time:" }
				
			]
		}
	],
	
	"rightbaritem":{"id":"save", "caption":"Save", "action":"Submit", "url":"http://escoz.com/loginPostAndGoBack.js" }
}
		
		