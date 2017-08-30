<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatIsYourDietryPreferences.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.WhatIsYourDietryPreferences" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.DietryPrefTitle%></title>
    <script src="../Scripts/WhatIsYourDietryPreferences.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <input type="hidden" id="hdnControlList" runat="server" />
    <input type="hidden" id="hdnShowControls" runat="server" />
    <input type="hidden" id="hdnNoOfDietryPreferences" runat="server" />
    <input type="hidden" id="hdnNoOfDietCheckBox" runat="server" />
    <input type="hidden" id="hdnSelectedDietry" runat="server" />
    <input type="hidden" id="hdnSelectedDietChk" runat="server" />
    <div id="header">
        <div id="navigation">
            <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
            <em>
                <asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,DietryPrefTitle%>"></asp:Label></em>
        </div>
        <div id="breadcrumbs">
            <img src="<%= Resources.GlobalResources.DietryPrefBreadCrumb%>" width="1152" height="51"
                alt="" /></div>
    </div>
    <div id="body_wrapper">
        <div id="contentHousehold">
            <div class="wrapperleft" style="width: 100%">
                <p class="italic">
                    <span id="Span2" runat="server">
                        <%= Resources.GlobalResources.DietryPrefLbl01%></span><span id="lblDietOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span>
                </p>
                <div class="focuspaneldiet curved">
                    <div class="inputtextcenter1 whitetext clear" id="divDietryPreferencesLbl" runat="server"
                        style="padding-top: 40px;">
                        <span id="Span3" runat="server" class="titletext">
                            <%= Resources.GlobalResources.DietryPrefLbl02%></span>
                    </div>
                    <asp:UpdatePanel ID="updateDietry" runat="server">
                        <ContentTemplate>
                            <div>
                                <%if (Convert.ToInt16(hdnNoOfDietryPreferences.Value) >= 1)
                                  { %>
                                <asp:LinkButton runat="server" ID="lnkDietary1" OnClientClick="return Dietary_Click('lnkDietary1');"
                                    class="dietary dietary1">
                                </asp:LinkButton>
                                <% }
                                  if (Convert.ToInt16(hdnNoOfDietryPreferences.Value) >= 2)
                                  { %>
                                <asp:LinkButton runat="server" ID="lnkDietary2" OnClientClick="return Dietary_Click('lnkDietary2');"
                                    class="dietary dietary2">
                                </asp:LinkButton>
                                <% }
                                  if (Convert.ToInt16(hdnNoOfDietryPreferences.Value) >= 3)
                                  { %>
                                <asp:LinkButton runat="server" ID="lnkDietary3" OnClientClick="return Dietary_Click('lnkDietary3');"
                                    class="dietary dietary3">
                                </asp:LinkButton>
                                <% }
                                  if (Convert.ToInt16(hdnNoOfDietryPreferences.Value) >= 4)
                                  { %>
                                <asp:LinkButton runat="server" ID="lnkDietary4" OnClientClick="return Dietary_Click('lnkDietary4');"
                                    class="dietary dietary4">
                                </asp:LinkButton>
                                <% }
                                  if (Convert.ToInt16(hdnNoOfDietryPreferences.Value) >= 5)
                                  { %>
                                <asp:LinkButton runat="server" ID="lnkDietary5" OnClientClick="return Dietary_Click('lnkDietary5');"
                                    class="dietary dietary5">
                                </asp:LinkButton>
                                <% } %>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <p style="clear: both">
                    </p>
                    <div class="inputtext">
                        &nbsp;</div>
                    <asp:UpdatePanel ID="UpdateDie" runat="server">
                        <ContentTemplate>
                            <div>
                                <%if (Convert.ToInt16(hdnNoOfDietCheckBox.Value) >= 1)
                                  { %>
                                <div>
                                    <asp:LinkButton runat="server" OnClientClick="return Dietary_ChkBox_Click('lnkChk1');"
                                        ID="lnkChk1" class="input92 inputbluesquare" Text="<%$ Resources:GlobalResources,DietryChk1%>"
                                        Style="text-decoration: none">                                      
                                    </asp:LinkButton>
                                </div>
                                <% }
                                  if (Convert.ToInt16(hdnNoOfDietCheckBox.Value) >= 2)
                                  { %>
                                <div>
                                    <asp:LinkButton runat="server" ID="lnkChk2" class="input92 inputbluesquare" Text="<%$ Resources:GlobalResources,DietryChk2%>"
                                        Style="text-decoration: none" OnClientClick="return Dietary_ChkBox_Click('lnkChk2');">                                
                                    </asp:LinkButton>
                                </div>
                                <% } %>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="buttons">
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
                <div class="buttonitems last paddingtop-13" runat="server" id="divconfirm" >
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,DietryToNextPage%>" ID="lnkNext" runat="server"
                             OnClientClick="return Validate(document.getElementById('hdnControlList'));"
                            OnClick="NextPage_Click" />
                    </div>
                <div class="buttonitems last paddingtop-13" runat="server" id="divsummary" style="display: none;">
                    <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary"
                        runat="server" OnClientClick="return Validate(document.getElementById('hdnControlList'));"
                        OnClick="NextPage_Click" />
                </div>
            </div>
        </div>
        <!--contentHousehold-->
    </div>
    <!--body_wrapper-->
    </form>
</body>
</html>
