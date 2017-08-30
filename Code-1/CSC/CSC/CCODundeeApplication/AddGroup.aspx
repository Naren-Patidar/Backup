<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddGroup.aspx.cs"
    Inherits="CCODundeeApplication.AddGroup" Title="Add Group" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="PageContainer" runat="server">

    <script src="JS/General.js" type="text/javascript"></script>

    <script src="JS/jquery-1.3.2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function ValidateFields() 
        {
            //var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
            

            var errMsgGroupName = '<%=Resources.CSCGlobal.errMsgGroupName %>';
            var errorFlag = "";
            var errMsgvalid = '<%=Resources.CSCGlobal.ValidGrpMsg %>'; //Please Enter a valid Group Name.";
            var errMsgvalidDes = '<%=Resources.CSCGlobal.errMsgvalidDes %>';
            var regSurName = /^[a-zA-Z0-9_ ]*$/;
            var groupName = trim(document.getElementById("<%=txtGroupName.ClientID%>").value);

                if (groupName == "") 
                {
                    //To clear the error messages if already displayed.
                    
                     document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = '';
                     document.getElementById("<%=txtGroupName.ClientID%>").className = '';
                     errorFlag="Error";
                }
                else 
                {
                   document.getElementById("<%=spnErrorMsg.ClientID%>").style.display = 'none';
                }

            errorFlag = ValidateTextBox("<%=txtGroupName.ClientID%>", regSurName, true, false, "spanValidName", errMsgvalid);
            errorFlag =errorFlag + ValidateTextBox("<%=txtDescription.ClientID%>", regSurName, true, false, "spanValidDesc", errMsgvalidDes);
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
                <span class="errorFields" runat="server" id="spnErrorMsg" style="display: none;">
                <asp:Localize ID="lclErrMsg" runat="server" meta:resourcekey="ErrMsg"></asp:Localize></span>
                <div class="UserDetails">
                    <h3><asp:Localize ID="lclHeader2" runat="server" meta:resourcekey="Header2"></asp:Localize>
                       </h3>
                    <p>
                        &nbsp;</p>
                </div>
                <div class="EmptyDiv">
                </div>
                <div style="width: 60%">
                    <br />
                    <ul class="customer">
                        <li>
                            <label for="UserName"> <asp:Localize ID="lclGroupName" runat="server" meta:resourcekey="lblGroupName"></asp:Localize>
                                <img class="required" src="I/asterisk.gif" alt="Required" /></label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtGroupName" name="UserName" type="text" 
                                    MaxLength="20" meta:resourcekey="txtGroupNameResource1" />
                                 <span class="errorFields" id="spanValidName" style="<%=spanValidName%>"><%=errMsgvalid%></span>
                            </div>
                        </li>
                        <li>
                            <label for="FirstName"><asp:Localize ID="lclDescription" runat="server" meta:resourcekey="lblDescription"></asp:Localize>
                             </label>
                            <div class="inputFields">
                                <asp:TextBox runat="server" ID="txtDescription" name="FirstName" type="text" 
                                    meta:resourcekey="txtDescriptionResource1" />
                               <span class="errorFields" id="spanValidDesc" style="<%=spanValidDesc%>"><%=errMsgvalidDes%></span>
                            </div>
                        </li>
                    </ul>
                    <ul class="customer">
                        <li>
                            <div class="inputFields" style="padding-left: 175px;">
                                <asp:Button ID="btnAdd" runat="server" Text="ADD" BackColor="#49BCD7" OnClientClick="return ValidateFields()"
                                    ForeColor="White" Font-Bold="True" Height="24px" Width="38px" 
                                    OnClick="btnAdd_Click" meta:resourcekey="btnAddResource1" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
