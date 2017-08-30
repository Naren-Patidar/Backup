(function ($, undefined) {
    $.widget('ui.Report', {
        options: {
            target:'',
            date: '',
            country: '',
            environment: ''        
        },
        _create: function () {
            var ctrl = this;
            var element = this.element;

            var ele = $(element).find('td:first');
            console.log(ele.length);
            $(ele).css('cursor','pointer');
            $(ele).attr('title','Click to view details');
            $(ele).attr('data-toggle','modal');
            $(ele).attr('data-target',ctrl.options.target);
            $(ele).click(function(){
                console.log(ctrl.options);
                ctrl._getDetails();
            });            
        },
        dataTable: {},      
        table: {},
        table_head: {},
        table_body: $('<tbody />'),
        _create_modal_ui: function (data) {
            var ctrl = this;
            var ele = this.element;
            var op = this.options;
            var body = $('#history-body');
            $('#history-header').css('background-color', '#0480be').css('color', '#fff');
            $('#history-title').text(op.category + ' | ' + op.country);
            $(body).html('');
            
            ctrl.table = $('<table />').addClass('table');
            ctrl.table_head = $('<thead />').append($('<tr />')
                            .append($('<th />').text('#'))
                            .append($('<th />').text('Test Name'))
                            .append($('<th />').text('Description'))
                            .append($('<th />').text('Categories'))
                            .append($('<th />').text('Message'))
                            .append($('<th />').text('Result'))
                            );
            $(body).append(ctrl.table);
            $(ctrl.table).append(ctrl.table_head);
            $(ctrl.table).append(ctrl.table_body);
            var id = 1;
            console.log('adding rows');
            $(ctrl.table_body).find('tr').each(function(){ $(this).remove();});
            $(data).each(function () {
                $(ctrl.table_body).append($('<tr/>')
                    .append($('<td />').text(id++))
                    .append($('<td />').text(this.TestName))
                    .append($('<td />').text(this.Description))
                    .append($('<td />').text(this.Categories))
                    .append($('<td />').text(this.Message))
                    .append($('<td />').text(':'+this.Result))                
                );
            });
            $(body).addClass('table-responsive');
            
            ctrl.dataTable = $(ctrl.table).dataTable({"pageLength": 4,
                "lengthMenu": [[1, 2, 3, 4, 5, 10, 25, 50, -1], [1, 2, 3, 4, 5, 10, 25, 50, "All"]],
                "columns": [
                    { "width": "2%" },
                    { "width": "10%" },
                    { "width": "40%" },
                    { "width": "10%" },
                    { "width": "35%" },
                    { "width": "3%" }
                  ]
            });            
        },
        _getDetails: function () {
            var ctrl = this;
            var ele = this.element;
            var o = this.options;
            var url = "../report?environment=" + o.environment + "&country=" + o.country + "&category=" + o.category + "&y=" + o.date.getFullYear()
            + "&m=" + o.date.getMonth() + "&d=" + o.date.getDate();
            $.ajax({
                type: "GET",
                url: url,
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    ctrl._create_modal_ui(data.results);                    
                    ctrl._destroy();
                },
                error: function (jqXHR, exception) {
                }
            });
        },
        _destroy: function () { },
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
        }

    });
})(jQuery);
