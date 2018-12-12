function AlertMessage(control, message) {
    $(".alert").remove();
    $(control).before('<div class="alert alert-success" role="alert">' +
                        //'<button type="button" class="close" data-dismiss="alert">×</button>' +
                        message +
                        '</div>');
    $(".alert").delay(2000).fadeOut("slow", function () { $(this).remove(); });
}