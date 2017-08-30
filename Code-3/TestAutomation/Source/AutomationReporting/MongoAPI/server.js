express = require('express');
app = express();
var SignalRJS = require('signalrjs');
mongoose   = require('mongoose');
runtest = require('./modules/RunTest');
JenkinAPI = require('./modules/Jenkin');
CONSTANTS = require('./constants');
var Logging = require('./logging');
Logger = new Logging();
 
signalR = SignalRJS();



var routes = require('./routes/index');
var add_routes = require('./routes/add');
var dis_routes = require('./routes/distinct');
var where_routes = require('./routes/where');
var fire_test = require('./routes/firetest');
var portal_route = require('./routes/portal');
var history_route = require('./routes/history');
var portal_static = require('./routes/static')

db = mongoose.connection;
db.on('error', function callback(err) {Logger.log("Database connection failed. Error: " + err , 'Server');});
db.once('open', function callback() {Logger.log("Database connection successful.", 'Server');});
db.on('close', function callback() {Logger.log("Database connection closed.", 'Server');});
mongoose.Promise = global.Promise;

// Mongoose Schema definition -------------------------------------------
TestResultsSchema = require('./modals/testresults');
RunStatusSchema = require('./modals/runstatus');

// Mongoose model defination -----------------------------------------
TRModel = mongoose.model(CONSTANTS.RESULT_COLLECTION, TestResultsSchema);
RSModel = mongoose.model(CONSTANTS.STATUS_COLLECTION, RunStatusSchema);

// setting routes ---------------------------------------
app.use(CONSTANTS.ROOT, routes);
app.use(CONSTANTS.ROOT + '/add', add_routes);
app.use(CONSTANTS.ROOT + '/distinct', dis_routes);
app.use(CONSTANTS.ROOT + '/where', where_routes);
app.use(CONSTANTS.ROOT + '/firetest', fire_test);
app.use(CONSTANTS.ROOT + '/history', history_route);
app.use(CONSTANTS.ROOT +  '/public', express.static('public'));
app.use(CONSTANTS.ROOT +  '/portal', portal_route);
app.use(CONSTANTS.ROOT +  '/static', portal_static);

// setting application port ---------------------------
app.set('port', CONSTANTS.APPLICATION_PORT);

//Create the hub connection
//NOTE: Server methods are defined as an object on the second argument
signalR.hub('ServerHub',{
	setRunning: function(environment, country, category, status){
		this.clients.all.invoke('updateRunning').withArgs([environment, country, category, status])
	}
});

app.use(signalR.createListener());

mongoose.set("debug",true);
// starting server ---------------------
var server = app.listen(app.get('port'), function(){
  mongoose.connect(CONSTANTS.MONGODB_URL, function (error) {
      if (error) {
          return Logger.log(error);
      }
      console.log('Application Server started listening at ' + CONSTANTS.APPLICATION_PORT);        
  }); 
});