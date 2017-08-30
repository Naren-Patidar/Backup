(function ($, undefined) {
    $.widget('ui.testDetails', {
        options: {
            id: ''            
        },
        _create: function () {
            var ctrl = this;
            ctrl._getDetails();
        },
        form: $('<form />').attr('role', 'form'),
        form_group: function (label, value) {
            var group = $('<div />').addClass('form-group');
            var labelCtrl = $('<label />').text(label);
            var parCtrl = $('<p />').addClass('form-control-static').text(value);
            $(group).append(labelCtrl).append(parCtrl);
            return group;
        },
        _create_modal_ui: function (data) {
            var ctrl = this;
            var ele = this.element;
            var op = this.options;
            var body = $('#test-body');
            $('#test-header').css('background-color', '#0480be').css('color', '#fff');
            $('#test-title').text(data[0].TestResults[0].TestName);
            $(body).append(ctrl.form);
            $(ctrl.form).html('');
            var id = 1;
            $(data).each(function () {
                $(ctrl.form).append(ctrl.form_group('Test Name', data[0].TestResults[0].TestName));
                $(ctrl.form).append(ctrl.form_group('Result', data[0].TestResults[0].Result));
                $(ctrl.form).append(ctrl.form_group('Duration', data[0].TestResults[0].Duration));
                $(ctrl.form).append(ctrl.form_group('Message', data[0].TestResults[0].Message));
                $(ctrl.form).append(ctrl.form_group('StackTrace', data[0].TestResults[0].StackTrace));
                $(ctrl.form).append(ctrl.form_group('Start Time', data[0].TestResults[0].StartTime));
                $(ctrl.form).append(ctrl.form_group('End Time', data[0].TestResults[0].EndTime));
            });
            $(body).addClass('table-responsive');
        },
        _getDetails: function () {
            var ctrl = this;
            var ele = this.element;
            var url = "DashboardV2.aspx/GetTestDetails?q=" + new Date().getTime();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ id: ctrl.options.id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data.d);
                    ctrl._create_modal_ui(data.d);
                    ctrl._destroy();
                },
                error: function (jqXHR, exception) {
                }
            });
        },
        _destroy: function () { }
    });
})(jQuery);
