<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preferences.aspx.cs" MasterPageFile="~/Site.Master"
    Inherits="CCODundeeApplication.Preferences" Title="Customer Preferences" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="mainContent">
       <div class="saveBtn" style="width:50px">
        <asp:ImageButton ImageAlign="Right" ID="ImageButton2" OnClick="UpdateCustomerPreference" ImageUrl="I/confirm.gif" runat="server" AlternateText="Confirm"
                        OnClientClick="javascript:return validate();" 
                        meta:resourcekey="btnConfirmPreferencesResource1" Height="18px" />
               
                </div></br>
                </br>
        <div class="ccBlueHeaderSection householdPreferences">
            <div class="cc_bluehead">
                <h3>
                    <asp:Label ID="lblHouseholdPref" runat="server" Text="Household Preferences" meta:resourcekey="lblHouseholdPrefResource1"></asp:Label>
                </h3>
            </div>
            <div class="replacementCardNo">
                <label for="cardNumber">
                    <asp:Label ID="lblMainCustomer" runat="server" Text="Main Customer:" meta:resourcekey="lblMainCustomerResource1"></asp:Label>
                </label>
                <div class="inputFields">
                    <p>
                        <asp:Label ID="lblCustomerName" runat="server" meta:resourcekey="lblCustomerNameResource1"></asp:Label></p>
                </div>
            </div>
            <div class="cc_body" runat="server" id="dvBody">
                <div class="alertMsgs" id="yourchangessaved" runat="server" visible="false">
                    <asp:Localize ID="lclChangesSaved" runat="server" Text="Your changes have been saved"
                        meta:resourcekey="lclChangesSavedResource1"></asp:Localize>
                </div>
                <div style="padding-left:10px;">
                 <%=Resources.CSCGlobal.lclMPDesc%>
                
                </div>
                       
                <ul class="customer">
                   <li runat="server" id="liTescoinfo">
                        <label for="receiveTesco">                        
                            <strong>
                                <%=Resources.CSCGlobal.lclHreceiveTesco %></strong><%=Resources.CSCGlobal.lclreceiveTesco %>
                        </label>
                        <div class="inputFields">
                            <asp:CheckBox ID="chkRecvTescoOffrnInfo" runat="server" meta:resourcekey="chkRecvTescoOffrnInfoResource1" />
                        </div>
                    </li>
                    <li runat="server" id="liPartnerinfo">
                        <label for="receiveTescoPartners">                         
                            <strong>
                                <%=Resources.CSCGlobal.lclHreceiveTescoPartners %></strong>
                            <%=Resources.CSCGlobal.lclreceiveTescoPartners %></label>
                        <div class="inputFields">
                            <asp:CheckBox ID="chkRecvPartnersOffrnInfo" runat="server" meta:resourcekey="chkRecvPartnersOffrnInfoResource1" />
                        </div>
                    </li>
                    <li runat="server" id="liResearch">
                        <label for="beContacted">
                            <strong>
                                <%=Resources.CSCGlobal.lclHbeContacted%></strong><%=Resources.CSCGlobal.lclbeContacted%></label>
                        <div class="inputFields">
                            <asp:CheckBox ID="chkDontContact" runat="server" meta:resourcekey="chkDontContactResource1" />
                        </div>
                    </li>
                    
                    <%--LCM changes begin--%>
                    <li runat="server" id="liBonusCoupon">
                        <label for="BonusCoupon">
                            <strong>
                                <%=Resources.CSCGlobal.lclHBonusCoupon%></strong><%=Resources.CSCGlobal.lclBonusCoupon%>
                        </label>
                        <div class="inputFields">
                            <asp:CheckBox ID="chkBonusCoupon" runat="server" meta:resourcekey="chkBonusCoupon1" />
                        </div>
                    </li>
                    <%--LCM changes end--%>
                    <%--Poland Legal Changes Start--%>
                    
                    <li id="liGrpTescoproducts" runat="server" visible="false">
                                                <label for="prodsServices">
                                                    <asp:Localize ID="lclGrpTescoproducts" runat="server" Text="Please tick if you do not want to receive offers and information about Tesco products and services:"
                                                        meta:resourcekey="lclGrpTescoproductsResource"></asp:Localize>
                                                </label>
                                                <asp:CheckBox runat="server" ID="chkGrpTescoProducts" name="prodsServices" type="checkbox"
                                                    meta:resourcekey="chkGrpTescoProductsResource" />
                                            </li>
                                            

<li id="liGrpTescoOffer" runat="server" visible="false">
                                                <label for="partnerServices">
                                                    <asp:Localize ID="lclGrpGrpTescoOffer" runat="server" Text="Please tick if you do not want Tesco to send you offers and information about the products and services offered by our partners:"
                                                        meta:resourcekey="lclGrpTescoOfferResource"></asp:Localize>
                                                </label>
                                                <asp:CheckBox ID="chkGrpPartnerOffers" runat="server" name="partnerServices" type="checkbox"
                                                    meta:resourcekey="chkGrpPartnerOffersResource" />
                                            </li>
                                            

