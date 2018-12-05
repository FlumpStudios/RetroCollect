﻿/*********************
 **  Event Methods  **
 *********************/

//$("#gameDescription").ready(function () {
//    alert('');
//    getGameDescription("resident-evil");
//});

$(document).ready(function () {
    $('.add-new-game').click(function () {
        handleAddNewGame(this.id);
    });
    $('.delete-game').click(function () {
        handleDeleteGame(this.id);
    });
    $('.pagination > li').click(function () {
        handlePagination(this.innerText);
    });
    $('.sorting-headers').click(function () {
        handleSorting(this.id);
    });


    //Select display between all games and personal Collection
    $('#show-all-games').click(function () {
        $('#Page').val(1);
        $('#showClientList').val(false);
        $('#game-form').submit();
    });

    $('#show-my-collection').click(function () {
        $('#Page').val(1);
        $('#showClientList').val(true);
        $('#game-form').submit();
    });


    $('.console-selection').click(
        function () {
            //If all has been selection send through null to back end
            if (this.id.toUpperCase() === "ALL") $('#Format').val(null);
            else $('#Format').val(this.id);

            //Reset paging to page 1
            $('#Page').val(1);
        });
});



/*********************
 ** Event Handlers  **
 *********************/

function handleAddNewGame(id) {
    var clientListModel = {
        gameId: id
    };

    $.ajax({
        url: '/ClientGamesList/Create',
        type: 'POST',
        data: clientListModel,
        success: function (result) {
            $.notify(result, "success");
        },
        error: function (e) {
            $.notify(e.responseText, "error");
        }
    });
}

function handleDeleteGame(id) {
    $.ajax({
        url: '/ClientGamesList/Delete/' + id,
        type: 'DELETE',
        success: function (result) {
            document.location.reload();
        },
        error: function (e) {
            $.notify("There has been an error deleting the title from your library, please contact administration for help.", "error");
        }
    });
}

function getGameDescription(gameName) {
    $.ajax({
        headers: {
            "user-key": "e71b082e22dee3f92e0ccd22c7b2fc4c",
            Accept: "application/json"
        },
        url: 'https://api-endpoint.igdb.com/games/' + gameName,
        type: 'get',
        success: function (result) {
            console.log(result);
        },
        error: function (e) {
            $.notify("There has been an error retriving description.", "error");
        }
    });
}


function handlePagination(pageSelection) {
    var currentPage = $('#Page').val();

    event.preventDefault();

    if (pageSelection === ">") {
        currentPage++;
        $('#Page').val(currentPage);
    } else if (pageSelection === "<") {
        currentPage--;
        $('#Page').val(currentPage);
    } else if (pageSelection === "<<") {
        currentPage = 1;
        $('#Page').val(currentPage);
    } else if (pageSelection === ">>") {
        currentPage = $('#LastPage').val();
        $('#Page').val(currentPage);
    } else {
        if (checkIfNumber(pageSelection)) $('#Page').val(pageSelection);
        else {
            $.notify("Invalid pagination selections", "error");
            return;
        }
    }
    $('#game-form').submit();
}

function handleSorting(id) {
    var sortSwitch = $('#switchsort').val();

    //Check if the current orderby option matches the one clicked, if it does match then reverse swtich sort.
    //This is to allow switching between ascending and decending sorting.
    if ($('#CurrentOrderBy').val() === id) {
        if (sortSwitch === 'true' || sortSwitch === true) sortSwitch = false;
        else sortSwitch = true;
    } else {
        sortSwitch = false;
    }

    $('#SortingOptions').val(id);
    $('#switchsort').val(sortSwitch);
    $('#game-form').submit();
}



/***********************
 **  Helper Functions **
 ***********************/

function checkIfNumber(x) {
    return !isNaN(x);
}