function chkDefaultValueOfDt() {
    var txtDt1 = document.getElementById('txtDt1').value;
    var txtDt2 = document.getElementById('txtDt2').value;
    var txtDt3 = document.getElementById('txtDt3').value;
    var txtDD = resources[2];
    var txtMM = resources[3];
    var txtYYYY = resources[4];
    var dateFormat = document.getElementById('hdnDateFormat').value;
    var bFlag = false;
    if (dateFormat == 'DMY') {
        if (txtDt1 == txtDD && txtDt2 == txtMM && txtDt3 == txtYYYY) {
            bFlag = true;
        }
    }
    if (dateFormat == 'DYM') {
        if (txtDt1 == txtDD && txtDt2 == txtYYYY && txtDt3 == txtMM) {
            bFlag = true;
        }
    }
    if (dateFormat == 'MDY') {
        if (txtDt1 == txtMM && txtDt2 == txtDD && txtDt3 == txtYYYY) {
            bFlag = true;
        }
    }
    if (dateFormat == 'MYD') {
        if (txtDt1 == txtMM && txtDt2 == txtYYYY && txtDt3 == txtDD) {
            bFlag = true;
        }
    }
    if (dateFormat == 'YDM') {
        if (txtDt1 == txtYYYY && txtDt2 == txtDD && txtDt3 == txtMM) {
            bFlag = true;
        }
    }
    if (dateFormat == 'YMD') {
        if (txtDt1 == txtYYYY && txtDt2 == txtMM && txtDt3 == txtDD) {
            bFlag = true;
        }
    }
    return bFlag;
}

function PreviousControl() {
    if (document.getElementById('txtDt1') != null && document.getElementById('dob').className == 'focuspaneldob curved') {
        return true;
    }
    else if (document.getElementById('txtAge1') != null && document.getElementById('pnlHHAges').className == 'focuspanelage curved') {
        if (document.getElementById('txtDt1') != null) {
            FocusDOB('txtDt1');
            document.getElementById('pnlnexthouseages').className = 'nexthouseages nexthouseagesfocus';
            return false;
        }
        else {
            return true;
        }
    }
}

function FocusDOB(objName) {

    var txt = resources[0];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('dob').className = 'focuspaneldob curved';
    document.getElementById('dobtxt').className = 'inputtext whitetext';
    

    if (document.getElementById('txtAge1') != null) {
        document.getElementById('pnlHHAges').className = '';
        document.getElementById('pnlHHAges1').className = 'inputtext';
        document.getElementById('lblAge1').className = 'smalltext1';
        document.getElementById('lblAge2').className = 'smalltext1';
        document.getElementById('lblAge3').className = 'smalltext1';
        document.getElementById('lblAge4').className = 'smalltext1';
        document.getElementById('lblAge5').className = 'smalltext1';
        document.getElementById('divAge6').className = 'inputtext';

        document.getElementById('pnlnexthouseages').className = 'nexthouseages nexthouseagesfocus';
        document.getElementById('divspacer').className = 'buttonspacer';
        document.getElementById('divsummary').style.display = 'none';
        document.getElementById('pnlnexthouseages').style.display = 'block';
        document.getElementById('divconfirm').style.display = 'none';
        
    }
    else {
        if (document.getElementById('hdnConfirmPg').value == "Y") {
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
            document.getElementById('pnlnexthouseages').style.display = 'none';
            document.getElementById('divconfirm').style.display = 'none';
        }
        else {
            document.getElementById('divspacer').className = 'buttonspacer7';
            document.getElementById('divsummary').style.display = 'none';
            document.getElementById('pnlnexthouseages').style.display = 'none';
            document.getElementById('divconfirm').style.display = 'block';
        }
    }
    document.getElementById(objName).focus();
    document.getElementById(objName).select();
    FocusObject(objName);

}
function FocusAge(objName) {
    if (document.getElementById('txtDt1') != null) {
        document.getElementById('dob').className = '';
        document.getElementById('dobtxt').className = 'inputtext';
    }
    var txt = resources[1];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('pnlHHAges').className = 'focuspanelage curved';
    document.getElementById('pnlHHAges1').className = 'inputtext whitetext';
    document.getElementById('lblAge1').className = 'smalltext1 whitetext';
    document.getElementById('lblAge2').className = 'smalltext1 whitetext';
    document.getElementById('lblAge3').className = 'smalltext1 whitetext';
    document.getElementById('lblAge4').className = 'smalltext1 whitetext';
    document.getElementById('lblAge5').className = 'smalltext1 whitetext';
    document.getElementById('divAge6').className = 'inputtext whitetext';

    if (document.getElementById('hdnConfirmPg').value == "Y") {
        document.getElementById('divspacer').className = 'buttonspacer3';
        document.getElementById('divsummary').style.display = 'block';
        document.getElementById('pnlnexthouseages').style.display = 'none';
        document.getElementById('divconfirm').style.display = 'none';
    }
    else {
        document.getElementById('divspacer').className = 'buttonspacer7';
        document.getElementById('divsummary').style.display = 'none';
        document.getElementById('pnlnexthouseages').style.display = 'none';
        document.getElementById('divconfirm').style.display = 'block';
    }
    //document.getElementById('pnlnexthouseages').className = 'nexthouseages nextdietary';
    document.getElementById(objName).focus();
    document.getElementById(objName).select();
    FocusObject(objName);
}

