
//TODO: Move into seperate JS file

$(document).ready(function () {

    //Add new game
    $('.add-new-game').click(function (data) {
        var clientListModel = { gameId: this.id };
        
        $.post("/ClientGamesList/Create", clientListModel, function (data, status) {
            if (status === "success") { alert("Game added to your library") }
            else { alert("There was an error adding game to your library") };
        });
    });


    //Sorting
    $('.sorting-headers').click(function (data) {

        var sortSwitch = $('#switchsort').val();
        if (sortSwitch == 'true' || sortSwitch == true) sortSwitch = false;
        else sortSwitch = true;

        $('#sortingOptions').val(this.id);
        $('#switchsort').val(sortSwitch);

        $('#game-form').submit();
    });


});
