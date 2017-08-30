var router = express.Router();
var url = require('url');


router.get('/', function(req, res){
	var url_parts = url.parse(req.url, true);
	var queryStrings = url_parts.query;
	var id = queryStrings.id;
	if(id)
	{
		TRModel.find({ batchid : id }, function(err, docs){
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
	var country = queryStrings.c;
	var environment = queryStrings.e;
	var category = queryStrings.cat;
	if(country && environment && category)
	{	
		var query = TRModel.find({ Country : country, Environment : environment, Category : category })
		.sort({ CreateTime: -1 });		
		query.exec(function (err, doc) {
  			if (err) 
  			{
  				console.error(err); 
  				res.send(err);
  			}  			  			
  			if(doc.length > 0)	
  			{
  				res.send(doc[0]);
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

module.exports = router;