<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchedulePopup.aspx.cs" Inherits="CCODundeeApplication.Reports.SchedulePopup" Title="Report Schedule" culture="auto" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

 <%--   <script type="text/javascript" language="javascript">
    </script>--%>
    <base target="_self"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>   
    <asp:Table ID="Table1" runat="server" BorderStyle="Solid" BorderWidth="1px" 
            BorderColor="#49BCD7" Width="500px">
    <asp:TableRow>
        <asp:TableCell>
            <asp:Table ID="TableT" runat="server" BorderStyle="Solid" BorderWidth="1px" 
            BorderColor="#49BCD7"  Width="500px" Font-Bold="true">
                <asp:TableRow>
                    <asp:TableCell >
                        <asp:label id="Label3" style="Z-INDEX: 101" Runat="server" 
                        ForeColor="Black"><asp:Label 
                        ID="lblSchedule" runat="server" Text="Schedule"></asp:label></asp:label> 
                    </asp:TableCell>
<%--                    <asp:TableCell Width="400px"></asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnClose" runat="server" Text="   " BackColor="White" OnClick="CloseBtn_Click"/>
                    </asp:TableCell>--%>
                </asp:TableRow>
            </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:label id="LblSch" style="Z-INDEX: 101" Runat="server" ForeColor="Blue"><asp:Label ID="lblScheduleDetails" 
            runat="server" ForeColor="Black" Text="Schedule Details"></asp:Label></asp:label>
            <asp:Table ID="Table2" runat="server" BorderStyle="Solid" BorderWidth="1px" 
            BorderColor="#49BCD7" CellSpacing="10" Width="490px">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:label id="LblRecurrence" style="Z-INDEX: 101" Runat="server" 
                        ForeColor="Blue"><span style="color:Red" class="formrequiredmarker">* </span>
                    <asp:Label ID="lblRecurrence1" runat="server" Text="Recurrence" 
                        ForeColor="Black"></asp:Label></asp:label>
                        <asp:Table ID="Table3" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                        BorderColor="#49BCD7" CellSpacing="10" Width="300px">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:RadioButton ID="RdbDaily" runat="server" GroupName="Recurrence"/>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="LblDaily" runat="server" Text="Daily"></asp:Label>                            
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:RadioButton ID="RdbWeekly" runat="server" GroupName="Recurrence"  
                                    Checked="true"/>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="LblWeekly" runat="server" Text="Weekly"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:RadioButton ID="RdbPeriodically" runat="server" GroupName="Recurrence"/>
                                 </asp:TableCell>
                                 <asp:TableCell>
                                    <asp:Label ID="LblPeriodically" runat="server" Text="Periodically"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:label id="Time" style="Z-INDEX: 101" Runat="server" ForeColor="Blue"><span style="color:Red" class="formrequiredmarker">* </span>
                    <asp:Label ID="lblTime" runat="server" ForeColor="Black" Text="Time"></asp:Label></asp:label>
                        <asp:Table ID="Table4" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                        BorderColor="#49BCD7" CellSpacing="10">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="Label1" runat="server" Text=" : "></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="TextBox2" runat="server" Width="20px" MaxLength="2"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="Label2" runat="server" Text="(24Hr)" Font-Bold="true" 
                                    Font-Size="Small"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell ColumnSpan="3">
                    <asp:label id="Email" style="Z-INDEX: 101" Runat="server" ForeColor="Blue"><span style="color:Red" class="formrequiredmarker">* </span>
                        <asp:Label ID="lblEmailrecipients" runat="server" ForeColor="Black" 
                            Text="Email recipients"></asp:label></asp:label>
                    <asp:Table ID="Table5" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                            BorderColor="#49BCD7" CellSpacing="10">                       
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:TextBox ID="TxtBxEmail" runat="server" Width="420px" Height="80px" 
                                MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                     </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>  
        </asp:TableCell>  
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="BtnSave" runat="server" BackColor="#49BCD7" 
                ForeColor="White" Font-Bold="True"  Text="Save" OnClick="btnSave_Click"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="BtnCancel" runat="server" BackColor="#49BCD7" 
                ForeColor="White" Font-Bold="True"  Text="Cancel" OnClick="btnCancel_Click"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="BtnTermSchedule" runat="server" BackColor="#49BCD7" 
                ForeColor="White" Font-Bold="True"  Text="Terminate Schedule" 
                OnClick="btnTerminateSchedule_Click"/>
            </asp:TableCell>
        </asp:TableRow>   
    </asp:Table>
    </div>
    </form>
</body>
</html>
