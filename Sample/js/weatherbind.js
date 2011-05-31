{
	"grouped":true,
	"title":"Weather",
	"dataroot":"weatherObservation",
	
	"root": [
		{ "caption":"Chicago", "elements":[ 
				{ "type":"StringElement", "caption":"Temperature", "bind":"temperature" },
				{ "type":"StringElement", "caption":"Wind Speed", "bind":"windSpeed" },
				{ "type":"StringElement", "caption":"Humidity", "bind":"humidity" }
			]
		} , 
		{ "elements":[ 
				{ "type":"ButtonElement", "caption":"Update Weather", "action":"Reload" }
			]
		}
	]

}