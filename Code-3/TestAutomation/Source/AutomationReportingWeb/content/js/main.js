

$(document).ready(function () {
    var refreshJob = function () {
        $('.job-footer').each(function () {
            UpdateJobStatus(this);
        })
    };

    var refreshMasterJob = function () {
        $('.job-panel').each(function () {
            getMasterstatus(this);
        })
    };
    //var timer2 = window.setInterval(refreshMasterJob, 5000);
    //var timer2 = window.setInterval(refreshJob, 5000);
    //clearTimeout(timer);

    $('.job-panel').on('click', function () {
        $('.job-panel').each(function () { $(this).removeClass('active'); });
        $(this).addClass('active');
        $('.job-panel').each(function () {
            if (!$(this).hasClass('active')) {
                $(this).click();
            }
        });
    });

    $('.btnRefresh').on('click', function () {
        var parent = $(this).closest('div.panel-footer');
        GetLatestStatus(parent);
    });
    $('.btnRerun').on('click', function () {
        var parent = $(this).closest('div.panel-footer');
        RunJob(parent);
    });
});

function getMasterstatus(parent) {
    $(parent).find('span.masterstatus').text('');
    $(parent).find('img.loader').show();
    var masterStatus = "Ready";
    //alert($(parent).find('span.status').length);
    $(parent).closest('div.panel').find('span.status').each(function () {
        if ($(this).text() == "Running")
        { masterStatus = "Running"; }
        else if ($(this).text() == "")
        { masterStatus = ""; }
    });
    $(parent).find('span.masterstatus').text(masterStatus);
    if (masterStatus != "") {
        $(parent).find('img.loader').hide();
    }
}

function UpdateJobStatus(parent) {
    $(parent).find('img.loader').show();
    $(parent).find('span.status').text(''); 
    var category = $(parent).find('span.category').attr('data');
    var country = $(parent).find('span.country').attr('data');
    var browser = $(parent).find('span.browser').attr('data');
    var reqURL = "Dashboard.aspx/GetStatus";
    jQuery.ajax({
        type: "POST",
        url: reqURL,
        data: '{country:"' + country + '",category:"' + category + '",browser:"' + browser + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, status, jqXHR) {
            $(parent).find('img.loader').hide();
            var currentStatus = $(parent).find('span.status').text();
            if (currentStatus != data.d) {
                //GetLatestStatus(parent)
            }
            $(parent).find('span.status').text(data.d);
        },
        error: function (jqXHR, status) {
            $(parent).find('img.loader').hide();
            //alert(JSON.stringify(jqXHR));

        }
    });
}

function GetLatestStatus(parent) { 
    var category = $(parent).find('span.category').attr('data');
    var country = $(parent).find('span.country').attr('data');
    var browser = $(parent).find('span.browser').attr('data');    
    var reqURL = "Dashboard.aspx/GetUpdatedInformation";
    jQuery.ajax({
        type: "POST",
        url: reqURL,
        data: '{country:"' + country + '",category:"' + category + '",browser:"' + browser + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, status, jqXHR) {

            var tile = $(parent).parent();
            $(tile).find('.lblPercentage').text(data.d.ResultPercentage);
            $(tile).find('.spnDuration').text(data.d.Duration);
            $(tile).find('.spnLU').text(data.d.ReportDateString + ' ' + data.d.ReportTimeString);
            var newClass = data.d.IsGreen ? "panel panel-green" : data.d.IsSuccess ? "panel panel-success" : data.d.IsAmber ? "panel panel-yellow" : "panel panel-red";
            $(tile).attr('class', newClass);
        },
        error: function (jqXHR, status) {
            //alert(JSON.stringify(jqXHR));

        }
    });
}


function RunJob(parent) {
    var category = $(parent).find('span.category').attr('data');
    var country = $(parent).find('span.country').attr('data');
    var browser = $(parent).find('span.browser').attr('data');    
    var reqURL = "Dashboard.aspx/Run";
    jQuery.ajax({
        type: "POST",
        url: reqURL,
        data: '{country:"' + country + '",category:"' + category + '",browser:"' + browser + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, status, jqXHR) {
            //alert(JSON.stringify(data));
        },
        error: function (jqXHR, status) {
            //alert(JSON.stringify(jqXHR));

        }
    });
}