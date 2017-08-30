<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs"
    Inherits="_Dashboard" MasterPageFile="~/Site.master" %>

<%@ Import Namespace="System.IO" %>
<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="cpDashboard">
    <h2 style='<%=(Request.QueryString["q"] == null) ? "": "display:none" %>'>
        <span class="text-primary">MCA Automation Report Dashboard</span></h2>
    <div class="col-lg-12 form-group">
    <div class="row   pull-right"><label for="ddlEnvironments">Select Environment :</label> 
        <asp:DropDownList ID="ddlEnvironments" runat="server" CssClass="form-control" 
            onselectedindexchanged="ddlEnvironments_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList></div>
    </div>
    <div class="col-lg-12">
        <asp:Repeater ID="rSumary" runat="server" OnItemDataBound="rSumary_ItemDataBound">
            <ItemTemplate>
                <div class="row">                
                    <div class='panel panel-success' name="<%# Eval("CountryCode")%>">
                        
                        <div class="panel-heading panel-collapse job-panel" style="cursor:pointer"  data-toggle="collapse" data-target="#<%# "pnl" + Eval("CountryCode").ToString() %>">
                        
                        Automation Reports for <%# Eval("CountryCode")%>
                        <div class="pull-right">Status: <span class="masterstatus"></span><img alt="loading.." src="content/images/loading.gif" class='loader' style="display:none;" />, Result Percentage: <%# Eval("PassPercentage") %>%, Execution Time (ET) : <%# Eval("ExecutionTime")%></div>   
                        
                        </div>
                        
                        <div class="collapse" id='<%# "pnl" + Eval("CountryCode").ToString() %>'>
                        <div class="panel-body"  >
                            <asp:Repeater ID="rReports" runat="server">
                                <ItemTemplate>
                                    <div class="col-lg-3 col-md-6">
                                        <div class='<%# Convert.ToBoolean(Eval("IsGreen")) ? "panel panel-green" : Convert.ToBoolean(Eval("IsSuccess")) ? "panel panel-success" : Convert.ToBoolean(Eval("IsAmber")) ? "panel panel-yellow" : "panel panel-red" %>'>
                                            <div><a href="<%# Eval("DetailLink") %>" target="_blank" >View Report</a></div>
                                                <div class="panel-heading   panel-collapse" data-toggle="collapse"
                                                data-target="#<%# "dtl" + Eval("ID").ToString() %>" style="cursor:pointer" >
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <i class="fa fa-2x">
                                                                <%# Eval("ResultPercentage")%></i>
                                                        </div>                                                        
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                        <label title="Execution Time">
                                                                Cat :</label>
                                                            <span class="spnCat"><%# Eval("CategoryName")%>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                        <label title="Execution Time">
                                                                Server :</label>
                                                            <span class="spnCat"><%# Eval("Server")%>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                        <label title="Execution Time">
                                                                Browser :</label>
                                                            <span class="spnCat"><%# Eval("Browser")%>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <label title="Execution Time">
                                                                ET :</label>
                                                            <span class="spnDuration"><%# Eval("Duration")%></span>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <label title="Last Updated">
                                                                LU :</label>
                                                           <span class="spnLU"> <%# Eval("ReportDateString").ToString() + " " + Eval("ReportTimeString").ToString()%></span>
                                                        </div>
                                                    </div>

                                                </div>
                                            
                                            <div class="panel-footer job-footer" >
                                                <span class="pull-left status"></span><img src="content/images/loading.gif" class='loader' style="display:none;" /> <span class="pull-right"><i class="fa fa-refresh btnRefresh" style="cursor:pointer"></i>
                                                 <i class="fa fa-play btnRerun" style="cursor:pointer"></i></span>
                                                <span>
                                                <span class="country" data="<%# Eval("CountryCode") %>"></span>
                                                <span class="category" data="<%# Eval("CategoryName") %>"></span>
                                                <span class="browser" data="GC"></span> 
                                                </span>
                                                <div class="clearfix">
                                                </div>
                                                <div id='<%# "dtl" + Eval("ID").ToString() %>' class="collapse">
                                                    <div class="list-group">
                                                        <a href="<%# "#" + Eval("CountryCode").ToString() %>" class="list-group-item panel-primary !important">
                                                            Total Tests <span class="pull-right text-muted small"><em>
                                                                <%# Eval("TotalTests")%></em> </span></a><a href="<%# "#" + Eval("CountryCode").ToString() %>"
                                                                    class="list-group-item panel-green">Passed <span class="pull-right text-muted small">
                                                                        <em>
                                                                            <%# Eval("PassedTests")%></em> </span></a><a href="<%# "#" + Eval("CountryCode").ToString() %>"
                                                                                class="list-group-item panel-yellow">Inconclusive <span class="pull-right text-muted small">
                                                                                    <em>
                                                                                        <%# Eval("InconclusiveTests")%></em> </span></a>
                                                        <a href="<%# "#" + Eval("CountryCode").ToString() %>" class="list-group-item panel-red">
                                                            Failed <span class="pull-right text-muted small"><em>
                                                                <%# Eval("FailedTests")%></em> </span></a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        </div>
                    </div>                    
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

  
    <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Modal Header</h4>
      </div>
      <div class="modal-body">
        <p>Some text in the modal.</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>
</asp:Content>

<asp:Content ID="searchContent" runat="server" ContentPlaceHolderID="cpSearch">
    <h2 style='<%=(Request.QueryString["q"] == null) ? "display:none": "" %>'>
        <span class="text-primary">Search Result</span></h2>
    <table class="table table-bordered table-hover table-striped" style='<%=(Request.QueryString["q"] == null) ? "display:none": "" %>'>
        <thead>
            <tr>
                <th>
                    Country
                </th>
                <th>
                    Category
                </th>
                <th>
                    Date
                </th>
                <th>
                    Percentage
                </th>
                <th>
                    Deatail
                </th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rpSearchReport" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("CountryCode") %>
                        </td>
                        <td>
                            <%# Eval("CategoryName")%>
                        </td>
                        <td>
                            <%# Eval("ReportDateString")%>
                        </td>
                        <td>
                            <%# Eval("ResultPercentage")%>
                        </td>
                        <td>
                            <a href='<%# Eval("DetailLink") %>' target="_blank">View</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</asp:Content>
