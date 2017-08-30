<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JqueryEx1.aspx.cs" Inherits="webApp.JQuery.JqueryEx1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript"  src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script language="javascript" type="text/javascript" >
        $(document).ready(function () { 

        $("p").on({
            mouseenter: function () {
                $(this).css("background-color", "lightgray");
            },
            mouseleave: function () {
                $(this).css("background-color", "lightblue");
            },
            click: function () {
                $(this).css("background-color", "yellow");
            },
            dblclick: function () {
                $(this).css("background-color", "green");
            }
        });
    });

    $(document).ready(function () {
        $("button").click(function () {
            $("#div1").fadeOut();
            $("#div2").fadeOut("slow");
            $("#div3").fadeOut(3000);
            return false;
        });
    });
    </script>  
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>1st thecxzcs
    <br />
    fdfdgdgdf
    <br />
    
    dfdfgdfg
    
    <br />
    
    dfgdfgdfgdf</p>
   
    </div>

    <button>Click to fade out boxes</button><br><br>

<div id="div1" style="width:80px;height:80px;background-color:red;"></div><br />
<div id="div2" style="width:80px;height:80px;background-color:green;"></div><br />
<div id="div3" style="width:80px;height:80px;background-color:blue;"></div>

    </form>
</body>
</html>
