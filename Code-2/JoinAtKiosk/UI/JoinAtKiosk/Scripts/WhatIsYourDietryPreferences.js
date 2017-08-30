function Dietary_ChkBox_Click(objName) {

    if (document.getElementById(objName).className == 'input92 inputbluesquare') {
        document.getElementById(objName).className = 'input92 inputbluesquaretick';
        document.getElementById('hdnSelectedDietChk').value = document.getElementById('hdnSelectedDietChk').value + objName;
    }
    else {
        document.getElementById(objName).className = 'input92 inputbluesquare';
        document.getElementById('hdnSelectedDietChk').value = document.getElementById('hdnSelectedDietChk').value.replace(objName, '');
    }
    return false;
}

function Dietary_Click(objName) {
    //debugger;
    if (objName == 'lnkDietary1') {
        document.getElementById('lnkDietary1').className = 'dietary dietary1 selected';
        if (document.getElementById('lnkDietary2') != null) { document.getElementById('lnkDietary2').className = 'dietary dietary2'; }
        if (document.getElementById('lnkDietary3') != null) { document.getElementById('lnkDietary3').className = 'dietary dietary3'; }
        if (document.getElementById('lnkDietary4') != null) { document.getElementById('lnkDietary4').className = 'dietary dietary4'; }
        if (document.getElementById('lnkDietary5') != null) { document.getElementById('lnkDietary5').className = 'dietary dietary5'; }
        document.getElementById('hdnSelectedDietry').value = 'ID_DietryPrefOption1';
    }
    if (objName == 'lnkDietary2') {
        document.getElementById('lnkDietary1').className = 'dietary dietary1';
        document.getElementById('lnkDietary2').className = 'dietary dietary2 selected';
        if (document.getElementById('lnkDietary3') != null) { document.getElementById('lnkDietary3').className = 'dietary dietary3'; }
        if (document.getElementById('lnkDietary4') != null) { document.getElementById('lnkDietary4').className = 'dietary dietary4'; }
        if (document.getElementById('lnkDietary5') != null) { document.getElementById('lnkDietary5').className = 'dietary dietary5'; }
        document.getElementById('hdnSelectedDietry').value = 'ID_DietryPrefOption2';
    }
    if (objName == 'lnkDietary3') {
        document.getElementById('lnkDietary1').className = 'dietary dietary1';
        document.getElementById('lnkDietary2').className = 'dietary dietary2';
        document.getElementById('lnkDietary3').className = 'dietary dietary3 selected';
        if (document.getElementById('lnkDietary4') != null) { document.getElementById('lnkDietary4').className = 'dietary dietary4'; }
        if (document.getElementById('lnkDietary5') != null) { document.getElementById('lnkDietary5').className = 'dietary dietary5'; }
        document.getElementById('hdnSelectedDietry').value = 'ID_DietryPrefOption3';
    }
    if (objName == 'lnkDietary4') {
        document.getElementById('lnkDietary1').className = 'dietary dietary1';
        document.getElementById('lnkDietary2').className = 'dietary dietary2';
        document.getElementById('lnkDietary3').className = 'dietary dietary3';
        document.getElementById('lnkDietary4').className = 'dietary dietary4 selected';
        if (document.getElementById('lnkDietary5') != null) { document.getElementById('lnkDietary5').className = 'dietary dietary5'; }
        document.getElementById('hdnSelectedDietry').value = 'ID_DietryPrefOption4';
    }
    if (objName == 'lnkDietary5') {
        document.getElementById('lnkDietary1').className = 'dietary dietary1';
        document.getElementById('lnkDietary2').className = 'dietary dietary2';
        document.getElementById('lnkDietary3').className = 'dietary dietary3';
        document.getElementById('lnkDietary4').className = 'dietary dietary4';
        document.getElementById('lnkDietary5').className = 'dietary dietary5 selected';
        document.getElementById('hdnSelectedDietry').value = 'ID_DietryPrefOption5';
    }
    return false;
}

function Validate(controlList) {
    //debugger;
    if (controlList != "") {
        var controlList = controlList.value.split("|");
        for (var i = 1; i < controlList.length; i++) {
            var control = controlList[i].split(":");
            var controlName = control[0].toString().toUpperCase();
            if (controlName == "DIETRYPREFERENCES") {
                var validate = control[1].split(",");
                if (validate[1].toString().toUpperCase() == "TRUE" ) {
                    if ((document.getElementById('hdnSelectedDietry').value != '') || ((document.getElementById('lnkChk1') != null && document.getElementById('lnkChk1').className == 'input92 inputbluesquaretick') || (document.getElementById('lnkChk2') != null && document.getElementById('lnkChk2').className == 'input92 inputbluesquaretick'))) {
                        {
                            return true;
                        }
                    }
                    else {
                        //alert("field is required");
                        window.location = "ErrorMessage.aspx?PgName=WhatIsYourDietryPreferences&ctrlID=Diet&resID=DietErr&imgID=DietryPrefBreadCrumb";
                        return false;
                    }
                }
            }
        }
    }
    return true;
}
