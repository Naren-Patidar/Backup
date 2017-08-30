<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PointsSummary.aspx.cs"
    Inherits="CCODundeeApplication.PointsSummary" Title="Customer Points" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div id="tc_right">
            <h1>
                <asp:Literal runat="server" ID="ltrStatementTitleNov" Text="Statement Summary" 
                    meta:resourcekey="ltrStatementTitleNovResource1" /></h1>
            <div class="tc_box tc_wide">
                <div class="tc_crbottomleft tc_corner816">
                    &shy;</div>
                <div class="tc_crbottomright tc_corner816">
                    &shy;</div>
                <div class="tc_header tc_lightblue tc_wheader">
                    <div class="tc_crtopleft tc_corner836">
                        &shy;</div>
                    <div class="tc_crtopright tc_corner836">
                        &shy;</div>
                    <h3>
                        <asp:Literal ID="ltrStatementRangeNov" runat="server" 
                            meta:resourcekey="ltrStatementRangeNovResource1" /></h3>
                </div>
                <!-- /header -->
                <div class="tc_half">
                    <div class="tc_box tc_table tc_lightblue tc_noborder" runat="server" id="dvNonXmasVoucher">
                        <div class="tc_crtopleft tc_corner836">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner836">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner836">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner836">
                            &shy;</div>
                        <div id="tc_standard_reward">
                            <span class="tc_large">
                                <asp:Literal Text="Voucher total" ID="ltrTotalRewardLabelNov" 
                                runat="server" meta:resourcekey="ltrTotalRewardLabelNovResource1" /></span>
                            <span class="tc_pricecell">
                                <asp:Literal ID="ltrTotalRewardNov" runat="server" 
                                meta:resourcekey="ltrTotalRewardNovResource1" /></span>
                            <div class="tc_wrapper">
                            </div>
                        </div>
                    </div>
                    <div class="tc_box tc_table tc_lightblue tc_noborder" runat="server" id="dvXmasVoucher">
                        <div class="tc_crtopleft tc_corner836">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner836">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner836">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner836">
                            &shy;</div>
                        <div id="tc_christmas_total">
                            <span class="tc_large">
                                <asp:Literal Text="Voucher total" ID="ltrTotalRewardLabelNovXmas" 
                                runat="server" meta:resourcekey="ltrTotalRewardLabelNovXmasResource1" /></span>
                            <span class="tc_pricecell">
                                <asp:Literal ID="ltrTotalRewardNovXmas" runat="server" 
                                meta:resourcekey="ltrTotalRewardNovXmasResource1" /></span>
                            <div class="tc_wrapper">
                            </div>
                        </div>
                    </div>
                    <div class="tc_box tc_grey tc_posup tc_underlying tc_more" runat="server" id="dvCarryFrdPts">
                        <div class="tc_crbottomleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner89">
                            &shy;</div>
                        <table class="tc_table tc_grey">
                            <tbody>
                                <tr>
                                    <th id="nonXmasRewardLable" runat="server">
                                        <asp:Literal ID="ltrTescoTotalRewardLabelNov" runat="server" 
                                            meta:resourcekey="ltrTescoTotalRewardLabelNovResource1"></asp:Literal>
                                    </th>
                                    <th id="xmasRewardLabel" runat="server">
                                        <asp:Literal ID="ltrTescoTotalRewardLabelXmas" runat="server" 
                                            meta:resourcekey="ltrTescoTotalRewardLabelXmasResource1"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTtlCarriedForwardPointsNov" runat="server" 
                                            meta:resourcekey="ltrTtlCarriedForwardPointsNovResource1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!-- /half -->
                <div class="tc_half">
                    <div class="tc_box tc_grey tc_more">
                        <div class="tc_crtopleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner89">
                            &shy;</div>
                        <table class="tc_table tc_grey">
                            <tbody>
                                <tr>
                                    <th>
                                    <asp:Localize ID="lclPointsCollected" runat="server" 
                                            Text="Points collected with Tesco" 
                                            meta:resourcekey="lclPointsCollectedResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoPointsNov" runat="server" 
                                            meta:resourcekey="ltrTescoPointsNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                    <asp:Localize ID="lclPointsColTescoBank" runat="server" 
                                            Text="Points collected with Tesco Bank" 
                                            meta:resourcekey="lclPointsColTescoBankResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoBankPointsNov" runat="server" 
                                            meta:resourcekey="ltrTescoBankPointsNovResource1" />
                                    </td>
                                </tr>
                                <tr class="tc_bottomline">
                                    <th colspan="2">
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                    <asp:Localize ID="Localize2" runat="server" Text="Total points" 
                                            meta:resourcekey="Localize2Resource1"></asp:Localize>
                                       
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrPointsTotalNov" runat="server" 
                                            meta:resourcekey="ltrPointsTotalNovResource1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="tc_box tc_grey" id="dvMilesSection" runat="server" visible="false">
                        <div class="tc_crtopleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner89">
                            &shy;</div>
                        <table class="tc_table tc_grey">
                            <tbody>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltrLabelPtsConvertedNov" runat="server" 
                                            meta:resourcekey="ltrLabelPtsConvertedNovResource1" />
                                    </th>
                                    <td>
                                        <asp:Literal ID="LtrPtsConvertedToMilesNov" runat="server" 
                                            meta:resourcekey="LtrPtsConvertedToMilesNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltrLabelVoucherTtl1Nov" runat="server" 
                                            meta:resourcekey="ltrLabelVoucherTtl1NovResource1" />
                                    </th>
                                    <td>
                                        <asp:Literal ID="LtrMilesAwardedNov" runat="server" 
                                            meta:resourcekey="LtrMilesAwardedNovResource1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="tc_box tc_grey" id="dvXmasSaver" runat="server" visible="false">
                        <div class="tc_crtopleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner89">
                            &shy;</div>
                        <table class="tc_table tc_grey">
                            <tbody>
                                <tr>
                                    <th>
                                     <asp:Localize ID="lclClubcardVouchertotal" runat="server" 
                                            Text="Clubcard voucher total" 
                                            meta:resourcekey="lclClubcardVouchertotalResource1"></asp:Localize>
                                       
                                    </th>
                                    <td>
                                        <asp:Literal ID="LtrCCVoucherTtlNov" runat="server" 
                                            meta:resourcekey="LtrCCVoucherTtlNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                     <asp:Localize ID="lclVoucherTotal" runat="server" Text="Top up voucher total" 
                                            meta:resourcekey="lclVoucherTotalResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="LtrTopUpTtlNov" runat="server" 
                                            meta:resourcekey="LtrTopUpTtlNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                    <asp:Localize ID="lclBonusVoucherTotal" runat="server" Text="Bonus voucher total" 
                                            meta:resourcekey="lclBonusVoucherTotalResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="LtrBonusVoucherTtlNov" runat="server" 
                                            meta:resourcekey="LtrBonusVoucherTtlNovResource1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!-- /half -->
                <div class="tc_wrapper">
                    &shy;</div>
            </div>
            <!-- /wide lightblue box -->
            <div class="tc_box tc_wide">
                <div class="tc_crtopleft tc_corner816">
                    &shy;</div>
                <div class="tc_crtopright tc_corner816">
                    &shy;</div>
                <div class="tc_crbottomleft tc_corner816">
                    &shy;</div>
                <div class="tc_crbottomright tc_corner816">
                    &shy;</div>
                <div class="tc_half">
                    <div class="tc_box tc_points">
                        <div class="tc_header tc_darkblue">
                            <h3><asp:Localize ID="lclClubcardPntsfrmTesco" runat="server" 
                                    Text="Clubcard points from Tesco" 
                                    meta:resourcekey="lclClubcardPntsfrmTescoResource1"></asp:Localize>
                               </h3>
                            <div class="tc_crtopleft tc_corner836">
                                &shy;</div>
                            <div class="tc_crtopright tc_corner836">
                                &shy;</div>
                        </div>
                        <div>
                            <asp:Panel ID="pnlTescoPointsTotals" runat="server" 
                                meta:resourcekey="pnlTescoPointsTotalsResource1">
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="tc_box tc_grey tc_posup">
                        <div class="tc_crtopleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner89">
                            &shy;</div>
                        <table class="tc_table tc_grey">
                            <tbody>
                                <tr>
                                    <th class="tc_nobold tc_left">
                                    <asp:Localize ID="lclPntscarfwdprevsmt" runat="server" 
                                            Text="Points carried forward from your previous statement" 
                                            meta:resourcekey="lclPntscarfwdprevsmtResource1"></asp:Localize>
                                        
                                    </th>
                                    <td >
                                        <asp:Literal ID="ltrTescoBroughtForwardPointsNov" runat="server" 
                                            meta:resourcekey="ltrTescoBroughtForwardPointsNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th class="tc_nobold tc_left">
                                     <asp:Localize ID="lclPntChangeClubRewards" runat="server" 
                                            Text="Points change from Clubcard rewards" 
                                            meta:resourcekey="lclPntChangeClubRewardsResource1"></asp:Localize>
                                       
                                    </th>
                                    <td >
                                        <asp:Literal ID="ltrTescoPointsChangeFromRewardsNov" runat="server" 
                                            meta:resourcekey="ltrTescoPointsChangeFromRewardsNovResource1" />
                                    </td>
                                </tr>
                                <tr class="tc_bottomline">
                                    <th colspan="2">
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                      <asp:Localize ID="lclTescoTotalPoints" runat="server" Text="Total Tesco points" 
                                            meta:resourcekey="lclTescoTotalPointsResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoPointsTotalNov" runat="server" 
                                            meta:resourcekey="ltrTescoPointsTotalNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="LtrLabelTescoVchrTtlNov" runat="server" 
                                            meta:resourcekey="LtrLabelTescoVchrTtlNovResource1" />
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoTotalRewardNov" runat="server" 
                                            meta:resourcekey="ltrTescoTotalRewardNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                     <asp:Localize ID="lclTescoPntCarFwd" runat="server" 
                                            Text="Tesco points carried forward" 
                                            meta:resourcekey="lclTescoPntCarFwdResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoCarriedForwardPointsNov" runat="server" 
                                            meta:resourcekey="ltrTescoCarriedForwardPointsNovResource1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!-- /half -->
                <div class="tc_half">
                    <div class="tc_box tc_points">
                        <div class="tc_header tc_darkblue">
                            <h3><asp:Localize ID="lclClubPntfrmTescoBank" runat="server" 
                                    Text="Clubcard points from Tesco Bank" 
                                    meta:resourcekey="lclClubPntfrmTescoBankResource1"></asp:Localize>
                                </h3>
                            <div class="tc_crtopleft tc_corner836">
                                &shy;</div>
                            <div class="tc_crtopright tc_corner836">
                                &shy;</div>
                        </div>
                        <div>
                            <asp:Panel ID="pnlTescoBankPointsTotals" runat="server" 
                                meta:resourcekey="pnlTescoBankPointsTotalsResource1">
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="tc_box tc_grey tc_posup tc_more">
                        <div class="tc_crtopleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crtopright tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomleft tc_corner89">
                            &shy;</div>
                        <div class="tc_crbottomright tc_corner89">
                            &shy;</div>
                        <table class="tc_table tc_grey">
                            <tbody>
                                <tr>
                                    <th class="tc_nobold tc_left">
                                    <asp:Localize ID="lclPntCarrFwdfrmPrevStmt" runat="server" 
                                            Text="Points carried forward from your previous statement" 
                                            meta:resourcekey="lclPntCarrFwdfrmPrevStmtResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoBankBroughtForwardPointsNov" runat="server" 
                                            meta:resourcekey="ltrTescoBankBroughtForwardPointsNovResource1" />
                                    </td>
                                </tr>
                                <tr class="tc_bottomline">
                                    <th colspan="2">
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                     <asp:Localize ID="lclTotTesBankPnts" runat="server" Text="Total Tesco Bank points" 
                                            meta:resourcekey="lclTotTesBankPntsResource1"></asp:Localize>
                                        
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoBankPointsTotalNov" runat="server" 
                                            meta:resourcekey="ltrTescoBankPointsTotalNovResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltrLabelVoucherTtlNov" runat="server" 
                                            meta:resourcekey="ltrLabelVoucherTtlNovResource1" />
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoBankTotalRewardNov" runat="server" 
                                            meta:resourcekey="ltrTescoBankTotalRewardNovResource1" />
                                    </td>
                                </tr>
                                <tr class="tc_nobold">
                                    <th>
                                    <asp:Localize ID="lclTesBankPntsCarFwd" runat="server" 
                                            Text="Tesco Bank points carried forward" 
                                            meta:resourcekey="lclTesBankPntsCarFwdResource1"></asp:Localize>
                                       
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltrTescoBankCarriedForwardPointsNov" runat="server" 
                                            meta:resourcekey="ltrTescoBankCarriedForwardPointsNovResource1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <p>
                    <asp:Localize ID="lclReviewSumm" runat="server" 
                            Text="To review a summary of the previous collection period." 
                            meta:resourcekey="lclReviewSummResource1"></asp:Localize>
                        
                    </p>
                    <p>
                        <a id="btnSeePointsDetail" runat="server" href="#" class="tc_button" style="color: White;
                            color: White; float: right; margin: 0; min-height: 17px; padding: 3px 0 0; width: 220px;">
                             <asp:Localize ID="lclSeePrevPntsSumm" runat="server" 
                            Text="See previous points summary" 
                            meta:resourcekey="lclSeePrevPntsSummResource1"></asp:Localize>
                             </a>
                    </p>
                </div>
                <!-- /half -->
                <div class="tc_wrapper">
                    &shy;</div>
            </div>
            <!-- /wide box -->
        </div>
    </div>
</asp:Content>
