<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FurtherPersonalDetails2.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.FurtherPersonalDetails2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title>
        <%= Resources.GlobalResources.FPD2Title%></title>
    <script type="text/javascript" language="javascript">

        function title_Race1Click(obj) {
            obj.className = "title_Race1 select";
            if (document.getElementById('divRace2') != null) {
                document.getElementById('divRace2').className = "title_Race2";
            }
            if (document.getElementById('divRace3') != null) {
                document.getElementById('divRace3').className = "title_Race3";
            }
            if (document.getElementById('divRace4') != null) {
            document.getElementById('divRace4').className = "title_Race4";
            }
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';

            TitleFocus();
        }

        function title_Race2Click(obj) {
            obj.className = "title_Race2 select";
            if (document.getElementById('divRace1') != null) {
                document.getElementById('divRace1').className = "title_Race1";
            }
            if (document.getElementById('divRace3') != null) {
                document.getElementById('divRace3').className = "title_Race3";
            }
            if (document.getElementById('divRace4') != null) {
                document.getElementById('divRace4').className = "title_Race4";
            }
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';

            TitleFocus();
        }

        function title_Race3Click(obj) {
            obj.className = "title_Race3 select";
            if (document.getElementById('divRace1') != null) {
                document.getElementById('divRace1').className = "title_Race1";
            }
            if (document.getElementById('divRace2') != null) {
                document.getElementById('divRace2').className = "title_Race2";
            }
            if (document.getElementById('divRace4') != null) {
                document.getElementById('divRace4').className = "title_Race4";
            }
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';

            TitleFocus();
        }

        function title_Race4Click(obj) {
            obj.className = "title_Race4 select";
            if (document.getElementById('divRace1') != null) {
                document.getElementById('divRace1').className = "title_Race1";
            }
            if (document.getElementById('divRace2') != null) {
                document.getElementById('divRace2').className = "title_Race2";
            }
            if (document.getElementById('divRace3') != null) {
                document.getElementById('divRace3').className = "title_Race3";
            }
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';

            TitleFocus();
        }

        function TitleFocus() {
            if (document.getElementById('divRace1') != null) {
                if (document.getElementById('divRace1').className == "title_Race1 select") {
                    document.getElementById('divRace1').className = "title_Race1 select";
                    if (document.getElementById('divRace2') != null) {
                        document.getElementById('divRace2').className = "title_Race2 unselect";
                    }
                    if (document.getElementById('divRace3') != null) {
                        document.getElementById('divRace3').className = "title_Race3 unselect";
                    }
                    if (document.getElementById('divRace4') != null) {
                        document.getElementById('divRace4').className = "title_Race4 unselect";
                    }
                }
            }
            if (document.getElementById('divRace2') != null) {
                if (document.getElementById('divRace2').className == "title_Race2 select") {
                    if (document.getElementById('divRace1') != null) {
                        document.getElementById('divRace1').className = "title_Race1 unselect";
                    }
                    document.getElementById('divRace2').className = "title_Race2 select";
                    if (document.getElementById('divRace3') != null) {
                        document.getElementById('divRace3').className = "title_Race3 unselect";
                    }
                    if (document.getElementById('divRace4') != null) {
                        document.getElementById('divRace4').className = "title_Race4 unselect";
                    }
                }
            }
            if (document.getElementById('divRace3') != null) {
                if (document.getElementById('divRace3').className == "title_Race3 select") {
                    if (document.getElementById('divRace1') != null) {
                        document.getElementById('divRace1').className = "title_Race1 unselect";
                    }
                    if (document.getElementById('divRace2') != null) {
                        document.getElementById('divRace2').className = "title_Race2 unselect";
                    }
                    document.getElementById('divRace3').className = "title_Race3 select";
                    if (document.getElementById('divRace4') != null) {
                        document.getElementById('divRace4').className = "title_Race4 unselect";
                    }
                }
            }
            if (document.getElementById('divRace4') != null) {
                if (document.getElementById('divRace4').className == "title_Race4 select") {
                    if (document.getElementById('divRace1') != null) {
                        document.getElementById('divRace1').className = "title_Race1 unselect";
                    }
                    if (document.getElementById('divRace2') != null) {
                        document.getElementById('divRace2').className = "title_Race2 unselect";
                    }
                    if (document.getElementById('divRace3') != null) {
                        document.getElementById('divRace3').className = "title_Race3 unselect";
                    }
                    document.getElementById('divRace4').className = "title_Race4 select";
                }
            }

            /*This condition is to check the redirection from summary page or not */
            if (document.getElementById('hdnRedirectionVal').value == 'FurtherPersonalDetails') {
                document.getElementById('divspacer').style.display = 'block';
                document.getElementById('divspacer').className = 'buttonspacer2';
                document.getElementById('divsummary').style.display = 'block';
                document.getElementById('divs').style.display = 'none';
                document.getElementById('divnextbutton').style.display = 'none';
            }
            else {
                document.getElementById('divspacer').style.display = 'block';
                document.getElementById('divspacer').className = 'buttonspacer';
                document.getElementById('divnext').style.display = 'none';
                document.getElementById('divs').style.display = 'block';
                document.getElementById('divsummary').style.display = 'none';
            }

            

        }

        function Loading() {
            if (document.getElementById('divTitle') == null) {
                document.getElementById('divnext').style.display = 'none';
            }
            if (document.getElementById('hdnBack').value != "") {
                if (document.getElementById('divTitle').className == 'holdertitlefocus curved') {
                    if (document.getElementById('divRace1') != null) {
                        if (document.getElementById('divRace1').className == "title_Race1 select") {
                            document.getElementById('divRace1').className = "title_Race1 focus";
                            if (document.getElementById('divRace2') != null) {
                                document.getElementById('divRace2').className = "title_Race2";
                            }
                            if (document.getElementById('divRace3') != null) {
                                document.getElementById('divRace3').className = "title_Race3";
                            }
                            if (document.getElementById('divRace4') != null) {
                                document.getElementById('divRace4').className = "title_Race4";
                            }
                        }
                    }
                    if (document.getElementById('divRace2') != null) {
                        if (document.getElementById('divRace2').className == "title_Race2 select") {
                            if (document.getElementById('divRace1') != null) {
                                document.getElementById('divRace1').className = "title_Race1";
                            }
                            document.getElementById('divRace2').className = "title_Race2 focus";
                            if (document.getElementById('divRace3') != null) {
                                document.getElementById('divRace3').className = "title_Race3";
                            }
                            if (document.getElementById('divRace4') != null) {
                                document.getElementById('divRace4').className = "title_Race4";
                            }
                        }
                    }
                    if (document.getElementById('divRace3') != null) {
                        if (document.getElementById('divRace3').className == "title_Race3 select") {
                            if (document.getElementById('divRace1') != null) {
                                document.getElementById('divRace1').className = "title_Race1";
                            }
                            if (document.getElementById('divRace2') != null) {
                                document.getElementById('divRace2').className = "title_Race2";
                            }
                            document.getElementById('divRace3').className = "title_Race3 focus";
                            if (document.getElementById('divRace4') != null) {
                                document.getElementById('divRace4').className = "title_Race4";
                            }
                        }
                    }
                    if (document.getElementById('divRace4') != null) {
                        if (document.getElementById('divRace4').className == "title_Race4 select") {
                            if (document.getElementById('divRace1') != null) {
                                document.getElementById('divRace1').className = "title_Race1";
                            }
                            if (document.getElementById('divRace2') != null) {
                                document.getElementById('divRace2').className = "title_Race2";
                            }
                            if (document.getElementById('divRace3') != null) {
                                document.getElementById('divRace3').className = "title_Race3";
                            }
                            document.getElementById('divRace4').className = "title_Race4 focus";
                        }
                    }
                }
            }
            if (document.getElementById('hdnRedirectionVal').value == 'FurtherPersonalDetails') {
                document.getElementById('divspacer').style.display = 'block';
                document.getElementById('divspacer').className = 'buttonspacer2';
                document.getElementById('divsummary').style.display = 'block';
                document.getElementById('divs').style.display = 'none';
                document.getElementById('divnextbutton').style.display = 'none';
            }
            else {
                document.getElementById('divspacer').style.display = 'block';
                document.getElementById('divspacer').className = 'buttonspacer';
                document.getElementById('divnext').style.display = 'none';
                document.getElementById('divs').style.display = 'block';
                document.getElementById('divsummary').style.display = 'none';
            }
        }

    </script>
