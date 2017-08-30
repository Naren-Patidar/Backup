var resources;


function RemoveFous() {
    if (document.getElementById('MobileNo1') != null) {
        document.getElementById('MobileNo1').className = '';
        document.getElementById('MobileNo2').className = 'inputtext';
        document.getElementById('MobileNo3').className = 'input322';
        FocusObject('txtMobileNo');
        //document.getElementById('txtMobileNo').value = document.getElementById('txtMobileNo').value;
    }
    if (document.getElementById('EveningNo1') != null) {
        document.getElementById('EveningNo1').className = '';
        document.getElementById('EveningNo2').className = 'inputtext';
        document.getElementById('EveningNo3').className = 'input322';
        FocusObject('txtEveningNo');
        //document.getElementById('txtEveningNo').value = document.getElementById('txtEveningNo').value;
    }
    if (document.getElementById('Email1') != null) {
        document.getElementById('Email1').className = '';
        document.getElementById('Email2').className = 'inputtext';
        document.getElementById('Email3').className = 'input322';
        document.getElementById('Email3').disabled = false;
        document.getElementById('txtEmailId').disabled = false;
        FocusObject('txtEmailId');
        //document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
    }
    if (document.getElementById('DayTimeNo1') != null) {
        document.getElementById('DayTimeNo1').className = '';
        document.getElementById('DayTimeNo2').className = 'inputtext';
        document.getElementById('DayTimeNo3').className = 'input322';
        FocusObject('txtDayTimeNo');
        //document.getElementById('txtDayTimeNo').value = document.getElementById('txtDayTimeNo').value;
    }
}

