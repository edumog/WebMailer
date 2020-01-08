$(document).ready(function () {
    $("#MailPass").attr('type', 'password');
    $(".show-pass").click(function () {
        var passType = $("#MailPass").attr('type');
        if (passType == "password"){
            $("#MailPass").attr('type', 'text');
            $(this).addClass('hiden-pass');
        }
        if (passType == "text") {
            $("#MailPass").attr('type', 'password');
            $(this).removeClass('hiden-pass');
        }
    })
})