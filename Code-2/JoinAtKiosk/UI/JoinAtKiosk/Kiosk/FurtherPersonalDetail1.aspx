<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FurtherPersonalDetail1.aspx.cs" Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.FurtherPersonalDetail1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title><%= Resources.GlobalResources.LPSHead%></title>
    <script src="../Scripts/Common.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function Loading() {

            if (document.getElementById('divSSN1') != null) {
                document.getElementById('divSSN1').className = 'Initial';
            }

            if (document.getElementById('divPassport') != null) {
                document.getElementById('divPassport').className = 'Initial';
            }

            if (document.getElementById('divTitle') == null) {
                if (document.getElementById('divPassport') == null) {
                    EnableSecond();
                }
                else { EnableFirstName(); }
            }
            else {

                if (document.getElementById('divPassport') == null) {
                    document.getElementById('divnextbutton').className = "nextbutton nextsurnamefocus";
                }
                else {
                    document.getElementById('divnextbutton').className = "nextbutton nextfirstnamefocus";
                }
                document.getElementById('divspacer').className = "buttonspacer4";

            }
            document.getElementById('btnBack').src = "<%= Resources.GlobalResources.BackBtn%>";

            var errorCtrl = document.getElementById('hdnErrorCtrl').value;
            if (errorCtrl == "txtPassport") {
                EnableFirstName();
                document.getElementById('txtPassport').focus();
                document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
            }
            if (errorCtrl == "txtSSN") {
                EnableSecond();
                document.getElementById('txtSSN').focus();
                document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
            }
        }

        function title_lang1Click(obj) {
            if (document.getElementById('divPassport') == null) {
                EnableSecond();
                document.getElementById('txtSSN').focus();
                document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
            }
            else {
                EnableFirstName();
                document.getElementById('txtPassport').focus();
                document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
            }
             CallLast(); 
        }
        function title_lang2Click(obj) {
            if (document.getElementById('divPassport') == null) {
                EnableSecond();
                document.getElementById('txtSSN').focus();
                document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
            }
            else {
                EnableFirstName();
                document.getElementById('txtPassport').focus();
                document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
            }
            CallLast();
        }
        function title_lang3Click(obj) {
            if (document.getElementById('divPassport') == null) {
                EnableSecond();
                document.getElementById('txtSSN').focus();
                document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
            }
            else {
                EnableFirstName();
                document.getElementById('txtPassport').focus();
                document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
            }
            CallLast();
        }
        function title_lang4Click(obj) {
            if (document.getElementById('divPassport') == null) {
                EnableSecond();
                document.getElementById('txtSSN').focus();
                document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
            }
            else {
                EnableFirstName();
                document.getElementById('txtPassport').focus();
                document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
            }
            CallLast();
        }
        function CallLast() {
            if (document.getElementById('txtPassport') == null && document.getElementById('txtSSN') == null && document.getElementById('divnextbutton') != null) {
                document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
                document.getElementById('divspacer').className = "buttonspacer5";
            }

         
        }
        function EnableFirstName() {
           
            DisableAll();
            var txt = "<%= Resources.GlobalResources.LPSPassport%>";
            document.getElementById('lbltitle').innerHTML = txt;
            if (document.getElementById('txtPassport') != null) {
                document.getElementById('divPassport').className = 'focuspanel curved paddingtop-10';
                document.getElementById('divPassport2').className = 'inputtext twolines whitetext paddingtop10';
                document.getElementById('divPassport3').className = 'input322 paddingtop10';
                document.getElementById('txtPassport').disabled = false;
                //document.getElementById('txtPassport').focus();
                // FocusObject('txtPassport');
                window.external.showKeyboard(1);
                //document.getElementById('txtPassport').focus();
            }
            Text_Check();
            document.getElementById('txtPassport').focus();
            window.external.showKeyboard(1);
        }

        function EnableSecond() {
            
            DisableAll();
            if (document.getElementById('txtSSN') != null) {
                var txt = "<%= Resources.GlobalResources.LPSSSN%>";
                document.getElementById('lbltitle').innerHTML = txt;
                document.getElementById('divSSN1').className = 'focuspanel curved paddingtop-10';
                document.getElementById('divSSN2').className = 'inputtext whitetext paddingtop10';
                document.getElementById('divSSN3').className = 'input322 paddingtop10';
                document.getElementById('txtSSN').disabled = false;
                //document.getElementById('txtSSN').focus();
                //FocusObject('txtSSN');
                window.external.showKeyboard(1);
                //document.getElementById('txtSSN').focus();
            }
            Text_Check();
            window.external.showKeyboard(1);
        }

        function DisableAll() {
            if (document.getElementById('divPassport') != null) {
                document.getElementById('divPassport').className = '';
                document.getElementById('divPassport2').className = 'inputtext';
                document.getElementById('divPassport3').className = 'input322';
            }
            if (document.getElementById('divSSN1') != null) {
                document.getElementById('divSSN1').className = '';
                document.getElementById('divSSN2').className = 'inputtext';
                document.getElementById('divSSN3').className = 'input322';
            }
            if (document.getElementById('divTitle') != null) {
                document.getElementById('divTitle').className = '';
                document.getElementById('Span1').className = 'titletext grey';
                if (document.getElementById('hdnBack').value == "") {
                    if (document.getElementById('divmr') != null && document.getElementById('divmr').className != "title_lang1 select") document.getElementById('divmr').className = "title_lang1 unselect";
                    if (document.getElementById('divmiss') != null && document.getElementById('divmiss').className != "title_lang2 select") document.getElementById('divmiss').className = "title_lang2 unselect";
                    if (document.getElementById('divmrs') != null && document.getElementById('divmrs').className != "title_lang3 select") document.getElementById('divmrs').className = "title_lang3 unselect";
                    if (document.getElementById('divms') != null && document.getElementById('divms').className != "title_lang4 select") document.getElementById('divms').className = "title_lang4 unselect";
                }
                if (document.getElementById('hdnBack').value == "Back") {
                    if (document.getElementById('divmr') != null && document.getElementById('divmr').className == "title_lang1 focus") {
                        document.getElementById('divmr').className = "title_lang1 select";
                        document.getElementById('divms').className = "title_lang4 unselect";
                        document.getElementById('divmiss').className = "title_lang2 unselect";
                        document.getElementById('divmrs').className = "title_lang3 unselect";
                    }
                    if (document.getElementById('divmiss') != null && document.getElementById('divmiss').className == "title_lang2 focus") {
                        document.getElementById('divmiss').className = "title_lang2 select";
                        document.getElementById('divmr').className = "title_lang1 unselect";
                        document.getElementById('divms').className = "title_lang4 unselect";
                        document.getElementById('divmrs').className = "title_lang3 unselect";
                    }
                    if (document.getElementById('divmrs') != null && document.getElementById('divmrs').className == "title_lang3 focus") {
                        document.getElementById('divmrs').className = "title_lang3 select";
                        document.getElementById('divmiss').className = "title_lang2 unselect";
                        document.getElementById('divmr').className = "title_lang1 unselect";
                        document.getElementById('divms').className = "title_lang4 unselect";
                    }
                    if (document.getElementById('divms') != null && document.getElementById('divms').className == "title_lang4 focus") {
                        document.getElementById('divms').className = "title_lang4 select";
                        document.getElementById('divmrs').className = "title_lang3 unselect";
                        document.getElementById('divmiss').className = "title_lang2 unselect";
                        document.getElementById('divmr').className = "title_lang1 unselect";
                    }
                }
            }
        }
        
        function FirstName_Click() { EnableFirstName(); }
        function Surname_Click() {EnableSecond();}

        function Text_Check() {
           
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';

            var sLanguage = document.getElementById('hdnLanguage').value;
            var sPassport = document.getElementById('hdnPassport').value;
            var sSSN = document.getElementById('hdnSSN').value;
            var sLN = "";
            
                if (document.getElementById('txtPassport') != null && document.getElementById('divPassport').className == "focuspanel curved paddingtop-10") {
                    sLN = trim(document.getElementById('txtPassport').value);
                    var lenFn = sLN.length;
                    if (document.getElementById('txtSSN') != null) {
                        document.getElementById('divnextbutton').className = "nextbutton nextsurname";
                        if (lenFn >= 2 || sPassport != "true") {
                            document.getElementById('divnextbutton').className = "nextbutton nextsurnamefocus";
                        }
                        document.getElementById('divspacer').className = "buttonspacer4";
                    } else {
                        var errorCtrl = document.getElementById('hdnConfirm').value;
                        if (errorCtrl == "Confirm" && document.getElementById('hdnNextPage').value != "FurtherPersonalDetails2") {
                            document.getElementById('divnextbutton').style.display = 'none';
                            document.getElementById('divspacer').className = 'buttonspacer2';
                            document.getElementById('divsummary').style.display = 'block';
                        }
                        else {
                            document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
                            if (lenFn >= 2 || sPassport != "true") {
                                //document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton confirmnamefocus";
                            }
                            document.getElementById('divspacer').className = "buttonspacer5";
                        }
                    }
                }
                if (document.getElementById('txtSSN') != null && document.getElementById('divSSN1').className == "focuspanel curved paddingtop-10") {
                    sLN = trim(document.getElementById('txtSSN').value);
                    var lenFn = sLN.length;
                    var errorCtrl = document.getElementById('hdnConfirm').value;
                    if (errorCtrl == "Confirm" && document.getElementById('hdnNextPage').value != "FurtherPersonalDetails2") {
                        document.getElementById('divnextbutton').style.display = 'none';
                        document.getElementById('divspacer').className = 'buttonspacer2';
                        document.getElementById('divsummary').style.display = 'block';
                    }
                    else {
                        document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
                        if (lenFn >= 2 || sSSN != "true") {
                            //document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton confirmnamefocus";
                        }
                        document.getElementById('divspacer').className = "buttonspacer5";
                    }
                 }
                 if (document.getElementById('txtPassport') == null && document.getElementById('txtSSN') == null && sLanguage == "true") {
                     if (document.getElementById('divmr').className == "title_lang1 select" || document.getElementById('divmiss').className == "title_lang2 select" ||
                     document.getElementById('divmrs').className == "title_lang3 select" || document.getElementById('divms').className == "title_lang4 select" ) {
                         document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
                     } else document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
                     document.getElementById('divspacer').className = "buttonspacer5";
                     var errorCtrl = document.getElementById('hdnConfirm').value;
                     if (errorCtrl == "Confirm" && document.getElementById('hdnNextPage').value != "FurtherPersonalDetails2") {
                         document.getElementById('divnextbutton').style.display = 'none';
                         document.getElementById('divspacer').className = 'buttonspacer2';
                         document.getElementById('divsummary').style.display = 'block';
                     }
                 } 
        }

        function trim(str) {
            str = str.replace(/^\s+|\s+$/g, "");
            return str;
        }
        function focus(objName) {
            window.external.showKeyboard(1);
            document.getElementById(objName).focus();
        }
        function Next_Value(obj) {
            
            //if (document.getElementById('divnextbutton').className.toString().indexOf("focus") > -1) {
            if (document.getElementById('txtPassport') != null &&
                document.getElementById('divPassport').className == "focuspanel curved paddingtop-10" &&
                obj == "next") {
                if (document.getElementById('txtSSN') != null) {
                    EnableSecond();
                    document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
                    return false;
                } else {return true;}
            }
            else if (document.getElementById('txtPassport') != null &&
                document.getElementById('divPassport').className == 'Initial' &&
                obj == "next") {

                EnableFirstName();
                document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
                return false;
            }
            else if (document.getElementById('txtSSN') != null &&
                document.getElementById('divSSN1').className =='Initial' &&
                obj == "next" ) {

                EnableSecond();
                document.getElementById('txtSSN').value = document.getElementById('txtSSN').value;
                return false;
            } 
            else if (document.getElementById('txtSSN') != null &&
                document.getElementById('divSSN1').className == "focuspanel curved paddingtop-10" &&
                obj == "prev") {
                if (document.getElementById('txtPassport') != null) {
                    EnableFirstName();
                    document.getElementById('txtPassport').value = document.getElementById('txtPassport').value;
                    return false;
                } else { window.location = document.getElementById('hdnPrevPage').value; }
               
            }
           
            else if (obj == "prev") window.location = document.getElementById('hdnPrevPage').value;
//                if((document.getElementById('txtSSN') == null  && document.getElementById('txtPassport') == null||
//                (document.getElementById('txtPassport') == null && document.getElementById('divSSN1').className == "focuspanel curved paddingtop-10"))||
//                document.getElementById('divPassport').className == "focuspanel curved paddingtop-10") {
//                window.location = "WhatIsYourTitleandName.aspx?page=PassportSSN";
//                }
           // }
                return true;
        }  
    </script>

