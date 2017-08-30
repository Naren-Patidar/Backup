<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PromotionCodeReport.aspx.cs" MasterPageFile="~/Site.Master" Title="Promotional Code Report" Inherits="CCODundeeApplication.Reports.PromotionCodeReport" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

<script src="../JS/General.js" type="text/javascript"></script>

    <script src="../JS/CustomerDetails.js" type="text/javascript"></script>
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
          <div class="cc_bluehead">
                 <h3><asp:Localize ID="lclHeader" runat="server" Text="Promotional Code Report" 
                         meta:resourcekey="lclBHeaderResource1"></asp:Localize>
                   </h3>
               
                
            </div>
            <div class="cc_body">
        
            <div class="UserDetails">
                 
                      <span runat="server" id="spanMessageTop"><b><asp:Localize Text="Select input" meta:resourcekey="lclSelectInputResource1" ID="lclSelectInput" runat="server"></asp:Localize></b></span>
                </div>
                            
              
          <b>
               <span runat="server" id="spanMessageBelow">
               <br />
              
                
                <asp:Localize Text="Time frame" meta:resourcekey="lclTimeFrameResource1" ID="lclTimeFrame" runat="server"></asp:Localize>
                <img class="required" src="../I/asterisk.gif" alt="Required" />
                 
                  </span>
                        </b>          
                        
                        
                  <br />
                <br />
                                        
                        
                        <table width="100%">
                        <tr>
                                <td width="1%"> 
                                <asp:RadioButton id="rdoDate" checked="true" runat="server" AutoPostBack="True" EnableViewState="true"
                                        oncheckedchanged="TimeFrame_SelectedIndexChanged" GroupName="TimeFrame"/>
                                </td>
                                <td width="14%">
                                <span runat="server" id="spanStartDateMessage">
                                
                                <asp:Localize ID="lclStartDate" runat="server" Text="Start date" meta:resourcekey="lclStartDateResource1"></asp:Localize>
                                </span>
                                </td>
                                <td width="35%">
                                <span id="spanStartDate0" class="<%=spanClassDOBDropDown0%>">
                                <asp:DropDownList runat="server" ID="ddlStartDay" class="day">
                                    <asp:ListItem Value="0">Day</asp:ListItem>
                                    <asp:ListItem Value="01">1</asp:ListItem>
                                    <asp:ListItem Value="02">2</asp:ListItem>
                                    <asp:ListItem Value="03">3</asp:ListItem>
                                    <asp:ListItem Value="04">4</asp:ListItem>
                                    <asp:ListItem Value="05">5</asp:ListItem>
                                    <asp:ListItem Value="06">6</asp:ListItem>
                                    <asp:ListItem Value="07">7</asp:ListItem>
                                    <asp:ListItem Value="08">8</asp:ListItem>
                                    <asp:ListItem Value="09">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="21">21</asp:ListItem>
                                    <asp:ListItem Value="22">22</asp:ListItem>
                                    <asp:ListItem Value="23">23</asp:ListItem>
                                    <asp:ListItem Value="24">24</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="26">26</asp:ListItem>
                                    <asp:ListItem Value="27">27</asp:ListItem>
                                    <asp:ListItem Value="28">28</asp:ListItem>
                                    <asp:ListItem Value="29">29</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="31">31</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlStartMonth" class="month" />
                                <asp:DropDownList runat="server" ID="ddlStartYear" class="year" />
                            </span>
                                </td>
                                <td width="1%">
                                
                                </td>
                                <td width="14%">
                                 <span runat="server" id="spanEnddateMessage">
                                 <asp:Localize ID="lclEndDate" runat="server" Text="End date" meta:resourcekey="lclEndDateResource1"></asp:Localize>
                                 
                                 </span>
                                </td>
                                <td width="35%">
                                <span id="spanEnddate0" class="<%=spanClassDOBDropDown0%>">
                                <asp:DropDownList runat="server" ID="ddlEndDay" class="day">
                                    <asp:ListItem Value="0">Day</asp:ListItem>
                                    <asp:ListItem Value="01">1</asp:ListItem>
                                    <asp:ListItem Value="02">2</asp:ListItem>
                                    <asp:ListItem Value="03">3</asp:ListItem>
                                    <asp:ListItem Value="04">4</asp:ListItem>
                                    <asp:ListItem Value="05">5</asp:ListItem>
                                    <asp:ListItem Value="06">6</asp:ListItem>
                                    <asp:ListItem Value="07">7</asp:ListItem>
                                    <asp:ListItem Value="08">8</asp:ListItem>
                                    <asp:ListItem Value="09">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="21">21</asp:ListItem>
                                    <asp:ListItem Value="22">22</asp:ListItem>
                                    <asp:ListItem Value="23">23</asp:ListItem>
                                    <asp:ListItem Value="24">24</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="26">26</asp:ListItem>
                                    <asp:ListItem Value="27">27</asp:ListItem>
                                    <asp:ListItem Value="28">28</asp:ListItem>
                                    <asp:ListItem Value="29">29</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="31">31</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlEndMonth" class="month" />
                                <asp:DropDownList runat="server" ID="ddlEndYear" class="year" />
                            </span>
                                </td>
                        </tr>
                        
                        <tr>
                                <td>
                                    &nbsp;</td>
                                <td  colspan="2" style="width: 45%">
                                    <div id="divStartDate"  runat="server"></div></td>
                                <td >
                                
                                    &nbsp;</td>
                                <td  colspan="2" style="width: 45%">
                                     <div id="divEndDate"  runat="server"></div></td>
                        </tr>
                        
                        <tr>
                                <td colspan="6" align="center">
                                   <%-- <div class="UserDetails">--%>
                                        <div id="divComparision" runat="server">
                                        </div>
                                  <%--  </div>--%>
                                </td>
                        </tr>
                        
                          <tr>
                                 <td>
                                 <asp:RadioButton id="rdoWeekNo" runat="server" AutoPostBack="True" EnableViewState="true"
                                 oncheckedchanged="TimeFrame_SelectedIndexChanged" GroupName="TimeFrame"/>
                                </td>
                                <td>
                                 <span runat="server" id="spnWeekNo">
                                  <asp:Localize ID="lclWeekNumber" runat="server" Text="Week number" meta:resourcekey="lclWeekNumberResource1"></asp:Localize>
                                 </span>
                                </td>
                                <td>
                               
                                <asp:DropDownList runat="server" ID="ddlWeek" class="month" />
                                
                                </td>
                                <td>
                                  <asp:RadioButton  id="rdoPeriod" runat="server" AutoPostBack="True" EnableViewState="true"
                                  oncheckedchanged="TimeFrame_SelectedIndexChanged" GroupName="TimeFrame"/>
                                </td>
                                <td>
                                <span runat="server" id="spnPeriod">
                                <asp:Localize ID="lclPeriod" runat="server" Text="Period" meta:resourcekey="lclPeriodResource1"></asp:Localize>
                                </span>
                                </td>
                                <td>
                                <asp:DropDownList runat="server" ID="ddlPeriod" class="month" />
                                </td>
                        </tr>
                        
                          <tr>
                                 <td>
                                
                                </td>
                                <td colspan="2">
                                <div id="divWeek"  runat="server"></div>
                                </td>
                                <td>
                                
                                </td>
                                <td colspan="2">
                                <div id="divPeriod"  runat="server"></div>
                                </td>
                        </tr>
                        </table>
                      
                     <br />
               <table>
               <tr>
               <td><asp:Button ID="btnScheduleRpt" text="Schedule report >" runat="server" 
                       onclick="btnScheduleRpt_Click" OnClientClick="return ValidateDateRegReport();"/></td>
               <td><asp:Button ID="btnRunRpt" text="Run report >" runat="server" 
                       onclick="btnRunRpt_Click" OnClientClick="return ValidateDateRegReport();"/></td>
               </tr>
               </table>
                        
                  </div>
                  
        </div>
    </div>

    
    
    <script type="text/javascript">
        function ValidateDateRegReport() {

            var ddlStartDay = document.getElementById('<%= ddlStartDay.ClientID %>');
            var ddlStartMonth = document.getElementById('<%= ddlStartMonth.ClientID %>');
            var ddlStartYear = document.getElementById('<%= ddlStartYear.ClientID %>');
            var ddlEndDay = document.getElementById('<%= ddlEndDay.ClientID %>');
            var ddlEndMonth = document.getElementById('<%= ddlEndMonth.ClientID %>');
            var ddlEndYear = document.getElementById('<%= ddlEndYear.ClientID %>');
            var ddlWeek = document.getElementById('<%= ddlWeek.ClientID %>');
            var ddlPeriod = document.getElementById('<%= ddlPeriod.ClientID %>');
            var divStartDate = document.getElementById('<%= divStartDate.ClientID %>');
            var divEndDate = document.getElementById('<%= divEndDate.ClientID %>');
            var divComparision = document.getElementById('<%= divComparision.ClientID %>');

            if (document.getElementById('<%= rdoWeekNo.ClientID %>').checked == true) {
                if (ddlWeek.value == "-1") {
                    document.getElementById('<%= divWeek.ClientID %>').innerText = "";
                    document.getElementById('<%= divWeek.ClientID %>').innerText = "Invalid Week";
                    document.getElementById('<%= divWeek.ClientID %>').style.color = "red";
                    return false;
                }

            }
            if (document.getElementById('<%= rdoPeriod.ClientID %>').checked == true) {
                if (ddlPeriod.value == "-1") {
                    document.getElementById('<%= divPeriod.ClientID %>').innerText = "";
                    document.getElementById('<%= divPeriod.ClientID %>').innerText = "Invalid Period";
                    document.getElementById('<%= divPeriod.ClientID %>').style.color = "red";
                    return false;
                }

            }
            if (document.getElementById('<%= rdoDate.ClientID %>').checked == true) {

                document.getElementById('<%= divStartDate.ClientID %>').innerText = "";
                document.getElementById('<%= divEndDate.ClientID %>').innerText = "";
                document.getElementById('<%= divComparision.ClientID %>').innerText = "";

                if (ddlStartDay.value == "0" && ddlStartMonth.value == "- Select Month -" && ddlStartYear.value == "Year" && ddlEndDay.value == "0" && ddlEndMonth.value == "- Select Month -" && ddlEndYear.value == "Year") {
                    document.getElementById('<%= divStartDate.ClientID %>').innerText = "Invalid Start date";
                    document.getElementById('<%= divEndDate.ClientID %>').innerText = "Invalid End date";
                    document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                    document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                    return false;
                }
                else if (ddlStartDay.value != "0" && ddlStartMonth.value != "- Select Month -" && ddlStartYear.value != "Year" && ddlEndDay.value != "0" && ddlEndMonth.value != "- Select Month -" && ddlEndYear.value != "Year") {

                    var errorFlagStartDate = "";
                    var errorFlagEndDate = "";
                    errorFlagStartDate = ValidateDateReport(ddlStartDay.value, ddlStartMonth.value, ddlStartYear.value, "0", true);
                    errorFlagEndDate = ValidateDateReport(ddlEndDay.value, ddlEndMonth.value, ddlEndYear.value, "1", true);

                    if (errorFlagStartDate != "" && errorFlagEndDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = "Invalid Start date";
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = "Invalid End date";
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                        return false;
                    }
                    if (errorFlagStartDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = "Invalid Start date";
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        return false;
                    }

                    if (errorFlagEndDate != "") {
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = "Invalid End date";
                        document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                        return false;
                    }
                    if (errorFlagStartDate == "" && errorFlagEndDate == "") {

                        var startDay = document.getElementById('<%= ddlStartDay.ClientID %>').value;
                        var startMonth = document.getElementById('<%= ddlStartMonth.ClientID %>').value;
                        var startYear = document.getElementById('<%= ddlStartYear.ClientID %>').value;

                        var endDay = document.getElementById('<%= ddlEndDay.ClientID %>').value;
                        var endMonth = document.getElementById('<%= ddlEndMonth.ClientID %>').value;
                        var endYear = document.getElementById('<%= ddlEndYear.ClientID %>').value;

                        var FromDate = new Date(startYear, startMonth - 1, startDay);
                        var ToDate = new Date(endYear, endMonth - 1, endDay);

                        var today = new Date();
                        var dd = today.getDate();
                        var mm = today.getMonth(); //January is 0!
                        var yyyy = today.getFullYear();

                        var varCurrentDate = new Date(yyyy, mm, dd);
                        var dayDiff = days_between(FromDate, ToDate);
                        if (dayDiff > 42) {
                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "Date Difference Shouldn't Be More Than 42 Days";
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }
                        if (FromDate > ToDate) {

                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "End date should be greater than Start date";
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (ToDate > varCurrentDate) {
                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "End date should not be greater than Today date";
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (FromDate > varCurrentDate) {
                            document.getElementById('<%= divStartDate.ClientID %>').innerText = "Start date should not be greater than Today date";
                            document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }


                        else {
                            return true;
                        }



                    }
                    else {
                        return true;
                    }
                }
                else if (ddlStartDay.value != "0" && ddlStartMonth.value != "- Select Month -" && ddlStartYear.value != "Year" && ddlEndDay.value == "0" && ddlEndMonth.value == "- Select Month -" && ddlEndYear.value == "Year") {
                    document.getElementById('<%= divEndDate.ClientID %>').innerText = "Invalid End date";
                    document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                    return false;
                }
                else if (ddlStartDay.value == "0" && ddlStartMonth.value == "- Select Month -" && ddlStartYear.value == "Year" && ddlEndDay.value != "0" && ddlEndMonth.value != "- Select Month -" && ddlEndYear.value != "Year") {
                    document.getElementById('<%= divStartDate.ClientID %>').innerText = "Invalid Start date";
                    document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                    return false;
                }
                else {

                    var errorFlagStartDate = "";
                    var errorFlagEndDate = "";
                    errorFlagStartDate = ValidateDateReport(ddlStartDay.value, ddlStartMonth.value, ddlStartYear.value, "0", true);
                    errorFlagEndDate = ValidateDateReport(ddlEndDay.value, ddlEndMonth.value, ddlEndYear.value, "1", true);

                    if (errorFlagStartDate != "" && errorFlagEndDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = "Invalid Start date";
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = "Invalid End date";
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                        return false;
                    }
                    if (errorFlagStartDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = "Invalid Start date";
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        return false;
                    }

                    if (errorFlagEndDate != "") {
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = "Invalid End date";
                        document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                        return false;
                    }
                    if (errorFlagStartDate == "" && errorFlagEndDate == "") {

                        var startDay = document.getElementById('<%= ddlStartDay.ClientID %>').value;
                        var startMonth = document.getElementById('<%= ddlStartMonth.ClientID %>').value;
                        var startYear = document.getElementById('<%= ddlStartYear.ClientID %>').value;

                        var endDay = document.getElementById('<%= ddlEndDay.ClientID %>').value;
                        var endMonth = document.getElementById('<%= ddlEndMonth.ClientID %>').value;
                        var endYear = document.getElementById('<%= ddlEndYear.ClientID %>').value;

                        var FromDate = new Date(startYear, startMonth - 1, startDay);
                        var ToDate = new Date(endYear, endMonth - 1, endDay);

                        var today = new Date();
                        var dd = today.getDate();
                        var mm = today.getMonth(); //January is 0!
                        var yyyy = today.getFullYear();

                        var varCurrentDate = new Date(yyyy, mm, dd);
                        var dayDiff = days_between(FromDate, ToDate);
                        if (dayDiff > 42) {
                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "Date Difference Shouldn't Be More Than 42 Days";
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }
                        if (FromDate > ToDate) {

                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "End date should be greater than Start date";
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (ToDate > varCurrentDate) {
                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "End date should not be greater than Today date";
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (FromDate > varCurrentDate) {
                            document.getElementById('<%= divStartDate.ClientID %>').innerText = "Start date should not be greater than Today date";
                            document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }


                        else {
                            return true;
                        }



                    }
                    else {
                        return true;
                    }
                }
            }


        }

        function ValidateDateReport(dayVal, monthVal, yearVal, ID, isRequired) {
            var daysInMonth = DaysArray(12);
            var day = dayVal;
            var month = monthVal;
            var year = yearVal;
            var invalidDate = false;
            if ((day == "" && (month == "" || month == "- Select Month -") && (year == "" || year == "Year"))) {
                if (isRequired == true)
                    invalidDate = true;
            }
            else if ((day == "" || (month == "" || month == "- Select Month -") || (year == "" || year == "Year"))) {
                invalidDate = true;
            }
            else if ((month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month])
                invalidDate = true;

            if (invalidDate) {

                return "Error";
            }

            else {


                return "";
            }

        }
        function days_between(date1, date2) {

            // The number of milliseconds in one day
            var ONE_DAY = 1000 * 60 * 60 * 24

            // Convert both dates to milliseconds
            var date1_ms = date1.getTime()
            var date2_ms = date2.getTime()

            // Calculate the difference in milliseconds
            var difference_ms = Math.abs(date1_ms - date2_ms)

            // Convert back to days and return
            return Math.round(difference_ms / ONE_DAY)

        }
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style1
        {
            height: 13px;
        }
    </style>

</asp:Content>