<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Join.aspx.cs" MasterPageFile="~/Site.Master"
    Inherits="CCODundeeApplication.Join" Title="Create New Customer" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
      function ValidateFields() {
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            var regPostCode = /\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g;
            var regNumeric = /^[0-9]*$/;
            var regSurName = /^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
            var regMail = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

            var errMsgCardNo ='<%=Resources.CSCGlobal.errMsgCardNo %>'; //"Please enter a valid Card Number"; //errMsgCardNo
            var errMsgName ='<%=Resources.CSCGlobal.errMsgName %>'; //"Please enter a valid Name";          //errMsgName
            var errMsgSurname ='<%=Resources.CSCGlobal.errMsgSurname %>'; //"Surname must be atleast 2 letters long"; //errMsgSurname
            var errMsgEmailAddress ='<%=Resources.CSCGlobal.errMsgEmailAddress %>'; //"Please enter a valid Email Address"; //errMsgEmailAddress
            var errorFlag = "";

            var cardNumber = trim(document.getElementById("<%=txtCardNumber.ClientID%>").value);
            var firstName = trim(document.getElementById("<%=txtFirstName.ClientID%>").value);
            var lastName = trim(document.getElementById("<%=txtLastName.ClientID%>").value);
             var emailAddress = trim(document.getElementById("<%=txtEmailAddress.ClientID%>").value);
            
            if (cardNumber == "" || firstName == "" || lastName == "" || emailAddress == "") {

                document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = '';

                //To clear the error messages if already displayed.
                document.getElementById("spanCardNumber").style.display = 'none';
                document.getElementById("<%=txtCardNumber.ClientID%>").className = '';
                document.getElementById("spanLastName").style.display = 'none';
                document.getElementById("<%=txtLastName.ClientID%>").className = '';
                document.getElementById("spanFirstName").style.display = 'none';
                document.getElementById("<%=txtFirstName.ClientID%>").className = '';
                return false;
            }
            else
            {
                document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = 'none';
            }

            errorFlag = ValidateTextBox("<%=txtCardNumber.ClientID%>", regNumeric, true, false, "spanCardNumber", errMsgCardNo);
            errorFlag = errorFlag + ValidateTextBox("<%=txtLastName.ClientID%>", regSurName, true, false, "spanLastName", errMsgName);
            errorFlag = errorFlag + ValidateTextBox("<%=txtFirstName.ClientID%>", regSurName, true, false, "spanFirstName", errMsgName);
            errorFlag = errorFlag + ValidateTextBox("<%=txtEmailAddress.ClientID%>", regSurName, true, false, "spanEmailAddress",errMsgEmailAddress);

          
            //To check the length of the Cardnumber
            if (trim(document.getElementById("<%=txtCardNumber.ClientID%>").value) != "") {
                var cardNumber = trim(document.getElementById("<%=txtCardNumber.ClientID%>").value);
                if (cardNumber.length < 16 || cardNumber.length > 18) {
                    document.getElementById("spanCardNumber").style.display = '';
                    document.getElementById("spanCardNumber").innerText = errMsgCardNo;
                    document.getElementById("<%=txtCardNumber.ClientID%>").className = 'errorFld';
                    errorFlag = "Error";
                }
            }

            if (errorFlag != "") {
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="Create New Customer"><asp:Localize ID="lclNewCustomer" 
                        runat="server" Text="Create New Customer" 
                        meta:resourcekey="lclNewCustomerResource1"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <%--<span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">Please
                    enter all required fields.</span>--%>
                    <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;"><%=Resources.CSCGlobal.ValidateReqFields%></span>
                <div style="width: 70%">
                    <br />
                    <ul class="customer">
                        <li>
                            <label style="width: 150px;height:30px">
                                <label for="Reward Card Number:">
                            <asp:Localize ID="lclReward" 
                                runat="server" 
                                Text="Reward Card Number:" meta:resourcekey="lclRewardResource1" ></asp:Localize></label><img class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtCardNumber" name="UserName" type="text" 
                                    MaxLength="20" meta:resourcekey="txtCardNumberResource1" />
                                <span class="errorFields" id="spanCardNumber" style="<%=spanCardNumber%>">
                                    <%=errMsgCardNumber%></span>
                            </div>
                        </li>
                        <li>
                            <label style="width: 150px;">
                                <label for="First Name:">
                                <asp:Localize ID="lclFirstName" runat="server" 
                                Text="First Name:" meta:resourcekey="lclFirstNameResource1"></asp:Localize></label><img class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtFirstName" name="FirstName" type="text" 
                                    MaxLength="15" meta:resourcekey="txtFirstNameResource1" />
                                <span class="errorFields" id="spanFirstName" style="<%=spanFirstName%>">
                                    <%=errMsgFirstName%></span>
                            </div>
                        </li>
                        <li>
                            <label style="width: 150px;">
                                <label for="Last Name:"><asp:Localize ID="lclLastName" runat="server" 
                                Text="Last Name:" meta:resourcekey="lclLastNameResource1"></asp:Localize></label><img class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtLastName" name="UserName" type="text" 
                                    MaxLength="25" meta:resourcekey="txtLastNameResource1" />
                                <span class="errorFields" id="spanLastName" style="<%=spanLastName%>">
                                    <%=errMsgLastName%></span>
                            </div>
                        </li>
                        <li>
                            <label style="width: 150px;">
                                <label for="Email Address:"><asp:Localize ID="lclEnail" runat="server" 
                                Text="Email Address:" meta:resourcekey="lclEnailResource1"></asp:Localize></label><img class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtEmailAddress" name="FirstName" type="text" 
                                    meta:resourcekey="txtEmailAddressResource1" />
                                <span class="errorFields" id="spanEmailAddress" style="<%=spanEmailAddress%>">
                                    <%=errMsgEmailAddress%></span>
                            </div>
                        </li>
                    </ul>
                    <ul class="customer">
                        <li>
                            <div class="inputFields" style="padding-left: 250px;">
                                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" BackColor="#49BCD7" ForeColor="White" style="text-align:center;"
                                    Font-Bold="True" Height="24px" Width="83px" OnClick="btnConfirm_Click" 
                                    meta:resourcekey="btnConfirmResource1" /></div>
                </div>
                </li> </ul>
            </div>
        </div>
    </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnNumericeg" Value="" />
    <asp:HiddenField runat="server" ID="hdnname1reg" Value="" />
    <asp:HiddenField runat="server" ID="hdnname3reg" Value="" />
    <asp:HiddenField runat="server" ID="hdnemailreg" Value="" />
    
</asp:Content>
