<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactPreferences.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.ContactPreferences" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.ContactPrefTitle%>
    </title>
    <script src="../Scripts/Common.js" type="text/javascript" language="javascript"></script>
    <script type="text/javascript" language="javascript">
        var resources;
        function Loading() {
            resources = document.getElementById('hdnResources').value;
            resources = resources.split(',');
            document.getElementById('lbltitle').innerHTML = resources[0];
              <% if(Request.QueryString["ctrlID"]!=null) {%>
                var ctrl = '<% =Request.QueryString["ctrlID"]%>';
                if (ctrl.toUpperCase() == 'PROMOCODE') {
                    Nextcontrol_Click();
		    FocusObject('txtPromotionalCode');
         	    document.getElementById('txtPromotionalCode').value = document.getElementById('txtPromotionalCode').value;
                }
            <%} %>
        }

        function ContactPref_Click(objName) {
               document.getElementById('lbltitle').innerHTML = resources[0];
            if (document.getElementById(objName).className == 'inputbluesquareCP') {
                document.getElementById(objName).className = 'inputbluesquareCPtick';
                objName = objName.replace('lnk', '');
                document.getElementById('hdnSelectedChk').value = document.getElementById('hdnSelectedChk').value + objName;
            }
            else {
                document.getElementById(objName).className = 'inputbluesquareCP';
                objName = objName.replace('lnk', '');
                document.getElementById('hdnSelectedChk').value = document.getElementById('hdnSelectedChk').value.replace(objName, '');
            }

            if (document.getElementById('hdnShowPromoCode').value == 'Y') {
                document.getElementById('divConfirm').style.display = 'none';
                document.getElementById('divNext').style.display = 'block';
                document.getElementById('divNext').setAttribute('style', 'margin-top: 15px;'); 
            }
            return false;
        }
        function Nextcontrol_Click() {
            var txt = resources[1];
            document.getElementById('lbltitle').innerHTML = txt;
            document.getElementById('divNext').style.display = 'none';
            document.getElementById('divConfirm').style.display = 'block';
            document.getElementById('divConfirm').setAttribute('style', 'margin-top: 0px;margin-left: -20px;'); 
            FocusObject('txtPromotionalCode');
            document.getElementById('txtPromotionalCode').focus();
            return false;
        }
    </script>
