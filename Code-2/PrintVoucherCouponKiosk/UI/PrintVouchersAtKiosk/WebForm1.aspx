<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="PrintVouchersAtKiosk.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
    <div >
    <asp:DropDownList ID="Mes" runat="server" style="width:43px"  AutoPostBack="true"  
            onselectedindexchanged="Mes_SelectedIndexChanged">
            <asp:ListItem Value="0" Selected="True">select</asp:ListItem>
        <asp:ListItem Value="1">Jan</asp:ListItem>
        <asp:ListItem Value="2">Feb</asp:ListItem>
        <asp:ListItem Value="3">March</asp:ListItem>
        <asp:ListItem Value="4">April</asp:ListItem>
        <asp:ListItem Value="5">May</asp:ListItem>
        <asp:ListItem Value="6">June</asp:ListItem>
        <asp:ListItem Value="7">July</asp:ListItem>
        <asp:ListItem Value="8">Aug</asp:ListItem>
        <asp:ListItem Value="9">Sept</asp:ListItem>
        <asp:ListItem Value="10">Oct</asp:ListItem>
        <asp:ListItem Value="11">Nov</asp:ListItem>
        <asp:ListItem Value="12">Dec</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="Ano" runat="server" style="width:63px"  AutoPostBack="true" 
            onselectedindexchanged="Ano_SelectedIndexChanged">
            <asp:ListItem Value="0" Selected="True">select</asp:ListItem>
        <asp:ListItem Value="2010">2010</asp:ListItem>
        <asp:ListItem Value="2011">2011</asp:ListItem>
        <asp:ListItem Value="2012">2012</asp:ListItem>
        <asp:ListItem Value="2013">2013</asp:ListItem>
    </asp:DropDownList>
    </div>

    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <asp:CheckBox ID="CheckBox1" runat="server"  AutoPostBack="true"  OnCheckedChanged="CheckBox1_CheckedChanged"/>
        <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" 
            oncheckedchanged="CheckBox2_CheckedChanged" />
    </asp:PlaceHolder>
    </form>
</body>
</html>
