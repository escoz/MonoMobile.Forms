{
	"grouped":true,
	"title":"Some Website",
	
	"root": [
		{ "caption":"Login", "footer":"Please enter your credentials.", "elements": [ 
				{ "id":"login", "type":"EntryElement", "caption":"Username","placeholder":"Username", "keyboard":"EmailAddress" },
				{ "id":"password", "type":"EntryElement", "caption":"Password", "placeholder":"Password", "ispassword":true },
				{ "id":"remember_me", "type":"BooleanElement", "caption":"Remember me", "value":true },
				{ "id":"session_id", "type":"HiddenElement", "value":"mysession" }
			]
		},
		{ "elements":[
				{ "id":"login", "type":"SubmitElement", "caption":"Login", "url":"http://escoz.com/loginPost.js" }
			]
		},
		{ "elements":[
				{ "id":"back", "type":"SubmitElement", "caption":"Post and go back", "url":"http://escoz.com/loginPostAndGoBack.js" }
			]
		},
		{ "elements":[
				{ "id":"error", "type":"SubmitElement", "caption":"Validation Error", "url":"http://escoz.com/loginValidationError.js" }
			]
		}
	]

}