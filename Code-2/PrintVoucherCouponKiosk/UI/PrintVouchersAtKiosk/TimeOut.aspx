<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeOut.aspx.cs" Inherits="PrintVouchersAtKiosk.TimeOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Time Out</title>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
<script type="text/javascript" language="javascript">
                window.external.showKeyboard(0);
            </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">

        <div id="header">
            <div id="navigation"><img src="images/title-5.png" width="838" height="37"></div>
            <div id="breadcrumbs"><img src="images/crumb1.png"width="1152" height="45"></div>
        </div>

       <div id="body_wrapper">
            <div id="contentCancel">
                <div class="boxbckgrd curved">
                    <div class="box">
                        <p>Sorry to interrupt, but no one has<br />
                        touched the screen in a while.<br /></p>
                    </div>
                </div>
                <div class="clear confirm">
                    <asp:LinkButton ID="lnkNext" runat="server" onclick="Cancel_Restart"><span class="twolines">I'M THE NEXT CUSTOMER</span></asp:LinkButton>
                </div>
            </div>
            
            <div class="clear22"></div>
       </div>
        <!--body_wrapper-->
    </div>
    <!--wrapper-->
    </form>
</body>
</html>
