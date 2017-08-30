<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="PrintVouchersAtKiosk.Verification" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector" />
    <title></title>
    <script type="text/javascript" language="javascript">


        function EnableNext(TextBoxName, TextBoxSlNo, TextValueIsOptional) {

            var dateFormat; var dateControl1; var dateControl2; var dateControl3;
            txtboxname = document.getElementById(TextBoxName).value;
            document.getElementById('lblCurrentField').value = TextBoxSlNo;

            if (txtboxname.length >= 1) 
            {
                if (TextBoxSlNo == 8) 
                {
                    dateFormat = '<%= ConfigurationSettings.AppSettings["DateFormat"].ToString().ToLower() %>';
                    dateControl1 = document.getElementById("txtDOB" + dateFormat).value;
                    dateControl2 = document.getElementById("txtmonth" + dateFormat).value;
                    dateControl3 = document.getElementById("txtyear" + dateFormat).value;

                    if ((isNaN(dateControl1) || isNaN(dateControl2) || isNaN(dateControl3)) && TextValueIsOptional=="false") 
                    {
                        document.getElementById('lnkConfirm').disabled = "disabled";
                        document.getElementById('pnlConfirm').className = 'confirm inactive';
                    }
                    else 
                    {
                        document.getElementById('lnkConfirm').disabled = "";
                        document.getElementById('pnlConfirm').className = 'confirm';
                    }
                }
                else 
                {
                    document.getElementById('lnkConfirm').disabled = "";
                    document.getElementById('pnlConfirm').className = 'confirm';
                }
            }
            else {
                if (TextValueIsOptional == "false") {
                    document.getElementById('lnkConfirm').disabled = "disabled";
                    document.getElementById('pnlConfirm').className = 'confirm inactive';
                }
            }
        }

        function Focus_DateControl(ctrl) {

            document.getElementById(ctrl).select();
        }

        function Focus_FirstControl(ctrl) {
            
            document.getElementById(ctrl).select();
            EnableNext(ctrl, '8', '<%= ConfigurationSettings.AppSettings["IsDateOfBirthOptional"].ToString().ToLower() %>');
        }
        function isNumericKey(e) {
            var filter = /[0-9]/;
            var code = window.event.keyCode;
            var keyPressed = String.fromCharCode(code);
            document.getElementById('lblCharCode').value = keyPressed;
            if (!filter.test(keyPressed)) {
                return false;
            }
            return true;
        }

    </script>
