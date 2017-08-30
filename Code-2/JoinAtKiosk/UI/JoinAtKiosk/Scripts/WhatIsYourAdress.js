var resources;
var finalCtrl = "";

function Postcode_Check() {
    var sLN = document.getElementById('txtUKPostCode').value;
    sLN = trim(sLN);
    var lenFn = sLN.length;
    if (lenFn >= 2) //If input string valid
    {
        document.getElementById('plokkAdreessGrey').style.display = 'none';
        document.getElementById('plokkAdreess').style.display = 'block';
    }
    else {
        document.getElementById('plokkAdreessGrey').style.display = 'block';
        document.getElementById('plokkAdreess').style.display = 'none';
        return false;
    }
}

function trim(str) {
    str = str.replace(/^\s+|\s+$/g, "");
    return str;
}

function UKPostcode_Click() {
    ClearFocusAll();
    if (document.getElementById('divUKPostCode1') != null) {
        document.getElementById('lbltitle').innerHTML = resources[5];
        document.getElementById('divUKPostCode1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('divUKPostCode2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('divUKPostCode3').className = 'input322 paddingtop10';
        document.getElementById('txtUKPostCode').disabled = false;
        FocusObject('txtUKPostCode');
        //document.getElementById('txtUKPostCode').value = document.getElementById('txtUKPostCode').value;

        document.getElementById('plokkAdreessGrey').style.display = 'none';
        document.getElementById('plokkAdreess').style.display = 'block';
        document.getElementById('divconfirm').style.display = 'none';
        document.getElementById('divnextbutton').style.display = 'none';
        Postcode_Check();
    }
}

function ClearFocusAll() {
    if (document.getElementById('AddressLine1div1') != null) {
        document.getElementById('AddressLine1div1').className = '';
        document.getElementById('AddressLine1div2').className = 'inputtext twolines';
        document.getElementById('AddressLine1div3').className = 'input322';

        document.getElementById('AddressLine1div3').disabled = false;
        document.getElementById('txtAddressLine1').disabled = false;
        FocusObject('txtAddressLine1');
        //document.getElementById('txtAddressLine1').value = document.getElementById('txtAddressLine1').value;
    }
    if (document.getElementById('AddressLine2div1') != null) {
        document.getElementById('AddressLine2div1').className = '';
        document.getElementById('AddressLine2div2').className = 'inputtext';
        document.getElementById('AddressLine2div3').className = 'input322';

        document.getElementById('AddressLine2div3').disabled = false;
        document.getElementById('txtAddressLine2').disabled = false;
        FocusObject('txtAddressLine2');
        //document.getElementById('txtAddressLine2').value = document.getElementById('txtAddressLine2').value;
    }
    if (document.getElementById('AddressLine3div1') != null) {
        document.getElementById('AddressLine3div1').className = '';
        document.getElementById('AddressLine3div2').className = 'inputtext';
        document.getElementById('AddressLine3div3').className = 'input322';

        document.getElementById('AddressLine3div3').disabled = false;
        document.getElementById('txtAddressLine3').disabled = false;
        FocusObject('txtAddressLine3');
        //document.getElementById('txtAddressLine3').value = document.getElementById('txtAddressLine3').value;
    }
    if (document.getElementById('AddressLine4div1') != null) {
        document.getElementById('AddressLine4div1').className = '';
        document.getElementById('AddressLine4div2').className = 'inputtext';
        document.getElementById('AddressLine4div3').className = 'input322';

        document.getElementById('AddressLine4div3').disabled = false;
        document.getElementById('txtAddressLine4').disabled = false;
        FocusObject('txtAddressLine4');
        //document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
    }
    if (document.getElementById('AddressLine5div1') != null) {
        document.getElementById('AddressLine5div1').className = '';
        document.getElementById('AddressLine5div2').className = 'inputtext';
        document.getElementById('AddressLine5div3').className = 'input322';

        document.getElementById('AddressLine5div3').disabled = false;
        document.getElementById('txtAddressLine5').disabled = false;
        FocusObject('txtAddressLine5');
        //document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
    }
    if (document.getElementById('divGrPostcode1') != null) {
        document.getElementById('divGrPostcode1').className = '';
        document.getElementById('divGrPostcode2').className = 'inputtext';
        document.getElementById('divGrPostcode3').className = 'input322';

        document.getElementById('divGrPostcode3').disabled = false;
        document.getElementById('txtGrPostcode').disabled = false;
        FocusObject('txtGrPostcode');
        //document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
    }
    if (document.getElementById('divUKPostCode1') != null) {
        document.getElementById('divUKPostCode1').className = '';
        document.getElementById('divUKPostCode2').className = 'inputtext';
        document.getElementById('divUKPostCode3').className = 'input322';

        document.getElementById('divUKPostCode3').disabled = false;
        document.getElementById('txtUKPostCode').disabled = false;
        FocusObject('txtUKPostCode');
        //document.getElementById('txtUKPostCode').value = document.getElementById('txtUKPostCode').value;
        
        document.getElementById('plokkAdreessGrey').style.display = 'none';
        document.getElementById('plokkAdreess').style.display = 'none';
    }
}

function AddressLine2_Click() {
    document.getElementById('lbltitle').innerHTML = resources[1];
    document.getElementById('divconfirm').style.display = 'none';

    ClearFocusAll();
 
    if (document.getElementById('AddressLine2div1') != null) {
        document.getElementById('AddressLine2div1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('AddressLine2div2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('AddressLine2div3').className = 'input322 paddingtop10';

        document.getElementById('AddressLine2div3').disabled = false;
        document.getElementById('txtAddressLine2').disabled = false;
        FocusObject('txtAddressLine2');
        //document.getElementById('txtAddressLine2').value = document.getElementById('txtAddressLine2').value;
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonstreet nextbuttontownfocus";

        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divsummary').style.display = 'none';
    }
    if (finalCtrl == "ADDRESSLINE2") {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';

        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    
    return false;
}


function AddressLine1_Enable() {
    document.getElementById('divconfirm').style.display = 'none';
    ClearFocusAll();
    if (document.getElementById('AddressLine1div1') != null) {
        document.getElementById('lbltitle').innerHTML = resources[0];

        document.getElementById('AddressLine1div1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('AddressLine1div2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('AddressLine1div3').className = 'input322 paddingtop10';
        document.getElementById('AddressLine1div3').disabled = false;
        document.getElementById('txtAddressLine1').disabled = false;
        FocusObject('txtAddressLine1');
        //document.getElementById('txtAddressLine1').focus();
        //document.getElementById('txtAddressLine1').value = document.getElementById('txtAddressLine1').value;
    }
    document.getElementById('divspacer').className = 'buttonspacer1';
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';
    document.getElementById('divnextbutton').style.display = 'block';
    document.getElementById('divnextbutton').className = "nextbuttonstreet nextbuttonstreetfocus";
    document.getElementById('divsummary').style.display = 'none';
   
}

function AddressLine1_Click() {
    document.getElementById('divconfirm').style.display = 'none';
    if (document.getElementById('divUKPostCode1') != null) {
        AddressLine1_Enable();
    }
    else {
        AddressLine1_Enable();
    }
    if (document.getElementById('hdnCountryCode').value != "en-GB") {
        document.getElementById('lbltitle').innerHTML = resources[0];
        document.getElementById('AddressLine1div1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('AddressLine1div2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('AddressLine1div3').className = 'input322 paddingtop10';
        document.getElementById('AddressLine1div3').disabled = false;
        document.getElementById('txtAddressLine1').disabled = false;
        FocusObject('txtAddressLine1');
        //document.getElementById('txtAddressLine1').focus();
        //document.getElementById('txtAddressLine1').value = document.getElementById('txtAddressLine1').value;
       
    }
    if (finalCtrl == "ADDRESSLINE1") {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';

        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    return false;
}

function AddressLine3_Click() {
    document.getElementById('lbltitle').innerHTML = resources[2];
    document.getElementById('divconfirm').style.display = 'none';
    ClearFocusAll();
    if (document.getElementById('AddressLine3div1') != null) {
        document.getElementById('AddressLine3div1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('AddressLine3div2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('AddressLine3div3').className = 'input322 paddingtop10';

        document.getElementById('txtAddressLine3').disabled = false;
        FocusObject('txtAddressLine3');
        //document.getElementById('txtAddressLine3').value = document.getElementById('txtAddressLine3').value;
    }

    if (document.getElementById('AddressLine4div1') != null) {
        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonaddressline4";
        document.getElementById('divconfirm').style.display = 'none';
        document.getElementById('divsummary').style.display = 'none';

    }
    else if (document.getElementById('AddressLine5div1') != null) {
        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonaddressline5";
        document.getElementById('divconfirm').style.display = 'none';
        document.getElementById('divsummary').style.display = 'none';

    }
    else if (document.getElementById('divGrPostcode1') != null) {
        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonpostcode";
        document.getElementById('divconfirm').style.display = 'none';
        document.getElementById('divsummary').style.display = 'none';

    }
    else {
        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
        else {
            document.getElementById('divspacer').className = 'buttonspacer2';
            document.getElementById('divconfirm').style.display = 'block';
            document.getElementById('divsummary').style.display = 'none';
        }
    }
    if (finalCtrl == "ADDRESSLINE3") {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';
        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }

    return false;
}

function AddressLine4_Click() {
    document.getElementById('lbltitle').innerHTML = resources[3];
    document.getElementById('divconfirm').style.display = 'none';
    ClearFocusAll();
    if (document.getElementById('AddressLine4div1') != null) {
        document.getElementById('AddressLine4div1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('AddressLine4div2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('AddressLine4div3').className = 'input322 paddingtop10';
        document.getElementById('AddressLine4div3').disabled = false;
        document.getElementById('txtAddressLine4').disabled = false;
        FocusObject('txtAddressLine4');
        //document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonaddressline5";
        document.getElementById('divconfirm').style.display = 'none';
    }
    if (document.getElementById('AddressLine5div1') == null && document.getElementById('divGrPostcode1') != null) {
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonpostcode";
        document.getElementById('divconfirm').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divsummary').style.display = 'none';
    }
  
    document.getElementById('divspacer').className = 'buttonspacer1';
    document.getElementById('divsummary').style.display = 'none';
    if (finalCtrl == "ADDRESSLINE4") {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';

        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    return false;
}

function AddressLine5_Click() {
    document.getElementById('lbltitle').innerHTML = resources[4];
    document.getElementById('divconfirm').style.display = 'none';
    ClearFocusAll();
   
    if (document.getElementById('AddressLine5div1') != null) {
        document.getElementById('AddressLine5div1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('AddressLine5div2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('AddressLine5div3').className = 'input322 paddingtop10';

        document.getElementById('AddressLine5div3').disabled = false;
        document.getElementById('txtAddressLine5').disabled = false;
        FocusObject('txtAddressLine5');
        //document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;

        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbuttonpostcode";
        document.getElementById('divconfirm').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divsummary').style.display = 'none';
    }
    if (document.getElementById('divGrPostcode1') == null) {
        document.getElementById('divspacer').className = 'buttonspacer1';
        document.getElementById('divsummary').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';
        document.getElementById('divsummary').style.display = 'none';
        document.getElementById('divs').style.display = 'none';
    }
    if (finalCtrl == "ADDRESSLINE5") {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';

        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    return false;
}

function AddressLine6_Click() {
    document.getElementById('lbltitle').innerHTML = resources[5];
    document.getElementById('divconfirm').style.display = 'none';
    ClearFocusAll();
    if (document.getElementById('divGrPostcode1') != null) {
        document.getElementById('divGrPostcode1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('divGrPostcode2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('divGrPostcode3').className = 'input322 paddingtop10';

        document.getElementById('divGrPostcode3').disabled = false;
        document.getElementById('txtGrPostcode').disabled = false;
        FocusObject('txtGrPostcode');
        //document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;

        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'none';
        }
        else {
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer2';
            document.getElementById('divconfirm').style.display = 'block';
            document.getElementById('divsummary').style.display = 'none';
            document.getElementById('divs').style.display = 'none';
        }

    }
    if (finalCtrl == "ADDRESSLINE6") {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';

        document.getElementById('divs').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    return false;
}


function Next_Value() {
    
    if (document.getElementById('AddressLine1div1') != null) {
        if (document.getElementById('AddressLine1div1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('AddressLine2div1') != null) {
                AddressLine2_Click();
                document.getElementById('txtAddressLine2').focus();
                document.getElementById('txtAddressLine2').value = document.getElementById('txtAddressLine2').value;
            }
            else if (document.getElementById('AddressLine3div1') != null) {
                AddressLine3_Click();
                document.getElementById('txtAddressLine3').focus();
                document.getElementById('txtAddressLine3').value = document.getElementById('txtAddressLine3').value;
            }
            else if (document.getElementById('AddressLine4div1') != null) {
                AddressLine4_Click();
                document.getElementById('txtAddressLine4').focus();
                document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
            }
            else if (document.getElementById('AddressLine5div1') != null) {
                AddressLine5_Click();
                document.getElementById('txtAddressLine5').focus();
                document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
            }
            else if (document.getElementById('divGrPostcode1') != null) {
                AddressLine6_Click();
                document.getElementById('txtGrPostcode').focus();
                document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
            }
            return false;
        }
    }
    if (document.getElementById('AddressLine2div1') != null) {
        if (document.getElementById('AddressLine2div1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('AddressLine3div1') != null) {
                AddressLine3_Click();
                document.getElementById('txtAddressLine3').focus();
                document.getElementById('txtAddressLine3').value = document.getElementById('txtAddressLine3').value;
            }
            else if (document.getElementById('AddressLine4div1') != null) {
                AddressLine4_Click();
                document.getElementById('txtAddressLine4').focus();
                document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
            }
            else if (document.getElementById('AddressLine5div1') != null) {
                AddressLine5_Click();
                document.getElementById('txtAddressLine5').focus();
                document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
            }
            else if (document.getElementById('divGrPostcode1') != null) {
                AddressLine6_Click();
                document.getElementById('txtGrPostcode').focus();
                document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
            }
            return false;
        }
    }
    if (document.getElementById('AddressLine3div1') != null) {
        if (document.getElementById('AddressLine3div1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('AddressLine4div1') != null) {
                AddressLine4_Click();
                document.getElementById('txtAddressLine4').focus();
                document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
            }
            else if (document.getElementById('AddressLine5div1') != null) {
                AddressLine5_Click();
                document.getElementById('txtAddressLine5').focus();
                document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
            }
            else if (document.getElementById('divGrPostcode1') != null) {
                AddressLine6_Click();
                document.getElementById('txtGrPostcode').focus();
                document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
            }
            return false;
        }
    }
    if (document.getElementById('AddressLine4div1') != null) {
        if (document.getElementById('AddressLine4div1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('AddressLine5div1') != null) {
                AddressLine5_Click();
                document.getElementById('txtAddressLine5').focus();
                document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
                return false;
            }
            else if (document.getElementById('divGrPostcode1') != null) {
                AddressLine6_Click();
                document.getElementById('txtGrPostcode').focus();
                document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
            }
            return false
        }
    }
    if (document.getElementById('AddressLine5div1') != null) {
        if (document.getElementById('AddressLine5div1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('divGrPostcode1') != null) {
                AddressLine6_Click();
                document.getElementById('txtGrPostcode').focus();
                document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
            }
            return false
        }
    }
}


function Loading() {

    var final = document.getElementById('lblCurrentField').value;
    final = final.split(",");
    var i = final.length - 1;
    finalCtrl = final[i];
    if (document.getElementById('divUKPostCode1') != null) {
        if (document.getElementById('divUKPostCode1').className == "focuspanel curved paddingtop-10") {
            document.getElementById('txtUKPostCode').disabled = false;
            FocusObject('txtUKPostCode');
            document.getElementById('txtUKPostCode').focus();
            document.getElementById('txtUKPostCode').value = document.getElementById('txtUKPostCode').value;
            Postcode_Check();
        }
    }
    else {
        AddressLine1_Click();
        document.getElementById('txtAddressLine1').focus();
        document.getElementById('txtAddressLine1').value = document.getElementById('txtAddressLine1').value;
        return false;
    }
   
    var errorMsg = document.getElementById('hdnErrorMsg').value;
    if (errorMsg != "") {
        if (errorMsg == "AddressLine1") {
            AddressLine1_Click();
            document.getElementById('txtAddressLine1').focus();
            document.getElementById('txtAddressLine1').value = document.getElementById('txtAddressLine1').value;
        }
        if (errorMsg == "AddressLine2") {
            AddressLine2_Click();
            document.getElementById('txtAddressLine2').focus();
            document.getElementById('txtAddressLine2').value = document.getElementById('txtAddressLine2').value;
        }
        if (errorMsg == "AddressLine3") {
            AddressLine3_Click();
            document.getElementById('txtAddressLine3').focus();
            document.getElementById('txtAddressLine3').value = document.getElementById('txtAddressLine3').value;
        }
        if (errorMsg == "AddressLine4") {
            AddressLine4_Click();
            document.getElementById('txtAddressLine4').focus();
            document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
        }
        if (errorMsg == "AddressLine5") {
            AddressLine5_Click();
            document.getElementById('txtAddressLine5').focus();
            document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
        }
        if (errorMsg == "GrPostcode") {
            AddressLine6_Click();
            document.getElementById('txtGrPostcode').focus();
            document.getElementById('txtGrPostcode').value = document.getElementById('txtGrPostcode').value;
        }
        if (errorMsg == "Postcode") {
            document.getElementById('divUKPostCode1').disabled = false;
            document.getElementById('txtUKPostCode').disabled = false;
            FocusObject('txtUKPostCode');
            document.getElementById('txtUKPostCode').focus();
            document.getElementById('txtUKPostCode').value = document.getElementById('txtUKPostCode').value;
            document.getElementById('plokkAdreess').style.display = 'block';
            document.getElementById('plokkAdreessGrey').style.display = 'none';
        }
    }
}

function ValidateControlData() {
    var regValidation = true;
    var ValidateString = document.getElementById('hdnValidateString').value;
    ValidateString = ValidateString.split("|");
    for (var i = 1; i < ValidateString.length; i++) {
        var control = controlList[i].split(":");
        var controlName = control[0];
        controlName = "txt" + controlName;
        if (document.getElementById(controlName) != null) {
            var validate = control[1].split(",");
            if (control[0] == "AddressLine1") {
                if (document.getElementById(controlName).value == "") {
                    if (validate[1] == "true") {
                        document.getElementById('divError').style.display = 'block';
                        var txt = "<%= Resources.GlobalResources.AddressLine1Error%>";

                        document.getElementById('lblError').innerHTML = txt;
                        regValidation = false;

                        return regValidation;
                    }

                    if (document.getElementById('AddressLine1div1') != null && regValue[1] == "true") {
                        if (document.getElementById('txtAddressLine1').value == "") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.AddressLine1Error%>";

                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                        }
                        else {
                            if (document.getElementById('hdnUkPostcode').value == "true") {
                                var value = document.getElementById('txtAddressLine1').value;
                                if (value.substr(0, 1) == "<") {
                                    document.getElementById('divError').style.display = 'block';
                                    var txt = "<%= Resources.GlobalResources.AddressLine1Error%>";

                                    document.getElementById('lblError').innerHTML = txt;
                                    regValidation = false;
                                }
                            }
                        }
                    }
                }
            }
            if (document.getElementById('AddressLine2div1') != null && regValue[1] == "true") {
                if (document.getElementById('txtAddressLine2').value == "") {
                    document.getElementById('divError').style.display = 'block';
                    var txt = "<%= Resources.GlobalResources.AddressLine2Error%>";

                    document.getElementById('lblError').innerHTML = txt;
                    regValidation = false;
                }
                else {
                    if (document.getElementById('hdnUkPostcode').value == "true") {
                        var value = document.getElementById('txtAddressLine1').value;
                        if (value.substr(0, 1) == "<") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.AddressLine2Error%>";

                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                        }
                    }
                }
            }
            if (document.getElementById('AddressLine3div1') != null && regValue[1] == "true") {
                if (document.getElementById('txtAddressLine3').value == "") {
                    document.getElementById('divError').style.display = 'block';
                    var txt = "<%= Resources.GlobalResources.InvalidPostcode%>";

                    document.getElementById('lblError').innerHTML = txt;
                    regValidation = false;
                }
                else {
                    if (document.getElementById('hdnUkPostcode').value == "true") {
                        var value = document.getElementById('txtAddressLine3').value;
                        if (value.substr(0, 1) == "<") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.AddressLine3Error%>";

                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                        }
                    }
                }
            }
            if (document.getElementById('AddressLine4div1') != null && regValue[1] == "true") {
                if (document.getElementById('txtAddressLine4').value == "") {
                    document.getElementById('divError').style.display = 'block';
                    var txt = "<%= Resources.GlobalResources.InvalidPostcode%>";

                    document.getElementById('lblError').innerHTML = txt;
                    regValidation = false;
                }
                else {
                    if (document.getElementById('hdnUkPostcode').value == "true") {
                        var value = document.getElementById('txtAddressLine4').value;
                        if (value.substr(0, 1) == "<") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.AddressLine4Error%>";

                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                        }
                    }
                }
            }
            if (document.getElementById('AddressLine5div1') != null && regValue[1] == "true") {
                if (document.getElementById('txtAddressLine5').value == "") {
                    document.getElementById('divError').style.display = 'block';
                    var txt = "<%= Resources.GlobalResources.InvalidPostcode%>";

                    document.getElementById('lblError').innerHTML = txt;
                    regValidation = false;
                }
                else {
                    if (document.getElementById('hdnUkPostcode').value == "true") {
                        var value = document.getElementById('txtAddressLine5').value;
                        if (value.substr(0, 1) == "<") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.AddressLine5Error%>";

                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                        }
                    }
                }
            }
            if (document.getElementById('divGrPostcode1') != null && regValue[1] == "true") {
                if (document.getElementById('txtGrPostcode').value == "") {
                    document.getElementById('divError').style.display = 'block';
                    var txt = "<%= Resources.GlobalResources.InvalidPostcode%>";

                    document.getElementById('lblError').innerHTML = txt;
                    regValidation = false;
                }
                else {
                    if (document.getElementById('hdnUkPostcode').value == "true") {
                        var value = document.getElementById('txtGrPostcode').value;
                        if (value.substr(0, 1) == "<") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.AddressLine6Error%>";

                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                        }
                    }
                }
            }
        }
    }
    return regValidation;
}

function PreviousControl() {
    if (document.getElementById('AddressLine1div1') != null) {
        if (document.getElementById('AddressLine1div1').className == 'focuspanel curved paddingtop-10') {
            document.getElementById('AddressLine1div1').className = '';
            document.getElementById('AddressLine1div2').className = 'inputtext';
            document.getElementById('AddressLine1div3').className = 'input322';
            if (document.getElementById('divUKPostCode1') != null) {
                document.getElementById('divUKPostCode1').className = 'focuspanel curved paddingtop-10';
                document.getElementById('divUKPostCode2').className = 'inputtext twolines whitetext paddingtop10';
                document.getElementById('divUKPostCode3').className = 'input322 paddingtop10';
                document.getElementById('txtUKPostCode').disabled = false;
                FocusObject('txtUKPostCode');
                document.getElementById('txtUKPostCode').focus();
                document.getElementById('txtUKPostCode').value = document.getElementById('txtUKPostCode').value;

                document.getElementById('plokkAdreessGrey').style.display = 'none';
                document.getElementById('plokkAdreess').style.display = 'block';
                document.getElementById('divconfirm').style.display = 'none';
                Postcode_Check();
            }
            else {
                if (document.getElementById('hdnErrorCtrl').value == "Confirm") {

                    window.location = "WhatIsYourTitleandName.aspx?page=Confirm";
                }
                else {
                    window.location = "WhatIsYourTitleandName.aspx?page=Address";
                }
                return true;
            }
            document.getElementById('divs').style.display = 'none';
            return false;

        }
    }
    if (document.getElementById('divUKPostCode1') != null) {
        if (document.getElementById('divUKPostCode1').className == 'focuspanel curved paddingtop-10') {
            window.location = "WhatIsYourTitleandName.aspx?page=Address";
        }
    }
    if (document.getElementById('AddressLine2div1') != null) {
        if (document.getElementById('AddressLine2div1').className == 'focuspanel curved paddingtop-10') {
            AddressLine1_Click();
            document.getElementById('txtAddressLine1').focus();
            document.getElementById('txtAddressLine1').value = document.getElementById('txtAddressLine1').value;
            return false;
        }
    }
    if (document.getElementById('AddressLine3div1') != null) {
        if (document.getElementById('AddressLine3div1').className == 'focuspanel curved paddingtop-10') {
            AddressLine2_Click();
            document.getElementById('txtAddressLine2').focus();
            document.getElementById('txtAddressLine2').value = document.getElementById('txtAddressLine2').value;
            return false;
        }
    }
    if (document.getElementById('AddressLine4div1') != null) {
        if (document.getElementById('AddressLine4div1').className == 'focuspanel curved paddingtop-10') {
            AddressLine3_Click();
            document.getElementById('txtAddressLine3').focus();
            document.getElementById('txtAddressLine3').value = document.getElementById('txtAddressLine3').value;
            return false;
        }
    }
    if (document.getElementById('AddressLine5div1') != null) {
        if (document.getElementById('AddressLine5div1').className == 'focuspanel curved paddingtop-10') {
            AddressLine4_Click();
            document.getElementById('txtAddressLine4').focus();
            document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
            return false;
        }
    }
    if (document.getElementById('divGrPostcode1') != null) {
        if (document.getElementById('divGrPostcode1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('AddressLine5div1') != null) {
                AddressLine5_Click();
                document.getElementById('txtAddressLine5').focus();
                document.getElementById('txtAddressLine5').value = document.getElementById('txtAddressLine5').value;
            }
            else if (document.getElementById('AddressLine4div1') != null) {
                AddressLine4_Click();
                document.getElementById('txtAddressLine4').focus();
                document.getElementById('txtAddressLine4').value = document.getElementById('txtAddressLine4').value;
            }
            return false;
        }
    }
}