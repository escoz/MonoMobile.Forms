{
	"grouped":true,
	"title":"Person",
	
	"root": [
		{ "title":"Details", "elements":[
				{ "type":"StringElement", "caption":"Name", "bind":"firstName" },
				{ "type":"StringElement", "caption":"Last Name", "bind":"lastName" },
				{ "type":"StringElement", "caption":"Age", "bind":"age" }
			]
		},
		{ "iterate":"phoneNumbers", "template":
			{ "type":"StringElement", "caption":"Fone", "bind":"number" }
		}
	]

}