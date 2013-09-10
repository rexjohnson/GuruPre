/*********************
//* jQuery Multi Level CSS Menu #2- By Dynamic Drive: http://www.dynamicdrive.com/
//* Last update: Nov 7th, 08': Limit # of queued animations to minmize animation stuttering
//* Menu avaiable at DD CSS Library: http://www.dynamicdrive.com/style/
*********************/
var arrowimages = {
    down: ['', ''],
    right: ['', '']
}
var jqueryslidemenu = {

    animateduration: {
        over: 200,
        out: 200
    }, //duration of slide in/ out animation, in milliseconds
    buildmenu: function (menuid, arrowsvar) {
        jQuery(document).ready(function ($) {
            $(" #main_navigation a").removeAttr("title");
            var $mainmenu = $("#" + menuid + ">ul")
            var $headers = $mainmenu.find("ul").parent()
            $headers.each(function (i) {
                var $curobj = $(this)
                var $subul = $(this).find('ul:eq(0)')
                this._dimensions = {
                    w: this.offsetWidth,
                    h: this.offsetHeight,
                    subulw: $subul.outerWidth(),
                    subulh: $subul.outerHeight()
                }
                this.istopheader = $curobj.parents("ul").length == 1 ? true : false
                $subul.css({
                    top: this.istopheader ? this._dimensions.h + "px" : 0
                })

                var $targetul = $(this).children("ul:eq(0)")
                this._offsets = {
                    left: $(this).offset().left,
                    top: $(this).offset().top
                }
                var menuleft = this.istopheader ? 0 : this._dimensions.w
                menuleft = (this._offsets.left + menuleft + this._dimensions.subulw > $(window).width()) ? (this.istopheader ? -this._dimensions.subulw + this._dimensions.w : -this._dimensions.w) + 12 : menuleft
                if ($targetul.queue().length <= 1) //if 1 or less queued animations
                    if (menuleft == 0) {
                        $targetul.css({
                            left: menuleft + "px",
                            width: this._dimensions.subulw + 'px'
                        }).removeClass("menu_flip")
                    }
                if (menuleft != 0 && this.istopheader) {
                    $targetul.css({
                        left: menuleft + "px",
                        width: this._dimensions.subulw + 'px'
                    }).addClass("menu_flip")
                }
                else {
                    $targetul.css({
                        left: menuleft + "px",
                        width: this._dimensions.subulw + 'px'
                    }).removeClass("menu_flip")
                }

                $curobj.hover(function (e) {
                    var $targetul = $(this).children("ul:eq(0)")
                    this._offsets = {
                        left: $(this).offset().left,
                        top: $(this).offset().top
                    }
                    var menuleft = this.istopheader ? 0 : this._dimensions.w
                    menuleft = (this._offsets.left + menuleft + this._dimensions.subulw > $(window).width()) ? (this.istopheader ? -this._dimensions.subulw + this._dimensions.w : -this._dimensions.w) + 12 : menuleft
                    if ($targetul.queue().length <= 1) //if 1 or less queued animations
                        if (menuleft == 0) {
                            $targetul.css({
                                left: menuleft + "px",
                                width: this._dimensions.subulw + 'px'
                            }).removeClass("menu_flip").slideDown(jqueryslidemenu.animateduration.over)
                        }
                    if (menuleft != 0 && this.istopheader) {
                        $targetul.css({
                            left: menuleft + "px",
                            width: this._dimensions.subulw + 'px'
                        }).addClass("menu_flip").slideDown(jqueryslidemenu.animateduration.over)
                    }
                    else {
                        $targetul.css({
                            left: menuleft + "px",
                            width: this._dimensions.subulw + 'px'
                        }).removeClass("menu_flip").slideDown(jqueryslidemenu.animateduration.over)
                    }
                }, function (e) {
                    var $targetul = $(this).children("ul:eq(0)")
                    $targetul.slideUp(jqueryslidemenu.animateduration.out)
                }) //end hover
            }) //end $headers.each()
            $mainmenu.find("ul").css({
                display: 'none',
                visibility: 'visible'
            })
        }) //end document.ready
    }
}

// Making Slide Menu
jqueryslidemenu.buildmenu("nav-wrapper", arrowimages)

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
    return pattern.test(emailAddress);
}

