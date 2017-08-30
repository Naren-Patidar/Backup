var express = require('express');
var app = express();
var fs = require("fs");
var bodyParser = require('body-parser');
var multer  = require('multer');
var common  = require('./Util/common.js');

// Create application/x-www-form-urlencoded parser
var urlencodedParser = bodyParser.urlencoded({ extended: false })

app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});

// Middleware
//app.use(app.router); // you need this line so the .get etc. routes are run and if an error within, then the error is parsed to the next middleware (your error reporter)
app.use(function(err, req, res, next) {
   console.log("error!!!" + err);
    if(!err) return next(); // you also need this line
    console.log("error!!!" + err);
    res.send(500, {
          "status": "error",
          "message": err.message
       });
});


app.get('/all', function (req, res) {

   common.scrapWebDir();
   console.log("RawData is created");
   var arr = common.proccessRawData('rawData.txt', function(response){
     res.send(response);
   });   //console.log(txt);
    //res.send("Data is processing");
});
app.get('/Search/mn/:mailingNo/country/:countryName', function (req, res) {
  //url-http://127.0.0.1:8081/search/mn/m80/country/PL
  //throw "some exception";
  var mailingNo = req.params.mailingNo;
  var countryName = req.params.countryName;

  common.scrapWebDirWithMailingNumber(mailingNo, countryName, function(response){
    console.log("RawData is created for : " + mailingNo);
    console.log(response);
    if(!(response == "error")){
     var arr = common.proccessRawData('rawData1.txt', countryName, function(response){
      res.send(response);
    });
  } else {
      res.send("error");
  }
  });

});
app.post('/country/:countryName', urlencodedParser, function (req, res) {
  //url-http://127.0.0.1:8081/search/mn/m80/country/PL
  //var mailingNo = req.params.mailingNo;
  var countryName = req.params.countryName;
  var data = req.body.img;
  console.log(data)
  common.getTheRescponseOfImageList(countryName, data, function(response){
    console.log("RawData is created for : " + countryName);
    res.send(response);
  });

});

app.get('/image/name/:imageName/country/:countryName', function (req, res) {
  //console.log(req.params.imageName);
  // http://127.0.0.1:8081/image/name/M80_10500_T.jpg/country/PL
  var imageName=req.params.imageName;
  var countryName=req.params.countryName;
  var url = common.createURL(imageName, countryName);
  console.log(url);
  common.getHttpResponseOfURL(url, function(response){
  console.log(response);
  res.send(response.toString());
  });
});
app.get('/image/name1/:imageName', function (req, res) {
  //console.log(req.params.imageName);
  // http://127.0.0.1:8081/image/name1/M80_10500_T.jpg
  var imageName=req.params.imageName;
  var url = common.createURL(imageName);
  common.getHttpResponseOfURL(url, function(response){
    //res.send(response);
    res.body.first_name1='Narendra';
     res.sendFile( __dirname + "/" + "index.html" );
  });
});

var server = app.listen(8081, function () {

   var host = server.address().address
   var port = server.address().port

   console.log("Image verification app listening at http://%s:%s", host, port)
});