</head>
<body onload="Loading()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <input type="hidden" id="hdnRedirectionVal" runat="server" />
    <input type="hidden" id="hdnBack" runat="server" />
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>" />
                <em>
                    <asp:Label ID="lbltitle" runat="server" Text="<%$ Resources:GlobalResources,FPD2Title%>"></asp:Label></em>
            </div>
            <div id="breadcrumbs">
                <img src="<%= Resources.GlobalResources.NameBreadCrumb%>" width="1152" height="51"
                    alt="" /></div>
        </div>
        <div id="body_wrapper">
            <div id="FPD2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="wrapperleft">
                            <div class="holdertitlefocus curved" id="divTitle" runat="server">
                            </div>
                            <span id="Span1" runat="server" class="titletext">
                                <%= Resources.GlobalResources.lblRaceText%></span>
                            <div>
                                <asp:LinkButton runat="server" ID="divRace1" class="title_Race1" OnClientClick="title_Race1Click(divRace1)" Visible=false
                                    OnClick="title_Race1Click"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divRace2" class="title_Race2" OnClientClick="title_Race2Click(divRace2)" Visible=false
                                    OnClick="title_Race2Click"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divRace3" class="title_Race3" OnClientClick="title_Race3Click(divRace3)" Visible=false
                                    OnClick="title_Race3Click"></asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton runat="server" ID="divRace4" class="title_Race4" OnClientClick="title_Race4Click(divRace4)" Visible=false
                                    OnClick="title_Race4Click"></asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<div class="wrapperright">
                </div>--%>
                <div class="ErrorDiv" style="width: 485px; display: none; height: 80px; margin-bottom: 20px"
                    id="divError" runat="server">
                    <div class="panel curved" style="height: 80px; width: 485px; margin-left: 260px;
                        background-color: #fff;">
                        <asp:Label ID="lblError" runat="server" BorderColor="Red" ForeColor="Red" Style="font-size: 24px;"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="buttons" style="width: 100%">
                    <div id="divBack" class="buttonitems" runat="server">
                        <asp:LinkButton runat="server" ID="btnBack" class="backbutton" OnClick="Previous_Click"></asp:LinkButton>
                    </div>
                    <div class="buttonitems">
                        <div class="cancelStart">
                            <asp:LinkButton runat="server" ID="LinkButton1" class="cancelStart" OnClick="Cancel_Restart"></asp:LinkButton>
                        </div>
                    </div>
                    <%-- <div class="buttonitems">
                        <a href="TermsAndCondition.aspx?page=FurtherPersonalDetails2">
                            <img src="<%= Resources.GlobalResources.TCImg%>" style="border: none" alt="" /></a></div>--%>
                    <div class="buttonspacer" id="divspacer" runat="server">
                        &nbsp;</div>
                    <div class="buttonitems last">
                        <div runat="server" id="divnext" class="nextbutton" style="display:none;">
                        </div>
                        <div id="divs" runat="server" class="buttonitemsConfirm">
                            <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,DOBAndHHConfirmImg%>" ID="divnextbutton" runat="server"
                                OnClick="Next_Click" />
                            <%--<asp:ImageButton runat="server" ID="divnextbutton1" class="nextbuttongrey" OnClick="Next_Click"></asp:ImageButton>--%>
                        </div>
                        <div class="buttonsummary" runat="server" id="divsummary" style="display: none;">
                            <asp:ImageButton ImageUrl="<%$ Resources:GlobalResources,BackToSummaryBtn%>" ID="imgConfirmSummary" runat="server"
                                OnClick="Next_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--buttons-->
    </div>
    <div class="keyboard">
        <script type="text/javascript" language="javascript">
            //window.external.showKeyboardTemplate('StandardUpperNE');
             
        </script>
    </div>
    </form>
</body>
</html>
