<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Prime.aspx.cs" Inherits="webApp.Forms.Prime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox> 
    <asp:Button ID="btnFInd" runat="server" Text="Check for prime number" 
            onclick="btnFInd_Click" />     
    </div>
    <div>
    Click to generate fibonacci series <asp:Button runat="server" Text="CLick me" 
            onclick="Unnamed1_Click" />    
    </div>
    <br />
    <br />

     <div>
     <table>
     <tr>
     <td>
     Enter list item
     <asp:TextBox ID="txtListItem" runat="server"></asp:TextBox>  
     </td>
     </tr>
     <tr>
     <td>
     <asp:Button ID="btnAdd" runat ="server" Text="Add and show items" onclick="btnAdd_Click" />  
     </td>
     </tr>
     <tr>
     <td>
     <asp:Literal ID="litResult" runat="server"></asp:Literal>  
     </td>
     </tr>
     </table> 
    
    </div>
    </form>
</body>
</html>
