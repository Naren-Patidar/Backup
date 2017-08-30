<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FindGroup.aspx.cs"
    Inherits="CCODundeeApplication.FindGroup" Title="Find Group" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() 
        {
          
            var errMsgGroupName = '<%=Resources.CSCGlobal.errMsgEditGroupName %>';//"Please Enter Group Name.";
            var errMsgvalid = '<%=Resources.CSCGlobal.errMsgEditvalid %>';//"Please Enter a valid String.";
            var errMsgAddLimit = '<%=Resources.CSCGlobal.errMsgEditAddLimit %>';//"Please enter a numeric value";
            var errMsgSubLimit = '<%=Resources.CSCGlobal.errMsgEditSubLimit %>';//"Please enter a numeric value";
            var errorFlag = "";
            var regNumeric = /^[0-9]*$/;
            var groupName = trim(document.getElementById("<%=txtEditGroupName.ClientID%>").value);

                if (groupName == "") 
                {
                    //To clear the error messages if already displayed.
                     document.getElementById("spanGroupName").style.display = '';
                     document.getElementById("<%=txtEditGroupName.ClientID%>").className = 'errorFld';
                     document.getElementById("spanGroupName").innerText = errMsgGroupName;
                     errorFlag="Error";
                }
                else 
                {
                     document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = 'none';
                     document.getElementById("spanGroupName").style.display =  'none';
                     document.getElementById("<%=txtEditGroupName.ClientID%>").className = '';
                }
                
              errorFlag = ValidateTextBox("<%=txtAddLimit.ClientID%>", regNumeric, true, false, "spanAddLimit", errMsgAddLimit);
              errorFlag = errorFlag + ValidateTextBox("<%=txtSubstract.ClientID%>", regNumeric, true, false, "spanSubLimit", errMsgSubLimit);
            if (errorFlag != "") {
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3><asp:Localize ID="lclHeader" runat="server" meta:resourcekey="Header"></asp:Localize>
                    </h3>
            </div>
            <div class="cc_body">
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">
                <asp:Localize ID="lclErrMsg" runat="server" meta:resourcekey="lblErrMsg"></asp:Localize>
                </span>
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div class="UserDetails">
                    <h3><asp:Localize ID="lclGrpHeader" runat="server" meta:resourcekey="lblGrpHeader"></asp:Localize>
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
                                <asp:Localize ID="lclGroupName" runat="server" meta:resourcekey="lblGroupName"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtGroupName" name="GroupName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtGroupNameResource1" />
                                   
                                </div>
                            </li>
                            <li>
                                <label for="FirstName">
                                 <asp:Localize ID="lclDescription" runat="server" meta:resourcekey="lblDescription"></asp:Localize>
                                    </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtDescription" name="Description" type="text" 
                                        meta:resourcekey="txtDescriptionResource1" />
                                    <%--<span class="errorFields" id="span3" style="<%=spanStyleFirstName0%>"><%=errMsgFirstName%></span>--%>
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
                                <h4><asp:Localize ID="lclSearch" runat="server" meta:resourcekey="lblSearch"></asp:Localize>
                                    </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdGroupDetails" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" PagerSettings-Visible="false" AlternatingRowStyle-CssClass="alternate"
                                OnRowCommand="grdGroupDetails_RowCommand" OnRowDataBound="grdCustomerDetail_RowDataBound"
                                OnRowDeleting="grdGroupDetails_RowDeleting" 
                                OnPageIndexChanging="grdGroupDetails_PageIndexChanging" 
                                meta:resourcekey="grdGroupDetailsResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("RoleID")  + ";" + Eval("RoleName") + ";" + Eval("RoleDesc") + ";" + Eval("AmendBy") + ";" + Eval("AmendDateTime") %>'
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
                                            <asp:Literal ID="ltrUserName" runat="server" Text='<%# Bind("RoleName") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-Font-Bold="true" 
                                        meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrDescription" runat="server" Text='<%# Bind("RoleDesc") %>' />
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
                                </Columns>
                                <EmptyDataTemplate>
                                 <asp:Localize ID="lclEmpty" runat="server" meta:resourcekey="lblEmpty"></asp:Localize>
                                </EmptyDataTemplate>
                                <PagerStyle CssClass="pagination" />
                                <PagerSettings Mode="NumericFirstLast" PreviousPageImageUrl="I/previousDisabled.gif"
                                    NextPageImageUrl="I/next.gif" />

                            <AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="dvEditUser" runat="server" visible="false">
                    <div class="AddUser">
                        <ul class="customer">
                            <li>
                                <label for="firstName">
                                <asp:Localize ID="lclEditGroupName" runat="server" meta:resourcekey="lblEditGroupName"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditGroupName" name="firstName" type="text" 
                                        meta:resourcekey="txtEditGroupNameResource1" />
                                    <span class="errorFields" id="spanGroupName" style="<%=spanGroupName%>">
                                        <%=errMsgGroupName%></span>
                                </div>
                            </li>
                            <li>
                                <label for="initial">
                                <asp:Localize ID="lclEditDescription" runat="server" meta:resourcekey="lblEditDescription"></asp:Localize>
                                    </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditGroupDescription" name="initial" 
                                        type="text" meta:resourcekey="txtEditGroupDescriptionResource1" />
                                    <%--<span class="errorFields" id="spanMiddleName0" style="<%=spanStyleMiddleName0%>">
                                        <%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="AddUser">
                        <h3>
                         <asp:Localize ID="lclEditAccessHeader" runat="server" meta:resourcekey="lblEditAccessHeader"></asp:Localize>
                            </h3>
                        <p>
                            &nbsp;</p>
                    </div>
                    <div style="float: left;" align="center">
                        <asp:ImageButton ID="ImgSave" ImageUrl="~/I/btnSave.jpg" runat="server" OnClientClick="return ValidateFields()"
                            Height="37px" OnClick="ImgSave_Click" 
                            meta:resourcekey="ImgSaveResource1" />
                    </div>
                    <div class="EditGrpMembership" id="divEditGroup" runat="server">
                        <div style="width: 95%;">
                            <div class="clubcardAct_head">
                                <h4>
                                    <asp:Localize ID="Header2" runat="server" Text="Edit Group Membership" meta:resourceKey="SearchResults"></asp:Localize>
                                </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdRoleCapability" runat="server" PageSize="5"
                                AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="grdRoleMembership_RowCommand"
                                OnRowDeleting="grdRoleCapability_RowDeleting" OnRowDataBound="grdRoleCapability_RowDataBound"
                                OnPageIndexChanging="grdRoleCapability_PageIndexChanging" 
                                meta:resourcekey="grdRoleCapabilityResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Accessible Screens" 
                                        meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrLastUpdatedBy" runat="server" Text='<%# Bind("CapabilityName") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove" 
                                        meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Delete" Text="Delete" CommandArgument='<%# Eval("RoleID")  + ";" + Eval("CapabilityID") + ";" %>'
                                                ID="lnkDeleteCapabilty" runat="server" 
                                                meta:resourcekey="lnkDeleteCapabiltyResource1" />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                              <EmptyDataTemplate>
                                 <asp:Localize ID="lclEmptyMsg" runat="server" meta:resourcekey="lblEmptyMsg"></asp:Localize>
                                </EmptyDataTemplate>
                                <AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                        </div>
                        <div>
                        </div>
                    </div>
                    <div style="height: 50px; float: left" class="AddUser">
                        <label for="firstName">
                        <asp:Localize ID="lclAddScreen" runat="server" meta:resourceKey="lblAddScreen"></asp:Localize>
                            </label>
                        <div class="" style="float: left;">
                            <asp:DropDownList ID="ddlCapability"  runat="server" 
                                meta:resourcekey="ddlCapabilityResource1">
                            </asp:DropDownList>
                            <%--<span class="errorFields" id="span3" style="<%=spanStyleFirstName0%>"><%=errMsgFirstName%></span>--%></div>
                        <div class="" style="float: left">
                            <asp:Button ID="btnAdd" runat="server" Text="ADD" BackColor="#49BCD7" ForeColor="White"
                                Font-Bold="True" Height="24px" Width="34px" OnClick="btnAdd_Click" 
                                meta:resourcekey="btnAddResource1" />
                        </div>
                    </div>
                    <div class="AddUser" id="dvChangePointsLimit" runat="server">
                        <h3>
                         <asp:Localize ID="lclEditPointHeader" runat="server" meta:resourceKey="lblEditPointHeader"></asp:Localize>
                            </h3>
                        <p>
                            &nbsp;</p>
                    </div>
                    <div class="AddUser" id="changepointslimit" runat="server">
                        <table width="95%" style="height: 50px;">
                            <tr>
                                <td>
                                 <asp:Localize ID="lclMaxPoints" runat="server" meta:resourceKey="lblMaxPoints"></asp:Localize>
                                    
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAddLimit" name="AddLimit" Text="0" type="text"
                                        Height="25px" Width="100px" meta:resourcekey="txtAddLimitResource1" />
                                    <span class="errorFields" id="spanAddLimit" style="<%=spanAddLimit%>">
                                        <%=errMsgAddLimit%></span>
                                </td>
                                <td>
                                <asp:Localize ID="lclMinPoints" runat="server" meta:resourceKey="lblMinPoints"></asp:Localize>
                                   
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSubstract" name="SubstractLimit" Text="0" type="text"
                                        Height="25px" Width="100px" meta:resourcekey="txtSubstractResource1" />
                                    <span class="errorFields" id="spanSubLimit" style="<%=spanSubLimit%>">
                                        <%=errMsgSubLimit%></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnNumericeg" Value="" />
</asp:Content>
