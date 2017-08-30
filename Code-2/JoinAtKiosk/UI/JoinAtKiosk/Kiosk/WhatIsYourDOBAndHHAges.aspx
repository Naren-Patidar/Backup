<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatIsYourDOBAndHHAges.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.WhatIsYourDOBAndHHAges" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.DOBTitle%></title>
    <script src="../Scripts/WhatIsYourDOBAndHHAges.js" type="text/javascript"></script>
    <script src="../Scripts/Common.js" type="text/javascript" language="javascript"></script>
    <script type="text/javascript" language="javascript">
   
        
        function Loading() {
        // debugger;
        resources = document.getElementById('hdnResources').value;
        resources = resources.split(',');
            if (document.getElementById('txtDt1') != null) {
             var txt = resources[0];
                document.getElementById('lbltitle').innerHTML = txt;
                FocusDOB('txtDt1');
            }
            else {
             var txt = resources[1];
                document.getElementById('lbltitle').innerHTML = txt;
                FocusAge('txtAge1');
            }
            <% if(Request.QueryString["ctrlID"]!=null) {%>
                var ctrl = '<% =Request.QueryString["ctrlID"]%>';
                if (ctrl.toUpperCase() == 'DT') {
                    FocusDOB('txtDt1');
                }
                else if (ctrl.toUpperCase() == 'TXTAGE1' || ctrl.toUpperCase() == 'TXTAGE2' || ctrl.toUpperCase() == 'TXTAGE3' || ctrl.toUpperCase() == 'TXTAGE4' || ctrl.toUpperCase() == 'TXTAGE5') {
                    FocusAge(ctrl);
                }
            <%} %>
        }
    </script>
