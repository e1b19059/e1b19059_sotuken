mergeInto(LibraryManager.library, {
	JavaScriptAlert: function (str) {
		window.alert(UTF8ToString(str));
	},

	doCode: function(turn){
		IsMyturn = turn;
		if(IsMyturn){
			code = 'initiate();\n' + Blockly.JavaScript.workspaceToCode(workspace_readOnly) + 'terminate();\n';
		}else{
			code = 'initiate();\n' + Blockly.JavaScript.workspaceToCode(workspace_rival) + 'terminate();\n';
		}
		myInterpreter = new Interpreter(code, initFunc);
		function stepCode() {
			if (myInterpreter.step()) {
				window.setTimeout(stepCode, 50);
			}
		}
		stepCode();
	},

	setPlayerCharacter: function(obj){
		player_character = UTF8ToString(obj);
	},

	setPlayerPos: function(x, z){
		playerPos = {x: x, z: z};
	},

	setPlayerDir: function(x, z){
		playerDir = {x: x, z: z};
	},

	setData: function(data){
		data_json = JSON.parse(UTF8ToString(data));
	},

	getBlockFromWorkspace: function(){
		let xml = Blockly.Xml.workspaceToDom(workspace);
		let ret = Blockly.Xml.domToText(xml);
		let size = lengthBytesUTF8(ret) + 1;
		let ptr = _malloc(size);
		stringToUTF8(ret, ptr, size);
		return ptr;
	},

	setBlockToWorkspace: function(xmlText){
		let xml = Blockly.Xml.textToDom(UTF8ToString(xmlText));
		workspace.clear();
		Blockly.Xml.domToWorkspace(xml, workspace);
	},

	setFriendBlock: function(xmlText){
		let xml = Blockly.Xml.textToDom(UTF8ToString(xmlText));
		workspace_readOnly.clear();
		Blockly.Xml.domToWorkspace(xml, workspace_readOnly);
	},

	setRivalBlock: function(xmlText){
		let xml = Blockly.Xml.textToDom(UTF8ToString(xmlText));
		workspace_rival.clear();
		Blockly.Xml.domToWorkspace(xml, workspace_rival);
	},

	switchEditable: function(){
		let elm = document.getElementById('readOnlyParent');
		elm.className = 'bottom';
	},

	switchReadOnly: function(){
		let elm = document.getElementById('readOnlyParent');
		elm.className = 'top';
	},

	replaceBlock: function(){
		workspace_readOnly.clear();
		Blockly.Xml.domToWorkspace(Blockly.Xml.workspaceToDom(workspace), workspace_readOnly);
	},

	clearRival: function(){
		workspace_rival.clear();
	},

	initWorkspace: function(){
		workspace.clear();
		workspace_readOnly.clear();
		workspace_rival.clear();
	},

	leaveAlert: function(){
		alert('Master Client disconnected!');
	}

});