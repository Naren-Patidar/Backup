$(document).ready(function () {
    SelectTab();
    if ($(".message-error").is(":visible")) {
        //check if membership error
        if ($('#lblErrorId').text() == 'membership') {
            var selectedId = $('#statement-preferences input[groupname="preference"]:checked').attr("id");
            $("[data-conditionalContent=" + selectedId + "] .ddl-form-field").addClass("ddl-error").children('p').show();
        }
        else {
            $("#divnoemail .ddl-form-field").addClass("ddl-error").children('p').show();
            $('#email_validation').hide();
            $('#confirm_email_validation_invalid').hide();
            $('#confirm_email_validation_mismatch').hide();
        }
    }
    $('#statement-preferences input[GroupName="preference"]').on('click', function () {
        ResetError();
    });
    $(".ddl-form-field input[type='text']").on('click', function () {
        ResetError();
    });
    $('#txtAviosMembershipID').on('change', function () {
        if (ValidateStatementPref()) {
            $('#hdnAviosMembershipID').val($(this).val());
        }
    });
    $('#txtBAvios').on('change', function () {
        if (ValidateStatementPref()) {
            $('#hdnBAAviosMembershipID').val($(this).val());
        }
    });
    $('#txtconfirmemail').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#txtemail').on('change', function () {
        $('#hdnemail').val($(this).val());
    });
    if (isEmailVisible()) {
        var conditional_index = $('.conditional-content>div:visible:first').attr('data-conditionalcontent');
        $('#statementMessage').show();
        $('#divEmail').show();
        $('span[data-attr="' + conditional_index + '"]').show();
    }
    else {
        $('#statementMessage').hide();
        $('#divEmail').hide();
    }
    setScrollPosition();
});

$(document).on('change', '#statement-preferences input[GroupName="preference"]', function (event) {
    $('#txtAviosMembershipID').val("");
    $('#txtVirgnMembershipID').val("");
    $('#txtBAvios').val("");
    $('#txtemail').val("");
    $('#txtconfirmemail').val("");
    $('[data-conditionalContent="' + this.id + '"]').show();
    if (["1", "2", "3"].indexOf(this.id) != -1) {
        divEmailToggle();
        $('#statementMessage').hide();
        $('#divEmail').hide();
        $('#statementMessage').fadeIn();
        $('#divEmail').fadeIn();
        $('.schemeName').hide();
        $('span[data-attr="' + this.id + '"]').show();
    }
    else {
        $('#statementMessage').hide();
        $('#divEmail').hide();
    }
});

function divEmailToggle() {
    var hasEmail = $('#divEmail').attr('data-attr');
    if (hasEmail == '1') {
        $('#divhasemail').show();
        $('#divnoemail').hide();
    }
    else {
        $('#divhasemail').hide();
        $('#divnoemail').show();
    }
}

function SelectTab() {
    //make selected tab visible
    var selectedId = $('#statement-preferences input[groupname="preference"]:checked').attr("tabIndex");
    var header = $('#statement-preferences li[data-tabControl="' + selectedId + '"]');
   
    $("[tabIndex='" + selectedId + "'").click();
}


function ResetStatementPref() {
    $('#statement-preferences input[groupname="preference"]').prop('checked', false);
    $('.message-success').hide();
    ResetError();
    ResetOptedPreferences();
    scrollTo('#statement-preferences');
    return false;
}

function ValidateStatementPref() {
    var selectedId = $('#statement-preferences input[groupname="preference"]:checked').attr("id");
    
    var ctrl_id = "";
    var length = 0;
    switch (selectedId) {
        case "3":
            ctrl_id = "txtBAvios";
            wrapper_id = "miles_wrapper";
            length = 8;
            showTextError($('#miles_wrapper'), false);
            break;
        case "2":
            ctrl_id = "txtVirgnMembershipID";
            wrapper_id = "virgin_wrapper";
            length = 10;
            showTextError($('#virgin_wrapper'), false);
            break;
        case "1":
            ctrl_id = "txtAviosMembershipID";
            wrapper_id = "avios_wrapper";
            length = 16;                     
            showTextError($('#avios_wrapper'), false);
            break;
        default:
            // reset email to orignal
            $('#hdnemail').val($('#lblEmail').text());
    }
    var value = $('#' + ctrl_id).val();

        if (value == null)
            return true;

        if ($.trim(value).length >= 0 && $.trim(value).length < length) {
            showTextError($('#' + wrapper_id), true);
            return false;
        }
        else if (value) {
            if (!$.isNumeric(value)) {
                showTextError($('#' + wrapper_id), true);
                return false;
            }

            showTextError($('#' + wrapper_id), false);
        }
    return true;
}

