var mongoose  = require('mongoose');
var Schema = mongoose.Schema;


var RunStatus = new Schema({
	category: String,
	country: String,
	environment: String,
	status: Number,
	lastStartTime : Date,
	lastEndTime: Date
});

module.exports = RunStatus;