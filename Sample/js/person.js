{
	"grouped":true,
	"title":"Person",
	
	"root": [
		{ "caption":"Details", "elements":[
				{ "type":"StringElement", "caption":"Name", "bind":"firstName" },
				{ "type":"StringElement", "caption":"Last Name", "bind":"lastName" },
				{ "type":"StringElement", "caption":"Age", "bind":"age" }
			]
		},
		{ "caption":"Address", "iterateproperties":"address", "template":
			{ "type":"KeyValueElement" }
		},
		{ "captions":"Contact", "iterate":"phoneNumbers", "template":
			{ "type":"StringElement", "caption":"Fone", "bind":"number" },
			"footer":"Please do not call these numbers."
		}
	]

}