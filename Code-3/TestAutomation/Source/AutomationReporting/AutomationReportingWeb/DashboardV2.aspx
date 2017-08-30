<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardV2.aspx.cs" Inherits="DashboardV2" MasterPageFile="~/SiteV2.master"  %>


<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="cpDashboard">
    <h2 style='<%=(Request.QueryString["q"] == null) ? "": "display:none" %>' id='sendmessage'>
        <span class="text-primary">MCA Automation Report Dashboard</span></h2>
    <div class="col-lg-12 form-group">
        <div class="row   pull-right">
            <label for="ddlEnvironments">
                Select Environment :</label>
            <asp:DropDownList ID="ddlEnvironments" runat="server" CssClass="form-control ddlenvironment">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="row">
            <div id="dashboard">
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="row">
    <div id="flot-placeholder" class="flot-placeholder"></div>
    </div>
    </div>
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <%--<!-- Modal content-->--%>
            <div class="modal-content">
                <div class="modal-header" id='detail-header'>
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title" id='detail-title'>
                        Modal Header</h4>
                </div>
                <div class="modal-body" id='detail-body'>                    
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="historyModel" role="dialog">
        <div class="modal-dialog modal-lg">
            <%--<!-- Modal content-->--%>
            <div class="modal-content">
                <div class="modal-header" id='history-header'>
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title" id='history-title'>
                        Modal Header</h4>
                </div>
                <div class="modal-body" id='history-body'>                    
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="testModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <%--<!-- Modal content-->--%>
            <div class="modal-content">
                <div class="modal-header" id='test-Header'>
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title" id='test-title'>
                        Modal Header</h4>
                </div>
                <div class="modal-body" id='test-body'>                    
                </div>
            </div>
        </div>
    </div>
    <iframe src="http://localhost:9000/automation/public/hello.html"></iframe>
</asp:Content>
<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="HeadContent">
<style>
.flot-placeholder
{
    height: 150px;
}  

</style>
<script type="text/javascript" language="javascript" src="content/js/dashboard.js"></script>
<script type="text/javascript" language="javascript" src="content/js/runbatch.js"></script>
<script type="text/javascript" language="javascript" src="content/js/detailmodel.js"></script>
<script type="text/javascript" language="javascript" src="content/js/testhistory.js"></script>
<script type="text/javascript" language="javascript" src="content/js/testdetails.js"></script>


<script type="text/javascript" language="javascript" src="content/js/float/jquery.flot.js"></script>
<script type="text/javascript" language="javascript" src="content/js/float/jquery.flot.pie.js"></script>
<script type="text/javascript" language="javascript" src="content/js/float/jquery.flot.resize.js"></script>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {        
        var d = $("#dashboard").dashboard({ environmentCtrl: $('.ddlenvironment').attr('id') });
    });

    
  
</script>

</asp:Content>
