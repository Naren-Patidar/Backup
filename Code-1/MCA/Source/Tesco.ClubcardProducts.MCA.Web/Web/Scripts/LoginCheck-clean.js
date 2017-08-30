/*
The obfuscated version of this file is present here - /Scripts/v_xdfgkeuryt.js
https://javascriptobfuscator.com/Javascript-Obfuscator.aspx
*/
function PSC() {
    var tidvalue = gC("CID");
    if ( tidvalue == null || tidvalue.length < 1) {
        history.go(1);
    }
}


$(function () {
    $(window).unload(function () { $(window).unbind('unload'); });
    PSC();
});
 
function gC(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}
