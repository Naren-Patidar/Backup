<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CardType.aspx.cs"
    Inherits="CCODundeeApplication.CardType" Title="Card Type" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

<script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() 
        {
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            var errAddName = '<%=Resources.CSCGlobal.errAddName %>' ;//"Please Enter Card Type Name.";
            var errAddLength = '<%=Resources.CSCGlobal.errAddLength %>';//"Please Enter valid Card Length.";
            var errorFlag = "";
            var errMsgvalid =  '<%=Resources.CSCGlobal.errMsgvalid %>';//"Please Enter a valid Card Type.";
            var regNumeric = /^[0-9]*$/;
            var regName=/^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
          
            errorFlag =  ValidateTextBox("<%=txtAddName.ClientID%>", regName, false, false, "spanAddFromCardRange", errAddName);
            errorFlag =errorFlag + ValidateTextBox("<%=txtAddLength.ClientID%>", regNumeric, false, false, "spanAddToCardRange", errAddLength);
            if (errorFlag != "") 
            {
                return false;
            }
            else {
                return true;
            }
        }
          function ValidateEditFields() 
        {
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            var errEditName = '<%=Resources.CSCGlobal.errAddName %>';
            var errorFlagEdit = "";
            var errMsgvalidEdit = '<%=Resources.CSCGlobal.errMsgvalid %>';
            var regNumeric = /^[0-9]*$/;
            var regName=/^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
          
            errorFlagEdit =  ValidateTextBox("<%=txtEditName.ClientID%>", regName, false, false, "spanEditName", errEditName);
            if (errorFlagEdit != "") 
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
    </script>
    <div id="mainContent">
        <div class="ccBlueHeaderSection">
            <div class="cc_bluehead">
                <h3>
                    <asp:label ID="lblHeader1" runat="server" Text="System Admin" 
                        meta:resourcekey="lblHeader1Resource1"></asp:label></h3>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" 
                    meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <div>
                    <h3 >
                        <asp:Label ID="lblHeader" runat="server" Text="Card Types" 
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
                                 <asp:Localize ID="lblName" runat="server" Text="Name:" 
                                    meta:resourcekey="lblNameResource1"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtAddName" name="UserName" type="text" 
                                        MaxLength="20" meta:resourcekey="txtAddNameResource1" />
                                    <span class="errorFields" id="spanAddFromCardRange" style="<%=spanAddFromCardRange%>"><%=errName%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lblLength" runat="server" Text="Length:" 
                                    meta:resourcekey="lblLengthResource1"></asp:Localize>
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtAddLength" name="FirstName" type="text" 
                                        meta:resourcekey="txtAddLengthResource1" />
                                    <span class="errorFields" id="spanAddToCardRange" style="<%=spanAddToCardRange%>"><%=errLength%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lblSeqTable" runat="server" Text="Sequence Table:" 
                                    meta:resourcekey="lblSeqTableResource1"></asp:Localize>
                                    </label>
                                <div class="inputFields">
                                   <asp:TextBox runat="server" ID="txtAddSequenceTable" name="FirstName" 
                                        type="text" meta:resourcekey="txtAddSequenceTableResource1" />
                                    <span class="errorFields" id="span1" style="<%=spanAddToCardRange%>"><%=errSequenceTable%></span>
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
                    <div  class="CardRangeSearch" id="dvSearchResults" visible="true" runat="server">
               
                        <div class="clubcardAcct">   
                          <div class="clubcardAct_head">
                                <h4> <asp:Localize ID="lblGridHeader" runat="server" Text="Administer : Card Types" 
                                        meta:resourcekey="lblGridHeaderResource1"></asp:Localize>
                                    </h4>
                            </div>
                            <asp:GridView CssClass="cardHolderTbl" ID="grdCardType" Visible="true" runat="server" AutoGenerateColumns="False" AllowSorting="true" 
                                AllowPaging="True" OnRowCommand="grdCardType_RowCommand" AlternatingRowStyle-CssClass="alternate"
                                OnRowDataBound="grdCardType_RowDataBound"  OnSorting="grdCardType_Sorting"
                                OnPageIndexChanging="grdCardType_PageIndexChanging" 
                                meta:resourcekey="grdCardTypeResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-BackColor="Red" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Select" CommandArgument='<%# Eval("ClubcardTypeID")  + ";" + Eval("ClubcardTypeDesc") + ";" + Eval("CardNumberLength")+ ";" + Eval("SequenceTable") %>'
                                                ID="lnkSelectCustomer" runat="server" Text="Go" 
                                                meta:resourcekey="lnkSelectCustomerResource1"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Font-Bold="true" 
                                        HeaderStyle-ForeColor="White" SortExpression="ClubcardTypeDesc" 
                                        meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrCardType" runat="server" Text='<%# Bind("ClubcardTypeDesc") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTdLeft" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Length" HeaderStyle-Font-Bold="true"  HeaderStyle-ForeColor="White"
                                     SortExpression="CardNumberLength" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrNoofDigits" runat="server" Text='<%# Bind("CardNumberLength") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTdCenter" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sequence Table" HeaderStyle-Font-Bold="true"  HeaderStyle-ForeColor="White"
                                     SortExpression="SequenceTable" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrFromCardNo" runat="server" 
                                                Text='<%# Bind("SequenceTable") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="GridTd" />
                                        <FooterStyle CssClass="GridFooterTd" />
                                        <HeaderStyle CssClass="GridHeader" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                               <asp:label ID="lblEmptyMsg" runat="server" Text="No Details Found....!!" 
                                        meta:resourcekey="lblEmptyMsgResource1"></asp:label>
                                   
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
                   <asp:Button ID="btnAddCardType" runat="server" Text="Add" BackColor="#49BCD7" ForeColor="White"
                                        Font-Bold="True" Height="24px" Width="50px" 
                           onclick="btnAddCardType_Click" meta:resourcekey="btnAddCardTypeResource1"/>
                  </div>
                  </li>
                  </ul>
                 </div> 
                <div id="dvEditUser" runat="server" style="height: 100%" visible="false">
                    <div style="padding-bottom: 10px;">
                        <ul class="customer">
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lblEditName" runat="server" Text="Name:" 
                                    meta:resourcekey="lblEditNameResource1"></asp:Localize> 
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditName" name="firstName"
                                        type="text" meta:resourcekey="txtEditNameResource1" />
                                    <span class="errorFields" id="spanEditName" style="<%=spanEditName%>">
                                        <%=errEditName%></span>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lblEditLength" runat="server" Text="Length:" 
                                    meta:resourcekey="lblEditLengthResource1"></asp:Localize> 
                                   <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditLength"  ReadOnly="True"  name="initial" 
                                        type="text" meta:resourcekey="txtEditLengthResource1"/>
                                    <%--<span class="errorFields" id="spanMiddleName0" style="<%=spanStyleMiddleName0%>">
                                        <%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <label style="width: 170px;">
                                 <asp:Localize ID="lblEditSeqTable" runat="server" Text="Sequence Table:" 
                                    meta:resourcekey="lblEditSeqTableResource1"></asp:Localize> 
                                    </label>
                                <div class="inputFields">
                                    <asp:TextBox runat="server" ID="txtEditSequenceTable" name="initial" 
                                        type="text" meta:resourcekey="txtEditSequenceTableResource1" />
                                    <%--<span class="errorFields" id="spanMiddleName0" style="<%=spanStyleMiddleName0%>">
                                        <%=errMsgMiddleName%></span>--%>
                                </div>
                            </li>
                            <li>
                                <div class="inputFields" style="padding-left: 175px;">
                                    <asp:Button ID="btnEditSave" runat="server" Text="Save" BackColor="#49BCD7" 
                                        ForeColor="White"  OnClientClick="return ValidateEditFields()"
                                        Font-Bold="True" Height="24px" Width="50px" onclick="btnEditSave_Click" 
                                        meta:resourcekey="btnEditSaveResource1"/>
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
