const https = require('https');

var baseUrl = 'https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/';

const getHttpResponseOfURL = function(imageName){
  return imageName;
};


var createURL = function(imageName){
  return baseUrl + imageName;
};
var getReponse = function(url){
  //console.log(url);
  getHttpResponseOfURL(url, function(response){
  return response;
  });
  //return url;
}

var getStatus = function(){

$https({
    method: 'GET',
    url: 'https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/M80_10500_T.jpg'
}).then(function successCallback(response) {
    var status = response.status;
  //  console.log(status); // gives the status 200/401
    var statusText = response. statusText;
    //console.log(status); // HTTP status text of the response
    return status;
}, function errorCallback(response) {
return '1';
});
}

function checkHttpStatus(url) {
    $.ajax({
        type: "GET",
        data: {},
        url: url,
        error: function(response) {
            //alert(url + " returns a " + response.status);
            return '1';
        }, success() {
            //alert(url + " Good link");
            return '2';
        }
    });
}


module.exports.getHttpResponseOfURL = getHttpResponseOfURL;
exports.createURL=createURL;
exports.getStatus=checkHttpStatus;
