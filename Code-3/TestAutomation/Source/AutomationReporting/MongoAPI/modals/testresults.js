var mongoose   = require('mongoose');
var TestCaseSchema = require('./testcase');
var BuildInfoSchema = require('./buildInfo');

var Schema = mongoose.Schema;

var TestResultsSchema = new Schema({
  batchid: String,   
  ServerName: String,
  Environment: String,
  Country: String,
  Browser: String,
  Category: String,
  Duration: String,
  CreateTime : { type: Date, default: Date.now },
  TestResults: [TestCaseSchema],
  BuildInfo: BuildInfoSchema
});

module.exports = TestResultsSchema;