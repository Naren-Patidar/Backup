<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageForAddress.aspx.cs"
    Inherits="PrintVouchersAtKiosk.MessageForAddress" Culture="auto" UICulture="auto"
    meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>Message For Address</title>
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);
    </script>
</head>
<body>
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
            <div id="contentCancel">
                <div class="boxbckgrd curved">
                    <div class="box">
                        <p class="red" style="padding-bottom: 10px; padding-top: 25px;">
                            <span id="spanmsg" runat="server" style="color: Red"></span>
                        </p>
                    </div>
                </div>
                <div class="startError">
                    <div class="back">
                        <a href="<%=sPageName %>">
                            <div style="color: Red;">
                                <asp:Literal ID="lblError" runat="server" meta:resourcekey="lblErrorResource1"></asp:Literal>
                            </div>
                        </a>
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
