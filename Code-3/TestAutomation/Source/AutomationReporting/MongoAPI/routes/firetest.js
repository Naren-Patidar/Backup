var assert = require('assert');
var router = express.Router();
var url = require('url');
var http = require('http');

/*
**************************************************************
Route for executing automation using MsTest
***************************************************************
*/
router.get('/', function(req, res){
  //get parameters from querystring
  var url_parts = url.parse(req.url, true);
  var queryStrings = url_parts.query;
  var category = queryStrings.category;
  var environment = queryStrings.environment;
  var country = queryStrings.country;
  var browser = queryStrings.browser || 'GC';
  if(typeof(category) == typeof(undefined))
  {
    res.send("<h1>Server Error</h1><br/> please sepecify the category")
  }
  else if(typeof(environment) == typeof(undefined))
  {
    res.send("<h1>Server Error</h1><br/> please sepecify the environment")
  }
  else if(typeof(country) == typeof(undefined))
  {
    res.send("<h1>Server Error</h1><br/> please sepecify the country")
  }
  else
  {
    // instansiate the object of RunTest module
    var runtest_handle = new runtest(category,
      environment,
      country,
      browser,      
      new JenkinAPI('pvcdljenma001uk.global.tesco.org')
    );
    // trigger the execution
    runtest_handle.Execute(function(runstatus){
      Logger.log('Triggered Automation Successfully!!');      
      res.send({ status : runstatus });
    });
  }  
});

/*
**************************************************************
Route for executing batch file on local server
***************************************************************
*/
router.get('/batch', function(req, res){
  //get parameters from querystring
  var url_parts = url.parse(req.url, true);
  console.log(url_parts);
  var queryStrings = url_parts.query;
  var category = queryStrings.category;
  var environment = queryStrings.environment;
  var country = queryStrings.country;
  var browser = queryStrings.browser || 'GC';
  if(typeof(category) == typeof(undefined))
  {
    res.send("<h1>Server Error</h1><br/> please sepecify the category")
  }
  else if(typeof(environment) == typeof(undefined))
  {
    res.send("<h1>Server Error</h1><br/> please sepecify the environment")
  }
  else if(typeof(country) == typeof(undefined))
  {
    res.send("<h1>Server Error</h1><br/> please sepecify the country")
  }
  else
  {
    var root_folder = CONSTANTS.RUNCONFIG[environment][country].path;
    var browser = CONSTANTS.RUNCONFIG[environment][country].browser;    
    var batFile = root_folder + category + '\\' + category + '.' + browser  + '.' + environment + '.bat';
    console.log(batFile);
    var spawn = require('child_process').spawn,
    ls = spawn('cmd.exe', ['/c', batFile], { detached: true });

    ls.stdout.on('data', function (data) {
       console.log('stdout: ' + data);
    });

    ls.stderr.on('data', function (data) {
      console.log('stderr: ' + data);
    });

    ls.on('exit', function (code) {
      console.log('child process exited with code ' + code);
    });
    res.send({ status : 200 });
  }  
});

/*
**************************************************************
Route for executing batch file on remote servers
by calling self hosted wcf service 
***************************************************************
*/
router.get('/batch/*', function(req, res){
  //get parameters from querystring
  var url_parts = url.parse(req.url, true);
  var path = url_parts.path.split('/');
  var parameters = path.slice(2,path.length);
  var api_path = CONSTANTS.TRIGGER_PATH + parameters.join('/');

  console.log(api_path);
  var options = {
    host: CONSTANTS.TRIGGER_HOST, 
    port: 8000,  
    path: api_path,
    method: 'GET'
  };

  var request = http.request(options, function(response) {
    response.setEncoding('utf8');
    response.on('data', function (chunk) {  
      var data = JSON.parse(chunk);   
      console.log(data.IsSuccess);
      if(data.IsSuccess)
      { 
        var category = parameters[2];
        if(category == 'Sa') { category = 'Sanity'; }
        if(category == 'Ba') { category = 'BasicFunctionality'; }
        var query = { category: category , country: parameters[1], environment: parameters[0] };
        var update = { $set: { status: 1, lastStartTime: new Date() } };
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
      }
      res.send(chunk);
    });
  });
  request.on('error', function(err) {
    res.send({error: err});
  });
  request.end();  
});

/*
**************************************************************
Route for executing calling 'batch' route depending on input
Environment , Country, Category
***************************************************************
*/
router.get('/batchtrigger/*', function(req, res){
  try
  {
    //get parameters from querystring
    var url_parts = url.parse(req.url, true);
    var path = url_parts.path.split('/');
    var parameters = path.slice(2,path.length);
    var api_path = CONSTANTS.TRIGGERAPI_PATH + parameters.join('/');
    var key = parameters[1].toUpperCase() + '_' + parameters[0].toUpperCase();
    var trigger_host = CONSTANTS.TriggerAPI[key];
    var trigger_port = CONSTANTS.TriggerAPIPort[key];
    
    var options = {
      host: trigger_host, 
      port: trigger_port,  
      path: api_path,
      method: 'GET'
    };
    Logger.log(options, 'batchtrigger.request.options');
    var request = http.request(options, function(response) {
      response.setEncoding('utf8');
      response.on('data', function (chunk) {  
        var data = JSON.parse(chunk);   
        Logger.log(data, 'batchtrigger.request.data');
        if(data.IsSuccess)
        {                 
          var category = parameters[2];
          if(category == 'Sa') { category = 'Sanity'; }
          if(category == 'Ba') { category = 'BasicFunctionality'; }
          var query = { category: category , country: parameters[1], environment: parameters[0] };
          var update = { $set: { status: 1, lastStartTime: new Date() } };
          var options = { upsert: true, new: true, setDefaultsOnInsert: true };
          Logger.log(query);
          RSModel.update(query, update, options, function(err){
            if(err)
            {         
              Logger.log('Error : ' + err, 'UpdateStatus');
              res.send({error: err});
            }
            Logger.log('Status updated in to database.');       
            Logger.log('Ending' , 'UpdateStatus');
          });
        }
        res.send(chunk);
      });
    });
    request.on('error', function(err) {
      res.send({error: err});
    });
    request.end();
  }
  catch(error)
  {
    res.send({error:error});
  }  
});

/*
**************************************************************
Route for getting the status of job from mongo db
***************************************************************
*/
router.get('/getbatchstatus/*', function(req, res){
  //get parameters from querystring
  var url_parts = url.parse(req.url, true);
  var path = url_parts.path.split('/');
  var parameters = path.slice(2,path.length);
  RSModel.findOne({ category: parameters[2] , country: parameters[1], environment: parameters[0] }, 'status lastStartTime', function (err, doc) {
      if (err)
      {       
        return Logger.log('Error : ' + err, 'GetStatus');              
      }   
      Logger.log('Ending ' + doc , 'GetStatus');                  
      res.send(doc);
    });  
});

module.exports = router;