 $('[data-modalSpecs]').each(function (i) {

            var $this = $(this), $specsString = $this.attr('data-modalSpecs');

            function QueryStringToJSON() {

                var pairs = $specsString.split(',');

                var result = {};

                pairs.forEach(function (pair) {
                    pair = pair.split('=');
                    result[pair[0]] = decodeURIComponent(pair[1] || '');
                });

                return JSON.parse(JSON.stringify(result));

            }

            var $specsObject = QueryStringToJSON();

            if ($specsObject.method === 'static') {

                $this.hide();

                //console.log('static');

                if ($specsObject.trigger === 'load') {

                    setTimeout(function () {

                        var $theClone = $this.clone();

                        $('#modal-content').html($theClone);

                        $('[data-modalMethod]', '#modal-content').show();

                        $('#modal-wrapper').addClass('active');

                        $(window).keydown(function (e) {

                            if (e.which == 27) {

                                $('#close-modal').click();

                            }
                        });


                    });

                } else {

                    $('#' + $specsObject.trigger).on('click', function (e) {

                        var $theClone = $this.clone(true);

                        e.preventDefault();

                        $('#modal-content').html($theClone);

                        $('[data-modalMethod]', '#modal-content').show();

                        $('#modal-wrapper').addClass('active');

                    });

                }

            } else if ($specsObject.method === 'remote') {

                //Deal with remote

            }


        });

        $('#modal-overlay, #close-modal, .close-modal').on('click', function (e) {

            e.preventDefault();
            console.log(e.type);

            $('#modal-wrapper').removeClass('loading').removeClass('active');
            $('#modal-content').html("");

        });
        $('#modal-content').on('click', function (e) {

            e.stopPropagation();

        });