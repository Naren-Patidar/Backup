<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatIsYourAdress.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.WhatIsYourAdress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <script src="../Scripts/WhatIsYourAdress.js" type="text/javascript"></script>
    <script src="../Scripts/Common.js" type="text/javascript"></script>
    <title><%= Resources.GlobalResources.WhatIsAddressMsg%></title>
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
    <input type="hidden" id="hdnErrorMsg" runat="server"  />
    <input type="hidden" id="hdnErrorCtrl" runat="server"  />
     <%var showAddressControls = lblCurrentField.Value;%>
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>
                    <asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,AddressLine6Tit%>"
                        ></asp:Label></em>
                        </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.AddressBreadCrumb%>" width="1152" height="45"/>
            </div>     
        </div>
        <div id="body_wrapper1">
            <div id="contentAddress">
             <asp:HiddenField  ID="lblCurrentField"  runat="server"/>

             <p class="italic"><%= Resources.GlobalResources.PostalCodemsg%></p>
               <%-- <p style="padding-top: 5px;">
                </p>--%>
                <%
                    if(showAddressControls.Contains("UKPOSTCODE")) 
                  { %>
                 
                  <div class="wrapperleft">
                 <div class="focuspanel curved paddingtop-10" id="divUKPostCode1" onclick="return UKPostcode_Click()"  runat="server">
                        <div class="inputtext twolines whitetext paddingtop10" id="divUKPostCode2" runat="server">
                             <%= Resources.GlobalResources.PostCode%><br /><span id="lblUKPOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="divUKPostCode3" runat="server">
                            <div class="textbox" onkeyup="Postcode_Check()">
                                <asp:TextBox runat="server" CssClass="text" ID="txtUKPostCode" Font-Size="X-Large" MaxLength="9" ></asp:TextBox></div>
                        </div>
                    </div>
                   <p id="plokkAdreessGrey" style="padding-top: 128px;" runat="server">
                        <img src="<%= Resources.GlobalResources.LookUpAddressGrey%>" alt="" style="border: none" /></p>
                    <p id="plokkAdreess" style="padding-top: 128px; display: none" runat="server">
                        <asp:ImageButton ID="btnPostCode" runat="server" ImageUrl="<%$ Resources:GlobalResources,LookUpAddress%>" OnClick="Next_FindAddress"  />
                    </p>
                   
                 </div>
                
                <%} %>
               <div class="wrapperright">
     
                  <%  if (showAddressControls.Contains("ADDRESSLINE1"))
                    { %>
                    <div class="grey" id="AddressLine1div1" onclick="return AddressLine1_Click()"
                        runat="server" >
                        <div class="inputtext" id="AddressLine1div2" runat="server">
                           <%= Resources.GlobalResources.AddressLine1%><br /><span id="lblAd1Opt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="AddressLine1div3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtAddressLine1" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                <%} 
                    if (showAddressControls.Contains("ADDRESSLINE2"))
                    {
                    %>
                    
                     <div class="grey" id="AddressLine2div1" onclick="return AddressLine2_Click()"
                        runat="server" >
                        <div class="inputtext" id="AddressLine2div2" runat="server">
                           <%= Resources.GlobalResources.AddressLine2%><br /><span id="lblAd2Opt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="AddressLine2div3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtAddressLine2" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                  
                     <%} 
                    if (showAddressControls.Contains("ADDRESSLINE3"))
                    {
                    %>
                     <div class="grey" id="AddressLine3div1" onclick="return AddressLine3_Click()"
                        runat="server" >
                        <div class="inputtext" id="AddressLine3div2" runat="server">
                           <%= Resources.GlobalResources.AddressLine3%><br /><span id="lblAd3Opt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="AddressLine3div3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtAddressLine3" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                    
                     <%} %>
                 </div>
                   <% if (showAddressControls.Contains("GRPOSTCODE"))
                      {
                    %>
                 <div class="wrapperright">
                 <% if (showAddressControls.Contains("ADDRESSLINE4"))
                    {
                    %>
                     <div class="grey" id="AddressLine4div1" onclick="return AddressLine4_Click()"
                        runat="server" >
                        <div class="inputtext" id="AddressLine4div2" runat="server">
                           <%= Resources.GlobalResources.AddressLine4%><br /><span id="lblAd4Opt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="AddressLine4div3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtAddressLine4" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                     <%}
                    if (showAddressControls.Contains("ADDRESSLINE5"))
                    {
                    %>
                     <div class="grey" id="AddressLine5div1" onclick="return AddressLine5_Click()"
                        runat="server">
                        <div class="inputtext" id="AddressLine5div2" runat="server">
                           <%= Resources.GlobalResources.AddressLine5%><br /><span id="lblAd5Opt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="AddressLine5div3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtAddressLine5" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                  
                     <%}
                    if (showAddressControls.Contains("GRPOSTCODE"))
                    {
                    %>
                     <div class="grey" id="divGrPostcode1" onclick="return AddressLine6_Click()"
                        runat="server">
                        <div class="inputtext" id="divGrPostcode2" runat="server">
                           <%= Resources.GlobalResources.ChPostcode%><br /><span id="lblAd6Opt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="divGrPostcode3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtGrPostcode" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                  <%} %>
       
                </div>
                <%} %>
                
                <div class="buttons" style="width: 100%">
                     <div class="buttonitems">
                        <a href="WhatIsYourTitleandName.aspx">
                            <asp:ImageButton ID="btnBack" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>" runat="server" OnClientClick="return PreviousControl()" OnClick="btnBack_click" />
                        </a>
                    </div>
                    <div class="buttonitems">                       
                        <div class="cancelStart">
                            <asp:LinkButton runat="server" ID="LinkButton1" class="cancelStart" OnClick="lnkCancel_Click" ></asp:LinkButton>
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
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,ConformAddressBtn%>" ID="imgConfirmAddress" runat="server"
                            OnClick="Confirm_AddressClick" />
                    </div>
                     <div class="buttonitems last paddingtop-13" runat="server" id="divsummary" style="display: none">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary" runat="server" 
                              OnClick="Confirm_AddressClick"/>
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
