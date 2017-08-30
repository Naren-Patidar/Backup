<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatIsYourContactDetails.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.WhatIsYourContactDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%= Resources.GlobalResources.EmailTitleMsg%></title>
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <script src="../Scripts/WhatIsYourContactDetails.js" type="text/javascript"></script>
    <script src="../Scripts/Common.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        resources = "<%=resourceStr %>";
        resources = resources.split(',');
     </script>
</head>
<body onload="Loading()">
    <form id="form1" runat="server">
    <input type="hidden" id="hdnPostRegExpList" runat="server" />
    <input type="hidden" id="hdnUkPostcode" runat="server" />
    <input type="hidden" id="hdnPostCode" runat="server"  />
    <input type="hidden" id="hdnCountryCode" runat="server"  />
    <input type="hidden" id="hdnValidateString" runat="server"  />
    <input type="hidden" id="hdnErrorCtrl" runat="server"  />
     <%var showAddressControls = lblCurrentField.Value;%>
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>
                    <asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,EmailTitle%>"
                        ></asp:Label></em>
                        </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.EmailBreadCrumb%>" width="1152" height="45"/>
            </div>     
        </div>
        <div id="body_wrapper1">
            <div id="contentAddress">
             <asp:HiddenField  ID="lblCurrentField"  runat="server"/>

             <p class="italic"></p>
                <p style="padding-top: 5px;">
                </p>
                
                  <div class="wrapperleft">
                  <%
                      if (showAddressControls.Contains("EMAIL"))
                      { %>
                 
                     <div class="focuspanel curved paddingtop-10" id="Email1" onclick="return Email1_Click()"
                        runat="server" >
                        <div class="inputtext twolines whitetext paddingtop10" id="Email2" runat="server">
                           <%= Resources.GlobalResources.EmailAddress%><br /><span class="smalltext" id="lblEmailOption" runat="server"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="Email3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtEmailId" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                    <% }
                        if (showAddressControls.Contains("MOBILENO")) 
                       {%>

                     <div class="grey" id="MobileNo1" onclick="return MobileNo_Click()"
                        runat="server">
                        <div class="inputtext" id="MobileNo2" runat="server">
                           <%= Resources.GlobalResources.PhoneNumber%><br /><span class="smalltext" id="lblMobileOption" runat="server"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="MobileNo3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtMobileNo" Font-Size="X-Large"  onKeyPress="return AlloNumbersOnly(this);" ></asp:TextBox></div>
                        </div>
                    </div>

                 </div>
                
                <%} %>
                <div class="wrapperright">
     
                  <%  if (showAddressControls.Contains("EVENINGNO"))
                    { %>
                    <div class="grey" id="EveningNo1" onclick="return EveningNo_Click()"
                        runat="server" >
                        <div class="inputtext" id="EveningNo2" runat="server">
                           <%= Resources.GlobalResources.EveningNumber%><br /><span class="smalltext" id="lblEvNOption" runat="server"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="EveningNo3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtEveningNo" Font-Size="X-Large"  onKeyPress="return AlloNumbersOnly(this);"  ></asp:TextBox></div>
                        </div>
                    </div>
                <%}
                    if (showAddressControls.Contains("DAYTIMENO"))
                    {
                    %>
                    
                     <div class="grey" id="DayTimeNo1" onclick="return DayTimeNo_Click()"
                        runat="server" >
                        <div class="inputtext" id="DayTimeNo2" runat="server">
                           <%= Resources.GlobalResources.DayTimeNumber%><br /><span class="smalltext" id="lblDNOption" runat="server"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="DayTimeNo3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtDayTimeNo" Font-Size="X-Large"  onKeyPress="return AlloNumbersOnly(this);"  ></asp:TextBox></div>
                        </div>
                    </div>
                  
                     <%} 
                   
                    %>
                     
                 </div>
                 
                <div class="buttons" style="width: 100%">
                     <div class="buttonitems">
                        <a href="WhatIsYourTitleandName.aspx">
                            <asp:ImageButton ID="btnBack" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>" runat="server" OnClientClick="return PreviousControl()" OnClick="btnBack_click" />
                        </a>
                    </div>
                    <div class="buttonitems">                       
                        <div class="cancelStart">
                            <asp:LinkButton runat="server" ID="LinkButton1" class="cancelStart" OnClick="lnkCancel_Click"></asp:LinkButton>
                        </div>                       
                    </div>
                      <div class="buttonspacer1" id="divspacer">
                        &nbsp;</div>
                     <div class="buttonitems last">
                        <div id="divnext" >
                        </div>
                        <div id="divs" runat="server" style="display: none">
                            <asp:LinkButton runat="server" ID="divnextbutton" class="nextbutton"  OnClientClick="return Next_Value()" ></asp:LinkButton>
                        </div>
                    </div>
                     <div class="buttonitems last paddingtop-13" runat="server" id="divconfirm" style="display: none">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,ConfirmContactBtn%>" ID="imgConfirmAddress" runat="server"
                             OnClick="Confirm_ContactClick"/>
                    </div>
                     <div class="buttonitems last paddingtop-13" runat="server" id="divsummary" style="display: none">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary" runat="server" 
                             OnClick="Confirm_ContactClick"/>
                    </div>
                </div>

            <!--body_wrapper-->
            </div>
        </div>
        </div>
        <!--wrapper-->
    </form>
</body>
</html>
