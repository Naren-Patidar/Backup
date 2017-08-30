//jshint devel:true

var DDL = DDL || {};

$(document).ready(function () {
    'use strict';

    //Prevent default iOS scroll apart from elements with class .scrollable  -------------------

    $(document).bind('touchmove', function (e) {

        if (!$('.scrollable').has($(e.target)).length) {

            e.preventDefault();
        }

    });

    //Detect Display  -------------------

    DDL.detectDisplay = (function () {
        //console.log('detectDisplay');

        var init = function () {

            //Add a class to the wrapper based on screen width
            if (Modernizr.mq('only screen and (max-width: 504px)')) {

                $('body').removeClass('smallest').removeClass('small').removeClass('medium').removeClass('large').removeClass('massive').addClass('base');

            } else if (Modernizr.mq('only screen and (min-width: 505px)') && Modernizr.mq('only screen and (max-width: 755px)')) {

                $('body').removeClass('base').removeClass('small').removeClass('medium').removeClass('large').removeClass('massive').addClass('smallest');

            } else if (Modernizr.mq('only screen and (min-width: 756px)') && Modernizr.mq('only screen and (max-width: 1007px)')) {

                $('body').removeClass('base').removeClass('smallest').removeClass('medium').removeClass('large').removeClass('massive').addClass('small');

            } else if (Modernizr.mq('only screen and (min-width: 1008px)') && Modernizr.mq('only screen and (max-width: 1259px)')) {

                $('body').removeClass('base').removeClass('smallest').removeClass('small').removeClass('large').removeClass('massive').addClass('medium');

            } else if (Modernizr.mq('only screen and (min-width: 1260px)') && Modernizr.mq('only screen and (max-width: 1511px)')) {

                $('body').removeClass('base').removeClass('smallest').removeClass('small').removeClass('medium').removeClass('massive').addClass('large');

            } else if (Modernizr.mq('only screen and (min-width: 1512px)')) {

                $('body').removeClass('base').removeClass('smallest').removeClass('small').removeClass('medium').removeClass('large').addClass('massive');

            }
        };

        init();

        $(window).on('resize', function () {

            init();

        });

        return {
            init: init
        };

    } ());


    //Primary utility  -------------------

    DDL.primaryUtility = (function () {
        //console.log('primaryUtility');

        var lowerViewportSettings = {
            viewport: 'lower',
            buttonVisible: true
        },

        higherViewportSettings = {
            viewport: 'higher',
            buttonVisible: false
        },

        settings, $SN1 = $('#secondary-navigation').clone(), $SN2 = $('#secondary-navigation').clone();

        $SN2.find('.accordion').removeClass('accordion');

        if ($('body').hasClass('base') || $('body').hasClass('small') || $('body').hasClass('smallest')) {//Depending on the viewport

            settings = lowerViewportSettings; //Make the settings lower viewport

        } else {

            settings = higherViewportSettings; //Make the settings higher viewport

        }

        var buildFlyout = (function () {
            //console.log('buildFlyout');

            function addControl() {
                //console.log('addControl');

                var $navControl = '<a href="#n" id="ddl-flyout-control" class="ddl-button ddl-icon-before ddl-icon-menu ddl-icon-only">Show / Hide navigation</a>'; //Make the HTML for the burger button

                $('#ddl-page-header').append($navControl); //Put the button in the header

                $('#ddl-flyout-control').on('click', function (e) {//Attach the click event to the button

                    e.preventDefault(); //Stop the default link action
                    e.stopPropagation(); //Stop the click from bubbling up

                    if ($('body').hasClass('ddl-flyout-active')) {//If the Navigation is already open

                        hideFlyout(); //Close it

                    } else {

                        showFlyout(); //Otherwise open it

                    }
                });
            }

            function removeControl() {
                //console.log('removeControl');

                $('#ddl-flyout-control').remove();
            }

            function arrangePN(viewport) {
                //console.log('arrangePN');

                var $GN = $('#global-nav').clone(true);

                $('#global-nav').remove();

                if (viewport === 'lower') {

                    $('#primary-navigation').after($GN);

                } else {

                    $('#ddl-global-header .container').prepend($GN);

                }
            }

            function arrangeSN(viewport) {
                //console.log('arrangeSN');

                $('#secondary-navigation').remove();

                if (viewport === 'lower') {

                    $('#secondary-navigation-wrapper-flyout').append($SN2);

                    var pageName = location.href.split('/').slice(-2)[0];
                    var pageType = getPageType(pageName);

                    setTimeout(function () {
                        if ($('#tertiary-utility').length) {

                            $('#default-open-for-clubcard').click();
                            if (pageType == 'grandChild') {

                                $('#accountManagementLink').click();
                            }

                        }
                    }, 0);


                    //setTimeout(function(){

                    //$('#default-open-for-clubcard').click();

                    //}, 500);

                } else {

                    $('#secondary-navigation-wrapper-page').append($SN1);

                }

            }

            function getPageType(pageName) {
                switch (pageName.toLowerCase()) {
                    case 'accountmanagement':
                        return 'grandChild';
                        break;
                    default:
                        return 'child';
                }

            }

            var showFlyout = function () {
                //console.log('showFlyout');

                $('html').addClass('ddl-flyout-active');

            };

            var hideFlyout = function () {
                //console.log('hideFlyout');

                $('html').removeClass('ddl-flyout-active');

            };

            var init = function () {
                //console.log('buildNavigation.init');

                if (settings.viewport === 'lower') {
                    //console.log('lower');

                    removeControl();
                    addControl();

                    arrangePN(settings.viewport);
                    arrangeSN(settings.viewport);

                } else {
                    //console.log('higher');

                    removeControl();

                    arrangePN(settings.viewport);
                    arrangeSN(settings.viewport);

                }

                $('#ddl-flyout, #ddl-wrapper').on('click', function (e) {

                    var $this = $(this);

                    if ($('html').hasClass('ddl-flyout-active')) {

                        hideFlyout();
                        $('#primary-navigation').find('.active').not('#tertiary-utility a').removeClass('active');

                    }

                });

                $('#primary-navigation, #ddl-global-header, #login, #global-nav').on('click', function (e) {

                    e.stopPropagation();

                });

                //Correct hover state on the generated Close button in the primary nav (Save us from having to put non semantic 'Close' in the primary nav)   -------------------

                $('#primary-navigation > ul > li > nav').on("mouseenter", function (e) {

                    $(this).parent().addClass('noCloseHover');

                });

                $('#primary-navigation > ul > li > nav').on("mouseleave", function (e) {

                    $(this).parent().removeClass('noCloseHover');

                });

            }

            return {
                showFlyout: showFlyout,
                hideFlyout: hideFlyout,
                init: init
            };

        } ());

        var traverseNav = (function () {

            var traverseUp = function ($li) {
                //console.log('traverseUp')

                $li.removeClass('back')

                $li.siblings('ul').removeClass('left');

                $li.siblings('ul').find('.active').not('#tertiary-utility a').removeClass('active');

                if (settings.viewport === 'lower') {

                    $li.siblings('ul').find('li:hidden').not('.ad').css('display', 'block');

                } else {

                    $li.siblings('ul').find('li:hidden').not('.ad').css('display', 'inline-block');

                }

                //$li.siblings('ul').find('nav').css('height', 'auto');

                $li.siblings('ul').find('ul').css('height', 'auto');

                $li.siblings('ul').css('height', 'auto');

            };

            var traverseDown = function ($li) {
                //console.log('traverseDown');

                var $count = 50, $height;

                $li.parent().siblings('.nav-header').addClass('back');

                $li.children('nav').children('ul').children('li:visible').each(function () {

                    var $this = $(this);

                    $count = $count + 50;

                });

                $li.parent('ul').css('height', $count + 'px');

                $('nav', $li).css('height', $count + 'px');

                setTimeout(function () {

                    $li.parent('ul').addClass('left');

                }, 200);

                setTimeout(function () {

                    $li.siblings('li:visible').hide();

                }, 600);
            };

            var goToLocation = function (el) {
                //  console.log('gotoLocation');

                var $location = el.attr('href');
                window.location = $location;
            };

            var init = function () {
                //console.log('traverseNav.init');

                $('#primary-navigation a').off('click');

                $('#primary-navigation a').on('click', function (e) {

                    e.preventDefault();
                    e.stopPropagation();

                    var $this = $(this), $parent = $this.parent();

                    if ($parent.children('nav').length > 0 && !$parent.hasClass('active')) {

                        $('#primary-navigation nav li').removeClass('active');
                        $parent.addClass('active');

                        if (settings.viewport === 'higher' && $parent.is('#primary-navigation > ul > li')) {
                            //console.log('Is higher viewport so should drop down');

                            buildFlyout.showFlyout();

                        } else if (settings.viewport === 'higher' && $parent.is('#primary-navigation > ul > li.active > nav > ul > li')) {

                            //console.log('Do not traverse');


                        } else if (settings.viewport === 'higher' && $parent.is('#primary-navigation > ul > li.active > nav > ul > li.active > nav > ul > li')) {//UNFINISHED (Build Back link DDL V2)////////////////////

                            //console.log('traverse for inner desktop');

                            $parent.parent('ul').parent('nav').parent('li').parent('ul').addClass('slide');

                            $parent.parent('ul').siblings('.nav-header').children('a').addClass('back');

                            $parent.parent('ul').parent('nav').parent('li').siblings('li').hide();

                        } else {
                            //console.log('Is lower viewport so should traverse');

                            traverseDown($parent);

                        }

                    } else if ($parent.hasClass('active')) {

                        $parent.removeClass('active');
                        buildFlyout.hideFlyout();

                    } else if ($parent.hasClass('back')) {

                        traverseUp($parent);


                    } else {
                        goToLocation($this);

                    }

                });

                $('#primary-navigation > ul > li').on('click', function () {//For the Close button on higher viewport only

                    var $this = $(this);

                    if (settings.viewport === 'higher') {

                        if ($this.hasClass('active')) {

                            $this.removeClass('active');
                            buildFlyout.hideFlyout();
                        }
                    }
                });

                $('#primary-navigation > ul > li > nav').on('click', function (e) {//Stop the Close button propagating from the inner nav

                    e.stopPropagation();

                });

            }

            return {
                traverseUp: traverseUp,
                traverseDown: traverseDown,
                init: init
            };

        } ());

        var init = function () {
            //console.log('primaryNav.init');

            if ($('body').hasClass('base') || $('body').hasClass('small') || $('body').hasClass('smallest')) {//Depending on the viewport when we call Init

                settings = lowerViewportSettings; //Make the settings lower viewport //NOT EVER UPDATING

                $('#primary-navigation > ul > li:visible').css('display', 'block');
                $('#primary-navigation').find('li:visible').css('display', 'block')


            } else {

                settings = higherViewportSettings; //Make the settings higher viewport //NOT EVER UPDATING

                $('#primary-navigation > ul > li').css('display', 'inline-block');
                $('#primary-navigation').find('li:hidden').css('display', 'block');

            }

            //console.log(settings.viewport);

            $('#primary-navigation ul, #primary-navigation nav').css('height', 'auto');
            $('#primary-navigation .left').removeClass('left');
            $('#primary-navigation .active').removeClass('active');
            $('html').removeClass('ddl-flyout-active');

            buildFlyout.init();
            traverseNav.init();

        };

        init();

        $(window).on('resize', function () {

            init();

        });

    } ());

    //tabbedcontent  -------------------

    DDL.tabbedContent = (function () {

        $('.tabbed-content').each(function () {

            var $this = $(this), $tabs = $('.tab', $this).length, $tabControlHighest = 0;

            //$('.tab-control li', $this).on('click', function(){

            $this.children('.tab-control').find('li').on('click', function () {

                var $clicked = $(this), assoTab = $('[data-tab=' + $clicked.attr('data-tabControl') + ']');

                if ($clicked.hasClass('active')) {

                    //console.log("Already active");

                    if ($this.hasClass('closable')) {

                        $this.children('.active').removeClass('active');


                        setTimeout(function () {

                            $this.children('.tab-control ').children('.active ').removeClass('active');

                        }, 800);

                    }

                } else {

                    //$('.tab-control li.active, .tab.active', $this).removeClass('active');

                    $this.children('.active').removeClass('active');

                    setTimeout(function () {

                        $this.children('.tab-control ').children('.active ').removeClass('active');

                        $clicked.addClass('active');
                        $(assoTab).addClass('active');

                    }, 800);

                }

            });

            if ($('.tab-control li.first').length > 0) {

                $('.tab-control li.first').click();

            }

        });

    } ());

    DDL.modal = (function () {

        //console.log("Called modalBoss");

        var $modal = '<div id="modal-wrapper"><div id="modal-wrapper-inner"><div id="modal"><a href="#n" id="close-modal" class="ddl-icon-before ddl-icon-close"><span class="ddl-visually-hidden">Close</span></a><div id="modal-content"></div></div></div></div>';

        //  $('body').append($modal);

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


            //console.log($specsObject);


            if ($specsObject.method === 'static') {

                $this.hide();

                //console.log('static');

                if ($specsObject.trigger === 'load') {

                    setTimeout(function () {

                        var $theClone = $this.clone();

                        $('#modal-content').html($theClone);

                        $('[data-modalSpecs]', '#modal-content').show();

                        $('#modal-wrapper').addClass('active');

                    }, 1000);

                } else {

                    $('#' + $specsObject.trigger).on('click', function (e) {

                        var $theClone = $this.clone(true);

                        e.preventDefault();

                        $('#modal-content').html($theClone);

                        $('[data-modalSpecs]', '#modal-content').show();

                        $('#modal-wrapper').addClass('active');

                    });
                }

            } else if ($specsObject.method === 'remote') {

                //Deal with remote

            }

        });

        $('#modal-wrapper, #close-modal, .close-modal').on('click', function (e) {

            e.preventDefault();

            $('#modal-wrapper').removeClass('loading').removeClass('active');
            $('#modal-content').html("");

        });

        $('#modal, #modal-content').on('click', function (e) {

            e.stopPropagation();

        });

    } ());

    //accordion  -------------------

    DDL.accordion = (function () {

        var init = function () {

            $('.accordion, .base .accordion-handheld, .smallest .accordion-handheld, .small .accordion-handheld').each(function () {

                var $this = $(this);

                $('.accordion-header', $this).each(function () {

                    var $ah = $(this), $ac = $ah.next('.accordion-content');

                    $ah.off();

                    $ah.on('click', function (e) {
                        e.preventDefault();
                        //e.stopPropagation();
                        //console.log("clicked accordion header");

                        if ($ac.hasClass('active')) {

                            $ah.removeClass('active');
                            $ac.removeClass('active');

                        } else {

                            //if(!$this.hasClass('independent-accordion')){

                            //console.log($this);

                            $(this).siblings('.accordion-header').removeClass('active');
                            $(this).siblings('.accordion-header').children('.accordion-content').removeClass('active');

                            //$('.accordion-header', $this).removeClass('active');
                            //$('.accordion-content', $this).removeClass('active');

                            //}

                            setTimeout(function () {

                                $ah.addClass('active');
                                $ac.addClass('active');

                            }, 500);

                        }

                    });

                    $('input', $ah).on('click', function (e) {

                        //e.stopPropagation();

                    });

                });

            });

        }


        $(window).on('resize', function () {

            init();

        });

        init();

    } ());

    //Equal height panels  -------------------

    DDL.equalHeight = (function () {

        var $highest = 0;

        $('.ddl-equalheight').each(function () {

            var $this = $(this);

            $('.ddl-equalchild', $this).each(function () {

                var $that = $(this);

                if (parseInt($that.outerHeight(), 10) > $highest) {

                    $highest = parseInt($that.outerHeight(), 10);

                }

            });


            $('.ddl-equalchild', $this).css('min-height', $highest);

        });

    } ());


    //AutoNextField  -------------------

    DDL.autoNextField = (function () {

        $('body').on('keyup', '.autotab input', function () {

            var $this = $(this);

            if ($this.val().length >= 1) {


                $(this).parent().nextAll().children(":input").eq(0).focus();


            }


        });

    } ());

    //Dismiss Message banner  -------------------

    DDL.dismissMessageBanner = (function () {

        $('#divDotcomInfo').each(function () {

            var $this = $(this);

            $('.close', $this).on('click', function (e) {

                e.preventDefault();

                $this.slideUp();

            });

        });

    } ());


    //Carousel (Slick)  -------------------

    DDL.carousel = (function () {

        if ($('.carousel').length > 0) {

            $('.carousel').slick({
                responsive: [{
                    breakpoint: 99999,
                    settings: "unslick"
                },
                {
                    breakpoint: 480,
                    settings: {
                        arrows: false,
                        dots: true,
                        infinite: false
                    }
                }]
            });

        }

    } ());

    //Select all  -------------------

    DDL.selectAll = (function () {

        $('[data-selectAllWrapper]').each(function () {

            var $this = $(this), $selectTrigger = $('[data-selectAllTrigger]', $this), $all = $('[data-selectAll]', $this);

            if ($all.hasClass('coupons-list')) {

                var $inputs = $all.find('input[type=checkbox]');

                $inputs.each(function () {

                    var $this = $(this);

                    if ($this.parent('.ddl-checkbox').siblings('label').children('img').length < 1) {

                        $this.prop("disabled", true);
                    }

                });
            }

            $selectTrigger.on('click', function () {

                var $inputs = $all.find('input[type=checkbox]');

                $inputs.each(function () {

                    var $this = $(this);

                    if (!$selectTrigger.is(':checked') && !$this.is(':disabled')) {

                        $this.prop('checked', false);

                    } else if ($selectTrigger.is(':checked') && !$this.is(':disabled')) {

                        $this.prop('checked', true);

                    }
                });

            });


        });

    } ());

    //Fieldset margins  ------------------- :( Did not want to do this

    DDL.fieldsetMargins = (function () {

        $('.ddl-fieldset').each(function () {

            var $this = $(this);

            if ($this.next().is('.copy-wrapper') || $this.next().is('.conditional-content')) {

                $this.css('margin-bottom', '0');


            }

        });

    } ());

    //Conditional content  -------------------

    DDL.conditionalContent = (function () {
        //console.log("1");

        $('.conditional-content').each(function () {

            var $this = $(this);

            $('[data-conditionalContent]', $this).each(function () {
                //console.log("2");

                var $cc = $(this);

                $($cc).hide();

                $('[data-contentTrigger="' + $cc.attr('data-conditionalContent') + '"]').on('click', function (e) {

                    var $clicked = $(this);

                    if ($clicked.is('a')) {

                        e.preventDefault();

                    }

                    $('[data-conditionalContent]', $this).hide();

                    $($cc).fadeIn();

                });

            });

            $('.current-selected').click();

        });

    } ());

    //Fix arrow direction on selects after selection  -------------------

    DDL.selectArrow = (function () {

        $('select').on('change', function () {

            var $this = $(this);

            $this.blur();

        });

    } ());

    //Dismiss cookie notification  -------------------

    $('#cookie-notification .ddl-icon-close').on("click", function (e) {

        $('#cookie-notification').slideUp();

    });

    $(".points-callout-progress .indicator i").each(function () {
        var a = $(this), e = a.parent().attr("data-start"), x = a.parent().attr("data-current"), i = a.parent().attr("data-goal"), n = (i - x) / 100;
        a.css("width", (e - x) / n + "%"); 
    })

    //PATTERN LIBARY ==========================================================

    DDL.patternLibraryIframe = (function () {

        $('.ddl-iframe').each(function (i) {

            var $this = $(this);

            $this.before("<ul class='sizer sizer-" + i + "'><li class='base'>base</li></li><li class='smallest'>smallest</li><li class='small'>small</li><li class='medium'>medium</li><li class='large active'>large</li><li class='massive'>massive</li></ul>")

            $('.sizer-' + i + ' li').on('click', function () {

                var $that = $(this);

                $('.sizer-' + i + ' li').removeClass('active');

                $that.addClass('active');

                $this.attr('class', '').addClass('ddl-iframe').addClass($that.attr('class'));


            });

            $this.load(function () {

                if ($(this).contents().height() < 400) {

                    $this.height(400);

                } else {

                    $this.height($(this).contents().height());

                }

            });

        });

    } ());

    //===========================================================================

});


