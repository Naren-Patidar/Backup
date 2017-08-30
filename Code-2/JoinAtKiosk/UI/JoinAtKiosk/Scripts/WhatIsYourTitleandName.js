var resources;

function title_mrClick(obj) {
    obj.className = "title_mr select";
    document.getElementById('divmiss').className = "title_miss";
    document.getElementById('divmrs').className = "title_mrs";
    document.getElementById('divms').className = "title_ms";
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';

    EnableFirstName();
    document.getElementById('txtFirstName').focus();
    document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
    TitleFocus();
}

function title_missClick(obj) {
    obj.className = "title_miss select";
    document.getElementById('divmr').className = "title_mr";
    document.getElementById('divmrs').className = "title_mrs";
    document.getElementById('divms').className = "title_ms";
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';

    EnableFirstName();
    document.getElementById('txtFirstName').focus();
    document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
    TitleFocus();
}

function title_mrsClick(obj) {
    obj.className = "title_mrs select";
    document.getElementById('divmr').className = "title_mr";
    document.getElementById('divmiss').className = "title_miss";
    document.getElementById('divms').className = "title_ms";
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';

    EnableFirstName();
    document.getElementById('txtFirstName').focus();
    document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
    TitleFocus();
}

function title_msClick(obj) {
    obj.className = "title_ms select";
    document.getElementById('divmr').className = "title_mr";
    document.getElementById('divmiss').className = "title_miss";
    document.getElementById('divmrs').className = "title_mrs";
    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';
    EnableFirstName();
    document.getElementById('txtFirstName').focus();
    document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
    TitleFocus();
}

