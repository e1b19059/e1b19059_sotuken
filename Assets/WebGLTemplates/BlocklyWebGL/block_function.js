document.addEventListener('click', function (e) {
	if (e.target.id == "unity-canvas") {
		// Clicked on canvas 
		unityInstance.SendMessage("PhotonLogin", "FocusCanvas", "1");
	} else {
		// Clicked outside of canvas 
		unityInstance.SendMessage("PhotonLogin", "FocusCanvas", "0");
	}
});

const initFunc = function (interpreter, scope) {
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
	let put_obstacle_wrapper = function (direction) {
		return put_obstacle_function(direction);
	};
	let destroy_wrapper = function (direction) {
		return destroy_obstacle_function(direction);
	};
	let initiate_wrapper = function () {
		return initiate();
	};
	let terminate_wrapper = function () {
		return terminate();
	};
	interpreter.setProperty(scope, 'move_left', interpreter.createNativeFunction(move_left_wrapper));
	interpreter.setProperty(scope, 'move_right', interpreter.createNativeFunction(move_right_wrapper));
	interpreter.setProperty(scope, 'move_forward', interpreter.createNativeFunction(move_forward_wrapper));
	interpreter.setProperty(scope, 'move_back', interpreter.createNativeFunction(move_back_wrapper));
	interpreter.setProperty(scope, 'turn_left', interpreter.createNativeFunction(turn_left_wrapper));
	interpreter.setProperty(scope, 'turn_right', interpreter.createNativeFunction(turn_right_wrapper));
	interpreter.setProperty(scope, 'check_point', interpreter.createNativeFunction(check_wrapper));
	interpreter.setProperty(scope, 'put_obstacle', interpreter.createNativeFunction(put_obstacle_wrapper));
	interpreter.setProperty(scope, 'destroy_obstacle', interpreter.createNativeFunction(destroy_wrapper));
	interpreter.setProperty(scope, 'initiate', interpreter.createNativeFunction(initiate_wrapper));
	interpreter.setProperty(scope, 'terminate', interpreter.createNativeFunction(terminate_wrapper));
}

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
	let player = data_json.objectData.find(object => object.name == player_character);
	switch(direction){
		case 'left':
			check_point = {x: -player.direction.z + player.position.x, z: player.direction.x + player.position.z};
			break;
		case 'right':
			check_point = {x: player.direction.z + player.position.x, z: -player.direction.x + player.position.z};
			break;
		case 'forward':
			check_point = {x: player.direction.x + player.position.x, z: player.direction.z + player.position.z};
			break;
		case 'back':
			check_point = {x: -player.direction.x + player.position.x, z: -player.direction.z + player.position.z};
			break;
	}
	return data_json.objectData.find(object => object.position.x == check_point.x && object.position.z == check_point.z && object.name == targetObj);
}

function put_obstacle_function(direction){
	let direction_number;
	switch(direction){
		case 'left':
			direction_number = 0;
			break;
		case 'right':
			direction_number = 1;
			break;
		case 'forward':
			direction_number = 2;
			break;
		case 'back':
			direction_number = 3;
			break;
	}
	unityInstance.SendMessage("PhotonLogin", "PutObstacle", direction_number);
}

function destroy_obstacle_function(direction){
	let direction_number;
	switch(direction){
		case 'left':
			direction_number = 0;
			break;
		case 'right':
			direction_number = 1;
			break;
		case 'forward':
			direction_number = 2;
			break;
		case 'back':
			direction_number = 3;
			break;
	}
	unityInstance.SendMessage("PhotonLogin", "DestroyObstacle", direction_number);
}

function initiate(){
	unityInstance.SendMessage(player_character, "Init");
	unityInstance.SendMessage("PhotonLogin", "StartUpdate");
}

function terminate(){
	unityInstance.SendMessage(player_character, "Termi");
	unityInstance.SendMessage("PhotonLogin", "StopUpdate");
	document.getElementById('readOnlyParent').className = 'bottom';
}
