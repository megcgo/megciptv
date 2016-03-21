/*!
 * MegCgo.JS
 * MegC <> megc.pt
 * Stuff needed for webapp to work
 *
*/

$(document).ready(function() {

    if ($("[data-toggle=tooltip]").length) {
        $("[data-toggle=tooltip]").tooltip();
    }

    $('#btnSubmit').bind("click", function (e) {
        //e.preventDefault();
        $('#btnSubmit').attr("disabled", true);
        $('#btnSubmit').html('<i class="fa fa-spinner"></i> Processing file...');
        $('#btnSubmit i').addClass('fa-spin');
    });

    if (typeof Clipboard != 'undefined') {
        var clipboard = new Clipboard('.btnCopy');

        clipboard.on('success', function(e) {
            var id = e.trigger.getAttribute("data-clipboard-text");
            var tri = $('.btnCopy[data-clipboard-text="' + id + '"]');
            tri.addClass('copied');
            setTimeout(function() { tri.removeClass('copied'); }, 4000);
        });

        clipboard.on('error', function(e) {
            console.log(e);
        });
    }


});