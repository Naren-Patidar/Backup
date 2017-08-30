<%@ Page Language="C#" AutoEventWireup="true" CodeFile="History.aspx.cs" Inherits="History"
    MasterPageFile="~/SiteV2.master" %>

<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="cpDashboard">
    <div id='divMainContent'>
        <div class='row'>
            <div class='col-md-12'>
                <h2>
                    <span class="text-primary">MCA Automation Report</span>
                </h2>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-3">
                <div class="panel panel-default">
                <div class="panel-heading">Filters</div>
                    <div class="panel-body">
                        <div id="divFilter">
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-lg-9">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div id='loader' style='display: none; text-align: center'>
                            <i class="fa fa-spinner fa-spin" style="font-size: 48px; color: red;"></i>
                        </div>
                        <div id="divGraph">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class='row'>
            <div class="col-lg-3">
                <div class="panel panel-default">
                <div class="panel-heading">Settings</div>
                    <div class="panel-body">
                        <label>
                            Graph for:
                        </label>
                        <select class="form-control" id='graphfor'>
                            <option>Country</option>
                            <option>Environment</option>
                            <option>Category</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" language="javascript" src="content/js/history_dashboard.js"></script>
    <script type="text/javascript" language="javascript" src="content/js/filter.js"></script>
    <script type="text/javascript" language="javascript" src="content/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" language="javascript" src="content/js/morris.min.js"></script>
    <link rel="Stylesheet" href="content/css/datepicker.css" />
    <script type="text/javascript">
        $(document).ready(function () {
            var weekBackDate = new Date();
            weekBackDate.setDate(weekBackDate.getDate() - 7);
            $('#divMainContent').HistoryDashboard({
                filters: [{ lable: 'Select FromDate', tag: 'input', type: 'text', defaultValue: weekBackDate.MMddyyyy(), attributes: [{ name: 'id', value: 'txtDate'}], datatype: 'date', className: 'form-control', data: [] },
                { lable: 'Select Environment', tag: 'select', defaultValue: 'GD', attributes: [{ name: 'id', value: 'ddlEnv'}], className: 'form-control', url: 'History.aspx/GetDistinct?q=Environment&t=' + new Date().getTime() },
                { lable: 'Select Category', tag: 'select', defaultValue: 'BasicFunctionality', attributes: [{ name: 'id', value: 'ddlCat'}], className: 'form-control', url: 'History.aspx/GetDistinct?q=Category&t=' + new Date().getTime() },
                { lable: 'Select Country', tag: 'select', attributes: [{ name: 'id', value: 'ddlCountry'}], className: 'form-control', url: 'History.aspx/GetDistinct?q=Country&t=' + new Date().getTime()}],
                CurrentFilter: { FromDate: weekBackDate.MMddyyyy(), Environment: 'GD', Category: 'BasicFunctionality' },
                load: true
            });
        });  
    </script>
</asp:Content>