function ValidateDate() {
    var sDate;
    var dateFormat = document.getElementById('hdnDateFormat').value;
    var regExpStr = document.getElementById('hdnDateRegExp').value;
    var reg = new RegExp(regExpStr);  ///^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/;
    if (dateFormat.indexOf("Y") != '0') {
        if (document.getElementById('txtDt1').value.trim().length < 2) {
            document.getElementById('txtDt1').value = "0" + document.getElementById('txtDt1').value.trim();
        }
        if (document.getElementById('txtDt2').value.trim().length < 2) {
            document.getElementById('txtDt2').value = "0" + document.getElementById('txtDt2').value.trim();
        }
        if (dateFormat == 'DMY') {
            sDate = document.getElementById('txtDt1').value.trim() + "/" + document.getElementById('txtDt2').value.trim() + "/" + document.getElementById('txtDt3').value.trim();
        }
        if (dateFormat == 'DYM') {
            sDate = document.getElementById('txtDt1').value.trim() + "/" + document.getElementById('txtDt3').value.trim() + "/" + document.getElementById('txtDt2').value.trim();
        }
        if (dateFormat == 'MDY') {
            sDate = document.getElementById('txtDt2').value.trim() + "/" + document.getElementById('txtDt1').value.trim() + "/" + document.getElementById('txtDt3').value.trim();
        }
        if (dateFormat == 'MYD') {
            sDate = document.getElementById('txtDt3').value.trim() + "/" + document.getElementById('txtDt1').value.trim() + "/" + document.getElementById('txtDt2').value.trim();
        }
    }
    else {
        if (document.getElementById('txtDt2').value.trim().length < 2) {
            document.getElementById('txtDt2').value = "0" + document.getElementById('txtDt2').value.trim();
        }
        if (document.getElementById('txtDt3').value.trim().length < 2) {
            document.getElementById('txtDt3').value = "0" + document.getElementById('txtDt3').value.trim();
        }

        if (dateFormat == 'YDM') {
            sDate = document.getElementById('txtDt2').value.trim() + "/" + document.getElementById('txtDt3').value.trim() + "/" + document.getElementById('txtDt1').value.trim();
        }
        if (dateFormat == 'YMD') {
            sDate = document.getElementById('txtDt3').value.trim() + "/" + document.getElementById('txtDt2').value.trim() + "/" + document.getElementById('txtDt1').value.trim();
        }
    }

    //checking for date
    if (!reg.test(sDate)) {
        document.getElementById('hdnErr').value = 'DtErr1';
        return false;
    }
    else {
        var sDt = new Date(sDate);
        var dt = new Date();
        var dtSpan = (dt - sDt) / (1000 * 60 * 60 * 24);
        if (dtSpan < document.getElementById('hdnDOBLimitInDays').value) {
            document.getElementById('hdnErr').value = 'DtErr1';
            return false;
        }
    }

    document.getElementById('hdnDt').value = sDate.toString();
    return true;
}

