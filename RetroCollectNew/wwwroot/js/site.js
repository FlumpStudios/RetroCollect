
//TODO: Move into seperate JS file

$(document).ready(function () {

    //Add new game
    $('.add-new-game').click(function () {addNewGame(this.id)});

    //Pagination
    $('.pagination > li').click(function () {paginationController(this.innerText) });
    

    //Sorting
    $('.sorting-headers').click(function () { sotringHeaderController(this.id) });


    $('.console-selection').click(
        function () {
            $('#Format').val(this.id);
            $('#Page').val(1);        
        });



});


function checkIfNumber(x)
{
    return (!isNaN(x))
}

function addNewGame(id) {
    var clientListModel = { gameId: id };

    $.post("/ClientGamesList/Create", clientListModel, function (data, status) {
        if (status === "success") { alert("Game added to your library") }
        else { alert("There was an error adding game to your library") };
    });
};

function paginationController(pageSelection)
{
        var currentPage = $('#Page').val();

        event.preventDefault()

        if (pageSelection == ">") {
            currentPage++;
            $('#Page').val(currentPage);
        }

        else if (pageSelection == "<") {
            currentPage--;
            $('#Page').val(currentPage);
        }
        else if (pageSelection == "<<") {
            currentPage = 1;
            $('#Page').val(currentPage);
        }
        else if (pageSelection == ">>") {
            currentPage = $('#LastPage').val();
            $('#Page').val(currentPage);
        }
        else {
            if (checkIfNumber(pageSelection)) $('#Page').val(pageSelection);
            else {
                console.log("Invalid pagination selections");
                return
            }
        }
        $('#game-form').submit();
}

function sotringHeaderController(id) {
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
