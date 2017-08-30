<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePrimaryCard.aspx.cs" Title="Select Primary Card" Inherits="CCODundeeApplication.ChangePrimaryCard" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="~/CSS/core.css" />

    <script language="javascript" type="text/javascript">
//        function loadValues() {
//            var opener = window.opener;
//            document.getElementById("ddlClubcards.ClientID").innerHTML = opener.document.getElementById("ddlClubcards.ClientID").innerHTML;
//        }
        function refreshParent() 
        {
         //alert('Primary Card has been changed successfully.');
         var AlertforPrimaryCard='<%=Resources.CSCGlobal.AlertforPrimaryCard %>';
         alert(AlertforPrimaryCard);
          window.opener.location.href = window.opener.location.href;

          if (window.opener.progressWindow)
            {
                window.opener.progressWindow.close()
            }
          window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="modalBox" class="modalBox" style="display: block;">
        <div>
            <table class="vouchDtlTbl">
                <thead>
                    <tr>
                        <th class="rounded-company first">
                            <label for="Select Primary Card"><asp:Localize ID="lclSelectPrimaryCard" 
                                runat="server" Text="Select Primary Card" 
                                meta:resourcekey="lclSelectPrimaryCardResource1"></asp:Localize></label>
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
                        <label for="Card Number"><asp:Localize ID="lclCardNumber" runat="server" 
                                Text="Card Number" meta:resourcekey="lclCardNumberResource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <asp:DropDownList ID="ddlClubcards" AutoPostBack="True"  runat="server" 
                                meta:resourcekey="ddlClubcardsResource1">
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
                            <asp:ImageButton ID="imgConfirm" ImageUrl="~/I/confirm.gif" runat="server" AlternateText="Confirm" 
                                OnClick="imgConfirm_Click" meta:resourcekey="imgConfirmResource1" />
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
