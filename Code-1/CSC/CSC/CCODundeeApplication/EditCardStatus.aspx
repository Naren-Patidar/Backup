<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCardStatus.aspx.cs"
    Inherits="CCODundeeApplication.EditCardStatus" Title="Edit Card Status" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="~/CSS/core.css" />

    <script language="javascript" type="text/javascript">
        function loadValues() {
            var opener = window.opener;
           // document.getElementById("modalCardNumber").innerHTML = opener.document.getElementById("modalCardNumber").innerHTML;
        }
        function refreshParent(cardStatus)
        {
             var AlertMsg1='<%=Resources.CSCGlobal.AlertMsg1 %>'//Your card has been successfully blocked and cannot be used for redemption!!
             var AlertMsg2='<%=Resources.CSCGlobal.AlertMsg2 %>'//Your cad status has been changed successfully
            if(cardStatus == 'LOSTSTOLENDAMAGED')
            {
                //alert('Your card has been successfully blocked and cannot be used for redemption!!');
                alert(AlertMsg1);
            }
            else
            {
                //alert('Your cad status has been changed successfully');
                alert(AlertMsg2);
            }
            window.opener.location.href = window.opener.location.href;
            if (window.opener.progressWindow)
            {
                window.opener.progressWindow.close()
            }
            window.close();
        }
        function ChangeCardStatus()
        {
            var status = document.getElementById(ddlStatusClientId).options[document.getElementById(ddlStatusClientId).selectedIndex].text;
            if(status.toUpperCase() == 'LOSTSTOLENDAMAGED')
            {
                if (!confirm("Are you sure you would like to block your card?"))
                    return false;
            }
            else
            {
                var message  = '<%=Resources.CSCGlobal.ConfirmationStatus %>' + status + "?";
                if (!confirm(message))
                    return false;
            }                    
        }
    </script>

</head>
<body onload="loadValues()">
    <form id="form1" runat="server">
    <div id="modalBox" class="modalBox" style="display: block;">
        <div>
            <table class="vouchDtlTbl">
                <thead>
                    <tr>
                        <th class="rounded-company first">
                           <label for="Edit Card Details"><asp:Localize ID="lclTitle" runat="server" 
                                Text="Edit Card Details" meta:resourcekey="lclTitleResource1"></asp:Localize></label>
                        </th>
                        <th class="rounded-q4 pointDetailPopup">
                            <!--Empty-->
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td class="rounded-Greyfoot-left">
                            <!--Empty-->
                        </td>
                        <td class="rounded-Greyfoot-right">
                            <!--Empty-->
                        </td>
                    </tr>
                </tfoot>
                <tbody>
                    <tr class="alternate">
                        <td>
                             <label for="Status"><asp:Localize ID="lclStatus" runat="server" Text="Status" 
                                 meta:resourcekey="lclStatusResource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <asp:DropDownList ID="ddlStatus" runat="server" 
                                meta:resourcekey="ddlStatusResource1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="last">
                        </td>
                    </tr>
                    <tr class="alternate">
                        <td>
                        </td>
                        <td class="last">
                        </td>
                    </tr>
                    <tr>
                        <td class="lastRw" colspan="2">
                            <asp:ImageButton ID="imgConfirm" ImageUrl="~/I/confirm.gif" runat="server" OnClick="imgConfirm_Click"
                                OnClientClick="return ChangeCardStatus();" 
                                meta:resourcekey="imgConfirmResource1" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <p class="pageAction" style="margin-bottom: 10px">
            <a onclick="javascript:window.close();" href="javascript:void(0);">
                <asp:Image ID="btnCloseThisWindow" CssClass="imgBtn" AlternateText="Close this window"
                    ImageUrl="~/I/closethiswindow.gif" runat="server" 
                meta:resourcekey="btnCloseThisWindowResource1" /></a>
        </p>
        <span class="voucherBtm">&nbsp;</span>
    </div>
    </form>
</body>
</html>
