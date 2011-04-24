{
	"grouped":true,
	"title":"Some Website",
	
	"root": [
		{ "caption":"Login", "footer":"Please enter your credentials.", "elements": [ 
				{ "id":"login", "type":"EntryElement", "caption":"Username","placeholder":"Username", "keyboard":"EmailAddress", "bind":"#login" },
				{ "id":"password", "type":"EntryElement", "caption":"Password", "placeholder":"Password", "ispassword":true, "bind":"#password"  },
				{ "id":"remember_me", "type":"BooleanElement", "caption":"Remember me", "value":false, "bind":"#remember_me"  },
				{ "id":"session_id", "type":"HiddenElement", "value":"sessionid_12345" }
			]
		},
		{ "elements":[
				{ "id":"login", "type":"ButtonElement", "caption":"Login", "action":"ShowPopup" },
				{ "id":"login", "type":"ActivityElement", "caption":"Login Activity", "activity":"Sample.Activities.ShowValuesInConsole" }
			]
		}
	]

}