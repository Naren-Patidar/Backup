<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorMessage.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.ErrorMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.ErrorScreenTitle%></title>
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);

        //Since all the pages have html image controls.
        function GetImageUrl() {
            var url = document.getElementById('hdnImageUrl').value;
            document.getElementById('imgBreadCrum').src = url;
        }

    </script>
</head>
<body onload="GetImageUrl()">
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>
                    <%= Resources.GlobalResources.ErrorPage%></em>
            </div>
            <div id="breadcrumbs">
                <img id="imgBreadCrum" runat="server" />
            </div>
        </div>
        <div id="body_wrapper">
            <div id="contentPrint">
                <div class="wrappercentre">
                    <div runat="server" id="divpnl" class="panel curved" style="height: 220px; color: #FF0000">
                        <span id="spanmsg" runat="server" style="color: Red"></span>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:HiddenField ID="hdnImageUrl" runat="server" />
                    </div>
                    <div class="arrows" id="CancelAndStartAgain" runat="server">
                        <a href="CancelAndRestart.aspx" style="text-decoration: none">
                            <div class="cancelStart startagain" style="margin-left: 171px">
                                &nbsp;</div>
                        </a>
                    </div>
                    <div class="arrows" id="Back" runat="server">
                        <a href="<%=sPageName %>">
                            <img src="<%= Resources.GlobalResources.CC1BackBtn%>" alt=""
                                style="border: none" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
