<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataConfiguration.aspx.cs"
    MasterPageFile="~/Site.Master" Title="Data Configuration" Inherits="CCODundeeApplication.DataConfiguration" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/CustomerDetails.js" type="text/javascript"></script>

    <div id="mainContent">
        <div class="ccBlueHeaderSection" id="dvBody" runat="server">
            <div class="cc_bluehead">
                <h3>
                <asp:Localize ID="lclDataCinfiguration" runat="server" Text="Data Configuration" 
                        meta:resourcekey="lclDataCinfigurationResource1"></asp:Localize>
                    </h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lbldate" meta:resourcekey="lbldateResource1"><font color="blue">Date Format - <b>dd/mm/yyyy</b></font> </asp:Label><br />
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div class="mainCustomerForConfig">
                    <h3>
                    <asp:Localize ID="lclHeader" runat="server" Text="Statement Processing" 
                            meta:resourcekey="lclHeaderResource1"></asp:Localize>
                       </h3>
                    <br />
                    <h5>
                    <asp:Localize ID="lclHeader1" runat="server" Text="Holding Page:" 
                            meta:resourcekey="lclHeader1Resource1"></asp:Localize>
                       </h5>
                    <ul class="customer">
                        <li>
                            <div class="inputFieldsForConfig">
                                <table>
                                    <tr>
                                        <td>
                                        <asp:Label ID="lclStart" runat="server" Text="Start:" Width="36px" 
                                                meta:resourcekey="lclStartResource1"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPtSummStartDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtPtSummStartDateResource1" />
                                            <span class="errorFields" id="spanPtSummStartDate" style="<%=spanStylePtSummStartDate%>">
                                                <%=errMsgPtSummStartDate%></span>
                                        </td>
                                        <td>
                                         <asp:Label ID="End" runat="server" Text="End:" Width="29px" 
                                                meta:resourcekey="EndResource1"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPtSummEnddate" name="StartDate" type="Date" 
                                                MaxLength="10" meta:resourcekey="txtPtSummEnddateResource1" />
                                            <span class="errorFields" id="spanPtSummEnddate" style="<%=spanStylePtSummEnddate%>">
                                                <%=errMsgPtSummEnddate%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                <div id="bigexchangedates" runat="server">
                <div class="mainCustomerForConfig">
                    <h3>
                     <asp:Localize ID="lclHeaderExchanges" runat="server" Text="Your Exchanges" 
                            meta:resourcekey="lclHeaderExchangesResource1"></asp:Localize>
                        </h3>
                    <br />
                    <h5>
                       <asp:Localize ID="lclHeaderBigExchange" runat="server" 
                            Text="Big Exchange Period:" meta:resourcekey="lclHeaderBigExchangeResource1"></asp:Localize>
                        </h5>
                    <ul class="customer">
                        <li>
                            <div class="inputFieldsForConfig">
                                <table>
                                    <tr>
                                        <td>
                                         <asp:Label ID="lclBigExStart" runat="server" Text="Start:" Width="36px" 
                                                meta:resourcekey="lclBigExStartResource1"></asp:Label>
                                          
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtExchangesStartDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtExchangesStartDateResource1" />
                                            <span class="errorFields" id="spanExchangesStartDate" style="<%=spanStyleExchangesStartDate%>">
                                                <%=errMsgExchangesStartDate%></span>
                                        </td>
                                        <td>
                                         <asp:Label ID="lclBigExEnd" runat="server" Text="End:" Width="29px" 
                                                meta:resourcekey="lclBigExEndResource1"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtExchangesEndDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtExchangesEndDateResource1" />
                                            <span class="errorFields" id="spanExchangesEndDate" style="<%=spanStyleExchangesEndDate%>">
                                                <%=errMsgExchangesEndDate%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                    <ul class="customer">
                        <h5>
                        <asp:Localize ID="lclErrorPage" runat="server" Text="Error Page:" 
                                meta:resourcekey="lclErrorPageResource1"></asp:Localize>
                            </h5>
                        <br />
                        <li>
                            <div class="inputFieldsForConfig">
                                <table>
                                    <tr>
                                        <td>
                                          <asp:Label ID="lclFlag" runat="server" Text="Flag:" Width="31px" 
                                                meta:resourcekey="lclFlagResource1"></asp:Label>
                                          
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFlag" name="StartDate" type="Numeric" 
                                                MaxLength="1" meta:resourcekey="txtFlagResource1" />
                                            <span class="errorFields" id="spanFlag" style="<%=spanStyleFlag%>">
                                                <%=errMsgFlag%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                </div>
                <div id="xmasdates" runat="server">
                <div class="mainCustomerForConfig">
                    <h3>
                    <asp:Localize ID="lclXmasSavers" runat="server" Text="Xmas Savers" 
                            meta:resourcekey="lclXmasSaversResource1"></asp:Localize>
                       </h3>
                    <ul class="customer">
                        <li>
                            <div class="inputFieldsForConfig">
                                <table>
                                    <tr>
                                        <td colspan="4" >
                                            <h5>
                                            <asp:Localize ID="lclCurrent" runat="server" Text="Current" 
                                                    meta:resourcekey="lclCurrentResource1"></asp:Localize>
                                                </h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                        <asp:Label ID="lclXmasStart" runat="server" Text="Start:" 
                                                meta:resourcekey="lclXmasStartResource1" Width="38px" ></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCurXmasStartDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtCurXmasStartDateResource1" />
                                            <span class="errorFields" id="spanCurXmasStartDate" style="<%=spanStyleCurXmasStartDate%>">
                                                <%=errMsgCurXmasStartDate%></span>
                                        </td>
                                        <td>
                                         <asp:Label ID="lclXmasEnd" runat="server" Text="End:" 
                                                meta:resourcekey="lclXmasEndResource1" Width="37px"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCurXmasEndDate" name="StartDate" type="Date" 
                                                MaxLength="10" meta:resourcekey="txtCurXmasEndDateResource1" />
                                            <span class="errorFields" id="spanCurXmasEndDate" style="<%=spanStyleCurXmasEndDate%>">
                                                <%=errMsgCurXmasEndDate%></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4"  >
                                            <h5>
                                               <asp:Localize ID="lclNext" runat="server" Text="Next" 
                                                    meta:resourcekey="lclNextResource1"></asp:Localize>
                                                </h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  >
                                         <asp:Label ID="lclNXmasStart" runat="server" Text="Start:" 
                                                meta:resourcekey="lclNXmasStartResource1" Width="36px"></asp:Label>
                                           
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNextXmasStartDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtNextXmasStartDateResource1" />
                                            <span class="errorFields" id="spanNextXmasStartDate" style="<%=spanStyleNextXmasStartDate%>">
                                                <%=errMsgNextXmasStartDate%></span>
                                        </td>
                                        <td>
                                         <asp:Label ID="lclNXmasEnd" runat="server" Text="End:" 
                                                meta:resourcekey="lclNXmasEndResource1" Width="24px"></asp:Label>
                                        
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNextXmasEndDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtNextXmasEndDateResource1" />
                                            <span class="errorFields" id="spanNextXmasEndDate" style="<%=spanStyleNextXmasEndDate%>">
                                                <%=errMsgNextXmasEndDate%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                </div>
                <div class="mainCustomerForConfig">
                    <h3>
                    <asp:Localize ID="lclVouchers" runat="server" Text="Your Vouchers" 
                            meta:resourcekey="lclVouchersResource1"></asp:Localize>
                        </h3>
                    <br />
                    <h5>
                    <asp:Localize ID="lclVouchersErrorPage" runat="server" Text="Error Page:" 
                            meta:resourcekey="lclVouchersErrorPageResource1"></asp:Localize>
                        </h5>
                    <ul class="customer">
                        <li>
                            <div class="inputFieldsForConfig">
                                <table>
                                    <tr>
                                        <td>
                                         <asp:Label ID="lclVoucherStart" runat="server" Text="Start:" Width="36px" 
                                                meta:resourcekey="lclVoucherStartResource1"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtVoucherStartDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtVoucherStartDateResource1" />
                                            <span class="errorFields" id="spanVoucherStartDate" style="<%=spanStyleVoucherStartDate%>">
                                                <%=errMsgVoucherStartDate%></span>
                                        </td>
                                        <td>
                                          <asp:Label ID="lclVoucherEnd" runat="server" Text="End:" Width="29px" 
                                                meta:resourcekey="lclVoucherEndResource1"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtVoucherEndDate" name="StartDate" type="Date" 
                                                MaxLength="10" meta:resourcekey="txtVoucherEndDateResource1" />
                                            <span class="errorFields" id="spanVoucherEndDate" style="<%=spanStyleVoucherEndDate%>">
                                                <%=errMsgVoucherEndDate%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="mainCustomerForConfig">
                    <h3>
                    <asp:Localize ID="Localize1" runat="server" Text="Your Vouchers" 
                            meta:resourcekey="lclMyStatementResource1"></asp:Localize>
                        </h3>
                    <br />
                    <h5>
                    <asp:Localize ID="lclmystatement" runat="server" Text="Holding Page:" 
                            meta:resourcekey="lclMyStatementholdingPageResource1"></asp:Localize>
                        </h5>
                    <ul class="customer">
                        <li>
                            <div class="inputFieldsForConfig">
                                <table>
                                    <tr>
                                        <td>
                                         <asp:Label ID="Label1" runat="server" Text="Start:" Width="36px" 
                                                meta:resourcekey="lclMyStatementStartResource1"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtLatestStatementStartDate" name="StartDate" type="Date"
                                                MaxLength="10" meta:resourcekey="txtMyStatementStartDateResource1" />
                                          <span class="errorFields" id="span1" style="<%=spanStyleLatestStatementStartDate%>">
                                                <%=errMsglateststatementStartDate%></span>
                                        </td>
                                        <td>
                                          <asp:Label ID="Label2" runat="server" Text="End:" Width="29px" 
                                                meta:resourcekey="lclMyStatementEndResource1"></asp:Label>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtLatestStatementEndDate" name="EndDate" type="Date" 
                                                MaxLength="10" meta:resourcekey="txtMyStatementEndDateResource1" />
                                          <span class="errorFields" id="span2" style="<%=spanStyleLatestStatementEndDate%>">
                                                <%=errMsglateststatementEndDate%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="padding-top: 10px">
                    <asp:ImageButton ID="btnConfirmConfigDtls" runat="server" ImageUrl="I/SaveChanges.gif"
                        CssClass="saveBtn" AlternateText="Confirm" 
                        OnClick="btnConfirmConfigDtls_Click" 
                        meta:resourcekey="btnConfirmConfigDtlsResource1" />
                    <input type="reset" value="Reset" style="background-color: #00539F; color: White" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdndatereg" runat="server" Value="" />
    <asp:HiddenField ID="hdnNumericeg" runat="server" Value="" />
</asp:Content>




