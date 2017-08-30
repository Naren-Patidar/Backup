<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TescoStore.aspx.cs"
    Inherits="CCODundeeApplication.TescoStore" Title="Stores" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
<script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() 
        {
            
            var errMsgTescoStore = '<%=Resources.CSCGlobal.errMsgTescoStore %>';//"Please Enter valid Store Number.";
            var errMsgWelcomePoints= '<%=Resources.CSCGlobal.errMsgWelcomePoints %>';//"Please Enter Valid Welcome Points.";
            var errorFlag = "";
            var errMsgStoreName =  '<%=Resources.CSCGlobal.errMsgStoreName %>';//"Please Enter a valid Store Name.";
            var regNumeric = /^[0-9]*$/;
            var regName=/^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
            
            errorFlag =  ValidateTextBox("<%=txtStoreName.ClientID%>", regName, false, false, "spanStoreName", errMsgStoreName);
            errorFlag =errorFlag + ValidateTextBox("<%=txtStoreWelPoints.ClientID%>", regNumeric, false, false, "spanStoreWelPoints", errMsgWelcomePoints);
            errorFlag =errorFlag + ValidateTextBox("<%=txtStoreNumber.ClientID%>", regNumeric, false, false, "spanStoreNumber", errMsgTescoStore);
            if (errorFlag != "") {
                return false;
            }
            else {
                return true;
            }
        }
         function ValidateEditFields() 
        {
            
//            var errEditMsgTescoStore = "Please Enter valid Store Number.";
            var errEditMsgWelcomePoints= '<%=Resources.CSCGlobal.errMsgWelcomePoints %>';
            var errorEditFlag = "";
            var errEditMsgStoreName = '<%=Resources.CSCGlobal.errMsgStoreName %>';
            var regNumeric = /^[0-9]*$/;
            var regName=/^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
            
            errorFlag =  ValidateTextBox("<%=txtEditStoreName.ClientID%>", regName, false, false, "spanEditStoreName", errEditMsgStoreName);
            errorFlag =errorFlag + ValidateTextBox("<%=txtEditStoreWelPoints.ClientID%>", regNumeric, false, false, "spanEditStoreWelPoints", errEditMsgWelcomePoints);
//            errorFlag =errorFlag + ValidateTextBox("<%=txtEditStoreNumber.ClientID%>", regNumeric, false, false, "spanEditStoreNumber", errEditMsgTescoStore);
            if (errorFlag != "") {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3><asp:Localize ID="lclHeader" runat="server" meta:resourcekey="Header"></asp:Localize>
                   </h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div>
                    <h3 >
                        <asp:Label ID="lblHeader" runat="server" Text="Stores" 
                            meta:resourcekey="lblHeaderResource1"></asp:Label></h3>
                 
               </div>
             <div class="CardSearch">
             &nbsp;
                </div>
                <div id="dvFindUser" runat="server">
                    <div style="width: 60%; float:left" id="dvAddUser" visible="false" runat="server">
                        <br />
                        <ul class="customer">
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclStoreName" runat="server" meta:resourcekey="lblStoreName"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtStoreName" name="UserName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtStoreNameResource1" />
                                    <span class="errorFields" id="spanStoreName" style="<%=spanStoreName%>"><%=errMsgStoreName%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclStoreNumber" runat="server" meta:resourcekey="lblStoreNumber"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtStoreNumber" name="FirstName" type="text" 
                                        meta:resourcekey="txtStoreNumberResource1"/>
                                    <span class="errorFields" id="spanStoreNumber" style="<%=spanStoreNumber%>"><%=errMsgTescoStore%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclStoreFormat" runat="server" meta:resourcekey="lblStoreFormat"></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlStoreFormat" runat="server" 
                                        meta:resourcekey="ddlStoreFormatResource1">
                                    </asp:DropDownList>
                                  <%--  <span class="errorFields" id="spanStoreFormat" style="<%=spanStoreFormat%>"><%=errStoreFormat%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclStoreWelcomePloints" runat="server" meta:resourcekey="lblStoreWelcomePloints"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtStoreWelPoints" name="FirstName" type="text" 
                                        meta:resourcekey="txtStoreWelPointsResource1" />
                                    <span class="errorFields" id="spanStoreWelPoints" style="<%=spanStoreWelPoints%>"><%=errMsgWelcomePoints%></span>
                                </div>
                            </li>
                             <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclRegion" runat="server" meta:resourcekey="lblRegion"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlRegion" runat="server" 
                                        meta:resourcekey="ddlRegionResource1">
                                    </asp:DropDownList>
                                 <%--   <span class="errorFields" id="spanRegion" style="<%=spanRegion%>"><%=errRegion%></span>--%>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnAddSave" runat="server" Text="Save" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnAddSave_Click" 
                                        OnClientClick="return ValidateFields()" 
                                        meta:resourcekey="btnAddSaveResource1"   />
                                       <asp:Button ID="btnAddCancel" runat="server" Text="Cancel" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnAddCancel_Click" 
                                        meta:resourcekey="btnAddCancelResource1" /> 
                                      
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div  class="CardRangeSearch"  id="dvSearchResults" runat="server">
               
                        <div class="clubcardAcct">   
                          <div class="clubcardAct_head">
                                <h4><asp:Localize ID="lclHeader1" runat="server" meta:resourcekey="lblHeader1"></asp:Localize>
                                   </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdStores" runat="server" 
                                AutoGenerateColumns="False" AllowSorting="True" 
                                AllowPaging="True" OnRowCommand="grdStores_RowCommand" AlternatingRowStyle-CssClass="alternate"
                                OnRowDataBound="grdStores_RowDataBound" 
                                OnPageIndexChanging="grdStores_PageIndexChanging" onsorting="grdStores_Sorting" 
                                meta:resourcekey="grdStoresResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("TescoStoreID")  + ";" + Eval("TescoStoreName") + ";" + Eval("StoreFormatID") + ";" + Eval("StoreRegionID") +  ";" + Eval("store_welcome_points") %>'
                                                ID="lnkSelectCustomer" runat="server" Text="Go"
                                                meta:resourcekey="lnkSelectCustomerResource1">
                                                <%--<asp:ImageButton ID="imgSelect" 
                                                runat="server"/>--%></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Store Number" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White" SortExpression="TescoStoreID" 
                                        meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrStoreNumber" runat="server" Text='<%# Bind("TescoStoreID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Store Name" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" SortExpression="TescoStoreName" 
                                        meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrStoreName" runat="server" Text='<%# Bind("TescoStoreName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Region" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" SortExpression="StoreRegionName" 
                                        meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrRegion" runat="server" 
                                                Text='<%# Bind("StoreRegionName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                <asp:Localize ID="lclEmptyMsg" runat="server" meta:resourcekey="lblEmptyMsg"></asp:Localize>
                                    
                                </EmptyDataTemplate>

<AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                
                 <div id="dvAddCardRange" align="right" runat="server" style="height: 100%">
                  <ul class="customer">
                  <li>
                   <div style="padding-left: 675px;" >
                   <asp:Button ID="btnAddTescoStore" runat="server" Text="Add" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" 
                           onclick="btnAddTescoStore_Click" 
                           meta:resourcekey="btnAddTescoStoreResource1"/>
                  </div>
                  </li>
                  </ul>
                 </div> 
                <div id="dvEditUser" runat="server" style="height: 100%" visible="false">
                    <div style="padding-bottom: 10px;">
                      
                      <ul class="customer">
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclEditStoreName" runat="server" meta:resourcekey="lblEditStoreName"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditStoreName" name="UserName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtEditStoreNameResource1" />
                                    <span class="errorFields" id="spanEditStoreName" style="<%=spanEditStoreName%>"><%=errEditMsgStoreName%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclEditStoreNumber" runat="server" meta:resourcekey="lblEditStoreNumber"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditStoreNumber" name="FirstName" 
                                        type="text"  ReadOnly="True" meta:resourcekey="txtEditStoreNumberResource1" />
                                   <%-- <span class="errorFields" id="spanEditStoreNumber" style="<%=spanEditStoreNumber%>"><%=errEditMsgTescoStore%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclEditStoreFormat" runat="server" meta:resourcekey="lblEditStoreFormat"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlEditStoreFormat" runat="server" 
                                        meta:resourcekey="ddlEditStoreFormatResource1">
                                    </asp:DropDownList>
                                  <%--  <span class="errorFields" id="spanStoreFormat" style="<%=spanStoreFormat%>"><%=errStoreFormat%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclEditStoreWelcomePoints" runat="server" meta:resourcekey="lblEditStoreWelcomePoints"></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditStoreWelPoints" name="FirstName" 
                                        type="text" meta:resourcekey="txtEditStoreWelPointsResource1" />
                                    <span class="errorFields" id="spanEditStoreWelPoints" style="<%=spanEditStoreWelPoints%>"><%=errEditMsgWelcomePoints%></span>
                                </div>
                            </li>
                             <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclEditRegion" runat="server" meta:resourcekey="lblEditRegion"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlEditRegion" runat="server" 
                                        meta:resourcekey="ddlEditRegionResource1">
                                    </asp:DropDownList>
                                 <%--   <span class="errorFields" id="spanRegion" style="<%=spanRegion%>"><%=errRegion%></span>--%>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnEditSave" runat="server" Text="Save" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditSave_Click" 
                                        OnClientClick="return ValidateEditFields()" 
                                        meta:resourcekey="btnEditSaveResource1"  />
                                       <asp:Button ID="btnEditCancel" runat="server" Text="Cancel" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditCancel_Click" 
                                        meta:resourcekey="btnEditCancelResource1" /> 
                                        
                                </div>
                            </li>
                        </ul>
                     
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