$(document).ready(function () {

    $('#trigger-details-correct-step-2').on('click', function (e) {

        e.preventDefault();

        $('#details-correct-step-1').hide();

        $('#details-correct-step-2').show();

    });

    // for coupon strip
    $('#liSignout').on('click', function () {
        // cleaning all session storage objects created by MCA
        sessionStorage.removeItem("cc");
    });

    var st = $('#couponsStrip');
    if (st.length > 0) {
        var cc = sessionStorage.getItem('cc');
        var url = $('#couponlink').html();
        if (cc === null) {
            $.ajax({
                type: "Get",
                url: url,
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    sessionStorage.setItem('cc', data.count);
                    st.attr('data-attr', 'get');
                    if (data.count > 0) {
                        $('#couponsTotal').text(data.count);
                        st.show();
                    }
                    else {
                        st.hide();
                    }
                },
                error: function (response) {
                    console.error(response.responseText);
                }
            });
        }
        else {
            if (cc > 0) {
                $('#couponsTotal').text(cc);
                st.show();
            } else {
                st.hide();
            }
        }
    }

    var st = $('#vouchersStrip');
    var vv = $('#vouchersValue');
    if (st.length > 0 && vv.length > 0) {
        var url = $('#voucherlink').html();
        var loader = new textLoader($('#vouchersValue'));
        loader.start();
        $.ajax({
            type: "Get",
            url: url,
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                loader.stop();
                $('#vouchersValue').hide();
                $('#vouchersValue').text(data.count);
                $('#vouchersValue').fadeIn();
                $('#voucherCurrencySymbol').fadeIn();
                $('#voucherAlphaSymbol').fadeIn();
            },
            error: function (response) {
                loader.stop();
                console.error(response.responseText);
            }
        });
    }
});