</head>
<body onload="Loading()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <input type="hidden" id="hdnControlList" runat="server" />
    <input type="hidden" id="hdnDt" runat="server" />
    <input type="hidden" id="hdnShowControls" runat="server" />
    <input type="hidden" id="hdnDateFormat" runat="server" />
    <input type="hidden" id="hdnDOBLimitInDays" runat="server" />
    <input type="hidden" id="hdnErr" runat="server" />
    <input type="hidden" id="hdnResources" runat="server" />
    <input type="hidden" id="hdnConfirmPg" runat="server" />
    <input type="hidden" id="hdnDateRegExp" runat="server" />
    <input type="hidden" id="hdnAgeRegExp" runat="server" />
    <% 
        var showControls = hdnShowControls.Value; %>
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>
                    <asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,DOBTitle%>"></asp:Label></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.DOBAndHHAgesBreadCrumb%>" width="1152" height="51"
                    alt="" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentHousehold">
                <div class="wrapperleft" style="width: 100%">
                    <p class="italic">
                        <span id="Span2" runat="server">
                            <%= Resources.GlobalResources.DOBAndHHLbl01%></span>
                    </p>
                    <%if (showControls.Contains("DATEOFBIRTH"))
                      { %>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div id="dob" class="focuspaneldob curved">
                                <div id="dobtxt" class="inputtext whitetext" style="padding-top: 30px;" onclick="FocusDOB('txtDt1')">
                                    <span id="Span3" runat="server" class="titletext">
                                        <%= Resources.GlobalResources.DOBAndHHLbl02%></span><br /><span id="lblDOBOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span>
                                </div>
                                <div id="pnldt1" runat="server" class="input92 paddingtop-10" onclick="FocusDOB('txtDt1')">
                                    <div class="textbox" onkeyup="Date_Check1()" style="padding-top: 30px;">
                                        <asp:TextBox ID="txtDt1" runat="server" class="text" Font-Size="X-Large" onclick="FocusDOB('txtDt1')" /></div>
                                </div>
                                <div id="pnldt2" runat="server" class="input92 paddingtop-10" onclick="FocusDOB('txtDt2')">
                                    <div class="textbox" onkeyup="Date_Check2()" style="padding-top: 30px;">
                                        <asp:TextBox ID="txtDt2" runat="server" class="text" Font-Size="X-Large" onclick="FocusDOB('txtDt2')" /></div>
                                </div>
                                <div id="pnldt3" runat="server" class="input92 paddingtop-10" onclick="FocusDOB('txtDt3')">
                                    <div class="textbox" style="padding-top: 30px;">
                                        <asp:TextBox ID="txtDt3" runat="server" class="text" Font-Size="X-Large" onclick="FocusDOB('txtDt3')" /></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%} %>
                    <p style="clear: both">
                    </p>
                    <%  if (showControls.Contains("HHAGES"))
                        { %>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlHHAges" runat="server">
                                <asp:Panel ID="pnlHHAges1" runat="server" class="inputtext grey" Style="padding-top: 15px;
                                    font-size: 16px">
                                    <span id="Span4" runat="server" class="titletext" onclick="FocusAge('txtAge1')">
                                        <%= Resources.GlobalResources.DOBAndHHLbl03%></span> <span id="lblHHAgeOpt" runat="server"
                                            class="smalltext">
                                            <%= Resources.GlobalResources.OptionalLbl%></span>
                                </asp:Panel>
                                <div id="ages">
                                    <div class="input92 input92" onclick="FocusAge('txtAge1')">
                                        <span id="lblAge1" runat="server" class="smalltext1">
                                            <%= Resources.GlobalResources.DOBAndHHLbl05%></span>
                                        <div class="textbox" onkeyup="Age_Check('txtAge1', 'txtAge2')">
                                            <asp:TextBox ID="txtAge1" runat="server" MaxLength="2" class="text" onclick="FocusAge('txtAge1')" /></div>
                                    </div>
                                    <div class="input92 input92" onclick="FocusAge('txtAge2')">
                                        <span id="lblAge2" runat="server" class="smalltext1">
                                            <%= Resources.GlobalResources.DOBAndHHLbl06%></span>
                                        <div class="textbox" onkeyup="Age_Check('txtAge2', 'txtAge3')">
                                            <asp:TextBox ID="txtAge2" runat="server" MaxLength="2" class="text" onclick="FocusAge('txtAge2')" /></div>
                                    </div>
                                    <div class="input92 input92" onclick="FocusAge('txtAge3')">
                                        <span id="lblAge3" runat="server" class="smalltext1 ">
                                            <%= Resources.GlobalResources.DOBAndHHLbl07%></span>
                                        <div class="textbox" onkeyup="Age_Check('txtAge3', 'txtAge4')">
                                            <asp:TextBox ID="txtAge3" runat="server" MaxLength="2" class="text" onclick="FocusAge('txtAge3')" /></div>
                                    </div>
                                    <div class="input92 input92" onclick="FocusAge('txtAge4')">
                                        <span id="lblAge4" runat="server" class="smalltext1">
                                            <%= Resources.GlobalResources.DOBAndHHLbl08%></span>
                                        <div class="textbox" onkeyup="Age_Check('txtAge4', 'txtAge5')">
                                            <asp:TextBox ID="txtAge4" runat="server" MaxLength="2" class="text" onclick="FocusAge('txtAge4')" /></div>
                                    </div>
                                    <div class="input92 input92" onclick="FocusAge('txtAge5')">
                                        <span id="lblAge5" runat="server" class="smalltext1">
                                            <%= Resources.GlobalResources.DOBAndHHLbl09%></span>
                                        <div class="textbox">
                                            <asp:TextBox ID="txtAge5" runat="server" MaxLength="2" class="text" onclick="FocusAge('txtAge5')" /></div>
                                    </div>
                                    <div id="divAge6" runat="server" class="inputtext" style="padding-top: 65px; font-size: 12px;
                                        font-style: italic">
                                        <span class="italic">
                                            <%= Resources.GlobalResources.DOBAndHHLbl10%></span></div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%} %>
                </div>
                <div class="buttons">
                    <div class="buttonitems">
                        <a href="#">
                            <asp:ImageButton runat="server" ID="btnBack" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>"
                                alt="" OnClientClick="return PreviousControl()" OnClick="btnBack_click" /></a>
                    </div>
                    <div class="buttonitems">
                        <div class="cancelStart">
                            <asp:LinkButton runat="server" ID="divcancel" class="cancelStart" OnClick="Cancel_Restart"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="buttonspacer" runat="server" id="divspacer">
                        &nbsp;</div>
                    <div class="buttonitems last">
                        <asp:Panel ID="pnlnexthouseages" class="nexthouseages nexthouseagesfocus" runat="server"
                            BackImageUrl="<%$ Resources:GlobalResources,DOBAndHHNextHHImg%>">
                            <asp:LinkButton runat="server" ID="lnkNext" class="nexthouseages nexthouseagesfocus"
                                OnClientClick="return NextButtonClick();"></asp:LinkButton>
                        </asp:Panel>
                    </div>
                    <div class="buttonitems last paddingtop-13" runat="server" id="divconfirm" style="display: none">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,DOBAndHHConfirmImg%>" ID="imgConfirmDetails"
                            runat="server" OnClientClick="return Validate(document.getElementById('hdnControlList'));"
                            OnClick="NextPage_Click" />
                    </div>
                    <div class="buttonitems last paddingtop-13" runat="server" id="divsummary" style="display: none;">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary"
                            runat="server" OnClientClick="return Validate(document.getElementById('hdnControlList'));"
                            OnClick="NextPage_Click" />
                    </div>
                </div>
            </div>
            <!--buttons-->
        </div>
        <!--contcontentAddressent22-->
        <%--        <div class="keyboard">
            <script type="text/javascript" language="javascript">
                window.external.showKeyboardTemplate('StandardUpperNE');
            </script>
        </div>--%>
    </div>
    <!--body_wrapper-->
    </form>
</body>
</html>
