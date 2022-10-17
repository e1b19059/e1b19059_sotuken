mergeInto(LibraryManager.library, {
  JavaScriptAlert: function (str) {
	window.alert(UTF8ToString(str));
  },

  doCode: function(){
	let code = 'initiate();\n' + Blockly.JavaScript.workspaceToCode(workspace) + 'terminate();\n';
	let myInterpreter = new Interpreter(code, initFunc);
	function stepCode() {
		if (myInterpreter.step()) {
			window.setTimeout(stepCode, 50);
		}
	}
	stepCode();
  },

  setTargetObject: function(obj){
	target_object = UTF8ToString(obj);
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

});