/*
FileName: CustomerDetails.js
Purpose: Javascript file for validating customer details screen
Author: Sadanand Vama
Date: 29 June 2010
Project: Clubcardonline (Dundee)
*/

 
function ValidatePage(ddlTitle0,
                    firstNameID0,
                    initialID0,
                    surnameID0,
                    ddlDayID0,
                    ddlMonthID0,
                    ddlYearID0,
                    radioMaleID0,
                    radioFemaleID0,
                    ddlTitle1,
                    firstNameID1,
                    initialID1,
                    surnameID1,
                    ddlDayID1,
                    ddlMonthID1,
                    ddlYearID1,
                    radioMaleID1,
                    radioFemaleID1,
                    postCodeID,
                    phoneNumberID,
                    noOfPeopleID,
                    age1ID,
                    age2ID,
                    age3ID,
                    age4ID,
                    age5ID,
                    hdnPostCodeNumberID,
                    ddlAddressID,
                    addressLineID,
                    streetID,
                    townID,
                    hdnNumberOfCustomers,
                    lblMessageID,
                    dvAssociateCustomer,
                    
                    RegionCode) {
    var regForeName = /^[a-zA-Z]+(([\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
    var regInitial = /^[a-zA-Z]*$/;
    var regSurName = /^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$/;
    var regNumeric = /^[0-9 ]*$/;
    var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
     var regPostCode1 = /^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}$/;
    //var regPostCode = /\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g;
    var regPostCodeForUSL = /^[0-9]*$/;

   //NGC Changes
    var regMail =  /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

    var gender, gender1;

    var errorFlag = "";
    var errMsgFirstName = "Please note First Name is required, or the entered name is not valid.";
    var errMsgMiddleName = "Please enter a valid letter";
    var errMsgSurname = "Please note Surname is required, or the entered name is not valid.";
    var errAddress = "Please select an Address or enter a house no./name";
    var errMsgPhoneNumber = "Please note Preferred contact number entered is not a valid number";
//    var errMsgNoHHPersons = "Please enter a valid number between 1 and 99";
//    var errMsgAge = "Please enter an Age between 0 and 99";
    var errMsgPostCode1 = "Postcode is required or not valid";
    var errMsgPostCodeForUSL = "Postcode is not valid";
    var errMsgPostCode2 = "Sorry, the Address change is unfinished. Please complete the update or change back to the original postcode";
    var errMsgGender1 = "Sorry, the Gender selected doesn't match with the Title choosen";
    var errMsgGender2 = "Please select Gender";
     //NGC Changes
    var errMsgEmail = "Please enter a valid Email Address";

    //Name validation
    errorFlag = ValidateTextBox(firstNameID0, regForeName, false, false, "spanFirstName0", errMsgFirstName);
    errorFlag = errorFlag + ValidateTextBox(initialID0, regInitial, true, true, "spanMiddleName0", errMsgMiddleName);
    errorFlag = errorFlag + ValidateTextBox(surnameID0, regSurName, false, false, "spanSurname0", errMsgSurname);

    //Date validation
    errorFlag = errorFlag + ValidateDate(ddlDayID0, ddlMonthID0, ddlYearID0, "0", false);

    //Gender validation
    //For Configuarable mandatory fields US/UK
    if (RegionCode == "en-GB")
    {
        if (document.getElementById(radioMaleID0).checked) {
            gender = "male";
        }
        else if (document.getElementById(radioFemaleID0).checked) {
            gender = "female";
        }
        else {
            document.getElementById("spanGender0").className = "gender errorFld";
            document.getElementById("spanGenderError0").innerText = errMsgGender2;
            document.getElementById("spanGenderError0").style.display = '';
            errorFlag = errorFlag + "gender error";
        }
    }
    //Gender validations
    var title = document.getElementById(ddlTitle0).value;
    if ((title == "Mr" && gender == "female") || ((title == "Mrs" || title == "Miss" || title == "Ms") && gender == "male")) {

        document.getElementById("spanGender0").className = "gender errorFld";
        document.getElementById("spanGenderError0").innerText = errMsgGender1;
        document.getElementById("spanGenderError0").style.display = '';

        errorFlag = errorFlag + "gender error";
    }
    else if (gender != null) {
        document.getElementById("spanGenderError0").style.display = 'none';
        document.getElementById("spanGender0").className = "gender";
    }

    //********************** Validations for Associate customer ***********************
    if(document.getElementById(dvAssociateCustomer) != null)
    {
        if (!document.getElementById(dvAssociateCustomer).disabled) {
            if (document.getElementById(hdnNumberOfCustomers).value > 1) {
                //Name validation
                errorFlag = errorFlag + ValidateTextBox(firstNameID1, regForeName, false, false, "spanFirstName1", errMsgFirstName);
                errorFlag = errorFlag + ValidateTextBox(initialID1, regInitial, true, true, "spanMiddleName1", errMsgMiddleName);
                errorFlag = errorFlag + ValidateTextBox(surnameID1, regSurName, false, false, "spanSurname1", errMsgSurname);

                //Date validation
                errorFlag = errorFlag + ValidateDate(ddlDayID1, ddlMonthID1, ddlYearID1, "1", false);

                //Gender validation
                //For Configuarable mandatory fields US/UK
                if (RegionCode == "en-GB")
                {
                    if (document.getElementById(radioMaleID1).checked) {
                        gender1 = "male";
                    }
                    else if (document.getElementById(radioFemaleID1).checked) {
                        gender1 = "female";
                    }
                    else {
                        document.getElementById("spanGender1").className = "gender errorFld";
                        document.getElementById("spanGenderError1").innerText = errMsgGender2;
                        document.getElementById("spanGenderError1").style.display = '';
                        errorFlag = errorFlag + "gender error";
                    }
                }
                //Gender validations
                var title = document.getElementById(ddlTitle1).value;
                if ((title == "Mr" && gender1 == "female") || ((title == "Mrs" || title == "Miss" || title == "Ms") && gender1 == "male")) {
                    document.getElementById("spanGender1").className = "gender errorFld";
                    document.getElementById("spanGenderError1").innerText = errMsgGender1;
                    document.getElementById("spanGenderError1").style.display = '';

                    errorFlag = errorFlag + "gender error";
                }
                else if (gender1 != null) {
                    document.getElementById("spanGenderError1").style.display = 'none';
                    document.getElementById("spanGender1").className = "gender";
                }
            }
        }
    }
    //****************************************************************

    //Post code validation
    //For Configuarable mandatory fields US/UK
    if (RegionCode == "en-GB")
    {
        var postCodeError = ValidateTextBox(postCodeID, regPostCode, false, false, "spanPostCode", errMsgPostCode1);
        var postCodeError = ValidateTextBox(postCodeID, regPostCode1, false, false, "spanPostCode", errMsgPostCode1);
    }
    else
    {
            var postCodeError = ValidateTextBox(postCodeID, regPostCodeForUSL, true, true, "spanPostCode", errMsgPostCodeForUSL);
    }

    errorFlag = errorFlag + postCodeError;
    //For Configuarable mandatory fields US/UK
    if (RegionCode == "en-GB")
    {
        if (postCodeError == "" && document.getElementById(hdnPostCodeNumberID) != null) {
            var hdnPostCode = document.getElementById(hdnPostCodeNumberID).value;
            var postCode = document.getElementById(postCodeID).value;

            if (hdnPostCode.toUpperCase() != postCode.toUpperCase())//If new post code is entered and find button is not clicked
            {
                document.getElementById("spanPostCode").innerText = errMsgPostCode2;
                document.getElementById("spanPostCode").style.display = '';
                errorFlag = errorFlag + "Find button is not clicked";
            }
            else {
                document.getElementById("spanPostCode").style.display = 'none';
            }
        }
    }

    //Address validation
    var houseNo = document.getElementById(addressLineID).value;

  
    if (document.getElementById(ddlAddressID).value == "" && houseNo == "")//if no house number is selected or entered
    {
        document.getElementById("spanAddressError").innerText = errAddress;
        document.getElementById("spanAddressError").style.display = '';
        document.getElementById("spanAddress").className = "errorFld dtAddress";

        errorFlag = errorFlag + "Address is not entered";
    }
    else {
        document.getElementById("spanAddressError").innerText = "";
        document.getElementById("spanAddressError").style.display = 'none';
        document.getElementById("spanAddress").className = "dtAddress";
    }
    
    //Phone number validation
    errorFlag = errorFlag + ValidateTextBox(phoneNumberID, regNumeric, true, false, "spanPhoneNumber", errMsgPhoneNumber);
    
    //NGC Chnage
    //Email number validation
     //For Configuarable mandatory fields US/UK
//    if (RegionCode == "en-GB")
//    {
//        errorFlag = errorFlag + ValidateTextBox(txtMail, regMail, true, false, "spanEmail", errMsgEmail);
//    }
//    else
//    {
//            errorFlag = errorFlag + ValidateTextBox(txtMail, regMail, false, false, "spanEmail", errMsgEmail);
//    }
    //Age validation
    //errorFlag = errorFlag + ValidateAge(noOfPeopleID, regNumeric, "spanNoHHPersons", errMsgNoHHPersons);
    //errorFlag = errorFlag + ValidateAge(age1ID, regNumeric, "spanAge1", errMsgAge);
    //errorFlag = errorFlag + ValidateAge(age2ID, regNumeric, "spanAge2", errMsgAge);
    //errorFlag = errorFlag + ValidateAge(age3ID, regNumeric, "spanAge3", errMsgAge);
    //errorFlag = errorFlag + ValidateAge(age4ID, regNumeric, "spanAge4", errMsgAge);
    //errorFlag = errorFlag + ValidateAge(age5ID, regNumeric, "spanAge5", errMsgAge);

//    if (errorFlag.indexOf("Age Error") == -1) {
//        errorFlag = errorFlag + ValidateAgeAgainstNoHHPersons(age1ID, age2ID, age3ID, age4ID, age5ID, noOfPeopleID);
//    }

//    if (errorFlag != "") {
//        document.getElementById(lblMessageID).innerText = "Please correct following information";
//        return false;
//    }
//    else {
//        return true;
//    }
}

function ValidatePostCode(postCodeID, successID, RegionCode) {
    //Post code validation
    var postCodeError = "";
    var regPostCode = /^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$/;
     //var regPostCode = /\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g;
    var regPostCode1 = /\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g;
    var regPostCodeForUS = /^[0-9]*$/;
    var errMsgPostCode = "Postcode is required or not valid";
    var errMsgPostCodeForUS = "Postcode is not valid";
    document.getElementById(successID).innerText = "";
    
    //For Configuarable mandatory fields US/UK
    if (RegionCode == "en-GB")
    {
        var postCodeError = ValidateTextBox(postCodeID, regPostCode, false, false, "spanPostCode", errMsgPostCode);
        var postCodeError = ValidateTextBox(postCodeID, regPostCode1, false, false, "spanPostCode", errMsgPostCode);
    }
    else
    {
            var postCodeError = ValidateTextBox(postCodeID, regPostCodeForUS, true, false, "spanPostCode", errMsgPostCodeForUS);

    }
    if (postCodeError == "")
        return true
    else
        return false
}

function ValidateAgeAgainstNoHHPersons(age1ID, age2ID, age3ID, age4ID, age5ID, noOfHHPeopleID) {
    var errorMessage = "Sorry, you have entered too many ages for the size of the household. Please check and re-enter.";

    var noOFHHPersons = document.getElementById(noOfHHPeopleID).value;

    var age1 = trim(document.getElementById(age1ID).value);
    var age2 = trim(document.getElementById(age2ID).value);
    var age3 = trim(document.getElementById(age3ID).value);
    var age4 = trim(document.getElementById(age4ID).value);
    var age5 = trim(document.getElementById(age5ID).value);

    var ageCount = 0;
    var errorTextBox = 0;
    var isValid = true;

    for (var j = 1; j <= 6; j++) {
        if (eval("age" + j) != "") {
            ageCount++;
            //errorTextBox = j;
        }

//        if (ageCount >= noOFHHPersons) {
//            isValid = false;
//            // break;
//        }
    }

//    if (!isValid && ageCount != 0) {

//        if (errorTextBox == 1) {
//            document.getElementById(age1ID).className = "errorFld";
//            document.getElementById("spanAge1").innerText = errorMessage;
//            document.getElementById("spanAge1").style.display = '';
//        }
//        else if (errorTextBox == 2) {
//            document.getElementById(age2ID).className = "errorFld";
//            document.getElementById("spanAge2").innerText = errorMessage;
//            document.getElementById("spanAge2").style.display = '';
//        }
//        else if (errorTextBox == 3) {
//            document.getElementById(age3ID).className = "errorFld";
//            document.getElementById("spanAge3").innerText = errorMessage;
//            document.getElementById("spanAge3").style.display = '';
//        }
//        else if (errorTextBox == 4) {
//            document.getElementById(age4ID).className = "errorFld";
//            document.getElementById("spanAge4").innerText = errorMessage;
//            document.getElementById("spanAge4").style.display = '';
//        }
//        else if (errorTextBox == 5) {
//            document.getElementById(age5ID).className = "errorFld";
//            document.getElementById("spanAge5").innerText = errorMessage;
//            document.getElementById("spanAge5").style.display = '';
//        }
//        return "Age error";
//    }
//    else {
//        return "";
//    }
}

function ValidateTextBox(objID, regExp, isEmptyAllowed, isOneCharAllowed, errorPlaceHolder, errMsg) {
    var obj = document.getElementById(objID);
    var objPlaceHolder = document.getElementById(errorPlaceHolder);
    var strValue = obj.value;
    strValue = trim(strValue);

    obj.value = strValue;
    var objRegExp = new RegExp(regExp);

    if (isEmptyAllowed && strValue == "") //If empty allowed
    {
        document.getElementById(errorPlaceHolder).style.display = 'none';
        obj.className = '';
        return "";
    }
    else if (regExp != "" && strValue != "" && objRegExp.test(strValue)) //If input string valid
    {
        objPlaceHolder.style.display = 'none';
        obj.className = '';
        return "";
    }
    else //In case of wrong input field show error message
    {
        objPlaceHolder.style.display = '';
        objPlaceHolder.innerText = errMsg;
        obj.className = 'errorFld';
        return "Error";
    }
}

function ValidateAge(ageId, regExp, errorPlaceHolder, errMsg) {
    var obj = document.getElementById(ageId);
    var objPlaceHolder = document.getElementById(errorPlaceHolder);

    var strValue = obj.value;
    strValue = trim(strValue);

    var objRegExp = new RegExp(regExp);

    if (regExp != "" && strValue != "") {
        if (objRegExp.test(strValue)) //If input string valid
        {
            if (((errorPlaceHolder == "spanNoHHPersons") && (strValue < 1 || strValue > 99))
            || ((errorPlaceHolder != "spanNoHHPersons") && (strValue < 0 || strValue > 99))) {
                objPlaceHolder.innerText = errMsg;
                objPlaceHolder.style.display = '';
                obj.className = 'errorFld';
                return "Age Error";
            }
            else {
                objPlaceHolder.style.display = 'none';
                obj.className = '';
                return "";
            }
        }
        else {
            objPlaceHolder.innerText = errMsg;
            objPlaceHolder.style.display = ''
            obj.className = 'errorFld';
            return "Age Error";
        }
    }
    else {
        objPlaceHolder.style.display = 'none';
        obj.className = '';
        return "";
    }
}

//Function to clear the dietary preferences
function ClearSelection(ID) {
    if (ID == 0) {
        document.getElementById("ctl00_PageContainer_radioVegeterian0").checked = false;
        document.getElementById("ctl00_PageContainer_radioKosher0").checked = false;
        document.getElementById("ctl00_PageContainer_radioHalal0").checked = false;
    }
    else if (ID == 1) {
        document.getElementById("ctl00_PageContainer_radioVegeterian1").checked = false;
        document.getElementById("ctl00_PageContainer_radioKosher1").checked = false;
        document.getElementById("ctl00_PageContainer_radioHalal1").checked = false;
    }
    return false;
}
//Added by Robin Apoto
//Date: 07/Sept/2010
function OnSelectedIndexChange(ddlAddress, addressLine1, addressLine1Index) {
    if (document.getElementById(ddlAddress) != null) {
        var index = document.getElementById(ddlAddress).selectedIndex;
        var addressValue = document.getElementById(ddlAddress).options[index].value;
        document.getElementById(addressLine1).value = addressValue;
        document.getElementById(addressLine1Index).value = index;
        true;
    }
    else {
        false
    }
}