var TestResultsProvider = function(environment, country, category){
	this.country = country;
	this.category = category;
	this.environment = environment;
}

TestResultsProvider.prototype.GetLatestPieDataByQuery = function(query, callback){
	Logger.log('GetLatestPieDataByQuery start');
	var responseObj = {};
	var dbquery = TRModel.find(query).sort({ CreateTime : -1 }).limit(1);
	dbquery.exec(function (err, doc) {			
		if(err)
		{			
			responseObj = { error : err };
			Logger.log(err);
		}		
		if(doc.length > 0)
		{			
			try{
				responseObj = {
					passed : doc[0].TestResults.filter(function(item) { return item.Result == 'Passed'; }).length,
					inconclusive : doc[0].TestResults.filter(function(item) { return item.Result == 'Inconclusive'; }).length,
					failed : doc[0].TestResults.filter(function(item) { return item.Result == 'Failed'; }).length,
					total : doc[0].TestResults.length,
					server : doc[0].ServerName,
					browser : doc[0].Browser,
					category : doc[0].Category,
					country : doc[0].Country,
					createtime : doc[0].CreateTime,
					duration: doc[0].Duration
				};						
				if(typeof(doc[0].BuildInfo) != typeof(undefined))
				{							
					responseObj.buildNumber = doc[0].BuildInfo.number;
					var v = doc[0].BuildInfo.actions.parameters.filter(function(item) { return item.name == 'VERSION'; });
					if(v.length  > 0)
					{
						responseObj.version = v[0].value;
					}
					var ch = doc[0].BuildInfo.actions.parameters.filter(function(item) { return item.name == 'CommitHistory'; });
					if(ch.length > 0)
					{
						responseObj.commitHistory = ch[0].value
					}					
				}
				Logger.log(responseObj);
    		}
    		catch(error)
    		{
    			Logger.log(error);
    			responseObj = { error : error };
    		}
    	}    
    	if(callback)	
    	{
    		callback(responseObj);
    	}
    });    
}

TestResultsProvider.prototype.GetLatestPieData = function(callback){
	Logger.log('GetLatestPieData start');
	var responseObj = {};
	var query = { Environment: this.environment, Country: this.country, Category: this.category };
	Logger.log(query);
	var dbquery = TRModel.find(query).sort({ CreateTime : -1 }).limit(1);
	dbquery.exec(function (err, doc) {		
		if(err)
		{
			Logger.log(err);
			responseObj ={ error: err };
		}
		if(doc.length > 0)
		{
			try{
				responseObj = {
					passed : doc[0].TestResults.filter(function(item) { return item.Result == 'Passed'; }).length,
					inconclusive : doc[0].TestResults.filter(function(item) { return item.Result == 'Inconclusive'; }).length,
					failed : doc[0].TestResults.filter(function(item) { return item.Result == 'Failed'; }).length,
					total : doc[0].TestResults.length,
					server : doc[0].ServerName,
					browser : doc[0].Browser,
					category : doc[0].Category,
					country : doc[0].Country,
					createtime : doc[0].CreateTime
				};						
				if(typeof(doc[0].BuildInfo) != typeof(undefined))
				{							
					responseObj.buildNumber = doc[0].BuildInfo.number;
					var v = doc[0].BuildInfo.actions.parameters.filter(function(item) { return item.name == 'VERSION'; });
					if(v.length  > 0)
					{
						responseObj.version = v[0].value;
					}
					var ch = doc[0].BuildInfo.actions.parameters.filter(function(item) { return item.name == 'CommitHistory'; });
					if(ch.length > 0)
					{
						responseObj.commitHistory = ch[0].value
					}					
				}
    		}
    		catch(error)
    		{
    			responseObj = { error : error };
    		}
    	}
    	if(callback)
    	{
    		callback(responseObj);
    	}
    });
    
}


TestResultsProvider.prototype.GetAllCategories = function(callback){
	Logger.log('GetLatestPieData start');
	var responseObj = {};
	TRModel.distinct('Category', function(error, data){
		if(error)
		{
			Logger.log(err);
			responseObj = { error: err };
		}
		responseObj = data;
		if(callback)
	    {
			callback(responseObj);
	    }
	});
}

TestResultsProvider.prototype.GetDashboardData = function(callback){
	var provider = this;
	Logger.log('GetLatestPieData start');
	var responseObj = [];
	var query = { Environment: this.environment, Country: this.country };
	Logger.log(query); 
	var index = 0;   
	provider.GetAllCategories(function(caltegories){
		caltegories.map(function(item, i){
			query.Category = item;
			provider.GetLatestPieDataByQuery(query, function(result){
				if(typeof(result.passed) != typeof(undefined))
				{
					responseObj.push(result);	
				}				
				if(++index == caltegories.length)
				{
					callback(responseObj);
				}
			});						
		});
		
	});
}

TestResultsProvider.prototype.GetReport = function(date, callback){
	var provider = this;
	Logger.log('GetReport start');
	var responseObj = [];
	var start = new Date(date.getFullYear(), date.getMonth(), date.getDate());
	var end = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
	
	var query = { Environment: this.environment, 
					Country: this.country,
					Category: this.category,
					CreateTime: { $gte: start, $lt: end }
				};
	Logger.log(query); 
	var dbquery = TRModel.find(query).sort({ CreateTime : -1 }).limit(1);
	dbquery.exec(function (err, doc) {		
		if(err)
		{
			Logger.log(err);
			responseObj = { error: err };
		}		
		if(doc.length > 0)
		{
			Logger.log('doc found')
			responseObj = { results: doc[0].TestResults };
			if(callback)
			{
				callback(responseObj);
			}
		}		
	});	
}

module.exports = TestResultsProvider;