<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerCardsRecarding.aspx.cs"
    MasterPageFile="~/Site.Master" Inherits="CCODundeeApplication.CustomerCardsRecarding"
    Title="Customer Cards" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">
    <script type="text/javascript">

        var isErrorMessage = false;
        var isUpperRadioCheck = false;
        var isLowerRadioCheck = false;


        function rdbCheckedMain(cardNumberVal) {

            if (cardNumberVal != null) {
                isUpperRadioCheck = true;
                if (document.getElementById("ctl00_PageContainer_rptCardDetails_ctl01_rdbReplaceMain") != null) {
                    var rdbMainCheck = document.getElementById("ctl00_PageContainer_rptCardDetails_ctl01_rdbReplaceMain");
                }
                if (document.getElementById("ctl00_PageContainer_rptCardDetailsAssociate_ctl01_rdbReplaceAssociate") != null) {
                    var rdbAssociateCheck = document.getElementById("ctl00_PageContainer_rptCardDetailsAssociate_ctl01_rdbReplaceAssociate");
                    rdbAssociateCheck.checked = false;
                }

                isLowerRadioCheck = false;
                var rdbMainCheck = document.getElementById("ctl00_PageContainer_rptCardDetailsAssociate_ctl01_rdbReplaceAssociate");
                var toDisplayReplaceCard = document.getElementById("<%= divReplaceCard.ClientID %>");
                var replaceCardSecMain = document.getElementById("<%= divReplaceSecMain.ClientID %>");
                var replaceCardNo = document.getElementById("<%= txtReplaceCardNo.ClientID %>");
                var replaceCardSecAssociate = document.getElementById("<%= divReplaceSecAssociate.ClientID %>");
                var btnConfirm = document.getElementById("<%= orderConfirm.ClientID %>");
                var errorMessage = document.getElementById("<%= divErrorMessage.ClientID %>");

                var radioLost = document.getElementById("<%= RadioLost.ClientID %>");
                var radioDamaged = document.getElementById("<%= RadioDamaged.ClientID %>");
                var radioStolen = document.getElementById("<%= RadioStolen.ClientID %>");
                var radioMoreFobs = document.getElementById("<%= RadioMoreFobs.ClientID %>");
                var radioOther = document.getElementById("<%= RadioOther.ClientID %>");

                var radioNewCard = document.getElementById("<%= RadioNewCard.ClientID %>");
                var radioNewFob = document.getElementById("<%= RadioNewFob.ClientID %>");
                var radioNewCardnFob = document.getElementById("<%= RadioNewCardnFob.ClientID %>");

                radioLost.checked = false;
                radioDamaged.checked = false;
                radioStolen.checked = false;
                radioMoreFobs.checked = false;
                radioOther.checked = false;
                radioNewCard.checked = false;
                radioNewFob.checked = false;
                radioNewCardnFob.checked = false;

                replaceCardNo.value = cardNumberVal;
                toDisplayReplaceCard.style.display = "block";
                replaceCardSecMain.style.display = "block";
                replaceCardSecAssociate.style.display = "none";
                btnConfirm.disabled = false;
                errorMessage.style.display = "none";
                isErrorMessage = true;
            }
        }

        function rdbCheckedAssociate(cardNumberVal) {
            if (cardNumberVal != null) {
                isLowerRadioCheck = true;

                if (document.getElementById("ctl00_PageContainer_rptCardDetails_ctl01_rdbReplaceMain") != null) {
                    var rdbMainCheck = document.getElementById("ctl00_PageContainer_rptCardDetails_ctl01_rdbReplaceMain");
                    rdbMainCheck.checked = false;
                }
                if (document.getElementById("ctl00_PageContainer_rptCardDetailsAssociate_ctl01_rdbReplaceAssociate") != null) {
                    var rdbAssociateCheck = document.getElementById("ctl00_PageContainer_rptCardDetailsAssociate_ctl01_rdbReplaceAssociate");
                }

                isUpperRadioCheck = false;
                var toDisplayReplaceCard = document.getElementById("<%= divReplaceCard.ClientID %>");
                var replaceCardSecAssociate = document.getElementById("<%= divReplaceSecAssociate.ClientID %>");
                var replaceCardNo = document.getElementById("<%= txtReplaceCardNo.ClientID %>");
                var replaceCardSecMain = document.getElementById("<%= divReplaceSecMain.ClientID %>");
                var btnConfirm = document.getElementById("<%= orderConfirm.ClientID %>");
                var errorMessage = document.getElementById("<%= divErrorMessage.ClientID %>");

                var radioNewCardMain = document.getElementById("ctl00_PageContainer_RadioNewCardMain");
                var radioNewFobMain = document.getElementById("ctl00_PageContainer_RadioNewFobMain");
                var radioNewCardnFobMain = document.getElementById("ctl00_PageContainer_RadioNewCardnFobMain");

                var radioLostMain = document.getElementById("ctl00_PageContainer_RadioLostMain");
                var radioDamagedMain = document.getElementById("ctl00_PageContainer_RadioDamagedMain");
                var radioStolenMain = document.getElementById("ctl00_PageContainer_RadioStolenMain");
                var radioMoreFobsMain = document.getElementById("ctl00_PageContainer_RadioMoreFobsMain");
                var radioOtherMain = document.getElementById("ctl00_PageContainer_RadioOtherMain");

                radioDamagedMain.checked = false;
                radioLostMain.checked = false;
                radioMoreFobsMain.checked = false;
                radioNewCardMain.checked = false;
                radioNewCardnFobMain.checked = false;
                radioNewFobMain.checked = false;
                radioOtherMain.checked = false;
                radioStolenMain.checked = false;

                replaceCardNo.value = cardNumberVal;
                toDisplayReplaceCard.style.display = "block";
                replaceCardSecAssociate.style.display = "block";
                replaceCardSecMain.style.display = "none";
                btnConfirm.disabled = false;
                errorMessage.style.display = "none";
                isErrorMessage = true;

            }

        }
        function modalMoreInfoHide() {
            document.getElementById("modalBox").style.display = "none";
            return false;
        }
        function modalMoreInfoShow(cardNumber) {
            //document.getElementById("modalCardNumber").innerHTML = cardNumber;

            var userWidth = screen.availWidth;
            var userHeight = screen.availHeight;
            var leftPos;
            var topPos;
            var popW = 710;   //set width here
            var popH = 387;   //set height here

            var settings = 'modal,scrollBars=no,resizable=no,toolbar=no,menubar=no,location=no,directories=no,';
            leftPos = (userWidth - popW) / 2,
            topPos = (userHeight - popH) / 2;
            settings += 'left=' + leftPos + ',top=' + topPos + ',width=' + popW + ', height=' + popH + '';

            var ShowInfo = window.open(cardNumber, 'ShowInfo', settings);
            // var ShowInfo = window.open('EditCardStatus.aspx?' + cardNumber, 'ShowInfo', settings);
            ShowInfo.focus();
            return false;
        }
        //New function added to show Main and Assocative card in PopupWindow MKTG00007324 07-08-2012  Kumar P
        function modalMoreInfoShowReplacePrimAsscoCard(cardNumber) {

            var NewcardNumber = 0;
            //var cardNumber = "ChangePrimaryCard.aspx?custID=WwoAocIKzIo="
            var GetPageURl = cardNumber.split('=');
            var strURL;
            for (var i = 0; i < GetPageURl.length; i++) {
                strURL = GetPageURl[i];
                break;
            }
            //Added the conditino to check if radio button Main/Assoc are visable in the screen 08-08-2012  Kumar P
            if (document.getElementById('<%=RBMain.ClientID%>') && document.getElementById('<%=RBAssociative.ClientID%>') != null) {
                if (document.getElementById('<%=RBMain.ClientID%>').checked) {
                    NewcardNumber = strURL + "=" + document.getElementById('<%=hdnPassMainCusId.ClientID%>').value;
                }
                if (document.getElementById('<%=RBAssociative.ClientID%>').checked) {
                    NewcardNumber = strURL + "=" + document.getElementById('<%=hdnPassAssCusID.ClientID%>').value;
                }
            }
            else {
                NewcardNumber = strURL + "=" + document.getElementById('<%=hdnPassMainCusId.ClientID%>').value;
            }
            var userWidth = screen.availWidth;
            var userHeight = screen.availHeight;
            var leftPos;
            var topPos;
            var popW = 710;   //set width here
            var popH = 387;   //set height here
            var settings = 'modal,scrollBars=no,resizable=no,toolbar=no,menubar=no,location=no,directories=no,';
            leftPos = (userWidth - popW) / 2,
            topPos = (userHeight - popH) / 2;
            settings += 'left=' + leftPos + ',top=' + topPos + ',width=' + popW + ', height=' + popH + '';
            var ShowInfo = window.open(NewcardNumber, 'ShowInfo', settings);
            ShowInfo.focus();
            return false;
        }
        function errorMessageCheck() {

            var errorMessageCheck = true;
            var replaceCardSecAssociate = document.getElementById("<%= divReplaceSecAssociate.ClientID %>");
            var replaceSecAssociateSub = document.getElementById("<%= divReplaceSecAssociateSub.ClientID %>");

            var replaceCardNo = document.getElementById("<%= txtReplaceCardNo.ClientID %>");
            var replaceCardSecMain = document.getElementById("<%= divReplaceSecMain.ClientID %>");
            var replaceSecMainSub = document.getElementById("<%= divReplaceSecMainSub.ClientID %>");


            //var btnConfirm= document.getElementById("<%= pbtnConfirm.ClientID %>");
            var errorMessage = document.getElementById("<%= divErrorMessage.ClientID %>");

            var radioNewCardMain = document.getElementById("<%= RadioNewCardMain.ClientID %>");
            var radioNewFobMain = document.getElementById("<%= RadioNewFobMain.ClientID %>");
            var radioNewCardnFobMain = document.getElementById("<%= RadioNewCardnFobMain.ClientID %>");
            if (isUpperRadioCheck)
                radioNewCardnFobMain.checked = true;
            else
                radioNewCardnFobMain.checked = false;

            if (radioNewCardMain.checked || radioNewFobMain.checked || radioNewCardnFobMain.checked) {
                document.getElementById("<%= hdnSelCustID.ClientID %>").value = document.getElementById("<%= hdnMainCusId.ClientID %>").value;
            }

            var radioLostMain = document.getElementById("<%= RadioLostMain.ClientID %>");
            var radioDamagedMain = document.getElementById("<%= RadioDamagedMain.ClientID %>");
            var radioStolenMain = document.getElementById("<%= RadioStolenMain.ClientID %>");
            var radioMoreFobsMain = document.getElementById("<%= RadioMoreFobsMain.ClientID %>");
            var radioOtherMain = document.getElementById("<%= RadioOtherMain.ClientID %>");

            var radioLost = document.getElementById("<%= RadioLost.ClientID %>");
            var radioDamaged = document.getElementById("<%= RadioDamaged.ClientID %>");
            var radioStolen = document.getElementById("<%= RadioStolen.ClientID %>");
            var radioMoreFobs = document.getElementById("<%= RadioMoreFobs.ClientID %>");
            var radioOther = document.getElementById("<%= RadioOther.ClientID %>");

            var radioNewCard = document.getElementById("<%= RadioNewCard.ClientID %>");
            var radioNewFob = document.getElementById("<%= RadioNewFob.ClientID %>");
            var radioNewCardnFob = document.getElementById("<%= RadioNewCardnFob.ClientID %>");
            var whichPanel = "";
            var boolShowPanel = true;

            if (isLowerRadioCheck)
                radioNewCardnFob.checked = true;
            else
                radioNewCardnFob.checked = false;

            if (radioNewCard.checked || radioNewFob.checked || radioNewCardnFob.checked) {
                document.getElementById("<%= hdnSelCustID.ClientID %>").value = document.getElementById("<%= hdnAssCusID.ClientID %>").value;
            }

            if (isErrorMessage) {

                //btnConfirm.style.display ="block";
                if (radioNewCardMain.checked || radioNewFobMain.checked || radioNewCardnFobMain.checked) {
                    boolWhichPanel = "upper";
                    if (radioLostMain.checked || radioDamagedMain.checked || radioStolenMain.checked || radioMoreFobsMain.checked || radioOtherMain.checked) {
                        errorMessage.innerText = "";
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "none";
                        replaceCardSecMain.style.display = "block";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = true;

                    }
                    else {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqReasion %>'; //"Please select a request reason";//ReqReasion
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "none";
                        replaceCardSecMain.style.display = "block";
                        replaceCardSecMain.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecMainSub.className = 'replaceRightBorderSection';
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                        boolShowPanel = false;
                    }
                }

                else if (radioLostMain.checked || radioDamagedMain.checked || radioStolenMain.checked || radioMoreFobsMain.checked || radioOtherMain.checked) {
                    boolWhichPanel = "upper";
                    if (radioNewCardMain.checked || radioNewFobMain.checked || radioNewCardnFobMain.checked) {
                        errorMessage.innerText = "";
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "none";
                        replaceCardSecMain.style.display = "block";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = true;

                    }
                    else {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqType %>'; //"Please select a request type";//ReqType
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "none";
                        replaceCardSecMain.style.display = "block";
                        replaceCardSecMain.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecMainSub.className = 'replaceRightBorderSection';
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                        boolShowPanel = false;
                    }
                }

                else if (radioNewCard.checked || radioNewFob.checked || radioNewCardnFob.checked) {
                    var whichPanel = "lower";
                    if (radioLost.checked || radioDamaged.checked || radioStolen.checked || radioMoreFobs.checked || radioOther.checked) {
                        errorMessage.innerText = "";
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "block";
                        replaceCardSecMain.style.display = "none";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = true;


                    }
                    else {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqReasion %>'; //"Please select a request reason";//ReqReasion
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "block";
                        replaceCardSecAssociate.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecAssociateSub.className = 'replaceRightBorderSection';
                        replaceCardSecMain.style.display = "none";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                        boolShowPanel = false;
                    }
                }

                else if (radioLost.checked || radioDamaged.checked || radioStolen.checked || radioMoreFobs.checked || radioOther.checked) {
                    var whichPanel = "lower";
                    if (radioNewCard.checked || radioNewFob.checked || radioNewCardnFob.checked) {
                        errorMessage.innerText = "";
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "block";
                        replaceCardSecMain.style.display = "none";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = true;


                    }
                    else {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqType %>'; //"Please select a request type";//ReqType
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "block";
                        replaceCardSecAssociate.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecAssociateSub.className = 'replaceRightBorderSection';
                        replaceCardSecMain.style.display = "none";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                        boolShowPanel = false;
                    }
                }

                else if (boolShowPanel) {
                    if (whichPanel == "upper") {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqTypeandReasion %>'; //"Please select a request type and a request reason";//ReqTypeandReasion
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "none";
                        replaceCardSecMain.style.display = "block";
                        replaceCardSecMain.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecMainSub.className = 'replaceRightBorderSection';
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                    }
                    else if (isUpperRadioCheck) {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqTypeandReasion %>'; // "Please select a request type and a request reason";//ReqTypeandReasion
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "none";
                        replaceCardSecMain.style.display = "block";
                        replaceCardSecMain.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecMainSub.className = 'replaceRightBorderSection';
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                    }
                    else if (isLowerRadioCheck) {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqTypeandReasion %>'; //"Please select a request type and a request reason";//ReqTypeandReasion
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "block";
                        replaceCardSecAssociate.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecAssociateSub.className = 'replaceRightBorderSection';
                        replaceCardSecMain.style.display = "none";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                    }
                    else {
                        errorMessage.innerText = '<%=Resources.CSCGlobal.ReqTypeandReasion %>'; //"Please select a request type and a request reason";//ReqTypeandReasion
                        errorMessage.style.display = "block";
                        replaceCardSecAssociate.style.display = "block";
                        replaceCardSecAssociate.className = 'reissueCardReplace reissueCardReplaceRedHead';
                        replaceSecAssociateSub.className = 'replaceRightBorderSection';
                        replaceCardSecMain.style.display = "none";
                        replaceCardNo.style.display = "block";
                        errorMessageCheck = false;
                    }
                }
            }

            return errorMessageCheck;

        }
 
    </script>
    <div id="mainContent">
        <div class="ccBlueHeaderSection" id="divManageCardForm" runat="server">
            <div class="cc_bluehead">
                <h3>
                    <label for="Customer Cards">
                        <asp:Localize ID="lclCustomerCards" runat="server" Text="Customer Cards" meta:resourcekey="lclCustomerCardsResource1"></asp:Localize></label></h3>
            </div>
            <div class="replacementCardNo" id="divReplaceCardMessage" runat="server" style="display: none;">
                <div class="noteText errorFields">
                    <asp:Label runat="server" ID="lblPrimary"></asp:Label>
                </div>
                <div class="noteText errorFields">
                    <asp:Label runat="server" ID="lblAsscociate"></asp:Label>
                </div>
            </div>
            <div class="replacementCardNo" id="divReplaceCard" runat="server" style="display: none;">
                <label for="cardNumber">
                    <asp:Localize ID="lclcardNumber" runat="server" Text="CardNumber" meta:resourcekey="lclcardNumberResource1"></asp:Localize></label>
                <div class="inputFields">
                    <asp:TextBox ID="txtReplaceCardNo" runat="server" meta:resourcekey="txtReplaceCardNoResource1"></asp:TextBox>
                </div>
            </div>
            <div class="cc_body">
                <asp:Label runat="server" ID="lblSuccessMessage" class="alertMsgs" meta:resourcekey="lblSuccessMessageResource1"></asp:Label>
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">
                    <%=Resources.CSCGlobal.ValidateReqFields%></span>
                <div class="reissueClubcardAcct" id="divMain" runat="server">
                    <div class="clubcardAct_head">
                        <h4>
                            <asp:Literal ID="ltrCardHeader" runat="server" meta:resourcekey="ltrCardHeaderResource1"></asp:Literal></h4>
                    </div>
                    <asp:Repeater ID="rptCardDetails" runat="server" OnItemDataBound="rptCardDetailsMain_ItemDataBound"
                        OnItemCommand="rptCardDetails_ItemCommand">
                        <HeaderTemplate>
                            <table class="reissueCardHolderTbl">
                                <thead>
                                    <tr>
                                        <th>
                                            <label for="Card number" style="width: 100%">
                                                <asp:Localize ID="lclCardnumber" runat="server" Text="Card number" meta:resourcekey="lclCardnumberResource2"></asp:Localize></label>
                                        </th>
                                        <th>
                                            <label for="Issuedate" style="width: 100%">
                                                <asp:Localize ID="lclIssuedate" runat="server" Text="Issue date" meta:resourcekey="lclIssuedateResource1"></asp:Localize></label>
                                        </th>
                                        <th>
                                            <label for="Card status" style="width: 100%">
                                                <asp:Localize ID="lclCardstatus" runat="server" Text="Card status" meta:resourcekey="lclCardstatusResource1"></asp:Localize></label>
                                        </th>
                                        <th id="thTypeofcard" runat="server">
                                            <label for="Type of card" style="width: 100%">
                                                <asp:Localize ID="lclTypeofcard" runat="server" Text="Type of card" meta:resourcekey="lclTypeofcardResource1"></asp:Localize></label>
                                        </th>
                                        <th class="last" id="thReplace" runat="server">
                                            <label for="Replace" style="width: 100%">
                                                <asp:Localize ID="lclReplace" runat="server" Text="Replace" meta:resourcekey="lclReplaceResource1"></asp:Localize></label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltrClubcardNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>'></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrIssueDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CardIssuedDate") %>'></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="ltrCardStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'
                                        meta:resourcekey="ltrCardStatusResource2"></asp:LinkButton>
                                    <asp:Literal ID="lblCardStatus" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'></asp:Literal>
                                </td>
                                <td id="tdTypeofCard" runat="server">
                                    <asp:Literal ID="ltrTypeofCard" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardTypeDesc") %>'></asp:Literal>
                                </td>
                                <td id="tdrdbReplaceMain" runat="server">
                                    <asp:RadioButton ID="rdbReplaceMain" runat="server" Visible="False" GroupName="rdbOption"
                                        meta:resourcekey="rdbReplaceMainResource2" />
                                    <asp:HiddenField ID="hdnCardNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate">
                                <td>
                                    <asp:Literal ID="ltrClubcardNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>'></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrIssueDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CardIssuedDate") %>'></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="ltrCardStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'
                                        meta:resourcekey="ltrCardStatusResource1"></asp:LinkButton>
                                    <asp:Literal ID="lblCardStatus" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'></asp:Literal>
                                </td>
                                <td id="tdTypeofCard" runat="server">
                                    <asp:Literal ID="ltrTypeofCard" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardTypeDesc") %>'></asp:Literal>
                                </td>
                                <td id="tdrdbReplaceMain" runat="server">
                                    <asp:RadioButton ID="rdbReplaceMain" runat="server" Visible="False" GroupName="rdbOption"
                                        meta:resourcekey="rdbReplaceMainResource1" />
                                    <asp:HiddenField ID="hdnCardNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>' />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <span>
                        <!--Empty-->
                    </span>
                </div>
                <div class="reissueCardReplace" runat="server" id="divReplaceSecMain" style="display: none;">
                    <div class="clubcardAct_head">
                        <h4>
                            <select>
                                <option>
                                    <%=Resources.CSCGlobal.CardOption%></option>
                            </select>
                        </h4>
                    </div>
                    <div class="ccRightBorderSection" id="divReplaceSecMainSub" runat="server">
                        <div class="orderRplcmnterrorFld" style="display: none">
                            <div class="ccPurpleGreySection">
                                <div class="cc_purpleGreyhead">
                                    <h4 style="height: 20px">
                                        <label for="Replacement Request" style="width: 100%; height: 30px">
                                            <asp:Localize ID="lclReplacementRequest" runat="server" Text="Replacement Request"
                                                meta:resourcekey="lclReplacementRequestResource1"></asp:Localize></label></h4>
                                </div>
                                <div class="cc_greybody">
                                    <ul>
                                        <li>
                                            <label for="newCard">
                                                <strong>
                                                    <asp:Localize ID="lclnewCard" runat="server" Text="New card" meta:resourcekey="lclnewCardResource1"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioNewCardMain" runat="server" GroupName="OptionMain" meta:resourcekey="RadioNewCardMainResource1" />
                                        </li>
                                        <li>
                                            <label for="newFob" style="height: 30px;">
                                                <strong>
                                                    <asp:Localize ID="lclnewFob" runat="server" Text="New key fob (set of 2)" meta:resourcekey="lclnewFobResource1"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioNewFobMain" runat="server" GroupName="OptionMain" meta:resourcekey="RadioNewFobMainResource1" />
                                        </li>
                                        <li>
                                            <label for="newCardAndFob" style="height: 30px;">
                                                <strong>
                                                    <asp:Localize ID="lclnewCardAndFob" runat="server" Text="New card and 2 key fobs"
                                                        meta:resourcekey="lclnewCardAndFobResource1"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioNewCardnFobMain" runat="server" GroupName="OptionMain"
                                                meta:resourcekey="RadioNewCardnFobMainResource1" />
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="orderRplcmnterrorFld">
                            <div class="ccPurpleGreySection">
                                <div class="cc_purpleGreyhead">
                                    <h4 style="height: 20px">
                                        <label for="Reason" style="height: 30px">
                                            <asp:Localize ID="lclReasion" runat="server" Text="Reason" meta:resourcekey="lclReasionResource1"></asp:Localize></label></h4>
                                </div>
                                <div class="cc_greybody">
                                    <ul>
                                        <li>
                                            <label for="lost" style="height: 30px;">
                                                <strong>
                                                    <asp:Localize ID="lcllost" runat="server" Text="Lost my card / key fob" meta:resourcekey="lblLost"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioLostMain" runat="server" GroupName="ReasonMain" meta:resourcekey="RadioOtherResource1" />
                                        </li>
                                        <li>
                                            <label for="damaged">
                                                <strong>
                                                    <asp:Localize ID="lclstolen" runat="server" Text="Damaged" meta:resourcekey="lblDamaged"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioDamagedMain" runat="server" GroupName="ReasonMain" meta:resourcekey="RadioOtherResource1" />
                                        </li>
                                        <li>
                                            <label for="stolen">
                                                <strong>
                                                    <asp:Localize ID="Localize3" runat="server" Text="Stolen" meta:resourcekey="lblNone"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioStolenMain" runat="server" GroupName="ReasonMain" meta:resourcekey="RadioOtherResource1" />
                                        </li>
                                        <li style="height: 30px;display:none;">
                                            <label for="moreFobs" style="height: 30px;">
                                                <strong>
                                                    <asp:Localize ID="lclmoreFobs" runat="server" Text="Need more fobs for 
                                                your household" meta:resourcekey="lclmoreFobsResource1"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioMoreFobsMain" runat="server" GroupName="ReasonMain" meta:resourcekey="RadioMoreFobsMainResource1" />
                                        </li>
                                       
                                        <li style="display:none;">
                                            <label for="others">
                                                <strong>
                                                    <asp:Localize ID="lclothers" runat="server" Text="Other" meta:resourcekey="lclothersResource1"></asp:Localize></strong></label>
                                            <asp:RadioButton ID="RadioOtherMain" runat="server" GroupName="ReasonMain" meta:resourcekey="RadioOtherMainResource1" />
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <span>
                        <!--Empty-->
                    </span>
                </div>
                <%-- for horizontal row--%>
                <div class="reissueClubcardMain">
                    <div class="reissueClubcardAcct" id="divAssociate" runat="server" visible="false">
                        <div class="clubcardAct_head">
                            <h4>
                                <asp:Literal ID="ltrCardHeaderAssociate" runat="server" meta:resourcekey="ltrCardHeaderAssociateResource1"></asp:Literal></h4>
                        </div>
                        <asp:Repeater ID="rptCardDetailsAssociate" runat="server" OnItemDataBound="rptCardDetailsAssociate_ItemDataBound"
                            OnItemCommand="rptCardDetailsAssociate_ItemCommand">
                            <HeaderTemplate>
                                <table class="reissueCardHolderTbl">
                                    <thead>
                                        <tr>
                                            <th>
                                                <label for="Card number" style="width: 100%">
                                                    <asp:Localize ID="lclCardnumber" runat="server" Text="Card number" meta:resourcekey="lclCardnumberResource3"></asp:Localize></label>
                                            </th>
                                            <th>
                                                <label for="Issue date" style="width: 100%">
                                                    <asp:Localize ID="lclIssuedate" runat="server" Text="Issue date" meta:resourcekey="lclIssuedateResource2"></asp:Localize></label>
                                            </th>
                                            <th>
                                                <label for="Card status" style="width: 100%">
                                                    <asp:Localize ID="lclCardstatus" runat="server" Text="Card status" meta:resourcekey="lclCardstatusResource2"></asp:Localize></label>
                                            </th>
                                            <th id="thTypeofcard" runat="server">
                                                <label for="Type of card" style="width: 100%">
                                                    <asp:Localize ID="lclTypeofcard" runat="server" Text="Type of card" meta:resourcekey="lclTypeofcardResource2"></asp:Localize></label>
                                            </th>
                                            <th class="last" id="thReplace" runat="server">
                                                <label for="Replace" style="width: 100%">
                                                    <asp:Localize ID="lclReplace" runat="server" Text="Replace" meta:resourcekey="lclReplaceResource2"></asp:Localize></label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltrClubcardNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltrIssueDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CardIssuedDate") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="ltrCardStatusAssoc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'
                                            meta:resourcekey="ltrCardStatusAssocResource1"></asp:LinkButton>
                                        <asp:Literal ID="lblCardStatusAssoc" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'></asp:Literal>
                                    </td>
                                    <td id="tdTypeofCard" runat="server">
                                        <asp:Literal ID="ltrTypeofCard" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardTypeDesc") %>'></asp:Literal>
                                    </td>
                                    <td id="tdrdbReplaceMain" runat="server">
                                        <asp:RadioButton ID="rdbReplaceAssociate" runat="server" Visible="False" GroupName="rdbOption"
                                            meta:resourcekey="rdbReplaceAssociateResource2" />
                                        <asp:HiddenField ID="hdnCardNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate">
                                    <td>
                                        <asp:Literal ID="ltrClubcardNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltrIssueDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CardIssuedDate") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="ltrCardStatusAssoc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'
                                            meta:resourcekey="ltrCardStatusResource3"></asp:LinkButton>
                                        <asp:Literal ID="lblCardStatusAssoc" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardStatusDescEnglish") %>'></asp:Literal>
                                    </td>
                                    <td id="tdTypeofCard" runat="server">
                                        <asp:Literal ID="ltrTypeofCard" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClubcardTypeDesc") %>'></asp:Literal>
                                    </td>
                                    <td id="tdrdbReplaceMain" runat="server">
                                        <asp:RadioButton ID="rdbReplaceAssociate" runat="server" Visible="False" GroupName="rdbOption"
                                            meta:resourcekey="rdbReplaceAssociateResource1" />
                                        <asp:HiddenField ID="hdnCardNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ClubCardID") %>' />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                </tbody> </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <span>
                            <!--Empty-->
                        </span>
                    </div>
                    <div class="reissueCardReplace" runat="server" id="divReplaceSecAssociate" style="display: none;">
                        <div class="clubcardAct_head">
                            <h4>
                                <select>
                                    <option>
                                        <%=Resources.CSCGlobal.CardOption%></option>
                                </select>
                            </h4>
                        </div>
                        <div class="ccRightBorderSection" id="divReplaceSecAssociateSub" runat="server">
                            <div class="orderRplcmnterrorFld" style="display: none">
                                <div class="ccPurpleGreySection">
                                    <div class="cc_purpleGreyhead">
                                        <h4 style="height: 20px">
                                            <label for="Replacement Request" style="height: 30px; width: 100%">
                                                <asp:Localize ID="lclReplacement" runat="server" Text="Replacement Request" meta:resourcekey="lclReplacementResource1"></asp:Localize></label></h4>
                                    </div>
                                    <div class="cc_greybody">
                                        <ul>
                                            <li>
                                                <label for="newCard">
                                                    <strong>
                                                        <asp:Localize ID="lclnewCard1" runat="server" Text="New card" meta:resourcekey="lclnewCard1Resource1"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioNewCard" runat="server" GroupName="Option" meta:resourcekey="RadioNewCardResource1" />
                                            </li>
                                            <li>
                                                <label for="newFob" style="height: 30px">
                                                    <strong>
                                                        <asp:Localize ID="lclnewFob1" runat="server" Text="New key fob (set of 2)" meta:resourcekey="lclnewFob1Resource1"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioNewFob" runat="server" GroupName="Option" meta:resourcekey="RadioNewFobResource1" />
                                            </li>
                                            <li>
                                                <label for="newCardAndFob" style="height: 30px">
                                                    <strong>
                                                        <asp:Localize ID="lclnewCardAndFob1" runat="server" Text="New card and 2 key fobs"
                                                            meta:resourcekey="lclnewCardAndFob1Resource1"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioNewCardnFob" runat="server" GroupName="Option" meta:resourcekey="RadioNewCardnFobResource1" />
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="orderRplcmnterrorFld">
                                <div class="ccPurpleGreySection">
                                    <div class="cc_purpleGreyhead">
                                        <h4 style="height: 20px">
                                            <label for="Reason" style="height: 30px">
                                                <asp:Localize ID="lclReasion1" runat="server" Text="Reason" meta:resourcekey="lclReasion1Resource1"></asp:Localize></label></h4>
                                    </div>
                                    <div class="cc_greybody">
                                        <ul>
                                            <li>
                                                <label for="lost" style="height: 30px">
                                                    <strong>
                                                        <asp:Localize ID="lcllost2" runat="server" Text="Lost or stolen card/key fobs" meta:resourcekey="lblLost"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioLost" runat="server" GroupName="Reason" meta:resourcekey="RadioOtherResource1" />
                                            </li>
                                            <li>
                                                <label for="damaged">
                                                    <strong>
                                                        <asp:Localize ID="Localize2" runat="server" Text="Damaged card/key fobs" meta:resourcekey="lblDamaged"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioDamaged" runat="server" GroupName="Reason" meta:resourcekey="RadioOtherResource1" />
                                            </li>
                                            <li>
                                                <label for="stolen">
                                                    <strong>
                                                        <asp:Localize ID="lclstolen2" runat="server" Text="No previous card/key fobs " meta:resourcekey="lblNone"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioStolen" runat="server" GroupName="Reason" meta:resourcekey="RadioOtherResource1" />
                                            </li>
                                            <li style="display:none">
                                                <label for="moreFobs" style="height: 30px">
                                                    <strong>
                                                        <asp:Localize ID="lclmoreFobs2" runat="server" Text="Need more fobs for your household"
                                                            meta:resourcekey="lclmoreFobs2Resource1"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioMoreFobs" runat="server" GroupName="Reason" meta:resourcekey="RadioMoreFobsResource1" />
                                            </li>
                                            <listyle="display:none">
                                                <label for="others">
                                                    <strong>
                                                        <asp:Localize ID="lclothers2" runat="server" Text="Other" meta:resourcekey="lclothers2Resource1"></asp:Localize></strong></label>
                                                <asp:RadioButton ID="RadioOther" runat="server" GroupName="Reason" meta:resourcekey="RadioOtherResource1" />
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <span>
                            <!--Empty-->
                        </span>
                    </div>
                    <div class="noteText errorFields" id="divNonStandardCardMsg" runat="server" style="display: none;">
                        <label for="servicereissuing" style="width: 100%">
                            <asp:Localize ID="lclservicereissuing" runat="server" Text="This service is for reissuing standard clubcards only."
                                meta:resourcekey="lclservicereissuingResource1"></asp:Localize></label>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div class="cc_bluehead" style="height: 30px" runat="server" id="divAddCard">
                            <h3>
                                <label for="CusStatus">
                                    <asp:Localize ID="lclCusStatus" runat="server" Text="Add Card" meta:resourcekey="lclstatusofcustomerResource1"></asp:Localize></label></h3>
                        </div>
                        </br>
                        <div id="dvMainAssCus" runat="server" visible="false">
                            <table width="75%">
                                <tr>
                                    <td class="style3">
                                        <asp:RadioButton ID="RBMain" runat="server" GroupName="ReplacePrimary1" Checked="True"
                                            AutoPostBack="true" OnCheckedChanged="RBMain_CheckedChanged" /><b><asp:Label ID="Label1"
                                                Text="Main Customer" runat="server"></asp:Label></b>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RBAssociative" runat="server" GroupName="ReplacePrimary1" AutoPostBack="true"
                                            OnCheckedChanged="RBAssociative_CheckedChanged" /><b><asp:Label ID="Label2" Text="Associative Customer"
                                                runat="server"></asp:Label></b>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <p class="pageAction" id="pbtnConfirm" runat="server">
                            <table id="AddCardToAccount" runat="server" visible="false">
                                <tr>
                                    <td class="style1">
                                        <label for="Card No" style="width: 100%">
                                            <asp:Localize ID="lclCardNo1" runat="server" Text="Card No." meta:resourcekey="lclCardNo1Resource1"></asp:Localize></label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtCardNumber" MaxLength="18" runat="server" meta:resourcekey="txtCardNumberResource1"></asp:TextBox>
                                        <span class="errorFields" id="spanCardNumber" style="<%=spanCardNumber%>">
                                            <%=errMsgCardNumber%></span>
                                    </td>
                                    <td class="style1">
                                        <label for="Replace Primary" style="width: 100%">
                                            <asp:Localize ID="lclReplaceSummary" runat="server" Text="Replace Primary" meta:resourcekey="lclReplaceSummaryResource1"></asp:Localize></label>
                                    </td>
                                    <td class="style2">
                                        <asp:RadioButton ID="rdbReplacePrimaryYes" runat="server" GroupName="ReplacePrimary"
                                            Text="Yes" meta:resourcekey="rdbReplacePrimaryYesResource1" />
                                        <asp:RadioButton ID="rdbReplacePrimaryNo" runat="server" Checked="True" GroupName="ReplacePrimary"
                                            Width="100px" Text="No" meta:resourcekey="rdbReplacePrimaryNoResource1" />
                                    </td>
                                    <td class="style1">
                                        <asp:ImageButton ID="imgbtnAddCard" runat="server" CssClass="imgBtn" ImageUrl="~/I/AddCard.gif"
                                            OnClick="imgbtnAddCard_Click" meta:resourcekey="imgbtnAddCardResource1" />
                                    </td>
                                </tr>
                            </table>
                            <div style="float: left;">
                                <asp:ImageButton ID="imgbtnChangePrimary" runat="server" CssClass="imgBtn" ImageUrl="~/I/ChangePrimary.gif"
                                    OnClick="imgbtnChangePrimary_Click" meta:resourcekey="imgbtnChangePrimaryResource1" />
                            </div>
                            <asp:ImageButton ID="orderConfirm" CssClass="imgBtn" ImageUrl="~/I/Confirmbtn.JPG"
                                runat="server" OnClientClick="return errorMessageCheck();" OnClick="orderConfirm_Click"
                                meta:resourcekey="orderConfirmResource1" />
                        </p>
                    </div>
                    <div class="noteText errorFields" id="divErrorMessage" runat="server" style="display: none;">
                    </div>
                    <p class="noteText errorFields" id="pInProcessMsg" runat="server" style="display: none;">
                        <label for="reissue" style="width: 100%">
                            <asp:Localize ID="reissued" runat="server" Text="There is already a re-issue order processing for this account."
                                meta:resourcekey="reissuedResource1"></asp:Localize></label><br />
                        <br />
                        <label for="normalDelivary" style="width: 100%">
                            <asp:Localize ID="lclNrmlDelivary" runat="server" Text="Inform the Customer that the delivery is normally within 3 weeks of their order."
                                meta:resourcekey="lclNrmlDelivaryResource1"></asp:Localize></label>
                    </p>
                    <p class="noteText errorFields" id="pInConfirmMsg" runat="server" style="display: none;">
                        <label for="processReq" style="width: 100%">
                            <asp:Localize ID="processReq" runat="server" Text="Replacement request has been processed successfully."
                                meta:resourcekey="processReqResource1"></asp:Localize></label><br />
                        <br />
                        <label for="normalDelivary" style="width: 100%">
                            <asp:Localize ID="lclNrmlDelivary1" runat="server" Text="Inform the Customer that the delivery is normally within 3 weeks of their order."
                                meta:resourcekey="lclNrmlDelivary1Resource1"></asp:Localize></label>
                    </p>
                    <p class="noteText errorFields" id="pMaxOrdersReached" runat="server" style="display: none;">
                        <label for="cardReissue" style="width: 100%">
                            <asp:Localize ID="lclcardReissue" runat="server" Text="Inform Customer that they are no longer able to request Card Re-Issues through their"
                                meta:resourcekey="lclcardReissueResource1"></asp:Localize></label>
                        <label for="cconlineAcc" style="width: 100%">
                            <asp:Localize ID="lclcconlineAcc" runat="server" Text="Clubcard Online account as they have reached the online re-issue threshold."
                                meta:resourcekey="lclcconlineAccResource1"></asp:Localize></label>
                    </p>
                    <p class="noteText errorFields" id="pInvalidReason" runat="server" style="display: none;">
                        <label for="cardReissue" style="width: 100%">
                            <asp:Localize ID="lclInvalidresoncode" runat="server" Text="Please select Reason code"
                                meta:resourcekey="lclReasoncodeValidationResource1"></asp:Localize></label>
                    </p>
                    <%--<asp:Panel ID="pnlCards" runat="server" >
                </asp:Panel>--%>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBox" class="modalBox" style="display: none;">
        <div>
            <table class="vouchDtlTbl">
                <thead>
                    <tr>
                        <th class="rounded-company first">
                            <label for="EditCardDetails">
                                <asp:Localize ID="lclEditCardDetails" runat="server" Text="Edit Card Details" meta:resourcekey="lclEditCardDetailsResource1"></asp:Localize></label>
                        </th>
                        <th class="rounded-q4 pointDetailPopup">
                            <!--Empty-->
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td class="rounded-Greyfoot-left">
                            <!--Empty-->
                        </td>
                        <td class="rounded-Greyfoot-right">
                            <!--Empty-->
                        </td>
                    </tr>
                </tfoot>
                <tbody>
                    <tr class="alternate">
                        <td>
                            <label for="Status">
                                <asp:Localize ID="lclStatus1" runat="server" Text="Status" meta:resourcekey="lclStatus1Resource1"></asp:Localize></label>
                        </td>
                        <td class="last">
                            <asp:DropDownList ID="ddlStatus" runat="server" meta:resourcekey="ddlStatusResource1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="last">
                        </td>
                    </tr>
                    <tr class="alternate">
                        <td>
                        </td>
                        <td class="last">
                        </td>
                    </tr>
                    <tr>
                        <td class="lastRw" colspan="2">
                            <asp:ImageButton ID="imgConfirm" ImageUrl="~/I/confirm.gif" runat="server" meta:resourcekey="imgConfirmResource1" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <p class="pageAction" style="margin-bottom: 10px">
            <a onclick="javascript:window.close();" href="javascript:void(0);">
                <asp:Image ID="btnCloseThisWindow" CssClass="imgBtn" AlternateText="Close this window"
                    ImageUrl="~/I/closethiswindow.gif" runat="server" meta:resourcekey="btnCloseThisWindowResource1" /></a>
        </p>
        <span class="voucherBtm">&nbsp;</span>
    </div>
    <asp:HiddenField ID="hdnAssCusID" runat="server" />
    <asp:HiddenField ID="hdnMainCusId" runat="server" />
    <asp:HiddenField ID="hdnSelCustID" runat="server" Value="" />
    <asp:HiddenField ID="hdnNumofClubcardRecforASS" runat="server" />
    <asp:HiddenField ID="hdnNumberofClubcardforMain" runat="server" />
    <asp:HiddenField ID="hdnIsAddCard" runat="server" Value="false" />
    <asp:HiddenField ID="hdnPassMainCusId" runat="server" />
    <asp:HiddenField ID="hdnPassAssCusID" runat="server" />
    <asp:HiddenField ID="hdnClubcardnumberreg" runat="server" Value="" />
    <asp:HiddenField ID="hdnmaxreplacementsts" runat="server" Value="false" />
    <asp:HiddenField ID="hdnPrimaryCustomerUUID" runat="server" Value="" />
    <asp:HiddenField ID="hdnAssociateCustomerUUID" runat="server" Value="" />
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            height: 14px;
        }
        #AddCardToAccount
        {
            height: 50px;
            width: 336px;
            margin-left: 0px;
        }
        .style2
        {
            height: 14px;
            width: 98px;
        }
        .style3
        {
            width: 162px;
        }
    </style>
</asp:Content>
