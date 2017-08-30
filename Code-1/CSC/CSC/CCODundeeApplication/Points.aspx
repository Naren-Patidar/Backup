<%@ Page Title="Customer Points" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Points.aspx.cs" Inherits="CCODundeeApplication.Points" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"  %>

<script runat="server">

    protected void rptEarlierColPrds_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContainer" runat="server">

    <script type="text/javascript" language="javascript">

        function modalMoreInfoHide() {
            document.getElementById("modalBox").style.display = "none";
            return false;
        }
        function modalMoreInfoShow(clubcardTransID,offerID) {

            var userWidth = screen.availWidth;
            var userHeight = screen.availHeight;
            var leftPos;
            var topPos;
            var popW = 740;   //set width here
            var popH = 387;   //set height here

            var settings = 'modal,scrollBars=yes,resizable=no,toolbar=no,menubar=no,location=no,directories=no,';
            leftPos = (userWidth - popW) / 2,
            topPos = (userHeight - popH) / 2;
            settings += 'left=' + leftPos + ',top=' + topPos + ',width=' + popW + ', height=' + popH + '';

            var ShowInfo = window.open('ShowInfo.aspx?' + clubcardTransID + '&' + offerID , 'ShowInfo', settings);
            ShowInfo.focus();
            return false;
        }
    </script>
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3 style="height:20px">
                    <label for="Customer Points"><asp:Localize ID="lclCustomerPoint" runat="server" 
                        Text="Customer Points" meta:resourcekey="lclCustomerPointResource1"></asp:Localize></label></h3>
            </div>
            <div class="replacementCardNo">
                <label for="cardNumber">
                    <asp:Literal ID="ltrColPrd" runat="server" Text="Collection Period" 
                    meta:resourcekey="ltrColPrdResource1" /></label>
            </div>
            <div class="cc_body">
                <div class="customerPointsBlueHeaderSection" id="SummarySection" runat="server" visible="false">
                    <div class="cc_bluehead">
                        <h3 style="height:20px">
                            <label for="Summary"><asp:Localize ID="lclSummary" runat="server" 
                                Text="Summary" meta:resourcekey="lclSummaryResource1"></asp:Localize></label></h3>
                    </div>
                    <div class="cc_body">
                        <div class="ccPointDetails">
                            <ul>
                                <li id="currPoints">
                                    <div>
                                        <h3>
                                            <label for="totalpoints" style="width:100%"><asp:Localize ID="lcltotal" runat="server" 
                                                Text="Points total:" meta:resourcekey="lcltotalResource1"></asp:Localize></label></h3>
                                        <div class="infoBox_white">
                                            <h4>
                                                <asp:Literal ID="ltrTotalRewardPoints" runat="server" 
                                                    meta:resourcekey="ltrTotalRewardPointsResource1" /></h4>
                                        </div>
                                    </div>
                                </li>
                                <li id="vouchValues">
                                    <div>
                                        <h3>
                                            <asp:Literal ID="ltrTotalRewardValueTitle" Text="Value in Clubcard vouchers:" 
                                                runat="server" meta:resourcekey="ltrTotalRewardValueTitleResource1" /></h3>
                                        <div class="infoBox_white">
                                            <h4>
                                                <asp:Literal ID="ltrTotalRewardValue" runat="server" 
                                                    meta:resourcekey="ltrTotalRewardValueResource1" /></h4>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="customerPointsBlueHeaderSection" id="EarlierColPrdSection" runat="server"
                    visible="false">
                    <div class="cc_bluehead">
                        <h3 style="height:20px;">
                            <label for="collectionprd" style="width:100%"><asp:Localize ID="lclcollectionperiod" 
                                runat="server" Text="Earlier Collection Periods" 
                                meta:resourcekey="lclcollectionperiodResource1"></asp:Localize></label></h3>
                    </div>
                    <div class="cc_body">
                        <div class="christmasSaverMsg">
                            <p>
                                <strong><label for="viewpoints" style="width:100%"><asp:Localize ID="lclviewpoints" runat="server" 
                                    Text="To view your points, please select which collection period you are interested in." 
                                    meta:resourcekey="lclviewpointsResource1"></asp:Localize></label>
                                    </strong></p>
                        </div>
                        <asp:Repeater ID="rptEarlierColPrds" runat="server" OnItemDataBound="rptEarlierColPrds_ItemDataBound">
                            <HeaderTemplate>
                                <table class="pointsSmTbl colPrdSm">
                                    <thead>
                                        <tr>
                                            <th class="rounded-company first">
                                                <label for="Collection Period" style="width:100%;"><asp:Localize ID="clctionperiod" runat="server" 
                                                    Text="Collection Period" meta:resourcekey="clctionperiodResource1"></asp:Localize></label>
                                            </th>
                                            <th id="thSummary" runat="server">
                                                <label for="Summary" style="width:100%"><asp:Localize ID="lclSummary" runat="server" 
                                                    Text="Summary" meta:resourcekey="lclSummaryResource2"></asp:Localize></label>
                                            </th>
                                            <th class="rounded-q4" style="width:100%">
                                                <label for="Detail"><asp:Localize ID="lclDetail" runat="server" Text="Detail" 
                                                    meta:resourcekey="lclDetailResource1"></asp:Localize></label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="first">
                                        <asp:Literal ID="ltrOfferCaption" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "OfferPeriod") %>'></asp:Literal>
                                    </td>
                                    <td id="tdSummary" runat="server">
                                        <asp:HyperLink ID="lnkViewSummary" runat="server" 
                                            meta:resourcekey="lnkViewSummaryResource2" Text="View"></asp:HyperLink>
                                    </td>
                                    <td class="last">
                                        <asp:HyperLink ID="lnkViewDetail" runat="server" 
                                            meta:resourcekey="lnkViewDetailResource2" Text="View"></asp:HyperLink>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate">
                                    <td class="first">
                                        <asp:Literal ID="lnkOfferCaption" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "OfferPeriod") %>'></asp:Literal>
                                    </td>
                                    <td id="tdASummary" runat="server">
                                        <asp:HyperLink ID="lnkViewSummary" runat="server" 
                                            meta:resourcekey="lnkViewSummaryResource1" Text="View"></asp:HyperLink>
                                    </td>
                                    <td class="last">
                                        <asp:HyperLink ID="lnkViewDetail" runat="server" 
                                            meta:resourcekey="lnkViewDetailResource1" Text="View"></asp:HyperLink>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <tfoot>
                                    <tr>
                                        <td class="rounded-foot-left first">
                                            &nbsp;
                                        </td>
                                        <td id="tdFoot" runat="server">
                                            &nbsp;
                                        </td>
                                        <td class="rounded-foot-right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tfoot>
                                </tbody></table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="customerPointsWhiteHeaderSection" id="PointsSummarySection" runat="server"
                    visible="false">
                    <div class="cc_whitehead">
                        <span class="left">&nbsp;</span>
                    </div>
                    <div class="cc_body">
                        <div class="ccPurpleGreySection longTable">
                            <div class="cc_purpleGreyhead">
                                <h4>
                                    <label for="Clubcardpoints" style="width:100%"><asp:Localize ID="lclpoints" runat="server" 
                                        Text="Clubcard points from Tesco:" meta:resourcekey="lclpointsResource1"></asp:Localize></label></h4>
                            </div>
                            <div class="cc_greyLongbody">
                                <ul class="clubcPts">
                                    <li><strong><label for="quaterpoints"><asp:Localize ID="lclqtrpoints" 
                                            runat="server" Text="Points collected this quarter" 
                                            meta:resourcekey="lclqtrpointsResource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoPoints" runat="server" 
                                                meta:resourcekey="ltrTescoPointsResource1" /></span></div>
                                    </li>
                                    <li><strong><label for="previousStmt"><asp:Localize ID="lclPreviousStatement" 
                                            runat="server" Text="Points carried over from your previous statement" 
                                            meta:resourcekey="lclPreviousStatementResource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoBroughtForwardPoints" runat="server" 
                                                meta:resourcekey="ltrTescoBroughtForwardPointsResource1" /></span></div>
                                    </li>
                                    <li><strong><label for="Rewards"><asp:Localize ID="lclRewards" runat="server" 
                                            Text="Points credited from Clubcard rewards" 
                                            meta:resourcekey="lclRewardsResource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoPointsChangeFromRewards" runat="server" 
                                                meta:resourcekey="ltrTescoPointsChangeFromRewardsResource1" /></span></div>
                                    </li>
                                </ul>
                                <hr size="1" />
                                <ul class="clubcPts">
                                    <li><strong><label for="Totalpoints" style="width:100%;height:20px"><asp:Localize ID="lcltotpoints" runat="server" 
                                            Text="Clubcard points total up to" meta:resourcekey="lcltotpointsResource1"></asp:Localize></label>
                                        <asp:Literal ID="ltrOfferEndDate1" runat="server" 
                                            meta:resourcekey="ltrOfferEndDate1Resource1" /></strong><div class="pointCurve_l greyPointCurve_l">
                                            <span class="pointCurve_r greyPointCurve_r">
                                                <asp:Literal ID="ltrTescoPointsTotal" runat="server" 
                                                meta:resourcekey="ltrTescoPointsTotalResource1" /></span></div>
                                    </li>
                                    <li><strong>
                                        <asp:Literal Text="Voucher Total to date" ID="ltrTescoTotalRewardLabel" 
                                            runat="server" meta:resourcekey="ltrTescoTotalRewardLabelResource1" /></strong><div
                                            class="pointCurve_l greyPointCurve_l">
                                            <span class="pointCurve_r greyPointCurve_r">
                                                <asp:Literal ID="ltrTescoTotalReward" runat="server" 
                                                meta:resourcekey="ltrTescoTotalRewardResource1" /></span></div>
                                    </li>
                                </ul>
                                <hr size="1" />
                                <ul class="clubcPts">
                                    <li><strong><label for="carriedtonxtstmt"><asp:Localize ID="lclcarriedtonxtstmt" 
                                            runat="server" Text="Carried forward to your next statement" 
                                            meta:resourcekey="lclcarriedtonxtstmtResource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoCarriedForwardPoints" runat="server" 
                                                meta:resourcekey="ltrTescoCarriedForwardPointsResource1" /></span></div>
                                    </li>
                                </ul>
                                <asp:Panel ID="pnlTescoPointsTotals" runat="server" 
                                    meta:resourcekey="pnlTescoPointsTotalsResource1">
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="ccGreenGreySection">
                            <div class="cc_greenGreyhead">
                                <h4>
                                    <label for="ClubcardPointsFrmBank" style="width:100%"><asp:Localize ID="lclClubcardPointsFrmBank" 
                                        runat="server" Text="Clubcard points from Tesco Bank:" 
                                        meta:resourcekey="lclClubcardPointsFrmBankResource1"></asp:Localize></label></h4>
                            </div>
                            <div class="cc_greyLongbody">
                                <ul class="clubcPts">
                                    <li><strong><label for="QtrPoints"><asp:Localize ID="lclqtrPoints1" runat="server" 
                                            Text="Points collected this quarter" meta:resourcekey="lclqtrPoints1Resource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoBankPoints" runat="server" 
                                                meta:resourcekey="ltrTescoBankPointsResource1" /></span></div>
                                    </li>
                                    <li><strong><label for="CarriedFromPrevious">
                                        <asp:Localize ID="lclCarriedFromPrevious" runat="server" 
                                            Text="Points carried over from your previous statement" 
                                            meta:resourcekey="lclCarriedFromPreviousResource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoBankBroughtForwardPoints" runat="server" 
                                                meta:resourcekey="ltrTescoBankBroughtForwardPointsResource1" /></span></div>
                                    </li>
                                    <!-- Field removed 23/06/2010 - Padmanabh
                                        <li><strong>Points credited from xxx Clubcard rewards</strong><div class="pointCurve_l greyPointCurve_l">
                                            <span class="pointCurve_r greyPointCurve_r">xxx</span></div>
                                        </li>-->
                                </ul>
                                <hr size="1" />
                                <ul class="clubcPts">
                                    <li><strong><label for="TotalPointUpto"><asp:Localize ID="lclTotalPointUpto" 
                                            runat="server" Text="Clubcard points total up to" 
                                            meta:resourcekey="lclTotalPointUptoResource1"></asp:Localize></label>
                                        <asp:Literal ID="ltrOfferEndDate2" runat="server" 
                                            meta:resourcekey="ltrOfferEndDate2Resource1" /></strong><div class="pointCurve_l greyPointCurve_l">
                                            <span class="pointCurve_r greyPointCurve_r">
                                                <asp:Literal ID="ltrTescoBankPointsTotal" runat="server" 
                                                meta:resourcekey="ltrTescoBankPointsTotalResource1" /></span></div>
                                    </li>
                                    <li><strong>
                                        <asp:Literal ID="ltrTescoBankTotalRewardLabel" Text="Voucher Total to date" 
                                            runat="server" meta:resourcekey="ltrTescoBankTotalRewardLabelResource1" /></strong><div
                                            class="pointCurve_l greyPointCurve_l">
                                            <span class="pointCurve_r greyPointCurve_r">
                                                <asp:Literal ID="ltrTescoBankTotalReward" runat="server" 
                                                meta:resourcekey="ltrTescoBankTotalRewardResource1" /></span></div>
                                    </li>
                                </ul>
                                <hr size="1" />
                                <ul class="clubcPts">
                                    <li><strong><label for="CarriedTonxt"><asp:Localize ID="lclCarriedTonxt" 
                                            runat="server" Text="Carried forward to your next statement" 
                                            meta:resourcekey="lclCarriedTonxtResource1"></asp:Localize></label></strong><div class="pointCurve_l greyPointCurve_l">
                                        <span class="pointCurve_r greyPointCurve_r">
                                            <asp:Literal ID="ltrTescoBankCarriedForwardPoints" runat="server" 
                                                meta:resourcekey="ltrTescoBankCarriedForwardPointsResource1" /></span></div>
                                    </li>
                                </ul>
                                <asp:Panel ID="pnlTescoBankPointsTotals" runat="server" 
                                    meta:resourcekey="pnlTescoBankPointsTotalsResource1">
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="prevDtlsSmry">
                            <ul>
                                <li><strong><label for="txnDetails" style="width:100%"><asp:Localize ID="lcltxnDetails" runat="server" 
                                        Text="To view transaction detail for this period" 
                                        meta:resourcekey="lcltxnDetailsResource1"></asp:Localize></label></strong>
                                    <p class="pageAction">
                                        <a id="btnSeePointsDetail" runat="server">
                                            <img id="Img1" src="~/I/seepointsdetail.gif" runat="server" alt="See points detail" /></a>
                                    </p>
                                </li>
                                <!--<li>To review a summary of the previous collection period
                                        <p class="pageAction">
                                            <input type="image" src="~/I/seeprevpointsummary.gif" id="seeprevpointsummary" name="seeprevpointsummary"
                                                class="imgBtn" />
                                        </p>
                                    </li>
                                    -->
                            </ul>
                        </div>
                        <p class="noteText">
                            <!--T&C. Lorem ipsum dolor sit amet, consectetur adipiscing elit. In eget commodo ligula.
                            Aenean mi massa, tempus vitae euismod at, scelerisque vel velit. Morbi at libero.</p>-->
                    </div>
                </div>
                <div class="customerPointsBlueHeaderSection" id="TransactionSection" runat="server"
                    visible="false">
                    <div class="cc_bluehead">
                        <h3 style="height:20px;">
                            <label for="Txndetailss" style="width:100%"><asp:Localize ID="lclTxndetailss" runat="server" 
                                Text="Transaction Details" meta:resourcekey="lclTxndetailssResource1"></asp:Localize></label></h3>
                    </div>
                    <div class="cc_body" id="transSearch">
                        <label class="searchLabel">
                            <asp:localize ID="lclsearchLabel" runat="server" Text="Search by:" 
                            meta:resourcekey="lclsearchLabelResource1"></asp:localize></label>
                        <ul class="cardDetails">
                            <li>
                                <label for="cardNmb">
                                    <asp:localize ID="lclcardNmb" runat="server" Text="Card number:" 
                                    meta:resourcekey="lclcardNmbResource1"></asp:localize></label>
                                <asp:DropDownList CssClass="brderInput" ID="ddlCardNumbers" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCardNumbers_SelectedIndexChanged" 
                                    meta:resourcekey="ddlCardNumbersResource1" />
                            </li>
                            <li>
                                <label for="Transaction">
                                    <asp:localize ID="lclTransaction" runat="server" Text="Transaction:" 
                                    meta:resourcekey="lclTransactionResource1"></asp:localize></label>
                                <asp:DropDownList CssClass="brderInput" ID="ddlTransactionTypes" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlTransactionTypes_SelectedIndexChanged" 
                                    meta:resourcekey="ddlTransactionTypesResource1" />
                            </li>
                        </ul>
                        <p class="searchBy">
                            <asp:ImageButton ID="btnClearSelection" CssClass="imgBtn" ImageUrl="~/I/clearselection.gif"
                                runat="server" OnClick="btnClearSelection_Click" 
                                meta:resourcekey="btnClearSelectionResource1" />
                        </p>
                        <div>
                            <asp:GridView Width="98%" ID="grdTransactions" runat="server" AutoGenerateColumns="False"
                                CssClass="GridStyle" OnRowDataBound="grdTransactions_RowDataBound" AllowSorting="True"
                                OnSorting="grdTransactions_Sorting" ShowFooter="True" BorderWidth="0px" 
                                OnRowCreated="grdTransaction_RowCreated" 
                                meta:resourcekey="grdTransactionsResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrCardStatus" runat="server" Text='<%# Bind("CustType") %>' />
                                            <asp:Literal ID="ltrCardCancelled" runat="server" Text='<%# Bind("ClubcardStatusDescEnglish") %>'
                                                Visible="False" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeaderLeft" />
                                        <ItemStyle CssClass="GridTd" Font-Bold="true" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Card number" SortExpression="ClubcardID" 
                                        meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrCardNo" runat="server" Text='<%# Bind("ClubcardID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" SortExpression="TransactionDateTime" 
                                        meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrDatePtsAdded" runat="server" Text='<%# Bind("TransactionDateTime") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrTimePtsAdded" runat="server" Text='<%# Bind("TransactionDateTime") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transaction details" 
                                        SortExpression="TransactionDescription" 
                                        meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrTransactionDetails" runat="server" Text='<%# Bind("TransactionDescription") == null?"": Bind("TransactionDescription") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bonus Points" SortExpression="BonusPoints" 
                                        meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrBonusPoints" runat="server" Text='<%# Bind("BonusPoints") == null?0: Bind("BonusPoints") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Total Points" SortExpression="TotalPoints" 
                                        meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrTransTotalPoints" runat="server" Text='<%# Bind("NormalPoints") == null?0: Bind("NormalPoints") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Spend" ItemStyle-HorizontalAlign="Center" 
                                        ItemStyle-VerticalAlign="Middle" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                           <asp:Localize ID="lclCur1" runat="server" Text="$" meta:resourcekey="lclCurResource2"></asp:Localize> 
                                           <asp:Literal ID="ltrActualSpent" Text='<%# Bind("AmountSpent") %>' runat="server"></asp:Literal>
                                           <asp:Localize ID="lclCur" runat="server" Text="$" meta:resourcekey="lclCurResource1"></asp:Localize>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdRight" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Points" ItemStyle-HorizontalAlign="Right" 
                                        ItemStyle-VerticalAlign="Middle" meta:resourcekey="TemplateFieldResource9">
                                        <ItemTemplate>
                                          <asp:LinkButton ID="lnkShowInfo" CommandName="ShowInfo" 
                                                CommandArgument='<%# Bind("ClubcardTransactionID") %>' runat="server" 
                                                meta:resourcekey="lnkShowInfoResource1" Text="Show Info"></asp:LinkButton>
                                           <asp:Literal ID="ltrTotalPoints" runat="server" Visible="False" 
                                                Text='<%# Bind("NormalPoints") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdRightLast" />
                                        <HeaderStyle CssClass="GridHeaderRightLast" />
                                        <FooterStyle CssClass="GridFooterTdRight" />
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle CssClass="GridAlternateRow" />
                                <EmptyDataTemplate>
                                    <table class="alertMsgsInPoints">
                                        <tr>
                                            <td>
                                                <label for="notxnfound"><asp:Localize ID="lclnotxnfound" runat="server" 
                                                    Text="Could not find any transactions" 
                                                    meta:resourcekey="lclnotxnfoundResource1"></asp:Localize></label>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <p class="zeroPtsMsg">
                                <asp:Label ID="lblShow0PtsMsg" runat="server" Text="Could not find any transactions"
                                    Visible="False" class="alertMsgs" 
                                    meta:resourcekey="lblShow0PtsMsgResource1"></asp:Label></p>
                            <p class="pageAction">
                                <a href="Points.aspx" id="lnkGoBack" runat="server" visible="false">
                                    <asp:Image ID="imgBacktosummarypage" CssClass="imgBtn" AlternateText="Back to points page"
                                        ImageUrl="I/backtopointspage.gif" runat="server" 
                                    meta:resourcekey="imgBacktosummarypageResource1" /></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="customerPointsBlueHeaderSection" id="divChristmasSaverSummary" runat="server"
                    style="width: 744px">
                    <div class="cc_bluehead">
                        <h3 style="height:40px;>
                            <label for="christmasSaverSumary" style="width:100%"><asp:Localize ID="lclchristmasSaverSumary" 
                                runat="server" Text="Christmas Saver Summary" 
                                meta:resourcekey="lclchristmasSaverSumaryResource1"></asp:Localize></label></h3>
                    </div>
                    <div class="cc_body">
                        <div class="christmasSaverSmryMsg" style="height:100px">
                            <p>
                                <label for="memberofChristmas" style="width:90%"  ><asp:Localize ID="lclmemberofChristmas" 
                                    runat="server" Text="Whilst the customer was a member of the Christmas saver Club they saved some money, 
                                which is shown in the table." meta:resourcekey="lclmemberofChristmasResource1"></asp:Localize></label></p><br/>
                                <br />
                                <br />
                            <p>
                                <label for="vouchersBack" style="width:90%">
                                <asp:Localize ID="lclvouchersBack" runat="server" Text="Explain that this money will be given back to them as vouchers in their November
                                statement." meta:resourcekey="lclvouchersBackResource1"></asp:Localize></label></p>
                        </div>
                        <div>
                            <asp:Repeater ID="rptChristmasSummary" runat="server">
                                <HeaderTemplate>
                                    <table class="vouchDtlTbl christmasSaverSmry">
                                        <thead>
                                            <tr>
                                                <th class="rounded-company first">
                                                    <strong><label for="Datetoppedup" style="height:30px;width:50%"><asp:Localize ID="lclDatetoppedup" 
                                                        runat="server" Text="Date topped up" 
                                                        meta:resourcekey="lclDatetoppedupResource1"></asp:Localize></label></strong>
                                                </th>
                                                <th class="rounded-q4">
                                                    <strong><label for="Amounttoppedup" style="height:30px;width:50%"><asp:Localize ID="lclAmounttoppedup" 
                                                        runat="server" Text="Amount topped up" 
                                                        meta:resourcekey="lclAmounttoppedupResource1"></asp:Localize></label></strong>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <strong>
                                                <%# DataBinder.Eval(Container.DataItem, "TransactionDateTime", "{0 :dd/MM/yy}")%></strong>
                                        </td>
                                        <td class="last">
                                            <strong><asp:Localize ID="lclCurrency" runat="server" Text="$" meta:resourcekey="lclCurrencyResource1"></asp:Localize><%# DataBinder.Eval(Container.DataItem, "AmountSpent", "{0:C}")%></strong>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="alternate">
                                        <td>
                                            <strong>
                                                <%# DataBinder.Eval(Container.DataItem, "TransactionDateTime", "{0 :dd/MM/yy}")%></strong>
                                        </td>
                                        <td class="last">
                                            <strong><asp:Localize ID="lclCurr" runat="server" Text="$" meta:resourcekey="lclCurrResource1"></asp:Localize><%# DataBinder.Eval(Container.DataItem, "AmountSpent", "{0:C}")%></strong>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <tfoot>
                                    </tfoot>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBox" class="modalBox" style="display: none;">
        <div>
            <table class="vouchDtlTbl">
                <thead>
                    <tr>
                        <th class="rounded-company first">
                            <label for="txnDetails"><asp:Localize ID="lcltxnDetails1" runat="server" 
                                Text="Transaction Details" meta:resourcekey="lcltxnDetails1Resource1"></asp:Localize></label>
                        </th>
                        <th class="rounded-q4">
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
                            <label for="CardNum" style="text-align:left"><asp:Localize ID="lclCardNum" runat="server" 
                                Text="Card Number" meta:resourcekey="lclCardNumResource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <span id="modalCardNumber"></span>
                        </td>
                    </tr>
                    <tr class="alternate">
                        <td>
                            <label for="DateofTxn" style="text-align:left"><asp:Localize ID="lclDateofTxn" runat="server" 
                                Text="Date of transaction" meta:resourcekey="lclDateofTxnResource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <span id="modalDateofTransaction"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="TxnType"><asp:Localize ID="lclTxnType" runat="server" 
                                Text="Transaction type" meta:resourcekey="lclTxnTypeResource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <span id="modalTransactionType"></span>
                        </td>
                    </tr>
                    <tr class="alternate">
                        <td>
                            <label for="TotPoint"><asp:Localize ID="lclTotPoint" runat="server" 
                                Text="Total points" meta:resourcekey="lclTotPointResource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <span id="modalTotalPoints"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="lastRw">
                            <label for="Amount Spent"><asp:Localize ID="lclAmountSpent" runat="server" 
                                Text="Amount Spent" meta:resourcekey="lclAmountSpentResource1"></asp:Localize></label>
                        </td>
                        <td class="lastRw last">
                            <span id="modalAmountSpent"></span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <asp:HiddenField runat="server" ID="hdnIsSummaryEnabled" Value="true"/>
        <p class="pageAction">
            <a onclick="modalMoreInfoHide();">
                <asp:Image ID="btnCloseThisWindow" CssClass="imgBtn" AlternateText="Close this window"
                    ImageUrl="~/I/closethiswindow.gif" runat="server" 
                meta:resourcekey="btnCloseThisWindowResource1" /></a>
        </p>
    </div>
</asp:Content>
