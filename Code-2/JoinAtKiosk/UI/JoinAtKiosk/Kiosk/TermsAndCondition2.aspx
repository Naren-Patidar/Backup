<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TermsAndCondition2.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.TermsAndCondition2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.TCTitle%></title>
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
                    <%= Resources.GlobalResources.TCHead%></em>
            </div>
        </div>
        <div id="body_wrapper">
            <div id="contentCharter">
                <div class="wrappercentre">
                    <div class="panel curved TCPage2Height">
                        <%= Resources.GlobalResources.TCPage2 %>
                        <div class="buttons" id="TC2Buttons" runat="server">
                            <div id="pg2TC1" runat="server">
                                <a href="TermsAndCondition.aspx?page=<%=sPage %>">
                                    <div class="TC1normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                            <div class="TC2highlight">
                                &nbsp;</div>
                            <div id="pg2TC3" runat="server">
                                <a href="TermsAndCondition3.aspx?page=<%=sPage %>">
                                    <div class="TC3normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                            <div id="pg2TC4" runat="server">
                                <a href="TermsAndCondition4.aspx?page=<%=sPage %>">
                                    <div class="TC4normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                            <div id="pg2TC5" runat="server">
                                <a href="TermsAndCondition5.aspx?page=<%=sPage %>">
                                    <div class="TC5normal" style="text-decoration: none">
                                        &nbsp;</div>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="backbutton">
                        <asp:ImageButton ID="imgBack" OnClick="Back_Click" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>"
                            runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
