<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExceptionExamples.aspx.cs"
    Inherits="webApp.Forms.ExceptionExamples" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>Difference between "throw" and "throw ex" in .NET</b>
        <br />
        <b>Answer</b> If you use "throw" statement, it preserve original error stack information.
        <br />
        If you use "throw ex" statement, stack trace of the exception will be replaced with
        a stack trace starting at the re-throw point.
        <br />
        So it is very important to just use the throw statement, rather than throw ex because
        it will give you more accurate error stack information.
        <br />
        <asp:Button ID="btnGetErrorTraceWithThrow" runat="server" Text="Click to get error trace with Throw"
            OnClick="btnGetErrorTraceWithThrow_Click" />
        <asp:Button ID="btnGetErrorTraceWithThrowEX" runat="server" Text="Click to get error trace with  Throw ex "
            OnClick="btnGetErrorTraceWithThrowEX_Click" />
        <br />
        <asp:Literal ID="litResult" runat="server"></asp:Literal>
    </div>
    <br />
    <br />
    <br />
    <b>Example on the sequence of the execution of catch block if you have multiple catch
        blocks with the try section.</b>
    <br />
    <br />
    <b>Ans) Multiple catch blocks are evaluated from top to bottom, but only one catch block
        is executed for each exception thrown. The order of your catch blocks is important
        because .NET will go to the first catch block that is polymorphically compatible
        with the thrown exception. It is important to place catch blocks with the most specific
        — most derived — exception classes first. In your example, the 1st catch is type
        of Exception of which is base class for all exceptions and thereby polymorphically
        compatible with all thrown exceptions. so 2nd catch never can get execute. And regarding
        finally block, it always executes after the try and catch blocks execute. A finally
        block is always executed, regardless of whether an exception is thrown or whether
        a catch block matching the exception type is found. </b>
    </form>
</body>
</html>
