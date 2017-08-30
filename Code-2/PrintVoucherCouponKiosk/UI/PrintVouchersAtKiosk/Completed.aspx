<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Completed.aspx.cs" Inherits="PrintVouchersAtKiosk.Printed" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector">
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper" class="arrow arrowleft">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" 
                    meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" 
                    Text="Print out your Clubcard Vouchers" 
                    meta:resourcekey="lblScanYourClubcardHeaderTextResource1"></asp:Label></em></div>
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgBreadCrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgBreadCrumbsResource1" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentCancel">
                <div class="boxbckgrd curved">
                    <div class="box">
                        <p>
                            <asp:Localize ID="FinishedPrinting" runat="Server" 
                                meta:resourcekey="FinishedPrintingResource1"></asp:Localize>
                        </p>
                        <p class="thanks">
                            <asp:Localize ID="ThankYou" runat="server" meta:resourcekey="ThankYouResource1"></asp:Localize></p>
                    </div>
                </div>
                 <div class="startError1">
                 <div class="greybtn">
                 <div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                 <asp:LinkButton ID="lnkPrintOthers" runat="server" OnClick="lnkPrintOthers_Click" meta:resourcekey="lnkPrintOthersResource1"
                    Text="" Width="175px" ></asp:LinkButton>
                 </div>
                 </div>
                 <div class="startError2">
                  <div class="confirm1">
                    <asp:LinkButton ID="lnkNewCustomer" runat="server" 
                        OnClick="lnkNewCustomer_Click" meta:resourcekey="lnkNewCustomerResource1"></asp:LinkButton>
                </div>
                 </div>
            </div>
            <div class="clear22">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
