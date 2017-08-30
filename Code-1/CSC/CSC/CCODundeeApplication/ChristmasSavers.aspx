<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChristmasSavers.aspx.cs"
   MasterPageFile="~/Site.Master" Inherits="CCODundeeApplication.ChristmasSavers" UICulture="en-GB"  Title="Christmas Saver" culture="auto" meta:resourcekey="PageResource1"%>


<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="CusSavers"><asp:Localize ID="lclCusSavers" runat="server" 
                        Text="Christmas Savers customer" meta:resourcekey="lclCusSaversResource1"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                <div class="ccGreenInfoSection christmasVoucher">
                    <div class="cc_greeninfohead">
                        <!--cc_purpleinfohead-->
                        <span class="left">
                            <!--Empty-->
                        </span>
                    </div>
                    <div class="cc_greenbody">
                        <!--cc_purplebody-->
                        <div class="christmasSaverDtls">
                            <strong><%=Resources.CSCGlobal.CusSaved%></strong>
                            <div class="pointCurve_l vouchPoints">
                                <span class="pointCurve_r" id="spnTtlPnts" runat="server"></span>
                            </div>
                            <strong><%=Resources.CSCGlobal.FarNove%> <span id="spnYear1" runat="server"></span> <%=Resources.CSCGlobal.stmt%></strong>
                        </div>
                    </div>
                </div>
                <div class="christmasVoucherDtls" id="dvXmasSummary" runat="server">
                    <h4>
                        <label for="Summary"><asp:Localize ID="lclSummary" runat="server" 
                            Text="Summary for Christmas" meta:resourcekey="lclSummaryResource1"></asp:Localize></label><span id="spnYear2" runat="server"></span></h4>
                    <div class="chrisSavleft">
                        <table class="christSummary">
                            <thead>
                                <tr>
                                    <th class="rounded-whiteLft first">
                                        <!--Empty-->
                                    </th>
                                    <th class="rounded-whiteRt">
                                        <!--Empty-->
                                    </th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <td class="rounded-foot-left first">
                                        <strong><label for="TotVoucher" style="width:100%"><asp:Localize ID="lclTotVoucher" runat="server" 
                                            Text="Total vouchers so far:" meta:resourcekey="lclTotVoucherResource1"></asp:Localize></label></strong>
                                    </td>
                                    <td class="rounded-foot-right">
                                        <div class="pointCurve_l vouchPoints">
                                            <span class="pointCurve_r" id="spnTtlVouchersSoFar" runat="server"></span>
                                        </div>
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr>
                                    <td>
                                        <label for="SavedVouchers" style="width:100%"><asp:Localize ID="lclSavedVouchers" runat="server" 
                                            Text="Clubcard Vouchers saved so far" 
                                            meta:resourcekey="lclSavedVouchersResource1"></asp:Localize></label>
                                    </td>
                                    <td class="right last">
                                        <span id="spnCCVouchersSaved" runat="server"></span>
                                    </td>
                                </tr>
                                <tr class="alternate">
                                    <td>
                                        <label for="MnyTopped" style="width:100%"><asp:Localize ID="lclMnyTopped" runat="server" 
                                            Text="Money you have topped up" meta:resourcekey="lclMnyToppedResource1"></asp:Localize></label>
                                    </td>
                                    <td class="right last">
                                        <span id="ttlToppedUoMoney" runat="server"></span>
                                    </td>
                                </tr>
                                <tr id="trBonus" runat="server">
                                    <td>
                                        <label for="Bonus" style="width:100%"><asp:Localize ID="lclBonus" runat="server" 
                                            Text="Bonus voucher" meta:resourcekey="lclBonusResource1"></asp:Localize></label>
                                    </td>
                                    <td class="right last">
                                        <span id="spnBonusVoucher" runat="server"></span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="christmasVoucherDtls" id="dvVouchersSaved" runat="server">
                    <h4>
                        <label for="ccSavedVouchers" style="width:100%"><asp:Localize ID="lclccSavedVouchers" 
                            runat="server" Text="Clubcard vouchers saved so far" 
                            meta:resourcekey="lclccSavedVouchersResource1"></asp:Localize></label></h4>
                    <div class="chrisSavleft">
                        <table class="christSummary">
                            <thead>
                                <tr>
                                    <th class="rounded-company first">
                                        <strong><label for="Issuedstmt" style="width:100%"><asp:Localize ID="lclIssuedstmt" runat="server" 
                                            Text="Statement issued in" meta:resourcekey="lclIssuedstmtResource1"></asp:Localize></label></strong>
                                    </th>
                                    <th class="rounded-q4">
                                        <strong><label for="iValue" style="width:100%"><asp:Localize ID="lcliValue" runat="server" 
                                            Text="Value" meta:resourcekey="lcliValueResource1"></asp:Localize></label></strong>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <span id="spnVoucherSaved" runat="server"></span>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td class="rounded-foot-left first">
                                        <strong><label for="itot" style="width:100%"><asp:Localize ID="lclitot" runat="server" 
                                            Text="Total:" meta:resourcekey="lclitotResource1"></asp:Localize></label></strong>
                                    </td>
                                    <td class="rounded-foot-right singleCol">
                                        <div class="pointCurve_l vouchPoints">
                                            <span class="pointCurve_r" id="spnSumOfVouchersSaved" runat="server"></span>
                                        </div>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
                <div class="christmasVoucherDtls" id="dvMoneyToppedUp" runat="server">
                    <h4>
                        <label for="HavMnyTopped" style="Width:100%"><asp:Localize ID="lclHavMnyTopped" runat="server" 
                            Text="Money you have topped up" meta:resourcekey="lclHavMnyToppedResource1"></asp:Localize></label></h4>
                    <div class="chrisSavleft">
                        <asp:Repeater ID="rptXmasDetails" runat="server" OnItemDataBound="rptXmasDetails_ItemDataBound">
                            <HeaderTemplate>
                                <table class="christSummary">
                                    <thead>
                                        <tr>
                                            <th class="rounded-company first">
                                                <strong><label for="Dattetopped" style="width:100%"><asp:Localize ID="lclDattetopped" 
                                                    runat="server" Text="Date topped up" meta:resourcekey="lclDattetoppedResource1"></asp:Localize></label></strong>
                                            </th>
                                            <th class="rounded-q4">
                                                <strong><label for="dval" style="width:100%"><asp:Localize ID="lcldval" runat="server" 
                                                    Text="Value" meta:resourcekey="lcldvalResource1"></asp:Localize></label></strong>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblTransDate" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "TransactionDateTime") %>'></asp:Literal>
                                    </td>
                                    <td class="right last">
                                        <asp:Label ID="lblAmount" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "AmountSpent") %>' 
                                            meta:resourcekey="lblAmountResource2"></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate">
                                    <td>
                                        <asp:Literal ID="lblTransDate" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "TransactionDateTime") %>'></asp:Literal>
                                    </td>
                                    <td class="right last">
                                        <asp:Label ID="lblAmount" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "AmountSpent") %>' 
                                            meta:resourcekey="lblAmountResource1"></asp:Label>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <tfoot>
                                    <tr>
                                        <td class="rounded-foot-left first">
                                            <strong><label for="dtot"><asp:Localize ID="lcldtot" runat="server" 
                                                Text="Total:" meta:resourcekey="lcldtotResource1"></asp:Localize></label></strong>
                                        </td>
                                        <td class="rounded-foot-right singleCol">
                                            <div class="pointCurve_l vouchPoints">
                                                <span class="pointCurve_r">
                                                    <asp:Literal ID="LiteralTotal" runat="server" 
                                                    meta:resourcekey="LiteralTotalResource1"></asp:Literal>
                                                </span>
                                            </div>
                                        </td>
                                    </tr>
                                </tfoot>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="christmasSaverMsg">
                    <p>
                        <label for="paraone" style="width:100%"><asp:Localize ID="lclparaone" runat="server" 
                            Text="The Customer can top up their account to a maximum of $360." 
                            meta:resourcekey="lclparaoneResource1"></asp:Localize> </label><br />
                        <label for="paratwo" style="width:100%;height:25px"><asp:Localize ID="lclparatwo" runat="server" 
                            Text="If the Customer would like to put more money into their Christmas savers account,advise them to do this in-store." 
                            meta:resourcekey="lclparatwoResource1"></asp:Localize></label>
                        </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>