(function ($, undefined) {
    $.widget('ui.detail', {
        options: {
            country: '',
            environment: '',
            category: '',
            result: '',
            plot: {}
        },
        _create: function () {
            var ctrl = this;
            ctrl._getDetails();
        },
        model: $('<div />').addClass('modal fade').attr('role', 'dialog'),
        model_dialog: $('<div />').addClass('modal-dialog'),
        model_content: $('<div />').addClass('modal-content'),
        model_header: $('<div />').addClass('modal-header'),
        close_button: $('<button />').addClass('close').attr('data-dismiss', 'modal').text('&times;'),
        model_header_h4: $('<h4 />').addClass('modal-title'),
        model_body: $('<div />').addClass('modal-body'),
        table: $('<table />').addClass('table'),
        table_head: $('<thead />').append($('<tr />')
                            .append($('<th />').text('#'))
                            .append($('<th />').text('Test Name'))
                            .append($('<th />').text('History'))
                            .append($('<th />').text('Details'))
                            .append($('<th />').text('Current Result'))),
        table_body: $('<tbody />'),
        _get_test_history: function (test_name, country, environment) {
            var ctrl = this;
            var ele = this.element;
            var anchor = $('<a />').attr('href', '#').text('View History');
            $(anchor).click(function () {
                $(this).attr('data-toggle', 'modal').attr('data-target', '#historyModel')
                $(this).testHistory({
                    test_name: test_name,
                    country: country,
                    environment: environment
                });
            });
            return anchor;
        },
        _get_test_detail: function (id) {
            var anchor = $('<a />').attr('href', '#').text('View Details');
            $(anchor).click(function () {
                $(this).attr('data-toggle', 'modal').attr('data-target', '#testModal')
                $(this).testDetails({
                    id: id                    
                });
            });
            return anchor;
        },
        _create_modal_ui: function (data) {
            var ctrl = this;
            var ele = this.element;
            var op = this.options;
            var body = $('#detail-body');
            $('#detail-header').css('background-color', '#0480be').css('color', '#fff');
            $('#detail-title').text(op.country + ' | ' + op.category + ' | ' + op.environment + ' | ' + op.result);
            $(body).append(ctrl.table);
            $(ctrl.table).append(ctrl.table_head);
            $(ctrl.table).append(ctrl.table_body);
            $(ctrl.table_body).html('');
            var id = 1;
            $(data).each(function () {
                $(ctrl.table_body).append($('<tr/>')
                    .append($('<td />').text(id++))
                    .append($('<td />').text(this.TestName))
                    .append($('<td />').append(ctrl._get_test_history(this.TestName, op.country, op.environment)))
                    .append($('<td />').append(ctrl._get_test_detail(this._id)))
                    .append($('<td />').text(this.Result))
                );
            });
            $(body).addClass('table-responsive');
        },
        _getDetails: function () {
            var ctrl = this;
            var url = "DashboardV2.aspx/GetLatestResultDetail?q=" + new Date().getTime();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ environment: ctrl.options.environment, country: ctrl.options.country, category: ctrl.options.category, result: ctrl.options.result }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data.d.results);
                    ctrl._create_modal_ui(data.d.results);
                    $(ctrl.options.plot).attr('data-toggle', 'modal').attr('data-target', '#myModal');
                    ctrl._destroy();
                },
                error: function (jqXHR, exception) {
                }
            });
        },
        _destroy: function () {

        }
    });
})(jQuery);