function Email1_Click() {
    var txt = resources[0];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('divconfirm').style.display = 'none';
    RemoveFous();
    if (document.getElementById('Email1') != null) {
        document.getElementById('Email1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('Email2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('Email3').className = 'input322 paddingtop10';
        document.getElementById('Email3').disabled = false;
        document.getElementById('txtEmailId').disabled = false;
        //document.getElementById('txtEmailId').focus();
        FocusObject('txtEmailId');
        //document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
    }

    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';
    document.getElementById('divspacer').className = 'buttonspacer2';
    document.getElementById('divnextbutton').style.display = 'block';
    document.getElementById('divnextbutton').className = "nextphoneno";

    document.getElementById('divsummary').style.display = 'none';
    FocusObject('txtEmailId');
    //document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
    return false;
}

function MobileNo_Click() {
    var txt = resources[1];
    document.getElementById('lbltitle').innerHTML = txt;
    var finalBtn = false;
    var finalBtn1 = false;
    document.getElementById('divconfirm').style.display = 'none';
    if (document.getElementById('Email1') != null) {
        document.getElementById('Email1').className = '';
        document.getElementById('Email2').className = 'inputtext';
        document.getElementById('Email3').className = 'input322';
        document.getElementById('Email3').disabled = false;
        document.getElementById('txtEmailId').disabled = false;
        //document.getElementById('txtEmailId').focus();
        FocusObject('txtEmailId');
        //document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
    }
    if (document.getElementById('MobileNo1') != null) {
        document.getElementById('MobileNo1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('MobileNo2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('MobileNo3').className = 'input322 paddingtop10';
        document.getElementById('MobileNo3').disabled = false;
        document.getElementById('txtMobileNo').disabled = false;
        //document.getElementById('txtMobileNo').focus();
        FocusObject('txtMobileNo');
        //document.getElementById('txtMobileNo').value = document.getElementById('txtMobileNo').value;
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divnextbutton').style.display = 'block';
        document.getElementById('divnextbutton').className = "nexteveningno";
    }
    if (document.getElementById('EveningNo1') != null) {
        document.getElementById('EveningNo1').className = '';
        document.getElementById('EveningNo2').className = 'inputtext';
        document.getElementById('EveningNo3').className = 'input322';
        
    }
    else {
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divnextbutton').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextdaytimeno";
        finalBtn = true;
    }
    if (document.getElementById('DayTimeNo1') != null) {
        document.getElementById('DayTimeNo1').className = '';
        document.getElementById('DayTimeNo2').className = 'inputtext';
        document.getElementById('DayTimeNo3').className = 'input322';
    }
    else {
        finalBtn1 = true;
    }
    if (finalBtn == true && finalBtn1 == true) {
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';
        document.getElementById('divnextbutton').style.display = 'none';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';
    
    return false;

}

function EveningNo_Click() {
    var txt = resources[2];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('divconfirm').style.display = 'none';
    if (document.getElementById('Email1') != null) {
        document.getElementById('Email1').className = '';
        document.getElementById('Email2').className = 'inputtext';
        document.getElementById('Email3').className = 'input322';
        document.getElementById('Email3').disabled = false;
        document.getElementById('txtEmailId').disabled = false;
        //document.getElementById('txtEmailId').focus();
        FocusObject('txtEmailId');
        //document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
    }
    if (document.getElementById('MobileNo1') != null) {
        document.getElementById('MobileNo1').className = '';
        document.getElementById('MobileNo2').className = 'inputtext';
        document.getElementById('MobileNo3').className = 'input322';
    }
    if (document.getElementById('EveningNo1') != null) {
        document.getElementById('EveningNo1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('EveningNo2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('EveningNo3').className = 'input322 paddingtop10';
        document.getElementById('EveningNo3').disabled = false;
        document.getElementById('txtEveningNo').disabled = false;
        //document.getElementById('txtEveningNo').focus();
        FocusObject('txtEveningNo');
        //document.getElementById('txtEveningNo').value = document.getElementById('txtEveningNo').value;
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divnextbutton').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextdaytimeno";
    }
    else {
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    if (document.getElementById('DayTimeNo1') != null) {
        document.getElementById('DayTimeNo1').className = '';
        document.getElementById('DayTimeNo2').className = 'inputtext';
        document.getElementById('DayTimeNo3').className = 'input322';
    }
    else {
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divconfirm').style.display = 'block';
        document.getElementById('divnextbutton').style.display = 'none'; 
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';

    document.getElementById('divsummary').style.display = 'none';
    return false;
}

function DayTimeNo_Click() {
    var txt = resources[3];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('divconfirm').style.display = 'none';
    if (document.getElementById('Email1') != null) {
        document.getElementById('Email1').className = '';
        document.getElementById('Email2').className = 'inputtext';
        document.getElementById('Email3').className = 'input322';
        document.getElementById('Email3').disabled = false;
        document.getElementById('txtEmailId').disabled = false;
        //document.getElementById('txtEmailId').focus();
        FocusObject('txtEmailId');
        //document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
    }
    if (document.getElementById('MobileNo1') != null) {
        document.getElementById('MobileNo1').className = '';
        document.getElementById('MobileNo2').className = 'inputtext';
        document.getElementById('MobileNo3').className = 'input322';
    }
    if (document.getElementById('EveningNo1') != null) {
        document.getElementById('EveningNo1').className = '';
        document.getElementById('EveningNo2').className = 'inputtext';
        document.getElementById('EveningNo3').className = 'input322';
    }
    if (document.getElementById('DayTimeNo1') != null) {
        document.getElementById('DayTimeNo1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('DayTimeNo2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('DayTimeNo3').className = 'input322 paddingtop10';

        document.getElementById('DayTimeNo3').disabled = false;
        document.getElementById('txtDayTimeNo').disabled = false;
        //document.getElementById('txtDayTimeNo').focus();
        FocusObject('txtDayTimeNo');
        //document.getElementById('txtDayTimeNo').value = document.getElementById('txtDayTimeNo').value;

        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divconfirm').style.display = 'none';
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
        else {
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';
            document.getElementById('divspacer').className = 'buttonspacer2';
            document.getElementById('divconfirm').style.display = 'block';
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divsummary').style.display = 'none';
        }
    }

  
   
    return false;
}

function Next_Value() {
    if (document.getElementById('Email1') != null) {
        if (document.getElementById('Email1').className == 'focuspanel curved paddingtop-10') {
            MobileNo_Click();
            document.getElementById('txtMobileNo').focus();
            document.getElementById('txtMobileNo').value = document.getElementById('txtMobileNo').value;
            return false;
        }
    }
    if (document.getElementById('MobileNo1') != null) {
        if (document.getElementById('MobileNo1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('EveningNo1') != null) {
                EveningNo_Click();
                document.getElementById('txtEveningNo').focus();
                document.getElementById('txtEveningNo').value = document.getElementById('txtEveningNo').value;
            }
            else if (document.getElementById('DayTimeNo1') != null) {
                DayTimeNo_Click();
                document.getElementById('txtDayTimeNo').focus();
                document.getElementById('txtDayTimeNo').value = document.getElementById('txtDayTimeNo').value;
            }
            return false;
        }
    }
    if (document.getElementById('EveningNo1') != null) {
        if (document.getElementById('EveningNo1').className == 'focuspanel curved paddingtop-10') {
            DayTimeNo_Click();
            document.getElementById('txtDayTimeNo').focus();
            document.getElementById('txtDayTimeNo').value = document.getElementById('txtDayTimeNo').value;
            return false;
        }
    }
}

function PreviousControl() {
    if (document.getElementById('Email1') != null) {
        if (document.getElementById('Email1').className == 'focuspanel curved paddingtop-10') {
            window.location = "WhatIsYourAdress.aspx?page=ContactDetails";
           
        }
    }
    if (document.getElementById('MobileNo1') != null) {
        if (document.getElementById('MobileNo1').className == 'focuspanel curved paddingtop-10') {
            Email1_Click();
            document.getElementById('txtEmailId').focus();
            document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
            return false;
        }
    }
    if (document.getElementById('EveningNo1') != null) {
        if (document.getElementById('EveningNo1').className == 'focuspanel curved paddingtop-10') {
            MobileNo_Click();
            document.getElementById('txtMobileNo').focus();
            document.getElementById('txtMobileNo').value = document.getElementById('txtMobileNo').value;
            return false;
        }
    }
    if (document.getElementById('DayTimeNo1') != null) {
        if (document.getElementById('DayTimeNo1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('EveningNo1') != null) {
                EveningNo_Click();
                document.getElementById('txtEveningNo').focus();
                document.getElementById('txtEveningNo').value = document.getElementById('txtEveningNo').value;
            }
            else if (document.getElementById('MobileNo1') != null) {
                MobileNo_Click();
                document.getElementById('txtMobileNo').focus();
                document.getElementById('txtMobileNo').value = document.getElementById('txtMobileNo').value;
            }
            return false;
        }
    }
}

function Loading() {
    if (document.getElementById('Email1') != null) {
        if (document.getElementById('Email1').className = 'focuspanel curved paddingtop-10') {
            Email1_Click();
            document.getElementById('txtEmailId').focus();
            document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
        }
    }
    var errorCtrl = document.getElementById('hdnErrorCtrl').value;
    if (errorCtrl == "Email") {
        Email1_Click();
        document.getElementById('txtEmailId').focus();
        document.getElementById('txtEmailId').value = document.getElementById('txtEmailId').value;
        return false;
    }
    else if (errorCtrl == "MobileNumber") {
        MobileNo_Click();
        document.getElementById('txtMobileNo').focus();
        document.getElementById('txtMobileNo').value = document.getElementById('txtMobileNo').value;
        return false;
    }
    else if (errorCtrl == "EveningNumber") {
        EveningNo_Click();
        document.getElementById('txtEveningNo').focus();
        document.getElementById('txtEveningNo').value = document.getElementById('txtEveningNo').value;
        return false;
    }
    else if (errorCtrl == "DayTimeNumber") {
        DayTimeNo_Click();
        document.getElementById('txtDayTimeNo').focus();
        document.getElementById('txtDayTimeNo').value = document.getElementById('txtDayTimeNo').value;
        return false;
    }
}

function AlloNumbersOnly(val) {

    var e = event || val; // for trans-browser compatibility
    var charCode = e.which || e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}