function Date_Check1() {
    var sDOB = document.getElementById('txtDt1').value;
    var lenFn = sDOB.length;
    var lenFnChk = 2;
    var dateFormat = document.getElementById('hdnDateFormat').value;
    if (dateFormat.indexOf("Y") == '0') {
        lenFnChk = 4;
    }
    else {
        lenFnChk = 2;
    }
    if (lenFn == lenFnChk) //If input string valid
    {
        document.getElementById('txtDt2').focus();
        document.getElementById('txtDt2').select();
        FocusObject('txtDt2');
        return false;
    }
}

function Date_Check2() {
    var sDOB = document.getElementById('txtDt2').value;
    var lenFn = sDOB.length;
    var lenFnChk = 2;
    var dateFormat = document.getElementById('hdnDateFormat').value;
    if (dateFormat.indexOf("Y") == '1') {
        lenFnChk = 4;
    }
    else {
        lenFnChk = 2;
    }
    if (lenFn == lenFnChk) //If input string valid
    {
        document.getElementById('txtDt3').focus();
        document.getElementById('txtDt3').select();
        FocusObject('txtDt3');
        return false;
    }
}

function Age_Check(obj1, obj2) {
    var lenFn = document.getElementById(obj1).value.length;
    if (lenFn == 2) //If input string valid
    {
        document.getElementById(obj2).focus();
        document.getElementById(obj2).select();
        FocusObject(obj2);
        return false;
    }
}

