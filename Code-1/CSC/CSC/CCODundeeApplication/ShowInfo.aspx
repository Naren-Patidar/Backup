<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowInfo.aspx.cs" Inherits="CCODundeeApplication.ShowInfo"  Title="points" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="~/CSS/core.css" />
    <script language="javascript" type="text/javascript">
//        function loadValues() {
//            var opener = window.opener;
//            document.getElementById("modalCardNumber").innerHTML = opener.document.getElementById("modalCardNumber").innerHTML;
//            document.getElementById("modalDateofTransaction").innerHTML = opener.document.getElementById("modalDateofTransaction").innerHTML;
//            document.getElementById("modalTransactionType").innerHTML = opener.document.getElementById("modalTransactionType").innerHTML;
//            document.getElementById("modalTotalPoints").innerHTML = opener.document.getElementById("modalTotalPoints").innerHTML;
//            document.getElementById("modalAmountSpent").innerHTML = opener.document.getElementById("modalAmountSpent").innerHTML;
//        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="modalBox" class="modalBox" style="display:block;">
            <div>
                <table class="vouchDtlTbl">
                    <thead>
                        <tr>
                            <th class="rounded-company first">
                                <label for="Transaction Details" ><asp:Localize ID="lclTxnDetails" 
                                    runat="server" Text="Transaction Details" 
                                    meta:resourcekey="lclTxnDetailsResource1"></asp:Localize></label>
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
                            <td>
                                <label for="Card Number"><asp:Localize ID="lclCardNumber" runat="server" 
                                    Text="Card Number" meta:resourcekey="lclCardNumberResource1"></asp:Localize></label>
                            </td>
                            <td class="last">
                                <span id="modalCardNumber" runat="server"></span>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td>
                                <label for="Date of transaction"><asp:Localize ID="lclDateoftransaction" 
                                    runat="server" Text="Date of transaction" 
                                    meta:resourcekey="lclDateoftransactionResource1"></asp:Localize></label>
                            </td>
                            <td class="last">
                                <span id="modalDateofTransaction" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                             <label for="Transaction type"><asp:Localize ID="lclTransactiontype" runat="server" 
                                    Text="Transaction type" meta:resourcekey="lclTransactiontypeResource1"></asp:Localize></label>                         
                             </td>
                            <td class="last">
                                <span id="modalTransactionType" runat="server"></span>
                            </td>
                        </tr>
                         <tr class="alternate">
                            <td>
                            <label for="Transaction Reason"><asp:Localize ID="lclTransactionReason" 
                                    runat="server" Text="Transaction Reason" 
                                    meta:resourcekey="lclTransactionReasonResource1"></asp:Localize></label>                                 
                            </td>
                            <td class="last">
                                <span id="modalTransReason" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                             <label for=" Store/Partner"><asp:Localize ID="lclStore" runat="server" 
                                    Text="Store/Partner" meta:resourcekey="lclStoreResource1"></asp:Localize></label>                               
                            </td>
                            <td class="last">
                                <span id="modalStoreorPartner" runat="server"></span>
                            </td>
                        </tr>
                        
                        <tr class="alternate">
                            <td>
                            <label for="POS Number"><asp:Localize ID="lclPOSNumber" runat="server" 
                                    Text="POS Number" meta:resourcekey="lclPOSNumberResource1"></asp:Localize></label>  
                            </td>               
                            <td class="last">
                                <span id="modalPosNumber" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <label for="Receipt Number"><asp:Localize ID="lclReceiptNumber" runat="server" 
                                    Text="Receipt Number" meta:resourcekey="lclReceiptNumberResource1"></asp:Localize></label>                                 
                            </td>
                            <td class="last">
                                <span id="modalReceiptNumber" runat="server"></span>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td class="lastRw">
                            <label for="Amount Spent"><asp:Localize ID="lclAmountSpent" runat="server" 
                                    Text="Amount Spent" meta:resourcekey="lclAmountSpentResource1"></asp:Localize></label>
                            </td>
                            <td class="lastRw last">
                                <span id="modalAmountSpent" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lastRw">
                            <label for="Points"><asp:Localize ID="lclPoints" runat="server" Text="Points" 
                                    meta:resourcekey="lclPointsResource1"></asp:Localize></label>
                            </td>
                            <td class="lastRw last">
                                <span id="modalPoints" runat="server"></span>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td class="lastRw">
                              <label for="Product Points"><asp:Localize ID="lclProductPoints" runat="server" 
                                    Text="Product Points" meta:resourcekey="lclProductPointsResource1"></asp:Localize></label>
                            </td>
                            <td class="lastRw last">
                                <span id="modalProductPoints" runat="server"></span>
                            </td>
                        </tr>
                         <tr>
                            <td class="lastRw">
                             <label for="Welcome Points"><asp:Localize ID="lclWelcomePoints" runat="server" 
                                    Text="Welcome Points" meta:resourcekey="lclWelcomePointsResource1"></asp:Localize></label>
                            </td>
                            <td class="lastRw last">
                                <span id="modalWelcomePoints" runat="server"></span>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td class="lastRw">
                             <label for=" TxnSTPFPoints"><asp:Localize ID="lclTxnSTPFPoints" runat="server" 
                                    Text="TxnSTPFPoints" meta:resourcekey="lclTxnSTPFPointsResource1"></asp:Localize></label>
                            </td>
                            <td class="lastRw last">
                                <span id="modalTxnSTPFPoints" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="lastRw">
                             <label for="Partner Points"><asp:Localize ID="lclPartnerPoints" runat="server" 
                                    Text="Partner Points" meta:resourcekey="lclPartnerPointsResource1"></asp:Localize></label>
                            </td>
                            <td class="lastRw last">
                                <span id="modalPartnerPoints" runat="server"></span>
                            </td>
                        </tr>
                        <tr class="alternate">
                            <td>
                             <label for="Total points"><asp:Localize ID="lclTotalpoints" runat="server" 
                                    Text="Total points" meta:resourcekey="lclTotalpointsResource1"></asp:Localize></label>
                            </td>
                            <td class="last">
                                <span id="modalTotalPoints" runat="server"></span>
                            </td>
                        </tr>
                       
                    </tbody>
                </table>
            </div>
            <p class="pageAction" style="margin-bottom:10px">
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
