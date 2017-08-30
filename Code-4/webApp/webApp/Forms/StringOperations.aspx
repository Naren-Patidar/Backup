<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StringOperations.aspx.cs"
    Inherits="webApp.Forms.StringOperations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            Write code to print the words in reverse in given string without using any inbuilt
            functions. Not the entire string, only words in the given string should reverse.
        </div>
        <div>
            Enter String:
            <asp:TextBox ID="txtInput" runat="server">
    
            </asp:TextBox>
            <asp:Button Text="btnReverse string words" runat="server" ID="btnReverse" OnClick="btnReverse_Click" />
        </div>
        <div>
            Output:
            <asp:Literal runat="server" ID="litOutput"></asp:Literal>
        </div>
    </div>
    <asp:Button ID="btnReversestring" runat="server" Text="Reverse the string" OnClick="btnReversestring_Click" />
    <div>
        Enter Number :
        <asp:TextBox ID="txtEnterNumber" runat="server"></asp:TextBox>
        <asp:Button ID="btnAddDigits" runat="server" Text="Add digits" 
            onclick="btnAddDigits_Click" />
             <asp:Button ID="btnFIndFactorials" runat="server" Text="Find factorials" onclick="btnFIndFactorials_Click" 
             />
            <asp:Literal id="litResultSumofNumbers" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
