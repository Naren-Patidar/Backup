<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CardRanges.aspx.cs"
    Inherits="CCODundeeApplication.CardRanges" Title="Card Ranges" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() 
        {
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            var errAddFromCardRange = '<%=Resources.CSCGlobal.errAddFromCardRange %>' ; //Please Enter From Card Range.";
            var errAddToCardRange = '<%=Resources.CSCGlobal.errAddToCardRange %>' ; //"Please Enter From Card Range.";
            var errorFlag = "";
            var errMsgvalid = '<%=Resources.CSCGlobal.errMsgvalidCardRange %>' ;//"Please Enter a valid Card Range.";
            var regNumeric = /^[0-9]*$/;
            var fromCardRange = trim(document.getElementById("<%=txtAddFromCardRange.ClientID%>").value);
            var toCardRange = trim(document.getElementById("<%=txtAddToCardRange.ClientID%>").value);
            errorFlag =  ValidateTextBox("<%=txtAddFromCardRange.ClientID%>", regNumeric, false, false, "spanAddFromCardRange", errMsgvalid);
            errorFlag =errorFlag + ValidateTextBox("<%=txtAddToCardRange.ClientID%>", regNumeric, false, false, "spanAddToCardRange", errMsgvalid);
            if (errorFlag != "") {
                return false;
            }
            else {
                return true;
            }
        }
         function ValidateEditFields() 
        {
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            var errEditFromCardRange = '<%=Resources.CSCGlobal.errAddFromCardRange %>' ;
            var errEditToCardRange = '<%=Resources.CSCGlobal.errAddToCardRange %>';
            var errorFlagEdit = "";
            var errMsgvalidEdit = '<%=Resources.CSCGlobal.errMsgvalidCardRange %>' ;
            var regNumeric = /^[0-9]*$/;
            var fromCardRange = trim(document.getElementById("<%=txtFromCardNumber.ClientID%>").value);
            var toCardRange = trim(document.getElementById("<%=txtToCardNumber.ClientID%>").value);
            errorFlagEdit =  ValidateTextBox("<%=txtFromCardNumber.ClientID%>", regNumeric, false, false, "spanEditFromCardNumber", errMsgvalidEdit);
            errorFlagEdit =errorFlagEdit + ValidateTextBox("<%=txtToCardNumber.ClientID%>", regNumeric, false, false, "spanEditToCardNumber", errMsgvalidEdit);
            if (errorFlagEdit != "") {
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
                <h3>
                    <asp:Localize ID="lclHeader" runat="server" meta:resourcekey="Header"></asp:Localize></h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div>
                    <h3 >
                        <asp:Label ID="lblHeader" runat="server" Text="Card Ranges" 
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
                                <label style="width: 170px;"> <asp:Localize ID="lclFromCardNumber" runat="server" meta:resourcekey="lblFromCardNumber"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtAddFromCardRange" name="UserName" 
                                        type="text" MaxLength="20" meta:resourcekey="txtAddFromCardRangeResource1" />
                                    <span class="errorFields" id="spanAddFromCardRange" style="<%=spanAddFromCardRange%>"><%=errAddFromCardRange%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;"><asp:Localize ID="lclToCardNumber" runat="server" meta:resourcekey="lblToCardNumber"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtAddToCardRange" name="FirstName" type="text" 
                                        meta:resourcekey="txtAddToCardRangeResource1" />
                                    <span class="errorFields" id="spanAddToCardRange" style="<%=spanAddToCardRange%>"><%=errAddToCardRange%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;"><asp:Localize ID="lclCardType" runat="server" meta:resourcekey="lblCardType"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:DropDownList ID="ddlCardType" runat="server" 
                                        meta:resourcekey="ddlCardTypeResource1">
                                    </asp:DropDownList>
                                    <%--<span class="errorFields" id="spanLastName" style="<%=spanStyleMiddleName0%>"><%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnAddSave" runat="server" Text="Save" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnAddSave_Click" 
                                        OnClientClick="return ValidateFields()" 
                                        meta:resourcekey="btnAddSaveResource1" />
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
                                <h4><asp:Localize ID="lclHeader2" runat="server" meta:resourcekey="lblHeader2"></asp:Localize>
                                    </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdCardRanges" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" OnRowCommand="GrdCustomerDetail_RowCommand" 
                                AlternatingRowStyle-CssClass="alternate" AllowSorting="True"
                                OnRowDataBound="grdCustomerDetail_RowDataBound" 
                                OnPageIndexChanging="grdCustomerDetail_PageIndexChanging" 
                                OnSorting="grdCustomerDetail_Sorting" meta:resourcekey="grdCardRangesResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("ClubcardRangeID")  + ";" + Eval("MinCardNumber") + ";" + Eval("MaxCardNumber")+ ";" + Eval("InsertDateTime")+ ";" + Eval("ClubcardType")+ ";" + Eval("CardNumberLength") %>'
                                                ID="lnkSelectCustomer" runat="server" 
                                                meta:resourcekey="lnkSelectCustomerResource1"><%--<asp:Image ID="Image1" 
                                                runat="server" ImageUrl="I/GotoSearchCustomer.gif" 
                                                meta:resourcekey="Image1Resource1" />--%></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Card Type" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White" SortExpression="ClubCardTypeDesc" 
                                        meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrCardType" runat="server" Text='<%# Bind("ClubCardTypeDesc") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No of Digits" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White"  SortExpression="CardNumberLength" 
                                        meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrNoofDigits" runat="server" Text='<%# Bind("CardNumberLength") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Card Number" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White"  SortExpression="MinCardNumber" 
                                        meta:resourcekey="TemplateFieldResource4" >
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrFromCardNo" runat="server" Text='<%# Bind("MinCardNumber") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Card Number" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White"   SortExpression="MaxCardNumber" 
                                        meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrToCardNumber" runat="server" Text='<%# Bind("MaxCardNumber") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date Added" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White"  SortExpression="InsertDateTime" 
                                        meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrDateAdded" runat="server" Text='<%# Bind("InsertDateTime") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate><asp:Localize ID="lclEmptyMsg" runat="server" meta:resourcekey="lblEmptyMsg"></asp:Localize>
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
                   <asp:Button ID="btnAddCardRange" runat="server" Text="Add" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" 
                           onclick="btnAddCardRange_Click" 
                           meta:resourcekey="btnAddCardRangeResource1"/>
                  </div>
                  </li>
                  </ul>
                 </div> 
                <div id="dvEditUser" runat="server" style="height: 100%" visible="false">
                    <div style="padding-bottom: 10px;">
                        <ul class="customer">
                            <li>
                                <label style="width: 170px;">
                                <asp:Localize ID="lclDateAdded" runat="server" meta:resourcekey="lblDateAdded"></asp:Localize>
                                   </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtDateAdded" ReadOnly="True" name="firstName"
                                        type="text" meta:resourcekey="txtDateAddedResource1" />
                                
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;"><asp:Localize ID="lclEditFromCardNumber" runat="server" meta:resourcekey="lblEditFromCardNumber"></asp:Localize>
                                    <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtFromCardNumber" name="initial" type="text" 
                                        meta:resourcekey="txtFromCardNumberResource1"/>
                                    <span class="errorFields" id="spanEditFromCardNumber" style="<%=spanEditFromCardNumber%>">
                                        <%=errEditFromCardNumber%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;"><asp:Localize ID="lclEditToCardNumber" runat="server" meta:resourcekey="lblEditToCardNumber"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtToCardNumber" name="initial" type="text" 
                                        meta:resourcekey="txtToCardNumberResource1" />
                                    <span class="errorFields" id="spanEditToCardNumber" style="<%=spanEditToCardNumber%>">
                                        <%=errEditFromCardNumber%></span>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnEditSave" runat="server" Text="Save" BackColor="#49BCD7" 
                                        ForeColor="White" OnClientClick="return ValidateEditFields()"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditSave_Click" 
                                        meta:resourcekey="btnEditSaveResource1"/>
                                        <asp:Button ID="btnEditDelete" runat="server" Text="Delete" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditDelete_Click" meta:resourcekey="btnEditDeleteResource1" 
                                        />
                                          <asp:Button ID="btnEditCancel" runat="server" Text="Cancel" 
                                        BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" 
                                        onclick="btnEditCancel_Click" meta:resourcekey="btnEditCancelResource1" />
                                </div>
                            </li>
                        </ul>
                     
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
