<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportToExcel.aspx.cs" Inherits="CCODundeeApplication.Reports.ExportToExcel" ContentType="application/vnd.ms-excel" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Xml TransformSource="reportTransformations\textReport.xslt" Runat="server" 
    id="DownLoadXSL" />

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
--%>