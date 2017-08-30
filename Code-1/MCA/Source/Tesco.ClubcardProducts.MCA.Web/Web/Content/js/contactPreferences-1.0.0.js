$(document).ready(function () {
    SelectTab();
    $('input[type="text"]').on('keypress click', function () {
        ResetError();
    });
    $('#contact-preferences input[GroupName="ContactPreference"]').on('click', function () {
       // ResetError();
        DisableSection();
    });
    $('#fld_confirm_mobile').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#fld_Confirm_email').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    DisableSection();
   SetScrollPosition();
    SetDefaultFocus();
});

function DisableSection() {
    $('#contact-preferences input[groupname="ContactPreference"]').each(function () {
        if (!$(this).is(':checked')) {
            $(this).closest('fieldset').find('.ddl-input').each(function () {
                $(this).attr('disabled', true);
            });
        }
        else {
            $(this).closest('fieldset').find('.ddl-input').each(function () {
                $(this).attr('disabled', false);
            }); 
        }
    });
}

function SelectTab() {
    //make selected tab visible
    var selectedId = $('#contact-preferences input[groupname="ContactPreference"]:checked').attr("tabIndex");
   var header = $('#contact-preferences li[data-tabControl="' + selectedId + '"]');
    $("[tabIndex='" + selectedId + "'").click();
}

function showTextError(ctrl, chk) {
    if (chk) {
        $('#' + ctrl).addClass("ddl-error");
       // $('#'+ ctrl).each(function(){$(this).find('p').show()});
    }
    else {
        $('#' + ctrl).removeClass("ddl-error");
      //  $('.message-error').hide();
       // $('#' + ctrl).each(function () { $(this).find('p').hide() });
    }
    $('.message-success').hide();
}

function ResetError() {
    $('.ddl-error').each(function () {
        $(this).find('p').hide();
    });
    $('.message-error').hide();
    $('.message-success').hide();
    $('.ddl-form-field').removeClass('ddl-error');
   
}

function ValidateContactPref() {
    var selectedId = $('#contact-preferences input[GroupName="ContactPreference"]:checked').attr("tabIndex");
    var selectedTab = $('#contact-preferences input[GroupName="ContactPreference"]:checked').attr("id");
    var chk = true;
    switch (selectedTab) {
        case "email":
            chk = validateEmail();
            break;
        case "mobile":
            chk = validateMobile();
            break;
    }
    if (chk) {
        ResetError();
    }
    else {
        $('#contact-preferences li[data-tabControl="' + selectedId + '"]').click();
        $('.ddl-input').each(function () {
            $(this).attr('disabled', false);
        });
    }
    return chk;
}

function validateEmail() {
    var chk = true;
    var regEmail = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$/;
    var objRegExpEmail = new RegExp(regEmail);
    if ($.trim($('#fld_email').val()).length == 0) {
        showTextError('fld_Confirm_email_wrapper', false);
        showTextError('fld_email_wrapper', true);
        //$('.error').html($('#ContactPreference_InvalidEMail').val());
        $('#email_validation').show();
        chk = false;
    }
    else if (!objRegExpEmail.test($('#fld_email').val())) {
        showTextError('fld_Confirm_email_wrapper', false);
        showTextError('fld_email_wrapper', true);
        //$('.error').html($('#ContactPreference_InvalidEMail').val());
        $('#email_validation').show();
        chk = false;
    }
    else if ($.trim($('#fld_Confirm_email').val()).length == 0 || !objRegExpEmail.test($('#fld_Confirm_email').val())) {
        showTextError('fld_email_wrapper', false);
        showTextError('fld_Confirm_email_wrapper', true);
        //$('.error').html($('#ContactPreference_InvalidEMail').val());
        $('#confirm_email_validation_invalid').show();
        chk = false;
    }
    else if ($.trim($('#fld_email').val()) != $.trim($('#fld_Confirm_email').val())) {
        showTextError('fld_Confirm_email_wrapper', true);
        showTextError('fld_email_wrapper', false);
        // $('.error').html($('#ContactPreference_CompareEMail').val());
        $('#confirm_email_validation_mismatch').show();
        chk = false;
    }
    return chk;
}

function validateMobile() {
    var chk = true;
    var regNumeric = /^[0-9]*$/;
    var objRegExpNumeric = new RegExp(regNumeric);
    var minLength = parseInt($('#ContactPreference_MobileMinLength').val());
    var prefixes = $('#ContactPreference_MobilePrefixes').val().split(',');
    if ($.trim($('#fld_mobile').val()).length == 0) {
        showTextError('fld_confirm_mobile_wrapper', false);
        showTextError('fld_mobile_wrapper', true);
        // $('.error').html($('#ContactPreference_InvalidMobile').val());
        $('#mobile_validation').show();
        chk = false;
    }
    else if (!objRegExpNumeric.test($('#fld_mobile').val())) {
        showTextError('fld_confirm_mobile_wrapper', false);
        showTextError('fld_mobile_wrapper', true);
        // $('.error').html($('#ContactPreference_InvalidMobile').val());
        $('#mobile_validation').show();
        chk = false;
    }

    else if ($.trim($('#fld_mobile').val()).length < minLength) {
        showTextError('fld_confirm_mobile_wrapper', false);
        showTextError('fld_mobile_wrapper', true);
        // $('.error').html($('#ContactPreference_InvalidMobile').val());
        $('#mobile_validation').show();
        chk = false;
    }
    else if ($.trim($('#fld_confirm_mobile').val()).length == 0 || !objRegExpNumeric.test($('#fld_confirm_mobile').val()) || $.trim($('#fld_confirm_mobile').val()).length < minLength) {
        showTextError('fld_mobile_wrapper', false);
        showTextError('fld_confirm_mobile_wrapper', true);
        //$('.error').html($('#ContactPreference_InvalidMobile').val());
       $('#confirm_mobile_validation_invalid').show();
        chk = false;
    }
    else if ($.trim($('#fld_mobile').val()) != $.trim($('#fld_confirm_mobile').val())) {
        showTextError('fld_confirm_mobile_wrapper', true);
        showTextError('fld_mobile_wrapper', false);
        //$('.error').html($('#ContactPreference_CompareMobile').val());
        $('#confirm_mobile_validation_mismatch').show();
        chk = false;
    }
   
  /*  else 
    {
        var chk = false;
        $(prefixes).each(function () {
            if (!chk && $('#fld_mobile').val().substring(0, this.length) == this) {
                chk = true;
            }
        });
        if (!chk) {
            showTextError('fld_mobile_wrapper', true);
           // $('.error').html($('#ContactPreference_InvalidMobile').val());
        }
    } */
    
    return chk;
}

function scrollTo(section) {
    $('body').animate({
        scrollTop: $("#" + section).offset().top
    }, 'slow');
}

function SetScrollPosition() {    

    if ($('#sm-saved').is(':visible') || $('#sm-error').is(':visible')) {
        scrollTo('cpmessages');
    }
        if ($('#oiSaved').is(':visible')) {
            scrollTo('oiSaved');
        }
}

function SetDefaultFocus() {
    $('#divContactPrefs').on('click', function () {
        $("#divContactPrefs").keypress(function (e) 
        {
            if (e.keyCode == 13)
            {
                $("#btnSaveContactPrefs").click();
            }
        }); 
    });
    $('#CtrlOptIns').on('click', function () {
        $('#btnConfirm').focus(); 
    });
}