<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactPreferencesUK.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.ContactPreferencesUK" %>

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
            // debugger;
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
       // debugger;
            document.getElementById('lbltitle').innerHTML = resources[0];
            if (document.getElementById(objName).className == 'input92 inputbluesquaretick') {
                document.getElementById(objName).className = 'input92 inputbluesquare';
                objName = objName.replace('lnk', '');
                document.getElementById('hdnSelectedChk').value = document.getElementById('hdnSelectedChk').value.replace(objName, '');
            }
            else {
                document.getElementById(objName).className = 'input92 inputbluesquaretick';
                objName = objName.replace('lnk', '');
                document.getElementById('hdnSelectedChk').value = document.getElementById('hdnSelectedChk').value + objName;
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
                        <div class="wrappercentre">
                            <p class="italic">
                                <span id="Span1" runat="server">
                                    <%= Resources.GlobalResources.ContactPrefLbl01%></span></p>
                            <div class="wrapperLeftUpdated">
                                <div class="inputtextcentreUpdated" style="margin-top: 10px;">
                                    <span id="Span2" runat="server">
                                        <%= Resources.GlobalResources.ContactPrefLbl02%></span></div>
                                <div>
                                    <asp:LinkButton runat="server" ID="lnkTescoProduct" Style="margin-top: 0px;" class="input92 inputbluesquare"
                                        OnClientClick="return ContactPref_Click('lnkTescoProduct');"></asp:LinkButton>
                                </div>
                                <div class="inputtextcentreUpdated" style="margin-top: 10px">
                                    <span id="Span3" runat="server">
                                        <%= Resources.GlobalResources.ContactPrefLbl03%></span></div>
                                <div>
                                    <asp:LinkButton runat="server" ID="lnkTescoPartnerInfo" Style="margin-top: 0px;"
                                        class="input92 inputbluesquare" OnClientClick="return ContactPref_Click('lnkTescoPartnerInfo');"></asp:LinkButton>
                                </div>
                                <div class="inputtextcentreUpdated" style="margin-top: 10px">
                                    <span id="Span4" runat="server">
                                        <%= Resources.GlobalResources.ContactPrefLbl04%></span></div>
                                <div>
                                    <asp:LinkButton runat="server" ID="lnkCustomerResearch" Style="margin-top: 0px;"
                                        class="input92 inputbluesquare" OnClientClick="return ContactPref_Click('lnkCustomerResearch');"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="focuspanelUpdated curved paddingtop-10" id="divUKPostCode1" runat="server" 
                                style="margin-left: 750px; margin-top: -210px;">
                                <div class="inputtextpromocode whitetext paddingtop10" id="divUKPostCode2" runat="server">
                                    <%= Resources.GlobalResources.PromotionalCode%><br /><span id="lblPromOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                                <div class="input322Updated paddingtop10" id="divUKPostCode3" runat="server">
                                    <div class="textbox">
                                        <asp:TextBox runat="server" CssClass="text" ID="txtPromotionalCode" Font-Size="X-Large" onclick="return Nextcontrol_Click();"></asp:TextBox></div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="buttonsUpdated" style="margin-top: -25px">
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
                    <div class="buttonspacer6" runat="server" id="divspacer">
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
                <!--buttons-->
            </div>
            <!--contentAddress-->
        </div>
        <!--body_wrapper-->
    </div>
    <!--wrapper-->
    </form>
</body>
</html>
