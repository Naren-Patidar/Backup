<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThankYou.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.ThankYou" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.TYTitle%></title>
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgLogo" runat="server" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" width="234" height="24" />
                <em>
                    <%= Resources.GlobalResources.TYHead%></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.ImgCrumbPrint%>" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentPrint">
                <div class="wrappercentre">
                    <div class="panel curved" style="padding-top: 25px; height: 260px; margin-bottom: 50px;">
                        <strong>
                            <%= Resources.GlobalResources.TYHead%></strong><br />
                        <br />
                        <%= Resources.GlobalResources.TYMsg%>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="arrows">
                        <asp:ImageButton runat="server" ImageUrl="<%$ Resources:GlobalResources,NextCustomerBtn%>"
                            ID="divcancel" OnClick="Cancel_Restart"></asp:ImageButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
