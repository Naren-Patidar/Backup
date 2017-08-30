(function ($, undefined) {
    $.widget('ui.RunButton', {
        Container: {},
        options: {
            country: '',
            category: '',
            environment: '',
            browser: ''
        },
        _create: function () {
            var ctrl = this;
            ctrl._createUI();
        },
        _createUI: function () {
            var ctrl = this;
            var ele = this.element;
            var heading = $(ele).parent().find('.panel-heading').css('overflow', 'auto');
            ctrl.Container = $('<span />').addClass('pull-right');
            ctrl._getStatus(function (data) {
                console.log('data' + JSON.stringify(data));
                if (data.d == false) {
                    ctrl.Container.append($('<i />').addClass('fa fa-2x fa-frown-o red').css('title', 'error'));
                }
                else {
                    if ((data.d && data.d.status == 0) || data.d == null) {
                        ctrl.Container.append($('<i />').addClass('fa fa-2x fa-youtube-play').css('cursor', 'pointer'));
                        $(ctrl.Container).click(function () { ctrl._run(); });
                    }
                    else {

                        ctrl.Container.append($('<i />').addClass('fa fa-2x fa-spinner fa-spin')).attr('title', 'since :' + data.d.lastStartTime);
                    }
                }
            });
            $(heading).append(ctrl.Container);

        },
        _run: function () {
            var ctrl = this;
            var country = this.options.country;
            var environment = this.options.environment;
            var category = this.options.category.substring(0, 2);
            var data = { environment: environment, country: country, category: category };
            console.log(data);
            var url = "Ajax.aspx/Trigger";
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data.d && data.d.IsSuccess) {
                        $(ctrl.Container).html('');
                        $(ctrl.Container).append($('<i />').addClass('fa fa-2x fa-spinner fa-spin'));
                    }
                },
                error: function (jqXHR, exception) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        },
        _getStatus: function (callback) {
            var url = "Ajax.aspx/GetStatus";
            var country = this.options.country;
            var environment = this.options.environment;
            var category = this.options.category;
            var data = { environment: environment, country: country, category: category };
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    callback(data);
                },
                error: function (jqXHR, exception) {
                    console.log(JSON.stringify(jqXHR));
                    callback(false);
                }
            });
        }
    });
})(jQuery);
