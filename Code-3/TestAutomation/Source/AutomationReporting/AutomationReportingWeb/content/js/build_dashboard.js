(function ($, undefined) {
    $.widget('ui.BuildDashboard', {
        CurrentFilter: {},
        Chart: {},
        FilterElement: '#divFilter',
        DataElement: '#divGraph',
        ChartBy: '#graphfor',
        Loader: '#loader',
        Countries: [],
        Environments: [],
        charDrawn: false,
        options: {
            filters: [],
            tablecss:''
        },
        _create: function () {
            var ctrl = this;
            var ele = this.element;
            var filtersPH = $(ele).find(ctrl.FilterElement);                        
            $(ctrl.options.filters).each(function () { this.callback = ctrl._filter, this.parent = ctrl });
            $(filtersPH).ReportFilter({ filters: ctrl.options.filters });
            $(ctrl.ChartBy).change(function () {
                ctrl.refreshData();
            });
        },
        _filter: function (name, value, ctrl) {            
            name = name.substring(name.indexOf(' ') + 1);
            ctrl.CurrentFilter[name] = value;
            console.log(ctrl.CurrentFilter);
            ctrl.refreshData();

        },
        refreshData: function () {
            var ctrl = this;
            $('#' + ctrl.ChartElement).html('');
            $(ctrl.Loader).show();
            ctrl.getData();
        },
        getData: function () {
            var ctrl = this;
            var ele = this.element;
            $.ajax({
                type: "POST",
                url: "Ajax.aspx/GetBuildData?t=" + new Date().getDate(),
                data: JSON.stringify({
                    BuildInfo : ctrl.CurrentFilter.Build || '',
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log('graph data refreshed');
                    if (typeof data.d != 'undefined' && data.d != null && data.d.length > 0) {
                        console.log(data.d);
                        ctrl.Environments = ctrl.getEnvironments(data.d);
                        ctrl.Countries = ctrl.getCountries(data.d);
                        var categories = ctrl.CurrentFilter.Categories;
                        console.log(ctrl.Environments);
                        console.log(ctrl.Countries);
                        console.log(categories);
                        if(typeof categories != 'undefined' && categories.length > 0)
                        {
                            $(ctrl.DataElement).html('');
                            var mainContainer = $('<table />').addClass(ctrl.options.tablecss);
                            $(mainContainer).append($('<tr />')
                                .append($('<th rowspan="2" />').html('#').addClass('col-md-1 text-center  bg-primary')));
                            $(ctrl.Environments).each(function(){
                                    $(mainContainer).find('tr:first').append($('<th />').addClass('text-center bg-primary').attr('colspan', ctrl.CurrentFilter.Categories.length).html(this));                                
                            });
                            $(mainContainer).append($('<tr />'));
                            for(var i = 0 ; i < ctrl.Environments.length ; i++)
                            {
                                $(ctrl.CurrentFilter.Categories).each(function(){
                                    $(mainContainer).find('tr:last').append($('<th />').addClass('text-center  bg-primary').html(this.substring(0,2)));                                
                                });
                            }
                            $(ctrl.Countries).each(function(){
                                    var countryRow = $('<tr />').attr('id',this);
                                    $(mainContainer).append(countryRow);
                                    ctrl.createCountryData(data.d, this, countryRow);
                            }); 
                            $(ctrl.DataElement).append(mainContainer);
                            console.log(mainContainer);                                                     
                        }
                    }
                    else {

                    }
                    $(ctrl.Loader).hide();
                },
                error: function (jqXHR, exception) {
                    console.log(JSON.stringify(jqXHR));
                    $(ctrl.Loader).hide();
                }
            });
        },
        getEnvironments: function (data) {
            var ctrl = this;
            var environments = [];
            if (typeof data != 'undefined' && data.length > 0) {
                data.map(function (item, i) {
                    if(item['environment'] != '')
                    {
                        environments.push(item['environment']);
                    }
                });
                environments = $.unique(environments);
            }
            return environments;
        },
        getCountries: function(data){
            var ctrl = this;
            var countries = [];
            if (typeof data != 'undefined' && data.length > 0) {
                data.map(function (item, i) {
                    if(item['country'] != ''){
                        countries.push(item['country']);
                    }
                });
                countries = $.unique(countries);
            }
            return countries;
        },
        createCountryData: function(data, country, row) {
            var ctrl = this;
            $(row).append($('<td />').addClass('bg-primary').html(country));
            $(ctrl.Environments).each(function(){
                var env = this;
                $(ctrl.CurrentFilter.Categories).each(function(){ 
                    var cat = this;
                    var cdata = data.filter(function(item){                        
                        return item.country == country && item.environment == env && item.category == cat;
                    });
                    var value = (typeof cdata != 'undefined' && cdata.length > 0) ? cdata[0].percentage : 'N\A';
                    $(row).append($('<td />').addClass(ctrl.getColorClass(value)).html(value));
                });
            });
        },
        getColorClass: function(val){
            var cssName = '';
            console.log('css value: ' +val);
            if(parseInt(val) <= 50)
            {
                cssName= 'danger';
            }
            else{
                cssName= 'green';
            }
            return cssName;
        }
    });
})(jQuery);

Date.prototype.ddMMyyyy = function () {
    var mm = (this.getMonth() + 1) < 10 ? '0' + (this.getMonth() + 1) : this.getMonth() + 1;
    var dd = this.getDate() < 10 ? '0' + this.getDate() : this.getDate();
    return [dd, mm, this.getFullYear()].join('/'); // padding
};