// START Initiating
jQuery(document).ready(function () {

    /***************************************************
    ZOOM PORTFOLIO HOVER - COLUMN
    ***************************************************/
    jQuery("#folio a, #entries a, #sidebar a").contents("span.frame").hover(function () {
        jQuery(this).stop().animate({
            opacity: 0.5
        }, 500);
    }, function () {
        jQuery(this).stop().animate({
            opacity: 1
        }, 500);
    });

    /***************************************************
    JQUERY TOGGLE
    ***************************************************/
    $(".toggle-body").hide();
    $(".toggle-head").click(function () {
        var tb = $(this).next(".toggle-body");

        if (tb.is(':hidden')) {
            tb.prev('.toggle-head').children('h3').addClass('minus');
            tb.slideDown('slow');

        }
        else {
            tb.slideUp(200, function () {
                tb.prev('.toggle-head').children('h3').removeClass('minus');
            });
        }
    });


    /***************************************************
    TAB
    ***************************************************/
    //Default Action
    $(".tab_content").hide(); //Hide all content

    $("div.tabs a:first").removeClass("button-link-inactive"); //Activate first tab
    $("div.tabs a:first").addClass("button-link-active").show(); //Activate first tab
    $(".tab_content:first").show(); //Show first tab content

    //On Click Event
    $("div.tabs a").click(function () {
        $("div.tabs a").removeClass("button-link-active");
        $("div.tabs a").addClass("button-link-inactive");

        $(this).removeClass("button-link-inactive");

        $(this).addClass("button-link-active");


        $(".tab_content").hide(); //Hide all tab content
        var activeTab = $(this).attr("href"); //Find the rel attribute value to identify the active tab + content
        $(activeTab).fadeIn(); //Fade in the active content
        return false;
    });


    /***************************************************
    FADE ON HOVER
    ***************************************************/
    function fade() {
        jQuery('.fade').hover(function () {
            jQuery(this).stop().animate({
                opacity: 0.5
            }, 400);
        }, function () {
            jQuery(this).stop().animate({
                opacity: 1
            }, 400);
        });

    }

    function invers_fade() {

        $('.ifade').css({
            'opacity': '0.5'
        });

        jQuery('.ifade').hover(function () {
            jQuery(this).stop().animate({
                opacity: 1
            }, 400);
        }, function () {
            jQuery(this).stop().animate({
                opacity: 0.5
            }, 400);
        });

    }

    $('#s').addClass('fade');
    $('.flickr_badge_image').addClass('fade');

    fade();
    invers_fade();

    /***************************************************
    PrettyPhoto
    ***************************************************/
    jQuery("a[rel^='prettyPhoto'], a[rel^='lightbox']").prettyPhoto({
        overlay_gallery: false,
        "theme": 'light_rounded' /* light_square / dark_rounded / light_square / dark_square */
    });


    /***************************************************
    Smooth Scrolling
    http://github.com/kswedberg/jquery-smooth-scroll
    ***************************************************/
    function enable_smooth_scroll() {
        function filterPath(string) {
            return string;
            //.replace(/^\//,'')
            // .replace(/(index|default).[a-zA-Z]{3,4}$/,'')
            //.replace(/\/$/,'');
        }

        var locationPath = filterPath(location.pathname);

        var scrollElement = 'html, body';
        $('html, body').each(function () {
            var initScrollTop = $(this).attr('scrollTop');
            $(this).attr('scrollTop', initScrollTop + 1);
            if ($(this).attr('scrollTop') == initScrollTop + 1) {
                scrollElement = this.nodeName.toLowerCase();
                $(this).attr('scrollTop', initScrollTop);
                return false;
            }
        });

        $('a[href*=#header]').each(function () {
            var thisPath = filterPath(this.pathname) || locationPath;
            if (locationPath == thisPath
                && (location.hostname == this.hostname || !this.hostname)
                && this.hash.replace(/#/, '')
            ) {
                if ($(this.hash).length) {
                    $(this).click(function (event) {
                        var targetOffset = $(this.hash).offset().top;
                        var target = this.hash;
                        event.preventDefault();
                        $(scrollElement).animate(
                            { scrollTop: targetOffset },
                            500,
                            function () {
                                location.hash = target;
                            });
                    });
                }
            }
        });
    }

    enable_smooth_scroll();
    /***************************************************
    Contact Form
    ***************************************************/
    jQuery('form#contactform').submit(function () {

        var hasError = false;

        var name = $("input#fullname").val();
        if (jQuery.trim(name) == "") {
            $("input#fullname").prev('label').addClass('error');
            $("input#fullname").focus();
            hasError = true;

        }
        else {
            $("input#fullname").prev('label').removeClass('error');
        }

        var email = $("input#email").val();
        if (jQuery.trim(email) == "" || !isValidEmailAddress(email)) {
            $("input#email").prev('label').addClass('error');
            $("input#email").focus();
            hasError = true;
        }
        else {
            $("input#email").prev('label').removeClass('error');
        }

        var comment = $("#form_message").val();
        if (jQuery.trim(comment) == "") {
            $("#form_message").prev('label').addClass('error');
            $("#form_message").focus();
            hasError = true;
        }
        else {
            $("#form_message").prev('label').removeClass('error');
        }


        if (!hasError) {
            jQuery('form#contactform .ibutton').fadeOut('normal', function () {
                jQuery('.loading').css({
                    display: "block"
                });

            });

            $.post($("#contactform").attr('action'), $("#contactform").serialize(), function (data) {
                $('.log').html(data);
                $('.loading').remove();
                $('#contactform').slideUp('slow');
            });


        }

        return false;

    });


    /***************************************************
    Misc
    ***************************************************/
    $('.opacity').css('opacity', '0.8');

   


}); // End Initiating

