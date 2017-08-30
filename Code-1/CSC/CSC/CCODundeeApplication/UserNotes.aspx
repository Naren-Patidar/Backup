<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserNotes.aspx.cs"
    Inherits="CCODundeeApplication.UserNotes" Title="HelpLine Comment" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        function modalMoreInfoHide() {
            document.getElementById("modalBox").style.display = "none";
            return false;
        }
        function modalMoreInfoShow(arg) {

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

            var ShowInfo = window.open('ShowCallDetails.aspx?arg=' + arg,'ShowInfo', settings);
            ShowInfo.focus();
            return false;
        }
    </script>

    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <asp:Label ID="lblHeaderText" runat="server" meta:resourcekey="lblHeaderResource1"></asp:Label>
                    
                </h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;"></span>
                <div>
                    <br />
                    <br />
                </div>
                <table>
                    <tr>
                        <td>
                            <label style="width: 100px;">
                                <%=GetLocalResourceObject("lblReasonCodeResource").ToString()%>
                                <img class="required" src="I/asterisk.gif" alt="Required" />
                            </label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlReasonCode" runat="server" Width="200px" 
                                meta:resourcekey="ddlReasonCodeResource1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rflReasonCode" ControlToValidate="ddlReasonCode"
                                InitialValue="0" runat="server" meta:resourcekey="rflReasonCodeResource1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label style="width: 100px;">
                                <%=GetLocalResourceObject("lblCallResource").ToString()%>
                                <img class="required" src="I/asterisk.gif" alt="Required" />
                                <br />
                                <label style="width: 100px;">
                                <%=GetLocalResourceObject("lblMaxCharResource").ToString()%>
                                </label>
                                <br />
                                <label style="width: 100px;">
                                <%=GetLocalResourceObject("lblCharResource").ToString()%>
                                </label>
                            </label>
                        </td>
                        <td>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtCallDetails" TextMode="MultiLine" Height="117px"
                                    Width="413px" meta:resourcekey="txtCallDetailsResource1"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div style="float: right;">
                                <asp:ImageButton ID="btnConfirmCustomerDtls" runat="server" ImageUrl="I/SaveChanges.gif"
                                    CssClass="saveBtn" AlternateText="Confirm" 
                                    OnClick="btnConfirmCustomerDtls_Click" 
                                    meta:resourcekey="btnConfirmCustomerDtlsResource1" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="width: 70%;">
                    <asp:Label runat="server" ID="lblGridDetail" meta:resourcekey="lblGridDetailResource" style="width: 100px;">
                        
                    </asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblCallDetails" class="alertMsgs" 
                        meta:resourcekey="lblCallDetailsResource1"></asp:Label>
                    <div class="customerSearch" id="dvSearchResults" runat="server" visible="false">
                        <div class="clubcardAcct">
                            <div class="clubcardAct_head">
                                <h4><%=GetLocalResourceObject("lblHeaderResource").ToString()%></h4>
                            </div>
                            <div class="inputFields" style="width: 100%; float: left;">
                                <asp:GridView CssClass="cardHolderTbl" ID="grdCallDetails" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="True" OnPageIndexChanging="grdCallDetails_PageIndexChanging" PageSize="20" AlternatingRowStyle-CssClass="alternate"
                                    Width="738px" 
                                    OnRowDataBound="grdTransactions_RowDataBound" 
                                    onselectedindexchanged="grdCallDetails_SelectedIndexChanged" 
                                    meta:resourcekey="grdCallDetailsResource1">
                                    <PagerSettings Mode="Numeric" Position="Bottom" PageButtonCount="10"></PagerSettings>
                                    <PagerStyle HorizontalAlign="Center"></PagerStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date Logged" HeaderStyle-Font-Bold="true" 
                                            ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource1">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltrDateLogged" runat="server" Text='<%# Bind("DateLogged") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <ItemStyle CssClass="GridTd" />
                                            <FooterStyle CssClass="GridFooterTdLeft" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason Code" HeaderStyle-Font-Bold="true" 
                                            ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource2">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltrReason" runat="server" Text='<%# Bind("ReasonCodeID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <ItemStyle CssClass="GridTd" />
                                            <FooterStyle CssClass="GridFooterTdLeft" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Logged By" HeaderStyle-Font-Bold="true" 
                                            ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltrLoggedBy" runat="server" Text='<%# Bind("LoggedBy") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="true" CssClass="GridHeader" />
                                            <ItemStyle Wrap="true" CssClass="GridTdCenter" />
                                            <FooterStyle Wrap="true" CssClass="GridFooterTdLeft" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Call Details" HeaderStyle-Font-Bold="true" 
                                            ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource4">
                                            <ItemTemplate>
                                                <div style="float: left;">
                                                    <asp:Literal ID="ltrCallDetails" runat="server" Text='<%# Bind("CallDetail") %>' /></div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="GridTd" />
                                            <FooterStyle CssClass="GridFooterTd" />
                                            <HeaderStyle CssClass="GridHeader" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Full Call Details" HeaderStyle-Font-Bold="true" 
                                            ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource5">
                                            <ItemTemplate>
                                                <asp:LinkButton CommandName="ViewDetails" CommandArgument='<%# Bind("CallID") %>'
                                                    ID="lnkSelectCallDetails" runat="server" 
                                                    meta:resourcekey="lnkSelectCallDetailsResource1"></asp:LinkButton></div>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
