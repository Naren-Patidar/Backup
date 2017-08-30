<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageForAddress.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.MessageForAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title><%= Resources.GlobalResources.AddressNotFoundMsg%></title>
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
                <em><%= Resources.GlobalResources.WhatIsAddressMsg%></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.AddressBreadCrumb%>" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentPrint">
                <div class="wrappercentre">
                    <div runat="server" id="divpnl" class="panel curved" style="
                        height: 80px;color: #FF0000">
                        <span id="spanmsg" runat="server" style="color: Red"></span>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="arrows">
                        <a href="<%=sPageName %>">
                            <img src="<%= Resources.GlobalResources.BackToAddressBtn%>" alt="" style="border: none" /></a>
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
