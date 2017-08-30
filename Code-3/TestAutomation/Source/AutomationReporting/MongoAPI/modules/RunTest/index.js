var MSTest = require('mstest');

var msTest = new MSTest();
var RunTest = function(category, environment, country, browser, jenkinAPI)
{
	this.category = category;
	this.environment = environment;
	this.country = country;
	this.browser = browser;	
	this.StartTime = new Date();
	this.jenkinAPI = jenkinAPI;
	Logger.log(this, 'RunTest constructor');
}

RunTest.prototype.Execute = function(onExecuting){
	Logger.log('Starting' , 'Execute');
	var instance = this;
	// writing log for starting the execution
	Logger.log('starting the executoion for Category: ' + this.category 
		+ ', Environment: ' + this.environment
		+ ', Country: ' + this.country
		+ ', Browser: ' + this.browser);
	instance.GetStatus(function(status){
		Logger.log('from callback status: ' + status);		
		if(status == null)
		{
			Logger.log('Status : null' , 'Execute');
			instance.AddStatus(1, function(doc){
				Logger.log(doc, 'Run Status Added');
				instance.RunTest(onExecuting);
			});			
		}
		else if(status.status == 0)
		{
			Logger.log('Status : 0' ,'Execute');
			instance.UpdateStatus(1, function(){ 
				Logger.log(' Status updated as Running. ', 'Callback UpdateStatus'); 
				instance.RunTest(onExecuting);
			});			
		}
		else if(status.status == 1)
		{
			Logger.log('Status : 1' , 'Execute');
			onExecuting(1);
		}
		Logger.log('Ending' , 'Execute');
	});	
}

RunTest.prototype.RunTest = function(callback){
	Logger.log('Starting' , 'RunTest');
	var instance = this;
	try
	{
		msTest.setCategory(this.category);	
	    msTest.exePath = CONSTANTS.MSTEST_EXE_PATH;
	    msTest.testContainer = instance.GetDllFileName();
	    msTest.workingDir = instance.GetWorkingDirectory();
	    msTest.resultsFile = instance.GetOutputFileName();
	    msTest.details.errorMessage = true;
	    msTest.details.errorStackTrace = true;
	    msTest.details.description = true;
	    msTest.details.duration = true;
	    msTest.details.computerName = true; 
	    msTest.details.testCategoryID = true;
	    msTest.details.testName = true;
	    Logger.log(msTest, 'MSTEST Object');
	    msTest.runTests({
	          eachTest: function(test) {
	      		Logger.log(test.status + ' - ' + test.testName, 'MSTEST');
				Logger.log(test.status, 'MSTEST');
				Logger.log(test.StartTime,'MSTEST');
				Logger.log(test.EndTime,'MSTEST');
				Logger.log(test.duration,'MSTEST');
				Logger.log(test.errorMessage,'MSTEST');
				Logger.log(test.errorStackTrace, 'MSTEST');
	          },
	          done: function(results, passed, failed) {
	      		Logger.log('Passed: ' + passed.length + '/' + results.length);      		
	      		Logger.log('Failed: ' +failed.length + '/' + results.length);       		      		
	      		instance.CreateModel(results, function(updated_model){ 
	      			Logger.log('Create Model Callback.');	      			
	      			instance.UpdateStatus(0, function(){
		        		Logger.log('Job status is set as Ready');
		        		Logger.log(updated_model);
		        		var results = new TRModel(updated_model);		        		
						    results.save(function(err, TCE) { 				
			        		if (err) 
			        		{			        			
			        			Logger.error(err);		        				
			        		}
			        		else
			        		{		        		
			        			Logger.log('Document added in database', 'Result Added'); 		        			    		
			        	}
		        			}); 				
		        	}); 	      			
	      		});
	      		Logger.log('Ending' , 'RunTest');
	          }
	    });	
	    callback();
	}
	catch(err)
	{
		Logger.log(err.stack, 'Error RunTest');
	}
}

RunTest.prototype.UpdateStatus = function(status, callback)
{	
	Logger.log('Starting' , 'UpdateStatus');
	var instance = this;
	var query = { category: this.category , country: this.country, environment: this.environment };
	var update = { $set: { status: status } };
	var options = { multi: true };
	
		RSModel.update(query, update, options, function(err){
  			if(err)
  			{  				
  				return Logger.log('Error : ' + err, 'UpdateStatus'); 
  			}
  			Logger.log('Status updated in to database.');  			
  			Logger.log('Ending' , 'UpdateStatus');
  			callback();
  		});
		
}

