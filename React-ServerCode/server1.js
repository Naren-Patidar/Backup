var express = require('express');
var app = express();
var fs = require("fs");
var bodyParser = require('body-parser');
var multer  = require('multer');
//app.set('view engine','jade');

app.use(express.static('learnings'));
app.use(bodyParser.urlencoded({ extended: false }));
//app.use(multer({ dest: '/tmp/'}));

// This responds with "Hello World" on the homepage

var scrap = require('scrap');

scrap('https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/', function(err, $) {
  //console.log($('body').text().trim());
  var re = new RegExp('\w*.jpg');
  var re2 = /\w*.jpg/gi

  var x = $('pre').text().trim().match(re);
  var rawData = $('pre').html().trim()
//  console.log(x);
//  console.log(x[0][2]);
var arr = rawData.match(re2).toString().split(",");
console.log(arr.length);
console.log('https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/'+ arr[0]);

var https = require('https');
https.get('https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/test.jpg', function(res) {
  console.log("statusCode: ", res.statusCode); // <======= Here's the status code
//  console.log("headers: ", res.headers);

  res.on('data', function(d) {
    //process.stdout.write(d);
  });

}).on('error', function(e) {
  console.error(e);
});

  fs.writeFile('rawData.txt', rawData.match(re2),  function(err) {
   if (err) {
      return console.error(err);
   }
 });
 //Google
});

app.get('/index.html', function (req, res) {
   //res.sendFile( __dirname + "/" + "index.html" );

   const testFolder = 'https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/';
   const fs = require('fs');

fs.readdirSync(testFolder).forEach(file => {
  console.log(file);
})

})



app.get('/process_get', function (req, res) {
   // Prepare output in JSON format
   response = {
      first_name:req.query.first_name,
      last_name:req.query.last_name
   };
   console.log(response);
   res.end(JSON.stringify(response));
})

app.post('/file_upload', function (req, res) {
   //console.log(req.files.file.filename);
   console.log(req.files.file.path);
   console.log(req.file.type);
   var file = __dirname + "/tmp/" + req.file.name;

   fs.readFile( req.file.path, function (err, data) {
      fs.writeFile(file, data, function (err) {
         if( err ){
            console.log( err );
            }else{
               response = {
                  message:'File uploaded successfully',
                  filename:req.file.name
               };
            }
         console.log( response );
         res.end( JSON.stringify( response ) );
      });
   });
})

var server = app.listen(8081, function () {

   var host = server.address().address
   var port = server.address().port

   console.log("Example app listening at http://%s:%s", host, port)
})
