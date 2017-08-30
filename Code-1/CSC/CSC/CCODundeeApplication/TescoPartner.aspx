<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TescoPartner.aspx.cs" 
MasterPageFile="~/Site.Master"  Inherits="CCODundeeApplication.TescoPartner" Title="Partners" culture="auto"  uiculture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
<script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>
     <script language="javascript" type="text/javascript">
         function ValidateFields() {

             var errMsgPartnerNumber = '<%=Resources.CSCGlobal.errMsgPartnerNumber %>'; //"Please Enter valid Partner Number.";
             var errMsgAddLimit = '<%=Resources.CSCGlobal.errMsgAddLimit %>'; //"Please Enter valid Add Limits.";
             var errMsgSubtractLimit = '<%=Resources.CSCGlobal.errMsgSubtractLimit %>'; //"Please Enter Valid Subtract Limit.";
             var errorFlag = "";
             var errMsgPartnerName = '<%=Resources.CSCGlobal.errMsgPartnerName %>'; //"Please Enter a valid Partner Name.";
             var regNumeric = /^[0-9]*$/;
             var regName = /^[^'";*]*$/;
             var regPartNumber = /^(?![2-9].{3})\d{1,4}$/;
             

             errorFlag = ValidateTextBox("<%=txtPartnerName.ClientID%>", regName, false, false, "spanPartnerName", errMsgPartnerName);
             errorFlag = errorFlag + ValidateTextBox("<%=txtPartnerAddLimit.ClientID%>", regNumeric, false, false, "spanPartnerAddLimit", errMsgAddLimit);
             errorFlag = errorFlag + ValidateTextBox("<%=txtPartnerSubtractLimit.ClientID%>", regNumeric, false, false, "spanPartnerSubtractLimit", errMsgSubtractLimit);
             errorFlag = errorFlag + ValidateTextBox("<%=txtPartnerNumber.ClientID%>", regNumeric, false, false, "spanPartnerNumber", errMsgPartnerNumber);
             errorFlag = errorFlag + ValidateTextBox("<%=txtPartnerNumber.ClientID%>", regNumeric, false, false, "spanPartnerNumber", errMsgPartnerNumber);
             if (errorFlag != "") {
                 return false;
             }
             else {
                 return true;
             }
         }
         function ValidateEditFields() {
             var errEditMsgAddLimit = '<%=Resources.CSCGlobal.errEditMsgAddLimit %>';
             var errEditMsgSubtractLimit = '<%=Resources.CSCGlobal.errEditMsgSubtractLimit %>';
             var errorEditFlag = "";
             var errEditMsgPartnerName = '<%=Resources.CSCGlobal.errEditMsgPartnerName %>';
             var regNumeric = /^[0-9]*$/;
             var regName = /^[^'";*]*$/;
             errorEditFlag = ValidateTextBox("<%=txtEditPartnerName.ClientID%>", regName, false, false, "spanEditPartnerName", errEditMsgPartnerName);
             errorEditFlag = errorEditFlag + ValidateTextBox("<%=txtEditPartnerSubtractLimit.ClientID%>", regNumeric, false, false, "spanEditPartnerSubtractLimit", errEditMsgSubtractLimit);
             errorEditFlag = errorEditFlag + ValidateTextBox("<%=txtEditPartnerAddLimit.ClientID%>", regNumeric, false, false, "spanEditPartnerAddLimit", errEditMsgAddLimit);
             if (errorEditFlag != "") {
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
                <h3><asp:Localize ID="lclHeader" runat="server" 
                        meta:resourcekey="Header"></asp:Localize>
                   </h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" meta:resourcekey="lblSuccessMessageResource1" 
                    ></asp:Label>
                <div>
                    <h3 >
                        <asp:Label ID="lblHeader" runat="server" Text="Partners" meta:resourcekey="lblHeaderResource1" 
                            ></asp:Label></h3>
                 
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
                                <asp:Localize ID="lclPartnerNumber" runat="server" 
                                    meta:resourcekey="lblPartnerNumber"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtPartnerNumber" name="txtPartnerNumber" EnableViewState="false" type="text" meta:resourcekey="txtPartnerNumberResource1" 
                                        />
                                    <span class="errorFields" id="spanPartnerNumber" style="<%=spanPartnerNumber%>"><%=errMsgPartnerNumber%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclPartnerName" runat="server" 
                                    meta:resourcekey="lblPartnerName" ></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtPartnerName" name="txtPartnerName" type="text"  EnableViewState="false"
                                        MaxLength="40" meta:resourcekey="txtPartnerNameResource1" />
                                    <span class="errorFields" id="spanPartnerName" style="<%=spanPartnerName%>"><%=errMsgPartnerName%></span>
                                </div>
                            </li>
                            
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclPartnerType" runat="server" 
                                    meta:resourcekey="lblPartnerType"></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlPartnerType" runat="server" 
                                        meta:resourcekey="ddlPartnerTypeResource1" >
                                    </asp:DropDownList>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclPartnerAddLimit" runat="server" 
                                    meta:resourcekey="lblPartnerAddLimit"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtPartnerAddLimit" name="txtPartnerAddLimit"  EnableViewState="false"
                                        type="text" MaxLength="9" meta:resourcekey="txtPartnerAddLimitResource1" 
                                        />
                                    <span class="errorFields" id="spanPartnerAddLimit" style="<%=spanPartnerAddLimit%>"><%=errMsgAddLimit%></span>
                                </div>
                            </li>
                             <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclPartnerSubtractLimit" runat="server" 
                                     meta:resourcekey="lblPartnerSubtractLimit"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtPartnerSubtractLimit" name="txtPartnerSubtractLimit" EnableViewState="false"
                                        type="text" MaxLength="9" meta:resourcekey="txtPartnerSubtractLimitResource1" 
                                       />
                                    <span class="errorFields" id="spanPartnerSubtractLimit" style="<%=spanPartnerSubtractLimit%>"><%=errMsgSubtractLimit%></span>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnAddSave" runat="server" Text="Save" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnAddSave_Click" 
                                        OnClientClick="return ValidateFields()" 
                                        meta:resourcekey="btnAddSaveResource1"  />
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
                                <h4><asp:Localize ID="lclHeader1" runat="server" 
                                        meta:resourcekey="lclHeader1Resource1"></asp:Localize>
                                   </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdPartners" runat="server" 
                                AutoGenerateColumns="False" AllowSorting="True" 
                                AllowPaging="True" OnRowCommand="grdPartners_RowCommand" AlternatingRowStyle-CssClass="alternate"
                                OnRowDataBound="grdPartners_RowDataBound" 
                                OnPageIndexChanging="grdPartners_PageIndexChanging" 
                                onsorting="grdPartners_Sorting" meta:resourcekey="grdPartnersResource1" 
                                >
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("PartnerID")  + ";" + Eval("PartnerName") + ";" + Eval("PartnerType1") + ";" + Eval("PartnerAddPointsLimit") +  ";" + Eval("PartnerSubtractPointsLimit") %>'
                                                ID="lnkSelectCustomer" runat="server" Text="Go" 
                                                meta:resourcekey="lnkSelectCustomerResource1"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partner Number" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White" SortExpression="PartnerID" 
                                        meta:resourcekey="TemplateFieldResource2" >
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrPartnerNumber" runat="server" Text='<%# Bind("PartnerID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partner Name" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" SortExpression="PartnerName" 
                                        meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrPartnerName" runat="server" Text='<%# Bind("PartnerName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partner Type" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" SortExpression="PartnerType" 
                                        meta:resourcekey="TemplateFieldResource4" >
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrPartnerType" runat="server" 
                                                Text='<%# Bind("PartnerType") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                <asp:Localize ID="lclEmptyMsg" runat="server" 
                                        meta:resourcekey="lclEmptyMsgResource1" ></asp:Localize>
                                    
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
                   <asp:Button ID="btnAddTescoPartner" runat="server" Text="Add" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" 
                           onclick="btnAddTescoPartner_Click" 
                           meta:resourcekey="btnAddTescoPartnerResource1" />
                  </div>
                  </li>
                  </ul>
                 </div> 
                <div id="dvEditUser" runat="server" style="height: 100%" visible="false">
                    <div style="padding-bottom: 10px;">
                      
                      <ul class="customer">
                      <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclEditPartnerNumber" runat="server" 
                                    meta:resourcekey="lblEditPartnerNumber"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditPartnerNumber" name="txtEditPartnerNumber" 
                                        type="text"  ReadOnly="True" 
                                        meta:resourcekey="txtEditPartnerNumberResource1" />
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclEditPartnerName" runat="server" 
                                    meta:resourcekey="lblEditPartnerName" ></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditPartnerName" name="txtEditPartnerName" type="text" 
                                        MaxLength="40" meta:resourcekey="txtEditPartnerNameResource1" />
                                    <span class="errorFields" id="spanEditPartnerName" style="<%=spanEditPartnerName%>"><%=errEditMsgPartnerName%></span>
                                </div>
                            </li>
                            
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclEditPartnerType" runat="server" 
                                    meta:resourcekey="lblEditPartnerType"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlEditPartnerType" runat="server" 
                                        meta:resourcekey="ddlEditPartnerTypeResource1" >
                                    </asp:DropDownList>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclEditPartnerAddLimit" runat="server" 
                                    meta:resourcekey="lblEditPartnerAddLimit"></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditPartnerAddLimit" name="txtEditPartnerAddLimit" 
                                        type="text" MaxLength="9" meta:resourcekey="txtEditPartnerAddLimitResource1" />
                                    <span class="errorFields" id="spanEditPartnerAddLimit" style="<%=spanEditPartnerAddLimit%>"><%=errEditMsgAddLimit%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lclEditPartnerSubtractLimit" runat="server" 
                                    meta:resourcekey="lblEditPartnerSubtractLimit" ></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditPartnerSubtractLimit" name="txtEditPartnerSubtractLimit" 
                                        type="text" MaxLength="9" meta:resourcekey="txtEditPartnerSubtractLimitResource1" />
                                    <span class="errorFields" id="spanEditPartnerSubtractLimit" style="<%=spanEditPartnerSubtractLimit%>"><%=errEditMsgSubtractLimit%></span>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnEditSave" runat="server" Text="Save" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditSave_Click" 
                                        OnClientClick="return ValidateEditFields()" 
                                        meta:resourcekey="btnEditSaveResource1" />
                                       <asp:Button ID="btnEditCancel" runat="server" Text="Cancel" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditCancel_Click" 
                                        meta:resourcekey="btnEditCancelResource1"  /> 
                                        
                                </div>
                            </li>
                        </ul>
                     
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
