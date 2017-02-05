var moveShapeHub = $.connection.moveShapeHub;

moveShapeHub.client.updateBallInfo = function (id, ball) {
    var textbox = document.getElementById('textbox');
    var x = ball.left;
    var y = ball.top;
    var radius = ball.radius;

    var circleId = "circle" + id;
    var circleObj = document.getElementById(circleId);
    if (circleObj === null) {
        textbox.value = "new circle id=" + circleId + " left=" + x + " top=" + y;
        addCircle(circleId, x, y);
    }
    else {
        var d = radius * 2;
        $('#' + circleId).css({ left: x + 'px', top: y + 'px', position: 'absolute', width:d+'px', height:d+'px' });
    }
}

moveShapeHub.client.removeBall = function (id)
{
    var circleId = "circle" + id;
    textbox.value = "remove circle id=" + circleId;
    $('#' + circleId).remove();
}

var addCircle = function (newid, x, y) {
    //var $circleNew = $("#circle0").clone().prop('id', newid);
    //$circleNew.appendTo("#circles");

    var $newdiv = $("<div id='" + newid + "' class='circle'></div>");
    $newdiv.css({ left: x + 'px', top: y + 'px' }).appendTo("#circles");
}

$.connection.hub.logging = true;
$.connection.hub.start();
