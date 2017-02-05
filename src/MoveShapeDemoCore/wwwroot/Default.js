var moveShapeHub = $.connection.moveShapeHub;

var dict;

moveShapeHub.client.updateBallInfo = function (id, ball) {
    var textbox = document.getElementById('textbox');
    var x = ball.left;
    var y = ball.top;

    var circleId = "circle" + id;
    var circleObj = document.getElementById(circleId);
    if (circleObj === null) {
        textbox.value = "new circle id=" + circleId + " left=" + x + " top=" + y;
        addCircle(circleId, x, y);
    }
    else {
        textbox.value = "id=" + circleId + " left=" + x + " top=" + y;
        
        $('#' + circleId).css({ left: x + 'px', top: y + 'px', position: 'absolute' });
    }
}

var addCircle = function (newid, x, y) {
    //$c = $("#circle").clone();
    //$c.css({ left: x + 'px', top: y + 'px', position: 'absolute'});
    //$c.appendTo("#circles");

    var $circleNew = $("#circle0").clone().prop('id', newid);
    $circleNew.appendTo("#circles");
}

$.connection.hub.logging = true;
$.connection.hub.start();
