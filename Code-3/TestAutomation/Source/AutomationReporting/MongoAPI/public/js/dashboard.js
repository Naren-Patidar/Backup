(function ($, undefined) {
    $.widget('ui.dashboard', {  
        Headers: [],
        Tiles: [],
        pieOptions: {
            series: {
                pie: {
                    show: true
                }
            },
            grid: {
                hoverable: true,
                clickable: true 
            },
            label: {
                show: false,                
            },
            legend: {
                show: false
            },
            threshold : 0.1
        },
        options: {
            environmentCtrl: '',
            showExpanding: true,
            notify: {}
        },
        _create: function () {
            var ctrl = this;
            ctrl._getEnvironments();            
            $('#' + ctrl.options.environmentCtrl).on('change', function () {                
                ctrl._reset();
            });
        },
        _reset: function () {
            var ele = this.element;
            $(ele).find('.country').each(function(){
                if($(this).find('.panel-body').hasClass('in'))
                {
                    $(this).find('.panel-heading').click();
                }
            });            
        },
        _getEnvironments: function () {
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
                    var countries = data;
                    data.map(function(item,i){
                        $('#ddlEnvironments').append($('<option />').text(item));
                    });
                    instance._getCountries();
                },
                error: function (jqXHR, exception) {
                }
            });
        },
        _getCountries: function () {
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
                    var countries = data;
                    instance._getCategories(countries);
                },
                error: function (jqXHR, exception) {
                }
            });
        },
        _getCategories: function (countries) {
            var ctrl = this;
            var url = "../distinct?q=Category&t=" + new Date().getTime();
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {                    
                    var categories = data;
                    if (countries.length > 0) {
                        ctrl._createUI(countries, categories);
                    }
                },
                error: function (jqXHR, exception) {
                }
            });
        },
        _getTestResult: function (category, country, tile_container) {
            var ctrl = this;
            var environment = $('.ddlenvironment').val();
            var url = "../where/latest/piedata?Country=" + country + "&Category=" + category + "&Environment=" + environment;
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {                    
                    ctrl._createPie(country, category, environment, data, tile_container);
                },
                error: function (jqXHR, exception) {
                    alert(JSON.stringify(jqXHR));
                }
            });
        },
        _createUI: function (countries, categories) {
            var ctrl = this;
            var environment = $('#' + ctrl.options.environmentCtrl).val();
            $(countries).each(function () {
                var heading = ctrl._createHeading(this, categories);
                ctrl.Headers.push({ country: this, environment: environment, ctrl: heading });
            });
            if(ctrl.options.showExpanding)
            {
                $(ctrl.Headers[0].ctrl).trigger('click');
            }
        },
        _createHeading: function (country, categories) {
            var ctrl = this;
            var ele = this.element;
            var container = $("<div />").addClass("panel panel-success country").attr("name", name);
            var heading = $("<div />").addClass("panel-heading panel-collapse job-panel").css("cursor", "pointer").attr('data-toggle','collapse').attr('data-target','#pnl'+ country);
            var bodycontainer = $("<div />").addClass('panel-body collapse').attr('id','pnl' + country );
            var body = $("<div />").addClass('panel-body');
            $(heading).text(country);
            $(container).append(heading);
            $(bodycontainer).append(body);
            $(container).append(bodycontainer);
            $(heading).click(function () {
                ctrl._createTiles(categories, country, body);
            });
            $(ele).append(container);
            return heading;
        },
        _createTiles: function (categories, country, body) {
            var ctrl = this;
            var ele = this.element;
            var environment = $('#' + ctrl.options.environmentCtrl).val();
            console.log('_createTiles started');
            $(body).html('');
            // check if control already have tiles
            //console.log(ctrl.Tiles);
            console.log('before filter ' + ctrl.Tiles.length);
            ctrl.Tiles = $(ctrl.Tiles).filter(function(item){return (item.country !== country && item.environment !== environment) });
            console.log('after filter ' + ctrl.Tiles.length);
            
            $(categories).each(function () {
                var container = $("<div />").addClass("panel panel-success");
                var tile_body = $("<div />").addClass("panel-body").attr('id', 'tile_body_' + environment + '_' + + country + '_' + this);
                var tile_heading = $("<div />").addClass("panel-heading");
                var tile_footer = $("<div />").addClass("panel-footer");
                var col = $("<div />").addClass("col-lg-3");

                var cat = this;
                $(tile_heading).append($('<sapn />').text(cat));
                $(tile_footer).append($('<div />').addClass('row').append($('<label />').text(' Total: ').css('margin-left','2px')).append($('<sapn />').addClass('total')));
                $(tile_footer).append($('<div />').addClass('row').append($('<label />').text(' Time: ').css('margin-left','2px')).append($('<sapn />').addClass('time')));
                $(tile_footer).append($('<div />').addClass('row').append($('<label />').text(' Server: ').css('margin-left','2px')).append($('<sapn />').addClass('server')));
                $(tile_footer).append($('<div />').addClass('row').append($('<label />').text(' Browser: ').css('margin-left','2px')).append($('<sapn />').addClass('browser')));
                $(tile_footer).append($('<div />').addClass('row').append($('<label />').text(' Core Build: ').css('margin-left','2px')).append($('<sapn />').addClass('build')));

                $(col).append($(container).append(tile_heading).append(tile_body).append(tile_footer));
                ctrl.Tiles.push({environment: environment, category: this, country: country, container: container});
                ctrl._getTestResult(cat, country, tile_body);
                $(body).append(col);
            });            
            console.log('_createTiles Ends');
        },
        _createPie: function (country, category, environment, data, tile_container) {
            var ctrl = this;
            console.log(country);
            console.log(category);
            console.log(environment);
            console.log(data);
            var ele = this.element;
            if (data) {
                var dataSet = [
                    { label: "Passed", data: data.passed, color: "#31a05a" },
                    { label: "Inconclusive", data: data.inconclusive, color: "#40dd7a" },
                    { label: "Failed", data: data.failed, color: "#FF0000" }
                ];
                var wid = $(tile_container).RunButton({
                    country: country,
                    category: category,
                    environment: environment,
                    notify: ctrl.options.notify                   
                });
                var container = $("<div />").addClass("flot-placeholder col-lg-12")
                .attr('id', country + '_' + category + '_' + environment + '_pie')
                .bind("plotclick", function (event, pos, item) {
                    var index = parseInt(item['seriesIndex']);
                    console.log('clicked at : ' + dataSet[index].label + ' plot of ' + country + ', ' + category +', ' + environment);                    
                    $(this).attr('data-toggle', 'modal').attr('data-target', '#myModal');
                    
                    var plot = this;
                    $("#dashboard").detail({ country : country, environment :environment, category : category, result : dataSet[index].label , plot : plot, time : new Date().getTime() });                
                    $("#dashboard").detail("destroy");
                    return false;
                });
                $(tile_container).append(container);                
                $.plot($(container), dataSet, ctrl.pieOptions);
                $(tile_container).parent().find('.total').text('  ' + data.total);
                var date_time = 'NA';
                if(typeof data.createtime != 'undefined')
                {
                    date_time = data.createtime.substring(0, 10) + ' ' + data.createtime.substring(11, 16);
                }
                $(tile_container).parent().find('.time').text('  ' + date_time);
                var server = 'NA';
                if(typeof data.createtime != 'undefined')
                {
                    server = data.server.substring(7);
                }
                $(tile_container).parent().find('.server').text('  ' + server);
                $(tile_container).parent().find('.browser').text('  ' + data.browser);
                $(tile_container).parent().find('.build').text('  ' + data.buildNumber + ' | ' + data.version);
                $(tile_container).parent().find('.build').attr('title', data.commitHistory);
            }
            else {
                $(tile_container).parent().remove();
            }
        }
    });
})(jQuery);
