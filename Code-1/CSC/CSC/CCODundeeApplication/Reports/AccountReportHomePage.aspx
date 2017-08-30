<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountReportHomePage.aspx.cs"
    Inherits="CCODundeeApplication.Reports.AccountReportHomePage" MasterPageFile="~/Site.Master" Title="Account Report Home Page" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
             <div class="cc_bluehead">
                 <h3><asp:Localize ID="lclHeader" runat="server" 
                         Text="Account and operations report" meta:resourcekey="lclHeaderResource1"></asp:Localize>
                  </h3> 
            </div>
            <div class="cc_body">
            <div  id="dvSearchResults" runat="server">
                    <div class="clubcardAcct" style="width:50%">
                        <div class="clubcardAct_head">
                            <h4>
                                <asp:Localize ID="lclGridHeader" runat="server" 
                                    Text="Account and operations reports" meta:resourcekey="lclGridHeaderResource1"></asp:Localize></h4>
                                    
                        </div>
                        <table class="cardHolderTbl" id="grdReportList" width="100%">
                        <tr>
                        <td align="left" width="20%" bgcolor="#FF3300">
                         <label id="lblSelect" style="color:White;font-weight:bold">
                         <asp:Localize ID="lclSelect" runat="server" Text="Select" 
                                meta:resourcekey="lclSelectResource1"></asp:Localize></label>
                        </td>
                          <td align="left"  width="80%" bgcolor="#3333CC">
                          <label id="lblReport" style="color:White;font-weight:bold"> 
                              <asp:Localize ID="Localize3" runat="server" Text="Report" 
                                  meta:resourcekey="Localize3Resource1"></asp:Localize></label>
                        
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <a href="ClubcardRegistrationReport.aspx" runat="server" id="lnkClubcardRegistrationReport">
                        <img src="../I/GotoSearchCustomer.gif" />
                        </a>
                        </td>
                        <td align="left">
                         
                            <label id="ltrRegistrationsReport" style="width:70%;color:#000000">
                             <asp:Localize ID="Localize1" runat="server" 
                                Text="New Clubcard registrations report" meta:resourcekey="Localize1Resource1"></asp:Localize>
                            </label>
                        </td>
                        </tr>
                        <tr style="background-color:#EEEEEE">
                        <td>
                         <a href="CustomerLoadReport.aspx" runat="server" id="lnkCustomerLoadReport">
                        <img src="../I/GotoSearchCustomer.gif" />
                        </a>
                        </td>
                        <td align="left">
                        <label id="ltrLoadReport" style="width:45%;color:#000000">
                         <asp:Localize ID="Localize2" runat="server" Text="Customer load report" 
                                meta:resourcekey="Localize2Resource1"></asp:Localize></label>
                        
                        </td>
                        </tr>
                        </table>
           
             </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
