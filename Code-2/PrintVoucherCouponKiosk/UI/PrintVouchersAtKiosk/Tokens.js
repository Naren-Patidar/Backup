//var status = 'EMPTY';
//var timeout;
//var pos = 0;

//var todo = [
// "PrintD0098('10.00','11-09-2011','9684150021219864997729','7492113','0','0');",
// "PrintD0098('10.00','11-09-2011','9684150021219864997730','7492113','0','0');",
// "PrintD0098('10.00','11-09-2011','9684150021219864997731','7492113','0','0');"]


//function PrintTokens()
//{
//    //alert('status' + status);
//    if (status != 'EMPTY')
//    {

//        if (status == 'RETRACTING' || status == 'RETRACTED')
//        {
//            location.replace('Error.aspx');
//        }
//        else
//        {
//            timeout = setTimeout("PrintTokens()", 100);
//        }
//    }
//    else
//    {
//        //alert('pos' + pos);
//        //alert('todo line' + todo[pos]);
//        eval(todo[pos]);
//        window.external.deviceExecutive('Printer', 'Print', '');
//        pos = pos + 1;
//        //alert('new pos' + pos);
//        //alert('todo length' + todo.length);
//        if (pos < todo.length)
//        {
//            timeout = setTimeout("PrintTokens()", 100);
//        }
//    }
//}

//function deviceCallback(sWhat)
//{

//    var startOfStatusTag = sWhat.indexOf('<strPrinterPresenterStatus>');
//    if (startOfStatusTag >= 0)
//    {
//        var endofStatusTag = sWhat.indexOf('</strPrinterPresenterStatus>');

//        var tempStatus = sWhat.substr(startOfStatusTag + '<strPrinterPresenterStatus>'.length, endofStatusTag - (startOfStatusTag + '<strPrinterPresenterStatus>'.length));

//        alert(tempStatus);

//        if (tempStatus = 'EMPTY' && status == 'RETRACTING')
//        {
//            status = 'RETRACTED';
//        }
//        else
//        {
//            status = tempStatus;
//        }

//        clearTimeout(timeout);
//    }

//}

function GetDateAndTime() {
    var currentDateTime;

    //Format current date & time
    currentDateTime = new Date();
    var currentDateFormatted = currentDateTime.getDate() + "-" + (currentDateTime.getMonth() + 1) + "-" + currentDateTime.getFullYear();
    var currentTimeFormatted = currentDateTime.getHours() + ":";
    var minutes = currentDateTime.getMinutes();

    if (minutes < 10) {
        currentTimeFormatted = currentTimeFormatted + "0";
    }

    currentTimeFormatted = currentTimeFormatted + minutes;

    return currentDateFormatted + "     " + currentTimeFormatted;

}
function PrintVoucher(value, custname, clubcardno, onlinecode, ean, expirydate, storeId, tillId) {
    window.external.deviceExecutive('Printer', 'AddImage', 'Templates\\TescoLogo.jpg|c||+8|165|47');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|18|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText1").ToString() %>|c||+8');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|12|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText2").ToString() %>|c||');
    window.external.deviceExecutive('Printer', 'AddText', '****************************|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|20|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '**** ' + value + ' ****|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|12|Regular');
    window.external.deviceExecutive('Printer', 'AddText', custname + '|c||+8');
    window.external.deviceExecutive('Printer', 'AddText', clubcardno + '|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|8|Regular');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText3").ToString() %> |c||+8');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText4").ToString() %>|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|12|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText5").ToString() %>|c||+8');
    window.external.deviceExecutive('Printer', 'AddText', onlinecode + '|c||');
    window.external.deviceExecutive('Printer', 'AddBarcode', ean + '|Code128|Y|Y|C||+10|190|100');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText6").ToString() %>' + expirydate + '|c||+10');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|8|Regular');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText7").ToString()|c||+10');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText8").ToString()|c||+3');
    window.external.deviceExecutive('Printer', 'AddText', '********************|c||');
    window.external.deviceExecutive('Printer', 'AddText', GetDateAndTime() + '     ' + storeId + '     ' + tillId + '|c||+8');
    window.external.deviceExecutive('Printer', 'AddNewPage', '');
}

function PrintCoupon(couponDescription, custname, clubcardno, onlinecode, ean, expirydate, storeId, tillId) {
    window.external.deviceExecutive('Printer', 'AddImage', 'Templates\\TescoLogo.jpg|c||+8|165|47');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|18|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("CouponText1").ToString() %>|c||+8');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|12|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText2").ToString() %>|c||');
    window.external.deviceExecutive('Printer', 'AddText', '****************************|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|20|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '**** ' + couponDescription + ' ****|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|12|Regular');
    window.external.deviceExecutive('Printer', 'AddText', custname + '|c||+8');
    window.external.deviceExecutive('Printer', 'AddText', clubcardno + '|c||');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|8|Regular');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("CouponText2").ToString() %> |c||+8');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|12|Bold');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText5").ToString() %>|c||+8');
    window.external.deviceExecutive('Printer', 'AddText', onlinecode + '|c||');
    window.external.deviceExecutive('Printer', 'AddBarcode', ean + '|Code128|Y|Y|C||+10|190|100');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText6").ToString() %>' + expirydate + '|c||+10');
    window.external.deviceExecutive('Printer', 'SetFont', 'Arial|8|Regular');
    window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("CouponText3").ToString() %>|c||+10');
    window.external.deviceExecutive('Printer', 'AddText', '********************|c||');
    window.external.deviceExecutive('Printer', 'AddText', GetDateAndTime() + '     ' + storeId + '     ' + tillId + '|c||+8');
    window.external.deviceExecutive('Printer', 'AddNewPage', '');
}


