{
	"grouped":true,
	"title":"Some Website",
	
	"root": [
		{ "caption":"Login", "footer":"Please enter your credentials.", "elements": [ 
				{ "id":"login", "type":"EntryElement", "caption":"Username","placeholder":"Username", "keyboard":"EmailAddress", "bind":"#login" },
				{ "id":"password", "type":"EntryElement", "caption":"Password", "placeholder":"Password", "ispassword":true, "bind":"#password"  },
				{ "id":"remember_me", "type":"BooleanElement", "caption":"Remember me", "value":false, "bind":"#remember_me"  },
				{ "id":"session_id", "type":"HiddenElement", "value":"mysession" }
			]
		},
		{ "elements":[
				{ "id":"login", "type":"ActionElement", "caption":"Login", "url":"http://escoz.com/loginPost.js" }
			]
		},
		{ "elements":[
				{ "id":"back", "type":"ActionElement", "caption":"Post and go back", "url":"http://escoz.com/loginPostAndGoBack.js" }
			]
		},
		{ "elements":[
				{ "id":"error", "type":"ActionElement", "caption":"Validation Error", "url":"http://escoz.com/loginValidationError.js" }
			]
		}
	]

}