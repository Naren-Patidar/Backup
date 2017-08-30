$(document).ready(function () {
    var ButtonKeys = { "EnterKey": 13 };
    $(function () {
        $("#MainForm").keypress(function (e) {
            if (e.which == ButtonKeys.EnterKey) {
                var defaultButtonId = $(this).attr("defaultbutton");
                $("#" + defaultButtonId).click();
                return false;
            }
        });
    });

    $("#fld_yearOfBirth").change(function () {
        val = $(this).val();
        if (val == '') {
            $("#txtYear").val("-Year-");
        }
        else {
            $("#txtYear").val(val);
        }
    });
});

