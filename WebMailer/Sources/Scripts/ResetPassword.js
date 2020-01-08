$(document).ready(function () {
    $("#pwdConf").change(function () {
        var passwordsMatch = this.value == $("#pwd").val();
        if (passwordsMatch) {
            this.setCustomValidity("");
        } else {
            this.setCustomValidity("Las contraseñas no coinciden. Por favor corrija los errores e inténtelo nuevamente.");
        }
    })
})