RunTest.prototype.AddStatus = function(status, callback)
{
	Logger.log('Starting' , 'AddStatus');
	var sm = new RSModel();
  	sm.country = this.country;
  	sm.category = this.category;
  	sm.environment = this.environment;
  	sm.status = status;
  	sm.lastStartTime = new Date();
  	
  		sm.save(function(err, TCE) { 				
		if (err) 
		{ 		    
		    return Logger.log('Error : ' +  err, 'AddStatus'); 		       		
		}		
		Logger.log(TCE);
		Logger.log('Ending' , 'AddStatus');
		callback(TCE);
		});		
}

RunTest.prototype.GetStatus = function(callback)
{
	Logger.log('Starting' , 'GetStatus');
	
		RSModel.findOne({ category: this.category , country: this.country, environment: this.environment }, 'status', function (err, status) {
	  	if (err)
	  	{	  		
	  		return Logger.log('Error : ' + err, 'GetStatus');   					 
	  	}		
	  	Logger.log('Ending' + status , 'GetStatus');  				 				
	  	callback(status);
		});	
}

RunTest.prototype.GetDllFileName = function()
{
	Logger.log('Starting' , 'GetDllFileName');
	var dllName = CONSTANTS.AUTOMATION_ROOT 
			+ 'Framework_' + this.country
			+ '_' + this.environment
			+ '/Tesco.Framework.UITesting.'+ this.browser +'.dll'; 
	Logger.log(dllName, 'GetDllFileName');
	Logger.log('Ending' , 'GetDllFileName');
	return dllName;
}

RunTest.prototype.GetWorkingDirectory = function()
{
	Logger.log('Starting' , 'GetWorkingDirectory');
	var workDir = CONSTANTS.AUTOMATION_ROOT + 'TRXs/' + this.country;
	Logger.log(workDir, 'GetWorkingDirectory');
	Logger.log('Ending' , 'GetWorkingDirectory');
	return workDir;
}

RunTest.prototype.GetOutputFileName = function()
{
	Logger.log('Starting' , 'GetOutputFileName');
	var trxName = this.category + '_' + new Date().getTime() +  '.trx';
	Logger.log(trxName, 'GetOutputFileName');
	Logger.log('Ending' , 'GetOutputFileName');
	return trxName;
}

RunTest.prototype.CreateModel = function(results, onModel_Updated){	
	Logger.log('Starting' , 'CreateModel');
	var instance = this;
	var endTime = new Date();
	if(results.length > 0)
	{
		var build = this.jenkinAPI.GetBuildInfo('36_Core_Deploy%20to%20ST', function(data) {
			try{						
				Logger.log(data, 'Jenkin API response');
				data = JSON.parse(data);
				var TC = new TRModel();
				for(var i in results)
				{	
					TC.TestResults.push({
						TestName: results[i].testName,
						Result: results[i].status,
						StartTime: results[i].StartTime,
						EndTime: results[i].EndTime,
				 		Duration : results[i].duration,
				  		Message: results[i].errorMessage,
				  		StackTrace: results[i].errorStackTrace
					});
			  	} 
				TC.batchid = new Date().getTime();			
				TC.ServerName = results[0].computerName;
				TC.Environment = instance.environment;
				TC.Country = instance.country;
				TC.Browser = instance.browser;
				TC.Category = instance.category;
				TC.Duration = endTime - instance.StartTime;
				TC.BuildInfo = data;
				TC.BuildInfo.actions = data.actions[0];
				Logger.log('Ending' , 'CreateModel');
				onModel_Updated(TC);
			}
			catch(err)
			{
				return Logger.log(err.stack , 'Error CreateModel');
			}
		});			
	}
	else
	{
		Logger.log('No results from mstest', 'CreateModel');
		Logger.log('Ending' , 'CreateModel');
		instance.UpdateStatus(0, function(){
			Logger.log(' Status updated as Ready. ', 'Callback UpdateStatus');
		})
	}
}

module.exports = RunTest;