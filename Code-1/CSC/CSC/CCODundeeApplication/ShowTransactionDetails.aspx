<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowTransactionDetails.aspx.cs"
    Inherits="CCODundeeApplication.ShowTransactionDetails" Title="ShowTransactionDetails" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowInfo.aspx.cs" Inherits="ClubcardOnline.Web.MyAccount.Points.ShowInfo"  Title="points"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="~/CSS/core.css" />
    <script language="javascript" type="text/javascript">
        function loadValues() {
            var opener = window.opener;
            document.getElementById("modalCardNumber").innerHTML = opener.document.getElementById("modalCardNumber").innerHTML;
            document.getElementById("modalDateofTransaction").innerHTML = opener.document.getElementById("modalDateofTransaction").innerHTML;
            document.getElementById("modalTransactionType").innerHTML = opener.document.getElementById("modalTransactionType").innerHTML;
            document.getElementById("modalTotalPoints").innerHTML = opener.document.getElementById("modalTotalPoints").innerHTML;
            document.getElementById("modalAmountSpent").innerHTML = opener.document.getElementById("modalAmountSpent").innerHTML;
        }
    </script>
</head>
<body onload="loadValues()">
    <form id="form1" runat="server">
    <div id="modalBox" class="modalBox" style="display:block;">
            <div>
                <table class="vouchDtlTbl">
                    <thead>
                        <tr>
                            <th class="rounded-company first">
                                Transaction Details
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
                     <tr>
                            <td colspan="2" class="last">
                               Customer
                            </td>
                          
                        </tr>
                        <tr>
                            <td>
                               Customer Name
                            </td>
                            <td class="last">
                                <span id="modalCustomerName"></span>
                            </td>
                        </tr>
                        <tr  class="alternate">
                            <td>
                                Card Number
                            </td>
                            <td class="last">
                                <span id="modalCardNumber"></span>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="2" class="last">
                               Transaction Identification
                            </td>
                          
                        </tr>
                        <tr>
                            <td>
                                UserName
                            </td>
                            <td class="last">
                                <span id="modalUserName"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date of transaction
                            </td>
                            <td class="last">
                                <span id="modalDateofTransaction"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Transaction type
                            </td>
                            <td class="last">
                                <span id="modalTransactionType"></span>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td>
                                Total points
                            </td>
                            <td class="last">
                                <span id="modalTotalPoints"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lastRw">
                                Amount Spent
                            </td>
                            <td class="lastRw last">
                                <span id="modalAmountSpent"></span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="pageAction" style="margin-bottom:10px">
                <a onclick="javascript:window.close();" href="javascript:void(0);">
                    <asp:Image ID="btnCloseThisWindow" CssClass="imgBtn" AlternateText="Close this window"
                        ImageUrl="~/I/closethiswindow.gif" runat="server" /></a>
            </p>
            <span class="voucherBtm">&nbsp;</span>
        </div>
    </form>
</body>
</html>
