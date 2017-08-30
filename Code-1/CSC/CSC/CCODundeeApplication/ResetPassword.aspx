<%@ Page Language="C#" Title="Reset Password" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="ResetPassword.aspx.cs" Inherits="CCODundeeApplication.ResetPassword" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <asp:Localize ID="lclResetPwd" Text="Reset Password" runat="server" 
                        meta:resourcekey="lclResetPwdResource1" ></asp:Localize></h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">
                 <asp:Localize ID="lclMsg" Text="Please enter group details to search" 
                    runat="server" meta:resourcekey="lclMsgResource1" ></asp:Localize></span>
                <div class="UserDetails">
                    <h3>
                    <asp:Localize ID="lclMainCustomer" Text="Main Customer :" runat="server" 
                            meta:resourcekey="lclMainCustomerResource1" ></asp:Localize> 
                        
                        <asp:Label ID="lblCustName" runat="server" 
                            meta:resourcekey="lblCustNameResource1"></asp:Label></h3>
                    <p>
                        &nbsp;</p>
                </div>
                <div class="EmptyDiv">
                </div>
                <div align="center" class="resetpwd">
                    <div style="width: 30%;">
                        <asp:ImageButton ID="imgbtnEnablePswd" runat="server" ImageAlign="Middle" ImageUrl="~/I/enablepswd.jpg"
                            OnClick="imgbtnEnablePswd_Click" 
                            meta:resourcekey="imgbtnEnablePswdResource1" /></div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnEmailAddress" runat="server" />
    </div>
</asp:Content>
