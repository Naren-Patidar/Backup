<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalSubmitionScreen.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.FinalSubmitionScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.FinalSubmitionTitle%></title>
        <script type="text/javascript" language="javascript">
            window.external.showKeyboard(0);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%var showNameSection = lblNameSection.Value;%>
    <%var showAddressSection = lblAddressSection.Value;%>
    <%var showEmailSection = lblEmailSection.Value;%>
    <%var showPreferredLanguageSection = lblPreferredLanguageSection.Value;%>
    <%var showDOBHHAgesSection = lblDOBHHAges.Value;%>
    <%var showDietaryPreferencesSection = lblDietaryPreferences.Value;%>
    <%var showRaceSection = lblRaceSection.Value;%>
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgLogo" runat="server" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>"
                    Width="234" Height="24" />
                <em>
                    <%= Resources.GlobalResources.Confirmation%></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.ImgCrumbPrint%>" alt="" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentPrint">
                <asp:HiddenField ID="lblConfiguredSections" runat="server" />
                <asp:HiddenField ID="lblNameSection" runat="server" />
                <asp:HiddenField ID="lblAddressSection" runat="server" />
                <asp:HiddenField ID="lblEmailSection" runat="server" />
                <asp:HiddenField ID="lblPreferredLanguageSection" runat="server" />
                <asp:HiddenField ID="lblDOBHHAges" runat="server" />
                <asp:HiddenField ID="lblDietaryPreferences" runat="server" />
                <asp:HiddenField ID="lblRaceSection" runat="server" />
                <asp:HiddenField ID="hdnJoinRouteCode" runat="server" />
                <%  if (showNameSection.ToString() != string.Empty && showNameSection.ToString() != null)
                    { %>
                <div class="holdertitle">
                    <div class="leftcontent1 first" style="padding-top: 0px;">
                        <%= Resources.GlobalResources.Name%>
                        <br />
                        <b>
                            <asp:Label ID="lblName" runat="server"></asp:Label></b></div>
                    <div class="edit">
                        <asp:LinkButton runat="server" ID="editname" class="edit" OnClick="edit_Name"></asp:LinkButton>
                    </div>
                </div>
                <%} %>
                <%  if (showAddressSection.ToString() != string.Empty && showAddressSection.ToString() != null)
                    { %>
                <div class="holdertitle">
                    <div class="leftcontent3 first" style="padding-top: 0px;">
                        <%= Resources.GlobalResources.Address%>
                        <br />
                        <b>
                            <asp:Label ID="lblAddress" runat="server"></asp:Label><br />
                            <asp:Label ID="lblPostCode" runat="server"></asp:Label></b></div>
                    <div class="edit">
                        <asp:LinkButton runat="server" ID="editpostcode" class="edit" OnClick="edit_Address"></asp:LinkButton>
                    </div>
                </div>
                <%} %>
                <%  if (showEmailSection.ToString() != string.Empty && showEmailSection.ToString() != null)
                    { %>
                <div class="holdertitle">
                    <div class="leftcontent2 first" style="padding-top: 0px;">
                        <%= Resources.GlobalResources.Email%>
                        <br />
                        <b>
                            <asp:Label ID="lblEmail" CssClass="clsWrap" runat="server"></asp:Label></b></div>
                    <div class="leftcontent2" style="padding-top: 0px;">
                        <%= Resources.GlobalResources.PhoneNumberLblFinal%>
                        <br />
                        <b>
                            <asp:Label ID="lblPhone" CssClass="clsWrap" runat="server"></asp:Label></b></div>
                    <div class="leftcontent2">
                        &nbsp;</div>
                    <div class="edit">
                        <asp:LinkButton runat="server" ID="editEmail" class="edit" OnClick="edit_Email"></asp:LinkButton>
                    </div>
                </div>
                <%} %>
                <%  if ((showPreferredLanguageSection.ToString() != string.Empty && showPreferredLanguageSection.ToString() != null) || (showRaceSection.ToString() != string.Empty && showRaceSection.ToString() != null))
                    { %>
                <div class="holdertitle">
                    <div class="leftcontent2 first" style="padding-top: 0px;" id="divPreferredLanguage"
                        runat="server">
                        <%= Resources.GlobalResources.PreferredLanguage%>
                        <br />
                        <b>
                            <asp:Label ID="lblPreferredLanguage" CssClass="clsWrap" runat="server"></asp:Label></b></div>
                    <div class="leftcontent2" style="padding-top: 0px;" id="divPassportNumber" runat="server">
                        <%= Resources.GlobalResources.PassportNumber%>
                        <br />
                        <b>
                            <asp:Label ID="lblPassportNumber" CssClass="clsWrap" runat="server"></asp:Label></b></div>
                    <div class="leftcontent2" style="padding-top: 0px;">
                        <div id="divSSN" runat="server">
                            <%= Resources.GlobalResources.SocialSecurityNumber%><br />
                            <b>
                                <asp:Label ID="lblSocialSecurityNumber" runat="server"></asp:Label></b></div>
                        <div id="divRace" runat="server">
                            <%= Resources.GlobalResources.FCRace%>
                            <b>
                                <asp:Label ID="lblRace" runat="server"></asp:Label></b>
                        </div>
                    </div>
                    <div class="edit" id="divEdit" runat="server">
                        <asp:LinkButton runat="server" ID="editLang" OnClick="edit_Lang" class="edit"></asp:LinkButton>
                    </div>
                </div>
                <%} %>
                <%  
                    if ((showDOBHHAgesSection.ToString() != string.Empty && showDOBHHAgesSection.ToString() != null) || (showDietaryPreferencesSection.ToString() != string.Empty && showDietaryPreferencesSection.ToString() != null))
                    { %>
                <div class="holdertitle">
                    <div class="leftcontent2 first" style="padding-top: 0px;" id="divDOB" runat="server">
                        <%= Resources.GlobalResources.DateOfBirth%>
                        <br />
                        <b>
                            <asp:Label ID="lblDOB" runat="server"></asp:Label></b></div>
                    <div class="leftcontent2" style="padding-top: 0px;" id="divHousehold" runat="server">
                        <%= Resources.GlobalResources.HouseHoldAges%>
                        <br />
                        <b>
                            <asp:Label ID="lblAges" runat="server"></asp:Label></b></div>
                    <div class="leftcontent2" style="padding-top: 0px;" id="divDietaryNeeds" runat="server">
                        <%= Resources.GlobalResources.DietaryNeeds%><br />
                        <b>
                            <asp:Label ID="lblDietry" CssClass="clsWrap" runat="server"></asp:Label></b></div>
                    <div class="edit" id="divEditDOB" runat="server">
                        <asp:LinkButton runat="server" ID="editDOB" class="edit" OnClick="edit_DOB"></asp:LinkButton>
                    </div>
                </div>
                <%} %>
                <div class="clear" style="height: 10px">
                </div>
                <p class="italic">
                    <%= Resources.GlobalResources.PrintClubcardText%></p>
                <div class="buttons">
                    <div class="buttonitems">
                        <asp:ImageButton ID="imgback" runat="server" ImageUrl="<%$ Resources:GlobalResources,ImgBack%>"
                            OnClick="imgBack_Click" />
                    </div>
                    <div class="buttonitems">
                        <div class="cancelStart">
                            <asp:LinkButton runat="server" ID="divcancel" class="cancelStart" OnClick="Cancel_Restart"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="buttonspacer">
                        &nbsp;</div>
                    <div id="div1" class="buttonitems" runat="server">
                        <asp:ImageButton ID="imgTC" runat="server" ImageUrl="<%$ Resources:GlobalResources,ImgTandC%>"
                            OnClick="TC_Click" />
                    </div>
                    <div class="buttonitems" id="imgp" runat="server">
                        <asp:ImageButton ID="imgPrint" runat="server" ImageUrl="<%$ Resources:GlobalResources,ImgPrint%>"
                            OnClick="imgPrint_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
