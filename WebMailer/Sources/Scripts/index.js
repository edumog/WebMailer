$(document).ready(
    function () {       
        var camps = [];
        campaigns();

        $("#loc").change(
            function () {
                campaigns();
            });

        function campaigns() {
            var loc = $("#loc").val();
            $.ajax({
                type: "GET",
                url: "/Campaigns/GetCampaignsInLocation?locationId=" + loc

            }).done(function (response) {
                camps = response.campaigns;
                displayCampaigns();
            }).fail(function () {
                alert("La solicitud ajax fallo");
            });
        }

        function displayCampaigns() {
            clearList();
            var fila;
            for (var i = 0; i < camps.length; i++) {
                fila = '<option value="' + camps[i].CampaignID + '">' + camps[i].CampaignName + '</option>';
                $("#cam").append(fila);
            }
        }

        function clearList() {
            var camps = $("#cam")[0];
            while (camps.firstChild) {
                camps.removeChild(camps.firstChild);
            }
        }

        $("#Contact").change(
            function () {
                var file = new FileReader();
                file.onload = function () {
                    document.getElementById("mails").textContent = this.result;
                }
                file.readAsText(this.files[0]);
            }
        );
        $("#mails").change(
            function () {
                this.textContent = this.textContent.replace(/  */g, " ").toLowerCase();
            });
    });