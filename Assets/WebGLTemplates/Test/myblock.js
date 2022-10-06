const move_left = {
    "type": "move_left",
    "message0": "左に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const move_right = {
    "type": "move_right",
    "message0": "右に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const move_forward = {
    "type": "move_forward",
    "message0": "前に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const move_back = {
    "type": "move_back",
    "message0": "後ろに下がる",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const get_left = {
    "type": "get_left",
    "message0": "左",
    "inputsInline": true,
    "output": "direction",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const get_right = {
    "type": "get_right",
    "message0": "右",
    "inputsInline": true,
    "output": "direction",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const get_forward = {
    "type": "get_forward",
    "message0": "前",
    "inputsInline": true,
    "output": "direction",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const get_back = {
    "type": "get_back",
    "message0": "後ろ",
    "inputsInline": true,
    "output": "direction",
    "colour": 230,
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
            "check": "object",
        }
    ],
    "inputsInline": true,
    "output": "Boolean",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const obstacle = {
    "type": "obstacle",
    "message0": "障害物",
    "inputsInline": true,
    "output": "object",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const put_obstacle = {
  "type": "put_obstacle",
  "message0": "%1 方向に障害物を設置",
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

Blockly.Blocks['obstacle'] = {
    init: function () {
        this.jsonInit(obstacle);
    }
};

Blockly.Blocks['put_obstacle'] = {
  init: function () {
    this.jsonInit(put_obstacle);
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

Blockly.JavaScript['obstacle'] = function () {
    let code = '\'障害物\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['put_obstacle'] = function (block) {
    var value_name = Blockly.JavaScript.valueToCode(block, 'direction', Blockly.JavaScript.ORDER_ATOMIC);
    let code = 'put_obstacle(' + value_name + ');\n';
    return code;
};