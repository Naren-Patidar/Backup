var assert = require('assert');
var router = express.Router();


/*
**************************************************************
Route for storing test results from trx file
and broadcast the signal to all connected clients
this route is called from batch files
***************************************************************
*/
router.post('/', function(req, res){	
	var body = [];
	req.on('data', function(chunk) {
  		body.push(chunk);  		
	}).on('end', function() {
  		body = Buffer.concat(body).toString();  		
  		body = JSON.parse(body); 
  		var batchid = new Date().getTime(); 
  		console.log(body);
  		if(body.UnitTestResults.length > 0)
  		{  			
			var TC = new TRModel();			
  			for(var i in body.UnitTestResults)
			{	
				TC.TestResults.push({
					TestName: body.UnitTestResults[i].TestName,
			  		Result: body.UnitTestResults[i].Result,
			  		StartTime: body.UnitTestResults[i].StartTime,
			  		EndTime: body.UnitTestResults[i].EndTime,
			  		Duration : body.UnitTestResults[i].Duration,
			  		Message: body.UnitTestResults[i].Output.Error.Message,
			  		StackTrace: body.UnitTestResults[i].Output.Error.StackTrace,
			  		Description: body.UnitTestResults[i].TestDescription,
			  		Categories : body.UnitTestResults[i].Categories
				});
  			} 
	        TC.batchid = batchid;			
			TC.ServerName = body.UnitTestResults[0].ServerName;
			TC.Environment = body.Environment;
			TC.Country = body.Country;
			TC.Browser = body.Browser;
			TC.Category = body.Category;					
	        TC.BuildInfo = body.BuildInfo;
	        TC.Duration = body.Duration;
	        TC.BuildInfo.actions =  body.BuildInfo.actions[0];			
	        TC.BuildInfo.actions.parameters =  body.BuildInfo.actions[0].parameters;
  			TC.save(function(err, TCE) { 				
        		if (err) { return console.error(err); }
        			console.log('Document added');
        			var query = { category: body.Category , country: body.Country, environment: body.Environment };
	        		var update = { $set: { status: 0, lastEndTime: new Date() } };
	        		var options = { upsert: true, new: true, setDefaultsOnInsert: true };
	        		console.log(query);
	        		RSModel.update(query, update, options, function(err){
	            		if(err)
	            		{         
	              				return Logger.log('Error : ' + err, 'UpdateStatus'); 
	            		}
	            		Logger.log('Status updated in to database.');       
	            		Logger.log('Ending' , 'UpdateStatus');
	        		});
	        		var msg = 'Job Completed for ' + body.Category + ' country: ' + body.Country + ', category : ' + body.Category;
	        		signalR.broadcast({data : { 
	        			environment: body.Environment, 
	        			country: body.Country,
	        			category: body.Category,
	        			status: 0 },
	        			msg: msg});
        			res.send({ status : true });   
        		}        		
        	); 
  		}	
  		
	});
});

addTestResults = function(testResult, index,  modelObj){
	modelObj.TestResults[index] = testResult;
}

module.exports = router;