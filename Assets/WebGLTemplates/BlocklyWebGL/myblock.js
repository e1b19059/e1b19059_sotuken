const move_left = {
    "type": "move_left",
    "message0": "左に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 195,
    "tooltip": "",
    "helpUrl": ""
};

const move_right = {
    "type": "move_right",
    "message0": "右に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 195,
    "tooltip": "",
    "helpUrl": ""
};

const move_forward = {
    "type": "move_forward",
    "message0": "前に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 195,
    "tooltip": "",
    "helpUrl": ""
};

const move_back = {
    "type": "move_back",
    "message0": "後ろに下がる",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 195,
    "tooltip": "",
    "helpUrl": ""
};

const turn_left = {
    "type": "turn_left",
    "message0": "90度左回転",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 195,
    "tooltip": "",
    "helpUrl": ""
};

const turn_right = {
    "type": "turn_right",
    "message0": "90度右回転",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 195,
    "tooltip": "",
    "helpUrl": ""
};

const get_left = {
    "type": "get_left",
    "message0": "左",
    "inputsInline": true,
    "output": "direction",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const get_right = {
    "type": "get_right",
    "message0": "右",
    "inputsInline": true,
    "output": "direction",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const get_forward = {
    "type": "get_forward",
    "message0": "前",
    "inputsInline": true,
    "output": "direction",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const get_back = {
    "type": "get_back",
    "message0": "後ろ",
    "inputsInline": true,
    "output": "direction",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const check_point = {
    "type": "check_point",
    "message0": "%1 方向が %2 である",
    "args0": [
        {
            "type": "input_value",
            "name": "direction",
            "check": "direction",
        },
        {
            "type": "input_value",
            "name": "object",
            "check": [
                "object1",
                "object2"
            ]
        }
    ],
    "inputsInline": true,
    "output": "Boolean",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const wall = {
    "type": "wall",
    "message0": "壁",
    "inputsInline": true,
    "output": "object1",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const coin = {
    "type": "coin",
    "message0": "コイン",
    "inputsInline": true,
    "output": "object1",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const player = {
    "type": "player",
    "message0": "プレイヤー",
    "inputsInline": true,
    "output": "object1",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const anything = {
    "type": "anything",
    "message0": "何らかのオブジェクト",
    "inputsInline": true,
    "output": "object1",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const empty = {
    "type": "empty",
    "message0": "何もない空間",
    "inputsInline": true,
    "output": "object1",
    "colour": 240,
    "tooltip": "",
    "helpUrl": ""
}

const obstacle = {
    "type": "obstacle",
    "message0": "障害物",
    "inputsInline": true,
    "output": "object2",
    "colour": 250,
    "tooltip": "",
    "helpUrl": ""
}

const bomb = {
    "type": "bomb",
    "message0": "爆弾",
    "inputsInline": true,
    "output": "object2",
    "colour": 250,
    "tooltip": "",
    "helpUrl": ""
}

const put_object = {
    "type": "put_object",
    "message0": "%1 方向に %2 を設置",
    "args0": [
        {
            "type": "input_value",
            "name": "direction",
            "check": "direction"
        },
        {
            "type": "input_value",
            "name": "object",
            "check": "object2"
        }
    ],
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 270,
    "tooltip": "爆弾または障害物を設置できる"
};

const destroy_obstacle = {
    "type": "destroy_obstacle",
    "message0": "%1 方向の障害物を破壊",
    "args0": [
        {
            "type": "input_value",
            "name": "direction",
            "check": "direction",
        }
    ],
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 270,
    "tooltip": "",
    "helpUrl": ""
};

const pick_bomb = {
    "type": "pick_bomb",
    "message0": "%1 方向の爆弾を回収",
    "args0": [
        {
            "type": "input_value",
            "name": "direction",
            "check": "direction",
        }
    ],
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 270,
    "tooltip": "",
    "helpUrl": ""
};

const custom_number = {
  "type": "custom_number",
  "message0": "%1",
  "args0": [
    {
      "type": "field_number",
      "name": "number",
      "value": 3,
      "min": 0,
      "max": 5
    }
  ],
  "output": "Number",
  "colour": 230,
  "tooltip": "",
  "helpUrl": ""
}

Blockly.Blocks['move_left'] = {
    init: function () {
        this.jsonInit(move_left);
    }
};

Blockly.Blocks['move_right'] = {
    init: function () {
        this.jsonInit(move_right);
    }
};

Blockly.Blocks['move_forward'] = {
    init: function () {
        this.jsonInit(move_forward);
    }
};

Blockly.Blocks['move_back'] = {
    init: function () {
        this.jsonInit(move_back);
    }
};

Blockly.Blocks['turn_left'] = {
    init: function () {
        this.jsonInit(turn_left);
    }
};

Blockly.Blocks['turn_right'] = {
    init: function () {
        this.jsonInit(turn_right);
    }
};

Blockly.Blocks['get_left'] = {
    init: function () {
        this.jsonInit(get_left);
    }
};

Blockly.Blocks['get_right'] = {
    init: function () {
        this.jsonInit(get_right);
    }
};

Blockly.Blocks['get_forward'] = {
    init: function () {
        this.jsonInit(get_forward);
    }
};

Blockly.Blocks['get_back'] = {
    init: function () {
        this.jsonInit(get_back);
    }
};

Blockly.Blocks['check_point'] = {
    init: function () {
        this.jsonInit(check_point);
    }
};

Blockly.Blocks['wall'] = {
    init: function () {
        this.jsonInit(wall);
    }
};

Blockly.Blocks['coin'] = {
    init: function () {
        this.jsonInit(coin);
    }
};

Blockly.Blocks['player'] = {
    init: function () {
        this.jsonInit(player);
    }
};

Blockly.Blocks['anything'] = {
    init: function () {
        this.jsonInit(anything);
    }
};

Blockly.Blocks['empty'] = {
    init: function () {
        this.jsonInit(empty);
    }
};

Blockly.Blocks['obstacle'] = {
    init: function () {
        this.jsonInit(obstacle);
    }
};

Blockly.Blocks['bomb'] = {
    init: function () {
        this.jsonInit(bomb);
    }
};

Blockly.Blocks['put_object'] = {
    init: function () {
        this.jsonInit(put_object);
    }
};

Blockly.Blocks['destroy_obstacle'] = {
    init: function () {
        this.jsonInit(destroy_obstacle);
    }
};

Blockly.Blocks['pick_bomb'] = {
    init: function () {
        this.jsonInit(pick_bomb);
    }
};

Blockly.Blocks['custom_number'] = {
    init: function () {
        this.jsonInit(custom_number);
    }
};

Blockly.JavaScript['move_left'] = function() {
    let code = 'move_left();\n';
    return code;
};

Blockly.JavaScript['move_right'] = function() {
    let code = 'move_right();\n';
    return code;
};

Blockly.JavaScript['move_forward'] = function() {
    let code = 'move_forward();\n';
    return code;
};

Blockly.JavaScript['move_back'] = function() {;
    let code = 'move_back();\n';
    return code;
}

Blockly.JavaScript['turn_left'] = function() {
    let code = 'turn_left();\n';
    return code;
};

Blockly.JavaScript['turn_right'] = function() {
    let code = 'turn_right();\n';
    return code;
};

Blockly.JavaScript['get_left'] = function () {
    var code = '\'left\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['get_right'] = function () {
    var code = '\'right\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['get_forward'] = function () {
    var code = '\'forward\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['get_back'] = function () {
    var code = '\'back\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['check_point'] = function (block) {
    let code;
    let value_direction = Blockly.JavaScript.valueToCode(block, 'direction', Blockly.JavaScript.ORDER_ATOMIC) || null;
    let value_object = Blockly.JavaScript.valueToCode(block, 'object', Blockly.JavaScript.ORDER_ATOMIC) || null;
    if (value_direction == null || value_object == null) {
        code = 'false';
    } else {
        code = 'check_point(' + value_direction + ', ' + value_object + ')';
    }
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['wall'] = function () {
    let code = '\'Wall\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['coin'] = function () {
    let code = '\'Coin\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['player'] = function () {
    let code = '\'Player\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['anything'] = function () {
    let code = '\'anything\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['empty'] = function () {
    let code = '\'empty\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['obstacle'] = function () {
    let code = '\'\Obstacle\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['bomb'] = function () {
    let code = '\'Bomb\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['put_object'] = function (block) {
    var direction = Blockly.JavaScript.valueToCode(block, 'direction', Blockly.JavaScript.ORDER_ATOMIC) || null;
    var object = Blockly.JavaScript.valueToCode(block, 'object', Blockly.JavaScript.ORDER_ATOMIC) || null;
    if(direction == null || object == null){
        return 'increase_miss();\n';
    }else{
        return 'put_object(' + direction + ', ' + object + ');\n';
    }
};

Blockly.JavaScript['destroy_obstacle'] = function (block) {
    var value_name = Blockly.JavaScript.valueToCode(block, 'direction', Blockly.JavaScript.ORDER_ATOMIC) || null;
    if(value_name == null){
        return 'increase_miss();\n';
    }else{
        return 'destroy_obstacle(' + value_name + ');\n';
    }
};

Blockly.JavaScript['pick_bomb'] = function (block) {
    var value_name = Blockly.JavaScript.valueToCode(block, 'direction', Blockly.JavaScript.ORDER_ATOMIC) || null;
    if(value_name == null){
        return 'increase_miss();\n';
    }else{
        return 'pick_bomb(' + value_name + ');\n';
    }
};

Blockly.JavaScript['custom_number'] = function(block) {
    var number_value = block.getFieldValue('number');
    var code = number_value;
    return [code, Blockly.JavaScript.ORDER_NONE];
};
