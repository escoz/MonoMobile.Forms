{
	"grouped":true,
	"title":"Weather",
	
	"root": [
		{ "caption":"Chicago", "elements":[ 
				{ "type":"StringElement", "caption":"Temperature", "bind":"#temperature" },
				{ "type":"StringElement", "caption":"Wind Speed", "bind":"#windspeed" },
				{ "type":"StringElement", "caption":"Humidity", "bind":"#humidity" }
			]
		} , 
		{ "elements":[ 
				{ "type":"ActivityElement", "caption":"Update Weather", "activity":"Sample.Activities.ShowWeatherInChicago" }
			]
		}
	]

}