/*
filters: [{ lable: '', tag: '', type: '', datatype: '', className: '' , dataurl: ''  , data: {} , callback: ''}]
lable : text of the lable
tag : html tag name
type : html attribute type
datatype: data type of user input (default string)
className : css class
dataurl : rest url for getting data
data : datasource to bind to control
*/

(function ($, undefined) {
    $.widget('ui.ReportFilter', {
        options: {
            filters: []
        },
        _create: function () {
            var ctrl = this;
            var ele = this.element;
            ctrl._filter_init(0);
        },
        _filter_init: function (index) {
            var ctrl = this;
            var ele = this.element;
            var filter = ctrl.options.filters[index];
            if (filter.data) {
                console.log('provided data for - ' + index);
                ctrl._createfilter(index, filter.data);
                index = index + 1;
                if (index < ctrl.options.filters.length) {
                    ctrl._filter_init(index);
                }
            }
            else {
                console.log('getting data for - ' + index);
                $.ajax({
                    type: "POST",
                    url: filter.url,
                    data: [],
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);
                        ctrl._createfilter(index, data.d);
                        index = index + 1;
                        if (index < ctrl.options.filters.length) {
                            ctrl._filter_init(index);
                        }
                    },
                    error: function (jqXHR, exception) {
                        console.log(JSON.stringify(jqXHR));
                    }
                });
            }
        },
        _createfilter: function (index, data) {
            console.log('creating filter..' + index);
            var ctrl = this;
            var ele = this.element;
            var filter = ctrl.options.filters[index];
            console.log(filter);
            var container = $('<div />').addClass('form-group');
            var lblCtr = $('<label />').attr('for', filter.lable).text(filter.lable);
            var dataCtr = $('<' + filter.tag + '/>').addClass(filter.className);
            
            $(filter.attributes).each(function () {
                $(dataCtr).attr(this.name, this.value);
            });
            switch (filter.tag) {
                case 'select':
                    console.log('data length ' + data.length);
                    $(dataCtr).append($('<option />').text('<<- ' + filter.lable + ' ->>').val(''));
                    data.map(function (item, i) {
                        $(dataCtr).append($('<option />').text(item));
                    });
                    if (filter.selectedIndex == 'last') {
                        $(dataCtr).val($(dataCtr).find('option:last').val());
                        filter.callback(filter.lable, $(dataCtr).val(), filter.parent);
                    }
                    else if (filter.selectedIndex && filter.selectedIndex.indexOf('-') > 0) {
                        var parts = filter.selectedIndex.split('-');
                        for (var i = parseInt(parts[0]); i < parseInt(parts[1]); i++) {
                            var option = $(dataCtr).find('option').get(i);
                            option.selected = true;
                        }
                        filter.callback(filter.lable, $(dataCtr).val(), filter.parent);
                    }
                    else if (filter.selectedIndex) {
                        var index = parseInt(filter.selectedIndex);
                        var option = $(dataCtr).find('option').get(index);
                        option.selected = true;
                        filter.callback(filter.lable, $(dataCtr).val(), filter.parent);
                    }
                    if (filter.defaultValue) {
                        $(dataCtr).val(filter.defaultValue);
                    }
                    $(dataCtr).on('change', function () {
                        filter.callback(filter.lable, $(dataCtr).val(), filter.parent);
                    });
                    break;
                case 'input':
                    $(dataCtr).attr('type', filter.type);
                    if (filter.defaultValue) {
                        $(dataCtr).val(filter.defaultValue);
                    }
                    if (filter.datatype == 'date') {
                        $(dataCtr).datepicker({
                            autoclose: true
                        }).on('changeDate', function (ev) {
                            $(dataCtr).change();
                        });
                    }
                    $(dataCtr).on('change', function () {
                        filter.callback(filter.lable, $(dataCtr).val(), filter.parent);
                    });
                    break;
            }

            $(container).append(lblCtr);
            $(container).append(dataCtr);
            $(ele).append(container);
        }
    });
})(jQuery);