function Validate(controlList) {
    //debugger;
    if (controlList != "") {
        var controlList = controlList.value.split("|");
        for (var i = 1; i < controlList.length; i++) {
            var control = controlList[i].split(":");
            var controlName = control[0];
            if (control[0].toString().toUpperCase() == "DATEOFBIRTH") {
                var validate = control[1].split(",");
                if (document.getElementById('txtDt1') != null ) {
                    var txtDt1 = document.getElementById('txtDt1').value;
                    var txtDt2 = document.getElementById('txtDt2').value;
                    var txtDt3 = document.getElementById('txtDt3').value;
                    if (validate[1].toString().toUpperCase() == "TRUE") {
                        if ((txtDt1 == "" || txtDt2 == "" || txtDt3 == "") || chkDefaultValueOfDt()) {
                            document.getElementById('hdnErr').value = 'DtErr2';
                            return true;
                        }
                        var regNumber = document.getElementById('hdnAgeRegExp').value;
                        var reg = new RegExp(regNumber);
                        //checking for numbers
                        if (((!reg.test(txtDt1)) || (!reg.test(txtDt2)) || (!reg.test(txtDt3))) || !ValidateDate()) {
                            document.getElementById('hdnErr').value = 'DtErr1';
                            return true;
                        }
//                        if (ValidateDate() == false) {
//                            document.getElementById('hdnErr').value = 'DtErr1';
//                            return true;
//                        }

                    }
                    else {
                        //checking for numbers
                        if (!(txtDt1 == "" && txtDt2 == "" && txtDt3 == "")) {
                            if (!chkDefaultValueOfDt()) {
                                var regNumber = document.getElementById('hdnAgeRegExp').value;
                                var reg = new RegExp(regNumber);
                                if ((!reg.test(txtDt1)) || (!reg.test(txtDt2)) || (!reg.test(txtDt3))) {
                                    document.getElementById('hdnErr').value = 'DtErr1';
                                    return true;
                                }
                                else {
                                    if (!ValidateDate()) {
                                        document.getElementById('hdnErr').value = 'DtErr1';
                                        return true;
                                    }
                                }
                            }
                            else {
                                document.getElementById('hdnDt').value = 'DDMMYYY';
                            }
                        }
                    }
                }
            }

            if (control[0].toString().toUpperCase() == "HHAGE1") {
                if (document.getElementById('txtAge1') != null ) {
                    var validate = control[1].split(",");
                    var txtAge1 = document.getElementById('txtAge1').value;
                    var regNumber = document.getElementById('hdnAgeRegExp').value;
                    var reg = new RegExp(regNumber);
                    if (!reg.test(txtAge1)) {
                        document.getElementById('hdnErr').value = 'txtAge1|HHErr1';
                        return true;
                    }
                    if (validate[1].toString().toUpperCase() == "TRUE") {
                        if (txtAge1 == "") {
                            document.getElementById('hdnErr').value = 'txtAge1|HHErr2';
                            return true;
                        }
                    }
                }
            }
            if (control[0].toString().toUpperCase() == "HHAGE2") {
                if (document.getElementById('txtAge2') != null ) {
                    var validate = control[1].split(",");
                    var txtAge2 = document.getElementById('txtAge2').value;
                    var regNumber = document.getElementById('hdnAgeRegExp').value;
                    var reg = new RegExp(regNumber);
                    if (!reg.test(txtAge2)) {
                        document.getElementById('hdnErr').value = 'txtAge2|HHErr1';
                        return true;
                    }
                    if (validate[1].toString().toUpperCase() == "TRUE") {
                        if (txtAge2 == "") {
                            document.getElementById('hdnErr').value = 'txtAge2|HHErr2';
                            return true;
                        }
                    }
                }
            }
            if (control[0].toString().toUpperCase() == "HHAGE3") {
                if (document.getElementById('txtAge3') != null ) {
                    var validate = control[1].split(",");
                    var txtAge3 = document.getElementById('txtAge3').value;
                    var regNumber = document.getElementById('hdnAgeRegExp').value;
                    var reg = new RegExp(regNumber);
                    if (!reg.test(txtAge3)) {
                        document.getElementById('hdnErr').value = 'txtAge3|HHErr1';
                        return true;
                    }
                    if (validate[1].toString().toUpperCase() == "TRUE") {
                        if (txtAge3 == "") {
                            document.getElementById('hdnErr').value = 'txtAge3|HHErr2';
                            return true;
                        }
                    }
                }
            }
            if (control[0].toString().toUpperCase() == "HHAGE4") {
                if (document.getElementById('txtAge4') != null ) {
                    var validate = control[1].split(",");
                    var txtAge4 = document.getElementById('txtAge4').value;
                    var regNumber = document.getElementById('hdnAgeRegExp').value;
                    var reg = new RegExp(regNumber);
                    if (!reg.test(txtAge4)) {
                        document.getElementById('hdnErr').value = 'txtAge4|HHErr1';
                        return true;
                    }
                    if (validate[1].toString().toUpperCase() == "TRUE") {
                        if (txtAge4 == "") {
                            document.getElementById('hdnErr').value = 'txtAge4|HHErr2';
                            return true;
                        }
                    }
                }
            }
            if (control[0].toString().toUpperCase() == "HHAGE5") {
                if (document.getElementById('txtAge5') != null) {
                    var validate = control[1].split(",");
                    var txtAge5 = document.getElementById('txtAge5').value;
                    var regNumber = document.getElementById('hdnAgeRegExp').value;
                    var reg = new RegExp(regNumber);
                    if (!reg.test(txtAge5)) {
                        document.getElementById('hdnErr').value = 'txtAge5|HHErr1';
                        return true;
                    }
                    if (validate[1].toString().toUpperCase() == "TRUE") {
                        if (txtAge5 == "") {
                            document.getElementById('hdnErr').value = 'txtAge5|HHErr2';
                            return true;
                        }
                    }
                }
            }
        }
    }
    if (document.getElementById('txtDt1') != null && document.getElementById('dob').className == 'focuspaneldob curved') {
        if (document.getElementById('txtAge1') != null) {
            FocusAge('txtAge1');
        }
        else {
            return true;
        }

    }
    else if (document.getElementById('txtAge1') != null && document.getElementById('pnlHHAges').className == 'focuspanelage curved') {
        return true;
    }
    return false;
}

function NextButtonClick() {
    if (document.getElementById('txtDt1') != null && document.getElementById('dob').className == 'focuspaneldob curved') {
        if (document.getElementById('txtAge1') != null) {
            FocusAge('txtAge1');
        }
        else {
            return true;
        }

    }
    else if (document.getElementById('txtAge1') != null && document.getElementById('pnlHHAges').className == 'focuspanelage curved') {
        return true;
    }
    return false;
}