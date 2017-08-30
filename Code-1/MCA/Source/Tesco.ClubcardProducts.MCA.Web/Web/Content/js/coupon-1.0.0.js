$(document).ready(function () {

    $('.select').click(function () {
        $('.message-error').hide();
        $('.selectall').prop('checked', $('.select:checked').length == $('.select').length);
    });

    $('#btnDownloadCoupons').click(function () {
        if ($('.select:checked').length == 0) {
            $('.message-error').show();
            return false;
        }
    });

    $('.selectall').on('click', function () {
        $('.message-error').hide();
        $('.select').each(function () {            
           if (typeof $(this).attr('disabled') == typeof undefined) {            
            $(this).prop('checked', $('.selectall').prop('checked'));
            }
        });
    });

    $('.previewCoupon').on('click', function () {
        var userWidth = screen.availWidth;
        var userHeight = screen.availHeight;
        var leftPos;
        var topPos;
        var popW = 800;   //set width here
        var popH = 510;   //set height here
        var settings = 'modal,scrollBars=no,resizable=no,toolbar=no,menubar=no,location=no,directories=no,';
        leftPos = (userWidth - popW) / 2,
                topPos = (userHeight - popH) / 2;
        settings += 'left=' + leftPos + ',top=' + topPos + ',width=' + popW + ', height=' + popH + '';

        var ShowInfo = window.open($(this).attr('href'), 'ShowImage', settings, false);
        ShowInfo.focus();
        return false;
    });
});