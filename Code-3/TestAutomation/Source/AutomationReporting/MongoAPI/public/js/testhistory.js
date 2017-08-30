(function ($, undefined) {
    $.widget('ui.testHistory', {
        options: {
            test_name: '',
            country: '',
            environment: ''        
        },
        _create: function () {
            var ctrl = this;
            ctrl._getDetails();
        },        
        table: $('<table />').addClass('table'),
        table_head: $('<thead />').append($('<tr />')
                            .append($('<th />').text('#'))
                            .append($('<th />').text('Test Name'))
                            .append($('<th />').text('Start Time'))
                            .append($('<th />').text('End Time'))
                            .append($('<th />').text('Duration'))
                            .append($('<th />').text('Result'))
                            ),
        table_body: $('<tbody />'),
        _get_test_detail: function () {
            var anchor = $('<a />').attr('href', '#').text('View Details');

            return anchor;
        },
        _create_modal_ui: function (data) {
            var ctrl = this;
            var ele = this.element;
            var op = this.options;
            var body = $('#history-body');
            $('#history-header').css('background-color', '#0480be').css('color', '#fff');
            $('#history-title').text(op.test_name + ' | ' + op.country);
            $(body).append(ctrl.table);
            $(ctrl.table).append(ctrl.table_head);
            $(ctrl.table).append(ctrl.table_body);
            $(ctrl.table_body).html('');
            var id = 1;
            $(data).each(function () {
                $(ctrl.table_body).append($('<tr/>')
                    .append($('<td />').text(id++))
                    .append($('<td />').text(this.TestResults[0].TestName))
                    .append($('<td />').text(this.TestResults[0].StartTime))
                    .append($('<td />').text(this.TestResults[0].EndTime))
                    .append($('<td />').text(this.TestResults[0].Duration))
                    .append($('<td />').text(this.TestResults[0].Result))                
                );
            });
            $(body).addClass('table-responsive');
        },
        _getDetails: function () {
            var ctrl = this;
            var ele = this.element;
            var url = "DashboardV2.aspx/GetTestHistory?q=" + new Date().getTime();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ test_name: ctrl.options.test_name, country: ctrl.options.country, environment: ctrl.options.environment }),
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
