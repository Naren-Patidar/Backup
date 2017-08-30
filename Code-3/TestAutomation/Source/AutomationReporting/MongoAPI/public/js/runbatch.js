(function ($, undefined) {
    $.widget('ui.RunButton', {
        Container: {},
        options: {
            country: '',
            category: '',
            environment: '',
            browser: '',
            notify: {}
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
                if (data == false) {
                    ctrl.Container.append($('<i />').addClass('fa fa-2x fa-frown-o red').css('title', 'error'));
                }
                else {
                    if ((data && data.status == 0) || data == null) {
                        ctrl.Container.append($('<i />').addClass('fa fa-2x fa-youtube-play').css('cursor', 'pointer'));
                        $(ctrl.Container).click(function () { ctrl._run(); });
                    }
                    else {

                        ctrl.Container.append($('<i />').addClass('fa fa-2x fa-spinner fa-spin')).attr('title', 'since :' + data.lastStartTime);
                    }
                }
            });
            $(heading).append(ctrl.Container);

        },
        _run: function () {
            var ctrl = this;
            var environment = this.options.environment;
            var country = this.options.country;
            var category = this.options.category;
            var url = '../firetest/batchtrigger/' + environment + '/' + country + '/' + category.substring(0,2);
            console.log('Run Button Clicked for ' + environment + ', ' + country + ', ' + category);
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data && data.IsSuccess) {
                        ctrl.options.notify(environment, country, category, 1);
                        ctrl._updateStatus(1);
                    }
                },
                error: function (jqXHR, exception) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        },
        _updateStatus: function(status){
            var ctrl = this;
            $(ctrl.Container).html('');
            if(status == 1)
            {
                ctrl.Container.append($('<i />').addClass('fa fa-2x fa-spinner fa-spin'));
            }
            else
            {
                ctrl.Container.append($('<i />').addClass('fa fa-2x fa-youtube-play').css('cursor', 'pointer'));
            }
        },
        _getStatus: function (callback) {
            var url = '../firetest/getbatchstatus/' + this.options.environment + '/' + this.options.country + '/' + this.options.category;
            console.log(url);
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if(data=='')
                    {
                        data = {status: 0};
                    }
                    callback(data);
                },
                error: function (jqXHR, exception) {
                    console.log('error' + JSON.stringify(jqXHR) + JSON.stringify(exception));
                    callback({status: 0});
                }
            });
        }
    });
})(jQuery);
