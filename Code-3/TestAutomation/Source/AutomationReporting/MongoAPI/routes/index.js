
var url = require('url');
var assert = require('assert');
var mongo_url = 'mongodb://localhost:27017/Automation';
var router = express.Router();
var resObject = [];
var Provider = require('./../providers/testresults');



router.get('/', function(req, res){
	res.send("<h1>Welcome to API 2</h1>");
});


router.get('/report', function(req, res){
	var url_parts = url.parse(req.url, true);
	var qs = url_parts.query;	
	var environment = qs.environment;
	var country = qs.country;
	var category = qs.category;
	var date = new Date(qs.y, qs.m, qs.d);
	var p = new Provider(environment,country,category);
	p.GetReport(date, function(data){
		res.send(data);
	})
});


module.exports = router;