function EnableFirstName() {
    var txt = resources[0];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('divsummary').style.display = 'none';
    var finalCtrl1 = "false";
    var finalCtrl2 = "false";
    if (document.getElementById('divMiddleName1') != null) {
        document.getElementById('divMiddleName1').className = '';
        document.getElementById('divMiddleName2').className = 'inputtext';
        document.getElementById('divMiddleName3').className = 'input322';

        document.getElementById('divMiddleName3').disabled = false;
        document.getElementById('txtMiddleName').disabled = false;
        FocusObject('txtMiddleName');
        //document.getElementById('txtMiddleName').value = document.getElementById('txtMiddleName').value;
    }
    else {
        finalCtrl1 = "true";
    }
    if (document.getElementById('divSurname1') != null) {
        document.getElementById('divSurname1').className = '';
        document.getElementById('divSurname2').className = 'inputtext';
        document.getElementById('divSurname3').className = 'input322';

        document.getElementById('divSurname3').disabled = false;
        document.getElementById('txtSurname').disabled = false;
        FocusObject('txtSurname');
        //document.getElementById('txtSurname').value = document.getElementById('txtSurname').value;

    }
    else {
        finalCtrl2 = "true";
    }
    if (document.getElementById('divTitle') != null) {
        document.getElementById('divTitle').className = '';
        document.getElementById('Span1').className = 'titletext grey';
    }
    if (document.getElementById('divFirstName1') != null) {
        document.getElementById('divFirstName1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('divFirstName2').className = 'inputtext twolines whitetext paddingtop10';
        document.getElementById('divFirstName3').className = 'input322 paddingtop10';
        document.getElementById('txtFirstName').disabled = false;
        document.getElementById('divnext').style.display = 'none';
        FocusObject('txtFirstName');
        //document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        var backBtn = resources[4];
        document.getElementById('btnBack').src = backBtn;
        document.getElementById('divBtn').className = 'buttonitems last paddingtop-10';
        document.getElementById('divnextbutton').className = "nextbutton nextmiddlefocus paddingtop-13";
    }
    if (finalCtrl1 == "true") {
        document.getElementById('divnextbutton').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbutton nextsurnamefocus paddingtop-13";
    }
    if (finalCtrl1 == "true" && finalCtrl2 == "true") {
        var errorCtrl = document.getElementById('hdnErrorCtrl').value; 
        if (errorCtrl == "Confirm") {
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
        else {
            document.getElementById('divnext').style.display = 'none';
            document.getElementById('divs').style.display = 'block';
            document.getElementById('divspacer').className = 'buttonspacer2';
            document.getElementById('divnextbutton').className = 'nextbutton ConfirmNamebutton confirmnamefocus';
            return true;
        }
    }
    if (document.getElementById('divTitle') != null) {
        TitleFocus();
    }
    return false;
}

function Text_Check() {
    var sLN;
    if (document.getElementById('txtSurname') != null) {
        sLN = trim(document.getElementById('txtSurname').value);
    }
    var lenFn = sLN.length;
    if (lenFn >= 2 || document.getElementById('hdnControlList').value == "FALSE") //If input string valid
    {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divnextbutton').className = 'nextbutton ConfirmNamebutton confirmnamefocus';
    }
    else {
        document.getElementById('divnext').style.display = 'block';
        document.getElementById('divs').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divnext').className = 'nextbutton ConfirmNamebutton';
    }
}

function trim(str) {
    str = str.replace(/^\s+|\s+$/g, "");
    return str;
}

function MiddleName_Click() {
    var txt = resources[1];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('divsummary').style.display = 'none';  
    if (document.getElementById('divFirstName1') != null) {
        document.getElementById('divFirstName1').className = '';
        document.getElementById('divFirstName2').className = 'inputtext';
        document.getElementById('divFirstName3').className = 'input322';

        document.getElementById('divFirstName3').disabled = false;
        document.getElementById('txtFirstName').disabled = false;
        FocusObject('txtFirstName');
        //document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
    }
    if (document.getElementById('divSurname1') != null) {
        document.getElementById('divSurname1').className = '';
        document.getElementById('divSurname2').className = 'inputtext';
        document.getElementById('divSurname3').className = 'input322';

        document.getElementById('divSurname3').disabled = false;
        document.getElementById('txtSurname').disabled = false;
        FocusObject('txtSurname');
        //document.getElementById('txtSurname').value = document.getElementById('txtSurname').value;
    }
    else {
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divspacer').className = 'buttonspacer2';
        document.getElementById('divBtn').className = 'buttonitems last paddingtop-13';
        document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton confirmnamefocus";
        var errorCtrl = document.getElementById('hdnErrorCtrl').value;
        if (errorCtrl == "Confirm") {
            document.getElementById('divnextbutton').style.display = 'none';
            document.getElementById('divspacer').className = 'buttonspacer3';
            document.getElementById('divsummary').style.display = 'block';
        }
    }
    if (document.getElementById('divTitle') != null) {
        document.getElementById('divTitle').className = '';
        TitleFocus();
    }
    if (document.getElementById('divMiddleName1') != null) {
        document.getElementById('divMiddleName1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('divMiddleName2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('divMiddleName3').className = 'input322 paddingtop10';
        document.getElementById('txtMiddleName').disabled = false;
        FocusObject('txtMiddleName');
        //document.getElementById('txtMiddleName').value = document.getElementById('txtMiddleName').value;
        document.getElementById('divBtn').className = 'buttonitems last paddingtop-10';
        document.getElementById('divnext').style.display = 'none';
        document.getElementById('divs').style.display = 'block';
        document.getElementById('divnextbutton').style.display = 'block';
        document.getElementById('divnextbutton').className = "nextbutton nextsurnamefocus paddingtop-13";
    }
    var backBtn = resources[3];
    document.getElementById('btnBack').src = backBtn;
    var errorCtrl = document.getElementById('hdnErrorCtrl').value;
    return false;
}

function FirstName_Click() {
    EnableFirstName();
    return false;
}

function Surname_Click() {
    var txt = resources[2];
    document.getElementById('lbltitle').innerHTML = txt;
    document.getElementById('divsummary').style.display = 'none';  
    if (document.getElementById('divFirstName1') != null) {
        document.getElementById('divFirstName1').className = '';
        document.getElementById('divFirstName2').className = 'inputtext';
        document.getElementById('divFirstName3').className = 'input322';

        document.getElementById('txtFirstName').disabled = false;
        FocusObject('txtFirstName');
        //document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
    }
    if (document.getElementById('divMiddleName1') != null) {
        document.getElementById('divMiddleName1').className = '';
        document.getElementById('divMiddleName2').className = 'inputtext';
        document.getElementById('divMiddleName3').className = 'input322';

        document.getElementById('txtMiddleName').disabled = false;
        FocusObject('txtMiddleName');
        //document.getElementById('txtMiddleName').value = document.getElementById('txtMiddleName').value;
    }
    if (document.getElementById('divSurname1') != null) {
        document.getElementById('divSurname1').className = 'focuspanel curved paddingtop-10';
        document.getElementById('divSurname2').className = 'inputtext whitetext paddingtop10';
        document.getElementById('divSurname3').className = 'input322 paddingtop10';
        document.getElementById('txtSurname').disabled = false;
        FocusObject('txtSurname');
        //document.getElementById('txtSurname').value = document.getElementById('txtSurname').value;

    }
    if (document.getElementById('divTitle') != null) {
        document.getElementById('divTitle').className = '';
        TitleFocus();
    }

    document.getElementById('divnext').style.display = 'none';
    document.getElementById('divs').style.display = 'block';
    document.getElementById('divspacer').className = 'buttonspacer2';
    document.getElementById('divBtn').className = 'buttonitems last paddingtop-13';
    document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
    var backBtn = resources[3];
    document.getElementById('btnBack').src = backBtn;
    var errorCtrl = document.getElementById('hdnErrorCtrl').value;
    if (errorCtrl == "Confirm") {
        document.getElementById('divnextbutton').style.display = 'none';
        document.getElementById('divspacer').className = 'buttonspacer3';
        document.getElementById('divsummary').style.display = 'block';
    }
    Text_Check();
    return false;
}

function TitleFocus() {
    if (document.getElementById('divmr').className == "title_mr select") {
        document.getElementById('divmr').className = "title_mr select";
        document.getElementById('divmiss').className = "title_miss unselect";
        document.getElementById('divmrs').className = "title_mrs unselect";
        document.getElementById('divms').className = "title_ms unselect";
    }
    if (document.getElementById('divmiss').className == "title_miss select") {
        document.getElementById('divmr').className = "title_mr unselect";
        document.getElementById('divmiss').className = "title_miss select";
        document.getElementById('divmrs').className = "title_mrs unselect";
        document.getElementById('divms').className = "title_ms unselect";
    }
    if (document.getElementById('divmrs').className == "title_mrs select") {
        document.getElementById('divmr').className = "title_mr unselect";
        document.getElementById('divmiss').className = "title_miss unselect";
        document.getElementById('divmrs').className = "title_mrs select";
        document.getElementById('divms').className = "title_ms unselect";
    }
    if (document.getElementById('divms').className == "title_ms select") {
        document.getElementById('divmr').className = "title_mr unselect";
        document.getElementById('divmiss').className = "title_miss unselect";
        document.getElementById('divmrs').className = "title_mrs unselect";
        document.getElementById('divms').className = "title_ms select";
    }
    var errorCtrl = document.getElementById('hdnErrorCtrl').value;
    if (errorCtrl == "Address") {
        if (document.getElementById('divmr').className == "title_mr focus") {
            document.getElementById('divmr').className = "title_mr select";
            document.getElementById('divmiss').className = "title_miss unselect";
            document.getElementById('divmrs').className = "title_mrs unselect";
            document.getElementById('divms').className = "title_ms unselect";
        }
        if (document.getElementById('divmiss').className == "title_miss focus") {
            document.getElementById('divmiss').className = "title_miss select";
            document.getElementById('divmr').className = "title_mr unselect";
            document.getElementById('divmrs').className = "title_mrs unselect";
            document.getElementById('divms').className = "title_ms unselect";
        }
        if (document.getElementById('divmrs').className == "title_mrs focus") {
            document.getElementById('divmrs').className = "title_mrs select";
            document.getElementById('divmiss').className = "title_miss unselect";
            document.getElementById('divmrs').className = "title_mrs unselect";
            document.getElementById('divms').className = "title_ms unselect";
        }
        if (document.getElementById('divms').className == "title_ms focus") {
            document.getElementById('divms').className = "title_ms select";
            document.getElementById('divmrs').className = "title_mrs unselect";
            document.getElementById('divmiss').className = "title_miss unselect";
            document.getElementById('divmrs').className = "title_mrs unselect";
        }
    }
    else {
        if (document.getElementById('divmr').className != "title_mr select" && document.getElementById('divms').className != "title_ms select"
        && document.getElementById('divmiss').className != "title_miss select" && document.getElementById('divmrs').className != "title_mrs select") {

            document.getElementById('divmr').className = "title_mr unselect";
            document.getElementById('divmiss').className = "title_miss unselect";
            document.getElementById('divmrs').className = "title_mrs unselect";
            document.getElementById('divms').className = "title_ms unselect";
        }
    }
}

function Next_Value() {
  
    if (document.getElementById('divFirstName1') != null) {
        if (document.getElementById('divFirstName1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('divnextbutton').className == 'nextbutton nextsurnamefocus paddingtop-13') {
                Surname_Click();
                document.getElementById('txtSurname').focus();
                document.getElementById('txtSurname').value = document.getElementById('txtSurname').value;
                return false;
            }
            if (document.getElementById('divnextbutton').className == 'nextbutton ConfirmNamebutton confirmnamefocus') {
                return true;
            }
            else {
                var txt = resources[1];
                document.getElementById('lbltitle').innerHTML = txt;
                MiddleName_Click();
                document.getElementById('txtMiddleName').focus();
                document.getElementById('txtMiddleName').value = document.getElementById('txtMiddleName').value;
                return false;
            }
        }
    }
    if (document.getElementById('divMiddleName1') != null) {
        if (document.getElementById('divMiddleName1').className == 'focuspanel curved paddingtop-10') {
            var txt = resources[1];
            document.getElementById('lbltitle').innerHTML = txt;
            if (document.getElementById('divFirstName1') != null) {
                document.getElementById('divFirstName1').className = '';
                document.getElementById('divFirstName2').className = 'inputtext twolines';
                document.getElementById('divFirstName3').className = 'input322';
            }
            if (document.getElementById('divMiddleName1') != null) {
                document.getElementById('divMiddleName1').className = '';
                document.getElementById('divMiddleName2').className = 'inputtext';
                document.getElementById('divMiddleName3').className = 'input322';
            }
            if (document.getElementById('divSurname1') != null) {
                document.getElementById('divSurname1').className = 'focuspanel curved paddingtop-10';
                document.getElementById('divSurname2').className = 'inputtext whitetext paddingtop10';
                document.getElementById('divSurname3').className = 'input322 paddingtop10';
                document.getElementById('divSurname3').disabled = false;
                document.getElementById('txtSurname').disabled = false;
                Surname_Click();
                FocusObject('txtSurname');
                document.getElementById('txtSurname').focus();
                document.getElementById('txtSurname').value = document.getElementById('txtSurname').value;
                document.getElementById('divspacer').className = 'buttonspacer2';
                document.getElementById('divBtn').className = 'buttonitems last paddingtop-13';
                document.getElementById('divnextbutton').className = "nextbutton ConfirmNamebutton";
            }
            else {
                if (document.getElementById('divnextbutton').className != "nextbutton ConfirmNamebutton confirmnamefocus") {
                    document.getElementById('divnextbutton').className = "nextbutton nextsurnamefocus paddingtop-13";
                    document.getElementById('divnext').style.display = 'none';
                    var backBtn = resources[4];
                    document.getElementById('btnBack').src = backBtn;
                }
            }

            document.getElementById('divnext').style.display = 'none';
            var backBtn = resources[3];
            document.getElementById('btnBack').src = backBtn;

            Text_Check();
            return false;
        }
    }
}

function PreviousControl() {
    if (document.getElementById('divMiddleName1') != null) {
        if (document.getElementById('divMiddleName1').className == 'focuspanel curved paddingtop-10') {
            FirstName_Click();
            document.getElementById('txtFirstName').focus();
            document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
            return false;
        }
    }
    if (document.getElementById('divSurname1') != null) {
        if (document.getElementById('divSurname1').className == 'focuspanel curved paddingtop-10') {
            if (document.getElementById('divMiddleName1') != null) {
                var txt = resources[2];
                MiddleName_Click();
                document.getElementById('txtMiddleName').focus();
                document.getElementById('txtMiddleName').value = document.getElementById('txtMiddleName').value;
                return false;
            }
            else {
                FirstName_Click();
                document.getElementById('txtFirstName').focus();
                document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
                return false;
            }
        }
    }
}
function Loading() {
    var errorCtrl = document.getElementById('hdnErrorCtrl').value;
    if (document.getElementById('divTitle') == null) {
        if (document.getElementById('divFirstName1') != null) {
            document.getElementById('divFirstName1').className = 'focuspanel curved paddingtop-10';
            document.getElementById('divFirstName2').className = 'inputtext twolines whitetext paddingtop10';
            document.getElementById('divFirstName3').className = 'input322 paddingtop10';
            document.getElementById('divnext').style.display = 'none';
            FirstName_Click();
            document.getElementById('txtFirstName').focus();
            document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
        }
    }
    else {
        if (errorCtrl == "Address" || errorCtrl == "Confirm") {
            FirstName_Click();
            document.getElementById('txtFirstName').focus();
            document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
            return false;
        }
    }

    if (errorCtrl == "Title") {
        document.getElementById('divTitle').className = 'holdertitlefocus curved';
        document.getElementById('divmr').className = 'title_mr';
        document.getElementById('divmiss').className = 'title_miss';
        document.getElementById('divmrs').className = 'title_mrs';
        document.getElementById('divms').className = 'title_ms';
        return false;
    }
    if (errorCtrl == "FirstName") {
        FirstName_Click();
        document.getElementById('txtFirstName').focus();
        document.getElementById('txtFirstName').value = document.getElementById('txtFirstName').value;
        return false;
    }
    if (errorCtrl == "MiddleName") {
        MiddleName_Click();
        document.getElementById('txtMiddleName').focus();
        document.getElementById('txtMiddleName').value = document.getElementById('txtMiddleName').value;
        return false;
    }
    if (errorCtrl == "Surname") {
        Surname_Click();
        document.getElementById('txtSurname').focus();
        document.getElementById('txtSurname').value = document.getElementById('txtSurname').value;
        return false;
    }
    if (errorCtrl == "") {
        if (document.getElementById('divTitle') != null) {
            if (document.getElementById('divmr').className == 'title_mr select') {
                title_mrClick(divmr);
                return false;
            }
            if (document.getElementById('divmrs').className == 'title_mrs select') {
                title_mrsClick(divmrs);
                return false;
            }
            if (document.getElementById('divmiss').className == 'title_miss select') {
                title_missClick(divmiss);
                return false;
            }
            if (document.getElementById('divms').className == 'title_ms select') {
                title_msClick(divms);
                return false;
            }
        }
    }

}

function Validate(controlList) {
    if (document.getElementById('divmr') != null) {
        if (document.getElementById('divmr').className != "title_mr select" && document.getElementById('divmiss').className !== "title_miss select" &&
                    document.getElementById('divmrs').className != "title_mrs select" && document.getElementById('divms').className != "title_ms select") {
            document.getElementById('divError').style.display = 'block';
            var text = "<%= Resources.GlobalResources.TitleError%>";
            document.getElementById('lblError').innerHTML = text;

            document.getElementById('divTitle').className = "holdertitlefocus curved";
            var txt = "<%= Resources.GlobalResources.Title%>";
            document.getElementById('lbltitle').innerHTML = txt;
            document.getElementById('divFirstName1').className = '';
            document.getElementById('divFirstName2').className = 'inputtext';
            document.getElementById('divFirstName3').className = 'input322';
            document.getElementById('divMiddleName1').className = '';
            document.getElementById('divMiddleName2').className = 'inputtext';
            document.getElementById('divMiddleName3').className = 'input322';
            document.getElementById('divSurname1').className = '';
            document.getElementById('divSurname2').className = 'inputtext';
            document.getElementById('divSurname3').className = 'input322';
            document.getElementById('divnextbutton').className = "nextbutton";
            document.getElementById('Span1').className = "titletext";
            return false;
        }
    }
    if (controlList != "") {
        var controlList = controlList.split("|");
        var regValidation = true;
        for (var i = 1; i < controlList.length; i++) {
            var control = controlList[i].split(":");
            var controlName = control[0];
            controlName = "txt" + controlName;
            if (document.getElementById(controlName) != null) {
                var validate = control[1].split(",");
                if (control[0] == "FirstName") {
                    if (document.getElementById(controlName).value == "") {
                        if (validate[1] == "true") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.Name1Error%>";
                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                            FirstName_Click();
                            return regValidation;
                        }
                    }
                    else {
                        var reg = new RegExp(validate[0]);
                        var pattern = validate[0]; //new RegExp(validate[0]);
                        var controlValue = document.getElementById(controlName).value;
                        if (!reg.test(controlValue)) {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.Name1Error%>";
                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                            FirstName_Click();
                            return regValidation;
                        }
                    }
                }
                if (control[0] == "MiddleName") {
                    if (document.getElementById(controlName).value == "") {
                        if (validate[1] == "true") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.Name2Error%>";
                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                            MiddleName_Click();
                            return regValidation;
                        }
                    }
                    else {
                        var reg = new RegExp(validate[0]);
                        var controlValue = document.getElementById(controlName).value;
                        if (!reg.test(controlValue)) {
                            var txt = "<%= Resources.GlobalResources.Name2Error%>";
                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                            MiddleName_Click();
                            return regValidation;
                        }
                    }
                }
                if (control[0] == "Surname") {
                    if (document.getElementById(controlName).value == "") {
                        if (validate[1] == "true") {
                            document.getElementById('divError').style.display = 'block';
                            var txt = "<%= Resources.GlobalResources.Name3Error%>";
                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                            Surname_Click();
                            return regValidation;
                        }
                    }
                    else {
                        var reg = new RegExp(validate[0]);
                        var controlValue = document.getElementById(controlName).value;
                        if (!reg.test(controlValue)) {
                            var txt = "<%= Resources.GlobalResources.Name3Error%>";
                            document.getElementById('divError').style.display = 'block';
                            document.getElementById('lblError').innerHTML = txt;
                            regValidation = false;
                            Surname_Click();
                            return regValidation;
                        }
                    }
                }
            }
        }
        return regValidation;
    }
}