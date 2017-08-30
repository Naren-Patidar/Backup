<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerCoupons.aspx.cs"
    MasterPageFile="~/Site.Master" Inherits="CCODundeeApplication.CustomerCoupons"
    Title="Customer Coupons" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div>
            <h3 style="height: 20px">
                &nbsp;&nbsp;<asp:Localize ID="lclcouponsmry" runat="server" Text="Coupons Summary"
                    meta:resourcekey="lclcouponsmryResource1"></asp:Localize></h3>
            <br />
            <br />
            <div id="dvCouponDetail" runat="server" class="ccBlueHeaderSection">
                <div class="cc_bluehead">
                    <h3 style="height: 20px">
                        <asp:Localize ID="Localize1" runat="server" Text="Active coupons" meta:resourcekey="Localize1Resource1"></asp:Localize></h3>
                </div>
                <div id="dvMsgActiveCoupons" runat="server" visible="false" class="pageMessage">
                    <br />
                    <p style="margin-left: 200px">
                        <label for="coupons" style="width: 100%">
                            <asp:Localize ID="lclNoActive" runat="server" Text="No active coupons at the moment."
                                meta:resourcekey="lclNoActiveResource1"></asp:Localize></label></p>
                </div>
                <div id="Div2" runat="server" style="overflow:auto;width: 761px;">
                <asp:Repeater ID="rptCouponDetails" runat="server" OnItemDataBound="rptCouponDetails_ItemDataBound">
                    <HeaderTemplate>
                        <table class="pointsSmTbl" style="width: 600; overflow-x: scroll;">
                            <thead>
                                <tr>
                                    <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1"></asp:Literal>
                                    <th class="rounded-company first" style="width: 17%">
                                        <asp:Localize ID="lclcouponsmry" runat="server" Text="Issue date" meta:resourcekey="lclcouponsmryResource2"></asp:Localize>
                                    </th>
                                    <th style="width: 17%">
                                        <asp:Localize ID="lclExpiry" runat="server" Text="Expiry date" meta:resourcekey="lclExpiryResource1"></asp:Localize>
                                    </th>
                                    <th>
                                        <asp:Localize ID="lclfulCoupon" runat="server" Text="Full coupon description" meta:resourcekey="lclfulCouponResource1"></asp:Localize>
                                    </th>
                                    <th>
                                        <asp:Localize ID="lclOnlinecode" runat="server" Text="Online code" meta:resourcekey="lclOnlinecodeResource1"></asp:Localize>
                                    </th>
                                    <th id="thBarCode" runat="server">
                                        <asp:Localize ID="lclBarCode" runat="server" Text="Barcode number" meta:resourcekey="lclBarCodeResource1"></asp:Localize>
                                    </th>
                                    <th id="thRedemptionremain" runat="server">
                                        <asp:Localize ID="lclRedemptionRemain" runat="server" Text="Redemption remaining"
                                            meta:resourcekey="lclRedemptionRemainResource"></asp:Localize>
                                    </th>
                                    <th class="rounded-q4" id="thTotRedemptions" runat="server">
                                        <asp:Localize ID="lclTotRedemptions" runat="server" Text="Total redemptions" meta:resourcekey="lclTotRedemptionsResource"></asp:Localize>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="center">
                                <asp:Literal ID="ltrRedemptionStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IssuanceStartDate") %>'></asp:Literal>
                            </td>
                            <td class="center">
                                <asp:Literal ID="ltrRedemptionEndDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionEndDate") %>'></asp:Literal>
                            </td>
                            <td class="center">
                                <asp:Literal ID="ltrOffer" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CouponDescription") %>'></asp:Literal>
                            </td>
                            <td class="center">
                                <asp:Literal ID="ltrOnlineCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SmartAlphaNumeric") %>'></asp:Literal>
                            </td>
                            <td class="center" id="tdBarCode" runat="server">
                                <asp:Literal ID="ltrBarcode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SmartBarcode") %>'></asp:Literal>
                            </td>
                            <td class="center" id="tdRedemptionremain" runat="server">
                                <asp:Literal ID="ltrRedemptionRemain" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxRedemptionLimit") %>'></asp:Literal>
                            </td>
                            <td class="center last" id="tdTotRedemptions" runat="server">
                                <asp:Literal ID="ltrTotRedemptions" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxRedemptionLimit") %>'></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate">
                            <td class="center">
                                <asp:Literal ID="ltrRedemptionStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IssuanceStartDate") %>'></asp:Literal>
                            </td>
                            <td class="center">
                                <asp:Literal ID="ltrRedemptionEndDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionEndDate") %>'></asp:Literal>
                            </td>
                            <td class="center">
                                <asp:Literal ID="ltrOffer" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CouponDescription") %>'></asp:Literal>
                            </td>
                            <td class="center">
                                <asp:Literal ID="ltrOnlineCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SmartAlphaNumeric") %>'></asp:Literal>
                            </td>
                            <td class="center" id="tdBarCode" runat="server">
                                <asp:Literal ID="ltrBarcode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SmartBarcode") %>'></asp:Literal>
                            </td>
                            <td class="center" id="tdRedemptionremain" runat="server">
                                <asp:Literal ID="ltrRedemptionRemain" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxRedemptionLimit") %>'></asp:Literal>
                            </td>
                            <td class="center last" id="tdTotRedemptions" runat="server">
                                <asp:Literal ID="ltrTotRedemptions" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxRedemptionLimit") %>'></asp:Literal>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                </div>
            </div>
            <p>
                &nbsp;<br />
            </p>
            <div id="dvRedeemedCouponDetail" runat="server" class="ccBlueHeaderSection">
                <div class="cc_bluehead">
                    <h3 style="height: 20px">
                        <asp:Localize ID="lclremcou" runat="server" Text="Redeemed coupons" meta:resourcekey="lclremcouResource1"></asp:Localize></h3>
                </div>
                <div id="dvMsgRedeemedCoupons" runat="server" visible="false" class="pageMessage">
                    <br />
                    <p style="margin-left: 100px">
                        <asp:Localize ID="lclCusRem" runat="server" Text="Customer haven't redeemed any coupons in the past 4 weeks"
                            meta:resourcekey="lclCusRemResource1"></asp:Localize></p>
                </div>
                <div id="Div1" runat="server" style="overflow:auto;width: 761px;">
                    <asp:Repeater ID="rptUsedCouponDetails" runat="server" OnItemDataBound="rptUsedCouponDetails_ItemDataBound">
                        <HeaderTemplate>
                            <table class="pointsSmTbl" style="width: 600; overflow-x: scroll;">
                                <thead>
                                    <tr>
                                        <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource2"></asp:Literal>
                                        <th class="rounded-company first" id="thClubcardNo" runat="server">
                                            <asp:Localize ID="lclClubcardNoResource" runat="server" Text="Clubcard number" meta:resourcekey="lclClubcardNoResource"></asp:Localize>
                                        </th>
                                        <th id="thBarnum" runat="server">
                                            <asp:Localize ID="lclBarnum" runat="server" Text="Issued date" meta:resourcekey="lclBarnumResource1"></asp:Localize>
                                        </th>
                                        <th>
                                            <asp:Localize ID="lclfuldesc" runat="server" Text="Full coupon description" meta:resourcekey="lclfuldescResource1"></asp:Localize>
                                        </th>
                                        <th>
                                            <asp:Localize ID="lcldate" runat="server" Text="Date of redemption" meta:resourcekey="lcldateResource1"></asp:Localize>
                                        </th>
                                        <th id="thplace" runat="server">
                                            <asp:Localize ID="lclplace" runat="server" Text="Place redeemed" meta:resourcekey="lclplaceResource1"></asp:Localize>
                                        </th>
                                        <th id="thBarcocde" runat="server">
                                            <asp:Localize ID="lclBarcocde" runat="server" Text="Barcode" meta:resourcekey="lclBarcocdeResource"></asp:Localize>
                                        </th>
                                        <th id="thCouponStatus" runat="server">
                                            <asp:Localize ID="lclCouponStatus" runat="server" Text="Used/Voided" meta:resourcekey="lclCouponStatusResource"></asp:Localize>
                                        </th>
                                        <th id="thRedemptionNo" runat="server">
                                            <asp:Localize ID="lclRedemptionNo" runat="server" Text="Redemption number" meta:resourcekey="lclRedemptionNoResource"></asp:Localize>
                                        </th>
                                        <th class="rounded-q4" id="thTotRedemption" runat="server">
                                            <asp:Localize ID="lclTotRedemption" runat="server" Text="Total redemptions" meta:resourcekey="lclTotRedemptionResource"></asp:Localize>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="center" id="tdClubCardNo" runat="server">
                                    <asp:Literal ID="ltrClubCardNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardNo") %>'></asp:Literal>
                                </td>
                                <td class="center">
                                    <asp:Literal ID="ltrIssuanceDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IssuanceStartDate") %>'></asp:Literal>
                                </td>
                                <td class="center">
                                    <asp:Literal ID="ltrDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CouponDescription") %>'></asp:Literal>
                                </td>
                                <td class="center">
                                    <asp:Literal ID="ltrRedemptionDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionDate") %>'></asp:Literal><br />
                                    <asp:Literal ID="ltrRedemptionTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionDate") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdRedemptionStore" runat="server">
                                    <asp:Literal ID="ltrRedemptionStore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionStoreName") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdBarCode" runat="server">
                                    <asp:Literal ID="ltrBarCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BarCodeNo") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdCouponStatus" runat="server">
                                    <asp:Literal ID="ltrCouponStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CouponStatus") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdRedemptionNo" runat="server">
                                    <asp:Literal ID="ltrRedemptionNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionCount") %>'></asp:Literal>
                                </td>
                                <td class="center last" id="tdTotalRedemption" runat="server">
                                    <asp:Literal ID="ltrTotalRedemption" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TotalRedemption") %>'></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate">
                                <td class="center" id="tdClubCardNo" runat="server">
                                    <asp:Literal ID="ltrClubCardNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardNo") %>'></asp:Literal>
                                </td>
                                <td class="center">
                                    <asp:Literal ID="ltrIssuanceDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IssuanceStartDate") %>'></asp:Literal>
                                </td>
                                <td class="center">
                                    <asp:Literal ID="ltrDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CouponDescription") %>'></asp:Literal>
                                </td>
                                <td class="center">
                                    <asp:Literal ID="ltrRedemptionDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionDate") %>'></asp:Literal><br />
                                    <asp:Literal ID="ltrRedemptionTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionDate") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdRedemptionStore" runat="server">
                                    <asp:Literal ID="ltrRedemptionStore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionStoreName") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdBarCode" runat="server">
                                    <asp:Literal ID="ltrBarCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BarCodeNo") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdCouponStatus" runat="server">
                                    <asp:Literal ID="ltrCouponStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CouponStatus") %>'></asp:Literal>
                                </td>
                                <td class="center" id="tdRedemptionNo" runat="server">
                                    <asp:Literal ID="ltrRedemptionNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedemptionCount") %>'></asp:Literal>
                                </td>
                                <td class="center last" id="tdTotalRedemption" runat="server">
                                    <asp:Literal ID="ltrTotalRedemption" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TotalRedemption") %>'></asp:Literal>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
