<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClubcardCharter1.aspx.cs"
    Inherits="Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk.ClubcardCharter1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= Resources.GlobalResources.CC1Title%></title>
    <link rel="stylesheet" href="../CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
        <script type="text/javascript" language="javascript">
            window.external.showKeyboard(0);

            function EnableCC() {
                var enableCC1 = '<%= ConfigurationManager.AppSettings["ShowClubcardCharter1"].ToString().ToUpperInvariant() %>';
                var enableCC2 = '<%= ConfigurationManager.AppSettings["ShowClubcardCharter2"].ToString().ToUpperInvariant()%>';
                var enableCC3 = '<%= ConfigurationManager.AppSettings["ShowClubcardCharter3"].ToString().ToUpperInvariant() %>';
                var enableCC4 = '<%= ConfigurationManager.AppSettings["ShowClubcardCharter4"].ToString().ToUpperInvariant() %>';
                var enableCC5 = '<%= ConfigurationManager.AppSettings["ShowClubcardCharter5"].ToString().ToUpperInvariant() %>';

                document.getElementById('lnkTC1').className = 'TC1highlight';

                if (enableCC1 == 'FALSE') {
                    document.getElementById('lnkTC1').style.display = "none";
                }
                else {
                    document.getElementById('lnkTC1').style.display = "block";
                }

                if (enableCC2 == 'FALSE') {
                    document.getElementById('lnkTC2').style.display = "none";
                }
                else {
                    document.getElementById('lnkTC2').style.display = "block";
                }

                if (enableCC3 == 'FALSE') {
                    document.getElementById('lnkTC3').style.display = "none";
                }
                else {
                    document.getElementById('lnkTC3').style.display = "block";
                }

                if (enableCC4 == 'FALSE') {
                    document.getElementById('lnkTC4').style.display = "none";
                }
                else {
                    document.getElementById('lnkTC4').style.display = "block";
                }

                if (enableCC5 == 'FALSE') {
                    document.getElementById('lnkTC5').style.display = "none";
                }
                else {
                    document.getElementById('lnkTC5').style.display = "block";
                }
            }

            function showCC1() {
                document.getElementById('CCPage1Text').style.display = "block";
                document.getElementById('CCPage2Text').style.display = "none";
                document.getElementById('CCPage3Text').style.display = "none";
                document.getElementById('CCPage4Text').style.display = "none";
                document.getElementById('CCPage5Text').style.display = "none";

                document.getElementById('lnkTC1').className = 'TC1highlight';
                document.getElementById('lnkTC2').className = 'TC2normal';
                document.getElementById('lnkTC3').className = 'TC3normal';
                document.getElementById('lnkTC4').className = 'TC4normal';
                document.getElementById('lnkTC5').className = 'TC5normal';
            }

            function showCC2() {
                document.getElementById('CCPage1Text').style.display = "none";
                document.getElementById('CCPage2Text').style.display = "block";
                document.getElementById('CCPage3Text').style.display = "none";
                document.getElementById('CCPage4Text').style.display = "none";
                document.getElementById('CCPage5Text').style.display = "none";

                document.getElementById('lnkTC1').className = 'TC1normal';
                document.getElementById('lnkTC2').className = 'TC2highlight';
                document.getElementById('lnkTC3').className = 'TC3normal';
                document.getElementById('lnkTC4').className = 'TC4normal';
                document.getElementById('lnkTC5').className = 'TC5normal';
            }

            function showCC3() {
                document.getElementById('CCPage1Text').style.display = "none";
                document.getElementById('CCPage2Text').style.display = "none";
                document.getElementById('CCPage3Text').style.display = "block";
                document.getElementById('CCPage4Text').style.display = "none";
                document.getElementById('CCPage5Text').style.display = "none";

                document.getElementById('lnkTC1').className = 'TC1normal';
                document.getElementById('lnkTC2').className = 'TC2normal';
                document.getElementById('lnkTC3').className = 'TC3highlight';
                document.getElementById('lnkTC4').className = 'TC4normal';
                document.getElementById('lnkTC5').className = 'TC5normal';

            }

            function showCC4() {
                document.getElementById('CCPage1Text').style.display = "none";
                document.getElementById('CCPage2Text').style.display = "none";
                document.getElementById('CCPage3Text').style.display = "none";
                document.getElementById('CCPage4Text').style.display = "block";
                document.getElementById('CCPage5Text').style.display = "none";

                document.getElementById('lnkTC1').className = 'TC1normal';
                document.getElementById('lnkTC2').className = 'TC2normal';
                document.getElementById('lnkTC3').className = 'TC3normal';
                document.getElementById('lnkTC4').className = 'TC4highlight';
                document.getElementById('lnkTC5').className = 'TC5normal';
            }

            function showCC5() {
                document.getElementById('CCPage1Text').style.display = "none";
                document.getElementById('CCPage2Text').style.display = "none";
                document.getElementById('CCPage3Text').style.display = "none";
                document.getElementById('CCPage4Text').style.display = "none";
                document.getElementById('CCPage5Text').style.display = "block";

                document.getElementById('lnkTC1').className = 'TC1normal';
                document.getElementById('lnkTC2').className = 'TC2normal';
                document.getElementById('lnkTC3').className = 'TC3normal';
                document.getElementById('lnkTC4').className = 'TC4normal';
                document.getElementById('lnkTC5').className = 'TC5highlight';
            }

    </script>

</head>
<body onload="EnableCC();">
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgLogo" runat="server" ImageUrl="<%$ Resources:GlobalResources,CardLogo%>"
                    Width="234" Height="24" />
                <em>
                    <%= Resources.GlobalResources.CC1Head%></em>
            </div>
        </div>
        <div id="body_wrapper">
            <div id="contentCharter">
                <div class="wrappercentre">
                    <div class="panel1 curved"  style="height:550px;">
                    <div style="height:410px;">
                  
                        <span id="CCPage1Text"> <%=Resources.GlobalResources.CCPage1%></span>
                        <span id="CCPage2Text" style="display:none;" > <%=Resources.GlobalResources.CCPage2%></span>
                        <span id="CCPage3Text" style="display:none;"> <%=Resources.GlobalResources.CCPage3%></span>
                        <span id="CCPage4Text" style="display:none;"> <%=Resources.GlobalResources.CCPage4%></span>
                        <span id="CCPage5Text" style="display:none;" ><%=Resources.GlobalResources.CCPage5%></span>
              
                    </div>
                      <div class="buttons">
                            <a ID="lnkTC1" onclick="showCC1()" class="TC1normal" style="display:none;"> </a>
                            <a ID="lnkTC2" onclick="showCC2()" class="TC2normal" style="display:none;"> </a>
                            <a ID="lnkTC3" onclick="showCC3()" class="TC3normal" style="display:none;"> </a>
                            <a ID="lnkTC4" onclick="showCC4()" class="TC4normal" style="display:none;">  </a>
                            <a ID="lnkTC5" onclick="showCC5()" class="TC5normal" style="display:none;"> </a>
                        </div>     
                       
                    </div>
                    <div class="clear">
                    </div>
                   <div class="backbutton">
                        <asp:ImageButton ID="ImageButton1" OnClick="Back_Click" ImageUrl="<%$ Resources:GlobalResources,BackBtn%>"
                            runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
