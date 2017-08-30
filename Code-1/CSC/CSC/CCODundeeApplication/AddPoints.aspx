<%@ Page Title="Add Points" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="AddPoints.aspx.cs" Inherits="CCODundeeApplication.AddPoints" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() {
            
            document.getElementById("lblChooseReasonCode").style.display = "none";
            var result;
            //var errMsgPoints = "Please enter a numeric value";
            var errMsgPoints = '<%=Resources.CSCGlobal.ValidateNumValue %>';
            var errorFlag = "";
            var regNumeric = /^[0-9]*$/;

            errorFlag = ValidateTextBox("<%=txtPoints.ClientID%>", regNumeric, false, false, "spanPoints", errMsgPoints);

            if (errorFlag != "") {
                result = false;
            }
            else {
                if (document.getElementById(ddlReasonCodeClientId).value == -1) {
                    document.getElementById("lblChooseReasonCode").style.display = "block";
                    result = false;
                }
                else {
                    result = true;
                }
            }
            return result;
        }
    </script>

    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <label for="addpoints">
                        <asp:Localize ID="lclAddPoints" runat="server" Text="Add Points" meta:resourcekey="lclAddPointsResource1"></asp:Localize></label></h3>
            </div>
            <div class="cc_body">
                <div class="customerPointsBlueHeaderSection" id="SummarySection" runat="server">
                    <div class="ccPointDetails">
                        <ul>
                            <li>
                                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" meta:resourcekey="lblSuccessMessageResource2"></asp:Label>
                            </li>
                            <li id="currPoints">
                                <div>
                                    <h3>
                                        <label for="addpoints">
                                            <asp:Localize ID="lclTotPoints" runat="server" Text="Points total:" meta:resourcekey="lclTotPointsResource1"></asp:Localize></label></h3>
                                    <div class="infoBox_white">
                                        <h4>
                                            <asp:Literal ID="ltrTotalPoints" Text="0" runat="server" meta:resourcekey="ltrTotalPointsResource1" /></h4>
                                    </div>
                                </div>
                            </li>
                            <li id="Li1" style="margin-bottom: 0.6em;">
                                <div>
                                    <div style="float: left;">
                                        <asp:Localize ID="lrtPoints" Text="Enter number of points to be added: " runat="server"
                                            meta:resourcekey="lrtPointsResource1"></asp:Localize></div>
                                    <div style="float: right;">
                                        <asp:TextBox ID="txtPoints" runat="server" Width="50px" name="Points" Text="0" type="text"
                                            meta:resourcekey="txtPointsResource1"></asp:TextBox>
                                        <%--<span class="errorFields" id="spanPoints" style="<%=spanPoints%>">
                                            <%=errMsgPoints%></span>--%>
                                    </div>
                                </div>
                            </li>
                            <li><span class="errorFields" id="spanPoints" style="<%=spanPoints%>">
                                <%=errMsgPoints%></span> </li>
                            <li id="Li3" style="margin-bottom: 0.6em;">
                                <asp:RadioButton ID="rBtnStore" runat="server" AutoPostBack="true" GroupName="ReplacePrimary1"
                                    Checked="True" OnCheckedChanged="rBtnStore_OnCheckedChanged" />
                                <div id="dStoreHolder" runat="server">
                                    <div style="float: left;">
                                        <asp:Localize ID="lclStore" Text="Store : " runat="server" meta:resourcekey="lclStoreResource1"></asp:Localize></div>
                                    <div style="float: right;">
                                        <asp:DropDownList ID="ddlStore" runat="server" Width="200px" meta:resourcekey="ddlStoreResource1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </li>
                            <li id="Li4" style="margin-bottom: 0.6em;">
                                <asp:RadioButton ID="rBtnPartner" runat="server" AutoPostBack="true" GroupName="ReplacePrimary1"
                                    OnCheckedChanged="rBtnPartner_OnCheckedChanged" />
                                <div id="dPartnerHolder" runat="server">
                                    <div style="float: left;">
                                        <asp:Localize ID="lclPartner" Text="Parnter : " runat="server" meta:resourcekey="lclPartnerResource1"></asp:Localize></div>
                                    <div style="float: right;">
                                        <asp:DropDownList ID="ddlPartner" Enabled="false" runat="server" Width="200px" meta:resourcekey="ddlStoreResource1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </li>
                            <li><span class="errorFields" id="spanPartner" style="<%=spanPartner%>">
                                <%=errPartner%></span> </li>
                            <li id="Li5" style="margin-bottom: 0.6em;">
                                <div>
                                    <div style="float: left;">
                                        <asp:Localize ID="Literal1" Text="Reason Code: " runat="server" meta:resourcekey="Literal1Resource1"></asp:Localize></div>
                                    <div style="float: right;">
                                        <asp:DropDownList ID="ddlReasonCode" runat="server" Width="200px" meta:resourcekey="ddlReasonCodeResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rflReasonCode" runat="server" ControlToValidate="ddlReasonCode"
                                            InitialValue="0" meta:resourcekey="rflReasonCodeResource1"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <label id="lblChooseReasonCode" style="display: none; width: 200px;" class="errorFields">
                                    <asp:Localize ID="lclReasionCode" runat="server" Text="Please select reason code"
                                        meta:resourcekey="lclReasionCodeResource1"></asp:Localize></label>
                                <span class="errorFields" id="spanReasonCode" style="<%=spanReasonCode%>">
                                    <%=errReasonCode%></span> </li>
                            <li id="Li2">
                                <div>
                                    <asp:ImageButton ID="btnAddPoints" runat="server" ImageUrl="~/I/AddPoints.bmp"  OnClick="btnAddPoints_Click" meta:resourcekey="btnAddPointsResource1" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