function textLoader(span) {
    this.textControl = span;
    this.isRunning = false;
    this.Char = ".";
    this.Text = this.Char;
    this.Max = 5;
    this.Current = 0;
    this.IsFading = true;
    for (var i = 0; i < this.Max; i++) {
        $(this.textControl).append($('<span />').addClass('fade').attr('id', i).css('width', '2px').text(this.Char));
    }
    this.Loader;
}
textLoader.prototype.start = function () {
    var ctrl = this;
    if (!this.isRunning) {
        this.isRunning = true;
        this.Loader = setInterval(function () {
            var fade = $(ctrl.textControl).find('#' + ctrl.Current);
            if (ctrl.IsFading) {
                $(fade).removeClass('fade').addClass('fadeout');
                //$(fade).fadeOut();
                $(fade).css("opacity", 0);
                ctrl.Current += 1;
                if (ctrl.Current == ctrl.Max - 1) {
                    ctrl.IsFading = false;
                }
            }
            else {
                ctrl.Current -= 1;
                if (ctrl.Current == 0) {
                    ctrl.IsFading = true;
                }
                $(fade).removeClass('fadeout').addClass('fade');
                //$(fade).fadeIn();
                $(fade).css("opacity", 100);
            }
        }, 500);
    }
}

textLoader.prototype.stop = function () {
    if (this.isRunning) {
        clearInterval(this.Loader);
        this.isRunning = false;
        $(this.textControl).text("");
    }
}

