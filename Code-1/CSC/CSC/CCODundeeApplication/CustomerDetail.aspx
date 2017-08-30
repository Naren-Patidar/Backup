<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDetail.aspx.cs"
    Inherits="CCODundeeApplication.CustomerDetail" MasterPageFile="~/Site.Master"
    Title="Customer details" Culture="auto" meta:resourcekey="PageResource2" UICulture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/CustomerDetails.js" type="text/javascript"></script>
    <script src="JS/UnlockAccount.js" type="text/javascript"></script>
    
     <script language="javascript" type="text/javascript">
         //GroupCR CR13
         function setStatusDropdownValue() {
             document.getElementById('ctl00_PageContainer_hdnConfirmMsg').value = 'true';
         }

         function clearOnConfirm() {
             var s = document.getElementById('ctl00_PageContainer_hdnConfirmMsg').value
             
             if (s == 'true') {
                 var msg = '<%=Resources.CSCGlobal.CustomerDetailConfirmMsg %>'
                 if (confirm(msg)) {
                     return true;
                 }
                 else {
                     if (document.getElementById('ctl00_PageContainer_lblSuccessMessage')) {
                         document.getElementById('ctl00_PageContainer_lblSuccessMessage').outerText = '';
                     }
                     return false;
                 }
             }
             else {

                 return true;
             }
         }
     </script>

    <div id="mainContent" runat="server">
        <div id="accountUnlock" runat="server" class="accountUnlock" style="display: none;">
            <div class="statusUnlock">
                <asp:Localize ID="lblalheader" runat="server" Text="Account Locked" meta:resourcekey="lclAccountLocked"></asp:Localize>
                <span>|</span>
                <asp:Localize ID="lblattempts" runat="server" Text="Total Attempts : " meta:resourcekey="lclAttempts"></asp:Localize>
                <span id='attemptsVal'></span> 
                <span>|</span>
            </div>
            <div class="btnUnlock" onclick="return unlockAccount()">
                <asp:Localize ID="lblbtnunlock" runat="server" Text="Unlock" meta:resourcekey="lclUnlock"></asp:Localize>
            </div>
            <div class="UnlockError" style="display:none">
                <asp:Localize ID="lblError" runat="server" Text="Unlock Error" meta:resourcekey="lclUnlockError"></asp:Localize>
            </div>
        </div>
        <div class="saveBtn">
            <asp:ImageButton ID="btnConfirmCustomerDtls" runat="server" ImageUrl="I/SaveChanges.gif"
                CssClass="saveBtn" AlternateText="Confirm" OnClick="btnConfirmCustomerDtls_Click" onclientclick="return clearOnConfirm();"
                meta:resourcekey="btnConfirmCustomerDtlsResource1" />
        </div>
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="Customerdetails">
                        <asp:Localize ID="lclCustomerDetails" runat="server" Text="Customer details" meta:resourcekey="lclCustomerDetailsResource1"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgsCustomerDetail" meta:resourcekey="lblSuccessMessageResource2"></asp:Label>
                <div class="mainCustomer">
                    <h3 style="height: 30px">
                        <label for="MainCustomer" style="width: 75%">
                            <asp:Localize ID="lclMainCustomer" runat="server" Text="Main Customer" meta:resourcekey="lclMainCustomerResource1"></asp:Localize></label></h3>
                    <ul class="customer">
                        <li runat="server" id="titlemain" style="display: block;" visible="false">
                            <label for="title">
                                <asp:Localize ID="lclTitle" runat="server" Text="Title:" meta:resourcekey="lclTitleResource1"></asp:Localize><img
                                    class="required" runat="server" visible="false" id="imgTilte" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <span class="SelectBorder">
                                    <asp:DropDownList runat="server" ID="ddlTitle0" meta:resourcekey="ddlTitle0Resource1">
                                    </asp:DropDownList>
                                </span><span id="spanTitle0" class="errorFields" style="display: none">
                                    <%=Resources.CSCGlobal.validTitleMsg %></span>
                            </div>
                        </li>
                        <li runat="server" id="fnamelist" runat="server" visible="false">
                            <label for="firstName">
                                <asp:Localize ID="lclFirstName" runat="server" Text="First Name:" meta:resourcekey="lclFirstNameResource1"></asp:Localize><img
                                    class="required" id="imgmandFN" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtFirstName0" name="firstName" type="text" MaxLength="20"
                                    meta:resourcekey="txtFirstName0Resource1" />
                                <span class="errorFields" id="spanFirstName0" style="<%=spanStyleFirstName0%>">
                                    <%=errMsgFirstName%></span>
                            </div>
                        </li>
                        <li runat="server" id="middlenamemain" style="display: block;" visible="false">
                            <label for="initial">
                                <asp:Localize ID="lclinitials" runat="server" Text="Middle Initial(s):" meta:resourcekey="lclinitialsResource1"></asp:Localize><img
                                    class="required" id="imgmandMN" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtInitial0" name="initial" type="text" 
                                    meta:resourcekey="txtInitial0Resource1" />
                                <span class="errorFields" id="spanMiddleName0" style="<%=spanStyleMiddleName0%>">
                                    <%=errMsgMiddleName%></span>
                            </div>
                        </li>
                        <li runat="server" id="liSurnameMain" visible="false">
                            <label for="surname" id="lblSurname" runat="server" visible="true">
                                <asp:Localize ID="lclSurname" runat="server" Text="Surname:" meta:resourcekey="lclSurnameResource1"></asp:Localize><img
                                    class="required" id="imgmandLN" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <label for="surname" id="lblLastName" runat="server" visible="false">
                                <asp:Localize ID="lcllastname" runat="server" Text="Last Name:" meta:resourcekey="lcllastnameResource1"></asp:Localize><img
                                    class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtSurname0" name="surname" type="text" 
                                    meta:resourcekey="txtSurname0Resource1" />
                                <span class="errorFields" id="spanSurname0" style="<%=spanStyleSurname0%>">
                                    <%=errMsgSurname%></span>
                            </div>
                        </li>
                        <li>
                            <label for="initial">
                                <asp:Localize ID="lclEmaiAdd" runat="server" Text="Email address:" meta:resourcekey="lclsemailResource1"></asp:Localize><img
                                    class="required" runat="server" visible="false" id="imgmandEmail" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtEmailAddress" name="contact" type="text" MaxLength="60"
                                    meta:resourcekey="txtEmailAddressResource1" />
                                <span class="errorFields" id="spanEmailAddress" style="<%=spanStyleEmailAddress%>">
                                    <%=errMsgEmailAddress%></span>
                            </div>
                        </li>
                        <li>
                            <label for="initial" style="height:30px">
                                <asp:Localize ID="lclContactMob" runat="server" Text="Mobile Number:" meta:resourcekey="lclContactMobResource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgMandMobile" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtMobileNumber" name="contact" type="text" meta:resourcekey="txtMobileNumberResource1" />
                                <span class="errorFields" id="spanMoblieNumber" style="<%=spanStyleMoblieNumber%>">
                                    <%=errMsgMobileNumber%></span>
                                <asp:HiddenField ID="HiddenField1" runat="server" OnValueChanged="HiddenField1_ValueChanged" />
                            </div>
                        </li>
                        <li>
                            <label for="initial" style="height:30px">
                                <asp:Localize ID="lclPreContact" runat="server" Text="Daytime Number" meta:resourcekey="lclcontactnoResource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgmandPhoneNo" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtPhoneNumber" name="contact" type="text" meta:resourcekey="txtPhoneNumberResource2" />
                                <span class="errorFields" id="spanPhoneNumber" style="<%=spanStylePhoneNumber%>">
                                    <%=errMsgPhoneNumber%></span>
                            </div>
                        </li>
                        <li runat="server" id="EveningNumberMain" style="display: block;" visible="false">
                            <label for="initial" style="height:30px">
                                <asp:Localize ID="lclEveningPhoneNumber" runat="server" Text="Evening Number:" meta:resourcekey="lclEveningPhoneNumberResource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgEveningPhoneNumber" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtEveningPhoneNumber" name="contact" type="text"
                                    meta:resourcekey="txtEveningPhoneNumberResource1" />
                                <span class="errorFields" id="spanEveningPhoneNumber" style="<%=spanStyleEveningPhoneNumber%>">
                                    <%=errMsgEveningPhoneNumber%></span>
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                            </div>
                        </li>
                        <!-- NGC Changes -->
                        <li runat="server" id="liemailUS" style="display: none">
                            <label for="Email">
                                <asp:Localize ID="lclEmail" runat="server" Text="Email:" meta:resourcekey="lclEmailResource1"></asp:Localize><img
                                    class="required" runat="server" visible="false" id="imgEmail" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox ID="txtEmail" runat="server" name="Email" MaxLength="250" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                                <span id="spanEmail" class="errorFields" style="<%=spanEmail%>">
                                    <%=errMsgEmail%></span>
                            </div>
                        </li>
                        <li runat="server" id="liPrimaryID" style="display: none">
                            <label for="primaryid" style="height: 40px;">
                                <asp:Localize ID="lclPrimaryID" runat="server" Text="Primary ID(Social Security No):"
                                    meta:resourcekey="lclPrimaryIDResource1"></asp:Localize>
                                <img class="required" visible="false" id="imgmandPrimId" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtPrimId" name="txtPrimId" type="text"  meta:resourcekey="txtPrimIdResource1" />
                                <span class="errorFields" id="span14" style="<%=spanStylePrimaryId%>">
                                    <%=errMsgPrimaryId%></span>
                            </div>
                        </li>
                        <li runat="server" id="liSecondaryID" style="display: none">
                            <label for="secondaryid" style="height: 40px;">
                                <asp:Localize ID="lclSecondaryID" runat="server" Text="Secondary ID(passport,officer number):"
                                    meta:resourcekey="lclSecondaryIDResource1"></asp:Localize>
                                <img class="required" visible="false" id="imgmandSecId" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtSecId" name="txtSecId" type="text" meta:resourcekey="txtSecIdResource1" />
                                <span class="errorFields" id="span15" style="<%=spanStyleSecondaryId%>">
                                    <%=errMsgSecondaryId%></span>
                            </div>
                        </li>
                        <li runat="server" id="liLanguage" style="display: none">
                            <label>
                                <asp:Localize ID="lclLanguage" runat="server" Text="Preferred Language:" meta:resourcekey="lclLanguageResource1"></asp:Localize>
                                <img class="required" id="imgmandLanguage" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" />
                            </label>
                            <div class="inputFields">
                                <asp:DropDownList ID="rdoLanguage" runat="server" 
                                    meta:resourcekey="rdoLanguageResource1">
                                </asp:DropDownList>
                                <span id="span17" class="errorFields" style="<%=spanStyleLanguage%>">
                                    <%=errMsgLanguage%></span>
                            </div>
                        </li>
                        <li runat="server" id="liRace" style="display: none">
                            <label for="race">
                                <asp:Localize ID="lclRace" runat="server" Text="Please select race:" meta:resourcekey="lclRaceResource1"></asp:Localize>
                                <img class="required" visible="false" id="imgmandRace" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:DropDownList runat="server" ID="ddlRace" class="year" CssClass="year" 
                                    meta:resourcekey="ddlRaceResource1" />
                                <span class="errorFields" id="span13" style="<%=spanStyleRace%>">
                                    <%=errMsgRace%></span>
                            </div>
                        </li>
                        <!-- NGC Changes -->
                        <li>
                            <label for="dob">
                                <asp:Localize ID="lclDOB" runat="server" Text="Date Of Birth:" meta:resourcekey="lclDOBResource1"></asp:Localize><img
                                    class="required" id="imgmandDOB" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" />
                            </label>
                            <div class="inputFields">
                                <span id="spanDOBError0" class="<%=spanClassDOBDropDown0%>">
                                    <asp:DropDownList runat="server" ID="ddlDay0" class="day" meta:resourcekey="ddlDay0Resource1">
                                        <asp:ListItem Value="" meta:resourcekey="ListItemResource1" Text="Day"></asp:ListItem>
                                        <asp:ListItem Value="01" meta:resourcekey="ListItemResource2" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="02" meta:resourcekey="ListItemResource3" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="03" meta:resourcekey="ListItemResource4" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="04" meta:resourcekey="ListItemResource5" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="05" meta:resourcekey="ListItemResource6" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="06" meta:resourcekey="ListItemResource7" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="07" meta:resourcekey="ListItemResource8" Text="7"></asp:ListItem>
                                        <asp:ListItem Value="08" meta:resourcekey="ListItemResource9" Text="8"></asp:ListItem>
                                        <asp:ListItem Value="09" meta:resourcekey="ListItemResource10" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="10" meta:resourcekey="ListItemResource11" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" meta:resourcekey="ListItemResource12" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" meta:resourcekey="ListItemResource13" Text="12"></asp:ListItem>
                                        <asp:ListItem Value="13" meta:resourcekey="ListItemResource14" Text="13"></asp:ListItem>
                                        <asp:ListItem Value="14" meta:resourcekey="ListItemResource15" Text="14"></asp:ListItem>
                                        <asp:ListItem Value="15" meta:resourcekey="ListItemResource16" Text="15"></asp:ListItem>
                                        <asp:ListItem Value="16" meta:resourcekey="ListItemResource17" Text="16"></asp:ListItem>
                                        <asp:ListItem Value="17" meta:resourcekey="ListItemResource18" Text="17"></asp:ListItem>
                                        <asp:ListItem Value="18" meta:resourcekey="ListItemResource19" Text="18"></asp:ListItem>
                                        <asp:ListItem Value="19" meta:resourcekey="ListItemResource20" Text="19"></asp:ListItem>
                                        <asp:ListItem Value="20" meta:resourcekey="ListItemResource21" Text="20"></asp:ListItem>
                                        <asp:ListItem Value="21" meta:resourcekey="ListItemResource22" Text="21"></asp:ListItem>
                                        <asp:ListItem Value="22" meta:resourcekey="ListItemResource23" Text="22"></asp:ListItem>
                                        <asp:ListItem Value="23" meta:resourcekey="ListItemResource24" Text="23"></asp:ListItem>
                                        <asp:ListItem Value="24" meta:resourcekey="ListItemResource25" Text="24"></asp:ListItem>
                                        <asp:ListItem Value="25" meta:resourcekey="ListItemResource26" Text="25"></asp:ListItem>
                                        <asp:ListItem Value="26" meta:resourcekey="ListItemResource27" Text="26"></asp:ListItem>
                                        <asp:ListItem Value="27" meta:resourcekey="ListItemResource28" Text="27"></asp:ListItem>
                                        <asp:ListItem Value="28" meta:resourcekey="ListItemResource29" Text="28"></asp:ListItem>
                                        <asp:ListItem Value="29" meta:resourcekey="ListItemResource30" Text="29"></asp:ListItem>
                                        <asp:ListItem Value="30" meta:resourcekey="ListItemResource31" Text="30"></asp:ListItem>
                                        <asp:ListItem Value="31" meta:resourcekey="ListItemResource32" Text="31"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlMonth0" class="month" meta:resourcekey="ddlMonth0Resource1" />
                                    <asp:DropDownList runat="server" ID="ddlYear0" class="dobyear" meta:resourcekey="ddlYear0Resource1" />
                                </span><span id="spanDOB0" class="errorFields" style="<%=spanStyleDOB0%>">
                                    <%=errMsgDOB%></span>
                            </div>
                        </li>
                        <li runat="server" id="MainGender" visible="false">
                            <label>
                                <asp:Localize ID="lclGender" runat="server" Text="Gender:" meta:resourcekey="lclGenderResource1"></asp:Localize><img
                                    class="required" runat="server" visible="false" id="imgGender" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <span id="spanGender0" class="<%=spanClassGender%>">
                                    <asp:RadioButton runat="server" ID="radioMale0" name="male" GroupName="Gender" meta:resourcekey="radioMale0Resource1" />
                                    <label for="male">
                                        <asp:Localize ID="lclmale" runat="server" Text="Male" meta:resourcekey="lclmaleResource1"></asp:Localize></label>
                                    <asp:RadioButton runat="server" ID="radioFemale0" name="female" GroupName="Gender"
                                        meta:resourcekey="radioFemale0Resource1" />
                                    <label for="female">
                                        <asp:Localize ID="Localize1" runat="server" Text="Female" meta:resourcekey="Localize1Resource1"></asp:Localize></label>
                                </span><span id="spanGenderError0" class="errorFields" style="<%=spanStyleGender0%>">
                                    <%=errMsgGender%></span>
                            </div>
                        </li>
                    </ul>
                    <p class="dietaryNeeds">
                        <strong>
                            <label for="Dietary needs" style="width: 100%">
                                <asp:Localize ID="lclDietaryNeeds" runat="server" Text="Dietary needs" meta:resourcekey="lclDietaryNeedsResource1"></asp:Localize></label></strong></p>
                    <asp:PlaceHolder ID="phDietaryNeeds" runat="server">
                        <asp:CheckBoxList ID="cblDietaryNeeds" runat="server" RepeatColumns="4" BorderStyle="NotSet"
                            Style="text-decoration: none; height: 80px" RepeatDirection="Horizontal" 
                            RepeatLayout="Table" meta:resourcekey="cblDietaryNeedsResource1">
                        </asp:CheckBoxList>
                    </asp:PlaceHolder>
                    <ul class="ccDetailsList diet" runat="server" id="pnlUKDietaryDetails" visible="false">
                        <li>
                            <label for="diabetic" style="text-align: left">
                                <asp:Localize ID="lclDiabetic" runat="server" Text="Diabetic" meta:resourcekey="lclDiabeticResource1"></asp:Localize></label>
                            <asp:CheckBox runat="server" ID="chkDiabetic0" name="diabetic" type="checkbox" meta:resourcekey="chkDiabetic0Resource1" />
                        </li>
                        <li>
                            <label for="tee">
                                <asp:Localize ID="lclaTeetotal" runat="server" Text="Teetotal" meta:resourcekey="lclaTeetotalResource1"></asp:Localize></label>
                            <asp:CheckBox runat="server" ID="chkTeeTotal0" name="tee" type="checkbox" meta:resourcekey="chkTeeTotal0Resource1" />
                        </li>
                    </ul>
                    <ul class="ccDetailsList diet3" runat="server" id="pnlUKRBDietaryDetails" visible="false">
                        <li>
                            <label for="veg">
                                <asp:Localize ID="lclVegetarian" runat="server" Text="Vegetarian" meta:resourcekey="lclVegetarianResource1"></asp:Localize></label>
                            <asp:RadioButton runat="server" ID="radioVegeterian0" name="veg" GroupName="DietPref"
                                meta:resourcekey="radioVegeterian0Resource1" />
                        </li>
                        <li>
                            <label for="kosher">
                                &nbsp;&nbsp;&nbsp;<asp:Localize ID="lclKosher" runat="server" Text="Kosher" meta:resourcekey="lclKosherResource1"></asp:Localize></label><asp:RadioButton runat="server" ID="radioKosher0" name="kosher" GroupName="DietPref"
                                meta:resourcekey="radioKosher0Resource1" />
                        </li>
                        <li>
                            <label for="halal">
                                <asp:Localize ID="lclHalal" runat="server" Text="Halal" meta:resourcekey="lclHalalResource1"></asp:Localize></label>
                            <asp:RadioButton runat="server" ID="radioHalal0" name="halal" GroupName="DietPref"
                                meta:resourcekey="radioHalal0Resource1" />
                        </li>
                    </ul>
                </div>
                <div class="associateCustomer" id="dvAssociateCustomer" runat="server" visible="false">
                    <h3 style="height: 30px">
                        <label for="Associate Customer" style="width: 75%">
                            <asp:Localize ID="lclAssociateCustomer" runat="server" Text="Associate Customer"
                                meta:resourcekey="lclAssociateCustomerResource1"></asp:Localize></label>
                        <span id="Span1" class="householdStatusAssociate" style="display: none" runat="server"
                            nowrap="nowrap">
                            <asp:Label class="errorFields" ID="spnAssAddressError" runat="server" Visible="False"
                                meta:resourcekey="spnAssAddressErrorResource1">
                                <asp:Localize ID="lclAddressinError" runat="server" Text="Address in Error" meta:resourcekey="lclAddressinErrorResource1"></asp:Localize></asp:Label>
                            <asp:Label class="errorFields" ID="spnAssDuplicateError" runat="server" Visible="False"
                                meta:resourcekey="spnAssDuplicateErrorResource1">
                                <asp:Localize ID="lclDuplicate" runat="server" Text="Duplicate" meta:resourcekey="lclDuplicateResource1"></asp:Localize></asp:Label>
                            <asp:Label class="errorFields" ID="spnAssBannedError" runat="server" Visible="False"
                                meta:resourcekey="spnAssBannedErrorResource1">
                                <asp:Localize ID="lclBanned" runat="server" Text="Banned" meta:resourcekey="lclBannedResource1"></asp:Localize></asp:Label>
                            <asp:Label class="errorFields" ID="spnAssLeftError" runat="server" Visible="False"
                                meta:resourcekey="spnAssLeftErrorResource1">
                                <asp:Localize ID="lclleftScheme" runat="server" Text="Left Scheme" meta:resourcekey="lclleftSchemeResource1"></asp:Localize></asp:Label>
                        </span>
                    </h3>
                    <ul class="customer">
                        <li runat="server" id="titleAss" style="display: block;">
                            <label for="title">
                                <asp:Localize ID="lclatitle" runat="server" Text="Title:" meta:resourcekey="lclatitleResource1"></asp:Localize><img
                                    class="required" id="imgmandTittle1" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <span class="SelectBorder">
                                    <asp:DropDownList runat="server" ID="ddlTitle1" meta:resourcekey="ddlTitle1Resource1">
                                    </asp:DropDownList>
                                </span><span id="spanTitle1" class="errorFields" style="display: none">
                                    <%=Resources.CSCGlobal.validTitleMsg %></span>
                            </div>
                        </li>
                        <li id="aCusfName" runat="server" visible="false">
                            <label for="associateFirstName">
                                <asp:Localize ID="lclaFirstName" runat="server" Text="First Name:" meta:resourcekey="lclaFirstNameResource1"></asp:Localize><img
                                    class="required" id="imgmandFN1" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtFirstName1" name="firstName" type="text" MaxLength="20"
                                    meta:resourcekey="txtFirstName1Resource1" />
                                <span class="errorFields" id="spanFirstName1" style="<%=spanStyleFirstName1%>">
                                    <%=errMsgFirstName%></span>
                            </div>
                        </li>
                        <li runat="server" id="middlenameAss" style="display: block;">
                            <label for="associateMiddleinitial">
                                <asp:Localize ID="lclaMiddleInitial" runat="server" Text="Middle Initial(s):" meta:resourcekey="lclaMiddleInitialResource1"></asp:Localize><img
                                    class="required" id="imgmandMN1" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtInitial1" name="initial" type="text" 
                                    meta:resourcekey="txtInitial1Resource1" />
                                <span class="errorFields" id="spanMiddleName1" style="<%=spanStyleMiddleName1%>">
                                    <%=errMsgMiddleName%></span>
                            </div>
                        </li>
                        <li id="liSurnameAss" runat="server" visible="false">
                            <label for="associateSurname">
                                <asp:Localize ID="lclaSurname" runat="server" Text="Surname:" meta:resourcekey="lclaSurnameResource1"></asp:Localize><img
                                    class="required" id="imgmandLN1" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtSurname1" name="surname" type="text" 
                                    meta:resourcekey="txtSurname1Resource1" />
                                <span class="errorFields" id="spanSurname1" style="<%=spanStyleSurname1%>">
                                    <%=errMsgSurname%></span>
                            </div>
                        </li>
                        <li>
                            <label for="initial">
                                <asp:Localize ID="lclAssocEmaiAdd" runat="server" Text="Email address:" meta:resourcekey="lclsAssocemailResource1"></asp:Localize><img
                                    class="required" id="imgmandAssocEmail" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAssocEmailAddress" name="contact" type="text"
                                    MaxLength="60" meta:resourcekey="txtAssocEmailAddressResource1" />
                                <span class="errorFields" id="spanAssocEmailAddress" style="<%=spanStyleAssocEmailAddress%>">
                                    <%=errMsgAssocEmailAddress%></span>
                            </div>
                        </li>
                        <li>
                            <label for="initial" style="height:30px">
                                <asp:Localize ID="lclAssocContactMob" runat="server" Text="Mobile Number:" meta:resourcekey="lclAssocContactMobResource1"></asp:Localize><img
                                    class="required" id="imgMandAssocMobile" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAssocMobileNumber" name="contact" type="text"
                                    meta:resourcekey="txtAssocMobileNumberResource1" />
                                <span class="errorFields" id="spanAssocMoblieNumber" style="<%=spanStyleAssocMoblieNumber%>">
                                    <%=errMsgAssocMobileNumber%></span>
                            </div>
                        </li>
                        <li>
                            <label for="initial" style="height:30px">
                                <asp:Localize ID="lclAssocDaytimePhoneNumber" runat="server" Text="Daytime Number:"
                                    meta:resourcekey="lclAssocDaytimePhoneNumberResource1"></asp:Localize><img class="required"
                                        id="imgAssocDaytimePhoneNumber" visible="false" runat="server" src="I/asterisk.gif"
                                        alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAssocDaytimePhoneNumber" name="contact" type="text"
                                    meta:resourcekey="txtAssocDaytimePhoneNumberResource1" />
                                <span class="errorFields" id="span9" style="<%=spanStyleAssocDaytimePhoneNumber%>">
                                    <%=errMsgAssocDaytimePhoneNumber%></span>
                            </div>
                        </li>
                        <li runat="server" id="EveningNumberAss" style="display: block;">
                            <label for="initial" style="height:30px">
                                <asp:Localize ID="lclAssocEveningPhoneNumber" runat="server" Text="Evening Number:"
                                    meta:resourcekey="lclAssocEveningPhoneNumberResource1"></asp:Localize><img class="required"
                                        id="imgAssocEveningPhoneNumber" visible="false" runat="server" src="I/asterisk.gif"
                                        alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAssocEveningPhoneNumber" name="contact" type="text"
                                    meta:resourcekey="txtAssocEveningPhoneNumberResource1" />
                                <span class="errorFields" id="spanAssocEveningPhoneNumber" style="<%=spanStyleAssocEveningPhoneNumber%>">
                                    <%=errMsgAssocEveningPhoneNumber%></span>
                            </div>
                        </li>
                        <li runat="server" id="liAssoPrimaryID" style="display: none">
                            <label for="primaryid">
                                <asp:Localize ID="lclAssoPrimaryID" runat="server" Text="Primary ID(Social Security No):"
                                    meta:resourcekey="lclAssoPrimaryIDResource1"></asp:Localize>
                                <img class="required" visible="false" id="imgmandAssoPrimId" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAssoPrimId" name="txtAssoPrimId" type="text" meta:resourcekey="txtAssoPrimIdResource1" />
                                <span class="errorFields" id="span2" style="<%=spanStyleAssoPrimaryId%>">
                                    <%=errMsgAssoPrimaryId%></span>
                            </div>
                        </li>
                        <li runat="server" id="liAssoSecondaryID" style="display: none">
                            <label for="secondaryid" style="height: 40px;">
                                <asp:Localize ID="lclAssoSecondaryID" runat="server" Text="Secondary ID(passport,officer number):"
                                    meta:resourcekey="lclAssoSecondaryIDResource1"></asp:Localize>
                                <img class="required" visible="false" id="imgAssomandSecId" runat="server" src="../I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAssoSecId" name="txtAssoSecId" type="text"  meta:resourcekey="txtAssoSecIdResource1" />
                                <span class="errorFields" id="span3" style="<%=spanStyleAssoSecondaryId%>">
                                    <%=errMsgAssoSecondaryId%></span>
                            </div>
                        </li>
                        <li runat="server" id="liAssoLanguage" style="display: none">
                            <label>
                                <asp:Localize ID="lclAssoLanguage" runat="server" Text="Preferred Language:" meta:resourcekey="lclAssoLanguageResource1"></asp:Localize>
                                <img class="required" id="imgmandAssoLanguage" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" />
                            </label>
                            <div class="inputFields">
                                <asp:DropDownList ID="rdoAssoLanguage" runat="server" 
                                    meta:resourcekey="rdoAssoLanguageResource1">
                                </asp:DropDownList>
                                <span id="span5" class="errorFields" style="<%=spanStyleAssoLanguage%>">
                                    <%=errMsgAssoLanguage%></span>
                            </div>
                        </li>
                        <li runat="server" id="liAssoRace" style="display: none">
                            <label for="race">
                                <asp:Localize ID="lclAssoRace" runat="server" Text="Please select race:" meta:resourcekey="lclAssoRaceResource1"></asp:Localize>
                                <img class="required" visible="false" id="imgAssoRace" runat="server" src="../I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:DropDownList runat="server" ID="ddlAssoRace" class="year" CssClass="year" 
                                    meta:resourcekey="ddlAssoRaceResource1" />
                                <span class="errorFields" id="span6" style="<%=spanStyleAssoRace%>">
                                    <%=errMsgAssoRace%></span>
                            </div>
                        </li>
                        <li>
                            <label for="associateday">
                                <asp:Localize ID="lclaDOB" runat="server" Text="Date Of Birth:" meta:resourcekey="lclaDOBResource1"></asp:Localize><img
                                    class="required" id="imgmandDOB1" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" />
                            </label>
                            <div class="inputFields">
                                <span id="spanDOBError1" class="<%=spanClassDOBDropDown1%>">
                                    <asp:DropDownList runat="server" ID="ddlDay1" class="day" meta:resourcekey="ddlDay1Resource1">
                                        <asp:ListItem Value="" meta:resourcekey="ListItemResource33" Text="Day"></asp:ListItem>
                                        <asp:ListItem Value="01" meta:resourcekey="ListItemResource34" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="02" meta:resourcekey="ListItemResource35" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="03" meta:resourcekey="ListItemResource36" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="04" meta:resourcekey="ListItemResource37" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="05" meta:resourcekey="ListItemResource38" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="06" meta:resourcekey="ListItemResource39" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="07" meta:resourcekey="ListItemResource40" Text="7"></asp:ListItem>
                                        <asp:ListItem Value="08" meta:resourcekey="ListItemResource41" Text="8"></asp:ListItem>
                                        <asp:ListItem Value="09" meta:resourcekey="ListItemResource42" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="10" meta:resourcekey="ListItemResource43" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" meta:resourcekey="ListItemResource44" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" meta:resourcekey="ListItemResource45" Text="12"></asp:ListItem>
                                        <asp:ListItem Value="13" meta:resourcekey="ListItemResource46" Text="13"></asp:ListItem>
                                        <asp:ListItem Value="14" meta:resourcekey="ListItemResource47" Text="14"></asp:ListItem>
                                        <asp:ListItem Value="15" meta:resourcekey="ListItemResource48" Text="15"></asp:ListItem>
                                        <asp:ListItem Value="16" meta:resourcekey="ListItemResource49" Text="16"></asp:ListItem>
                                        <asp:ListItem Value="17" meta:resourcekey="ListItemResource50" Text="17"></asp:ListItem>
                                        <asp:ListItem Value="18" meta:resourcekey="ListItemResource51" Text="18"></asp:ListItem>
                                        <asp:ListItem Value="19" meta:resourcekey="ListItemResource52" Text="19"></asp:ListItem>
                                        <asp:ListItem Value="20" meta:resourcekey="ListItemResource53" Text="20"></asp:ListItem>
                                        <asp:ListItem Value="21" meta:resourcekey="ListItemResource54" Text="21"></asp:ListItem>
                                        <asp:ListItem Value="22" meta:resourcekey="ListItemResource55" Text="22"></asp:ListItem>
                                        <asp:ListItem Value="23" meta:resourcekey="ListItemResource56" Text="23"></asp:ListItem>
                                        <asp:ListItem Value="24" meta:resourcekey="ListItemResource57" Text="24"></asp:ListItem>
                                        <asp:ListItem Value="25" meta:resourcekey="ListItemResource58" Text="25"></asp:ListItem>
                                        <asp:ListItem Value="26" meta:resourcekey="ListItemResource59" Text="26"></asp:ListItem>
                                        <asp:ListItem Value="27" meta:resourcekey="ListItemResource60" Text="27"></asp:ListItem>
                                        <asp:ListItem Value="28" meta:resourcekey="ListItemResource61" Text="28"></asp:ListItem>
                                        <asp:ListItem Value="29" meta:resourcekey="ListItemResource62" Text="29"></asp:ListItem>
                                        <asp:ListItem Value="30" meta:resourcekey="ListItemResource63" Text="30"></asp:ListItem>
                                        <asp:ListItem Value="31" meta:resourcekey="ListItemResource64" Text="31"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList runat="server" ID="ddlMonth1" class="month" meta:resourcekey="ddlMonth1Resource1" />
                                    <asp:DropDownList runat="server" ID="ddlYear1" class="year" meta:resourcekey="ddlYear1Resource1" />
                                </span><span id="spanDOB1" class="errorFields" style="<%=spanStyleDOB1%>">
                                    <%=errMsgDOB%></span>
                            </div>
                        </li>
                        <li runat="server" id="AssGender" visible="false">
                            <label>
                                <asp:Localize ID="lclaGender" runat="server" Text="Gender:" meta:resourcekey="lclaGenderResource1"></asp:Localize><img
                                    class="required" id="imgmandGender1" visible="false" runat="server" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <span id="spanGender1" class="<%=spanClassGender1%>">
                                    <asp:RadioButton runat="server" ID="radioMale1" name="male" GroupName="Gender1" meta:resourcekey="radioMale1Resource1" />
                                    <label for="associateMale">
                                        <asp:Localize ID="lclaMale" runat="server" Text="Male" meta:resourcekey="lclaMaleResource1"></asp:Localize></label>
                                    <asp:RadioButton runat="server" ID="radioFemale1" name="female" GroupName="Gender1"
                                        meta:resourcekey="radioFemale1Resource1" />
                                    <label for="associateFemale">
                                        <asp:Localize ID="lclaFemale" runat="server" Text="Female" meta:resourcekey="lclaFemaleResource1"></asp:Localize></label>
                                </span><span id="spanGenderError1" class="errorFields" style="<%=spanStyleGender1%>">
                                    <%=errMsgGender%></span>
                            </div>
                        </li>
                    </ul>
                    <p class="dietaryNeeds">
                        <strong>
                            <label for="Dietary needs" style="width: 100%">
                                <asp:Localize ID="lclaDietaryneeds" runat="server" Text="Dietary needs" meta:resourcekey="lclaDietaryneedsResource1"></asp:Localize></label></strong></p>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <asp:CheckBoxList ID="cblDietaryNeeds1" runat="server" RepeatColumns="4" BorderStyle="NotSet"
                            Style="text-decoration: none; height: 80px" RepeatDirection="Horizontal" 
                            RepeatLayout="Table" meta:resourcekey="cblDietaryNeeds1Resource1">
                        </asp:CheckBoxList>
                    </asp:PlaceHolder>
                    <ul id="Ul1" class="ccDetailsList diet" runat="server" visible="false">
                        <li>
                            <label for="diabetic">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Localize ID="lclaDiabetic" runat="server" Text="Diabetic"
                                    meta:resourcekey="lclaDiabeticResource1"></asp:Localize></label><asp:CheckBox runat="server" ID="chkDiabetic1" name="diabetic" type="checkbox" meta:resourcekey="chkDiabetic1Resource1" />
                        </li>
                        <li>
                            <label for="tee">
                                <asp:Localize ID="lclTeetotala" runat="server" Text="Teetotal" meta:resourcekey="lclTeetotalaResource1"></asp:Localize></label>
                            <asp:CheckBox runat="server" ID="chkTeeTotal1" name="tee" type="checkbox" meta:resourcekey="chkTeeTotal1Resource1" />
                        </li>
                    </ul>
                    <ul id="Ul2" class="ccDetailsList diet3" runat="server" visible="false">
                        <li>
                            <label for="veg">
                                <asp:Localize ID="lclaVegetarian" runat="server" Text="Vegetarian" meta:resourcekey="lclaVegetarianResource1"></asp:Localize></label>
                            <asp:RadioButton runat="server" ID="radioVegeterian1" name="veg" GroupName="DietPref1"
                                meta:resourcekey="radioVegeterian1Resource1" />
                        </li>
                        <li>
                            <label for="kosher">
                                &nbsp;&nbsp;&nbsp;<asp:Localize ID="lclaKosher" runat="server" Text="Kosher" meta:resourcekey="lclaKosherResource1"></asp:Localize></label><asp:RadioButton runat="server" ID="radioKosher1" name="kosher" GroupName="DietPref1"
                                meta:resourcekey="radioKosher1Resource1" />
                        </li>
                        <li>
                            <label for="halal">
                                <asp:Localize ID="lclaHalal" runat="server" Text="Halal" meta:resourcekey="lclaHalalResource1"></asp:Localize></label>
                            <asp:RadioButton runat="server" ID="radioHalal1" name="halal" GroupName="DietPref1"
                                meta:resourcekey="radioHalal1Resource1" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div>
            <div class="ccBlueHeaderSection" runat="server" id="divCustomerStatusForUSL">
                <div class="cc_bluehead">
                    <h3>
                        <label for="CusStatus">
                            <asp:Localize ID="lclCusStatus" runat="server" Text="Customer Status" meta:resourcekey="lclstatusofcustomerResource1"></asp:Localize></label></h3>
                </div>
                <div class="cc_body">
                    <div class="mainCustomer">
                        <h3 style="height: 30px">
                            <label for="MainCusStatus" style="width: 75%">
                                <asp:Localize ID="lclMainCusStatus" runat="server" Text="Main Customer" meta:resourcekey="lclstatusMainCustomerResource1"></asp:Localize></label></h3>
                        <ul class="customer">
                            <li>
                                <label for="Status">
                                    <asp:Localize ID="lclStatusMain" runat="server" Text="Customer Status" meta:resourcekey="lclstatusResource1"></asp:Localize></label>
                                <div class="inputFields">
                                    <span class="SelectBorder">
                                    <asp:Label ID="lblCustomerStatus" runat="server"  meta:resourcekey="lblCustomerStatusResource1"></asp:Label>
                                       <asp:DropDownList runat="server" ID="ddlCustomerStatus" onChange="javascript:setStatusDropdownValue()" />
                                    </span>
                                </div>
                            </li>
                            <li>
                                <label for="Status">
                                    <asp:Localize ID="lclEmailStatus" runat="server" Text="Email Status" meta:resourcekey="lclEmailStatusResource1"></asp:Localize></label>
                                <div class="inputFields">
                                    <span class="SelectBorder">
                                        <asp:Label ID="lblEmailStatus" runat="server"  meta:resourcekey="lblEmailStatusResource1"></asp:Label>
                                       <asp:DropDownList runat="server" ID="ddlEmailStatus"  onChange="javascript:setStatusDropdownValue()"  />
                                    </span>
                                </div>
                            </li>
                            <li>
                                <label for="Status">
                                    <asp:Localize ID="lclMobileStaus" runat="server" Text="Mobile Status" meta:resourcekey="lclMobileStausResource1"></asp:Localize></label>
                                <div class="inputFields">
                                    <span class="SelectBorder">
                                     <asp:Label ID="lblMobileStatus" runat="server"  meta:resourcekey="lblMobileStatusResource1"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlMobileStatus"  onChange="javascript:setStatusDropdownValue()" />
                                    </span>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="associateCustomer" id="divAssocCustStatus" visible="false" runat="server">
                        <h3 style="height: 30px">
                            <label for="AssStatus" style="width: 75%">
                                <asp:Localize ID="lclAssCus" runat="server" Text="Associate Customer" meta:resourcekey="lclassociatecustomerstatusResource1"></asp:Localize></label>
                        </h3>
                        <ul class="customer">
                            <li>
                                <label for="Status">
                                    <asp:Localize ID="lclAssstCus" Text="Customer Status" runat="server" meta:resourcekey="lclcusStatusResource1"></asp:Localize></label>
                                <div class="inputFields">
                                    <span class="SelectBorder">
                                    <asp:Label ID="lblAssocCustStatus" runat="server"  meta:resourcekey="lblAssocCustStatusResource1"></asp:Label>
                                     <asp:DropDownList runat="server" ID="ddlAssocCustStatus"   onChange="javascript:setStatusDropdownValue()" />
                                    </span>
                                </div>
                            </li>
                            <li>
                                <label for="Status">
                                    <asp:Localize ID="lclAssoEmailStatus" runat="server" Text="Email Status" meta:resourcekey="lclAssoEmailStatusResource1"></asp:Localize></label>
                                <div class="inputFields">
                                    <span class="SelectBorder">
                                     <asp:Label ID="lblAssoEmailStatus" runat="server"  meta:resourcekey="lblAssoEmailStatusResource1"></asp:Label>
                                       <asp:DropDownList runat="server" ID="ddlAssoEmailStatus"  onChange="javascript:setStatusDropdownValue()"  />
                                    </span>
                                </div>
                            </li>
                            <li>
                                <label for="Status">
                                    <asp:Localize ID="lclAssoMobileStatus" runat="server" Text="Mobile Status" meta:resourcekey="lclAssoMobileStausResource1"></asp:Localize></label>
                                <div class="inputFields">
                                    <span class="SelectBorder">
                                      <asp:Label ID="lblAssoMobileStatus" runat="server"  meta:resourcekey="lblAssoMobileStatusResource1"></asp:Label>
                                     <asp:DropDownList runat="server" ID="ddlAssoMobileStatus"   onChange="javascript:setStatusDropdownValue()" />
                                    </span>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="ccBlueHeaderSection" visible="false" runat="server" id="divBusinessDetails">
            <div class="cc_bluehead">
                <h3>
                    <label for="CusStatus">
                        <asp:Localize ID="lclBusniessDetails" runat="server" Text="Busniess Details" meta:resourcekey="lclBusniessDetails"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                            <div class="addressChange">
                <ul class="customer">
                    <li visible="false" runat="server" id="liBusniessName">
                        <label for="address">
                            <asp:Localize ID="lclBusniessName" runat="server" Text="BusniessName:" meta:resourcekey="lclBusniessName"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="span4" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="TxtBusniessName" name="BusniessName" type="text"
                                    MaxLength="36" />
                            </span>
                            <%--<span class="errorFields" id="spBusniessName" style="<%=spanStyleAddress%>"><%=errMsgBusniessName%></span>--%>
                        </div>
                    </li>
                    <li visible="false" runat="server" id="liBusniessRegNo">
                        <label for="address">
                            <asp:Localize ID="lclBusniessRegNo" runat="server" Text="Business Registration No:" meta:resourcekey="lclBusniessRegNo"></asp:Localize></label>
                        <div>
                            <span id="spBusniessRegNo">
                               <label style="text-align: left; border:1px solid #0054a4; height:18px;"><asp:Localize ID="lclBusniessRegNoVal" runat="server"></asp:Localize></label>
                            </span>
                        </div>
                    </li>
                    <li visible="false" runat="server" id="liBusinessType">
                    <label for="address">
                            <asp:Localize ID="lclBusinessTypeCobrand" runat="server" Text="Business Type-Cobrand:" meta:resourcekey="lclBusinessTypeCobrand"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessTypeCobrand" class="<%=spanClassAddress%>">
                               <asp:DropDownList runat="server" ID="ddlBusinessType" Width="19em" meta:resourcekey="ddlAddressResource2" />
                            </span>
                           <%-- <span class="errorFields" id="span12" style="<%=spanStyleAddress%>"><%=errMsgBType%></span>--%>
                        </div>
                    </li>                    
                    <li visible="false" runat="server" id="liBusinessAddress1">
                         <label for="address">
                            <asp:Localize ID="lclBusinessAddress1" runat="server" Text="Address 1:" meta:resourcekey="lclBusinessAddress1"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessAddress1" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessAddress1" name="BusinessAddress1" type="text"
                                    MaxLength="160" />
                            </span>
                            <%--<span class="errorFields" id="spBusinessAddress1" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liBusinessAddress2">
                     <label for="address">
                            <asp:Localize ID="lclBusinessAddress2" runat="server" Text="Address 2:" meta:resourcekey="lclBusinessAddress2"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessAddress2" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessAddress2" name="BusinessAddress2" type="text"
                                    MaxLength="160" />
                            </span>
                            <%--<span class="errorFields" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liBusinessAddress3">
                    <label for="address">
                            <asp:Localize ID="lclBusinessAddress3" runat="server" Text="Address 3:" meta:resourcekey="lclBusinessAddress3"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessAddress3" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessAddress3" name="BusinessAddress3" type="text"
                                    MaxLength="160" />
                            </span>
                            <%--<span class="errorFields" id="SpBusinessAddress3" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liBusinessAddress4">
                    <label for="address">
                            <asp:Localize ID="lclBusinessAddress4" runat="server" Text="Address 4:" meta:resourcekey="lclBusinessAddress4"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessAddress4" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessAddress4" name="BusinessAddress4" type="text"
                                    MaxLength="160" />
                            </span>
                            <%--<span class="errorFields" id="SpBusinessAddress4" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liBusinessAddress5">
                    <label for="address">
                            <asp:Localize ID="lclBusinessAddress5" runat="server" Text="Address 5:" meta:resourcekey="lclBusinessAddress5"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessAddress5" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessAddress5" name="BusinessAddress5" type="text"
                                    MaxLength="160" />
                            </span>
                            <%--<span class="errorFields" id="SpBusinessAddress5" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liBusinessAddress6">
                    <label for="address">
                            <asp:Localize ID="lclBusinessAddress6" runat="server" Text="Address 6:" meta:resourcekey="lclBusinessAddress6"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessAddress6" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessAddress6" name="BusinessAddress6" type="text"
                                    MaxLength="160" />
                            </span>
                            <%--<span class="errorFields" id="SpBusinessAddress6" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                    <li visible="false" runat="server" id="liBusinessPostcode">
                    <label for="address">
                            <asp:Localize ID="lclBusinessPostcode" meta:resourcekey="lclBusinessPostcode" runat="server"></asp:Localize></label>
                        <div class="inputFields">
                            <span id="spBusinessPostcode" class="<%=spanClassAddress%>">
                                <asp:TextBox runat="server" ID="txtBusinessPostcode" name="BusinessPostcode" type="text"
                                    MaxLength="20" />
                            </span>
                            <%--<span class="errorFields" id="SpBusinessPostcode" style="<%=spanStyleAddress%>"><%=errMsgAddress%></span>--%>
                        </div>

                    </li>
                </ul>
                </div>
            </div>
            <p></p>
        </div>
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="HouseHoldMain">
                        <asp:Localize ID="lclHouseHold" runat="server" Text="Household details" meta:resourcekey="lclHouseholddetailsResource1"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                <div class="addressChange">
                    <ul class="customer">
                        <li>
                        <div id="divlblpostcode" runat="server"> 
                            <label for="postCode">
                                <asp:Localize ID="lclpocode" runat="server" Text="Postcode:" meta:resourcekey="lclpocodeResource1"></asp:Localize><img
                                    class="required" runat="server" visible="false" id="imgPostcode" src="I/asterisk.gif"
                                    alt="Required" /></label> </div>
                            <div class="inputFields">
                                        <div id="divhidetxtpostcode" runat="server"> 
                                <asp:TextBox runat="server" ID="txtPostCode" name="postCode" type="text" MaxLength="9"
                                    meta:resourcekey="txtPostCodeResource2" /> </div>
                                <asp:ImageButton runat="server" src="I/findaddress.gif" ID="btnFindAddress" name="findAddress"
                                    OnClick="btnFindAddress_Click" class="imgBtn" meta:resourcekey="btnFindAddressResource2" />
                                <span class="errorFields" id="spanPostCode" style="<%=spanStylePostCode%>">
                                    <%=errMsgPostCode%></span>
                                <asp:HiddenField ID="hdnPostCodeNumber" runat="server" />
                            </div>
                        </li>
                        <li style="display: block;" runat="server" id="liAddress">
                            <label for="address">
                                <asp:Localize ID="lclchoseadd" runat="server" Text="Choose an address:" meta:resourcekey="lcladdressResource1"></asp:Localize><img
                                    class="required"  runat="server" id="imgAddress" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <span id="spanAddress" class="<%=spanClassAddress%>">
                                    <asp:DropDownList runat="server" ID="ddlAddress" Width="19em" meta:resourcekey="ddlAddressResource2" />
                                    
                                </span><span class="errorFields" id="spanAddressError" style="<%=spanStyleAddress%>">
                                    <%=errMsgAddress%></span>
                            </div>
                        </li>
                        <li>
                            <label for="address1">
                                <asp:Localize ID="lcladd1" runat="server" Text="House no/name:" meta:resourcekey="lcladdress1Resource1"></asp:Localize><img
                                    class="required" runat="server" id="imgHouseName" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtAddressLine1" name="AddressLine1" type="text"
                                    MaxLength="100" meta:resourcekey="txtAddressLine1Resource2" />
                                    <span class="errorFields" id="spanAddressLine1" style="<%=spanStyleAddressLine1%>">
                                      <%=errMsgAddressLine1%>
                                     </span>
                            </div>
                        </li>
                        <li>
                            <label for="address2">
                                <asp:Localize ID="lcladd2" runat="server" Text="Address Line 2:" meta:resourcekey="lcladdress2Resource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgadd2" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtStreet" name="AddressLine2" type="text" MaxLength="100"
                                    meta:resourcekey="txtStreetResource2" />
                                    <span class="errorFields" id="spanAddressLine2" style="<%=spanStyleAddressLine2%>">
                                    <%=errMsgAddressLine2%>
                               </span>
                            </div>
                        </li>
                        <li>
                            <label for="address3">
                                <asp:Localize ID="lcladd3" runat="server" Text="Address Line 3:" meta:resourcekey="lcladdress3Resource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgadd3" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtLocality" name="AddressLine3" type="text" MaxLength="100"
                                    meta:resourcekey="txtLocalityResource2" />
                                    <span class="errorFields" id="spanAddressLine3" style="<%=spanStyleAddressLine3%>">
                                     <%=errMsgAddressLine3%>
                                    </span>
                            </div>
                        </li>
                        <li>
                            <label for="address4">
                                <asp:Localize ID="lclAdd4" runat="server" Text="Address Line 4:" meta:resourcekey="lcladdress4Resource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgadd4" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtTown" name="AddressLine4" type="text" MaxLength="100"
                                    meta:resourcekey="txtTownResource2" />
                                    <span class="errorFields" id="spanAddressLine4" style="<%=spanStyleAddressLine4%>">
                                     <%=errMsgAddressLine4%>
                                     </span>
                            </div>
                        </li>
                        <li id="liCounty" runat="server">
                            <label for="address5">
                                <asp:Localize ID="lclAdd5" runat="server" Text="Address Line 5:" meta:resourcekey="lcladdress5Resource1"></asp:Localize><img
                                    class="required" visible="false" runat="server" id="imgadd5" src="I/asterisk.gif"
                                    alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtCountyDetails" name="AddressLine5" type="text"
                                    MaxLength="100" meta:resourcekey="txtCountyDetailsResource2" />
                                    <asp:DropDownList runat="server" ID="ddlProvince" Visible="false" meta:resourcekey="CusUseSelectProvince" />
                                    <span class="errorFields" id="spanAddressLine5" style="<%=spanStyleAddressLine5%>">
                                     <%=errMsgAddressLine5%>
                                     </span>
                            </div>
                        </li>
                    </ul>
                    <p class="pageAction">
                        <asp:ImageButton ID="btnSaveAddress" runat="server" ImageUrl="I/saveAddressChange1.gif"
                            CssClass="imgBtn" AlternateText="Save address" OnClick="btnConfirmCustomerDtls_Click" onclientclick="return clearOnConfirm();"
                            Visible="true" meta:resourcekey="btnSaveAddressResource2" />
                    </p>
                </div>
                <div class="contactDetails">
                    <ul class="customer">
                        <li>
                            <label for="Status" style="width: 14em;">
                                <asp:Localize ID="lclMailst" runat="server" Text="Mail Status" meta:resourcekey="lclmailstatusResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <span class="SelectBorder">
                                    <asp:Label ID="lblMailStatus" runat="server" meta:resourcekey="lblMailStatusResource1"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlMailStatus"  onChange="javascript:setStatusDropdownValue()"  />
                                </span>
                            </div>
                        </li>
                        <li>
                            <label for="contact" style="width: 14em;">
                                <asp:Localize ID="lcljoinro" runat="server" Text="Join Route Code:" meta:resourcekey="lcljoinroResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:Label runat="server" ID="lblJoinRoute" class="email" meta:resourcekey="lblJoinRouteResource1" />
                            </div>
                        </li>
                        <li>
                            <label for="contact" style="width: 14em;">
                                <asp:Localize ID="lclJoinedStoreID" runat="server" Text="Join Store:" meta:resourcekey="lclJoinedStoreIDResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:Label runat="server" ID="lblJoinedStoreID" class="email" meta:resourcekey="lblJoinedStoreIDResource1" />
                            </div>
                        </li>
                        <li>
                            <label for="contact" style="width: 14em;">
                                <asp:Localize ID="lclProCode" runat="server" Text="Promotional Code:" meta:resourcekey="lclProCodeResource1"></asp:Localize></label>
                            <div class="inputFields">
                                <asp:Label runat="server" ID="lblPromotionalCode" class="email" meta:resourcekey="lblPromotionalCodeResource1" />
                            </div>
                        </li>
                    </ul>
                </div>
                <ul class="people hideHousehold">
                    <li>
                        <label for="people">
                            <asp:Localize ID="lclPeopleCon" runat="server" Text="How many people are there in household?"
                                meta:resourcekey="lclpeopleResource1"></asp:Localize>
                                </label>
                        <asp:TextBox runat="server" ID="txtNoofPeople" name="people" type="text" MaxLength="2" AutoComplete="off"
                            meta:resourcekey="txtNoofPeopleResource2" />
                       <%-- <div class="inputFields">
                            <span class="errorFields pplWdth" id="spanNoHHPersons" style="<%=spanStyleNoHHPersons%>">
                                <%=errMsgNoHHPersons%></span>
                        </div>--%>
                    </li>
                </ul>
                <p>
                    <asp:Localize ID="lclAgeContacts" runat="server" Text="Ages of other people in household (for child under 1, enter 0)?"
                        meta:resourcekey="lclOtherAgesResource1"></asp:Localize></p>
                <ul class="ccDetailsList ccDetailsPadding">
                    <li id="liFirstAge" class="liAgeddl">
                        <label for="age1" class="lblAge">
                            <asp:Localize ID="lclage1" runat="server" Text="Age:" meta:resourcekey="lclage1Resource1"></asp:Localize>
                            </label>
                        <div class="inputAgeFields inputFields">
                            <asp:TextBox runat="server" ID="txtAge1" CssClass="txtAge" name="age1" type="text" MaxLength="2" meta:resourcekey="txtAge1Resource2" ReadOnly="True" />
                           <%-- <span class="errorFields dtlsWdth" id="spanAge1" style="<%=spanStyleAge1%>">
                                <%=errMsgAge1%></span>--%>
                        </div>
                    </li>
                    <li class="liAgeddl">
                        <label for="age2" class="lblAge">
                            <asp:Localize ID="lclMAge2" runat="server" Text="Age:" meta:resourcekey="lclage2Resource1"></asp:Localize>
                            </label>
                        <div class="inputAgeFields inputFields">
                            <%--<asp:TextBox runat="server" ID="txtAge2" name="age2" type="text" MaxLength="2" meta:resourcekey="txtAge2Resource2" />--%>
                            <asp:DropDownList ID="ddlAge2" CssClass="ddlAge" runat="server" DataTextField="Year"></asp:DropDownList>
                            <%--<span class="errorFields dtlsWdth" id="spanAge2" style="<%=spanStyleAge2%>">--%>
                                <%--<%=errMsgAge2%></span>--%>
                        </div>
                    </li>
                    <li class="liAgeddl">
                        <label for="age3" class="lblAge">
                            <asp:Localize ID="lclAge3" runat="server" Text="Age:" meta:resourcekey="lclage3Resource1"></asp:Localize>
                            </label>
                        <div class="inputAgeFields inputFields">
                           <%-- <asp:TextBox runat="server" ID="txtAge3" name="age3" type="text" MaxLength="2" meta:resourcekey="txtAge3Resource2" />
                            <span class="errorFields dtlsWdth" id="spanAge3" style="<%=spanStyleAge3%>">
                                <%=errMsgAge3%></span>--%>
                                <asp:DropDownList ID="ddlAge3" CssClass="ddlAge" runat="server" DataTextField="Year"></asp:DropDownList>
                        </div>
                    </li>
                    <li class="liAgeddl">
                        <label for="age4" class="lblAge">
                            <asp:Localize ID="lclAge4" runat="server" Text="Age:" meta:resourcekey="lclage4Resource1"></asp:Localize>
                            </label>
                        <div class="inputAgeFields inputFields">
                            <%--<asp:TextBox runat="server" ID="txtAge4" name="age4" type="text" MaxLength="2" meta:resourcekey="txtAge4Resource2" />
                            <span class="errorFields dtlsWdth" id="spanAge4" style="<%=spanStyleAge4%>">
                                <%=errMsgAge4%></span>--%>
                                <asp:DropDownList ID="ddlAge4" CssClass="ddlAge" runat="server" DataTextField="Year"></asp:DropDownList>
                        </div>
                    </li>
                    <li class="liAgeddl">
                        <label for="age5" class="lblAge">
                            <asp:Localize ID="lclAge5" runat="server" Text="Age:" meta:resourcekey="lclage5Resource1"></asp:Localize></label>
                        <div class="inputAgeFields inputFields">
                            <%--<asp:TextBox runat="server" ID="txtAge5" name="age5" type="text" MaxLength="2" meta:resourcekey="txtAge5Resource2" />
                            <span class="errorFields dtlsWdth" id="spanAge5" style="<%=spanStyleAge5%>">
                                <%=errMsgAge5%></span>--%>
                                <asp:DropDownList ID="ddlAge5" CssClass="ddlAge" runat="server" DataTextField="Year"></asp:DropDownList>
                        </div>
                    </li>
                      <li class="liAgeddl">
                        <label for="age6" class="lblAge">
                            <asp:Localize ID="lclAge6" runat="server" Text="Age:" meta:resourcekey="lclage6Resource1"></asp:Localize></label>
                        <div class="inputAgeFields inputFields">
                            <%--<asp:TextBox runat="server" ID="txtAge5" name="age5" type="text" MaxLength="2" meta:resourcekey="txtAge5Resource2" />
                            <span class="errorFields dtlsWdth" id="spanAge5" style="<%=spanStyleAge5%>">
                                <%=errMsgAge5%></span>--%>
                                <asp:DropDownList ID="ddlAge6" CssClass="ddlAge" runat="server" DataTextField="Year"></asp:DropDownList>
                        </div>
                    </li>
                </ul>
                <%--<ul class="householdStatus">
                    <li class="errorFields" id="spnAddressError" runat="server" visible="false">Address in Error</li>
                    <li class="errorFields" id="spnDuplicateError" runat="server" visible="false">Duplicate</li>
                    <li class="errorFields" id="spnBannedError" runat="server" visible="false">Banned</li>
                    <li class="errorFields" id="spnLeftError" runat="server" visible="false">Left Scheme</li>
                </ul>--%>
                <%--<p class="pageAction">
                    <asp:ImageButton ID="btnConfirmCustomerDtls" runat="server" ImageUrl="I/confirm.gif"
                            CssClass="imgBtn" AlternateText="Confirm" 
                        OnClick="btnConfirmCustomerDtls_Click" />
                </p>--%>
                <asp:HiddenField ID="hdnMobilePhoneNumber" runat="server" />
                <asp:HiddenField ID="hdnEveningPhoneNumber" runat="server" />
                <asp:HiddenField ID="hdnNumberOfCustomers" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPrimaryCustID" runat="server" />
                <asp:HiddenField ID="hdnAssociateCustID" runat="server" />
                <asp:HiddenField ID="hdnMailingAddressLine1" runat="server" />
                <asp:HiddenField ID="hdnMailingAddressLine1Index" runat="server" />
                <asp:HiddenField ID="hdnAssociateCustomerDiv" runat="server" Value="0" />
                <asp:DropDownList runat="server" ID="ddlbuildingNoStreetListWithoutStreet" Visible="False"
                    meta:resourcekey="ddlbuildingNoStreetListWithoutStreetResource2" />
                <!-- Below are the hidden fields to store the Associate customer detail when its disabled-->
                <asp:HiddenField ID="hdnddlTitle1" runat="server" />
                <asp:HiddenField ID="hdntxtFirstName1" runat="server" />
                <asp:HiddenField ID="hdntxtInitial1" runat="server" />
                <asp:HiddenField ID="hdntxtSurname1" runat="server" />
                <asp:HiddenField ID="hdnddlDay1" runat="server" />
                <asp:HiddenField ID="hdnddlMonth1" runat="server" />
                <asp:HiddenField ID="hdnddlYear1" runat="server" />
                <asp:HiddenField ID="hdnGender" runat="server" />
                <asp:HiddenField ID="hdnchkDiabetic1" runat="server" Value="0" />
                <asp:HiddenField ID="hdnchkTeeTotal1" runat="server" Value="0" />
                <asp:HiddenField ID="hdnradioVegeterian1" runat="server" Value="0" />
                <asp:HiddenField ID="hdnradioHalal1" runat="server" Value="0" />
                <asp:HiddenField ID="hdnradioKosher1" runat="server" Value="0" />
                <!-- Added for Mandatory Configuration -->
                <asp:HiddenField ID="hdnProfanityvalues" runat="server" />
                <asp:HiddenField ID="hdnJoinroute" runat="server" Value="33" />
                <asp:HiddenField ID="hdnName1" runat="server" Value="true" />
                <asp:HiddenField ID="hdnName2" runat="server" Value="true" />
                <asp:HiddenField ID="hdnName3" runat="server" Value="true" />
                <asp:HiddenField ID="hdnTitle" runat="server" Value="true" />
                <asp:HiddenField ID="hdnDOB" runat="server" Value="true" />
                <asp:HiddenField ID="hdnDateOfBirth" runat="server" Value="" />
                <asp:HiddenField ID="hdnSex" runat="server" Value="true" />
                <asp:HiddenField ID="hdnMailingAddress1" runat="server" Value="true" />
                <asp:HiddenField ID="hdnPostcode" runat="server" Value="true" />
                <asp:HiddenField ID="hdnlandline" runat="server" Value="true" />
                <asp:HiddenField ID="hdnMobile" runat="server" Value="true" />
                <asp:HiddenField ID="hdnEvening" runat="server" Value="true" />
                <asp:HiddenField ID="hdnEmail" runat="server" Value="true" />
                <asp:HiddenField ID="hdnSSN" runat="server" Value="true" />
                <asp:HiddenField ID="hdnPhoneNoMinVal" runat="server" />
                <asp:HiddenField ID="hdnPhoneNoPrefix" runat="server" />
                <asp:HiddenField ID="hdnMobileNoPrefix" runat="server" />
                <asp:HiddenField ID="hdnEveningNoPrefix" runat="server" />
                <asp:HiddenField ID="hdnPostCodeMinVal" runat="server" />
                <asp:HiddenField ID="hdnPostCodeFormat" runat="server" />
                <asp:HiddenField ID="hdnPostCodeFormat1" runat="server" />
                <asp:HiddenField ID="hdnAccountTypeMinVal" runat="server" />
                <asp:HiddenField ID="hdnAccountTypeFormat" runat="server" />
                <asp:HiddenField ID="hdnMobileNoMinVal" runat="server" />
                <asp:HiddenField ID="hdnConfigVisible" runat="server" Value="false" />
                <asp:HiddenField ID="hdnPrimId" runat="server" Value="true" />
                <asp:HiddenField ID="hdnSecId" runat="server" Value="true" />
                <asp:HiddenField ID="hdnLanguage" runat="server" Value="true" />
                <asp:HiddenField ID="hdnRace" runat="server" Value="true" />
                <asp:HiddenField ID="hdnNewpostcode" runat="server" Value="true" />
                <asp:HiddenField ID="hdnPrimIdMinValue" runat="server" />
                <asp:HiddenField ID="hdnPrimIdMaxValue" runat="server" />
                <asp:HiddenField ID="hdnSecIdMinValue" runat="server" />
                <asp:HiddenField ID="hdnSecIdMaxValue" runat="server" />
                <asp:HiddenField ID="hdnIdFormat" runat="server" />
                <asp:HiddenField ID="hdnAssoPrimId" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAssoSecId" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAssoLanguage" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAssoRace" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAssoPrimIdMinValue" runat="server" />
                <asp:HiddenField ID="hdnAssoPrimIdMaxValue" runat="server" />
                <asp:HiddenField ID="hdnAssoSecIdMinValue" runat="server" />
                <asp:HiddenField ID="hdnAssoSecIdMaxValue" runat="server" />
                <asp:HiddenField ID="hdnAddressLine1" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAddressLine2" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAddressLine3" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAddressLine4" runat="server" Value="true" />
                <asp:HiddenField ID="hdnAddressLine5" runat="server" Value="true" />
                 <asp:HiddenField ID="hdnChkPostcode" runat="server"/>
                <asp:HiddenField ID="hdnChkAddressline1" runat="server"/>
                <asp:HiddenField ID="hdnChkAddressline2" runat="server"/>
                <asp:HiddenField ID="hdnChkAddressline3" runat="server"/>
                <asp:HiddenField ID="hdnChkAddressline4" runat="server"/>
                 <asp:HiddenField ID="hdnChkAddressline5" runat="server"/>
                <asp:HiddenField ID="hdAddressLine1MinValue" runat="server" />
                <asp:HiddenField ID="hdAddressLine2MinValue" runat="server" />
                <asp:HiddenField ID="hdAddressLine3MinValue" runat="server" />
                <asp:HiddenField ID="hdAddressLine4MinValue" runat="server" />
                <asp:HiddenField ID="hdAddressLine5MinValue" runat="server" />
                <asp:HiddenField ID="hdnpostCodeMinVal2" runat="server" />
                <asp:HiddenField ID="hdnAddressGroupconfig" runat="server" Value="false" />
                <asp:HiddenField ID="hdnConfigDOB" runat="server" Value="1096" />
                <asp:HiddenField ID="hdnBT" runat="server" Value="false" />
                <asp:HiddenField ID="hdnBT1Exists" runat="server" Value="false" />
                <asp:HiddenField ID="hdnBT2Exists" runat="server" Value="false" />
                <asp:HiddenField ID="hdnBTforAssoc" runat="server" Value="false" />
                <asp:HiddenField ID="hdnUseStatus" runat="server"/>
                <asp:HiddenField ID="hdnUseStatus1" runat="server"/>
 	            <asp:HiddenField ID="hdnMEmailStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAEmailStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMMobileStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAMobileStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMailStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAddressLineFormat" runat="server" Value="" />
                <asp:HiddenField ID="hdnPostcoderegexp" runat="server" Value="" />
                <asp:HiddenField ID="hdnHidepostcodeFields" runat="server"/>
                 <asp:HiddenField ID="hdnDietaryPref" runat="server"  Value="false"/>
                <asp:HiddenField ID="hdnAllergyPref" runat="server"  Value="false"/>
                <asp:HiddenField ID="hdnSendEmailForDietaryPref" runat="server"  Value=""/>
                <asp:HiddenField ID="hdnSendEmailForAllergyPref" runat="server"  Value=""/>
                <asp:HiddenField ID="hdnAllergyPrefList" runat="server"  Value=""/>
                <asp:HiddenField ID="hdnDietaryPrefList" runat="server"  Value=""/>
				<asp:HiddenField ID="hdnISTitle" runat="server" Value="false" />
                <asp:HiddenField ID="hdnIsMiddleName" runat="server" Value="false" />
                <asp:HiddenField ID="hdnIsEvenNumbr" runat="server" Value="false" />
                 <asp:HiddenField ID="hdnFirstName" runat="server" Value="false" />
                 <asp:HiddenField ID="hdnSurName" runat="server" Value="false" />
                 <asp:HiddenField ID="hdnname2validation" runat="server" Value="false" />
                 <asp:HiddenField ID="hdnUSPostCodeFormat" runat="server" Value=""/>
                 <asp:HiddenField ID="hdnphonenumberreg" runat="server" Value=""/>
                 <asp:HiddenField ID="hdnemailreg" runat="server" Value=""/>
                 <asp:HiddenField ID="hdnmiddleinitialreg" runat="server" Value=""/>
                 <asp:HiddenField ID="hdnname1reg" runat="server" Value=""/>
                 <asp:HiddenField ID="hdnname3reg" runat="server" Value=""/>
                 <%--CR13 Changes--%>
                 <asp:HiddenField ID="hdnEditCustomerStatusSettings" runat="server" Value="false" />
                  <asp:HiddenField ID="hdnUpdateCustomerStatusCapability" runat="server" Value="false" />
                 <%--End of CR13 Changes--%>
                 <%--CR11 Changes --%>
                <asp:HiddenField ID="hdnHideBusinessDetails" runat="server" />
                <asp:HiddenField ID="hdnHideBusniessName" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessType" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusniessRegNo" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessAddr1" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessAddr2" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessAddr3" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessAddr4" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessAddr5" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessAddr6" runat="server" Value="true" />
                <asp:HiddenField ID="hdnHideBusinessPostcode" runat="server" Value="true" />
                <asp:HiddenField ID="hdnConfirmMsg" runat="server" Value="false" />

				<%--End of CR11 Changes--%>
            </div>
        </div>
        <div class="saveBtn">
            <asp:ImageButton ID="btnConfirmCustomerDtls1" runat="server" ImageUrl="I/SaveChanges.gif"
                CssClass="saveBtn" AlternateText="Confirm" OnClick="btnConfirmCustomerDtls_Click"  onclientclick="return clearOnConfirm();"
                meta:resourcekey="btnConfirmCustomerDtls1Resource2" />
        </div>
        <asp:HiddenField ID="hdnEmailAddress" runat="server" />
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
            $(".dobyear").change(function() {
                val = $(this).val();
                $(".txtAge").val(val);
            });
        });
    </script>
</asp:Content>
