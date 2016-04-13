$(function () {
    //Initial Setup
    Init();

    //Create reference to the hub
    var conn = $.connection.sigHub;

    //-----Connection Functions------//
    //-----Update from server--------//
    conn.client.sendmessage = function (username, message) //send chat messages
    {
        var encodedName = $('<div />').text(username).html();
        var encodedMsg = $('<div />').text(message).html();
        // Add the message to the page. 
        $('#discussion').append('<li><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');

        //Force chat window to scroll down on new content
        var out = $('.message-frame');
        // allow 1px inaccuracy by adding 1
        var isScrolledToBottom = out.scrollHeight - out.clientHeight <= out.scrollTop + 1;
        if (isScrolledToBottom)
            out.scrollTop = out.scrollHeight - out.clientHeight;
    }

    conn.client.redraw = function (cellID, color) //Update table cells
    {
        $('#' + cellID).css('background-color', color);
    }

    //----------Start Connection-----------//
    //-----------Push new data TO server---//
    $.connection.hub.start().done(function () {
        //Login
        $(function () {
            conn.server.login($('#displayname').val());
        });

        $(function () {
            conn.server.synchronize('A1','blue');
        })


        //Bind Dragover for color application
        $('th').bind('dragenter', function (ev) {
            conn.server.redraw(ev.target.id, $('#colorHex').val());
        });
        $('th').bind('dragstart', function (ev) {
            conn.server.redraw(ev.target.id, $('#colorHex').val());
        });

        $(window).unload(function () {
            conn.server.logoout($('#displayname').val());
        })



        //Chat
        $('#sendmessage').click(function () {
            // Call the SendMessage method on the hub. 
            conn.server.send($('#displayname').val(), $('#message').val());

            // Clear text box and reset focus for next comment. 
            $('#message').val('').focus();
        });

        //Draw
        $('th').click(function (ev) {
            var cellID = ev.target.id;
            var color = $('#colorHex').val();

            conn.server.redraw(cellID, color);
        });


    });

});

//Functions outside of connection
function Init() {

    //populate the board
    $('#drawBoard').html(createBoard());
    //Get username from user
    var user = prompt("Please enter a username");

    while (user === "" || user === null)
    {
        user = prompt('Please Enter a Username:')
    }
    $('#displayname').val(user);

    //Set focus on message board
    $('#message').focus();

    //Bind Enter Key to Button Press
    $(document).keypress(function (e) {
        if(e.which == 13)
        {
            $('#sendmessage').click();
        }
    });

    
}

function createBoard() {
    var id = 1;
    var tableHTML = '<table><tbody><tr>';
    var letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'W', 'X', 'Y', 'Z','AA','AB','AC','AD','AE','AF','AG','AH','AJ','AK'];

    letters.forEach(function(element)
    {
        tableHTML += '<th>' + element + '</th>';
    })

    tableHTML += '</tr>';
    for (var r = 1; r <= 22; r++) {

        tableHTML += '<tr><th>' + r + '</th>';

        //First Row and column are Headers
        for (var i = 0; i < letters.length-1; i++) {
            tableHTML += '<th id="' + letters[i] + id + '" class=""></th>';
        }

        tableHTML += '</tr>';
        id++;

    }

    return tableHTML;
}