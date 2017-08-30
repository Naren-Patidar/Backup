<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Aggregate.aspx.cs" Inherits="Aggregate"
    MasterPageFile="~/Site.master" %>

<%@ Import Namespace="System.IO" %>
<asp:Content ID="cntntHead" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" language="javascript">
    /*
    * Play with this code and it'll update in the panel opposite.
    *
    * Why not try some of the options above?
    */
    $(document).ready(function () {
        Morris.Line({
            element: 'chartByCountry',
            data: <%=CountryWiseData %>,
            xkey: <%=CountryYKeys %>,
            ykeys: <%=CountryXKeys %>,
            labels: <%=Categories %>,
            parseTime : false,
            hideHover : 'auto'
        });
        
        Morris.Line({
            element: 'chartByCategory',
            data: <%=CategoryWiseData %>,
            xkey: <%=CategoryYKeys %>,
            ykeys: <%=CategoryXKeys %>,
            labels: <%=Countries %>,
            parseTime : false,
            hideHover : 'auto'
        });    
    });
    
    </script>
</asp:Content>
<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="cpDashboard">
    <h2 style='<%=(Request.QueryString["q"] == null) ? "": "display:none" %>'>
        <span class="text-primary">Historical Data</span></h2>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-2 pull-right">
                    <label>
                        Select Category:</label>
                    <asp:DropDownList ID="ddlAllCategories" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlAllCategories_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div class="col-lg-2 pull-right">
                    <label>
                        Select Country:</label>
                    <asp:DropDownList ID="ddlCountries" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCountries_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </div>
            <!-- /.row (nested) -->
            <div class="row">
                <div id="chartByCountry" style="position: relative; -webkit-tap-highlight-color: rgba(0, 0, 0, 0);">
                </div>
            </div>
        </div>
        <!-- /.panel-body -->
    </div>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-2 pull-right">
                    <label>
                        Select Country:</label>
                    <asp:DropDownList ID="ddlAllCountries" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlAllCountries_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div class="col-lg-2 pull-right">
                    <label>
                        Select Category:</label>
                    <asp:DropDownList ID="ddlCategories" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </div>
            <!-- /.row (nested) -->
            <div class="row">
                <div id="chartByCategory" style="position: relative; -webkit-tap-highlight-color: rgba(0, 0, 0, 0);">
                </div>
            </div>
        </div>
        <!-- /.panel-body -->
    </div>
</asp:Content>
