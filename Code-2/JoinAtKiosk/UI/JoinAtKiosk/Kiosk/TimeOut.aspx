<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeOut.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.TimeOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Time Out</title>
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
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
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>Time Out</em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.ConformPageBrudCrumb%>"/></div>
        </div>
        <div id="body_wrapper">
            <div id="contentPrint">
                <div class="wrappercentre">
                    <div class="panel curved" style="padding-top: 40px; height: 135px; margin-bottom: 50px;">
                        Sorry to interrupt, but no one has<br />
                        touched the screen in a while.<br />
                    </div>
                    <div class="clear">
                    </div>
                    <div class="arrows">
                        <asp:ImageButton runat="server" ImageUrl="<%$ Resources:GlobalResources,NextCustomerBtn%>" ID="divcancel"
                            OnClick="Cancel_Restart"></asp:ImageButton>
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
