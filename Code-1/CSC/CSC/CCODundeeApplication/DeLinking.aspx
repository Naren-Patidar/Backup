<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeLinking.aspx.cs"
    Inherits="CCODundeeApplication.DeLinking" MasterPageFile="~/Site.Master"
    Title="Customer details" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    
    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/CustomerDetails.js" type="text/javascript"></script>

    <div id="mainContent" runat="server">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="Customerdetails"><asp:Localize ID="lclCustomerDetails" 
                        runat="server" Text="De-Link Accounts" 
                        meta:resourcekey="lclCustomerDetailsResource1"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div class="mainCustomer" id="divMainCustomer" runat="server">
                    <h3 style="height:30px">
                        <label for="MainCustomer" style="width:100%;"><asp:Localize ID="lclMainCustomer" runat="server" 
                            Text="Main Customer" meta:resourcekey="lclMainCustomerResource1"></asp:Localize></label></h3>
                    <ul class="customer">
                        <li>
                            <label for="title">
                                <asp:Localize ID="lclTitle" runat="server" Text="Title:" 
                                meta:resourcekey="lclTitleResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtTitle0" name="firstName" type="text" 
                                    MaxLength="20" meta:resourcekey="txtTitle0Resource1" Enabled="False" />
                            </div>
                        </li>
                        <li>
                            <label for="firstName">
                                <asp:Localize ID="lclFirstName" runat="server" Text="First Name:" 
                                meta:resourcekey="lclFirstNameResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtFirstName0" name="firstName" type="text" 
                                    MaxLength="20" meta:resourcekey="txtFirstName0Resource1" 
                                    Enabled="False"  />
                            </div>
                        </li>
                        <li>
                            <label for="initial">
                                <asp:Localize ID="lclinitials" runat="server" Text="Middle Initial(s):" 
                                meta:resourcekey="lclinitialsResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtInitial0" name="initial" type="text" 
                                    MaxLength="2" meta:resourcekey="txtInitial0Resource1" Enabled="False"  />
                            </div>
                        </li>
                        <li>
                            
                            
                            <label for="surname" id="lblSurname" runat="server" visible="true">
                                <asp:Localize ID="lclSurname" runat="server" Text="Surname:" 
                                meta:resourcekey="lclSurnameResource1" ></asp:Localize></label>
                                <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtSurname0" name="surname" type="text" 
                                    MaxLength="25" meta:resourcekey="txtSurname0Resource1" Enabled="False"  /></div>
                        </li>
                       
                    </ul>
                    <div id="dvMainAlterCustomer" runat="server">
                    <panel id="PanelforGrid" runat="server">
                    <h3 style="height:30px"> <label for="Delink" runat="server" id="lblDelink" style="width:100%">
                        <asp:Localize ID="Localize1" runat="server" 
                            Text="De-Link Account" meta:resourcekey="Localize1Resource1"></asp:Localize></label></h3>
                    <ul>
                    <li>
                     <label for="paragraphofDelink" runat="server" id="lblparagraphofDelink" style="width:100%;height:30px;">
                            <asp:Localize ID="lclDelinkParagraph" runat="server" 
                            Text="This ClubCard account should be de-linked from the associated Dot Com account" 
                            meta:resourcekey="lclDelinkParagraphResource1"></asp:Localize></label>
                    
                    </li>
                    <li>
                    
                        <asp:GridView CssClass="cardHolderTbl" ID="grdMainCustomer" runat="server"
                                AutoGenerateColumns="False" AllowPaging="True" 
                            OnRowDataBound="grdMainCustomer_RowDataBound" 
                            OnRowCommand="grdMainCustomer_RowCommand" OnRowDeleting="grdMainCustomer_RowDeleting"  
                                meta:resourcekey="grdRoleMembershipResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Teleshopper ID" 
                                        meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrLastUpdatedBy" runat="server" Text='<%# Bind("CusAlterId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delink" 
                                        meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Delete" Text="De-Link Account" CommandArgument='<%# Eval("CusAlterId") %>'
                                                ID="lnkDelCustomerDotcom" runat="server" 
                                                meta:resourcekey="lnkDeleteUserRoleResource1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                <asp:Localize ID="lclGridEmptyMsg" runat="server" meta:resourceKey="lblGridEmptyMsg"></asp:Localize>
                                    
                                </EmptyDataTemplate>
                                <AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                       
                    </li>
                    
                    </ul>
                      </panel>
                    </div>
                     
                </div>
                <div class="associateCustomer" id="dvAssociateCustomer" runat="server" visible="false">
                    <h3 style="height:30px">
                        <label for="Associate Customer" style="width:100%;">
                        <asp:Localize ID="lclAssociateCustomer" 
                            runat="server" Text="Associate Customer" 
                            meta:resourcekey="lclAssociateCustomerResource1"></asp:Localize>
                        </label></h3>
                    <ul class="customer">
                        <li>
                            <label for="title">
                            <asp:Localize ID="lclatitle" runat="server" Text="Title:" 
                                meta:resourcekey="lclatitleResource1"></asp:Localize></label>
                            <div class="inputFields">
                               <asp:TextBox ID="txtTitleAss" runat="server" MaxLength="10" Enabled="False" 
                                    meta:resourcekey="txtTitleAssResource1"  />
                            </div>
                        </li>
                        <li>
                            <label for="associateFirstName">
                            <asp:Localize ID="lclaFirstName" runat="server" Text="First Name:" 
                                meta:resourcekey="lclaFirstNameResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtFirstName1" name="firstName" type="text" 
                                    MaxLength="20" meta:resourcekey="txtFirstName1Resource1" 
                                    Enabled="False"  />
                            </div>
                        </li>
                        <li>
                            <label for="associateMiddleinitial">
                            <asp:Localize ID="lclaMiddleInitial" runat="server" 
                                Text="Middle Initial(s):" meta:resourcekey="lclaMiddleInitialResource1"></asp:Localize>
                            </label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtInitial1" name="initial" type="text" 
                                    MaxLength="2" meta:resourcekey="txtInitial1Resource1" Enabled="False"  />
                                </div>
                        </li>
                        <li>
                            <label for="associateSurname">
                            <asp:Localize ID="lclaSurname" runat="server" Text="Surname:" 
                                meta:resourcekey="lclaSurnameResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtSurname1" name="surname" type="text" 
                                    MaxLength="25" meta:resourcekey="txtSurname1Resource1" Enabled="False"  /></div>
                        </li>
                     </ul>
                  <div id="dvAssAlterCustomer" runat="server">
                    <h3 style="height:30px"> <label for="DelinkAss" runat="server" id="lblforAssCus" style="width:100%">
                        <asp:Localize ID="lclDelinkAss" runat="server" 
                            Text="De-Link Account" meta:resourcekey="lclDelinkAssResource1"></asp:Localize></label></h3>
                    <ul>
                    <li>
                     <label for="paragraphofDelinkAss" runat="server" id="lblparAssCus" style="width:100%;height:30px;">
                            <asp:Localize ID="lcl" runat="server" 
                            Text="This ClubCard account should be de-linked from the associated Dot Com account" 
                            meta:resourcekey="lclResource1"></asp:Localize></label>
                    
                    </li>
                    <li>
                     <asp:GridView CssClass="cardHolderTbl" ID="GridForAssociative" runat="server"
                                AutoGenerateColumns="False" AllowPaging="True" 
                            OnRowDataBound="GridForAssociative_RowDataBound" 
                            OnRowCommand="GridForAssociative_RowCommand" OnRowDeleting="GridForAssociative_RowDeleting"  
                                meta:resourcekey="grdRoleMembershipResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Teleshopper ID" 
                                        meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrLastUpdatedBy" runat="server" 
                                                Text='<%# Bind("CusAlterId") %>'   />
                                              
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="De-Link" 
                                        meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Delete" Text="De-Link Account" CommandArgument='<%# Eval("CusAlterId") %>'
                                                ID="lnkDelCustomerDotcomAss" runat="server" 
                                                meta:resourcekey="lnkDeleteUserRoleResource1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                <asp:Localize ID="lclGridEmptyMsg" runat="server" meta:resourceKey="lblGridEmptyMsg"></asp:Localize>
                                    
                                </EmptyDataTemplate>
                                <AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                     </li></ul>
        </div>
        </div>
        <asp:HiddenField ID="hdnNumberOfCustomers" runat="server" />
        <asp:HiddenField ID="hdnPrimaryCustID" runat="server" />
        <asp:HiddenField ID="hdnAssociateCustID" runat="server" />
         <!-- Below are the hidden fields to store the Associate customer detail when its disabled-->
        <asp:HiddenField ID="hdnddlTitle1" runat="server" />
        <asp:HiddenField ID="hdntxtFirstName1" runat="server" />
        <asp:HiddenField ID="hdntxtInitial1" runat="server" />
        <asp:HiddenField ID="hdntxtSurname1" runat="server" />
        <asp:HiddenField ID="hdnMEdit" runat="server" Value="false" />
        </div>
        </div>
        </div>
        
       
</asp:Content>
