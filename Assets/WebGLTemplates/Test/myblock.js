const go_left = {
    "type": "go_left",
    "message0": "左に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const go_right = {
    "type": "go_right",
    "message0": "右に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const go_up = {
    "type": "go_up",
    "message0": "上に進む",
    "inputsInline": true,
    "previousStatement": null,
    "nextStatement": null,
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
};

const go_down = {
    "type": "go_down",
    "message0": "下に進む",
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

const get_up = {
    "type": "get_up",
    "message0": "上",
    "inputsInline": true,
    "output": "direction",
    "colour": 230,
    "tooltip": "",
    "helpUrl": ""
}

const get_down = {
    "type": "get_down",
    "message0": "下",
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

Blockly.Blocks['go_left'] = {
    init: function () {
        this.jsonInit(go_left);
    }
};

Blockly.Blocks['go_right'] = {
    init: function () {
        this.jsonInit(go_right);
    }
};

Blockly.Blocks['go_up'] = {
    init: function () {
        this.jsonInit(go_up);
    }
};

Blockly.Blocks['go_down'] = {
    init: function () {
        this.jsonInit(go_down);
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

Blockly.Blocks['get_up'] = {
    init: function () {
        this.jsonInit(get_up);
    }
};

Blockly.Blocks['get_down'] = {
    init: function () {
        this.jsonInit(get_down);
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

Blockly.JavaScript['go_left'] = function() {
    let code = 'go_left();\n';
    return code;
};

Blockly.JavaScript['go_right'] = function() {
    let code = 'go_right();\n';
    return code;
};

Blockly.JavaScript['go_up'] = function() {
    let code = 'go_up();\n';
    return code;
};

Blockly.JavaScript['go_down'] = function() {;
    let code = 'go_down();\n';
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

Blockly.JavaScript['get_up'] = function () {
    var code = '\'up\'';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['get_down'] = function () {
    var code = '\'down\'';
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