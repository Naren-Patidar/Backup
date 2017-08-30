<%@ Page Language="C#" EnableSessionState="True" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CCODundeeApplication._Default" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <link rel="Stylesheet" type="text/css" href="CSS/core.css" />
    <link href="CSS/ie6.css" rel="stylesheet" type="text/css" />

    <script src="JS/General.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() {

            var errMsgUserID = '<%=Resources.CSCGlobal.ValidUsername %>'; // "Please enter a user ID";//ValidUsername
            var errMsgPassword = '<%=Resources.CSCGlobal.ValidPwd %>'; // "Please enter a password";//ValidPwd
            var errMsgDomain = '<%=Resources.CSCGlobal.ValidDomain %>'; // "Please select the domain";//ValidDomain
            var errorFlag = "";

            if (trim(document.getElementById("<%=txtUserID.ClientID%>").value) == "") {
                document.getElementById("<%=spanUserID.ClientID%>").style.display = '';
                document.getElementById("<%=spanUserID.ClientID%>").innerText = errMsgUserID;
                document.getElementById("<%=txtUserID.ClientID%>").className = 'errorFld';
                errorFlag = "Error";
            }
            else
            {
                document.getElementById("<%=spanUserID.ClientID%>").style.display = 'none';
                document.getElementById("<%=txtUserID.ClientID%>").className = '';
            }

            if (trim(document.getElementById("<%=txtPassword.ClientID%>").value) == "") {
                document.getElementById("<%=spanPassword.ClientID%>").style.display = '';
                document.getElementById("<%=spanPassword.ClientID%>").innerText = errMsgPassword;
                document.getElementById("<%=txtPassword.ClientID%>").className = 'errorFld';
                errorFlag = "Error";
            }
            else
            {
                 document.getElementById("<%=spanPassword.ClientID%>").style.display = 'none';
                  document.getElementById("<%=txtPassword.ClientID%>").className = '';
            }

            if (errorFlag != "") {
                return false;
            }
            else {
                CreateCookie("login", "Yes", 0);
                return true;
            }
        }

        function CreateCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }
        function DeleteAllCookies() {
            var cookies = document.cookie.split(";");
            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i];
                var eqPos = cookie.indexOf("=");
                var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
                document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
            }
        }
        DeleteAllCookies();

    </script>

</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnConfirmPersonalDtls">
    <div id="topSection" style="position: absolute; left: 255px; top: 170px;">
        <div style="width:83%">
            <p>
                <a href="<%$ Resources:CSCGlobal, tescoLogoURL %>" runat="server" >
                    <img src="I/tescoLogo.gif" alt="Tesco.com" /></a></p>
        </div>
        <div style="width:12%;margin-left:10px">
                <asp:Image ID="ImgLoginFlag" runat="server" Visible="false" 
                               Height="35px"/>
        </div>
    </div>

    <div style="position: absolute; left: 240px; top: 210px;">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                <label id="Login">
                    <asp:Localize ID="Localize1" runat="server" Text="Login" 
                            meta:resourcekey="lclLoginResource1"/></label></h3>
            </div>
            <div class="cc_body">
                <ul class="customer">
                    <li>
                        <label for="cardNumber" id="UserId">
                        <asp:Localize ID="lclUserID" runat="server" Text="User ID:" 
                            meta:resourcekey="lclUserIDResource1" /></label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtUserID" runat="server" MaxLength="50" TabIndex="1"
                                AutoCompleteType="Disabled" meta:resourcekey="txtUserIDResource1"></asp:TextBox>
                            <asp:ImageButton ID="imgbtnLocalFlag" runat="server" ImageUrl="~/I/flaguk.gif" 
                                onclick="imgbtnLocalFlag_Click" Width="90px" Height="45px" 
                                meta:resourcekey="imgbtnLocalFlagResource1" Visible="true"/>
                            <span id="spanUserID" class="errorFields" runat="server" style="<%=spanStyleUserID%>">
                            </span>
                        </div>
                    
                        
                    </li>
                    <li>
                        <label for="password" id="Pwd">
                        <asp:Localize ID="lclPassword" runat="server" Text="Password:" 
                            meta:resourcekey="lclPasswordResource1" />
                            </label>
                        <div class="inputFields">
                            <asp:TextBox TextMode="Password" ID="txtPassword" MaxLength="50" runat="server" TabIndex="2"
                                AutoCompleteType="Disabled" meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                             <asp:ImageButton ID="imgbtnFlag" runat="server" ImageUrl="<%$ Resources:CSCGlobal, tescoLogoURL %>" 
                                Width="90px" Height="45px" meta:resourcekey="imgbtnFlagResource1" Visible="true"
                                onclick="imgbtnFlag_Click" />
                            <span id="spanPassword" class="errorFields" runat="server" style="<%=spanStylePassword%>">
                            </span>
                        </div>
                    </li>
                    <li>
                        <label for="domain" id="domain">
                         <asp:Localize ID="lclDomain" runat="server" Text="Domain:" 
                            meta:resourcekey="lclDomainResource1" />
                            </label>
                        <div class="inputFields">
                            <asp:DropDownList ID="ddlDomain" runat="server" TabIndex="3" 
                                meta:resourcekey="ddlDomainResource1">
                                <%--<asp:ListItem Text="IN" Value="in" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                <asp:ListItem Text="DEV01" Value="dev01" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                <asp:ListItem Text="UKROI" Value="ukroi" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                <asp:ListItem Text="TSL" Value="tsl" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                <asp:ListItem Text="US" Value="us" meta:resourcekey="ListItemResource5"></asp:ListItem>--%>
                            </asp:DropDownList>
                            <span id="spanDomain" class="errorFields" runat="server"></span>
                        </div>
                    </li>
                </ul>
                <p class="pageAction">
                    <asp:ImageButton ID="btnConfirmPersonalDtls" runat="server" ImageUrl="<%$ Resources:CSCGlobal, ConfirmPersonalDtlsImgURL %>" TabIndex="4" 
                        CssClass="imgBtn" AlternateText="Login" OnClick="Login" 
                        OnClientClick="return ValidateFields()" 
                        meta:resourcekey="btnConfirmPersonalDtlsResource1" />
                </p>
                <p>
                    <label for="errorPwd" id="lblErrorMsg" runat="server" style="width:70%" class="errorFields" visible="false">
                         <asp:Localize ID="lclErrorMsg" runat="server" Text="User ID/Password is incorrect" meta:resourcekey="lclErrorMsg" />
                        </label>
                </p>
                <p>
                    <label id="lblAuthorization" runat="server" style="width:70%" class="errorFields"  visible="false">
                        <asp:Localize ID="lclAuthorization" runat="server" Text="You are not a authorized user" meta:resourcekey="lclAuthorization" /></label>
                </p>
                <p>
                    <label id="lblAuthenticationError" runat="server" style="width:70%" class="errorFields" visible="false">
                    <asp:Localize ID="lclAuthenticationError" runat="server" Text="User ID/Password/Domain is incorrect or service unavailable" meta:resourcekey="lclAuthenticationError" />
                       <%-- <asp:Localize ID="lclAuthenticationError" runat="server" Text="User ID/Password/Domain is incorrect or service unavailable" meta:resourcekey="lclAuthenticationError" />--%>
                    </label>
                </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
