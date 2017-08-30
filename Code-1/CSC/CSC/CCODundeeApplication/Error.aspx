<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="CCODundeeApplication.Error" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="Server">
    <div id="mainContent">
        <p class="pageDesc">
            <asp:Label ID="lblErrorRefNumber" 
                Text="Sorry, we are currently experiencing problems with our system, please try again later." 
                runat="server" ForeColor="Red" meta:resourcekey="lblErrorRefNumberResource1" />           
        </p>
    </div>
</asp:Content>