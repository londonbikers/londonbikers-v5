(function ($) {
    $("a[rel^='prettyPhoto']").prettyPhoto({ overlay_gallery: false, theme: 'facebook', social_tools: '', deeplinking: false });
    $('.nhome,.nnews,.nfeatures,.nphotos,.nevents,.ndatabase,.nshop,.ncommunity').append('<span class="hover"></span>').each(function () {
        var $span = $('> span.hover', this).css('opacity', 0);
        $(this).hover(function () {
            $span.stop().fadeTo(250, 1);
        }, function () {
            $span.stop().fadeTo(250, 0);
        });
    });

    // handle enter on form submit.
    $('#searchTerm').keypress(function (e) {
        if (e.which == 13) {
            GoSearch();
        }
    });

    // gallery sliders
    $('.boxgrid.caption').hover(function () {
        $(".cover", this).stop().animate({ top: '105px' }, { queue: false, duration: 160 });
    }, function () {
        $(".cover", this).stop().animate({ top: '125px' }, { queue: false, duration: 160 });
    });

    // features sliders
    $('.featureboxgrid.caption').hover(function () {
        $(".featureCover", this).stop().animate({ top: '105px' }, { queue: false, duration: 160 });
    }, function () {
        $(".featureCover", this).stop().animate({ top: '128px' }, { queue: false, duration: 160 });
    });

    var total = $('#tcv img').length;
    var rand = Math.floor(Math.random() * total);

    try {
        $('#tcv').nivoSlider({ controlNav: false, startSlide: rand, pauseOnHover: true });
    } catch (err) {
    }

})(window.jQuery);

function GoSearch() {
    $term = $("#searchTerm").val();
    $term = $term.replace(/-/g, "--");
    $term = $term.replace(/ /g, "-");
    window.location = "/search/" + $term;
}