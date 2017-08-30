var assert = require('assert');
var url = require('url');
var path = require('path');
var router = express.Router();

router.get('/files/*', function(req, res){	
	var url_parts = url.parse(req.url, true);
  	var req_path = url_parts.path.split('/');
  	var parameters = req_path.slice(2,path.length);
  	var file_path = parameters.join('\\');
  	var queryStrings = url_parts.query;
  	if(file_path.indexOf('?') > 0)
  	{
  		file_path = file_path.substring(0, file_path.indexOf('?'));
  	}
  	file_path  = path.join(__dirname ,'\\..\\public\\', file_path);  
  	console.log(file_path);	
  	res.sendFile(file_path);
});

module.exports = router;