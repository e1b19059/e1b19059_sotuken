let player_character;
let playerPos;
let playerDir;
let data_json;
let code;
let myInterpreter;
let IsMyturn;
let game_scene = false;

document.addEventListener('click', function (e) {
	if(game_scene){
		if (e.target.id == "unity-canvas") {
			// Clicked on canvas 
			unityInstance.SendMessage("PhotonLogin", "FocusCanvas", "1");
		} else {
			// Clicked outside of canvas 
			unityInstance.SendMessage("PhotonLogin", "FocusCanvas", "0");
		}
	}
});

const initFunc = function (interpreter, scope) {
	let wrapper = function(id) {
		id = id ? id.toString() : '';
		return highlightBlock(id);
	};
	let move_left_wrapper = function () {
		return move_left_function();
	};
	let move_right_wrapper = function () {
		return move_right_function();
	};
	let move_forward_wrapper = function () {
		return move_forward_function();
	};
	let move_back_wrapper = function () {
		return move_back_function();
	};
	let turn_left_wrapper = function () {
		return turn_left_function();
	};
	let turn_right_wrapper = function () {
		return turn_right_function();
	};
	let check_wrapper = function (direction, targetObj) {
		return check_point_function(direction, targetObj);
	};
	let put_object_wrapper = function (direction, object) {
		return put_object_function(direction, object);
	};
	let destroy_wrapper = function (direction) {
		return destroy_obstacle_function(direction);
	};
	let pick_bomb_wrapper = function (direction) {
		return pick_bomb_function(direction);
	};
	let initiate_wrapper = function () {
		return initiate();
	};
	let terminate_wrapper = function () {
		return terminate();
	};
	interpreter.setProperty(scope, 'highlightBlock', interpreter.createNativeFunction(wrapper));
	interpreter.setProperty(scope, 'move_left', interpreter.createNativeFunction(move_left_wrapper));
	interpreter.setProperty(scope, 'move_right', interpreter.createNativeFunction(move_right_wrapper));
	interpreter.setProperty(scope, 'move_forward', interpreter.createNativeFunction(move_forward_wrapper));
	interpreter.setProperty(scope, 'move_back', interpreter.createNativeFunction(move_back_wrapper));
	interpreter.setProperty(scope, 'turn_left', interpreter.createNativeFunction(turn_left_wrapper));
	interpreter.setProperty(scope, 'turn_right', interpreter.createNativeFunction(turn_right_wrapper));
	interpreter.setProperty(scope, 'check_point', interpreter.createNativeFunction(check_wrapper));
	interpreter.setProperty(scope, 'put_object', interpreter.createNativeFunction(put_object_wrapper));
	interpreter.setProperty(scope, 'destroy_obstacle', interpreter.createNativeFunction(destroy_wrapper));
	interpreter.setProperty(scope, 'pick_bomb', interpreter.createNativeFunction(pick_bomb_wrapper));
	interpreter.setProperty(scope, 'initiate', interpreter.createNativeFunction(initiate_wrapper));
	interpreter.setProperty(scope, 'terminate', interpreter.createNativeFunction(terminate_wrapper));
}

function highlightBlock(id) {
	if(IsMyturn){
		workspace_readOnly.highlightBlock(id);
	}else{
		workspace_rival.highlightBlock(id);
	}
}

function generateCodeAndLoadIntoInterpreter() {
	Blockly.JavaScript.STATEMENT_PREFIX = 'highlightBlock(%1);\n';
	Blockly.JavaScript.addReservedWords('highlightBlock');
}
generateCodeAndLoadIntoInterpreter();

function move_left_function() {
	unityInstance.SendMessage(player_character, "MoveLeft");
}

function move_right_function() {
	unityInstance.SendMessage(player_character, "MoveRight");
}

function move_forward_function() {
	unityInstance.SendMessage(player_character, "MoveForward");
}

function move_back_function() {
	unityInstance.SendMessage(player_character, "MoveBack");
}

function turn_left_function() {
	unityInstance.SendMessage(player_character, "TurnLeft");
}

function turn_right_function() {
	unityInstance.SendMessage(player_character, "TurnRight");
}

function check_point_function(direction, targetObj){		
	let check_point;
	switch(direction){
		case 'left':
			check_point = {x: -playerDir.z + playerPos.x, z: playerDir.x + playerPos.z};
			break;
		case 'right':
			check_point = {x: playerDir.z + playerPos.x, z: -playerDir.x + playerPos.z};
			break;
		case 'forward':
			check_point = {x: playerDir.x + playerPos.x, z: playerDir.z + playerPos.z};
			break;
		case 'back':
			check_point = {x: -playerDir.x + playerPos.x, z: -playerDir.z + playerPos.z};
			break;
	}
	if(targetObj == 'anything'){
		return data_json.objectData.find(object => object.position.x == check_point.x && object.position.z == check_point.z);
	}else if(targetObj == 'empty'){
		return !data_json.objectData.find(object => object.position.x == check_point.x && object.position.z == check_point.z);
	}else{
		return data_json.objectData.find(object => object.position.x == check_point.x && object.position.z == check_point.z && object.name.includes(targetObj));
	}
}

function put_object_function(direction, object){
	if(object.includes("Obstacle")){
		unityInstance.SendMessage(player_character, "PutObstacle", direction);
	}else if(object.includes("Bomb")){
		unityInstance.SendMessage(player_character, "PutBomb", direction);
	}
}

function destroy_obstacle_function(direction){
	if(direction != null)unityInstance.SendMessage(player_character, "DestroyObstacle", direction);
}

function pick_bomb_function(direction){
	if(direction != null)unityInstance.SendMessage(player_character, "PickBomb", direction);
}

function initiate(){
	unityInstance.SendMessage(player_character, "Init");
}

function terminate(){
	highlightBlock(false);
	unityInstance.SendMessage(player_character, "Termi");
}
