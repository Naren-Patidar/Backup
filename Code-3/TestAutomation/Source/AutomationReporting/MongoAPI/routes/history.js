var express = require('express');
var historyRouter = express.Router();
var url = require('url');

historyRouter.get('/', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;	
	if(queryStrings)
	{
		console.log(queryStrings);
		if(queryStrings.Environment == '')
		{
			delete queryStrings.Environment;			
		}
		if(queryStrings.Country == '')
		{
			delete queryStrings.Country;
		}
		if(queryStrings.Category == '')
		{
			delete queryStrings.Category;
		}
		if(queryStrings['BuildInfo.number'] == '')
		{
			delete queryStrings.BuildInfo.number;			
		}

		if(queryStrings.FromDate)
		{
			var fromdate = queryStrings.FromDate;			
			queryStrings.CreateTime = {
        			$gte: fromdate
    			};
		}
		delete queryStrings.FromDate;
		console.log(queryStrings);
		var query = TRModel.find(queryStrings).sort({ CreateTime : -1 });
		query.exec(function (err, doc) {			
			if(err)
			{
				res.send(err);
			}
			if(doc.length > 0)
			{
				try{
					var responseObj = [];
					doc.map(function(item, i){
						var res = {};
						var passed = item.TestResults.filter(function(item) { return item.Result == 'Passed'; }).length;
						var inconclusive = item.TestResults.filter(function(item) { return item.Result == 'Inconclusive'; }).length;					
						var total = item.TestResults.length;

						res.percentage = (((passed + inconclusive) / total)*100).toFixed(2);
						res.country = item.Country;
						res.category = item.Category;
						res.environment = item.Environment;
						res.createtime = item.CreateTime;
						responseObj.push(res);
					});						
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

module.exports = historyRouter;