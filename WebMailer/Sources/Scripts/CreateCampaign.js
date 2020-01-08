$(document).ready(function () {
    LocationValidation();
    $("#loc").click(function () {
        LocationValidation();
    })
    function LocationValidation() {
        if ($("#loc").val() == 0) {
            $("#loc")[0].setCustomValidity("La campaña debe tener una sede asociada.");
        } else {
            $("#loc")[0].setCustomValidity("");
        }
    }
})