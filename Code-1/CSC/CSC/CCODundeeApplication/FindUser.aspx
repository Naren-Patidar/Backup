<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FindUser.aspx.cs"
    Inherits="CCODundeeApplication.FindUser" Title="Find User" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3><asp:Localize ID="lclHeader" runat="server" meta:resourcekey="Header"></asp:Localize>
                   </h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div class="UserDetails">
                    <h3><asp:Localize ID="lclUserDetails" runat="server" meta:resourcekey="HeaderUserDetials"></asp:Localize>
                       </h3>
                    <p>
                        &nbsp;</p>
                </div>
                <div class="EmptyDiv">
                </div>
                <div id="dvFindUser" runat="server">
                    <div style="width: 60%" id="dvAddUser" runat="server">
                        <br />
                        <ul class="customer">
                            <li>
                                <label for="UserName">
                                <asp:Localize ID="lclUserName" runat="server" meta:resourcekey="lblUserName"></asp:Localize>
                                  </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtUserName" name="UserName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtUserNameResource1" />
                                    <%--<span class="errorFields" id="spanLastName" style="<%=spanStyleMiddleName0%>"><%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label for="FirstName">
                                 <asp:Localize ID="lclDescription" runat="server" meta:resourcekey="lblDescription"></asp:Localize>
                                    </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtDescription" name="FirstName" type="text" 
                                        meta:resourcekey="txtDescriptionResource1" />
                                    <%--<span class="errorFields" id="span3" style="<%=spanStyleFirstName0%>"><%=errMsgFirstName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label><asp:Localize ID="lclGroup" runat="server" meta:resourcekey="lblGroup"></asp:Localize>
                                   </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtGroup" name="initial" type="text" 
                                        meta:resourcekey="txtGroupResource1" />
                                    <%--<span class="errorFields" id="spanLastName" style="<%=spanStyleMiddleName0%>"><%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" OnClick="btnSearch_Click" 
                                        meta:resourcekey="btnSearchResource1" />
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="UserSearch" id="dvSearchResults" runat="server" visible="false">
                        <div class="clubcardAcct">
                            <div class="clubcardAct_head">
                                <h4><asp:Localize ID="lclGridHeader" runat="server" meta:resourcekey="lblGridHeader"></asp:Localize>
                                    </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdCustomerDetail" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" OnRowCommand="GrdCustomerDetail_RowCommand" AlternatingRowStyle-CssClass="alternate"
                                OnRowDataBound="grdCustomerDetail_RowDataBound" 
                                OnPageIndexChanging="grdCustomerDetail_PageIndexChanging" 
                                meta:resourcekey="grdCustomerDetailResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("UserID")  + ";" + Eval("UserName") + ";" + Eval("UserDescription")+ ";" + Eval("UserStatusCode")+ ";" + Eval("AmendBy")+ ";" + Eval("AmendDateTime") %>'
                                                ID="lnkSelectCustomer" runat="server" 
                                                meta:resourcekey="lnkSelectCustomerResource1"><%--<asp:Image ID="Image1" 
                                                runat="server" ImageUrl="I/GotoSearchCustomer.gif" 
                                                meta:resourcekey="Image1Resource1" />--%></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name" HeaderStyle-Font-Bold="true" 
                                        meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrUserName" runat="server" Text='<%# Bind("UserName") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-Font-Bold="true" 
                                        meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrDescription" runat="server" Text='<%# Bind("UserDescription") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Updated By" HeaderStyle-Font-Bold="true" 
                                        meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrLastUpdatedBy" runat="server" Text='<%# Bind("AmendBy") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Updated Date" HeaderStyle-Font-Bold="true" 
                                        meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrUpdatedDate" runat="server" Text='<%# Bind("AmendDateTime") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Font-Bold="true" 
                                        meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrStatus" runat="server" Text='<%# Bind("UserStatusCode") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                <asp:Localize ID="lclEmptyMsg" runat="server" meta:resourcekey="lblEmptyMsg"></asp:Localize>
                                   
                                </EmptyDataTemplate>

<AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="dvEditUser" runat="server" style="height: 100%" visible="false">
                    <div class="AddUser" style="padding-bottom: 10px;">
                        <ul class="customer">
                            <li>
                                <label for="firstName">
                                  <asp:Localize ID="lclEditUserName" runat="server" meta:resourcekey="lblEditUserName"></asp:Localize>
                                    </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditUserName" name="firstName" ReadOnly="True"
                                        type="text" meta:resourcekey="txtEditUserNameResource1" />
                                    <%--<span class="errorFields" id="spanFirstName0" style="<%=spanStyleFirstName0%>">
                                        <%=errMsgFirstName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label for="initial">
                                 <asp:Localize ID="lclEditDescription" runat="server" meta:resourcekey="lblEditDescription"></asp:Localize>
                                    </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditDescription" name="initial" type="text" 
                                        ReadOnly="True" meta:resourcekey="txtEditDescriptionResource1" />
                                    <%--<span class="errorFields" id="spanMiddleName0" style="<%=spanStyleMiddleName0%>">
                                        <%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                        </ul>
                        <div style="width: 55%;">
                            <label style="padding-left: 50px; width: 60px;">
                            <asp:Localize ID="lclEditStatus" runat="server" meta:resourcekey="lblEditStatus"></asp:Localize>
                                </label>
                            <asp:RadioButton runat="server" Checked="True" ID="rbtnEnabled" name="Enabled" GroupName="Gender"
                                Text="Enabled" meta:resourcekey="rbtnEnabledResource1" />
                            <asp:RadioButton runat="server" ID="rbtnDisabled" name="Disabled" GroupName="Gender"
                                Text="Disabled" meta:resourcekey="rbtnDisabledResource1" />
                        </div>
                    </div>
                    <div class="AddUser" style="width: 75%">
                        <h3>
                         <asp:Localize ID="lclEditGrpMemMsg" runat="server" meta:resourcekey="lblEditGrpMemMsg"></asp:Localize>
                            </h3>
                        <p>
                            &nbsp;</p>
                    </div>
                    <div style="float: left;" align="center">
                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/I/btnSave.jpg" runat="server" Height="37px"
                            OnClick="ImageButton1_Click" meta:resourcekey="ImageButton1Resource1" />
                    </div>
                    <div class="EditGrpMembership" id="Div1" runat="server">
                        <div>
                            <div class="clubcardAct_head">
                                <h4>
                                    <asp:Localize ID="Header2" runat="server" Text="Edit Group Membership" meta:resourceKey="SearchResults"></asp:Localize>
                                </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdRoleMembership" runat="server" PageSize="5"
                                AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="grdRoleMembership_RowCommand"
                                OnRowDataBound="grdRoleMembership_RowDataBound" OnRowDeleting="grdRoleMembership_RowDeleting"
                                OnPageIndexChanging="grdRoleMembership_PageIndexChanging" 
                                meta:resourcekey="grdRoleMembershipResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Group Membership" 
                                        meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrLastUpdatedBy" runat="server" Text='<%# Bind("RoleName") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove" 
                                        meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Delete" Text="Delete" CommandArgument='<%# Eval("RoleID")  + ";" + Eval("UserID") %>'
                                                ID="lnkDeleteUserRole" runat="server" 
                                                meta:resourcekey="lnkDeleteUserRoleResource1" />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                <asp:Localize ID="lclGridEmptyMsg" runat="server" meta:resourceKey="lblGridEmptyMsg"></asp:Localize>
                                    
                                </EmptyDataTemplate>
                                <AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                        </div>
                        <div>
                        </div>
                    </div>
                    <div style="width: 75%; height: 50px; float: left" class="AddUser">
                        <label for="firstName">
                        <asp:Localize ID="lclAddGroup" runat="server" meta:resourceKey="lblAddGroup"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:DropDownList ID="ddlGroups" Width="75%" runat="server" 
                                meta:resourcekey="ddlGroupsResource1">
                            </asp:DropDownList>
                            <%--<span class="errorFields" id="span3" style="<%=spanStyleFirstName0%>"><%=errMsgFirstName%></span>--%></div>
                        <div class="" style="float: left">
                            <asp:Button ID="btnAdd" runat="server" Text="ADD" BackColor="#49BCD7" ForeColor="White"
                                Font-Bold="True" Height="24px" Width="34px" OnClick="btnAdd_Click" 
                                meta:resourcekey="btnAddResource1" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
