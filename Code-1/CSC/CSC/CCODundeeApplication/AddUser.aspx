<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddUser.aspx.cs"
    Inherits="CCODundeeApplication.AddUser" Title="Add User" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() {

            var errMsgUserName ='<%=Resources.CSCGlobal.errMsgUserName %>';// "Please Enter Valid UserName to validate";
            var errorFlag = "";
            
            var userName = trim(document.getElementById("<%=txtUserName.ClientID%>").value);

            if (userName == "") 
            {
                //To clear the error messages if already displayed.
                
                 document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = '';
                 document.getElementById("<%=txtUserName.ClientID%>").className = '';
                return false;
            }
            else 
            {
                document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = 'none';
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
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">
                <asp:Localize ID="lclValidUserName" runat="server" meta:resourcekey="lblValidUserName"></asp:Localize></span>
                <div class="UserDetails">
                    <h3><asp:Localize ID="lclHeader1" runat="server" meta:resourcekey="Header1"></asp:Localize>
                       </h3>
                    <p>
                        &nbsp;</p>
                </div>
                <div class="EmptyDiv">
                </div>
                <div style="width: 60%">
                    <br />
                    <ul class="customer">
                        <li>
                            <label for="UserName"><asp:Localize ID="lclUserNAme" runat="server" meta:resourcekey="lblUserName"></asp:Localize>
                               <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields" style="float: left">
                                <asp:TextBox runat="server" ID="txtUserName" name="UserName" ToolTip="TPX ID" 
                                    type="text" meta:resourcekey="txtUserNameResource1" />
                                <%-- <span class="errorFields" id="spanUserName" style="<%=spanUserName%>"><%=errMsgUserName%></span>--%>
                            </div>
                            <div>
                                <asp:Button ID="btnValidate" runat="server" Text="Validate" BackColor="#49BCD7" OnClientClick="return ValidateFields()"
                                    ForeColor="White" Font-Bold="True" Height="24px" Width="64px" 
                                    OnClick="btnValidate_Click" meta:resourcekey="btnValidateResource1" /></div>
                        </li>
                        <li>
                            <label for="FirstName"><asp:Localize ID="lclFirstName" runat="server" meta:resourcekey="lblFirstName"></asp:Localize>
                                </label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtFirstName" name="FirstName" ReadOnly="True" 
                                    type="text" meta:resourcekey="txtFirstNameResource1" />
                                <%--<span class="errorFields" id="span3" style="<%=spanStyleFirstName0%>"><%=errMsgFirstName%></span>--%>
                            </div>
                        </li>
                        <li>
                            <label><asp:Localize ID="lclLastName" runat="server" meta:resourcekey="lclLastName"></asp:Localize>
                                </label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtLastName" name="initial" type="text" 
                                    ReadOnly="True" meta:resourcekey="txtLastNameResource1" />
                                <%--<span class="errorFields" id="spanLastName" style="<%=spanStyleMiddleName0%>"><%=errMsgMiddleName%></span>--%>
                            </div>
                        </li>
                    </ul>
                    <ul class="customer">
                        <li>
                            <label for="firstName"><asp:Localize ID="lclAddGroup" runat="server" meta:resourcekey="lblAddGroup"></asp:Localize>
                                </label>
                            <div class="inputFields">
                                <asp:DropDownList ID="ddlGroups" Width="77%" runat="server" 
                                    meta:resourcekey="ddlGroupsResource1">
                                  
                                </asp:DropDownList>
                                <%--<span class="errorFields" id="span3" style="<%=spanStyleFirstName0%>"><%=errMsgFirstName%></span>--%></div>
                        </li>
                        <li>
                            <div class="inputFields" style="padding-left: 175px;">
                                <asp:Button ID="btnAdd" runat="server" Text="ADD" BackColor="#49BCD7" ForeColor="White"
                                    Font-Bold="True" Height="24px" Width="38px" OnClick="btnAdd_Click" 
                                    Enabled="False" meta:resourcekey="btnAddResource1" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