</head>
<body  onload="Loading()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <input type="hidden" id="hdnControlList" runat="server" />
    <input type="hidden" id="hdnRegExpList" runat="server" />
    <input type="hidden" id="hdnShowControls" runat="server" />
    <input type="hidden" id="hdnPrevPage" runat="server" />
    <input type="hidden" id="hdnNextPage" runat="server" />
    <input type="hidden" id="hdnLanguage" runat="server" />
    <input type="hidden" id="hdnPassport" runat="server" />
    <input type="hidden" id="hdnSSN" runat="server" />
      <input type="hidden" id="hdnBack" runat="server" />
       <input type="hidden" id="hdnConfirm" runat="server" />
        <input type="hidden" id="hdnErrorCtrl" runat="server" />
    <% var showNameControls = hdnShowControls.Value; %>
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                 <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em><asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,LPSLanguage%>"
                        ></asp:Label></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.LPSCrumb%>" width="1152" height="51" alt="" /></div>
        </div>
        <div id="body_wrapper">
            <div id="contentNameLPS">

            <%if (showNameControls.Contains("LANGUAGE"))
              { %>
               <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <div class="wrapperleft">
                    <div class="holdertitlefocus curved" id="divTitle" runat="server">
                            </div>
                            <span id="Span1" runat="server" class="titletext"><%= Resources.GlobalResources.LPSLanguageNAV%></span><br /> <span id="lblPLOpt" runat="server" class="titletext"><%= Resources.GlobalResources.OptionalLbl%></span>
                            <div>
                                <asp:LinkButton runat="server" ID="divmr" class="title_lang1" OnClientClick="title_lang1Click(divmr)"
                                OnClick="title_lang1Click"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divmiss" class="title_lang2" OnClientClick="title_lang2Click(divmiss)"
                                OnClick="title_lang2Click"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divmrs" class="title_lang3" OnClientClick="title_lang3Click(divmrs)"
                                OnClick="title_lang3Click"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divms" class="title_lang4" OnClientClick="title_lang4Click(divms)"
                                OnClick="title_lang4Click"></asp:LinkButton>
                            </div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                <!--wrapperleft-->
                <%} %>
                <div class="wrapperright">
                 <%if (showNameControls.Contains("PASSPORT"))
                 { %>
                    <div class="grey" id="divPassport" onclick="return FirstName_Click()"
                        runat="server">
                        <div class="inputtext" id="divPassport2" runat="server">
                           <%= Resources.GlobalResources.LPSLblPassport%><br /><span id="lblPassOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322 paddingtop10" id="divPassport3" runat="server">
                            <div class="textbox" onkeyup="Text_Check()">
                                <asp:TextBox runat="server" CssClass="text" ID="txtPassport" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                    <%}
                       
                        if (showNameControls.Contains("SSN"))
                        {  %>
                    <div class="" id="divSSN1" onclick="return Surname_Click()" runat="server">
                        <div class="inputtext" id="divSSN2" runat="server">
                             <%= Resources.GlobalResources.LPSLblSSN%><br /><span id="lblSSNOpt" runat="server" class="smalltext"><%= Resources.GlobalResources.OptionalLbl%></span></div>
                        <div class="input322" id="divSSN3" runat="server">
                            <div class="textbox" onkeyup="Text_Check()">
                                <asp:TextBox runat="server" CssClass="text" ID="txtSSN" Font-Size="X-Large"  ></asp:TextBox></div>
                        </div>
                    </div>
                <%} %>
               </div> 

              <div class="ErrorDiv" style="width: 485px;display: none;height:80px;margin-bottom:20px" id="divError" runat="server">
                <div class="panel curved" style="height:80px;width:485px;margin-left:260px;background-color:#fff;"> 
                    <asp:Label ID="lblError" runat="server" BorderColor="Red" ForeColor="Red" style="font-size:24px;"></asp:Label>
                    </div>
                     
               </div>
               <br />
                <div class="buttons" style="width: 100%">
                        <a href="#"> <div class="buttonitems">
                        <asp:ImageButton ID="btnBack" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>" runat="server" OnClientClick="return Next_Value('prev')" OnClick="btnBack_click" />
                        </div></a>
                    <div class="buttonitems">                       
                            <div class="cancelStart">
                                <asp:LinkButton runat="server" ID="LinkButton1" class="cancelStart" OnClick="Cancel_Restart"></asp:LinkButton>
                            </div>                       
                    </div>
                    <%--<div class="buttonitems" >
                        <a href="TermsAndCondition.aspx?page=WhatIsYourTitleandName">
                            <img src="<%= Resources.GlobalResources.TCImg%>" style="border: none" alt=""  /></a></div>--%>
                    <div class="buttonspacer4" id="divspacer" runat="server">
                        &nbsp;</div>
                    <div class="buttonitems last">
                        <div runat="server" id="divnext" class="nextbutton" >
                        </div>
                        <div id="divs" runat="server" style="display: none">
                            <asp:LinkButton runat="server" ID="divnextbutton" class="nextbutton"  OnClientClick="return Next_Value('next')" OnClick="Confirm_Details"></asp:LinkButton>
                        </div>
                    </div>
                      <div class="buttonitems last paddingtop-13" runat="server" id="divsummary" style="display: none">
                        <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary" runat="server"
                            OnClick="Confirm_Details"/>
                    </div>

                    </div>
             </div>
                <!--buttons-->
        </div>
            <!--contcontentAddressent22-->
     </div>
        <%--<div class="keyboard">

            <script type="text/javascript" language="javascript">
                //window.external.showKeyboardTemplate('StandardUpperNE');
             
            </script>

        </div>--%>
        <!--body_wrapper-->
       
    </form>
</body>
</html>


