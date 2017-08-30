//const $ = require('scrap');
import $ from 'jquery';
import https from 'https';


const baseApiURL='http://127.0.0.1:8081/';

var downloadPDF= function()
{
  $.fileDownload('App.jsx')
    .done(function () { alert('File download a success!'); })
    .fail(function () { alert('File download failed!'); });
}

var getHttpResponseOfURL = function(url,callback){
  url = baseApiURL + 'image/name/M80_10500_T.jpg';
  //alert('getHttpResponseOfURL');
  var req = https.get(url, function(res) {
    //alert(res);

    res.on('data', function (chunk) {
      return callback(chunk);
    });

    });
  req.end();
};

const checkHttpStatus = function checkHttpStatus(imageName,countryName,callback) {
    //http://127.0.0.1:8081/image/name/M80_10500_T.jpg/country/CZ
    var url= baseApiURL + 'image/name/'+ imageName + "/country/" + countryName;
    $.ajax({
        type: "GET",
        data: {},
        url: url,
        withCredentials: true,
        error: function(response) {

            return callback(response);
        },
        success :function(response) {

            return callback(response);
        }
    });
}

const checkHttpStatusForMailingNumber = function checkHttpStatus(mailingNo, country, callback) {
    var url= baseApiURL + 'search/mn/'+ mailingNo + '/country/' + country;
    $.ajax({
        type: "GET",
        data: {},
        url: url,
        withCredentials: true,
        error: function(response) {

            return callback(response);
        },
        success :function(response) {

            return callback(response);
        }
    });
}

const ImageVarification = function ImageVarification(rawData, country, callback) {
    var url= baseApiURL + 'country/' + country;
    // alert(url)
    //alert(rawData);
    $.ajax({
        type: "Post",
        data:{img : rawData},
        url: url,
        withCredentials: true,
        error: function(response) {
            return callback(response);
        },
        success :function(response) {

            return callback(response);
        }
    });
}

function getStatus(url,state) {
  alert(url);
  url= baseApiURL + 'image/name/M80_10500_T.jpg'
        var request = new XMLHttpRequest();
        request.open("GET", url , true);
    request.onreadystatechange = function() {
      alert(request.readyState);
        if (request.readyState == 4){
            //request.status;//this contains the status code
              alert(request.status);
              state["response"] = request.status;
              return request.status;
        }
    };
request.send(null);
}

exports.checkHttpStatus = checkHttpStatus;
exports.checkHttpStatusForMailingNumber=checkHttpStatusForMailingNumber;
exports.downloadPDF=downloadPDF;
exports.ImageVarification=ImageVarification;
