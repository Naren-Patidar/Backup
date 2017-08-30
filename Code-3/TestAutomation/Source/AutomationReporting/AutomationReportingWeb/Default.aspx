<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/Site.master" %>

<asp:Content ID="mainContent" runat="server" ContentPlaceHolderID="cpDashboard">
    <h2 style='<%=(Request.QueryString["q"] == null) ? "" : "display:none" %>' >
        <span class="text-primary">MCA Automation Report Summary</span></h2>
    <div class="col-lg-12">
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
            <asp:Repeater ID="rpSummaryReport" runat="server">
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
    </div>
</asp:Content>
