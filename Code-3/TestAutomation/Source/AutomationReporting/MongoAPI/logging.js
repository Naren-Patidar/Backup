var fs = require('fs');
var util = require('util');
var log_file = fs.createWriteStream(__dirname + '/debug.log', {flags : 'w'});
var log_stdout = process.stdout;

var Logging = function(){};


Logging.prototype.log = function(msg, header)
{
	try
	{
		if(CONSTANTS.TRACE_ENABLE)
		{
			console.log(header + ' >> ' + util.format(msg) + '\n');
	//		log_file.write(new Date() + ' : ' + header + ' >> ' + util.format(msg) + '\r\n');			
	// 		log_stdout.write(new Date() + ' : ' + header + ' >> ' + util.format(msg) + '\r\n');
		}
	}
	catch(err){ console.log(err.stack); }
}

module.exports = Logging;