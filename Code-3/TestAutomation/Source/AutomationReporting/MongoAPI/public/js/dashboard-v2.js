(function ($, undefined) {
    $.widget('ui.dashboard', { 
        ChartLabels : ['Passed', 'Inconclusive', 'Failed'],
        CharxKey : 'y',
        ChartyKeys : ['Passed', 'Inconclusive', 'Failed'],
        Chart: {},
        Headers: [],
        Environments:[],
        Countries:[],
        Tiles: [],
        options: {
            tablecss:'table table-striped table-bordered table-hover text-center',
            notify: {}
        },
        _create: function () {
            var ctrl = this;
            ctrl._getInitialData(function(){
                ctrl._bindSelect($('#' + ctrl.options.environmentCtrl), ctrl.Environments);
                ctrl._createHeadings();
            });            
            $('#' + ctrl.options.environmentCtrl).on('change', function () {                
                ctrl._collapseAll();
            });
        },
        _bindSelect: function(selectCtrl, data)
        {
            data.map(function(item,i){
                selectCtrl.append($('<option />').text(item));
            });
        },
        _collapseAll: function () {
            var ele = this.element;
            $(ele).find('.country').each(function(){
                if($(this).find('.panel-body').hasClass('in'))
                {
                    $(this).find('.panel-heading').click();
                }
            });            
        },
        _getInitialData: function(callback){
            var ctrl = this;
            ctrl._getEnvironments(function(environments, hasEnv){
                if(hasEnv)
                {
                    ctrl.Environments = environments;
                    ctrl._getCountries(function(countries, hasCo){
                        if(hasCo)
                        {
                            ctrl.Countries = countries;
                            callback();
                        }                        
                    });    
                }                
            });
        },
        _getEnvironments: function (callback) {
            var ctrl = this;
            var url = "../distinct?q=Environment&t=" + new Date().getTime();
            var instance = this;            
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    callback(data, true);                    
                },
                error: function (jqXHR, exception) {
                    callback(exception, false);
                }
            });
        },
        _getCountries: function (callback) {
            var ctrl = this;
            var url = "../distinct?q=Country&t=" + new Date().getTime();
            var instance = this;            
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    callback(data, true);
                },
                error: function (jqXHR, exception) {
                    callback(exception, false);
                }
            });
        },
        _createHeadings: function(){
            var ctrl = this;
            var ele = this.element;
            ctrl.Countries.map(function(country, i){
                var container = $("<div />").addClass("panel panel-success country").attr("name", name);
                var heading = $("<div />").addClass("panel-heading panel-collapse job-panel").css("cursor", "pointer").attr('data-toggle','collapse').attr('data-target','#pnl'+ country);
                var bodycontainer = $("<div />").addClass('panel-body collapse').attr('id','pnl' + country );
                var body = $("<div />").addClass('panel-body');
                var chartcontainer = $("<div />").addClass('col-sm-4').attr('id', 'chart' + country);
                var detailcontainer = $("<div />").addClass('col-sm-8').attr('id', 'detail' + country);
                $(heading).text(country);
                $(container).append(heading);
                $(body).append(chartcontainer);
                $(body).append(detailcontainer);
                $(bodycontainer).append(body);
                $(container).append(bodycontainer);
                $(heading).click(function () {
                    ctrl._getTestResult(country, body);
                });
                $(ele).append(container);
            });            
        },
        _getTestResult: function (country, chart_container) {
            var ctrl = this;
            var environment = $('.ddlenvironment').val();
            var url = "../where/dashboarddata?Country=" + country + "&Environment=" + environment;
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {                    
                    ctrl._createChartElement(chart_container, country, data, function(data, id){
                        ctrl._createStackChart(data, id);
                        ctrl._createSummary(data, id);
                    });
                },
                error: function (jqXHR, exception) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        },
        _createChartElement: function(body, country, data, callback){
            var ctrl = this;
            var id = country + '_stackChart';
            var ele = $('<div />').attr('id', id);
            $(body).find('#' + id).remove();
            $(body).find('#chart' + country).append(ele);
            callback(data, country);
        },
        _createStackChart: function(data, country){
            var ctrl = this;
            var element = country + '_stackChart';
            if (typeof data != 'undefined' && data != null && data.length > 0) {                
                var graphData = ctrl.getGraphData(data);
                ctrl.Chart = Morris.Bar({
                    element: element,
                    xkey: ctrl.CharxKey,
                    ykeys: ctrl.ChartyKeys,
                    labels: ctrl.ChartLabels,            
                    stacked:true,
                    barColors: ['green', 'lightgreen', 'red']
                });
                ctrl.Chart.setData(graphData);
            }
            else {
                console.log('no data for chart');
            }            
        },
        _createSummary: function(data, country){
            var ctrl = this;
            var container = '#detail' + country;
            var chartContainer = '#chart' + country;
            $(container).css('overflow-y', 'auto');
            $(container).css('height', $(chartContainer).height());
            $(container).html('');
            if (typeof data != 'undefined' && data != null && data.length > 0) {                
                var mainContainer = $('<table />').addClass(ctrl.options.tablecss).addClass('small');

                $(mainContainer).append($('<thead />').append($('<tr />')));
                var columns = ['Category', 'Browser', 'Build', 'Execution Date' ,'Duration', 'Percentage' , 'Execute'];
                $(columns).each(function(){
                    $(mainContainer).find('thead>tr')
                    .append($('<th />')
                    .text(this));
                });
                $(mainContainer).append($('<tbody />'));
                console.log(data);
                $(data).map(function(i, item){
                    var row = ctrl.createSummaryRow(item);
                    $(mainContainer).find('tbody').append(row);
                });
                
                $(mainContainer).dataTable();
                $(container).append(mainContainer);
            }
            else {
                console.log('no data for summary');
            }            
        },
        createSummaryRow: function(item){
            var ctrl = this;
            var row = $('<tr />');
            var environment = 'GD';
            var percentage = parseInt(((item.passed + item.inconclusive) / item.total)*100);
            var divExe = $('<div />').addClass('panel-heading');
            var tdExe = $('<td />').append(divExe);
            $(divExe).RunButton({
                    country: item.country,
                    category: item.category,
                    environment: environment,
                    notify: ctrl.options.notify                   
                });            
            row.append($('<td />').text(item.category));
            row.append($('<td />').text(item.browser));
            row.append($('<td />').text(item.buildNumber + '|' + item.version));
            row.append($('<td />').text(item.createtime));
            row.append($('<td />').text(ctrl._getDisplayDuration(item.duration)));
            row.append($('<td />').text(percentage));
            $(row).Report({
                target: '#reportModel',
                country: item.country,
                category: item.category,
                environment: environment,
                date: new Date()
            });
            row.append(tdExe);
            
            return row;
        },
        _getDisplayDuration: function(Sec)
        {
            var time = '';
            var seconds = parseInt(Sec);
            if(seconds < 60)
            {
                time = seconds + ' sec'
            }
            else if(seconds < 3600)
            {
                time = parseInt(seconds / 60) + ' mins ' + parseInt(seconds % 60) + ' sec';
            }
            else
            {
                var hours = parseInt( seconds / 3600);
                var remaining = seconds % 3600;
                time = hours + ' Hrs ' + parseInt(remaining / 60) + ' Mins ' + parseInt(remaining % 60) + ' sec';
            }
            return time;
        },
        getGraphData: function (data) {
            var ctrl = this;
            if (typeof data != 'undefined' && data.length > 0) {    
                var graphData = [];
                var dates = [];
                data.map(function (item, i) {
                    graphData.push(
                    {
                        y : item.category,
                        Passed: item.passed,                        
                        Inconclusive: item.inconclusive,
                        Failed: item.failed,
                    }
                    );
                });
                console.log('data :' + JSON.stringify(graphData));
            }
            else {
                $(ctrl.Loader).hide();
            }
            return graphData;
        }
    });
})(jQuery);
