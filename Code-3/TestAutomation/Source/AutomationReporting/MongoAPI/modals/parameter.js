var mongoose  = require('mongoose');
var Schema = mongoose.Schema;

var Parameter = new Schema({
	name: String,
	value: String	
});

module.exports = Parameter;