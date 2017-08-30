<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FindCoupon.aspx.cs" Inherits="CCODundeeApplication.FindCoupon"
    MasterPageFile="~/Site.Master" Title="Find Coupon" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div>
                <div class="cc_bluehead">
                    <h3>
                        <asp:Localize Text="Find Coupon" runat="server" ID="lclFindCoupon" 
                            meta:resourcekey="lclFindCouponResource1"></asp:Localize></h3>
                </div>
                <div class="cc_body">
                    <br />
                    <br />
                    <ul class="customer">
                        <li>
                            <label for="CouponBarcode" style="width: 150px">
                                <asp:Localize Text="Coupon Barcode :" runat="server" ID="lclCouponBacode" 
                                meta:resourcekey="lclCouponBacodeResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox ID="txtCouponBarcode" runat="server" 
                                    meta:resourcekey="txtCouponBarcodeResource1"></asp:TextBox>
                                <span id="spanCouponBarcode" class="errorFields" style="<%=spanCouponBarcode%>">
                                </span>
                            </div>
                        </li>
                        <li>
                            <label for="OnlineCode" style="width: 150px">
                                <asp:Localize Text="Online Code :" runat="server" ID="lclOnlineCd" 
                                meta:resourcekey="lclOnlineCdResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox ID="txtOnlineCode" runat="server" 
                                    meta:resourcekey="txtOnlineCodeResource1" />
                                <span id="spanOnlineCode" class="errorFields" style="<%=spanOnlineCode%>"></span>
                            </div>
                        </li>
                    </ul>
                    <div id="Printmyvouchers">
                        <span class="button" style="margin-right: 200px"><span class="hide"></span><span
                            class="buttonbar" id="printvouchers">
                            <asp:Button runat="server" ID="btnFindCoupon" Text="Find Coupon" BackColor="#49BCD7" ForeColor="White" style="text-align:center;"
                             Font-Bold="True" Height="24px" Width="83px"
                            OnClick="btnFindCoupon_Click" meta:resourcekey="btnFindCouponResource1" />
                        </span><span class="buttonend"></span></span>
                    </div>
                    <br />
                    <br />
                    <span class="errorFields" runat="server" id="spnErrorMsg" visible="false" style="margin-left:150px">
                    <asp:Localize Text="Please enter a valid Coupon Barcode or Online Code" 
                        ID="lclValidBarcode" runat="server" meta:resourcekey="lclValidBarcodeResource1"></asp:Localize></span>
                </div>
            </div>
            <br />
            <br />
            
            <br />
            <div id="dvCouponInfo" runat="server" visible="false">
                <div class="clubcardAct_head" style="width:99%;margin-left:3px">
                    <h4>
                        <asp:Literal ID="ltrCouponDtl" runat="server" Text="Coupon Details" 
                            meta:resourcekey="ltrCouponDtlResource1"></asp:Literal></h4>
                </div>
                <table class="tblCoupon" style="width:99%;margin-left:3px">
                    <thead>
                        <tr>
                            <th>
                                <asp:Localize Text="Online Code" runat="server" ID="lclOnlineCode" 
                                    meta:resourcekey="lclOnlineCodeResource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize Text="Barcode Number" runat="server" ID="lclBarNumber" 
                                    meta:resourcekey="lclBarNumberResource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize Text="Full Coupon Description" runat="server" ID="lclFullDesc" 
                                    meta:resourcekey="lclFullDescResource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize Text="Coupon Status" runat="server" ID="lclCouponStatus" 
                                    meta:resourcekey="lclCouponStatusResource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize Text="Expiry Date" runat="server" ID="lclExpDate" 
                                    meta:resourcekey="lclExpDateResource1"></asp:Localize>
                            </th>
                            <th>
                                <asp:Localize Text="Total Redemptions" runat="server" ID="lclTtlRedem" 
                                    meta:resourcekey="lclTtlRedemResource1"></asp:Localize>
                            </th>
                            <th class="last">
                                <asp:Localize Text="Redemptions Remaining" runat="server" 
                                    ID="lclRedemRemaining" meta:resourcekey="lclRedemRemainingResource1"></asp:Localize>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblOnlineCode" runat="server" 
                                meta:resourcekey="lblOnlineCodeResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblBarcodeNo" runat="server" 
                                meta:resourcekey="lblBarcodeNoResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblCouponDescr" runat="server" 
                                meta:resourcekey="lblCouponDescrResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblCouponStatus" runat="server" 
                                meta:resourcekey="lblCouponStatusResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblExpDate" runat="server" 
                                meta:resourcekey="lblExpDateResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblTotalRedmtns" runat="server" 
                                meta:resourcekey="lblTotalRedmtnsResource1"></asp:Label>
                        </td>
                        <td class="last" align="center">
                            <asp:Label ID="lblRedmtnsRemain" runat="server" 
                                meta:resourcekey="lblRedmtnsRemainResource1"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div class="clubcardAct_head" style="width:99%;margin-left:3px">
                    <h4>
                        <asp:Literal ID="ltrCouponIssuance" runat="server" Text="Coupon Issuance" 
                            meta:resourcekey="ltrCouponIssuanceResource1"></asp:Literal></h4>
                </div>
                <table class="tbl" style="width:99%;margin-left:3px">
                    <thead>
                        <tr>
                            <th>
                                <asp:Localize Text="Issue Date Time" runat="server" ID="lclIssueDatetime" 
                                    meta:resourcekey="lclIssueDatetimeResource1"></asp:Localize>
                            </th>
                            <th style="width: 20%">
                                <asp:Localize Text="Issuance Channel" runat="server" ID="lclIssueChanel" 
                                    meta:resourcekey="lclIssueChanelResource1"></asp:Localize>
                            </th>
                            <th class="last">
                                <asp:Localize Text="Issuing Store" runat="server" ID="lclIssueStore" 
                                    meta:resourcekey="lclIssueStoreResource1"></asp:Localize>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblIssueDateTime" runat="server" 
                                meta:resourcekey="lblIssueDateTimeResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblIssuanceChannel" runat="server" 
                                meta:resourcekey="lblIssuanceChannelResource1"></asp:Label>
                        </td>
                        <td class="last" align="center">
                            <asp:Label ID="lblIssueStore" runat="server" 
                                meta:resourcekey="lblIssueStoreResource1"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div class="clubcardAct_head" style="width:99%;margin-left:3px">
                    <h4>
                        <asp:Literal ID="ltrCpnRedmtn" runat="server" Text="Coupon Redemption" 
                            meta:resourcekey="ltrCpnRedmtnResource1"></asp:Literal></h4>
                </div>
                <table class="tbl" style="width:99%;margin-left:3px">
                    <thead>
                        <tr>
                            <th>
                                <asp:Localize Text="Clubcard Number" runat="server" ID="lclClubNum" 
                                    meta:resourcekey="lclClubNumResource1"></asp:Localize>
                            </th>
                            <th style="width: 20%">
                                <asp:Localize Text="Date Time of Redemption" runat="server" 
                                    ID="lclDatetimeofRedem" meta:resourcekey="lclDatetimeofRedemResource1"></asp:Localize>
                            </th>
                            <th style="width: 20%">
                                <asp:Localize Text="Place Redeemed" runat="server" ID="lclPlaceRedmd" 
                                    meta:resourcekey="lclPlaceRedmdResource1"></asp:Localize>
                            </th>
                            <th class="last">
                                <asp:Localize Text="Transaction Type" runat="server" ID="lclTxnType" 
                                    meta:resourcekey="lclTxnTypeResource1"></asp:Localize>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblClubcardNo" runat="server" 
                                meta:resourcekey="lblClubcardNoResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblRedemtnDate" runat="server" 
                                meta:resourcekey="lblRedemtnDateResource1"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblRedemdPlace" runat="server" 
                                meta:resourcekey="lblRedemdPlaceResource1"></asp:Label>
                        </td>
                        <td class="last" align="center">
                            <asp:Label ID="lblTransType" runat="server" 
                                meta:resourcekey="lblTransTypeResource1"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
