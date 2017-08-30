<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Printing.aspx.cs" Inherits="PrintVouchersAtKiosk.Printing" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector">
  <%--  <script language="javascript" src="Tokens.js"></script>--%>
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);

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
            window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText7").ToString() %>|c||+10');
            window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText8").ToString() %>|c||+3');
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

            </script>
</head>

<body onload='setTimeout("Print()", 2000)'>
    <form id="form1" runat="server">
<div id="wrapper" class="arrow arrowleft">


        <div id="header">
            <div id="navigation"><img src="images/title-5.png" width="838" height="37"></div>
            <div id="breadcrumbs"><img src="images/crumb4.png"width="1152" height="45"></div>
        </div>



    <div id="body_wrapper">
        <div id="contentPrint">
            <div class="boxbckgrd curved">
                <div class="box">
                    <p class="printing"> <asp:Label ID="lblTxt1" runat="server" Text="Now printing..." 
                            meta:resourcekey="lblTxt1Resource1"></asp:Label></p>
                    <p class="vouchernumberlarge">
                        <asp:Label ID="lblExchangeTokens" runat="server" Text="0" 
                            meta:resourcekey="lblExchangeTokensResource1"></asp:Label>
                    </p>
                     <p><asp:Label ID="lblTxt2" runat="server" Text="Clubcard Vouchers" 
                             meta:resourcekey="lblTxt2Resource1"></asp:Label></p>
                </div>
            </div>
        </div>
    </div>

    <div class="clearPrint"></div>
    <div id="footer">
        <div class="greybtn marginleft50">
            <!--<asp:LinkButton ID="lnkCancel" runat="server" onclick="lnkCancel_Click"><span>Cancel and start again</span></asp:LinkButton>-->
        </div>
    </div>

</div>        
    

</form>
</body>
</html>
