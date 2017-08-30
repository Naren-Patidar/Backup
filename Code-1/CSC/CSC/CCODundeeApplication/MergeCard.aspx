<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="MergeCard.aspx.cs"
    Inherits="CCODundeeApplication.MergeCard" Title="Merge Cards" culture="auto" uiculture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
<script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

   
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3><asp:Localize ID="lclHeader" runat="server" Text= "Merge Card" 
                        meta:resourceKey="Header"></asp:Localize>
                   </h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
             
               </div>
             
                   
                   
                    <div  class="CardRangeSearch"  id="dvSearchResults" runat="server">
               
                        <div class="clubcardAcct">   
                          <div class="clubcardAct_head">
                                <h4><asp:Localize ID="lclHeader1" runat="server" Text="Card Details" 
                                        meta:resourceKey="lblHeader1"></asp:Localize>
                                   </h4>
                            </div>
                            <div>
                              <div class="reissueClubcardAcct" id="divMain" runat="server">
                    <asp:Repeater ID="rptCardDetails" runat="server">
                        <HeaderTemplate>
                            <table class="reissueCardHolderTbl">
                                <thead>
                                    <tr>
                                        <th>
                                            <label for="Card number" style="width:100%"><asp:Localize ID="lclCardnumber" runat="server" 
                                                Text="Card number" meta:resourcekey="lclCardnumberResource2"></asp:Localize></label>
                                        </th>
                                        <th>
                                            <label for="Issuedate" style="width:100%"><asp:Localize ID="lclIssuedate" runat="server" 
                                                Text="Issue date" meta:resourcekey="lclIssuedateResource1"></asp:Localize></label>
                                        </th>
                                        <th ID="thTypeofcard" runat="server">
                                            <label for="Type of card" style="width:100%"><asp:Localize ID="lclTypeofcard" runat="server" 
                                                Text="Type of card" meta:resourcekey="lclTypeofcardResource1"></asp:Localize></label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltrClubcardNumber" runat="server" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>'></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrIssueDate" runat="server" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "CardIssuedDate") %>'></asp:Literal>
                                </td>
                                <td id="tdTypeofCard" runat="server">
                                    <asp:Literal ID="ltrTypeofCard" runat="server" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardTypeDesc") %>'></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate">
                                <td>
                                    <asp:Literal ID="ltrClubcardNumber" runat="server" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>'></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrIssueDate" runat="server" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "CardIssuedDate") %>'></asp:Literal>
                                </td>
                                <td id="tdTypeofCard" runat="server">
                                    <asp:Literal ID="ltrTypeofCard" runat="server" 
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardTypeDesc") %>'></asp:Literal>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <span>
                        <!--Empty-->
                    </span>
                </div>
                    
                        </div>
                    </div>
                  
                 <div style="width: 60%; float:left" id="Div1" runat="server">
                        <br />
                        <ul class="customer">
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="Localize2" runat="server" Text="Clubcard to be merged with" 
                                    meta:resourceKey="lblStoreName"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtCardNumber" name="UserName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtCardNumberResource1"/>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="30px" OnClick="FindCustomer" meta:resourcekey="btnGoResource1" 
                                         />
                                     <asp:Button ID="btnMerge" runat="server" Text="Merge" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Visible="False" Width="50px" 
                                        onclick="btnMerge_Click" meta:resourcekey="btnMergeResource1"
                                         />
                                </div>
                                <div>
                                 <asp:GridView CssClass="cardHolderTbl" ID="grdCustomerDetail" runat="server" AutoGenerateColumns="False"
                            AllowPaging="True" PagerSettings-Visible="false" OnRowDataBound="GrdCustomerDetail_RowDataBound"
                            AlternatingRowStyle-CssClass="alternate" Width="738px" 
                            meta:resourcekey="grdCustomerDetailResource1" Visible="False">
                            <PagerSettings Visible="False"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrName" runat="server" Text='<%# Bind("Name1") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>
                                 <%-- <asp:TemplateField ItemStyle-Width="50px" HeaderText="Address" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <div style="width:100px;">
                                        <asp:Literal ID="ltrAddress" runat="server" Text='<%# Bind("MailingAddressLine1") %>' />
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="true" CssClass="GridHeader" />
                                    <ItemStyle Wrap="true" CssClass="GridTdCenter" />
                                    <FooterStyle Wrap="true" CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Address" ItemStyle-Wrap="true" 
                                    HeaderStyle-Font-Bold="true" meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrAddress" runat="server"  Text='<%# Bind("MailingAddressLine1") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTdCenter" Wrap="true"/>
                                    <FooterStyle CssClass="GridFooterTd" Wrap="true"/>
                                    <HeaderStyle CssClass="GridHeader" Wrap="true"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Points" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource3">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrCurrentPts" runat="server" Text='<%# Bind("CurrentPointsBalanceQty") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Previous Points" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource4">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPrevpts" runat="server" Text='<%# Bind("PreviousPointsBalanceQty") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Household ID" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrHousehold" runat="server" Text='<%# Bind("HouseHoldID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Status" HeaderStyle-Font-Bold="true" 
                                    meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrCustStatus" runat="server" Text='<%# Bind("CustomerUseStatusID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                            </Columns>
                            <%--<PagerStyle CssClass="pagination" />
                            <PagerSettings Mode="NumericFirstLast" PreviousPageImageUrl="I/previousDisabled.gif"
                                NextPageImageUrl="I/next.gif" />--%><AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                        </asp:GridView>
                       
                                </div>
                                    <div class="errorFields" runat="server" id="dvNoDataFound" visible="false">
                    <p style="text-align: center">
                        <strong>
                        <asp:Localize ID="lclNoDataFound" runat="server" 
                            Text="No account(s) were found matching the requested search." 
                            meta:resourcekey="lclNoDataFoundResource1"></asp:Localize></strong></p>
                </div>
                                
                            </li>
                          
                        </ul>
                    </div>
                  
            </div>
            <asp:HiddenField ID="hdnClubcard" runat="server"  />
            <asp:HiddenField ID="hdnFname" runat="server"  />
            <asp:HiddenField ID="hdnTitle" runat="server"  />
            <asp:HiddenField ID="hdnLname" runat="server"  />
             <asp:HiddenField ID="hdnMname" runat="server"  />
              <asp:HiddenField ID="hdncardNumber" runat="server"  />
               <asp:HiddenField ID="hdnhouseHoldID" runat="server"  />
                <asp:HiddenField ID="hdncurrentPoints" runat="server"  />
                 <asp:HiddenField ID="hdnjoinDate" runat="server"  />
                  <asp:HiddenField ID="hdnJoinRouteCode" runat="server"  />
                   <asp:HiddenField ID="hdnPromotionalCode" runat="server"  />
                   <asp:HiddenField ID="hdnamendBy" runat="server"  />
                   <asp:HiddenField ID="hdnamendDateTime" runat="server"  />
      
</asp:Content>