</head>
<body onload="Loading()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <input type="hidden" id="hdnShowPromoCode" runat="server" />
    <input type="hidden" id="hdnSelectedChk" runat="server" />
    <input type="hidden" id="hdnResources" runat="server" />
    
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>
                    <asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,ContactPrefTitle%>"></asp:Label></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.ContactPrefBreadCrumb%>" width="1152" height="51"
                    alt="" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentHousehold">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="wrappercentreUpdated">
                            <p class="italic" style="margin-top: -40px">
                                <span id="Span1" runat="server">
                                    <%= Resources.GlobalResources.ContactPrefLbl01%></span>
                            </p>
                            
                            <asp:Table ID="ContactPrefTable"  runat="server" CssClass="contactpreftbl">
                                <asp:TableRow ID="TableRow1" runat="server" CssClass="contactprefrow1">
                                    <asp:TableCell ID="TableCell1" runat="server" CssClass="contactprefcell11"></asp:TableCell>
                                    <asp:TableCell ID="TableCell2" runat="server" CssClass="contactprefcell12">
                                        <asp:Label ID="lblMail" runat="server" Text="<%$ Resources:GlobalResources,CPLblMail%>" ></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell3" runat="server"  CssClass="contactprefcell13" >
                                        <asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:GlobalResources,CPLblEmail%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell4" runat="server"  CssClass="contactprefcell14">
                                        <asp:Label ID="lblPhone" runat="server" Text="<%$ Resources:GlobalResources,CPLblPhone%>" ></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell5" runat="server" CssClass="contactprefcell15">
                                        <asp:Label ID="lblSMS" runat="server" Text="<%$ Resources:GlobalResources,CPLblSMS%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="TableRow2" CssClass="contactprefrow2">
                                    <asp:TableCell ID="TableCell6" CssClass="contactprefcell21">
                                        <asp:Label ID="lblTescogrp" runat="server" Text="<%$ Resources:GlobalResources,CPLblTescoGrp%>"
                                           CssClass="contactpreflbl" />
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell7" runat="server" CssClass="contactprefcell22">
                                        <asp:LinkButton ID="lnkTescoGroupMail" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkTescoGroupMail');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell8" runat="server" CssClass="contactprefcell23">
                                        <asp:LinkButton ID="lnkTescoGroupEmail" runat="server" class="inputbluesquareCP"
                                            OnClientClick="return ContactPref_Click('lnkTescoGroupEmail');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell9" runat="server" CssClass="contactprefcell24">
                                        <asp:LinkButton ID="lnkTescoGroupPhone" runat="server" class="inputbluesquareCP"
                                            OnClientClick="return ContactPref_Click('lnkTescoGroupPhone');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell10" runat="server" CssClass="contactprefcell25">
                                        <asp:LinkButton ID="lnkTescoGroupSMS" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkTescoGroupSMS');"></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="TableRow3"  CssClass="contactprefrow3">
                                    <asp:TableCell ID="TableCell11" align="left" runat="server" CssClass="contactprefcell31">
                                        <asp:Label ID="lblPartnerthirdparty" runat="server" Text="<%$ Resources:GlobalResources,CPLblPartner%>"
                                              CssClass="contactpreflbl2"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="PrefTableCell5" runat="server" CssClass="contactprefcell32">
                                        <asp:LinkButton ID="lnkPartnerMail" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkPartnerMail');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell12" runat="server"  CssClass="contactprefcell33">
                                        <asp:LinkButton ID="lnkPartnerEmail" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkPartnerEmail');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell13" runat="server"  CssClass="contactprefcell34">
                                        <asp:LinkButton ID="lnkPartnerPhone" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkPartnerPhone');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell14" runat="server"  CssClass="contactprefcell35">
                                        <asp:LinkButton ID="lnkPartnerSMS" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkPartnerSMS');"></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="TableRow4" runat="server"  CssClass="contactprefrow4">
                                    <asp:TableCell ID="TableCell15" runat="server" CssClass="contactprefcell41">
                                        <asp:Label ID="lblResearch" runat="server" Text="<%$ Resources:GlobalResources,CPLblResearch%>"
                                              CssClass="contactpreflbl2"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell16" runat="server" CssClass="contactprefcell42">
                                        <asp:LinkButton ID="lnkResearchMail" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkResearchMail');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell17" runat="server" CssClass="contactprefcell43">
                                        <asp:LinkButton ID="lnkResearchEmail" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkResearchEmail');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell18" runat="server" CssClass="contactprefcell44">
                                        <asp:LinkButton ID="lnkResearchPhone" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkResearchPhone');"></asp:LinkButton>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell19" runat="server" CssClass="contactprefcell45">
                                        <asp:LinkButton ID="lnkResearchSMS" runat="server" class="inputbluesquareCP" OnClientClick="return ContactPref_Click('lnkResearchSMS');"></asp:LinkButton>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <div class="focuspanelUpdated curved paddingtop-10" id="divUKPostCode1" runat="server" 
                                style="margin-left: 800px; margin-top: -170px;">
                                <div class="inputtextpromocode whitetext paddingtop10" id="divUKPostCode2" runat="server">
                                    <%= Resources.GlobalResources.PromotionalCode%><br /><span id="lblPromOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                                <div class="input322Updated paddingtop10" id="divUKPostCode3" runat="server" > 
                                    <div class="textbox">
                                        <asp:TextBox runat="server" CssClass="text" ID="txtPromotionalCode" Font-Size="X-Large"  onclick="return Nextcontrol_Click();"></asp:TextBox></div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="buttonsUpdated" style="margin-top: -360px">
                    <div class="buttonitems">
                        <a href="#">
                            <asp:ImageButton ID="btnBack" runat="server" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>"
                                alt="" OnClick="btnBack_click" /></a>
                    </div>
                    <div class="buttonitems">
                        <div class="cancelStart">
                            <asp:LinkButton runat="server" ID="divcancel" class="cancelStart" OnClick="Cancel_Restart"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="buttonspacer5" runat="server" id="divspacer">
                        &nbsp;</div>
                    <div class="buttonitems">
                        <asp:Panel ID="pnlClubcardCharter" runat="server">
                          <asp:LinkButton runat="server" ID="LinkButton1" class="charter" OnClick="CC_Click"></asp:LinkButton>
                           
                        </asp:Panel>
                    </div>
                    <div class="buttonitems last paddingtop-13"  style="margin-top: 15px"  runat="server"
                        id="divNext">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,ContactPrefToPromoCode%>"
                            ID="ImageButton1" runat="server" OnClientClick="return Nextcontrol_Click();" />
                    </div>
                    <div class="buttonitems last" style="display: none;" runat="server"
                        id="divConfirm">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,ContactPrefToNextPage%>"
                            ID="imgConfirmAddress" runat="server" OnClick="NextPage_Click" />
                    </div>
                </div>
            </div>
            <!--contentHousehold-->
        </div>
    </div>
    <!--body_wrapper-->
    </form>
</body>
</html>
