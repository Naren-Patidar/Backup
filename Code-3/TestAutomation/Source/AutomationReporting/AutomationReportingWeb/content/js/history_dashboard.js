(function ($, undefined) {
    $.widget('ui.HistoryDashboard', {
        Chart: {},
        FilterElement: '#divFilter',
        ChartElement: 'divGraph',
        ChartBy: '#graphfor',
        Loader: '#loader',
        IsLoading: false,
        ChartXKey: 'y',
        ChartYKeys: ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'],
        charDrawn: false,
        options: {
            filters: [],
            CurrentFilter: {},
            load: false
        },
        _create: function () {
            var ctrl = this;
            var ele = this.element;
            var filtersPH = $(ele).find(ctrl.FilterElement);
            console.log('filter placeholder: ' + filtersPH.length);
            console.log(ctrl.options.filters);
            $(ctrl.options.filters).each(function () { this.callback = ctrl._filter, this.parent = ctrl });
            $(filtersPH).ReportFilter({ filters: ctrl.options.filters });
            console.log('load: ' + ctrl.options.load);
            if (ctrl.options.load) {
                ctrl._plotChart();
            }
            $(ctrl.ChartBy).change(function () {
                ctrl._plotChart();
            });
        },
        _filter: function (name, value, ctrl) {
            if (ctrl.IsLoading)
            { alert('Please wait'); }
            else {
                console.log('filter changed : ' + name + ' value :' + value);
                name = name.substring(name.indexOf(' ') + 1);
                ctrl.options.CurrentFilter[name] = value;
                ctrl._plotChart();
            }
        },
        _plotChart: function () {
            var ctrl = this;
            ctrl.IsLoading = true;
            $('#' + ctrl.ChartElement).html('');
            $(ctrl.Loader).show();
            ctrl._getChartData();
        },
        _getChartData: function () {
            var ctrl = this;
            $.ajax({
                type: "POST",
                url: "History.aspx/GetGraphData?t=" + new Date().getDate(),
                data: JSON.stringify({
                    Environment: ctrl.options.CurrentFilter.Environment || '',
                    Category: ctrl.options.CurrentFilter.Category || '',
                    Country: ctrl.options.CurrentFilter.Country || '',
                    FromDate: ctrl.options.CurrentFilter.FromDate || ''
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log('graph data refreshed');
                    if (typeof data.d != 'undefined' && data.d != null && data.d.length > 0) {
                        console.log(data.d);
                        var graphData = ctrl.getGraphData(data.d);
                        var xKeys = ctrl.getxKeys(data.d);
                        var yKeys = ctrl.ChartXKey;
                        var labels = ctrl.getLabels(data.d);
                        console.log(graphData);
                        console.log(xKeys);
                        console.log(labels);
                        ctrl.Chart = Morris.Line({
                            element: ctrl.ChartElement,
                            xkey: 'y',
                            ykeys: xKeys,
                            labels: labels,
                            parseTime: false,
                            hideHover: 'auto',
                            continuousLine: true
                        });
                        ctrl.Chart.setData(graphData);
                    }
                    else {

                    }
                    $(ctrl.Loader).hide();
                    ctrl.IsLoading = false;
                },
                error: function (jqXHR, exception) {
                    console.log(JSON.stringify(jqXHR));
                    $(ctrl.Loader).hide();
                }
            });
        },
        getGraphData: function (data) {
            var ctrl = this;
            if (typeof data != 'undefined' && data.length > 0) {
                // sort data in ascending order of created date
                data.sort(function (a, b) {
                    return new Date(a.createtime) - new Date(b.createtime);
                })
                var labels = ctrl.getLabels(data);
                var xKeys = ctrl.getxKeys(data);
                var graphData = [];
                var dates = [];
                data.map(function (item, i) {
                    dates.push(new Date(item['createtime']).ddMMyyyy());
                });
                dates = $.unique(dates);

                var graphBy = $(ctrl.ChartBy).val();
                dates.map(function (d) {
                    var gdata = {};
                    gdata['y'] = d;
                    xKeys.map(function (x, i) {
                        var filteredData = data.filter(function (dataItem, j) {
                            return new Date(dataItem['createtime']).ddMMyyyy() == d && dataItem[graphBy.toLowerCase()] == labels[i];
                        });
                        if (typeof filteredData != 'undefined' && filteredData.length > 0) {
                            gdata[x] = filteredData[0].percentage;
                        }
                    });
                    graphData.push(gdata);
                });
                console.log('data :' + JSON.stringify(graphData));
            }
            else {
                $(ctrl.Loader).hide();
            }
            return graphData;
        },
        getxKeys: function (data) {
            var ctrl = this;
            var xKeys = [];
            var graphBy = $(ctrl.ChartBy).val();
            if (typeof data != 'undefined' && data.length > 0) {
                data.map(function (item, i) {
                    if (item[graphBy.toLowerCase()] != '') {
                        xKeys.push(item[graphBy.toLowerCase()]);
                    }
                });
                var count = $.unique(xKeys).length;
                xKeys = ctrl.ChartYKeys.slice(0, count);
                console.log('xKeys: ' + JSON.stringify(xKeys));
            }
            return xKeys;
        },
        getLabels: function (data) {
            var ctrl = this;
            var labels = [];
            var graphBy = $(ctrl.ChartBy).val();
            if (typeof data != 'undefined' && data.length > 0) {
                data.map(function (item, i) {
                    if (item[graphBy.toLowerCase()] != '') {
                        labels.push(item[graphBy.toLowerCase()]);
                    }
                });
                labels = $.unique(labels);
            }
            console.log('lables: ' + JSON.stringify(labels));
            return labels;
        },
        _reset: function () {
            this.CurrentFilter = {};
            this._plotChart();
        }
    });
})(jQuery);

Date.prototype.ddMMyyyy = function () {
    var mm = (this.getMonth() + 1) < 10 ? '0' + (this.getMonth() + 1): this.getMonth() + 1;
    var dd = this.getDate() < 10 ? '0' + this.getDate() : this.getDate();
    return [dd, mm, this.getFullYear() ].join('/'); // padding
};

Date.prototype.MMddyyyy = function () {
    var mm = (this.getMonth() + 1) < 10 ? '0' + (this.getMonth() + 1) : this.getMonth() + 1;
    var dd = this.getDate() < 10 ? '0' + this.getDate() : this.getDate();
    return [mm, dd, this.getFullYear()].join('/'); // padding
};
