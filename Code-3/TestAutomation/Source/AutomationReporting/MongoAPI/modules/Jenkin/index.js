var http = require('http');

var JenkinAPI = function(host, job)
{
	this.Jenkin_Host = host;	
}

JenkinAPI.prototype.GetBuildNumber = function(job, callback){
	var options = {
	  host: this.Jenkin_Host,	  
	  path: '/jenkins/job/MCA/job/'+ job +'/lastBuild/buildNumber',
	  method: 'GET'
	};

	http.request(options, function(res) {
	  console.log('STATUS: ' + res.statusCode);
	  console.log('HEADERS: ' + JSON.stringify(res.headers));
	  res.setEncoding('utf8');
	  res.on('data', function (chunk) {
	    console.log('BODY: ' + chunk);
	    return callback(chunk);
	  });
	}).end();
}

JenkinAPI.prototype.GetBuildInfo = function(job, callback){
	var options = {
	  host: this.Jenkin_Host,	  
	  path: '/jenkins/job/MCA/job/'+ job +'/lastBuild/api/json',
	  method: 'GET'
	};

	http.request(options, function(res) {
	  res.setEncoding('utf8');
	  res.on('data', function (chunk) {	    
	    callback(chunk);
	  });
	}).end();
}

module.exports = JenkinAPI;