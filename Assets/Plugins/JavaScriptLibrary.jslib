mergeInto(LibraryManager.library, {
  JavaScriptAlert: function (str) {
	window.alert(UTF8ToString(str));
  },
  doCode: function(){
	let code = Blockly.JavaScript.workspaceToCode(workspace);
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
});