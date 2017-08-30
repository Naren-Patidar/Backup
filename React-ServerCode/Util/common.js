
var express = require('express');
var app = express();
var fs = require("fs");
var scrap = require('scrap');
var https = require('https');
const countryUrl = {
  PL: "https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/",
  CZ: "https://zabezpeceni.itesco.cz/MojeClubcard/Ucet/I/CouponImages/",
  SK: "https://s.itesco.sk/mojaclubcard/ucet//I/couponimages/",
  HU: "https://s.tesco.hu/clubcardfiokom/fiokom/I/couponimages/"
};
var baseUrl = 'https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/';

  var scrapWebDir = function(){
  scrap(baseUrl, function(err, $) {
    //var re = new RegExp('\w*.jpg');
    var re = /\w*.jpg/gi
    var rawData = $('pre').html().trim();
    rawData = rawData.match(re);
    createRawDataFile(rawData, 'rawData.txt');
  });
};

var createRawDataFile = function(rawData,fileName,callback){
  fs.writeFile(fileName, rawData,  function(err) {
   if (err) {
      return "error";
   }
   else {
     return callback("success");
   }

 });
};

var proccessRawData = function(dataFile, countryName, callback){
  var rawData = fs.readFileSync(dataFile);
  //fs.close();
  var arr = rawData.toString().split(",");
  var array = [];

        arr.forEach(function(listItem, index){
          var url = createURL(listItem, countryName);
          getHttpResponseOfURL(url, function(response) {
              array.push(response);
              //console.log(array.length);
            if(arr.length == array.length){
              console.log('index=' + index);
              console.log('source index=' + arr.length);
              console.log('Data array=' + array.length);
              //createRawDataFile(array,'response.txt')
              //fs.writeFileSync('response.txt',array);
              //fs.close();
              return callback(array);
            }
          });
        });
};


var createURL = function(imageName, countryName){
  var url = countryUrl[countryName];
  return url + imageName;
};

var splitRawData = function(rawData){
  var arr = rawData.toString().split(",");
  return arr;
};

var getHttpResponseOfURL = function(url, callback){
  var req;
try{
   req = https.get(url, function(res){
     console.log(url);
    return callback(res.statusCode);
  });
  req.end();
} catch(err){
   //req.end();
   return callback("error");
   }
};

var scrapWebDirWithMailingNumber = function(mailingNo, countryName, callback){
var targetUrl = countryUrl[countryName];
try{
scrap(targetUrl, function(err, $) {
  if(err)
  throw err;
  //var re = new RegExp('\w*.jpg');
  console.log(mailingNo);
  console.log(targetUrl);
  var re = mailingNo + /\w*.jpg/gi
  var regEx = new RegExp(mailingNo + '\\w*.jpg', 'gi');
  console.log($('body').text());
  var rawData = $('pre').html().trim();
  rawData = rawData.match(regEx);
  console.log(rawData);
  createRawDataFile(rawData, 'rawData1.txt', callback);
});
} catch(er){
  return callback("error");
}

};

var scrapFullDir = function(countryName, data, callback){
var targetUrl = countryUrl[countryName];
var rawData;
var imgNotPresent = [];
try{
scrap(targetUrl, function(err, $) {
  if(err)
  throw err;
  console.log(targetUrl);
  var re = /\w*.jpg/gi
  rawData = $('pre').html().trim();
  rawData = rawData.match(re);
  console.log(rawData.length);
  var arr = data.toString().split(",");
  console.log(rawData[0]);
  arr.forEach(function(listItem, index){
    var i;
    var isMatched = 0;
    for(i = 0;i < rawData.length; i++)
    {
      if(listItem.toUpperCase() == rawData[i].toUpperCase())
      {
        console.log(listItem);
        isMatched = 1;
        break;
      }
    }
    if(isMatched == 0){
    imgNotPresent.push(listItem + " ");
    }
  });
    return callback(JSON.stringify(imgNotPresent));
});
}
catch(ex){
  return callback('error')
}
};

//--Get the response code of list of images

var getTheRescponseOfImageList = function(countryName, data, callback){
var targetUrl = countryUrl[countryName];
var rawData;
var imgListWithResponse = [];
var imgNotPresent=[];
var arr=[];
try{
  var arr1 = data.toString().split("\n");
  arr1.forEach(function(listItem, index){
    if(listItem!=""){
      arr.push(listItem);
    }
  });
  console.log(arr.length);
  console.log(arr1.length);
  arr.forEach(function(listItem, index){
  //consol.log("item=" + listItem);
    var imageUrl=targetUrl + listItem
    getHttpResponseOfURL(imageUrl, function(response){

        imgListWithResponse.push(listItem + ":" + response);
        if(response != 200)
        {
          imgNotPresent.push(listItem + " ");
        }
        if(imgListWithResponse.length == arr.length)
        {
          createRawDataFile(imgNotPresent, '../Client/imgNotPresentOnServer.txt', function(){

          });
          return callback(JSON.stringify(imgNotPresent));
        }
    });
  });
}
catch(ex){
  return callback('error')
}
};


module.exports.scrapWebDir = scrapWebDir;
//exports.createRawDataFile = createRawDataFile;
module.exports.proccessRawData = proccessRawData;
exports.createURL = createURL;
exports.getHttpResponseOfURL = getHttpResponseOfURL;
exports.scrapWebDirWithMailingNumber = scrapWebDirWithMailingNumber;
exports.scrapFullDir = scrapFullDir;
exports.getTheRescponseOfImageList=getTheRescponseOfImageList;
