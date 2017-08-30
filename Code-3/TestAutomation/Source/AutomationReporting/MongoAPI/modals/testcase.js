var mongoose  = require('mongoose');

var Schema = mongoose.Schema;

var TestCaseSchema = new Schema({
  		TestName: String,
  		Result: String,
  		StartTime: { type: Date, default: Date.now },
  		EndTime: { type: Date, default: Date.now },
  		Duration : String,
  		Message: String,
  		StackTrace: String,
  		Description: String,
  		Categories: [String]  
});

module.exports = TestCaseSchema;