$(document).ready(function () {
    if ($('.accountUnlock').length > 0) {
        getAccountStatus();
    }
});

function getAccountStatus() {
    $.ajax({
        type: "Post",
        url: "Ajax.aspx/GetAccountStatus",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d.ISBLOCKED == 'Y' || data.d.ACCESSATTEMPTS >= 8) {
                $('.accountUnlock').show();
                $('#attemptsVal').html(data.d.ACCESSATTEMPTS);
            }
        },
        error: function (response) {          
            console.error(response.d);
        }
    });
}

function unlockAccount() {
    $('.UnlockError').hide();
    $.ajax({
        type: "Post",
        url: "Ajax.aspx/UnlockAccount",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('.accountUnlock').hide();
        },
        error: function (response) {
            $('.btnUnlock').show();  
            console.error(response.d);
        }
    });
}