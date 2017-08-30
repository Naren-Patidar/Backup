var mongoose  = require('mongoose');
var ActionSchema = require('./action');
var Schema = mongoose.Schema;

var BuildInfo = new Schema({
  		actions: ActionSchema,
  		number: String,
  		timestamp: String,
  		builtOn: String
});

module.exports = BuildInfo;