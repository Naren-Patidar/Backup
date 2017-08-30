<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TermsAndCondition.aspx.cs"
    Inherits="PrintVouchersAtKiosk.TermsAndCondition" Culture="auto" UICulture="auto"
    meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);

        function EnableTC() {
            var enableTC1 = '<%= ConfigurationManager.AppSettings["ShowTermsAndConditons1"].ToString().ToLower() %>';
            var enableTC2 = '<%= ConfigurationManager.AppSettings["ShowTermsAndConditons2"].ToString().ToLower() %>';
            var enableTC3 = '<%= ConfigurationManager.AppSettings["ShowTermsAndConditons3"].ToString().ToLower() %>';
            var enableTC4 = '<%= ConfigurationManager.AppSettings["ShowTermsAndConditons4"].ToString().ToLower() %>';
            var enableTC5 = '<%= ConfigurationManager.AppSettings["ShowTermsAndConditons5"].ToString().ToLower() %>';
            
            document.getElementById('lnkTC1').className = 'TC1highlight';

            if (enableTC1 == 'true') {
                document.getElementById('lnkTC1').style.display = "inline-block";
            }

            if (enableTC2 == 'true') {
                document.getElementById('lnkTC2').style.display = "inline-block";
            }

            if (enableTC3 == 'true') {
                document.getElementById('lnkTC3').style.display = "inline-block";
            }

            if (enableTC4 == 'true') {
                document.getElementById('lnkTC4').style.display = "inline-block";
            }

            if (enableTC5 == 'true') {
                document.getElementById('lnkTC5').style.display = "inline-block";
            }
        }

        function showTC1() {
            document.getElementById('lblTC1').style.display = "block";
            document.getElementById('lblTC2').style.display = "none";
            document.getElementById('lblTC3').style.display = "none";
            document.getElementById('lblTC4').style.display = "none";
            document.getElementById('lblTC5').style.display = "none";

            document.getElementById('lnkTC1').className = 'TC1highlight';
            document.getElementById('lnkTC2').className = 'TC2normal';
            document.getElementById('lnkTC3').className = 'TC3normal';
            document.getElementById('lnkTC4').className = 'TC4normal';
            document.getElementById('lnkTC5').className = 'TC5normal';
        }

        function showTC2() {
            document.getElementById('lblTC1').style.display = "none";
            document.getElementById('lblTC2').style.display = "block";
            document.getElementById('lblTC3').style.display = "none";
            document.getElementById('lblTC4').style.display = "none";
            document.getElementById('lblTC5').style.display = "none";

            document.getElementById('lnkTC1').className = 'TC1normal';
            document.getElementById('lnkTC2').className = 'TC2highlight';
            document.getElementById('lnkTC3').className = 'TC3normal';
            document.getElementById('lnkTC4').className = 'TC4normal';
            document.getElementById('lnkTC5').className = 'TC5normal';
        }

        function showTC3() {
            document.getElementById('lblTC1').style.display = "none";
            document.getElementById('lblTC2').style.display = "none";
            document.getElementById('lblTC3').style.display = "block";
            document.getElementById('lblTC4').style.display = "none";
            document.getElementById('lblTC5').style.display = "none";

            document.getElementById('lnkTC1').className = 'TC1normal';
            document.getElementById('lnkTC2').className = 'TC2normal';
            document.getElementById('lnkTC3').className = 'TC3highlight';
            document.getElementById('lnkTC4').className = 'TC4normal';
            document.getElementById('lnkTC5').className = 'TC5normal';
            
        }

        function showTC4() {
            document.getElementById('lblTC1').style.display = "none";
            document.getElementById('lblTC2').style.display = "none";
            document.getElementById('lblTC3').style.display = "none";
            document.getElementById('lblTC4').style.display = "block";
            document.getElementById('lblTC5').style.display = "none";

            document.getElementById('lnkTC1').className = 'TC1normal';
            document.getElementById('lnkTC2').className = 'TC2normal';
            document.getElementById('lnkTC3').className = 'TC3normal';
            document.getElementById('lnkTC4').className = 'TC4highlight';
            document.getElementById('lnkTC5').className = 'TC5normal';
        }

        function showTC5() {
            document.getElementById('lblTC1').style.display = "none";
            document.getElementById('lblTC2').style.display = "none";
            document.getElementById('lblTC3').style.display = "none";
            document.getElementById('lblTC4').style.display = "none";
            document.getElementById('lblTC5').style.display = "block";

            document.getElementById('lnkTC1').className = 'TC1normal';
            document.getElementById('lnkTC2').className = 'TC2normal';
            document.getElementById('lnkTC3').className = 'TC3normal';
            document.getElementById('lnkTC4').className = 'TC4normal';
            document.getElementById('lnkTC5').className = 'TC5highlight';
        }

    </script>
</head>
<body onload="EnableTC();">
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" Text="Print out your Clubcard Vouchers"
                        meta:resourcekey="lblScanYourClubcardHeaderTextResource1"></asp:Label></em></div>
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgBreadCrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgBreadCrumbsResource1" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentTerms">
                <div class="wrappercentre">
                    <div class="box curved">
                        <h1>
                            <asp:Localize ID="TermsConditions" runat="server" meta:resourcekey="TermsConditionsResource1"></asp:Localize>
                        </h1>
                        <div id="TC1" style="height:400px;">
                            <p style="text-align: justify;">
                                <asp:Label runat="server" meta:resourcekey="TermsandConditionsCompleteTextResource1" ID="lblTC1"></asp:Label>
                                <asp:Label runat="server" meta:resourcekey="TermsandConditionsCompleteTextResource2" style="display:none;" ID="lblTC2"></asp:Label>
                                <asp:Label runat="server" meta:resourcekey="TermsandConditionsCompleteTextResource3" style="display:none;" ID="lblTC3"></asp:Label>
                                <asp:Label runat="server" meta:resourcekey="TermsandConditionsCompleteTextResource4" style="display:none;" ID="lblTC4"></asp:Label>
                                <asp:Label runat="server" meta:resourcekey="TermsandConditionsCompleteTextResource5" style="display:none;" ID="lblTC5"></asp:Label>
                            </p>
                        </div>
                        <div>
                            <a ID="lnkTC1" style="display:none" onclick="showTC1()" class="TC1normal"> </a>
                            <a ID="lnkTC2" style="display:none" onclick="showTC2()" class="TC2normal"> </a>
                            <a ID="lnkTC3" style="display:none" onclick="showTC3()" class="TC3normal"> </a>
                            <a ID="lnkTC4" style="display:none" onclick="showTC4()" class="TC4normal"> </a>
                            <a ID="lnkTC5" style="display:none" onclick="showTC5()" class="TC5normal"> </a>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="startTerms">
                    <div class="back">
                        <asp:LinkButton ID="lnkBack" runat="server" OnClick="Back_Click" meta:resourcekey="lnkBackResource1">
                           
                        </asp:LinkButton>
                    </div>
                </div>
                <!--wrappercentre-->
            </div>
            <!--contentCharter-->
        </div>
        <!--body_wrapper-->
    </div>
    <!--wrapper-->
    </form>
</body>
</html>
