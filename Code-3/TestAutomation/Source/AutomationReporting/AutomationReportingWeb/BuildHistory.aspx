<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuildHistory.aspx.cs" Inherits="BuildHistory"
    MasterPageFile="~/SiteV2.master" %>

<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="cpDashboard">
    <div id='divMainContent'>
        <div class='row'>
            <div class='col-md-12'>
                <h2>
                    <span class="text-primary">MCA Automation Build Report</span>
                </h2>
            </div>
        </div>
        <div class='row'>
            <div class='col-md-3'>
                <div class="panel panel-default">
                <div class="panel-heading">Filters</div>
                    <div class="panel-body">
                        <div id="divFilter">
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-9'>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Summary</div>
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
    </div>
</asp:Content>
<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" language="javascript" src="content/js/build_dashboard.js"></script>
    <script type="text/javascript" language="javascript" src="content/js/filter.js"></script>
    <script type="text/javascript" language="javascript" src="content/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" language="javascript" src="content/js/morris.min.js"></script>
    <link rel="Stylesheet" href="content/css/datepicker.css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#divMainContent').BuildDashboard({
                filters: [{ lable: 'Select Build', tag: 'select', attributes: [{ name: 'id', value: 'ddlBuild'}], selectedIndex: 'last', className: 'form-control', url: 'History.aspx/GetDistinct?q=BuildInfo.number&t=' + new Date().getTime() },
                { lable: 'Select Categories', tag: 'select', attributes: [{ name: 'multiple', value: true },{ name: 'id', value: 'ddlCat'}], selectedIndex: '1-6', className: 'form-control', url: 'History.aspx/GetDistinct?q=Category&t=' + new Date().getTime()}],
                tablecss: 'table table-striped table-bordered table-hover text-center'
            });

        });  
    </script>
</asp:Content>
