<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClubcardEntry.aspx.cs"
    Inherits="PrintVouchersAtKiosk.ClubcardEntry" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector">
</head>
<body onload="document.getElementById('txtClubcard').focus();document.getElementById('txtClubcard').value = document.getElementById('txtClubcard').value;">
    <form id="form1" runat="server">
    <!-- <asp:ScriptManager ID="ScriptManager1" runat="server" />-->
    <div id="wrapper">
        <!--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>-->
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" Text="Print out your Clubcard Vouchers"
                        meta:resourcekey="lblScanYourClubcardHeaderTextResource1"></asp:Label></em>
            </div>
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgBreadCrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgBreadCrumbsResource1" /></div>
        </div>
        <script language="javascript">
            function DisableContinue() {

                val = document.getElementById('txtClubcard').value;
                if (val.length >= 1) {
                    document.getElementById('lnkConfirm').disabled = "";

                    document.getElementById('pnlConfirm').className = 'confirm';
                }
                else {
                    document.getElementById('lnkConfirm').disabled = "disabled";

                    document.getElementById('pnlConfirm').className = 'confirm inactive';
                }
            }
        </script>
        <div id="body_wrapper">
            <div id="contentCCEntry">
                <asp:Image ID="imgClubcardNumber" runat="server" AlternateText="img Clubcard Number"
                    Width="344px" Height="226px" meta:resourcekey="imgClubcardNumberResource1" />
                <div class="textboxCC">
                    <div class="textCC">
                        <asp:Label ID="lblTypeYourClubcardNumber" runat="server" Text="Type in your Clubcard number"
                            meta:resourcekey="lblTypeYourClubcardNumberResource1"></asp:Label></div>
                    <asp:TextBox ID="txtClubcard" runat="server" CssClass="textCC" size="35" onkeyup="DisableContinue()"
                        meta:resourcekey="txtClubcardResource1"></asp:TextBox>
                    <asp:Panel ID="pnlError" runat="server" Visible="False" meta:resourcekey="pnlErrorResource1">
                        <div class="error">
                            <asp:Literal ID="lblError" runat="server" meta:resourcekey="lblErrorResource1"></asp:Literal>
                        </div>
                    </asp:Panel>
                </div>
                <asp:Image ID="imgClubcardNumber2" runat="server" AlternateText="img ClubcardNumber2"
                    Width="259px" Height="250px" meta:resourcekey="imgClubcardNumber2Resource1" />
            </div>
        </div>
        <div class="clear" id="footer" style="padding-bottom: 10px;">
            <div class="back">
                <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" meta:resourcekey="lnkBackResource1"></asp:LinkButton>
            </div>
            <div class="greybtn">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" meta:resourcekey="lnkCancelResource1"></asp:LinkButton>
            </div>
            <asp:Panel ID="pnlConfirm" runat="server" CssClass="confirm inactive" meta:resourcekey="pnlConfirmResource1">
                <asp:LinkButton ID="lnkConfirm" runat="server" OnClick="lnkConfirm_Click" meta:resourcekey="lnkConfirmResource1">                 
                </asp:LinkButton>
            </asp:Panel>
        </div>
        <!--   </ContentTemplate>
        </asp:UpdatePanel>-->
        <div class="clear keyboard">
            <script type="text/javascript" language="javascript">
                window.external.showKeyboardTemplate(1);
            </script>
        </div>
    </div>
    </form>
</body>
</html>