function showTextError(ctrl, chk) {
    if (chk) {
        ctrl.addClass("ddl-error");
        ctrl.children('p').show();
    }
    else {
        ctrl.removeClass("ddl-error");
        ctrl.children('p').hide();
    }
    $('.message-success').hide();
    $('.message-error').hide();
}

function setScrollPosition() {
    if ($('#sm-ValInvalidMembership').is(':visible') || $('#sm-saved').is(':visible')) {
        scrollTo('.message-level-2');
    }

}

function ResetError() {
    $('.ddl-form-field').removeClass("ddl-error");
    $('.message-error').hide();
    $('.message-success').hide();
    $('.ddl-form-field').each(function () { $(this).find('p').hide(); });
    
}

function ResetOptedPreferences() {
    $('[data-conditionalContent]').hide();
}

function scrollTo(section) {
    $('body').animate({
        scrollTop: $(section).offset().top
    }, 'slow');
}

function ValidateAndTrackGAEvents(isGARequired) {
    
    var selectedId = $('#statement-preferences input[groupname="preference"]:checked').attr("id");
    var gaEventvalue = $('#statement-preferences li[data-tabControl="' + selectedId + '"]').attr('gaid');
    if (isGARequired)
    {
        _gaq.push(['_trackEvent', 'OptionsAndBenefits', 'Preference Selected', gaEventvalue]);
    }
    var chk = ValidateStatementPref();
    var emailChk = validateEmail();
    return chk && emailChk;
}

function validateEmail() {
    var chk = true;
    var regEmail = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$/;
    var objRegExpEmail = new RegExp(regEmail);
    if (isValidateEmail()) {
        if ($.trim($('#txtemail').val()).length == 0) {
            showTextError($('#divforConfirmEmail'), false);
            showTextError($('#divforEmail'), true);
            $('#email_validation').show();
            chk = false;
        }
        else if (!objRegExpEmail.test($('#txtemail').val())) {
            showTextError($('#divforConfirmEmail'), false);
            showTextError($('#divforEmail'), true);
            $('#email_validation').show();
            chk = false;
        }
        else if ($.trim($('#txtconfirmemail').val()).length == 0 || !objRegExpEmail.test($('#txtconfirmemail').val())) {
            showTextError($('#divforEmail'), false);
            showTextError($('#divforConfirmEmail'), true);
            $('#confirm_email_validation_invalid').show();
            $('#confirm_email_validation_mismatch').hide();
            $('#email_duplicate').hide();
            chk = false;
        }
        else if ($.trim($('#txtemail').val()) != $.trim($('#txtconfirmemail').val())) {
            showTextError($('#divforConfirmEmail'), true);
            showTextError($('#divforEmail'), false);
            $('#confirm_email_validation_mismatch').show();
            $('#confirm_email_validation_invalid').hide();
            $('#email_duplicate').hide();
            chk = false;
        }
    }
    return chk;
}

function isEmailVisible() {
    var conditional_index = $('.conditional-content>div:visible:first').attr('data-conditionalcontent');
    if (["1", "2", "3"].indexOf(conditional_index) != -1) {
        return true;
    }
    else {
        return false;
    }
}

function isValidateEmail() {
    var chk = true;
    chk = isEmailVisible();
    if (chk)
    {
        chk = $('#divnoemail').is(":visible");
    }
    return chk;
}

function changeEmail() {
    $('#divhasemail').hide();
    $('#divnoemail').fadeIn();
    $('#txtemail').val('');
    $('#txtconfirmemail').val('');
    $('#txtemail').focus();
}