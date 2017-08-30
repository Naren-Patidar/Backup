<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintingCoupons.aspx.cs" Inherits="PrintVouchersAtKiosk.PrintingCoupons" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <title></title>
   <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector"/>
    <script src="Scripts/date.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);

        var line2FontName; var line2FontSize; var line2FontWeight; var line2Align;
        var line3FontName; var line3FontSize; var line3FontWeight; var line3Align;
        var line4FontName; var line4FontSize; var line4FontWeight; var line4Align;
        var line5FontName; var line5FontSize; var line5FontWeight; var line5CustomerNameAlign; var line5ClubcardNoAlign;
        var line6FontName; var line6FontSize; var line6FontWeight; var line6Align;
        var line7FontName; var line7FontSize; var line7FontWeight; var line7Align;
        var line9FontName; var line9FontSize; var line9FontWeight; var line9Align;
        var line10FontName; var line10FontSize; var line10FontWeight; var line10Align;
        var footerFontName; var footerFontSize; var footerFontWeight; var footerAlign;

        function SetPrintFormat() {
            line2FontName = '<%= ConfigurationSettings.AppSettings["Line2FontName"].ToString() %>';
            line2FontSize = '<%= ConfigurationSettings.AppSettings["Line2FontSize"].ToString() %>';
            line2FontWeight = '<%= ConfigurationSettings.AppSettings["Line2FontWeight"].ToString() %>';
            line2Align = '<%= ConfigurationSettings.AppSettings["Line2Align"].ToString() %>';

            line3FontName = '<%= ConfigurationSettings.AppSettings["Line3FontName"].ToString() %>';
            line3FontSize = '<%= ConfigurationSettings.AppSettings["Line3FontSize"].ToString() %>';
            line3FontWeight = '<%= ConfigurationSettings.AppSettings["Line3FontWeight"].ToString() %>';
            line3Align = '<%= ConfigurationSettings.AppSettings["Line3Align"].ToString() %>';

            line4FontName = '<%= ConfigurationSettings.AppSettings["Line4CouponDescFontName"].ToString() %>';
            line4FontSize = '<%= ConfigurationSettings.AppSettings["Line4CouponDescFontSize"].ToString() %>';
            line4FontWeight = '<%= ConfigurationSettings.AppSettings["Line4CouponDescFontWeight"].ToString() %>';
            line4Align = '<%= ConfigurationSettings.AppSettings["Line4CouponDescAlign"].ToString() %>';

            line5FontName = '<%= ConfigurationSettings.AppSettings["Line5FontName"].ToString() %>';
            line5FontSize = '<%= ConfigurationSettings.AppSettings["Line5FontSize"].ToString() %>';
            line5FontWeight = '<%= ConfigurationSettings.AppSettings["Line5FontWeight"].ToString() %>';
            line5CustomerNameAlign = '<%= ConfigurationSettings.AppSettings["line5CustomerNameAlign"].ToString() %>';
            line5ClubcardNoAlign = '<%= ConfigurationSettings.AppSettings["line5ClubcardNoAlign"].ToString() %>';

            line6FontName = '<%= ConfigurationSettings.AppSettings["Line6FontName"].ToString() %>';
            line6FontSize = '<%= ConfigurationSettings.AppSettings["Line6FontSize"].ToString() %>';
            line6FontWeight = '<%= ConfigurationSettings.AppSettings["Line6FontWeight"].ToString() %>';
            line6Align = '<%= ConfigurationSettings.AppSettings["Line6Align"].ToString() %>';

            line7FontName = '<%= ConfigurationSettings.AppSettings["Line7FontName"].ToString() %>';
            line7FontSize = '<%= ConfigurationSettings.AppSettings["Line7FontSize"].ToString() %>';
            line7FontWeight = '<%= ConfigurationSettings.AppSettings["Line7FontWeight"].ToString() %>';
            line7Align = '<%= ConfigurationSettings.AppSettings["Line7Align"].ToString() %>';

            line9FontName = '<%= ConfigurationSettings.AppSettings["Line9FontName"].ToString() %>';
            line9FontSize = '<%= ConfigurationSettings.AppSettings["Line9FontSize"].ToString() %>';
            line9FontWeight = '<%= ConfigurationSettings.AppSettings["Line9FontWeight"].ToString() %>';
            line9Align = '<%= ConfigurationSettings.AppSettings["Line9Align"].ToString() %>';

            line10FontName = '<%= ConfigurationSettings.AppSettings["Line10FontName"].ToString() %>';
            line10FontSize = '<%= ConfigurationSettings.AppSettings["Line10FontSize"].ToString() %>';
            line10FontWeight = '<%= ConfigurationSettings.AppSettings["Line10FontWeight"].ToString() %>';
            line10Align = '<%= ConfigurationSettings.AppSettings["Line10Align"].ToString() %>';

            footerFontName = '<%= ConfigurationSettings.AppSettings["footerFontName"].ToString() %>';
            footerFontSize = '<%= ConfigurationSettings.AppSettings["footerFontSize"].ToString() %>';
            footerFontWeight = '<%= ConfigurationSettings.AppSettings["footerFontWeight"].ToString() %>';
            footerAlign = '<%= ConfigurationSettings.AppSettings["footerAlign"].ToString() %>';
        }

        function GetDateAndTime() {
            var currentDateTime;
            var dateFormat;

            //Format current date & time
            dateFormat = '<%= System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern %>';
            currentDateTime = new Date();

            var currentDateFormatted = currentDateTime.toString(dateFormat);
            var currentTimeFormatted = currentDateTime.getHours() + ":";
            var minutes = currentDateTime.getMinutes();

            if (minutes < 10) {
                currentTimeFormatted = currentTimeFormatted + "0";
            }

            currentTimeFormatted = currentTimeFormatted + minutes;

            return currentDateFormatted + "     " + currentTimeFormatted;
        }

        function PrintCoupon(couponDescription, custName, clubcardNo, onlineCode, eanNo, expiryDate, storeId, tillId) {
            window.external.deviceExecutive('Printer', 'AddImage', '<%= GetLocalResourceObject("LogoImagePath").ToString() %>|c||+8|165|47');

            window.external.deviceExecutive('Printer', 'SetFont', line2FontName + '|' + line2FontSize + '|' + line2FontWeight);
            window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("CouponText1").ToString() %>' + line2Align);

            window.external.deviceExecutive('Printer', 'SetFont', line3FontName + '|' + line3FontSize + '|' + line3FontWeight);
            window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText2").ToString() %>' + line3Align);
            window.external.deviceExecutive('Printer', 'AddText', '----------------------------------|c||');

            window.external.deviceExecutive('Printer', 'SetFont', line4FontName + '|' + line4FontSize + '|' + line4FontWeight);
            window.external.deviceExecutive('Printer', 'AddText', couponDescription + line4Align);

            window.external.deviceExecutive('Printer', 'SetFont', line5FontName + '|' + line5FontSize + '|' + line5FontWeight);
            window.external.deviceExecutive('Printer', 'AddText', custName + line5CustomerNameAlign);
            window.external.deviceExecutive('Printer', 'AddText', clubcardNo + line5ClubcardNoAlign);

            if (onlineCode != "") {
                window.external.deviceExecutive('Printer', 'SetFont', line6FontName + '|' + line6FontSize + '|' + line6FontWeight);
                window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("CouponText2").ToString() %>' + line6Align);

                window.external.deviceExecutive('Printer', 'SetFont', line7FontName + '|' + line7FontSize + '|' + line7FontWeight);
                window.external.deviceExecutive('Printer', 'AddText', onlineCode + line7Align);
            }

            if (eanNo != "") {
                window.external.deviceExecutive('Printer', 'AddBarcode', eanNo + '|Code128|Y|Y|C||+10|190|100');
            }

            window.external.deviceExecutive('Printer', 'SetFont', line9FontName + '|' + line9FontSize + '|' + line9FontWeight);
            window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("VoucherText6").ToString() %>' + expiryDate + line9Align);

            window.external.deviceExecutive('Printer', 'SetFont', line10FontName + '|' + line10FontSize + '|' + line10FontWeight);
            window.external.deviceExecutive('Printer', 'AddText', '<%= GetLocalResourceObject("CouponText3").ToString() %>' + line10Align);
            window.external.deviceExecutive('Printer', 'AddText', '----------------------------------|c||');

            window.external.deviceExecutive('Printer', 'SetFont', footerFontName + '|' + footerFontSize + '|' + footerFontWeight);
            window.external.deviceExecutive('Printer', 'AddText', GetDateAndTime() + '     ' + storeId + '     ' + tillId + footerAlign);
            window.external.deviceExecutive('Printer', 'AddNewPage', '');
        }

      </script> 
</head>
<body onload='setTimeout("Print()", 2000);SetPrintFormat();'>
    <form id="form1" runat="server">
<div id="wrapper" class="arrow arrowleft">


        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" 
                    meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" 
                    Text="Print out your Clubcard Coupons" 
                    meta:resourcekey="lblScanYourClubcardHeaderTextResource"></asp:Label>
                </em>
            </div>
            <div id="breadcrumbs"><asp:Image Width="1152px" runat="server" Height="45px" meta:resourcekey="imgCrumbHeader"/></div>
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
           
        </div>
    </div>

</div>  
    </form>
</body>
</html>
