$(document).ready(function () {
    $("#usr").change(function () {
        var usr = this;
        var userName = this.value;
        $.ajax({
            url: "/Account/NameAvailability",
            type: "GET",
            data: { name: userName },
            success: function (response) {
                usr.setCustomValidity(response);
            },
            timeout: 20000
        })
    })
})