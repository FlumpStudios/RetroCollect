
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



    //Pagination
    $('.pagination > li').click(
        
        function () {            
            event.preventDefault()
            $('#Page').val(this.innerText);
            $('#game-form').submit();
        });
    

    //Reset page to 1
    $('.button_link').click(
        function () {
          
         
            $('#Page').val(1);          
        
        });


    //Sorting
    $('.sorting-headers').click(function (data) {
        
        var sortSwitch = $('#switchsort').val();

        //Check if the current orderby option matches the one clicked, if it does match then reverse swtich sort.
        //This is to allow switching between ascending and decending sorting.
        if ($('#CurrentOrderBy').val() === this.id) {          
            if (sortSwitch === 'true' || sortSwitch === true) sortSwitch = false;
            else sortSwitch = true;
        }
        else
        {
            sortSwitch = false;
        }

        $('#Page').val(1);
        $('#sortingOptions').val(this.id);
        $('#switchsort').val(sortSwitch);
        $('#game-form').submit();
    });


});
