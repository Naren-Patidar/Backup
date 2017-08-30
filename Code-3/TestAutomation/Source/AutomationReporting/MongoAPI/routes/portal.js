
var url = require('url');
var path = require('path');

var router = express.Router();
var resObject = [];


router.get('/dashboard', function(req, res){
	res.sendFile(path.join(__dirname ,'/../public/views', 'dashboardV2.html'));	
});



module.exports = router;