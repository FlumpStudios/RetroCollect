﻿$(document).ready(function () {

    //Add new game
    $('.add-new-game').click(function () { addNewGame(this.id); });

    $('.delete-game').click(function () { deleteGame(this.id); });

    //Pagination
    $('.pagination > li').click(function () { paginationController(this.innerText); });    

    //Sorting
    $('.sorting-headers').click(function () { SortingHeaderController(this.id);});

    //Select between all games and personal Collection
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


function checkIfNumber(x){
    return !isNaN(x);
}


function addNewGame(id) {
    var clientListModel = { gameId: id };

    $.ajax({
        url: '/ClientGamesList/Create',
        type: 'POST',
        data: clientListModel,
        success: function (result) {
            alert(result);
           
        }, error: function (e) {
            alert(e.responseText);
        }
    });
}


function deleteGame(id) {
    $.ajax({
        url: '/ClientGamesList/Delete/' + id,
        type: 'DELETE',
        success: function (result) {           
            console.log(result);
            alert("Game has been successfully removed from your library");
            document.location.reload();
        }, error: function (e) {
            console.log(e);
            alert("There has been an error deleting the title from your library, please contact administration for help.");
        }
    });
}


function paginationController(pageSelection)
{
        var currentPage = $('#Page').val();

        event.preventDefault();

        if (pageSelection === ">") {
            currentPage++;
            $('#Page').val(currentPage);
        }

        else if (pageSelection === "<") {
            currentPage--;
            $('#Page').val(currentPage);
        }
        else if (pageSelection === "<<") {
            currentPage = 1;
            $('#Page').val(currentPage);
        }
        else if (pageSelection === ">>") {
            currentPage = $('#LastPage').val();
            $('#Page').val(currentPage);
        }
        else {
            if (checkIfNumber(pageSelection)) $('#Page').val(pageSelection);
            else {
                console.log("Invalid pagination selections");
                return;
            }
        }
        $('#game-form').submit();
}

function SortingHeaderController(id) {
    var sortSwitch = $('#switchsort').val();

   
    //Check if the current orderby option matches the one clicked, if it does match then reverse swtich sort.
    //This is to allow switching between ascending and decending sorting.
    if ($('#CurrentOrderBy').val() === id) {
        if (sortSwitch === 'true' || sortSwitch === true) sortSwitch = false;
        else sortSwitch = true;
    }
    else {
        sortSwitch = false;
    }

    $('#SortingOptions').val(id);
    $('#switchsort').val(sortSwitch);
    $('#game-form').submit();
}