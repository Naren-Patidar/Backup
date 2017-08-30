<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintClubcard.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.PrintClubcard" %>]

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />

    <title><%= Resources.GlobalResources.PrintClubcard%></title>

    <script type="text/javascript" language="javascript">
    function PrintClubcard(clubcard) {
        var resources = "<%=resourceStr %>";
        resources = resources.split(',');
        var fontStyle = "<%=fontStyleStr %>";
        fontStyle = fontStyle.split(',');

        var barcodeConfig = "<%=barcodeConfigStr %>";
        barcodeConfig = barcodeConfig.split(',');
       var scard = clubcard;
       var sbarcode = scard.substring(barcodeConfig[0]);
       sbarcode = barcodeConfig[1] + sbarcode;

       window.external.deviceExecutive('Printer', 'AddImage', resources[0]);
       window.external.deviceExecutive('Printer', 'AddText', '******************************|c||');
       window.external.deviceExecutive('Printer', 'SetFont', fontStyle[0]);
       window.external.deviceExecutive('Printer', 'AddText', resources[1]);
       window.external.deviceExecutive('Printer', 'AddText', resources[2]);
       window.external.deviceExecutive('Printer', 'AddText', resources[3]);
       window.external.deviceExecutive('Printer', 'AddText', resources[4]);
       window.external.deviceExecutive('Printer', 'AddText', '******************************|c||');
       window.external.deviceExecutive('Printer', 'AddBarcode', sbarcode + '|Code128|Y|N|C||+10|190|100');
       window.external.deviceExecutive('Printer', 'SetFont', fontStyle[1]);
       window.external.deviceExecutive('Printer', 'AddText', clubcard + '|c||');
       window.external.deviceExecutive('Printer', 'SetFont', fontStyle[0]);
       window.external.deviceExecutive('Printer', 'AddText', '|c||');
       window.external.deviceExecutive('Printer', 'AddText', '******************************|c||');
       window.external.deviceExecutive('Printer', 'AddText', resources[5]);
       window.external.deviceExecutive('Printer', 'AddText', resources[6]);
       window.external.deviceExecutive('Printer', 'AddText', resources[7]);
       window.external.deviceExecutive('Printer', 'AddText', '******************************|c||');
       window.external.deviceExecutive('Printer', 'AddText', resources[8]);
       window.external.deviceExecutive('Printer', 'AddText', resources[9]);
       window.external.deviceExecutive('Printer', 'SetFont', fontStyle[2]);
       window.external.deviceExecutive('Printer', 'AddText', '.|c||+40');
       window.external.deviceExecutive('Printer', 'Print', '');
    }
    function Redirect()
    {
        window.location = 'ThankYou.aspx';
    }

    function Delay()
    {
        setTimeout("Redirect()", 2000);
    }
	window.external.showKeyboard(0);

    </script>


</head>
<body onload='Delay()'>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div id="navigation"> 
            <img src="<%= Resources.GlobalResources.ImageTescoClubcard%>" width="234" height="24">    
                <em><%= Resources.GlobalResources.Message1%></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.crumbPrint%>" alt=""></div>
        </div>
        <div id="body_wrapper">
            <div id="contentPrint">
                <div class="wrappercentre">
                    <div class="panel curved" style="padding-top: 130px; height: 200px">
                       <%= Resources.GlobalResources.Message2%>
                        <p class="italic">
                            <br />
                            <%= Resources.GlobalResources.Message3%></p>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="arrows">
                        <img src="<%= Resources.GlobalResources.ImageArrows%>" alt="" />
                    </div>
                </div>
                <!--wrappercentre-->
            </div>
            <!--contentPrint-->
        </div>
        <!--body_wrapper-->
    </div>
    <!--wrapper-->
    </form>
</body>
</html>