</head>
<body style="overflow-y:hidden;">
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="hdnDateFormat" />
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" meta:resourcekey="imgTescoClubcardLogoResource1"/>
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" Text="Print out your Clubcard Vouchers"
                        meta:resourcekey="lblVerificationHeaderTextResource1"></asp:Label></em>
                        </div>
           <%-- <div id="breadcrumbs">
                <img src="images/crumb2.png" width="1152" height="45"/>
            </div>  --%>   
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgbreadcrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgbreadcrumbsResource1" />
            </div>
        </div>
        <div id="body_wrapper">
            <div id="contentVerify">
             <asp:HiddenField  ID="lblCurrentField"  runat="server"/>
                <p style="padding-top: 5px;">
                </p>
                <div style="width: 100%;">
                    <div id="divFirstName" style="float: left;" runat="server">
                        <asp:Panel runat="server" ID="pnlFirstName1" 
                            meta:resourcekey="pnlFirstName1Resource1">
                            <div class="titletext Genericlabel" id="divHouse2" runat="server">
                                <asp:Label ID="lblFirstName1" runat="server" Text="First Name" 
                                    meta:resourcekey="lblFirstName1Resource1"></asp:Label>&nbsp;&nbsp;
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlFirstName" runat="server" 
                            CssClass="inputboxes addressboxwhite" meta:resourcekey="pnlFirstNameResource1">
                            <div class="textbox" onkeyup="EnableNext('txtFirstName',1,'<%= ConfigurationSettings.AppSettings["IsFirstNameOptional"].ToString().ToLower() %>')">
                                <asp:TextBox runat="server" CssClass="text" AutoComplete="off" ID="txtFirstName" TabIndex="3" 
                                    Visible="False" meta:resourcekey="txtFirstNameResource1"></asp:TextBox>
                                <asp:Label ID="lblFirstName" runat="server" CssClass="text" Visible="False" 
                                    meta:resourcekey="lblFirstNameResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="divLastName" style="float: left;" runat="server">
                        <asp:Panel runat="server" ID="pnlLastName1" 
                            meta:resourcekey="pnlLastName1Resource1">
                            <div class="titletext Genericlabel" id="div3" runat="server">                   
                                <asp:Label ID="lbllLastName1"  runat="server" Text="Last Name" 
                                    meta:resourcekey="lbllLastName1Resource1"></asp:Label>&nbsp;&nbsp;
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlLastName" runat="server" 
                            CssClass="inputboxes addressboxwhite" meta:resourcekey="pnlLastNameResource1">
                            <div class="textbox" onkeyup="EnableNext('txtLastName',2,'<%= ConfigurationSettings.AppSettings["IsLastNameOptional"].ToString() %>')">
                                <asp:TextBox runat="server" CssClass="text" AutoComplete="off" ID="txtLastName" TabIndex="3" 
                                    Visible="False" meta:resourcekey="txtLastNameResource1"></asp:TextBox>
                                <asp:Label ID="lblLastName" runat="server" CssClass="text" Visible="False" 
                                    meta:resourcekey="lblLastNameResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="divEmail" style="float: left;" runat="server">
                        <span class="titletext Genericlabel">
                            <asp:Label ID="lblEmail1" runat="server" Text="E-mail Id" 
                            meta:resourcekey="lblEmail1Resource1" style="overflow:hidden;" ></asp:Label>&nbsp;&nbsp;</span>
                        <asp:Panel ID="pnlEmail" runat="server" CssClass="inputboxes addressboxwhite" 
                            meta:resourcekey="pnlEmailResource1">
                            <div class="textbox" onkeyup="EnableNext('txtEmail',3,'<%= ConfigurationSettings.AppSettings["IsEmailOptional"].ToString().ToLower() %>')">
                                <asp:TextBox runat="server" CssClass="text" ID="txtEmail" AutoComplete="off" type="text" MaxLength="50"
                                    TabIndex="2" Visible="False" meta:resourcekey="txtEmailResource1" />
                                <asp:Label ID="lblEmail" runat="server" CssClass="text" style="" Visible="False" 
                                    meta:resourcekey="lblEmailResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="divHouseNumber" style="float: left;" runat="server">
                        <asp:Panel runat="server" ID="pnlHouseNo1" 
                            meta:resourcekey="pnlHouseNo1Resource1">
                            <div class="titletext Genericlabel" id="div6" runat="server">
                                <asp:Label ID="lblHouseNo1" runat="server" Text="House Number" 
                                    meta:resourcekey="lblHouseNo1Resource1"></asp:Label>&nbsp;&nbsp;
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlHouseNo" runat="server" CssClass="inputboxes addressboxwhite" 
                            meta:resourcekey="pnlHouseNoResource1">
                            <div class="textbox" onkeyup="EnableNext('txtHouseNo',4,'<%= ConfigurationSettings.AppSettings["IsHouseNumberOptional"].ToString().ToLower() %>')">
                                <asp:TextBox runat="server" CssClass="text" ID="txtHouseNo" AutoComplete="off" TabIndex="3" 
                                    Visible="False" meta:resourcekey="txtHouseNoResource1"></asp:TextBox>
                                <asp:Label ID="lblHouseNo" runat="server" CssClass="text" Visible="False" 
                                    meta:resourcekey="lblHouseNoResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="divPostcode" style="float: left;" runat="server">
                        <asp:Panel runat="server" ID="pnlPostcode1" 
                            meta:resourcekey="pnlPostcode1Resource1">
                            <div class="titletext Genericlabel" id="div10" runat="server">
                                <asp:Label ID="lblPostcode1" runat="server" Text="Post Code" 
                                    meta:resourcekey="lblPostcode1Resource1"></asp:Label>&nbsp;&nbsp;
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlPostcode" runat="server" 
                            CssClass="inputboxes addressboxwhite" meta:resourcekey="pnlPostcodeResource1">
                            <div class="textbox" onkeyup="EnableNext('txtPostcode',5,'<%= ConfigurationSettings.AppSettings["IsPostCodeOptional"].ToString().ToLower() %>')">
                                <asp:TextBox runat="server" CssClass="text" ID="txtPostcode" AutoComplete="off" TabIndex="3" 
                                    Visible="False" meta:resourcekey="txtPostcodeResource1"></asp:TextBox>
                                <asp:Label ID="lblPostcode" runat="server" CssClass="text" Visible="False" 
                                    meta:resourcekey="lblPostcodeResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="divPhoneNumber" style="float: left;" runat="server">
                        <asp:Panel runat="server" ID="Panel1" meta:resourcekey="Panel1Resource1">
                            <div class="titletext Genericlabel" id="div1" runat="server">
                                <asp:Label ID="lblPhoneNumber1" runat="server" Text="Mobile Number" 
                                    meta:resourcekey="lblPhoneNumber1Resource1"></asp:Label>&nbsp;&nbsp;</div>
                        </asp:Panel>
                        <asp:Panel ID="pnlPhoneNumber" runat="server" 
                            CssClass="inputboxes addressboxwhite" 
                            meta:resourcekey="pnlPhoneNumberResource1">
                            <div class="textbox" onkeyup="EnableNext('txtPhoneNumber',6, '<%= ConfigurationSettings.AppSettings["IsPhoneNumberOptional"].ToString().ToLower() %>')">
                                <asp:TextBox runat="server" CssClass="text" ID="txtPhoneNumber" TabIndex="3" AutoComplete="off" 
                                    Visible="False" meta:resourcekey="txtPhoneNumberResource1"></asp:TextBox>
                                <asp:Label ID="lblPhoneNumber" runat="server" CssClass="text" Visible="False" 
                                    meta:resourcekey="lblPhoneNumberResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                    <div id="divSSN" style="float: left;" runat="server">
                        <span class="titletext Genericlabel">
                            <asp:Label ID="lblSSN1" runat="server" Text="SSN" 
                            meta:resourcekey="lblSSN1Resource1"></asp:Label>&nbsp;&nbsp;</span>
                        <asp:Panel ID="pnlSSN" runat="server" CssClass="inputboxes addressboxwhite" 
                            meta:resourcekey="pnlSSNResource1">
                            <div class="textbox" onkeyup="EnableNext('txtSSN',7, '<%= ConfigurationSettings.AppSettings["IsSSNOptional"].ToString().ToLower() %>')">
                                <asp:TextBox runat="server" CssClass="text" ID="txtSSN" type="text" AutoComplete="off" 
                                    TabIndex="2" Visible="False" meta:resourcekey="txtSSNResource1" />
                                <asp:Label ID="lblSSN" runat="server" CssClass="text" Visible="False" 
                                    meta:resourcekey="lblSSNResource1"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                   
                     <div id="divDateOfBirth" style="float: left;" runat="server">
                        <asp:Panel runat="server" ID="pnlDOB1" meta:resourcekey="pnlDOB1Resource1">
                            <div class="titletext Genericlabel" id="div4" runat="server">
                                <asp:Label ID="lblDOB1" runat="server" Text="Date of Birth" 
                                    meta:resourcekey="lblDOB1Resource1"></asp:Label>&nbsp;&nbsp;
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlDOB" runat="server" 
                            meta:resourcekey="pnlDOBResource1">
                            
                          <% var dateFormat = hdnDateFormat.Value;
                               if (dateFormat.ToString().ToUpper() == "YMD")
                               {
                                 %>
                        <div id ="ymd" runat="server"  >
                        <div id="divyearymd" runat="server" class="input92 paddingtop-10 input116grey" style="padding-top: 8px;padding-left: 5px;">
                            <div class="textbox" >
                                <asp:TextBox ID="txtyearymd" runat="server" MaxLength="4" AutoComplete="off" class="text" meta:resourcekey="txtyearResource1" Text="" Font-Size="X-Large" Visible="False" onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtyearymd');"  />
                                <asp:Label ID="lblyearymd" runat="server" class="text" Visible="True"  meta:resourcekey="lblyearResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divmonthymd" runat="server" class="input92 paddingtop-10 input92grey">
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtmonthymd" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtmonthResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtmonthymd');"  />
                                <asp:Label ID="lblmonthymd" runat="server" class="text" Visible="True"  meta:resourcekey="lblmonthResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divDOBymd" runat="server" class="input92 paddingtop-10 input92grey" >
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtDOBymd" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtDOBResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtDOBymd');" />
                                <asp:Label ID="lblDOBymd" runat="server" class="text" Visible="True"  meta:resourcekey="lbDAYResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        </div>
                        <% }
                            if (dateFormat.ToString().ToUpper() == "YDM")
                            {%>
                         <div id ="ydm"  runat="server" >

                        <div id="divyearydm" runat="server" class="input92 paddingtop-10 input116grey" style="padding-top: 8px;padding-left: 5px;">
                            <div class="textbox" >
                                <asp:TextBox ID="txtyearydm" runat="server" MaxLength="4" class="text" AutoComplete="off" meta:resourcekey="txtyearResource1" Text="" Font-Size="X-Large" Visible="False" onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtyearydm');"  />
                                <asp:Label ID="lblyearydm" runat="server" class="text" Visible="True"  meta:resourcekey="lblyearResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                         <div id="divDOBydm" runat="server" class="input92 paddingtop-10 input92grey" >
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtDOBydm" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtDOBResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtDOBydm');"  />
                                <asp:Label ID="lblDOBydm" runat="server" class="text" Visible="True"  meta:resourcekey="lbDAYResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divmonthydm" runat="server" class="input92 paddingtop-10 input92grey">
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtmonthydm" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtmonthResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtmonthydm');"  />
                                <asp:Label ID="lblmonthydm" runat="server" class="text" Visible="True"  meta:resourcekey="lblmonthResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                       
                        </div>
                        <%} 
                            if (dateFormat.ToString().ToUpper() == "MDY")
                            {%>
                         <div id ="mdy"  runat="server"  >                        
                        <div id="divmonthmdy" runat="server" class="input92 paddingtop-10 input92grey">
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtmonthmdy" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtmonthResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtmonthmdy');" />
                                <asp:Label ID="lblmonthmdy" runat="server" class="text" Visible="True"  meta:resourcekey="lblmonthResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divDOBmdy" runat="server" class="input92 paddingtop-10 input92grey" >
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtDOBmdy" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtDOBResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtDOBmdy');"/>
                                <asp:Label ID="lblDOBmdy" runat="server" class="text" Visible="True"  meta:resourcekey="lbDAYResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divyearmdy" runat="server" class="input92 paddingtop-10 input116grey" style="padding-top: 8px;padding-left: 5px;">
                            <div class="textbox" >
                                <asp:TextBox ID="txtyearmdy" runat="server" MaxLength="4" class="text" AutoComplete="off" meta:resourcekey="txtyearResource1" Text="" Font-Size="X-Large" Visible="False" onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtyearmdy');" />
                                <asp:Label ID="lblyearmdy" runat="server" class="text" Visible="True"  meta:resourcekey="lblyearResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        </div>
                         <%}
                             if (dateFormat.ToString().ToUpper() == "DMY")
                             {%>
                         <div id ="dmy"  runat="server">
                        <div id="divDOBdmy" runat="server" class="input92 paddingtop-10 input92grey" >
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtDOBdmy" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtDOBResource1" Text="" Font-Size="X-Large" Visible="False" onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtDOBdmy');" />
                                <asp:Label ID="lblDOBdmy" runat="server" class="text" Visible="True"  meta:resourcekey="lbDAYResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divmonthdmy" runat="server" class="input92 paddingtop-10 input92grey">
                            <div class="textbox"  style="padding-top: 30px;" >
                                <asp:TextBox ID="txtmonthdmy" runat="server" MaxLength="2" class="text" AutoComplete="off" meta:resourcekey="txtmonthResource1" Text="" Font-Size="X-Large" Visible="False" onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtmonthdmy');" />
                                <asp:Label ID="lblmonthdmy" runat="server" class="text" Visible="True"  meta:resourcekey="lblmonthResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div id="divyeardmy" runat="server" class="input92 paddingtop-10 input116grey" style="padding-top: 8px;padding-left: 5px;">
                            <div class="textbox" >
                                <asp:TextBox ID="txtyeardmy" runat="server" MaxLength="4" class="text" AutoComplete="off" meta:resourcekey="txtyearResource1" Text="" Font-Size="X-Large" Visible="False"  onkeypress="return isNumericKey(event);" onFocus="Focus_DateControl('txtyeardmy');"/>
                                <asp:Label ID="lblyeardmy" runat="server" class="text" Visible="True"  meta:resourcekey="lblyearResource1" Text="" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        </div>
                        <%} %>
                       <div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                        </asp:Panel>
                    </div>
                    <div class="buttons" style="width: 1170px">
                        <div id="divBack" runat="server" class="back">
                            <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" 
                                meta:resourcekey="lnkBackResource1"><div>Back</div></asp:LinkButton>
                        </div>
                        <div class="greybtn">
                            <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" 
                                meta:resourcekey="lnkCancelResource1"><span class="cancelStart">CANCEL</span><span class="cancelStartagain">and start again</span></asp:LinkButton>
                        </div>
                        <div class="greybtn">
                <asp:LinkButton ID="lnkTerms" runat="server" OnClick="lnkTerms_Click" meta:resourcekey="lnkTermsResource1"
                    Text=""></asp:LinkButton>
            </div>
                        <asp:Panel ID="pnlConfirm" runat="server" CssClass="confirm inactive">
                            <asp:LinkButton ID="lnkConfirm" runat="server" OnClick="lnkConfirm_Click" 
                                TabIndex="4" ><span runat="server" id="spanNEXT"></span>
                        </asp:LinkButton>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <!--body_wrapper-->
            <div class="keyboard">
                <script type="text/javascript" language="javascript">
                    window.external.showKeyboardTemplate(1);
                </script>
            </div>
            <label id="lblCharCode" style="display:none;"></label>
        </div>
        <!--wrapper-->
    </div>
    </form>
</body>
</html>
