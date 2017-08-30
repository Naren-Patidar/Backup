var router = express.Router();
var url = require('url');
var Provider = require('./../providers/testresults');


router.get('/', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	if(queryStrings)
	{
		TRModel.find(queryStrings, function(error, docs) {    		
			if(error)
			{
				res.send(error);
			}
    		res.send(docs);
		});		
	}
	else
	{
		res.send("<h1>Server Error</h1><br/> No query to search")
	}	
});

router.get('/test', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	if(queryStrings)
	{
		TRModel.find(queryStrings, 'TestResults.$', function(error, docs) {    		
			if(error)
			{
				res.send(error);
			}
    		res.send(docs);
		});		
	}
	else
	{
		res.send("<h1>Server Error</h1><br/> No query to search")
	}	
});

router.get('/latest', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	if(queryStrings)
	{
		var query = TRModel.find(queryStrings).sort({ CreateTime : -1 }).limit(1);
		query.exec(function (err, doc) {			
			if(err)
			{
				res.send(err);
			}
    		res.send(doc[0]);
		});	
	}
	else
	{
		res.send("<h1>Server Error</h1><br/> No query to search")
	}	
});

router.get('/latest/piedata', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	if(queryStrings)
	{
		var query = TRModel.find(queryStrings).sort({ CreateTime : -1 }).limit(1);
		query.exec(function (err, doc) {			
			if(err)
			{
				res.send(err);
			}
			if(doc.length > 0)
			{
				try{
					var responseObj = {
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
	    			res.send(responseObj);
    			}
    			catch(error)
    			{
    				console.log(error);
    				res.send({ error : error });
    			}
    		}
    		else
			{
				res.send({});
			}

		});	
	}
	else
	{
		res.send("<h1>Server Error</h1><br/> No query to search")
	}	
});

router.get('/latest/testresult', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	var result = queryStrings.result || queryStrings.Result;
	delete queryStrings.Result;	
	delete queryStrings.result;	
	if(queryStrings)
	{		
		var query = TRModel.find(queryStrings).sort({ CreateTime : -1 }).limit(1);
		query.exec(function (err, doc) {			
			if(err)
			{
				res.send(err);
			}	
			if(doc.length > 0)
			{
    			res.send(
					{
						results : doc[0].TestResults.filter(function(item) { return item.Result == result; })
					}
    			);
    		}
    		else
			{
				res.send({});
			}

		});	
	}
	else
	{
		res.send("<h1>Server Error</h1><br/> No query to search")
	}	
});

router.get('/dashboarddata', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	var p = new Provider(queryStrings.Environment, queryStrings.Country, queryStrings.Category);
	p.GetDashboardData(function(data){
		res.send(data);
	})
});


module.exports = router;