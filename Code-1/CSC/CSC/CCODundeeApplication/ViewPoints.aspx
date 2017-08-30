<%@ Page Language="C#" AutoEventWireup="true" Title="View Points" MasterPageFile="~/Site.Master"
    CodeBehind="ViewPoints.aspx.cs" Inherits="CCODundeeApplication.ViewPoints" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="View Points"><asp:Localize ID="lclView" runat="server" 
                        Text="View Points" meta:resourcekey="lclViewResource1"></asp:Localize></label></h3>
            </div>
            <div class="replacementCardNo">
                <label for="cardNumber">
                    <asp:Literal ID="ltrColPrd" runat="server" Text="Collection Period" 
                    meta:resourcekey="ltrColPrdResource1" /></label>
            </div>
            <div class="cc_body">
                <div class="ccPointDetails" style="width: 55%;">
                    <ul>
                        <li id="currPoints">
                            <div>
                                <h3>
                                    <label for="Points total" style="width:60%"><asp:Localize ID="lclPointstotal" runat="server" 
                                        Text="Points total" meta:resourcekey="lclPointstotalResource1"></asp:Localize></label></h3>
                                <div class="infoBox_white">
                                    <h4>
                                        <asp:Literal ID="ltrTotalPoints" runat="server" 
                                            meta:resourcekey="ltrTotalPointsResource1" /></h4>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="customerSearch" id="dvSearchResults" runat="server">
                    <div class="clubcardAcct">
                        <div class="clubcardAct_head" style="height:30px;">
                            <h4>
                                <label for="Collection Period Summary" style="width:100%"><asp:Localize ID="lclCollection" 
                                    runat="server" Text="Collection Period Summary" 
                                    meta:resourcekey="lclCollectionResource1"></asp:Localize>
                                </label>
                            </h4>
                            <br/>
                        </div>
                        <asp:GridView CssClass="cardHolderTbl" ID="grdPointsSummary" AutoGenerateColumns="False"
                            runat="server" meta:resourcekey="grdPointsSummaryResource1">
                            <Columns>
                                <asp:TemplateField HeaderText="Points" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPoints" runat="server" Text='<%# Bind("Points") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Brought Forward" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrBroughtForward" runat="server" Text='<%# Bind("BroughtForward") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTdCenter" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bonus" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource3">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrBonus" runat="server" Text='<%# Bind("Bonus") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Welcome" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource4">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrWelcome" runat="server" Text='<%# Bind("Welcome") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="STPF" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrSTPF" runat="server" Text='<%# Bind("STPF") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Partner" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPartner" runat="server" Text='<%# Bind("Partner") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Green" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource7">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrGreen" runat="server" Text='<%# Bind("Green") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Normal" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource8">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrNormal" runat="server" Text='<%# Bind("Normal") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrOther" runat="server" Text='<%# Bind("Other") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Points" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource10">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrTotalPoints" runat="server" Text='<%# Bind("TotalPoints") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="cc_body">
                <div class="customerSearch" id="Div1" runat="server">
                    <div class="clubcardAcct">
                        <div class="clubcardAct_head" style="height:30px;">
                            <h4>
                                <label for="Yearly Summary" style="height:29px"><asp:Localize ID="lclYearlySummary" runat="server" 
                                    Text="Yearly Summary" meta:resourcekey="lclYearlySummaryResource1"></asp:Localize></label></h4>
                        </div>
                        <asp:GridView CssClass="cardHolderTbl" ID="GridView1" runat="server" AutoGenerateColumns="False"
                            AllowPaging="True" PagerSettings-Visible="false" 
                            AlternatingRowStyle-CssClass="alternate" meta:resourcekey="GridView1Resource1">
<PagerSettings Visible="False"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderText="Points Earned" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource11">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPointsEarned" runat="server" Text='<%# Bind("PointsEarned") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Points Cashed Out" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource12">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPointsCashedOut" runat="server" Text='<%# Bind("PointsCashedOut") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTdCenter" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Points Due for Expiry" 
                                    HeaderStyle-Font-Bold="true" meta:resourcekey="TemplateFieldResource13">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPointsDueforExpiry" runat="server" Text='<%# Bind("PointsDueforExpiry") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="4th Quater Rollover Points" 
                                    HeaderStyle-Font-Bold="true" meta:resourcekey="TemplateFieldResource14">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltr4thQuaterRolloverPoints" runat="server" Text='<%# Bind("4thQtrRolloverPoints") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expired Points" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource15">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrExpiredPoints" runat="server" Text='<%# Bind("ExpiredPoints") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                            </Columns>

<AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </div>
                 <div class="noteText errorFields" id="divVirtualCardMsg" runat="server" style="display: none;">
                        <label for="Customer card is not valid"><asp:Localize ID="lclCustomerCard" 
                            runat="server" Text="Customer card is not valid" 
                            meta:resourcekey="lclCustomerCardResource1"></asp:Localize></label>
                    </div>
                <div align="center">
                    <asp:ImageButton ID="btnAddPoints" runat="server" ImageUrl="~/I/AddPoints.bmp" 
                        OnClick="btnAddPoints_Click" meta:resourcekey="btnAddPointsResource1" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
