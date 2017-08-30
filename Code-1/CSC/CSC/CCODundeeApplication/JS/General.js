/*
FileName: PersonalDetails.js
Purpose: Generic javascript functions for validations at client side
Author: Akash Jain
Date: 15 June 2010
Project: Clubcardonline 
*/

function trim(str) {
    str = str.replace(/^\s+|\s+$/g, "");
    return str;
}

function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}

function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}

function ValidateDate(dayID, monthID, yearID, ID, isRequired) {
    var daysInMonth = DaysArray(12);
    var day = document.getElementById(dayID).value;
    var month = document.getElementById(monthID).value;
    var year = document.getElementById(yearID).value;
    var spanErrorID = "";
    var spanClass = "";

    if (ID == 0) {
        spanErrorID = "spanDOB0";
        spanClass = "spanDOBError0";
    }
    else if (ID == 1) {
        spanErrorID = "spanDOB1";
        spanClass = "spanDOBError1";
    }
    else if (ID == 2) {
        spanErrorID = "spanDOB1";
        spanClass = "spanDOBError1";
    }

    var invalidDate = false;
    var notEligible = false;
    
    if((day == "" && (month =="" || month == "- Select Month -") && (year == "" || year == "Year"))){
        if(isRequired == true)
            invalidDate = true;
    }
    else if ((day == "" || (month == "" || month == "- Select Month -") || (year == "" || year == "Year"))) {
        invalidDate = true;
    }  
    else if ((month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month])
        invalidDate = true;
    else {
        today = new Date();
        //DOB = new Date(year, month, day);
        age = today.getFullYear() - year;

        if (age == 0 || isNaN(age))
            invalidDate = true;
        else {
            var newYear = parseInt(year) + parseInt(age)
            newDate = new Date(newYear, month - 1, day); //Fixed MKTG00003208 defect. UI is sending Month from 1- 12.
            diff = today.getTime() - newDate.getTime();
            if (diff < 0)
                age = age - 1;

            if (age < 18)
                notEligible = true;
        }
    }

    if (invalidDate) {
        document.getElementById(spanErrorID).innerText = "Date Of Birth is invalid";
        document.getElementById(spanErrorID).style.display = '';
        document.getElementById(spanClass).className = "errorFld dtFld";

        return "Date Error";
    }
    else if (notEligible) {
        document.getElementById(spanErrorID).innerText = "Please note you must be over 18 to be a member of Clubcard";
        document.getElementById(spanErrorID).style.display = '';
        document.getElementById(spanClass).className = "errorFld dtFld";
        return "Date Error";
    }
    else {
        document.getElementById(spanErrorID).style.display = 'none';
        document.getElementById(spanClass).className = "dtFld";

        return "";
    }

}

//To check the age
function ValidateAge(dayID, monthID, yearID) {
    var daysInMonth = DaysArray(12);
    var day = document.getElementById(dayID).value;
    var month = document.getElementById(monthID).value;
    var year = document.getElementById(yearID).value;

    var invalidDate = false;
    var notEligible = false;

    today = new Date();
    age = today.getFullYear() - year;

    if (age == 0 || isNaN(age))
        invalidDate = true;
    else {
        var newYear = parseInt(year) + parseInt(age)
        newDate = new Date(newYear, month - 1, day);
        diff = today.getTime() - newDate.getTime();
        if (diff < 0)
            age = age - 1;

        return age;
    }
}

//Validates the textbox fields
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

function ValidateDateReport(dayID, monthID, yearID, ID, isRequired) {
    var daysInMonth = DaysArray(12);
    var day = document.getElementById(dayID).value;
    var month = document.getElementById(monthID).value;
    var year = document.getElementById(yearID).value;
    var invalidDate = false;
    if ((day == "" && (month == "" || month == "- Select Month -") && (year == "" || year == "Year"))) {
        if (isRequired == true)
            invalidDate = true;
    }
    else if ((day == "" || (month == "" || month == "- Select Month -") || (year == "" || year == "Year"))) {
        invalidDate = true;
    }
    else if ((month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month])
        invalidDate = true;
   
    if (invalidDate) {
       
     
    }

    else {
        

        return "";
    }

}