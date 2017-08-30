<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatIsYourTitleandName.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.WhatIsYourTitleandName" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title><%= Resources.GlobalResources.TitleHead%></title>
    <script src="../Scripts/WhatIsYourTitleandName.js" type="text/javascript"></script>
    <script src="../Scripts/Common.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        resources = "<%=resourceStr %>";
        resources = resources.split(',');
        
     </script>
</head>
<body onload="Loading()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <input type="hidden" id="hdnControlList" runat="server" />
    <input type="hidden" id="hdnRegExpList" runat="server" />
    <input type="hidden" id="hdnShowControls" runat="server" />
    <input type="hidden" id="hdnErrorCtrl" runat="server" />
    
    <% 
        var showNameControls = hdnShowControls.Value; %>
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                 <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em><asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,Title%>"
                        ></asp:Label></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.NameBreadCrumb%>" width="1152" height="51" alt="" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentName">

            <%if (showNameControls.Contains("TITLE"))
              { %>
               <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <div class="wrapperleft">
                    <div class="holdertitlefocus curved" id="divTitle" runat="server">
                            </div>
                            <span id="Span1" runat="server" class="titletext"><%= Resources.GlobalResources.TitleLbl%></span>
                            <div>
                                <asp:LinkButton runat="server" ID="divmr" class="title_mr" OnClientClick="title_mrClick(divmr)"
                                OnClick="title_mrClick"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divmiss" class="title_miss" OnClientClick="title_missClick(divmiss)"
                                OnClick="title_missClick"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divmrs" class="title_mrs" OnClientClick="title_mrsClick(divmrs)"
                                OnClick="title_mrsClick"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divms" class="title_ms" OnClientClick="title_msClick(divms)"
                                OnClick="title_msClick"></asp:LinkButton>
                            </div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                <!--wrapperleft-->
                <%} %>
                <div class="wrapperright">
                 <%if (showNameControls.Contains("FIRSTNAME"))
                 { %>
                    <div class="grey" id="divFirstName1" onclick="return FirstName_Click()"
                        runat="server">
                        <div class="inputtext" id="divFirstName2" runat="server">
                           <%= Resources.GlobalResources.FirstNameLbl%><br /><span id="lblFNOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="divFirstName3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtFirstName" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                    <%}
                        if (showNameControls.Contains("MIDDLENAME"))
                        {  %>
                    <div class="" onclick="return MiddleName_Click()" id="divMiddleName1" runat="server">
                        <div class="inputtext" id="divMiddleName2" runat="server">
                           <%= Resources.GlobalResources.MiddleNameLbl%><br /><span id="lblMNOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322" id="divMiddleName3" runat="server">
                            <div class="textbox">
                                <asp:TextBox runat="server" CssClass="text" ID="txtMiddleName" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                    <%}
                        if (showNameControls.Contains("SURNAME"))
                        {  %>
                    <div class="" id="divSurname1" onclick="return Surname_Click()" runat="server">
                        <div class="inputtext" id="divSurname2" runat="server">
                             <%= Resources.GlobalResources.SurnameLbl%><br /><span id="lblSNOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322" id="divSurname3" runat="server">
                            <div class="textbox" onkeyup="Text_Check()">
                                <asp:TextBox runat="server" CssClass="text" ID="txtSurname" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                <%} %>
               </div> 

                <div class="buttons" style="width: 100%">
                        <a href="#"> <div class="buttonitems">
                        <img id="btnBack" src="<%= Resources.GlobalResources.BackBtnGrey%>" alt="" onclick="return PreviousControl()"/></div></a>
                    <div class="buttonitems">                       
                            <div class="cancelStart">
                                <asp:LinkButton runat="server" ID="LinkButton1" class="cancelStart" OnClick="Cancel_Restart"></asp:LinkButton>
                            </div>                       
                    </div>
                    <div class="buttonitems" >
                            <asp:ImageButton ID="lnkTC" ImageUrl="<%$ Resources:GlobalResources,TCImg%>" runat="server" OnClick="TandC_Click" />
                            </div>
                    <div class="buttonspacer" id="divspacer" runat="server">
                        &nbsp;</div>
                    <div class="buttonitems last paddingtop-10" id="divBtn">
                        <div runat="server" id="divnext" class="nextbutton" >
                        </div>
                        <div id="divs" runat="server" style="display: none">
                            <asp:LinkButton runat="server" ID="divnextbutton" class="nextbutton"  OnClientClick="return Next_Value()" OnClick="Confirm_Details"></asp:LinkButton>
                        </div>
                    </div>
                      <div class="buttonitems last paddingtop-13" runat="server" id="divsummary" style="display: none">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary" runat="server"
                            OnClick="Confirm_Details" />
                    </div>
                    </div>
             </div>
                <!--buttons-->
               
        </div>
            <!--contcontentAddressent22-->
     </div>
       
        <!--body_wrapper-->
    </form>
</body>
</html>


