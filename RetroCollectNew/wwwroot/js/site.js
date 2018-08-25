
//TODO: Move into seperate JS file

$(document).ready(function () {
    $('.add-new-game').click(function (data) {
        var clientListModel = { gameId: this.id };
        console.log(clientListModel);

        $.post("/ClientGamesList/Create", clientListModel, function (data, status) {
            console.log(data);
            console.log(status);
            if (status === "success") { alert("Game added to your library") }
            else { alert("There was an error adding game to your library") };
        });
    });


    $('.sorting-headers').click(function (data) {

        var sortSwitch = $('#switchsort').val();
        if (sortSwitch) sortSwitch = false;
        else sortSwitch = true;

        $('#sortingOptions').val(this.id);
        $('#switchsort').val(sortSwitch);
        $('#game-form').submit();
    });
});