<li id="liGrpTescoCustomerReasearch" runat="server" visible="false">
                                                <label for="contactPermission">
                                                    <asp:Localize ID="lclGrpTescoCustomerReasearch" runat="server" Text="Please tick if you do not want to be contacted by Tesco for customer research purposes:"
                                                        meta:resourcekey="lclGrpTescoCustomerReasearchResource"></asp:Localize>
                                                </label>
                                                <asp:CheckBox ID="chkGrpResearch" runat="server" name="contactPermission" type="checkbox"
                                                    meta:resourcekey="chkGrpResearchResource" />
                                            </li>
                    
                      <%--Poland Legal Changes End--%>
                    
                                       
                    </ul>
                    <ul>
                        <li>
                            <asp:Table ID="DataProtectionTable" CellSpacing="0" runat="server" BorderWidth="0"
                                Width="80%" Visible="false" align="center">
                                <asp:TableRow CssClass="DataProtectionPointsTableheader" BorderWidth="1" align="center">
                                    <asp:TableCell BorderWidth="1" Width="27%"></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%"><asp:Localize id="lclpMail" runat="server" Text="Mail" meta:resourcekey="lclpMailResource1"></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%"><asp:Localize id="lclpEmail" runat="server" Text="EMail" meta:resourcekey="lclpEmailResource1"></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%"><asp:Localize id="lclphn" runat="server" Text="Phone" meta:resourcekey="lclphnResource1"></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%"><asp:Localize id="lclppSMS" runat="server" Text="SMS" meta:resourcekey="lclppSMSResource1"></asp:Localize></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow BorderWidth="1" CssClass="DataProtectionPointsTablerow" align="center">
                                    <asp:TableCell BorderWidth="1" Width="27%" align="left"><asp:Localize ID="lclTG" runat="server" Text="Tesco (Group)"  meta:resourcekey="lclTGResource1" ></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTGMail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTGEMail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTGPhone" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTGSms" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow CssClass="DataProtectionPointsTablerow" BorderWidth="1" align="center">
                                    <asp:TableCell BorderWidth="1" Width="27%" align="left"><asp:Localize ID="lclpartypre" runat="server" Text="Partner(3rd Party)"  meta:resourcekey="lclpartypreResource1"></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" ID="PrefTableCell5" runat="server" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTPMail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTPEmail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTPPhone" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkTPSms" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow BorderWidth="1" CssClass="DataProtectionPointsTablerow" align="center">
                                    <asp:TableCell BorderWidth="1" Width="27%" align="left"><asp:Localize ID="lclResearch" runat="server" Text="Research" meta:resourcekey="llclResearchResource1"></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkRMail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkREmail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkRphone" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkRSms" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow BorderWidth="1" CssClass="DataProtectionPointsTablerow" align="center" runat="server" ID="BCMPRE">
                                    <asp:TableCell BorderWidth="1" Width="27%" align="left"><asp:Localize ID="Localize1" runat="server" Text="Bonus Coupon Mailing" meta:resourcekey="llcBonuCoupon"></asp:Localize></asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkBCMMail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkBCMEmail" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkBCMPhone" runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell BorderWidth="1" Width="18%" Style="padding-left: 7%">
                                        <asp:CheckBox ID="chkBCMSms" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </li>
                    </ul>
                <div class="statementPreferences" runat="server" id="DVForStmtPreferences">
                    <h4>
                        <asp:Localize ID="lclStatementPreferences" runat="server" Text="Statement Preferences"
                            meta:resourcekey="lclStatementPreferencesResource1"></asp:Localize>
                    </h4>
                    <ul class="customer">
                        <li>
                            <div class="stmntPreferences">
                                <strong>
                                    <asp:Localize ID="lclinout" runat="server" Text="Opt In/Out" meta:resourcekey="lclinoutResource1"></asp:Localize></strong>
                            </div>
                            <label for="eCoupons" id="lbleCoupon" runat="server">
                                <strong>
                                    <asp:Localize ID="lcleCoupons" runat="server" Text="eCoupons" meta:resourcekey="lcleCouponsResource1"></asp:Localize></strong></label>
                            <div class="inputFields" runat="server" id="diveCoupon">
                                <asp:CheckBox ID="chkEcoupon" runat="server" meta:resourcekey="chkEcouponResource1" />
                            </div>
                            <label for="SaveTree" id="lblSaveTree" runat="server">
                                <strong>
                                    <asp:Localize ID="lclTogetherTrees" runat="server" Text="Together for Trees" meta:resourcekey="lclTogetherTreesResource1"></asp:Localize></strong></label>
                            <div class="inputFields" id="divSaveTree" runat="server">
                                <asp:CheckBox ID="chkSaveTree" runat="server" meta:resourcekey="chkSaveTreeResource1" />
                            </div>
                        </li>
                        <li runat="server" id="lichristmasaver">
                            <label for="christmasSaver">
                                <strong>
                                    <asp:Localize ID="lclChristmas" runat="server" Text="Christmas saver" meta:resourcekey="lclChristmasResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:RadioButton ID="chkXmasSaver" runat="server" GroupName="checkboxPreference"
                                    meta:resourcekey="chkXmasSaverResource1" />
                            </div>
                        </li>
                        <li runat="server" id="liairMiles" style ="width:100%;">
                            <label for="airMiles Standard">
                                <strong><span class="Amiles"></span>
                                    <asp:Localize ID="lclAvios" runat="server" Text="Air Miles" meta:resourcekey="lclAviosResource1"></asp:Localize>
                                    <asp:Localize ID="lclairpremium" runat="server" Text="Air Miles" meta:resourcekey="lclairpremiumResource1" Visible="false"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <%--<asp:CheckBox ID="chkAirmiles" runat="server" 
                                    meta:resourcekey="chkAirmilesResource1" />--%>
                                <asp:RadioButton ID="chkAviosStandard" runat="server" GroupName="checkboxPreference" />
                                <ASP:RadioButton ID="chkAviosPremium" runat="server" GroupName="checkboxPreference" Visible="false" />                      
                            </div>
                            <div id="divAviosMemberShip" runat="server" visible="true">
                           <div style="width: 32%; float: left; padding-bottom: 5px">
                                    <asp:TextBox runat="server" ID="txtAviosMembership" Width="200px" Height="20px"></asp:TextBox></div>
                                <span class="errorFields" runat="server" id="spnAviosMembership" style="display: none">
                                    <%=errMessageAviosMembership%></span>
                                   &nbsp; &nbsp; <asp:Localize ID="lclAviosMembership" runat="server" Text="Please enter membership number" meta:resourcekey="lclBAMemebership"></asp:Localize></div>
                        </li>
                        <li runat="server" id="liBAairmile" style="width:100%">
                            <label for="BAMiles">
                                <strong><span class="BAmiles"></span>
                                    <asp:Localize ID="lclBAvios" runat="server" Text="BA Avios Standard" meta:resourcekey="lclBAviosResource1"></asp:Localize>
                                    <asp:Localize ID="lclBRPRE" runat="server" Text="BA Avios Premium" meta:resourcekey="lclBRPREResource1" Visible="false"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <%--<asp:CheckBox ID="chkBAmiles" runat="server" 
                                    meta:resourcekey="chkBAmilesResource1" />--%>
                                <asp:RadioButton ID="chkBAviosStandard" runat="server" GroupName="checkboxPreference"/>
                                <asp:RadioButton ID="chkBAviosPremium" runat="server" GroupName="checkboxPreference" Visible="false"/>           
                                <%--meta:resourcekey="chkBAviosStandard"--%>
                                <%-- <asp:CheckBox ID="chkBAviosPremium" runat="server" 
                                    meta:resourcekey="chkBAviosPremium" OnClick="return CheckboxValidation(4);"/> --%>
                            </div>
                             <div style="width: 32%; float: left; padding-bottom: 5px">
                                    <asp:TextBox runat="server" ID="txtBAvios"  Width="200px" Height="20px"></asp:TextBox></div>
                                <span class="errorFields" runat="server" id="spanBA" style="display: none">
                                    <%=errMessageClubcardStatement%></span>
                                   &nbsp; &nbsp; <asp:Localize ID="lclMBAvios" runat="server" Text="Please enter membership number" meta:resourcekey="lclBAMemebership"></asp:Localize>
                            <%--  <label for="baMilesEarnRate"><asp:Localize ID="lclBAEarnRate" runat="server" 
                                Text="Earn Rate:" meta:resourcekey="lclBAEarnRateResource1"></asp:Localize>  &nbsp;&nbsp;</label>
                            <div class="inputFields earnRate">
                                <asp:Literal ID="ltrBAMilesEarnRate" runat="server" 
                                    meta:resourcekey="ltrBAMilesEarnRateResource1" />
                            </div>--%>
                        </li>
                        <li runat="server" id="liVirginMiles" style="width:100%">
                            <label for="VAtlantic">
                                <strong><span class="VirginAtlantic"></span>
                                    <asp:Localize ID="lclVirginAtlantic" runat="server" Text="Virgin Atlantic" meta:resourcekey="lclVirginAtlanticResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:RadioButton ID="chkVirginAtlantic" runat="server" GroupName="checkboxPreference"
                                    meta:resourcekey="chkVirginAtlanticResource1" />
                            </div>
                            <div style="width: 32%; float: left; padding-bottom: 5px">
                                    <asp:TextBox runat="server" ID="txtVirgnMembershipID" Width="200px"
                                        Height="20px"></asp:TextBox></div>
                                        <span class="errorFields" runat="server" id="spanVirgin" style="display: none">
                                    <%=errMessageClubcardStatementVirgin%></span>
                                   &nbsp;&nbsp; <asp:Localize ID="lclVirgin" runat="server" Text="Please enter membership number" meta:resourcekey="lclVirginMemebership"></asp:Localize>
                        </li>
                    </ul>
                </div>
               
                <p class="pageAction">
                 <asp:ImageButton ID="orderConfirm" CssClass="imgBtn" 
                        ImageUrl="~/I/confirm.gif" runat="server"
                        OnClick="UpdateCustomerPreference" Visible="false"/>
                    <asp:ImageButton ImageAlign="Right" ID="ImageButton1" CssClass="imgBtn" ImageUrl="~/I/clearselection.gif"
                        runat="server" AlternateText="Clear" OnClientClick="javascript:return ClearSelection();" 
                        meta:resourcekey="btnConfirmPreferencesResource1" />
                </p>
                <div class="statementPreferences" runat="server" id="divstatementPreferences">
                    <h4>
                        <asp:Localize ID="lclContact" runat="server" Text="Preferred contact channel:" meta:resourcekey="lclContactResource1"></asp:Localize>
                    </h4>
                    <ul class="customer">
                        <li>
                            <div class="stmntPreferences">
                                <strong>
                                    <asp:Localize ID="lclContnactpref" runat="server" Text="Opt In/Out" meta:resourcekey="lclContnactprefResource1"></asp:Localize></strong>
                            </div>
                        </li>
                        <li runat="server" id="liPost">
                            <label for="Post">
                                <strong><span class="Amiles"></span>
                                    <asp:Localize ID="lclPost" runat="server" Text="Post" meta:resourcekey="lclPostResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:RadioButton ID="radioPost" runat="server" GroupName="checkboxPreference1" meta:resourcekey="radioPostResource1" />
                            </div>
                        </li>
                        <li runat="server" id="liEmail">
                            <label for="Email">
                                <strong><span class="BAmiles"></span>
                                    <asp:Localize ID="lclEmail" runat="server" Text="Email" meta:resourcekey="lclEmailResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:RadioButton ID="radioEmail" runat="server" GroupName="checkboxPreference1" meta:resourcekey="radioEmailResource1" />
                            </div>
                            <label for="emailConfirm" style="height:40px;">
                                  
                                        <asp:TextBox ID="txtBTEmail" runat="server"></asp:TextBox>
                                    <span runat="server" class="errorFields" id="spanBnTEmail" style="display: none;width:300px">
                                        <%=errMessageBnTemail%></span>
                                    <span runat="server" class="errorFields" id="spanBTEmail" style="display: none;width:300px">
                                        <%=errMessageBTemail%></span>
                               &nbsp;&nbsp; <asp:Localize ID="lclpemailconfirm" runat="server" Text="Please verify email address"
                                    meta:resourcekey="lclpemailconfirmResource1"></asp:Localize>
                                &nbsp;&nbsp;</label>
                        </li>
                        <li runat="server" id="liSms">
                            <label for="SMS">
                                <strong><span class="VirginAtlantic"></span>
                                    <asp:Localize ID="lclSMS" runat="server" Text="SMS" meta:resourcekey="lclSMSResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:RadioButton ID="radioSMS" runat="server" meta:resourcekey="radioSMS" GroupName="checkboxPreference1" />
                                
                            </div>
                            <label for="emailConfirm" style="height:40px;">
                                 
                                        <asp:TextBox ID="txtBTMobile" runat="server"></asp:TextBox>
                                    <span runat="server" class="errorFields" id="spanBnTMobile" style="display: none;width:300px">
                                        <%=errMessageBnTMobile%></span>
                                    <span runat="server" class="errorFields" id="spanBTMobile" style="display: none;width:300px">
                                        <%=errMessageBTMobile%></span>
                               &nbsp;&nbsp; <asp:Localize ID="lclMobiileConfirm" runat="server" Text="Please verify mobile telephone number" meta:resourcekey="lclMobiileConfirmResource1"></asp:Localize>
                                &nbsp;&nbsp;</label>
                        </li>
                        <li runat="server" id="lilargeprint">
                            <label for="lp">
                                <strong><span class="VirginAtlantic"></span>
                                    <asp:Localize ID="lclLP" runat="server" Text="Large Print" meta:resourcekey="lclLPResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:CheckBox ID="chkLP" runat="server" meta:resourcekey="chkLP" OnClick="return CheckboxValidation();" />
                            </div>
                        </li>
                        <li runat="server" id="liBraille">
                            <label for="braille">
                                <strong><span class="VirginAtlantic"></span>
                                    <asp:Localize ID="lclbraille" runat="server" Text="Braille" meta:resourcekey="lclbrailleResource1"></asp:Localize></strong></label>
                            <div class="inputFields">
                                <asp:CheckBox ID="chkbraille" runat="server" meta:resourcekey="chkbrailleResource1"
                                    OnClick="return CheckboxValidation();" />
                            </div>
                        </li>
                    </ul>
                </div>
                 <div runat="server" class="statementPreferences" id="BntHeader" visible="false">
                                <h3>
                                    Baby &amp; Toddler Club</h3>
                                    </div> 
                <div class="mainCustomer" runat="server" id="DivBT"   style="height:367px">
                <ul>              <li id="liBabyTodlerOptIn" runat="server">
                            <div class="statementIconImg">
                               </div>
                            <div runat="server" id="divBTOptIn">
                                <h3>
                                    Main Customer </h3>
                                
                                <div id="divOptInBT" runat="server" style="display:none;width: 100%">
                                    <p>
                                        Please enter the due date or birthdates of all children under the age of
                                        <asp:Label runat="server" ID="lblChildNo"></asp:Label>:</p>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel">
                                        <ContentTemplate>
                                            <p>
                                                <asp:GridView Width="30%" ID="grdBabyTodlerOptIn" runat="server" OnRowDataBound="grdBabyTodlerOptIn_ItemDataBound"
                                                    AlternatingRowStyle-CssClass="alternate" AutoGenerateColumns="false" CssClass="cardHolderTbl"
                                                    BorderWidth="0">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Child">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrChildNumber" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <ItemStyle CssClass="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date Of Birth (DD/MM/YYYY)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDOB" MaxLength="10" runat="server" Text='<%# Bind("OriginalDateOfBirth") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <a><u>
                                                                    <asp:LinkButton runat="server" ID="lnkRemove1" OnClick="RemoveBtn_OnClick" Text="remove"></asp:LinkButton></u></a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="last right" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <FooterStyle CssClass="last right gvfooter" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="alternate" />
                                                </asp:GridView>
                                            </p>
                                            <p>
                                                <span runat="server" class="errorFields" id="spangrdOptIn" style="display: none">
                                                    <%=errMessage%></span></p>
                                            <p>
                                                <a><u>
                                                    <asp:LinkButton runat="server" ID="lnkAddChild" Text="+ add a child" OnClick="AddChildBtn_OnClick"></asp:LinkButton></u>
                                                </a>
                                            </p>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                           <%--         <div style="width: 50%;">
                                        Please confirm your email address*:</div>
                                    <div style="width: 20%; float: left; position: relative; top: -3px">
                                        <asp:TextBox ID="txtBTEmail" runat="server"></asp:TextBox></div>
                                    <span runat="server" class="errorFields" id="spanBTEmail" style="display: none">
                                        <%=errMessageBTemail%></span>
                                    <div style="width: 50%;">
                                        Please confirm your mobile number:</div>
                                    <div style="width: 20%; float: left; position: relative; top: -3px">
                                        <asp:TextBox ID="txtBTMobile" runat="server"></asp:TextBox></div>
                                    <span runat="server" class="errorFields" id="spanBTMobile" style="display: none">
                                        <%=errMessageBTMobile%></span>
                                    <div style="width: 55%;">
                                        How did you hear about this offer?</div>
                                    <div style="width: 20%; float: left; position: relative; top: 3px">
                                        <div class="inputFields">
                                            <span id="spanMedia" runat="server" class="dtFld">
                                                <asp:DropDownList runat="server" ID="ddlMedia" />
                                            </span>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                          
                          <asp:CheckBox ID="chkBabyTodlerOptIn" runat="server" OnClick="javascript:return ClickBabyTodlerOptin();" />
                            <label for="tescoServices" style="width:200px">
                                Opt-in to Baby &amp; Toddler Club
                            </label>
                        </li>
                        <li id="liBabyTodlerOptOut" runat="server">
                            <div class="statementIconImg">
                                </div>
                            <div>
                                <h3>
                                    Main Customer </h3>
                                <p>
                                    You are a member of our Baby and Toddler Club <span class="nl"></span></p>
                                <p>
                                    Please check the due date or birthdates of all children under the age of
                                    <asp:Label runat="server" ID="lbltotalChild"></asp:Label>:</p>
                                <p>
                                    &nbsp;<asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <p>
                                                <asp:GridView Width="30%" ID="grdBabyTodlerOptOut" runat="server" AlternatingRowStyle-CssClass="alternate"
                                                    AutoGenerateColumns="false" CssClass="cardHolderTbl" OnRowDataBound="grdBabyTodlerOptOut_ItemDataBound"
                                                    BorderWidth="0">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Child">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrChildNumber" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <ItemStyle CssClass="" />
                                                            <FooterStyle CssClass="right gvfooter" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date Of Birth (DD/MM/YYYY)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDOB" MaxLength="10" runat="server" Text='<%# Bind("OriginalDateOfBirth") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                            <FooterStyle CssClass="right gvfooter" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <a><u>
                                                                    <asp:LinkButton runat="server" ID="lnkRemove1" OnClick="RemoveBtnOptOut_OnClick"
                                                                        Text="remove"></asp:LinkButton></u></a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="last right" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <FooterStyle CssClass="last right gvfooter" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="alternate" />
                                                </asp:GridView>
                                            </p>
                                            </p>
                                            <p>
                                                <span runat="server" class="errorFields" id="spangrdOptOut" style="display: none">
                                                    <%=errMessage%></span></p>
                                            <p>
                                                <a><u>
                                                    <asp:LinkButton runat="server" ID="LinkButton2" Text="+ add a child" OnClick="AddBTOptOutChildBtn_OnClick"></asp:LinkButton></u></a></p>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </p>
                            </div>
                            <asp:CheckBox ID="chkBabyTodlerOptOut" runat="server" />
                            <label for="tescoServices" style="width:200px">
                                Opt-out of Baby &amp; Toddler Club
                            </label>
                        </li>
                   
                </ul>
                </div>
                   <div class="associateCustomer" runat="server" style="height:367px" visible="false" id="divBnTAsso">
                <ul>              <li id="liAssoBabyTodlerOptIn" runat="server">
                            <div class="statementIconImg">
                               </div>
                            <div runat="server" id="divAssoOptInBT">
                                <h3>
                                    Associate Customer</h3>
                                
                                <div id="divOptInBTAsso" runat="server" style="display:none;width: 100%">
                                    <p>
                                        Please enter the due date or birthdates of all children under the age of
                                        <asp:Label runat="server" ID="lblChildNoAsso"></asp:Label>:</p>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <p>
                                                <asp:GridView Width="30%" ID="grdBabyTodlerAssoOptIn" runat="server" OnRowDataBound="grdBabyTodlerAssoOptIn_ItemDataBound"
                                                    AlternatingRowStyle-CssClass="alternate" AutoGenerateColumns="false" CssClass="cardHolderTbl"
                                                    BorderWidth="0">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Child">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrChildNumber" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <ItemStyle CssClass="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date Of Birth (DD/MM/YYYY)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDOB" MaxLength="10" runat="server" Text='<%# Bind("OriginalDateOfBirth") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <a><u>
                                                                    <asp:LinkButton runat="server" ID="lnkRemove1" OnClick="RemoveAssoBtn_OnClick" Text="remove"></asp:LinkButton></u></a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="last right" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <FooterStyle CssClass="last right gvfooter" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="alternate" />
                                                </asp:GridView>
                                            </p>
                                            <p>
                                                <span runat="server" class="errorFields" id="spangrdOptInAsso" style="display: none">
                                                    <%=errAssoMessage%></span></p>
                                            <p>
                                                <a><u>
                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text="+ add a child" OnClick="AddChildAssoBtn_OnClick"></asp:LinkButton></u>
                                                </a>
                                            </p>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                           <%--         <div style="width: 50%;">
                                        Please confirm your email address*:</div>
                                    <div style="width: 20%; float: left; position: relative; top: -3px">
                                        <asp:TextBox ID="txtBTEmail" runat="server"></asp:TextBox></div>
                                    <span runat="server" class="errorFields" id="spanBTEmail" style="display: none">
                                        <%=errMessageBTemail%></span>
                                    <div style="width: 50%;">
                                        Please confirm your mobile number:</div>
                                    <div style="width: 20%; float: left; position: relative; top: -3px">
                                        <asp:TextBox ID="txtBTMobile" runat="server"></asp:TextBox></div>
                                    <span runat="server" class="errorFields" id="spanBTMobile" style="display: none">
                                        <%=errMessageBTMobile%></span>
                                    <div style="width: 55%;">
                                        How did you hear about this offer?</div>
                                    <div style="width: 20%; float: left; position: relative; top: 3px">
                                        <div class="inputFields">
                                            <span id="spanMedia" runat="server" class="dtFld">
                                                <asp:DropDownList runat="server" ID="ddlMedia" />
                                            </span>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                          
                          <asp:CheckBox ID="chkAssoBabyTodlerOptIn" runat="server" OnClick="javascript:return ClickBabyTodlerAssoOptin();" />
                            <label for="tescoServices" style="width:200px">
                                Opt-in to Baby &amp; Toddler Club
                            </label>
                        </li>
                        <li id="liAssoBabyTodlerOptOut" runat="server">
                            <div class="statementIconImg">
                                </div>
                            <div>
                                <h3>
                                    Associate Customer</h3>
                                <p>
                                    You are a member of our Baby and Toddler Club <span class="nl"></span></p>
                                <p>
                                    Please check the due date or birthdates of all children under the age of
                                    <asp:Label runat="server" ID="lblTotalChildNoAsso"></asp:Label>:</p>
                                <p>
                                    &nbsp;<asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <p>
                                                <asp:GridView Width="30%" ID="grdBabyTodlerAssoOptOut" runat="server" AlternatingRowStyle-CssClass="alternate"
                                                    AutoGenerateColumns="false" CssClass="cardHolderTbl" OnRowDataBound="grdBabyTodlerAssoOptOut_ItemDataBound"
                                                    BorderWidth="0">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Child">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrChildNumber" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <ItemStyle CssClass="" />
                                                            <FooterStyle CssClass="right gvfooter" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date Of Birth (DD/MM/YYYY)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDOB" MaxLength="10" runat="server" Text='<%# Bind("OriginalDateOfBirth") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                            <FooterStyle CssClass="right gvfooter" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <a><u>
                                                                    <asp:LinkButton runat="server" ID="lnkRemove1" OnClick="RemoveBtnAssoOptOut_OnClick"
                                                                        Text="remove"></asp:LinkButton></u></a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="last right" />
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <FooterStyle CssClass="last right gvfooter" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="alternate" />
                                                </asp:GridView>
                                            </p>
                                            </p>
                                            <p>
                                                <span runat="server" class="errorFields" id="spangrdOptOutAsso" style="display: none">
                                                    <%=errAssoMessage%></span></p>
                                            <p>
                                                <a><u>
                                                    <asp:LinkButton runat="server" ID="LinkButton3" Text="+ add a child" OnClick="AddBTOptOutAssoChildBtn_OnClick"></asp:LinkButton></u></a></p>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </p>
                            </div>
                            <asp:CheckBox ID="chkAssoBabyTodlerOptOut" runat="server" />
                            <label for="tescoServices" style="width:200px">
                                Opt-out of Baby &amp; Toddler Club
                            </label>
                        </li>
                   
                </ul>
                </div>
                
                <div class="saveBtn">
                
                    <asp:ImageButton ImageAlign="Right" ID="btnConfirmPreferences" OnClick="UpdateCustomerPreference" 
                        CssClass="imgBtn" ImageUrl="I/confirm.gif" runat="server" AlternateText="Confirm"
                        OnClientClick="javascript:return validate();" meta:resourcekey="btnConfirmPreferencesResource1" />
                <%--<p class="pageAction"></p>--%>
                </div>
            </div>
        </div>
            <asp:HiddenField ID="hdnEmail" runat="server" />
            <asp:HiddenField ID="hdnSms" runat="server" />
            <asp:HiddenField ID="hdnMailingAddress1" runat="server" />
            <asp:HiddenField ID="hdnMailingAddressPostcode" runat="server" />
            <asp:HiddenField ID="hdnMobileNoMinVal" runat="server" />
            <asp:HiddenField ID="hdnMobileNoPrefix" runat="server" />
            <asp:HiddenField ID="hdnLCMPre" runat="server" Value="false" />
            <asp:HiddenField ID="hdnTiltle" runat="server" />
            <asp:HiddenField ID="hdnName1" runat="server" />
            <asp:HiddenField ID="hdnName2" runat="server" />
            <asp:HiddenField ID="hdnName3" runat="server" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:HiddenField ID="hdnMailingAddress2" runat="server" />
            <asp:HiddenField ID="hdnMailingAddress3" runat="server" />
            <asp:HiddenField ID="hdnMailingAddress4" runat="server" />
            <asp:HiddenField ID="hdnMailingAddress5" runat="server" />
            <asp:HiddenField ID="hdnMailingAddress6" runat="server" />
            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:HiddenField ID="hdnEveningPhone" runat="server" />
            <asp:HiddenField ID="hdnDayTimePhone" runat="server" />
            <asp:HiddenField ID="hdnSSN" runat="server" />
            <asp:HiddenField ID="HiddenField3" runat="server" />
            <asp:HiddenField ID="HiddenField4" runat="server" />
            <asp:HiddenField ID="hdnPassport" runat="server" />
            <asp:HiddenField ID="hdnDOB" runat="server" />
            <asp:HiddenField ID="hdnSex" runat="server" />
            <asp:HiddenField ID="hdnRaceID" runat="server" />
            <asp:HiddenField ID="hdnLanguage" runat="server" />
            <asp:HiddenField ID="hdnCustomerMailStatus" runat="server" />
            <asp:HiddenField ID="hdnCustomerMobilePhoneStatus" runat="server" />
            <asp:HiddenField ID="hdnCustomerEmailStatus" runat="server" />
            <asp:HiddenField ID="hdnCustomerUseStatusMain" runat="server" />
            <asp:HiddenField ID="hdnConfigDOB" runat="server" Value="1096" />
            <asp:HiddenField ID="hdnBTCheck" runat="server" Value="0" />
            <asp:HiddenField ID="hdnBTAssoCheck" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDCCustomerID" runat="server" />
            <asp:HiddenField ID="hdnTotalHouseHoldMembers" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAge1" runat="server" />
            <asp:HiddenField ID="hdnAge2" runat="server" />
            <asp:HiddenField ID="hdnAge3" runat="server" />
            <asp:HiddenField ID="hdnAge4" runat="server" />
            <asp:HiddenField ID="hdnAge5" runat="server" />
            <asp:HiddenField ID="hdnPrimaryCustID" runat="server" />
             <asp:HiddenField ID="hdnAssociateCustID" runat="server" />
            <asp:HiddenField ID="hdnNoofCustomer" runat="server" />
            <asp:HiddenField ID="hdnIsBnT" runat="server" Value="false" />
            <asp:HiddenField ID="hdnSendEmailForEmail" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForSMS" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForPost" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForEcoupon" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForChristmasSaver" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForAvios" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForBAAvios" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForVirgin" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForSaveTrees" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendEmailForBandT" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP6" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP7" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP8" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP27" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP28" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP29" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP30" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP31" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP32" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP33" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP34" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP35" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP36" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP37" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP38" runat="server" Value="" />
            <asp:HiddenField ID="hdnPref43" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref44" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref45" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref48" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref6" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref7" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref8" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref10" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref11" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref12" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref13" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref15" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref16" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref17" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref27" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref28" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref29" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref30" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref31" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref32" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref33" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref34" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref35" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref36" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref37" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref38" runat="server" Value="false" />
             <%--LCM changes--%>
            <asp:HiddenField ID="hdnPrefBonus" runat="server" Value="false" />
            <asp:HiddenField ID="hdnSendMailForDPBonus" runat="server" Value="" />
            <asp:HiddenField ID="hdnPref39" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref40" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref41" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPref42" runat="server" Value="false" />
            <asp:HiddenField ID="hdnSendMailForDP39" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP40" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP41" runat="server" Value="" />
            <asp:HiddenField ID="hdnSendMailForDP42" runat="server" Value="" />
            <%--LCM changes--%>
            <asp:HiddenField ID="hdnemailreg" runat="server" Value="" />
            <asp:HiddenField ID="hdndatereg" runat="server" Value="" />
            <asp:HiddenField ID="hdnphonenumberreg" runat="server" Value="" />
            
    </div>

    <script type="text/javascript">
        function CheckboxValidation() {
            if (document.getElementById('<%= chkLP.ClientID %>').checked && document.getElementById('<%= chkbraille.ClientID %>').checked) {
                alert('Please select only one option Of Braille / Large Print');
                return false;
            }
        }
        function validate() {

            if ((document.getElementById('<%= radioEmail.ClientID %>')) && (document.getElementById('<%= txtBTEmail.ClientID %>'))) {
                if (document.getElementById('<%= radioEmail.ClientID %>').checked && document.getElementById('<%= txtBTEmail.ClientID %>').value == '') {
                    document.getElementById('<%=spanBTMobile.ClientID%>').style.display = 'none';
                    document.getElementById('<%=spanBTEmail.ClientID%>').style.display = 'none';
                    document.getElementById('<%=spanBnTEmail.ClientID%>').style.display = 'block';
                    return false;
                }
            }
            if ((document.getElementById('<%= radioSMS.ClientID %>')) && (document.getElementById('<%= txtBTMobile.ClientID %>'))) {
                if (document.getElementById('<%= radioSMS.ClientID %>').checked && document.getElementById('<%= txtBTMobile.ClientID %>').value == '') {
                    document.getElementById('<%=spanBTEmail.ClientID%>').style.display = 'none';
                    document.getElementById('<%=spanBTMobile.ClientID%>').style.display = 'none';
                    document.getElementById('<%=spanBnTMobile.ClientID%>').style.display = 'block';
                    return false;
                }
            }
            if ((document.getElementById('<%= radioEmail.ClientID %>') || document.getElementById('<%= radioSMS.ClientID %>')) && (document.getElementById('<%= chkLP.ClientID %>') || document.getElementById('<%= chkbraille.ClientID %>'))) {
                if ((document.getElementById('<%= radioEmail.ClientID %>').checked || document.getElementById('<%= radioSMS.ClientID %>').checked) && (document.getElementById('<%= chkLP.ClientID %>').checked || document.getElementById('<%= chkbraille.ClientID %>').checked)) {
                    alert('Please select any one contact preference.');
                    return false;
                }
            }
        }
        function ClickBabyTodlerOptin() {
            if (document.getElementById('<%=liBabyTodlerOptIn.ClientID%>') != null) {
                if (document.getElementById('<%=chkBabyTodlerOptIn.ClientID%>').checked) {
                    if (document.getElementById('<%=divOptInBT.ClientID%>') != null) {
                        document.getElementById('<%=divOptInBT.ClientID%>').style.display = 'block';
                        document.getElementById('<%=divOptInBT.ClientID%>').style.width = '100%';
                    }
                }
                else {

                    if (document.getElementById('<%=divOptInBT.ClientID%>') != null)
                        document.getElementById('<%=divOptInBT.ClientID%>').style.display = 'none';
                }
            }
        }

        function ClickBabyTodlerAssoOptin() {
            if (document.getElementById('<%=liAssoBabyTodlerOptIn.ClientID%>') != null) {
                if (document.getElementById('<%=chkAssoBabyTodlerOptIn.ClientID%>').checked) {
                    if (document.getElementById('<%=divOptInBTAsso.ClientID%>') != null) {
                        document.getElementById('<%=divOptInBTAsso.ClientID%>').style.display = 'block';
                        document.getElementById('<%=divOptInBTAsso.ClientID%>').style.width = '100%';
                    }
                }
                else {

                    if (document.getElementById('<%=divOptInBTAsso.ClientID%>') != null)
                        document.getElementById('<%=divOptInBTAsso.ClientID%>').style.display = 'none';
                }
            }
        }
        function ClearSelection() {
            if (document.getElementById('<%=chkAviosStandard.ClientID %>'))
                document.getElementById('<%= chkAviosStandard.ClientID %>').checked = false;
            if (document.getElementById('<%=chkAviosPremium.ClientID %>'))
                document.getElementById('<%= chkAviosPremium.ClientID %>').checked = false;
            if (document.getElementById('<%=chkVirginAtlantic.ClientID %>'))
                document.getElementById('<%= chkVirginAtlantic.ClientID %>').checked = false;
            if (document.getElementById('<%=chkBAviosStandard.ClientID %>'))
                document.getElementById('<%= chkBAviosStandard.ClientID %>').checked = false;
            if (document.getElementById('<%=chkAviosPremium.ClientID %>'))
                document.getElementById('<%=chkAviosPremium.ClientID %>').checked = false;
            if (document.getElementById('<%=chkBAviosPremium.ClientID %>'))
                document.getElementById('<%=chkBAviosPremium.ClientID %>').checked = false;
            if (document.getElementById('<%=chkAviosStandard.ClientID %>'))
                document.getElementById('<%= chkAviosStandard.ClientID %>').checked = false;
            if (document.getElementById('<%=chkXmasSaver.ClientID %>'))
                document.getElementById('<%= chkXmasSaver.ClientID %>').checked = false;
            if (document.getElementById('<%=spanVirgin.ClientID%>') != null)
                document.getElementById('<%=spanVirgin.ClientID%>').style.display = 'none';
            document.getElementById('<%=txtVirgnMembershipID.ClientID%>').className = '';
            document.getElementById('<%= txtVirgnMembershipID.ClientID %>').value = "";
            if (document.getElementById('<%=spanBA.ClientID%>') != null)
                document.getElementById('<%=spanBA.ClientID%>').style.display = 'none';
            document.getElementById('<%=txtBAvios.ClientID%>').className = '';
            document.getElementById('<%= txtBAvios.ClientID %>').value = "";
            if (document.getElementById('<%=spnAviosMembership.ClientID%>') != null)
                document.getElementById('<%=spnAviosMembership.ClientID%>').style.display = 'none';
            document.getElementById('<%=txtAviosMembership.ClientID%>').className = '';
            document.getElementById('<%= txtAviosMembership.ClientID %>').value = "";

            return false;
        }
    </script>

</asp:Content>
