<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchCustomer.aspx.cs"
    MasterPageFile="Site.Master" Inherits="CCODundeeApplication.SearchCustomer" Title="Find Customer" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>
    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() {
      
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            var regPostCode = /\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g;
            var regNumeric = /^[0-9]*$/;
             //var regSurName = /^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
            //NGC Changes    
            var regMail =  /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            
            var errMsgCardNo = '<%=Resources.CSCGlobal.errMsgCardNo %>'; //"Please enter a valid Card Number";
            var errMsgName = '<%=Resources.CSCGlobal.errMsgName %>'; //"Please enter a valid Name";
            var errMsgSurname = '<%=Resources.CSCGlobal.errMsgSurname %>'; //"Surname must be atleast 2 letters long";
            var errMsgPostCode = '<%=Resources.CSCGlobal.errMsgPostCode %>'; //"Please enter a valid Postcode";
            var errMsgAge = '<%=Resources.CSCGlobal.errMsgAge %>'; //"Customer must be over 18 to join Clubcard";
            //NGC Changes
            var errMsgEmail = '<%=Resources.CSCGlobal.errMsgEmail %>'; //"Please enter a valid Email Address";
            var errMsgPhone = '<%=Resources.CSCGlobal.errMsgPhone %>'; //"Please enter a valid Phone Number";
            var errBSName="Please Enter Valid Business name";
            var errMsgBusinessRegNumber="Please enter valid Businees Registration Number";
            //NGC Changes
            var errorFlag = "";

            var cardNumber = "";
            if (document.getElementById("<%=txtCardNumber.ClientID%>"))
            {
            cardNumber =trim(document.getElementById("<%=txtCardNumber.ClientID%>").value);
            }
            var firstName = "";
            if (document.getElementById("<%=txtFirstname.ClientID%>")) 
            {
                firstName = trim(document.getElementById("<%=txtFirstname.ClientID%>").value);
            }
            var surName = "";
            if (document.getElementById("<%=txtSurname.ClientID%>")) {
                surName = trim(document.getElementById("<%=txtSurname.ClientID%>").value);
            }
            var postCode = "";
           if( document.getElementById("<%=txtPostCode.ClientID%>"))
            {
              postCode = trim(document.getElementById("<%=txtPostCode.ClientID%>").value);
            }
          var day = "";
          if (document.getElementById("<%=ddlDay.ClientID%>")) 
          {
               day = document.getElementById("<%=ddlDay.ClientID%>").value;
          }
          var month = "";
          if (document.getElementById("<%=ddlMonth.ClientID%>"))
            {
             month = document.getElementById("<%=ddlMonth.ClientID%>").value;
            }
            //var year = document.getElementById("<%=ddlYear.ClientID%>").value;
            //NGC Changes
            var region = "";
            if (document.getElementById("<%=hdnRegion.ClientID%>"))
            {
             region = trim(document.getElementById("<%=hdnRegion.ClientID%>").value);
            }
           var email = "";
           if (document.getElementById("<%=txtEmail.ClientID%>"))
            {
             email = trim(document.getElementById("<%=txtEmail.ClientID%>").value);
         }
         var phoneNumber = "";
         if (document.getElementById("<%=txtPhoneNumber.ClientID%>")) 
         {
              phoneNumber = trim(document.getElementById("<%=txtPhoneNumber.ClientID%>").value);
          }
          var ssnvali = "";
          if (document.getElementById("<%=txtSSN.ClientID%>")) 
          {
              ssnvali = trim(document.getElementById("<%=txtSSN.ClientID%>").value);
          }
          var BusName = "";
          if (document.getElementById("<%=txtBSName.ClientID%>")) 
          {
               BusName = trim(document.getElementById("<%=txtBSName.ClientID%>").value);
           }
           var BusReg = "";
           if (document.getElementById("<%=txtBsRegNumber.ClientID%>")) 
           {
               BusReg = trim(document.getElementById("<%=txtBsRegNumber.ClientID%>").value);
           }
           var EvengNum = "";
           if (document.getElementById("<%=txtEveningNumber.ClientID%>")) 
           {
                EvengNum = trim(document.getElementById("<%=txtEveningNumber.ClientID%>").value);
            }
            var MobileNum = "";
            if (document.getElementById("<%=txtMobileNumber.ClientID%>"))
             {
                 MobileNum = trim(document.getElementById("<%=txtMobileNumber.ClientID%>").value);
            }
            if (cardNumber == "" && firstName == "" && surName == "" && postCode=="" &&
                                day == "" && month == "- Select Month -" && email == "" && phoneNumber == "" && ssnvali == "" && BusName == "" && BusReg == "" && EvengNum == "" && MobileNum=="") {
            //NGC Changes -- && year == "Year"

                document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = '';

                //To clear the error messages if already displayed.
                if (document.getElementById("<%=txtCardNumber.ClientID%>"))
                 {
                    document.getElementById("spanCardNumber").style.display = 'none';
                    document.getElementById("<%=txtCardNumber.ClientID%>").className = '';
                }
                if (document.getElementById("<%=txtSurname.ClientID%>")) {
                    document.getElementById("spanSurname").style.display = 'none';
                    document.getElementById("<%=txtSurname.ClientID%>").className = '';
                }
                if (document.getElementById("<%=txtFirstname.ClientID%>")) {
                    document.getElementById("spanFirstName").style.display = 'none';
                    document.getElementById("<%=txtFirstname.ClientID%>").className = '';
                }
                if (document.getElementById("<%=txtPostCode.ClientID%>"))
                {
                document.getElementById("spanPostcode").style.display = 'none';
                document.getElementById("<%=txtPostCode.ClientID%>").className = '';
            }
            if (document.getElementById("<%=txtSSN.ClientID%>"))
                {
                document.getElementById("<%=txtSSN.ClientID%>").className = '';
            }
                if( document.getElementById("<%=txtBSName.ClientID%>"))
                {
                document.getElementById("<%=txtBSName.ClientID%>").className = '';
                }
                if (document.getElementById("<%=txtBsRegNumber.ClientID%>"))
                {
                 document.getElementById("<%=txtBsRegNumber.ClientID%>").className = '';
                }
                
               
             //NGC Changes    
                document.getElementById("spanEmail").style.display = 'none';
                document.getElementById("<%=txtEmail.ClientID%>").className = '';
                document.getElementById("spanPhoneNo").style.display = 'none';
                document.getElementById("<%=txtPhoneNumber.ClientID%>").className = '';
                //NGC Changes
               
                return false;
            }
            else 
            {
                document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = 'none';
            }

            errorFlag = ValidateTextBox("<%=txtCardNumber.ClientID%>", regNumeric, true, false, "spanCardNumber", errMsgCardNo);
            //errorFlag = errorFlag + ValidateTextBox("<%=txtSurname.ClientID%>", regSurName, true, false, "spanSurname", errMsgName);
            //errorFlag = errorFlag + ValidateTextBox("<%=txtFirstname.ClientID%>", regSurName, true, false, "spanFirstName", errMsgName);
//            if(region == "en-GB")
//            {
//                errorFlag = errorFlag + ValidateTextBox("<%=txtPostCode.ClientID%>", regPostCode, true, false, "spanPostcode", errMsgPostCode);
//            }
//            else
//            {
//                errorFlag = errorFlag + ValidateTextBox("<%=txtPostCode.ClientID%>", regNumeric, true, false, "spanPostcode", errMsgPostCode);
//            }
            //NGC Changes    
            errorFlag = errorFlag + ValidateTextBox("<%=txtEmail.ClientID%>", regMail, true, false, "spanEmail", errMsgEmail);
            errorFlag = errorFlag + ValidateTextBox("<%=txtPhoneNumber.ClientID%>", regNumeric, true, false, "spanPhoneNo", errMsgPhone);
            //NGC Changes    
            errorFlag = errorFlag + ValidateDate1("<%=ddlDay.ClientID%>", "<%=ddlMonth.ClientID%>", "<%=ddlYear.ClientID%>");

            //To check the length of the surname
//            if (trim(document.getElementById("<%=txtSurname.ClientID%>").value) != "") {
//                if (trim(document.getElementById("<%=txtSurname.ClientID%>").value).length < 2) {
//                    document.getElementById("spanSurname").style.display = '';
//                    document.getElementById("spanSurname").innerText = errMsgSurname;
//                    document.getElementById("<%=txtSurname.ClientID%>").className = 'errorFld';
//                    errorFlag = "Error";
//                }
//            }

            //To check the length of the Cardnumber
//            if (trim(document.getElementById("<%=txtCardNumber.ClientID%>").value) != "") {
//                var cardNumber = trim(document.getElementById("<%=txtCardNumber.ClientID%>").value);
//                if (cardNumber.length < 13 || cardNumber.length > 18) {
//                    document.getElementById("spanCardNumber").style.display = '';
//                    document.getElementById("spanCardNumber").innerText = errMsgCardNo;
//                    document.getElementById("<%=txtCardNumber.ClientID%>").className = 'errorFld';
//                    errorFlag = "Error";
//                }
//            }

            //Date validation
            if (ValidateAge("<%=ddlDay.ClientID%>", "<%=ddlMonth.ClientID%>", "<%=ddlYear.ClientID%>") < 18) {
                document.getElementById("spanDOB").innerHTML = errMsgAge;
                errorFlag = "Error";
            }

            if (errorFlag != "") {
                return false;
            }
            else {
                return true;
            }
        }

        function ValidateDate1(dayID, monthID, yearID) {
            var daysInMonth = DaysArray(12);
            var day = document.getElementById(dayID).value;
            var month = document.getElementById(monthID).value;
            var year = document.getElementById(yearID).value;
            var spanErrorID = "";
            var spanClass = "";

            spanErrorID = "spanDOB";
            spanClass = "spanDOBError";

            var invalidDate = false;
            var notEligible = false;

            if ((day != "") && (month == "- Select Month -" || year == "Year")) {
                invalidDate = true;
            }
            else if ((month != "- Select Month -") && (day == "" || year == "Year")) {
                invalidDate = true;
            }
            else if ((year != "Year") && (month == "- Select Month -" || day == "")) {
                invalidDate = true;
            }
            else if ((month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month])
                invalidDate = true;
            else {
                today = new Date();
                age = today.getFullYear() - year;

                var newYear = parseInt(year) + parseInt(age)
                newDate = new Date(newYear, month - 1, day);
                diff = today.getTime() - newDate.getTime();
                if (diff < 0)
                    age = age - 1;

                if (age < 18)
                    notEligible = true;
            }

            if (invalidDate) {
                document.getElementById(spanErrorID).innerText = "Date Of Birth is invalid";
                document.getElementById(spanErrorID).style.display = '';
                document.getElementById(spanClass).className = "errorFld dtFld";

                return "Date Error";
            }
            else if (notEligible) {
                document.getElementById(spanErrorID).innerText = "Please note you must be over 18 to be a member of Clubcard";
                document.getElementById(spanErrorID).style.display = '';
                document.getElementById(spanClass).className = "errorFld dtFld";
                return "Date Error";
            }
            else {
                document.getElementById(spanErrorID).style.display = 'none';
                document.getElementById(spanClass).className = "dtFld";

                return "";
            }
        }

        function PageNoClicked(pageNo) {
            var hdnPageNo = document.getElementById("<%=hdnPageNo.ClientID%>").value;

            if (hdnPageNo == "") {
                hdnPageNo = 0;
            }

            if (pageNo == 'Prev') {
                if (hdnPageNo != 0) {
                    pageNo = parseInt(hdnPageNo) - 1;
                }
            }
            else if (pageNo == 'Next') {
                if (hdnPageNo < 19) {
                    pageNo = parseInt(hdnPageNo) + 1;
                }
            }

            document.getElementById("<%=hdnPageNo.ClientID%>").value = pageNo;
            document.forms[0].submit();
        }

        function ReadCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');

            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);

                if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
            }

            return null;
        }

        var login = ReadCookie('login');
        if (login == null) {
            window.location = "Default.aspx";
        }
    </script>
    
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                
               <asp:Localize ID="lclFindCustomer" runat="server" Text="Find Customer" 
                        meta:resourcekey="lclFindCustomerResource1"></asp:Localize></h3>
            </div>
            <div class="cc_body">
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">
                <asp:Localize ID="lclErrorMessage" runat="server" 
                    Text="Please enter customer details to searchr" 
                    meta:resourcekey="lclErrorMessageResource1"></asp:Localize></span>
                <ul class="customer">
                    <li>
                    <div class="customerSurName">
                        <label for="cardNumber">
                        <asp:Localize ID="lclCardNumber" runat="server" Text="Card Number:" 
                            meta:resourcekey="lclCardNumberResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="250" 
                                meta:resourcekey="txtCardNumberResource1"></asp:TextBox>
                            <span id="spanCardNumber" class="errorFields" style="<%=spanCardNumber%>">
                            <%=errMsgCardNumber%></span>
                        </div>
                        </div>
                    </li>
                    <li>
                    <div class="customerSurName" runat="server" id="divLname" visible="false">
                    <label for="surname" runat="server" id="lblSirname">
                            <asp:Localize ID="lclSurname" runat="server" Text="Surname:" 
                                meta:resourcekey="lclSurnameResource1"></asp:Localize>
                                </label>
                            <div class="inputFields">
                                <asp:TextBox ID="txtSurname" runat="server" 
                                    meta:resourcekey="txtSurnameResource1" />
                                <span id="spanSurname" class="errorFields" style="<%=spanSurname%>">
                                <%=errMsgSurname%></span>
                            </div>
                            </div>
                            <div class="customerFirstName" runat="server" id="divFName" visible="false">
                             <label for="firstName">
                             <asp:Localize ID="lclFirstName" runat="server" Text="First Name:" 
                                meta:resourcekey="lclFirstNameResource1"></asp:Localize>
                                </label>
                            <div class="inputFields">
                                <asp:TextBox ID="txtFirstname" runat="server" MaxLength="20" 
                                    meta:resourcekey="txtFirstnameResource1"></asp:TextBox>
                                <span id="spanFirstName" class="errorFields" style="<%=spanFirstName%>">
                                <%=errMsgFirstName%></span>
                            </div>
                            </div>
                    </li>
                    <li>
                    <div id="divpostcode" runat="server"> 
                    <div class="customerSurName">
                        <label for="postCode">
                         <asp:Localize ID="lclPostCode" runat="server" Text="Postcode:" 
                            meta:resourcekey="lclPostCodeResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtPostCode" runat="server"
                                meta:resourcekey="txtPostCodeResource1"></asp:TextBox>
                            <span id="spanPostcode" class="errorFields" style="<%=spanPostcode%>">
                            <%--<span id="span2" class="errorFields" style="<%=spanEmail%>">
                                <%=errBSName%></span>--%><%=errMsgPostCode%></span>
                        </div>
                        </div>
                        </div>
                        <div class="customerFirstName">
                        <label for="SSN">
                         <asp:Localize ID="lclSSN" runat="server" Text="SSN\NRIC:" meta:resourcekey="lclSSNResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtSSN" runat="server" MaxLength="250"></asp:TextBox>
                        </div>
                        </div>
                    </li>
                    <!-- NGC Changes -->
                    <li>
                    <div class="customerSurName">
                        <label for="Email" >
                         <asp:Localize ID="lclEmail" runat="server" Text="Email:" 
                            meta:resourcekey="lclEmailResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="60" 
                                meta:resourcekey="txtEmailResource1"></asp:TextBox>
                            <span id="spanEmail" class="errorFields" style="<%=spanEmail%>">
                            <%=errMsgPhoneNo%></span>
                        </div>
                        </div>
                        <div class="customerFirstName">
                        <label for="BSName" style="height:30px">
                         <asp:Localize ID="lclBSName" runat="server" Text="Business Name:" meta:resourcekey="lclBSNameResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtBSName" runat="server" MaxLength="250" 
                                ></asp:TextBox>
                            <%=errMsgPhoneNo%>
                        </div>
                        </div>
                    </li>
                    <li>
                    <div class="customerSurName">
                        <label for="Phone Number">
                         <asp:Localize ID="lclPhone" runat="server" Text="Day time Number:" 
                            meta:resourcekey="lclPhoneResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtPhoneNumber" runat="server" 
                                meta:resourcekey="txtPhoneNumberResource1"></asp:TextBox>
                            <span id="spanPhoneNo" class="errorFields" style="<%=spanPhoneNo%>">
                            <%=errMsgDOB%></span>
                        </div>
                        </div>
                        <div class="customerFirstName">
                        <label for="BSRegNum" style="height:30px">
                         <asp:Localize ID="lclBSRegNum" runat="server" Text="Registration No:" meta:resourcekey="lclBSRegNumResource1"></asp:Localize>
                            </label>
                            
                        <div class="inputFields">
                            <asp:TextBox ID="txtBsRegNumber" runat="server" MaxLength="250" ></asp:TextBox>
                        </div>
                        </div>
                        
              </li>
               <li>
                    <div class="customerSurName">
                        <label for="Phone Number">
                         <asp:Localize ID="lclEvengNumber" runat="server" Text="Evening Number:" meta:resourcekey="lclEvengNumberResource1"></asp:Localize>
                            </label>
                        <div class="inputFields">
                            <asp:TextBox ID="txtEveningNumber" runat="server" 
                                meta:resourcekey="txtPhoneNumberResource1"></asp:TextBox>
                            <span id="span1" class="errorFields" style="<%=spanPhoneNo%>">
                            <%--<asp:HyperLinkField HeaderText="Select" ControlStyle-CssClass="GridTd" HeaderStyle-CssClass="GridHeader"
                                                    Text="GO>" DataNavigateUrlFields="CustomerID" DataNavigateUrlFormatString="CustomerDetail.aspx?customerID={0}">
                                                    <ControlStyle CssClass="GridTd"></ControlStyle>
                                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                </asp:HyperLinkField>--%></span>
                        </div>
                        </div>
                        <div class="customerFirstName">
                        <label for="BSRegNum" style="height:30px">
                         <asp:Localize ID="lclMobileNumber" runat="server" Text="Mobile Number:" meta:resourcekey="lclMobileNumberResource1"></asp:Localize>
                            </label>
                            
                        <div class="inputFields">
                            <asp:TextBox ID="txtMobileNumber" runat="server"  ></asp:TextBox>
                        </div>
                        </div>
                        
              </li>
              
                    <!-- NGC Changes -->
                </ul>
                <span class="errorFields"></span><span class="errorFields"></span>
                <ul class="customer">
                    <li>
                        <div class="noteText">
                            <p>
                                <strong>
                                <asp:Localize ID="lclDataProtection" runat="server" 
                                    Text="Data Protection checks only:" 
                                    meta:resourcekey="lclDataProtectionResource1"></asp:Localize></strong></p>
                        </div>
                        <label for="dob">
                         <asp:Localize ID="lclDob" runat="server" Text="Date Of Birth:" 
                            meta:resourcekey="lclDobResource1"></asp:Localize>
                           
                        </label>
                        <div class="inputFields">
                            <span id="spanDOBError" class="dtFld">
                                <asp:DropDownList runat="server" ID="ddlDay" class="day" 
                                meta:resourcekey="ddlDayResource1">
                                    <asp:ListItem Value="" meta:resourcekey="ListItemResource1">Day</asp:ListItem>
                                    <asp:ListItem Value="01" meta:resourcekey="ListItemResource2">1</asp:ListItem>
                                    <asp:ListItem Value="02" meta:resourcekey="ListItemResource3">2</asp:ListItem>
                                    <asp:ListItem Value="03" meta:resourcekey="ListItemResource4">3</asp:ListItem>
                                    <asp:ListItem Value="04" meta:resourcekey="ListItemResource5">4</asp:ListItem>
                                    <asp:ListItem Value="05" meta:resourcekey="ListItemResource6">5</asp:ListItem>
                                    <asp:ListItem Value="06" meta:resourcekey="ListItemResource7">6</asp:ListItem>
                                    <asp:ListItem Value="07" meta:resourcekey="ListItemResource8">7</asp:ListItem>
                                    <asp:ListItem Value="08" meta:resourcekey="ListItemResource9">8</asp:ListItem>
                                    <asp:ListItem Value="09" meta:resourcekey="ListItemResource10">9</asp:ListItem>
                                    <asp:ListItem Value="10" meta:resourcekey="ListItemResource11">10</asp:ListItem>
                                    <asp:ListItem Value="11" meta:resourcekey="ListItemResource12">11</asp:ListItem>
                                    <asp:ListItem Value="12" meta:resourcekey="ListItemResource13">12</asp:ListItem>
                                    <asp:ListItem Value="13" meta:resourcekey="ListItemResource14">13</asp:ListItem>
                                    <asp:ListItem Value="14" meta:resourcekey="ListItemResource15">14</asp:ListItem>
                                    <asp:ListItem Value="15" meta:resourcekey="ListItemResource16">15</asp:ListItem>
                                    <asp:ListItem Value="16" meta:resourcekey="ListItemResource17">16</asp:ListItem>
                                    <asp:ListItem Value="17" meta:resourcekey="ListItemResource18">17</asp:ListItem>
                                    <asp:ListItem Value="18" meta:resourcekey="ListItemResource19">18</asp:ListItem>
                                    <asp:ListItem Value="19" meta:resourcekey="ListItemResource20">19</asp:ListItem>
                                    <asp:ListItem Value="20" meta:resourcekey="ListItemResource21">20</asp:ListItem>
                                    <asp:ListItem Value="21" meta:resourcekey="ListItemResource22">21</asp:ListItem>
                                    <asp:ListItem Value="22" meta:resourcekey="ListItemResource23">22</asp:ListItem>
                                    <asp:ListItem Value="23" meta:resourcekey="ListItemResource24">23</asp:ListItem>
                                    <asp:ListItem Value="24" meta:resourcekey="ListItemResource25">24</asp:ListItem>
                                    <asp:ListItem Value="25" meta:resourcekey="ListItemResource26">25</asp:ListItem>
                                    <asp:ListItem Value="26" meta:resourcekey="ListItemResource27">26</asp:ListItem>
                                    <asp:ListItem Value="27" meta:resourcekey="ListItemResource28">27</asp:ListItem>
                                    <asp:ListItem Value="28" meta:resourcekey="ListItemResource29">28</asp:ListItem>
                                    <asp:ListItem Value="29" meta:resourcekey="ListItemResource30">29</asp:ListItem>
                                    <asp:ListItem Value="30" meta:resourcekey="ListItemResource31">30</asp:ListItem>
                                    <asp:ListItem Value="31" meta:resourcekey="ListItemResource32">31</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlMonth" class="month" 
                                meta:resourcekey="ddlMonthResource1" />
                                <asp:DropDownList runat="server" ID="ddlYear" class="year" 
                                meta:resourcekey="ddlYearResource1" />
                            </span><span id="spanDOB" class="errorFields" style="<%=spanDOB%>">
                            <%--<asp:TemplateField HeaderText="Address" ItemStyle-Wrap="true" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <div style="width:50px;">
                                        <asp:Label ID="ltrAddress" runat="server" Width="50px" Text='<%# Bind("MailingAddressLine1") %>' />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTdCenter" Wrap="true" Width="50px"/>
                                    <FooterStyle CssClass="GridFooterTd" Wrap="true" Width="50px"/>
                                    <HeaderStyle CssClass="GridHeader" Wrap="true" Width="50px"/>
                                </asp:TemplateField>--%></span>
                        </div>
                    </li>
                </ul>
                <p class="findCustomer">
                    <asp:ImageButton ID="btnFindCustomer" runat="server" ImageUrl="I/FindCustomer.gif"
                        CssClass="imgBtn" AlternateText="Confirm" OnClick="FindCustomer" 
                        OnClientClick="return ValidateFields()" 
                        meta:resourcekey="btnFindCustomerResource1" />
                </p>
                <div class="customerSearch" id="dvSearchResults" runat="server" visible="false">
                    <div class="clubcardAcct">
                        <div class="clubcardAct_head">
                            <h4>
                              <asp:Localize ID="lclSearchResults" runat="server" Text="Search Results" 
                                    meta:resourcekey="lclSearchResultsResource1"></asp:Localize>
                                </h4>
                        </div>
                        <asp:GridView CssClass="cardHolderTbl" ID="grdCustomerDetail" runat="server" AutoGenerateColumns="False"
                            AllowPaging="True" PagerSettings-Visible="false" OnRowDataBound="GrdCustomerDetail_RowDataBound"
                            OnRowCommand="GrdCustomerDetail_RowCommand" 
                            AlternatingRowStyle-CssClass="alternate" Width="738px" 
                            meta:resourcekey="grdCustomerDetailResource1" >
                            <PagerSettings Visible="False"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Font-Bold="true" HeaderStyle-BackColor="Red"
								 meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:LinkButton CommandName="Select" CommandArgument='<%# Bind("CustomerID") %>'
                                            ID="lnkSelectCustomer" runat="server">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="I/GotoSearchCustomer.gif" /></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>
                                <%--<asp:HyperLinkField HeaderText="Select" ControlStyle-CssClass="GridTd" HeaderStyle-CssClass="GridHeader"
                                                    Text="GO>" DataNavigateUrlFields="CustomerID" DataNavigateUrlFormatString="CustomerDetail.aspx?customerID={0}">
                                                    <ControlStyle CssClass="GridTd"></ControlStyle>
                                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                                </asp:HyperLinkField>--%>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-Font-Bold="true" 
								meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrName" runat="server" Text='<%# Bind("Name1") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>
                                 <%-- <asp:TemplateField ItemStyle-Width="50px" HeaderText="Address" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <div style="width:100px;">
                                        <asp:Literal ID="ltrAddress" runat="server" Text='<%# Bind("MailingAddressLine1") %>' />
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="true" CssClass="GridHeader" />
                                    <ItemStyle Wrap="true" CssClass="GridTdCenter" />
                                    <FooterStyle Wrap="true" CssClass="GridFooterTdLeft" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Address" ItemStyle-Wrap="true" HeaderStyle-Font-Bold="true"
								 meta:resourcekey="TemplateFieldResource3">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrAddress" runat="server"  Text='<%# Bind("MailingAddressLine1") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTdCenter" Wrap="true"/>
                                    <FooterStyle CssClass="GridFooterTd" Wrap="true"/>
                                    <HeaderStyle CssClass="GridHeader" Wrap="true"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="D.O.B" HeaderStyle-Font-Bold="true"
								meta:resourcekey="TemplateFieldResource4">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrDOB" runat="server" Text='<%# Bind("DateOfBirth") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Points" HeaderStyle-Font-Bold="true"
								meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrCurrentPts" runat="server" Text='<%# Bind("CurrentPointsBalanceQty") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Previous Points" HeaderStyle-Font-Bold="true"
								meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrPrevpts" runat="server" Text='<%# Bind("PreviousPointsBalanceQty") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Join Date" HeaderStyle-Font-Bold="true"
								meta:resourcekey="TemplateFieldResource7">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrJoinDate" runat="server" Text='<%# Bind("JoinedDate") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Household ID" HeaderStyle-Font-Bold="true"
								meta:resourcekey="TemplateFieldResource8">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrHousehold" runat="server" Text='<%# Bind("HouseHoldID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Status" HeaderStyle-Font-Bold="true"
								meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltrCustStatus" runat="server" Text='<%# Bind("CustomerUseStatusID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="GridTd" />
                                    <FooterStyle CssClass="GridFooterTd" />
                                    <HeaderStyle CssClass="GridHeader" />
                                </asp:TemplateField>
                            </Columns>
                            <%--<PagerStyle CssClass="pagination" />
                            <PagerSettings Mode="NumericFirstLast" PreviousPageImageUrl="I/previousDisabled.gif"
                                NextPageImageUrl="I/next.gif" />--%><AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </div>
                <div class="pagination" id="dvPaging" visible="false" runat="server">
                    <ul>
                        <li>  <asp:Localize ID="lclPage" runat="server" Text="Page" 
                                meta:resourcekey="lclPageResource1"></asp:Localize></li>
                        <li>
                            <asp:PlaceHolder ID="pnlPageNos" runat="server"></asp:PlaceHolder>
                        </li>
                            <%--<PagerStyle CssClass="pagination" />
                            <PagerSettings Mode="NumericFirstLast" PreviousPageImageUrl="I/previousDisabled.gif"
                                NextPageImageUrl="I/next.gif" />--%>
                        <li>
                            <img id="ImgPrev1" runat="server" src="I/previous.gif" alt="previous" style="cursor: hand"
                                class="imgBtn" onclick="return PageNoClicked('Prev');" visible="false" /></li>
                        <li>
                            <img id="ImgPrevDisabled1" runat="server" src="I/previousDisabled.gif" style="cursor: default"
                                alt="previous" class="imgBtn" /></li>
                        <li>
                            <img id="ImgNext1" src="I/next.gif" alt="next" runat="server" style="cursor: hand"
                                onclick="return PageNoClicked('Next');" /></li>
                        <li class="nextPage">
                            <img id="ImgNextDisabled1" src="I/nextDisabled.gif" alt="next" runat="server" visible="false" /></li>
                    </ul>
                </div>
                <div class="errorFields" runat="server" id="dvNoDataFound" visible="false">
                    <p style="text-align: center">
                        <strong>
                        <asp:Localize ID="lclNoDataFound" runat="server" 
                            Text="No account(s) were found matching the requested search." 
                            meta:resourcekey="lclNoDataFoundResource1"></asp:Localize></strong></p>
                </div>
                <asp:HiddenField ID="hdnPageNo" runat="server" />
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdnRegion" Value="en-GB" />
         <asp:HiddenField runat="server" ID="hdnPostCodeFormat"/>
          <asp:HiddenField runat="server" ID="hdnPostCodeFormat1"  />
            <asp:HiddenField runat="server" ID="HidePostCode"  />
            <asp:HiddenField runat="server" ID="hdnname2validation" />
            <asp:HiddenField runat="server" ID="HiddenFName" value="true" />
             <asp:HiddenField runat="server" ID="HiddenLName" value="true" />
             <asp:HiddenField ID="hdnName3" runat="server" Value="true" />
             <asp:HiddenField ID="hdnClubcardnumberreg" runat="server" Value="" />
             <asp:HiddenField ID="hdnname1reg" runat="server" Value="" />
             <asp:HiddenField ID="hdnemailreg" runat="server" Value="" />
             
             
    </div>
    
    
</asp:Content>
