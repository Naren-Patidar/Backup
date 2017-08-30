
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PromotionCode.aspx.cs"
    Inherits="CCODundeeApplication.PromotionCode" Title="Promotion Code" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">


    <script language="javascript" type="text/javascript">
        function validatefields() {
             var fromdate = document.getElementById("<%=txtStartDatepro.ClientID%>").value;
             var todate = document.getElementById("<%=txtEndDatepro.ClientID%>").value;
             var ErrorField = "To date Should be greater than From date";
             if (fromdate != "" && todate != "") {
                 dt1 = parseInt(fromdate.substring(0, 2), 10);
                 mon1 = parseInt(fromdate.substring(3, 5), 10);
                 yr1 = parseInt(fromdate.substring(6, 10), 10);
                 dt2 = parseInt(todate.substring(0, 2), 10);
                 mon2 = parseInt(todate.substring(3, 5), 10);
                 yr2 = parseInt(todate.substring(6, 10), 10);
                 date1 = new Date(yr1, mon1, dt1);
                 date2 = new Date(yr2, mon2, dt2);

                 if (date2 <= date1) {
                     document.getElementById("spanStoreWelPoints").style.display = '';
                     document.getElementById("spanStoreWelPoints").innerText = ErrorField;
                     document.getElementById("<%=txtEndDatepro.ClientID%>").value = '';
                     document.getElementById("<%=txtEndDatepro.ClientID%>").focus();
                     document.getElementById("SpanStartdate").style.display = 'none';
                     document.getElementById("spanStoreName").style.display = 'none';
                     document.getElementById("spanStoreNumber").style.display = 'none';
                     document.getElementById("<%=txtPromotioncodepro.ClientID%>").className = '';
                     document.getElementById("<%=txtDescriptionpro.ClientID%>").className = '';
                     document.getElementById("<%=txtStartDatepro.ClientID%>").className = '';
                     return false;
                 }
             }


        
        
        }
        
    </script>
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3><asp:Localize ID="lclpromotion" runat="server" Text="Promotion Code" 
                        meta:resourcekey="lclpromotionResource2"></asp:Localize>
                   </h3>
            </div>
            <div class="cc_body">
               
                        <div class="clubcardAcct">   
                          <div class="clubcardAct_head">
                                <h4><asp:Localize ID="lclHeader1" runat="server" 
                                        Text="Administer : Promotion Codes" meta:resourcekey="lclHeader1Resource2" ></asp:Localize>
                                   </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdPromotionCodepro" runat="server" 
                                AutoGenerateColumns="False" AllowSorting="True" OnPageIndexChanging="grdPromotionCodepro_PageIndexChanging"
                                AllowPaging="True" OnRowCommand="grdPromotionCodepro_RowCommand" AlternatingRowStyle-CssClass="alternate"
                                OnRowDataBound="grdPromotionCodepro_RowDataBound" 
                                meta:resourcekey="grdPromotionCodeproResource2">
                                <Columns>
                                    <asp:TemplateField HeaderText="Go" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("PromotionCode")  + ";" + Eval("PromotionCodeDescEnglish") + ";" + Eval("StartDate") + ";" + Eval("EndDate") %>'
                                                ID="lnkSelnkSelectCustomerlectCustomer" runat="server" Text="Go" meta:resourcekey="lnkSelnkSelectCustomerlectCustomerResource2"
                                                ></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Promotion Code" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrpromotionCode" runat="server" Text='<%# Bind("PromotionCode") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrDescription" runat="server" Text='<%# Bind("PromotionCodeDescEnglish") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrStartDate" runat="server" 
                                                Text='<%# Bind("StartDate") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date" HeaderStyle-Font-Bold="true"  
                                        HeaderStyle-ForeColor="White" meta:resourcekey="TemplateFieldResource9">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrEndDate" runat="server" 
                                                Text='<%# Bind("EndDate") %>' />
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
                
                 <div id="dvAddUser1" runat="server">
                     <div class="cc_bluehead">
                <h3>
                    <label for="Customer Cards">
                    <asp:Localize ID="lclCustomerCards" runat="server" 
                        Text="Add Promotion Code" meta:resourcekey="lclCustomerCardsResource2"></asp:Localize></label></h3>
            </div>
                     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
                         CombineScripts="True">
                     </asp:ToolkitScriptManager>
                     <ul class="customer">
                     <li style="width:100%">
                               <asp:Label ID="lblSuccessMessage" runat="server" Font-Bold="True" 
                                   ForeColor="Red" Width="600px" meta:resourcekey="lblSuccessMessageResource2" ></asp:Label> 
                            </li><br />
                            <li>
                                <label style="width:35%" >
                                 <asp:Localize ID="lclPromotionCode" runat="server" Text="Promotion Code" 
                                    meta:resourcekey="lclPromotionCodeResource2"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtPromotioncodepro" name="UserName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtPromotioncodeproResource2"/>
                                    <span class="errorFields" id="spanStoreName" style="<%=spanStoreName%>"><%=errMsgStoreNamepro%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width:35%">
                                <asp:Localize ID="lclDescription" runat="server" Text="Description" 
                                    meta:resourcekey="lclDescriptionResource2"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtDescriptionpro" type="text" 
                                        meta:resourcekey="txtDescriptionproResource2"/>
                                   <span class="errorFields" id="spanStoreNumber" style="<%=spanStoreNumber%>"><%=errMsgTescoStore%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width:35%">
                                 <asp:Localize ID="lclStratdate" runat="server" Text="Start Date" 
                                    meta:resourcekey="lclStratdateResource2"></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                                        TargetControlID="txtStartDatepro" Format="dd/MM/yyyy" Enabled="True">
                                    </asp:CalendarExtender>
                                    <asp:TextBox runat="server" ID="txtStartDatepro" type="text" 
                                        meta:resourcekey="txtStartDateproResource2" />
                                    <span class="errorFields" id="SpanStartdate" style="<%=SpanStartdate%>"><%=errMsgStartdate%></span>
                                </div>
                            </li>
                             <li>
                                <label style="width:35%">
                                 <asp:Localize ID="lclEnddate" runat="server" Text="End Date" 
                                     meta:resourcekey="lclEnddateResource2"></asp:Localize>
                                 <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                                        TargetControlID="txtEndDatepro" Format="dd/MM/yyyy" Enabled="True">
                                    </asp:CalendarExtender>
                                    <asp:TextBox runat="server" ID="txtEndDatepro" name="FirstName" type="text" 
                                        meta:resourcekey="txtEndDateproResource2"/>
                                    <span class="errorFields" id="spanStoreWelPoints" style="<%=spanStoreWelPoints%>"><%=errMsgWelcomePoints%></span>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnUpdatepro" runat="server" Text="UPDATE" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnUpdatepro_Click" 
                                        OnClientClick="javascript:return validatefields();" 
                                        meta:resourcekey="btnUpdateproResource2"  />
                                        <asp:Button ID="btnAddSavepro" runat="server" Text="Save" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnAddSavepro_Click" 
                                        OnClientClick="javascript:return validatefields();" 
                                        meta:resourcekey="btnAddSaveproResource2" />
                                        <asp:Button ID="btnDeletepro" runat="server" Text="Delete" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnDeletepro_Click" 
                                        meta:resourcekey="btnDeleteproResource2" />
                                       <asp:Button ID="btnEditCancepro" runat="server" Text="Cancel" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" 
                                        onclick="btnEditCancepro_Click" 
                                        meta:resourcekey="btnEditCanceproResource2"/> 
                                        
                                </div>
                            </li>
                        </ul>
                     
                    </div>
                    
                </div>
    <asp:HiddenField ID="hdndatereg" runat="server" Value="" />
</asp:Content>


