var router = express.Router();
var url = require('url');

/*
**************************************************************
Route for getting distict data from mongo database
***************************************************************
*/
router.get('/', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;
	var q = queryStrings.q;
	if(q)
	{
		TRModel.distinct(q, function(error, ids) {    		
			if(error)
			{
				res.send(error);
			}
    		res.send(ids);
		});		
	}
	else
	{
		res.send("<h1>Server Error</h1><br/> No query to search")
	}	
});


module.exports = router;