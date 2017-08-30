<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowCallDetails.aspx.cs"
    Inherits="CCODundeeApplication.ShowCallDetails" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="~/CSS/core.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="lblHeader" class="alertMsgs" 
            meta:resourcekey="lblCallDetailsResource1"></asp:Label>
    </div>
    <div id="modalBox" class="modalBox" style="display: block;">
        <div>
            <table class="vouchDtlTbl">
                <thead>
                    <tr>
                        <th class="rounded-company first">
                            <asp:Label runat="server" ID="lblCallDetail" meta:resourcekey="lblHeaderResource1">Call Details</asp:Label>
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
                            <asp:Label ID="lblDateLogged" runat="server" meta:resourcekey="lblHeaderResource2">Date Logged</asp:Label>
                            
                        </td>
                        <td class="last">
                            <span id="modalDateLogged" runat="server"></span>
                        </td>
                    </tr>
                    <tr class="alternate">
                        <td>
                            <asp:Label ID="lblLoggedBy" runat="server" meta:resourcekey="lblHeaderResource3">Logged By</asp:Label>
                        </td>
                        <td class="last">
                            <span id="modalLoggedBy" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCallReason" runat="server" meta:resourcekey="lblHeaderResource4">Call Reason</asp:Label>
                        </td>
                        <td class="last">
                            <span id="modalReason" runat="server"></span>
                        </td>
                    </tr>
                    <tr class="alternate">
                        <td>
                            <asp:Label ID="lblCallDetails" runat="server" meta:resourcekey="lblHeaderResource5">Call Details</asp:Label>
                        </td>
                        <td class="last">
                            <asp:TextBox ID="txtCallDteails" runat="server" TextMode="MultiLine" 
                                Height="115px" Width="433px" meta:resourcekey="txtCallDteailsResource1"></asp:TextBox>
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