$(document).ready(function () {
    $('#btnDetailsAlreadyUpdated').css('cursor', 'pointer');
    $('#btnDetailsAlreadyUpdated').click(function () {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            type: "POST",
            url: './OneAccountPersonalDetails',
            data: {
                __RequestVerificationToken: token
            },
            success: function (result) {
                $('#divDotcomInfo').addClass("show");
                $('#divUpdateInfo').addClass("hide");
            },
            error: function (xhr, status, err) {
                console.log(err);

            }
        });

    });

    $('#btnDotcomDetailsAreCorrect').css('cursor', 'pointer');
    $('#btnDotcomDetailsAreCorrect').click(function () {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            type: "POST",
            url: './OneAccountPersonalDetails2',
            data: {
                __RequestVerificationToken: token
            },
            success: function (result) {
            },
            error: function (xhr, status, err) {
                console.log(err);
            }
        });
    });

    var vouchersCount = $('#vouchersCountStrip');
    var isVoucherValueVisible = $('#lblIsVoucherValueVisible');
    var txt = $('#vouhersLeft').text();


    if (vouchersCount != null && vouchersCount.length > 0) {

        var methodCall = $('#lblVoucherCountURL').html();

        $.ajax({
            type: "Get",
            url: methodCall,
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.count > 0) {

                    if (isVoucherValueVisible != null && isVoucherValueVisible.html().toUpperCase() == "TRUE") {
                        txt = txt.replace("Alpha", data.count);
                        $('#vouhersLeft').text(txt);
                    }
                    else {
                        $('#vouhersLeft').text("");
                    }

                    vouchersCount.show();
                }
                else {
                    vouchersCount.hide();
                }
            },
            error: function (response) {
                console.error(response.responseText);
            }
        });
    }
});


