<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clubcard.aspx.cs" Inherits="PrintVouchersAtKiosk.Clubcard"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);
    </script>
</head>
<body>
    <form id="form1" runat="server" >
    <div id="wrapper" >
        <script type="text/javascript">
            var fC = 0;
            registerMSR();
            runAnimation();
            function registerMSR() {
                window.external.extensionCall_Async2('VX700MCR', 'REGISTERCALLBACK;', 'MSRCallback');
                window.external.extensionCall('VX700MCR', 'SETDISPLAY:Insert card to begin');
            }

            function MSRCallback(sResult) {

                if (sResult == 'CARD REMOVED') {

                    window.external.extensionCall('VX700MCR', 'SETDISPLAY:Insert card to begin');
                }

                if (sResult.substr(0, 11) == 'CARDNUMBER:') {
                    document.getElementById("hdnClubcard").value = sResult.substr(11);
                    form1.submit();
                }
            }

            function deviceCallback(sWhat) {

                var startOfBarcodeTag = sWhat.indexOf('<Barcode>');
                var CultureCode = document.getElementById("hdnCountryCode").value;
                if (startOfBarcodeTag >= 0) {
                    var endofBarcodeTag = sWhat.indexOf('</Barcode>');

                    var barcode = sWhat.substr(startOfBarcodeTag + 10, endofBarcodeTag - (startOfBarcodeTag + 10));
                    var ScannedClubcardPrefix = '<%= ConfigurationSettings.AppSettings["ScannedClubcardPrefix"].ToString() %>'
                    var ActualClubcardPrefix = '<%= ConfigurationSettings.AppSettings["ActualClubcardPrefix"].ToString() %>'
                    if (ScannedClubcardPrefix != "" && ActualClubcardPrefix != "") {
                        var ScannedPrefixCode = barcode.substr(0, ScannedClubcardPrefix.length);
                        var strippedBarcode = barcode.substr(ScannedClubcardPrefix.length);
                        if (ScannedPrefixCode == ScannedClubcardPrefix) {
                            barcode = ActualClubcardPrefix + strippedBarcode;
                        }
                    }
                    document.getElementById("hdnClubcard").value = barcode;
                    form1.submit();
                }

            }

            function resetTimeout() {
                window.external.setTimeout(30);
            }

            function runAnimation() {
                fC = fC + 0.5;
                setChannel(16, Math.floor((Math.sin(fC) + 1) * 49));
                setChannel(16, Math.floor((Math.sin(fC / 2) + 1) * 49));
                setChannel(16, Math.floor((Math.sin(fC / 5) + 1) * 49));
                setTimeout('runAnimation()', 2);
            }

            function setChannel(ch, va) {
                window.external.extensionCall('KioskIO', 'Parameter:level;Scope:' + ch + ';Value:' + va + ';');
                window.external.extensionCall('KioskIO', 'PollController;')
            }
            resetTimeout();
        </script>
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" runat="server" AlternateText="Tesco Clubcard Logo"
                    Width="234px" Height="24px" meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" Text="Print out your Clubcard Vouchers"
                        meta:resourcekey="lblScanYourClubcardHeaderTextResource1"></asp:Label></em>
            </div>
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgbreadcrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgbreadcrumbsResource1" />
            </div>
        </div>
        <div id="body_wrapper">
            <div id="contentClubcard">
                <div class="clear5a">
                </div>
        <%--        <div class="holder1">
                    &nbsp;</div>--%>
                <div class="info-section">
                    <asp:Image ID="Image1" runat="server" AlternateText = "img Tesco Clubcard Scan" 
                           width="421" height="242" meta:resourcekey="imgTescoClubcardScanResource11"/>
                       <p> <asp:Label ID="lblBarcodeReader" runat="server" Text="Scan the barcode on the back of your Clubcard or key fob below"
                            meta:resourcekey="lblBarcodeReaderResource1"></asp:Label>
                    
                        </p>
                </div>
                <div class="info-section">
                    <asp:Image ID="imgTescoClubcardScan" runat="server" AlternateText = "img Tesco Clubcard Scan" 
                           width="421" height="132" meta:resourcekey="imgTescoClubcardScanResource1"/>
                             <p> <asp:Label ID="lblBarcodeReader2" runat="server" Text="If you have a Tesco Clubcard Credit Card<br/> or Privilege card, insert or swipe your card "
                            meta:resourcekey="lblBarcodeReaderResource2"></asp:Label>
                    
                        </p>
                </div>
                <div id="divTypeInYourClubcardNumber" class="holder3" style="width: 250px; height: 167px;"
                    runat="server">
                    <em>
                        <asp:Label ID="lblProblemScanning" runat="server" Text="Problem scanning?" meta:resourcekey="lblProblemScanningResource1"></asp:Label></em>
                   
                        <div class="greybtn">
                            <asp:LinkButton ID="lnkTypeInClubcard" runat="server" OnClick="lnkTypeInClubcard_Click"
                                meta:resourcekey="lnkTypeInClubcardResource1" Text="
                            &lt;span  class=&quot;typein&quot;&gt;TYPE IN CLUBCARD&lt;br /&gt;NUMBER&lt;/span&gt;
                            "></asp:LinkButton>
                        </div>
                    
                    <em>
                        <asp:Label ID="lblCustomerServiceDesk" runat="server" Text="Or go to the<br />Customer Service Desk"
                            meta:resourcekey="lblCustomerServiceDeskResource1"></asp:Label></em>
                    <asp:HiddenField ID="hdnClubcard" runat="server" />
                    <asp:HiddenField ID="hdnCountryCode" value="EN-GB" runat="server" />
                </div>
                <%--<asp:Panel ID="pnlHaveTescoClubcardCreditCardOrPrevilegeCard" runat="server" CssClass="holder4"
                    meta:resourcekey="pnlHaveTescoClubcardCreditCardOrPrevilegeCardResource1">
                    <p>
                        <asp:Image ID="imgTescoClubcardCreditCard" runat="server" 
                            AlternateText="img Tesco Clubcard Creditcard" width="215px" height="160px" 
                            meta:resourcekey="imgTescoClubcardCreditCardResource1" /></p>
                    <em>
                        <asp:Label ID="lblHaveTescoClubcard" runat="server" Text="Have a Tesco Clubcard Credit Card or Privilege Card?"
                            meta:resourcekey="lblHaveTescoClubcardResource1"></asp:Label></em>
                    <p>
                        <asp:Label ID="lblInsertAndRemove" runat="server" Text="Insert and remove your card from the slot below"
                            meta:resourcekey="lblInsertAndRemoveResource1"></asp:Label></p>
                    <p>
                        <asp:Image ID="imgSmallArrow" runat="server" AlternateText="img Small Arrow" 
                            width="79px" height="75px" meta:resourcekey="imgSmallArrowResource1"/></p>
                </asp:Panel>--%>
            </div>
        </div>
        <div class="clear5">
        </div>
        <div id="footer">
            <div class="greybtn">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" meta:resourcekey="lnkCancelResource1"
                    Text="&lt;span class=&quot;cancelStart&quot;&gt;CANCEL&lt;/span&gt;&lt;span class=&quot;cancelStartagain&quot;&gt;and start again&lt;/span&gt;"></asp:LinkButton>
            </div>
            <div class="greybtn">
                <asp:LinkButton ID="lnkTerms" runat="server" OnClick="lnkTerms_Click" meta:resourcekey="lnkTermsResource1"
                    Text="&lt;span class=&quot;terms&quot;&gt;TERMS &amp;amp;&lt;br /&gt;CONDITIONS&lt;/span&gt;"></asp:LinkButton>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
