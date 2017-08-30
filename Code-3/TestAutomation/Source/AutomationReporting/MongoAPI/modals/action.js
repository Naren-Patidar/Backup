var mongoose  = require('mongoose');
var ParamSchema = require('./parameter');
var Schema = mongoose.Schema;


var Action = new Schema({
	parameters: [ParamSchema]
});

module.exports = Action;