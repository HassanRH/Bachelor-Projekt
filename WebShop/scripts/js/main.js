(function ($) {
    "use strict";

    /*[ Load page ]
    ===========================================================*/
    $(".animsition").animsition({
        inClass: 'fade-in',
        outClass: 'fade-out',
        inDuration: 1500,
        outDuration: 800,
        linkElement: '.animsition-link',
        loading: true,
        loadingParentElement: 'html',
        loadingClass: 'animsition-loading-1',
        loadingInner: '<div class="loader05"></div>',
        timeout: false,
        timeoutCountdown: 5000,
        onLoadEvent: true,
        browser: [ 'animation-duration', '-webkit-animation-duration'],
        overlay : false,
        overlayClass : 'animsition-overlay-slide',
        overlayParentElement : 'html',
        transition: function(url){ window.location.href = url; }
    });
    
    /*[ Back to top ]
    ===========================================================*/
    var windowH = $(window).height()/2;

    $(window).on('scroll',function(){
        if ($(this).scrollTop() > windowH) {
            $("#myBtn").css('display','flex');
        } else {
            $("#myBtn").css('display','none');
        }
    });

    $('#myBtn').on("click", function(){
        $('html, body').animate({scrollTop: 0}, 300);
    });


    /*==================================================================
    [ Fixed Header ]*/
    var headerDesktop = $('.container-menu-desktop');
    var wrapMenu = $('.wrap-menu-desktop');

    if($('.top-bar').length > 0) {
        var posWrapHeader = $('.top-bar').height();
    }
    else {
        var posWrapHeader = 0;
    }
    

    if($(window).scrollTop() > posWrapHeader) {
        $(headerDesktop).addClass('fix-menu-desktop');
        $(wrapMenu).css('top',0); 
    }  
    else {
        $(headerDesktop).removeClass('fix-menu-desktop');
        $(wrapMenu).css('top',posWrapHeader - $(this).scrollTop()); 
    }

    $(window).on('scroll',function(){
        if($(this).scrollTop() > posWrapHeader) {
            $(headerDesktop).addClass('fix-menu-desktop');
            $(wrapMenu).css('top',0); 
        }  
        else {
            $(headerDesktop).removeClass('fix-menu-desktop');
            $(wrapMenu).css('top',posWrapHeader - $(this).scrollTop()); 
        } 
    });


    /*==================================================================
    [ Menu mobile ]*/
    $('.btn-show-menu-mobile').on('click', function(){
        $(this).toggleClass('is-active');
        $('.menu-mobile').slideToggle();
    });

    var arrowMainMenu = $('.arrow-main-menu-m');

    for(var i=0; i<arrowMainMenu.length; i++){
        $(arrowMainMenu[i]).on('click', function(){
            $(this).parent().find('.sub-menu-m').slideToggle();
            $(this).toggleClass('turn-arrow-main-menu-m');
        })
    }

    $(window).resize(function(){
        if($(window).width() >= 992){
            if($('.menu-mobile').css('display') == 'block') {
                $('.menu-mobile').css('display','none');
                $('.btn-show-menu-mobile').toggleClass('is-active');
            }

            $('.sub-menu-m').each(function(){
                if($(this).css('display') == 'block') { 
                    $(this).css('display','none');
                    $(arrowMainMenu).removeClass('turn-arrow-main-menu-m');
                }
            });
                
        }
    });


    /*==================================================================
    [ Show / hide modal search ]*/
    $('.js-show-modal-search').on('click', function(){
        $('.modal-search-header').addClass('show-modal-search');
        $(this).css('opacity','0');
    });

    $('.js-hide-modal-search').on('click', function(){
        $('.modal-search-header').removeClass('show-modal-search');
        $('.js-show-modal-search').css('opacity','1');
    });

    $('.container-search-header').on('click', function(e){
        e.stopPropagation();
    });


    /*==================================================================
    [ Isotope ]*/
    var $topeContainer = $('.isotope-grid');
    var $filter = $('.filter-tope-group');

    // filter items on button click
    $filter.each(function () {
        $filter.on('click', 'button', function () {
            var filterValue = $(this).attr('data-filter');
            $topeContainer.isotope({filter: filterValue});
        });
        
    });

    // init Isotope
    $(window).on('load', function () {
        var $grid = $topeContainer.each(function () {
            $(this).isotope({
                itemSelector: '.isotope-item',
                layoutMode: 'fitRows',
                percentPosition: true,
                animationEngine : 'best-available',
                masonry: {
                    columnWidth: '.isotope-item'
                }
            });
        });
    });

    var isotopeButton = $('.filter-tope-group button');

    $(isotopeButton).each(function(){
        $(this).on('click', function(){
            for(var i=0; i<isotopeButton.length; i++) {
                $(isotopeButton[i]).removeClass('how-active1');
            }

            $(this).addClass('how-active1');
        });
    });

    /*==================================================================
    [ Filter / Search product ]*/
    $(document).on('click', '.js-show-filter', function(){
        $(this).toggleClass('show-filter');
        $('.panel-filter').slideToggle(400);

        if($('.js-show-search').hasClass('show-search')) {
            $('.js-show-search').removeClass('show-search');
            $('.panel-search').slideUp(400);
        }    
    });

    $(document).on('click', '.js-show-search', function () {
        $('#searchinput').focus(); 
    });


    /*=================================================================
     [Product View] 

    $('.productView').on('click', function () {
        $('.js-panel-cart').addClass('show-header-cart');
    });

    $('.productView').on('click', function () {
        $('.js-panel-cart').removeClass('show-header-cart');
    });
    */

    /*==================================================================
    [ Cart ]*/
    $('.js-show-cart').on('click',function(){
        $('.js-panel-cart').addClass('show-header-cart');
    });

    $('.js-hide-cart').on('click',function(){
        $('.js-panel-cart').removeClass('show-header-cart');
    });

    /*==================================================================
    [ Cart ]*/


    /*==================================================================
    [ +/- num product ]*/
    $(document).on('click','.btn-num-product-down', function(){
        var numProduct = Number($(this).next().val());
        if (numProduct > 1) $(this).next().val(numProduct - 1);
    });

    $(document).on('click','.btn-num-product-up', function(){
        var numProduct = Number($(this).prev().val());
        $(this).prev().val(numProduct + 1);
    });
    
    /*==================================================================
    [ Show modal1 ]*/
    $(document).on('click', '.js-show-modal1', function(e){
        e.preventDefault();
        var $el = $(this);
        $.get("/umbraco/surface/Product/GetSingleProduct", {
            id: $el.data('id')
        }, function (data) {
                $('#singleProductView').replaceWith(data);
                $('.js-modal1').addClass('show-modal1');
                $('.gallery-lb').each(function () { // the containers for all your galleries
                    $(this).magnificPopup({
                        delegate: 'a', // the selector for gallery item
                        type: 'image',
                        gallery: {
                            enabled: true
                        },
                        mainClass: 'mfp-fade'
                    });
                });
                $('.wrap-slick3').each(function () {
                    $(this).find('.slick3').slick({
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        fade: true,
                        infinite: true,
                        autoplay: false,
                        autoplaySpeed: 6000,

                        arrows: true,
                        appendArrows: $(this).find('.wrap-slick3-arrows'),
                        prevArrow: '<button class="arrow-slick3 prev-slick3"><i class="fa fa-angle-left" aria-hidden="true"></i></button>',
                        nextArrow: '<button class="arrow-slick3 next-slick3"><i class="fa fa-angle-right" aria-hidden="true"></i></button>',

                        dots: true,
                        appendDots: $(this).find('.wrap-slick3-dots'),
                        dotsClass: 'slick3-dots',
                        customPaging: function (slick, index) {
                            var portrait = $(slick.$slides[index]).data('thumb');
                            return '<img src=" ' + portrait + ' "/><div class="slick3-dot-overlay"></div>';
                        },
                    });
                });
        });
  
    });

    $(document).on('click', '.js-hide-modal1', function (e) {
        $('#singleProductView').html("");
        $('.js-modal1').removeClass('show-modal1');
    });

    /*==================================================================
    [ Cart Sidebar ]*/

    $(document).on('click', '.js-add-cart', function (e) {
        e.preventDefault();
        var $el = $(this);
        var amountProduct = $('#amountOfItem').val();
        $.get("/umbraco/surface/Cart/AddProductToCart", {
            id: $el.data('id'),
            amount: amountProduct
        }, function (data) {
                $('#cart-html').replaceWith(data);
                swal({
                    title: amountProduct + "x " + $el.data('name'),
                    text: "is added to your cart",
                    icon: "success",
                });
                $.get("/umbraco/surface/Cart/UpdateCartLogo", {}, function (data) {
                    $(".cartlogo").replaceWith(data);
                });
        });
    })

    $(document).on('click', '.remove-from-cart', function (e) {
        e.preventDefault();
        var $el = $(this);
        var amountProduct = $('#amountOfItem').val();
        swal({
            dangerMode: true,
            buttons: true,
            title: amountProduct + "x " + $el.data('name'),
            text: "will be removed from the cart",
            icon: "warning",
        }).then(function (isConfirm) {
                if (isConfirm) {
                    $.get("/umbraco/surface/Cart/RemoveProductFromCart", {
                        id: $el.data('id')
                    }, function (data) {
                        $('#cart-html').replaceWith(data);
                        $('.js-panel-cart').addClass('show-header-cart');

                        $.get("/umbraco/surface/Cart/UpdateCartLogo", {}, function (data) {
                           $(".cartlogo").replaceWith(data);
                        });
                    });

                }
        });
    })

    $(document).on('click', '.checkout-click1', function (e) {
        e.preventDefault();
        $.get("/umbraco/surface/Checkout/GetCheckout", {}, function (data) {
            $('#cartForm').replaceWith(data);
        });

        
    })

    $(document).on('click', '.adjust-cart-amount', function (e) {
        e.preventDefault();
        var $el = $(this);
        var productID = $el.data('id');
        var amountInt = $('#amountOfProduct_' + $el.data('id')).val();
        
        $.get("/umbraco/surface/Cart/AdjustProductAmount", {
            id: $el.data('id'),
            amount: $('#' + $el.data('id')).val(),
        }, function (data) {
                $('#cartForm').replaceWith(data);
        });
    })

    $(document).on('click', '.remove-from-cart-page', function (e) {
        var $el = $(this);
        swal({
            dangerMode: true,
            buttons: true,
            title: $el.data('name'),
            text: "will be removed from the cart",
            icon: "warning",
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.get("/umbraco/surface/Cart/RemoveProductFromCartPage", {
                    id: $el.data('id')
                }, function (data) {
                        $('#fullCartView').replaceWith(data);

                        $.get("/umbraco/surface/Cart/GetSideShoppingCart", {}, function (data) {
                            $('#cart-html').replaceWith(data);
                        });

                        $.get("/umbraco/surface/Cart/UpdateCartLogo", {}, function (data) {
                            $(".cartlogo").replaceWith(data);
                        });
                });
            }
        });
    })

    $(document).on('click','.js-show-cart', function () {
        $('.js-panel-cart').addClass('show-header-cart');
    });

    $(document).on('click','.js-hide-cart', function () {
        $('.js-panel-cart').removeClass('show-header-cart');
    });

    $(document).on('click','.js-show-sidebar', function () {
        $('.js-sidebar').addClass('show-sidebar');
    });

    $(document).on('click','.js-hide-sidebar', function () {
        $('.js-sidebar').removeClass('show-sidebar');
    });

    /*==================================================================
        [ Contact Page ]*/
    $(document).on('click', '#contactFormSubmit', function (e) {
        //Dont prevent default because umbraco form is executed in contact controller
        var $el = $(this);
        swal({
            text: "Mail is sent",
            icon: "success",
        });
    })

    var timer;
    var loadMore = true;
    /*==================================================================
        [ Shop page ]*/
    $(document).on('click', '#loadmoreButton', function (e) {
        var $el = $(this);
        loadMore = true;
        $el.hide();
        $(document).scroll();
        loadMore = false;
    })

    /*==================================================================
        [ Scroll ]*/
    $(document).scroll(function () {
        if (loadMore) {
            var nearToBottom = 750;
            if ($("#myscrolllistener").length > 0) {
                if (timer) clearTimeout(timer);
                timer = setTimeout(function () {
                    if ($(window).scrollTop() + $(window).height() > $(document).height() - nearToBottom) {
                        $.get("/umbraco/surface/Shop/CheckProductFetch", {}, function (binaryBoolean) {
                            if (binaryBoolean == 1) {
                                $.get("/umbraco/surface/Shop/RenderPagenationProducts", {}, function (data) {
                                    $(data).hide().appendTo('#currentProducts').fadeIn(1000);
                                });
                            }
                        });
                    }
                }, 100);
            }
        }
    })

    /*==================================================================
        [ Filter price search ]*/
    $(document).on('click', '#filterpricesearch', function (e) {
        e.preventDefault();
        var $el = $(this);
        //$(".filterpriceclass").removeClass('filter-link-active');
        $.get("/umbraco/surface/Shop/SearchPrice", {
            price: $el.data('id')
        }, function (prodData) {
                $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(prodData).fadeIn(800); })
                //$el.addClass('filter-link-active');
                $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (filterData) {
                    $('#filterRowView').fadeOut(200, () => { $('#filterRowView').replaceWith(filterData).fadeIn(1000); })
                })
        });
    });

    /*==================================================================
        [ Filter tags search ]*/
    $(document).on('click', '#filtertagssearch', function (e) {
        e.preventDefault();
        var $el = $(this);
        //$(".filtertagsclass").removeClass('filter-link-active');
        $.get("/umbraco/surface/Shop/SearchTags", {
            tag: $el.data('id')
        }, function (prodData) {
                $('.js-modal1').removeClass('show-modal1');
                $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(prodData).fadeIn(800); })
                //$el.addClass('filter-link-active');
                $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (filterData) {
                    $('#filterRowView').fadeOut(200, () => { $('#filterRowView').replaceWith(filterData).fadeIn(1000); }) 
                })
        });
    });


    /*==================================================================
        [ Filter brand search ]*/
    $(document).on('click', '#filterbrandsearch', function (e) {
        e.preventDefault();
        var $el = $(this);
       
        $.get("/umbraco/surface/Shop/SearchBrand", {
            brand: $el.data('id')
        }, function (prodData) {
                $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(prodData).fadeIn(800); })   
                
                $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (filterData) {
                    $('#filterRowView').fadeOut(200, () => { $('#filterRowView').replaceWith(filterData).fadeIn(1000);   })
                })
        });
    });

    /*==================================================================
        [ Filter category search ]*/
    $(document).on('click', '#filtercategorysearch', function (e) {
        e.preventDefault();
        var $el = $(this);
       //$(".filtercategoryclass").removeClass('filter-link-active');
        $.get("/umbraco/surface/Shop/SearchCategory", {
            category: $el.data('id')
        }, function (prodData) {
           $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(prodData).fadeIn(800); })
                //$el.addClass('filter-link-active');
                $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (filterData) {
                    $('#filterRowView').fadeOut(200, () => { $('#filterRowView').replaceWith(filterData).fadeIn(1000); })
                })
        });
    });

    $(document).on('change', '#filtersortsearch', function (e) {
        e.preventDefault();
        $(".filtersortclass").addClass('filter-link-active');
        $.get("/umbraco/surface/Shop/SearchSort", {
            value: $(e.target).val()
        }, function (prodData) {
            $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(prodData).fadeIn(800); })
            //$el.addClass('filter-link-active');
            $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (filterData) {
                $('#filterRowView').fadeOut(200, () => { $('#filterRowView').replaceWith(filterData).fadeIn(1000); })
            })
        });
    });

    $(document).on('submit','#updateCartCheckout', function (e) {
        setTimeout(function() {
            $.get("/umbraco/surface/Cart/UpdateCartLogo", {}, function (data) {
                $(".cartlogo").replaceWith(data);
            });
        },5000)
    });

    $(document).on('click', '.removeFromQueryParams', function (e) {
        e.preventDefault();
        var $el = $(this);
        var filter = $el.data('key');
        var param = $el.data('value');
        $.get("/umbraco/surface/Shop/RemoveFilterParam", {
            key: filter,
            value: param
        }, function (data) {
                $el.fadeOut(200, () => { $el.remove() });
                $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (data) {
                    $('#filterRowView').fadeOut(200, () => { $('#filterRowView').html(data).fadeIn(800); })
                });
                $.get("/umbraco/surface/Shop/RenderProducts", {}, function (data) {
                    $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(data).fadeIn(1000); })   
                });
        })
    });

    $(document).on('click', '.removeAllFromQueryParams', function (e) {
        e.preventDefault();
        var $el = $(this);
        var filter = $el.data('key');
        var param = $el.data('value');

        $.get("/umbraco/surface/Shop/RemoveAllFilterParam", {}, function (data) {
            $("#searchinput").val("");
            $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (data) {
                $('#filterRowView').fadeOut(200, () => { $('#filterRowView').html(data).fadeIn(800); })
            });
            $.get("/umbraco/surface/Shop/RenderProducts", {}, function (data) {
                $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(data).fadeIn(1000); })
            });
        })
    });

    /*==================================================================
        [ search ]*/

    $(document).on('click', '#filtersearchbutton', function (e) {
        e.preventDefault();
        var value = $("#searchinput").val();
        $.get("/umbraco/surface/Shop/Search", {
            search: value
        }, function (data) {
             $('#productlistview').fadeOut(200, () => { $('#productlistview').html(data).fadeIn(200); })
        });
    });
    /*==================================================================
        [ search on keyup ]*/
    var lastValue = "";
    var timeout1;
    $(document).on('keyup', '#searchinput', function (e) {
        var $el = $(this);
        var value = $('#searchinput').val();
        if (value != lastValue) {
            lastValue = value;
            if (timeout1) { clearTimeout(timeout1); }

            timeout1 = setTimeout(function () {
                // Do the search!
                $.get("/umbraco/surface/Shop/Search", {
                    search: $el.val()
                }, function (data) {
                    $('#currentProducts').fadeOut(200, () => { $('#currentProducts').html(data).fadeIn(800); })

                    $.get("/umbraco/surface/Shop/RenderFilterRow", {}, function (filterData) {
                        $('#filterRowView').fadeOut(200, () => { $('#filterRowView').replaceWith(filterData).fadeIn(1000); })
                    })
                });
                // Process....
            }, 600);
        }
    });
})(jQuery);