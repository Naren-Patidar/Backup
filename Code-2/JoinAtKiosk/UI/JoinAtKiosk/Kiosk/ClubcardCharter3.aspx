<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClubcardCharter3.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.ClubcardCharter3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= Resources.GlobalResources.CC3Title%></title>
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
                <asp:Image ID="imgLogo" runat="server" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>"
                    Width="234" Height="24" />
                <em>
                    <%= Resources.GlobalResources.CC1Head%></em>
            </div>
        </div>
        <div id="body_wrapper">
            <div id="contentCharter">
                <div class="wrappercentre">
                    <div class="panel1 curved CCPage3Height">
                        <%= Resources.GlobalResources.CCPage3%>
                        <div class="buttons" id="CC3Buttons" runat="server">
                            <div id="pg3CC1" runat="server">
                                <a href="ClubcardCharter1.aspx?page=<%=sPage %>">
                                    <div class="TC1normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                            <div id="pg3CC2" runat="server">
                                <a href="ClubcardCharter2.aspx?page=<%=sPage %>">
                                    <div class="TC2normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                            <div class="TC3highlight">
                                &nbsp;</div>
                            <div id="pg3CC4" runat="server">
                                <a href="ClubcardCharter4.aspx?page=<%=sPage %>">
                                    <div class="TC4normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                            <div id="pg3CC5" runat="server">
                                <a href="ClubcardCharter5.aspx?page=<%=sPage %>">
                                    <div class="TC5normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="backbutton">
                        <asp:ImageButton ID="ImageButton1" OnClick="Back_Click" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>"
                            runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
