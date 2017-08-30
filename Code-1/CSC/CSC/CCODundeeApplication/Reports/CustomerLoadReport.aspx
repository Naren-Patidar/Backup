
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerLoadReport.aspx.cs"  MasterPageFile="~/Site.Master" Inherits="CCODundeeApplication.Reports.CustomerLoadReport" Title="Customer Load Report" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"  %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

<script src="../JS/General.js" type="text/javascript"></script>
<script src="../JS/CustomerDetails.js" type="text/javascript"></script>
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
                    document.getElementById('<%= divWeek.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidWeek %>'; //"Invalid Week"; //InvalidWeek
                    document.getElementById('<%= divWeek.ClientID %>').style.color = "red";
                    return false;
                }

            }
            if (document.getElementById('<%= rdoPeriod.ClientID %>').checked == true) {
                if (ddlPeriod.value == "-1") {
                    document.getElementById('<%= divPeriod.ClientID %>').innerText = "";
                    document.getElementById('<%= divPeriod.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidPeriod %>'//"Invalid Period"; //InvalidPeriod
                    document.getElementById('<%= divPeriod.ClientID %>').style.color = "red";
                    return false;
                }

            }
            if (document.getElementById('<%= rdoDate.ClientID %>').checked == true) {

                document.getElementById('<%= divStartDate.ClientID %>').innerText = "";
                document.getElementById('<%= divEndDate.ClientID %>').innerText = "";
                document.getElementById('<%= divComparision.ClientID %>').innerText = "";

                if (ddlStartDay.value == "0" && ddlStartMonth.value == "- Select Month -" && ddlStartYear.value == "Year" && ddlEndDay.value == "0" && ddlEndMonth.value == "- Select Month -" && ddlEndYear.value == "Year") {
                    document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidWeek %>'//"Invalid Start date"; //InvalidStartdate
                    document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidWeek %>'//"Invalid End date"; //InvalidEnddate
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

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidStartdate %>'; //"Invalid Start date"; //InvalidStartdate
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidEnddate %>'; //"Invalid End date"; //InvalidEnddate
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                        return false;
                    }
                    if (errorFlagStartDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidStartdate %>'; //"Invalid Start date"; //InvalidStartdate
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        return false;
                    }

                    if (errorFlagEndDate != "") {
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidEnddate %>'; //"Invalid End date"; //InvalidEnddate
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
//                        var dayDiff = days_between(FromDate, ToDate);
//                        if (dayDiff > 42) {
//                            document.getElementById('<%= divEndDate.ClientID %>').innerText = "Date Difference Shouldn't Be More Than 42 Days";
//                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
//                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
//                            return false;
//                        }
                        if (FromDate > ToDate) {

                            document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.EnddateshouldbegreaterthanStartdate %>';  //"End date should be greater than Start date"; //EnddateshouldbegreaterthanStartdate
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (ToDate > varCurrentDate) {
                            document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.EnddategreaterthanTodaydate %>'; //"End date should not be greater than Today date"; //EnddategreaterthanTodaydate
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (FromDate > varCurrentDate) {
                            document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.StartdateshouldnotbegreaterthanTodaydate %>'; //"Start date should not be greater than Today date"; //StartdateshouldnotbegreaterthanTodaydate
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
                document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidEnddate %>'; //"Invalid End date"; //InvalidEnddate
                    document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                    return false;
                }
                else if (ddlStartDay.value == "0" && ddlStartMonth.value == "- Select Month -" && ddlStartYear.value == "Year" && ddlEndDay.value != "0" && ddlEndMonth.value != "- Select Month -" && ddlEndYear.value != "Year") {
                document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidStartdate %>'; //"Invalid Start date"; //InvalidStartdate
                    document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                    return false;
                }
                else {

                    var errorFlagStartDate = "";
                    var errorFlagEndDate = "";
                    errorFlagStartDate = ValidateDateReport(ddlStartDay.value, ddlStartMonth.value, ddlStartYear.value, "0", true);
                    errorFlagEndDate = ValidateDateReport(ddlEndDay.value, ddlEndMonth.value, ddlEndYear.value, "1", true);

                    if (errorFlagStartDate != "" && errorFlagEndDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidStartdate %>'; //"Invalid Start date"; //InvalidStartdate
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidEnddate %>'; //"Invalid End date"; //InvalidEnddate
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                        return false;
                    }
                    if (errorFlagStartDate != "") {

                        document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidStartdate %>'; //"Invalid Start date"; //InvalidStartdate
                        document.getElementById('<%= divStartDate.ClientID %>').style.color = "red";
                        return false;
                    }

                    if (errorFlagEndDate != "") {
                        document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.InvalidEnddate %>'; //"Invalid End date"; //InvalidEnddate
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
                      
                        if (FromDate > ToDate) {

                            document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.EnddateshouldbegreaterthanStartdate %>'; //"End date should be greater than Start date"; //EnddateshouldbegreaterthanStartdate
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (ToDate > varCurrentDate) {
                            document.getElementById('<%= divEndDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.EnddategreaterthanTodaydate %>'; //"End date should not be greater than Today date"; //EnddategreaterthanTodaydate
                            document.getElementById('<%= divEndDate.ClientID %>').style.color = "red";
                            document.getElementById('<%= divStartDate.ClientID %>').style.bold = true;
                            return false;
                        }

                        if (FromDate > varCurrentDate) {
                            document.getElementById('<%= divStartDate.ClientID %>').innerText = '<%=Resources.CSCGlobal.StartdateshouldnotbegreaterthanTodaydate %>'; //"Start date should not be greater than Today date"; //StartdateshouldnotbegreaterthanTodaydate
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
    
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                 <h3><asp:Localize ID="lclHeader" runat="server" Text="Customer Load Report" 
                         meta:resourcekey="lclHeaderResource1"></asp:Localize>
                   </h3>
               
                
            </div>
            <div class="cc_body">
        
            <div class="UserDetails">
                 
                      <span runat="server" id="spanMessageTop"><b><asp:Localize ID="lclSelectInput" 
                          runat="server" Text="Select input" meta:resourcekey="lclSelectInputResource1"></asp:Localize></b></span>
                </div>
                            
              
          <b>
               <span runat="server" id="spanMessageBelow">
               <br />
              
                <asp:Localize ID="lclTimeFrame" runat="server" Text="Time frame" 
                    meta:resourcekey="lclTimeFrameResource1"></asp:Localize>
                
                <img class="required" src="../I/asterisk.gif" alt="Required" />
                 
                  </span>
                        </b>          
                        
                        
                  <br />
                <br />
                <table width="100%">
                        <tr>
                                <td width="1%"> 
                                <asp:RadioButton id="rdoDate" checked="True" runat="server" AutoPostBack="True"
                                        oncheckedchanged="TimeFrame_SelectedIndexChanged" GroupName="TimeFrame" 
                                        meta:resourcekey="rdoDateResource1"/>
                                </td>
                                <td width="14%">
                                <span runat="server" id="spanStartDateMessage">
                                 <asp:Localize ID="lclStartDate" runat="server" Text="Start date" 
                                        meta:resourcekey="lclStartDateResource1"></asp:Localize>
                                </span>
                                </td>
                                <td width="35%">
                                <span id="spanStartDate0" class="<%=spanClassDOBDropDown0%>">
                                <asp:DropDownList runat="server" ID="ddlStartDay" class="day" 
                                        meta:resourcekey="ddlStartDayResource1">
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
                                <asp:DropDownList runat="server" ID="ddlStartMonth" class="month" 
                                        meta:resourcekey="ddlStartMonthResource1" />
                                <asp:DropDownList runat="server" ID="ddlStartYear" class="year" 
                                        meta:resourcekey="ddlStartYearResource1" />
                            </span>
                                </td>
                                <td width="1%">
                                
                                </td>
                                <td width="14%">
                                 <span runat="server" id="spanEnddateMessage">
                                  <asp:Localize ID="lclEndDate" runat="server" Text="End date" 
                                        meta:resourcekey="lclEndDateResource1"></asp:Localize> </span>
                                </td>
                                <td width="35%">
                                <span id="spanEnddate0" class="<%=spanClassDOBDropDown0%>">
                                <asp:DropDownList runat="server" ID="ddlEndDay" class="day" 
                                        meta:resourcekey="ddlEndDayResource1">
                                    <asp:ListItem Value="" meta:resourcekey="ListItemResource33">Day</asp:ListItem>
                                    <asp:ListItem Value="01" meta:resourcekey="ListItemResource34">1</asp:ListItem>
                                    <asp:ListItem Value="02" meta:resourcekey="ListItemResource35">2</asp:ListItem>
                                    <asp:ListItem Value="03" meta:resourcekey="ListItemResource36">3</asp:ListItem>
                                    <asp:ListItem Value="04" meta:resourcekey="ListItemResource37">4</asp:ListItem>
                                    <asp:ListItem Value="05" meta:resourcekey="ListItemResource38">5</asp:ListItem>
                                    <asp:ListItem Value="06" meta:resourcekey="ListItemResource39">6</asp:ListItem>
                                    <asp:ListItem Value="07" meta:resourcekey="ListItemResource40">7</asp:ListItem>
                                    <asp:ListItem Value="08" meta:resourcekey="ListItemResource41">8</asp:ListItem>
                                    <asp:ListItem Value="09" meta:resourcekey="ListItemResource42">9</asp:ListItem>
                                    <asp:ListItem Value="10" meta:resourcekey="ListItemResource43">10</asp:ListItem>
                                    <asp:ListItem Value="11" meta:resourcekey="ListItemResource44">11</asp:ListItem>
                                    <asp:ListItem Value="12" meta:resourcekey="ListItemResource45">12</asp:ListItem>
                                    <asp:ListItem Value="13" meta:resourcekey="ListItemResource46">13</asp:ListItem>
                                    <asp:ListItem Value="14" meta:resourcekey="ListItemResource47">14</asp:ListItem>
                                    <asp:ListItem Value="15" meta:resourcekey="ListItemResource48">15</asp:ListItem>
                                    <asp:ListItem Value="16" meta:resourcekey="ListItemResource49">16</asp:ListItem>
                                    <asp:ListItem Value="17" meta:resourcekey="ListItemResource50">17</asp:ListItem>
                                    <asp:ListItem Value="18" meta:resourcekey="ListItemResource51">18</asp:ListItem>
                                    <asp:ListItem Value="19" meta:resourcekey="ListItemResource52">19</asp:ListItem>
                                    <asp:ListItem Value="20" meta:resourcekey="ListItemResource53">20</asp:ListItem>
                                    <asp:ListItem Value="21" meta:resourcekey="ListItemResource54">21</asp:ListItem>
                                    <asp:ListItem Value="22" meta:resourcekey="ListItemResource55">22</asp:ListItem>
                                    <asp:ListItem Value="23" meta:resourcekey="ListItemResource56">23</asp:ListItem>
                                    <asp:ListItem Value="24" meta:resourcekey="ListItemResource57">24</asp:ListItem>
                                    <asp:ListItem Value="25" meta:resourcekey="ListItemResource58">25</asp:ListItem>
                                    <asp:ListItem Value="26" meta:resourcekey="ListItemResource59">26</asp:ListItem>
                                    <asp:ListItem Value="27" meta:resourcekey="ListItemResource60">27</asp:ListItem>
                                    <asp:ListItem Value="28" meta:resourcekey="ListItemResource61">28</asp:ListItem>
                                    <asp:ListItem Value="29" meta:resourcekey="ListItemResource62">29</asp:ListItem>
                                    <asp:ListItem Value="30" meta:resourcekey="ListItemResource63">30</asp:ListItem>
                                    <asp:ListItem Value="31" meta:resourcekey="ListItemResource64">31</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlEndMonth" class="month" 
                                        meta:resourcekey="ddlEndMonthResource1" />
                                <asp:DropDownList runat="server" ID="ddlEndYear" class="year" 
                                        meta:resourcekey="ddlEndYearResource1" />
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
                                        <div id="div1" runat="server">
                                        </div>
                                  <%--  </div>--%>
                                </td>
                        </tr>
                        
                          <tr>
                                 <td>
                                 <asp:RadioButton id="rdoWeekNo" runat="server" AutoPostBack="True"
                                 oncheckedchanged="TimeFrame_SelectedIndexChanged" GroupName="TimeFrame" 
                                         meta:resourcekey="rdoWeekNoResource1"/>
                                </td>
                                <td>
                                 <span runat="server" id="spnWeekNo">
                                 <asp:Localize ID="lclWeekNumber" runat="server" Text="Week number" 
                                        meta:resourcekey="lclWeekNumberResource1"></asp:Localize>
                                 </span>
                                </td>
                                <td>
                               
                                <asp:DropDownList runat="server" ID="ddlWeek" class="month" 
                                        meta:resourcekey="ddlWeekResource1" />
                                
                                </td>
                                <td>
                                  <asp:RadioButton  id="rdoPeriod" runat="server" AutoPostBack="True"
                                  oncheckedchanged="TimeFrame_SelectedIndexChanged" GroupName="TimeFrame" 
                                        meta:resourcekey="rdoPeriodResource1"/>
                                </td>
                                <td>
                                <span runat="server" id="spnPeriod">
                                  <asp:Localize ID="lclPeriod" runat="server" Text="Period" 
                                        meta:resourcekey="lclPeriodResource1"></asp:Localize>
                                </span>
                                </td>
                                <td>
                                <asp:DropDownList runat="server" ID="ddlPeriod" class="month" 
                                        meta:resourcekey="ddlPeriodResource1" />
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
                        
                        <div class="UserDetails">
                        <div id="divComparision"  runat="server"></div>
                        </div>  
                     <br />
               <table>
               <tr>
               <td><asp:Button ID="btnScheduleRpt" text="Schedule report >" runat="server" 
                       onclick="btnScheduleRpt_Click" meta:resourcekey="btnScheduleRptResource1" OnClientClick="return ValidateDateRegReport()"/></td>
               <td><asp:Button ID="btnRunRpt" text="Run report >" runat="server" 
                       onclick="btnRunRpt_Click" meta:resourcekey="btnRunRptResource1" OnClientClick="return ValidateDateRegReport()"/></td>
               </tr>
               </table>
                        
                  </div>
                  
        </div>
    </div>
 
</asp:Content>
