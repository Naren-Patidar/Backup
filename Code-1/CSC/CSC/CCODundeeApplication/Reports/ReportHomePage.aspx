<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportHomePage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="CCODundeeApplication.Reports.ReportHomePage" Title="Report Home page"  meta:resourcekey="PageResource1" uiculture="auto"   %>



<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                
                  <h3><asp:Localize ID="lclHeader" runat="server" 
                         Text="Welcome to the NGC Reporting Application" meta:resourcekey="lclHeaderResource1"></asp:Localize>
                  </h3> 
                
            </div>
            <div class="cc_body">
           
 
                  <span class="errorFields" runat="server" id="spnMessage">
                  <asp:Localize ID="ltrRptMessage" runat="server"
                                meta:resourcekey="ltrMessage"></asp:Localize>
                
                  </span>
                  <br />
                   <br />
                    <br />
                     <br />
                      <br />
                       <br />
                        <br />
                         <br />
                         <br />
                         <br />
                         <br />
                         <br />
            </div>
        </div>
    </div>
</asp:Content>