"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

var group = "",
    user = "",
    userId = "";
var painter = "";

var points = 0,
    rounds = 0;

var wordToGuess = "";

var won = false;

//painting
var canvas,
    ctx;

var color = "black",
    brushSize = 2;

var w = 0,
    h = 0;

let coord = { x: 0, y: 0 };
let paint = false;

//functions
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

function ChangeSite(playerRole) {
    if (playerRole == "Painter") {
        $("div.painter").show();
        $("div.guesser").hide();
        $(".your-canvas-wrapper").css("cursor", "auto");
        $(".your-canvas-wrapper").css("pointer-events", "auto");
    }
    else{
        $("div.guesser").show();
        $("div.painter").hide();
        $(".your-canvas-wrapper").css("cursor", "not-allowed");
        $(".your-canvas-wrapper").css("pointer-events", "none");
    }
}

async function GameOver() {
    await connection.invoke("GameOver", document.getElementById("joinGroup").value);

    var dto = {
        "playerId": getCookie("UserId"),
        "isWon": won,
        "points": points,
    };
    
    await $.ajax({
        type: 'POST',
        url: 'https://localhost:5005/Game/Send',
        data: JSON.stringify(dto),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
        },
        error: function (error) {
            alert("error ");
        },
        failure: function (response) {
            alert("failure " );
        }
    });
    
    if (confirm("Press a button!")) {
        var url = "https://localhost:7187/Account/Profile";
        window.location.href = url;
    } else {
        var url = "https://localhost:7187/Account/Profile";
        window.location.href = url;
    }
}

function GetRole(roomId) {
    console.log("called ajax");
    $.ajax({
        type: 'GET',
        url: 'https://localhost:5005/Game/GetRole?id=' + roomId,
        dataType: 'text',
        success: function (data) {
            //ChangeSite(data);
            if (data == "Painter") {
                $("div.painter").show();
                $("div.guesser").hide();
                $(".your-canvas-wrapper").css("cursor", "auto");
                $(".your-canvas-wrapper").css("pointer-events", "auto");
            }
            else {
                $("div.guesser").show();
                $("div.painter").hide();
                $(".your-canvas-wrapper").css("cursor", "not-allowed");
                $(".your-canvas-wrapper").css("pointer-events", "none");
            }
        },
        error: function (xhr, textStatus, error) {
            alert(error);
        },
        failure: function (response) {
            alert("failure " + response.responseText);
        }
    });
}

function GetPainterFinished(roomId) {
    console.log("called GetPainterFinished");
    $.ajax({
        type: 'GET',
        url: 'https://localhost:5005/Game/GetPainterFinished?id=' + roomId,
        dataType: 'text',
        success: function (data) {
            console.log(data);
            if (data == "true") {
                connection.invoke("NextPlayer", group);
                connection.invoke("GetWord", group);
            }
        },
        error: function (xhr, textStatus, error) {
            alert(error);
        },
        failure: function (response) {
            alert("failure " + response.responseText);
        }
    });
}

//init
window.addEventListener('load', function () {
    init();
})

function init() {
    group = document.getElementById("joinGroup").value;
    canvas = document.getElementById('can');
    ctx = canvas.getContext("2d");
    points = 0;
    w = canvas.width;
    h = canvas.height;
    user = getCookie("UserName");
    userId = getCookie("UserId");

    canvas.addEventListener('mousedown', startPainting);
    canvas.addEventListener('mouseup', stopPainting);
    canvas.addEventListener('mousemove', sketch);
}

//events
document.getElementById("guessButton").addEventListener("click", async function (event) {
    var guess = document.getElementById("guessText").value;
    console.log(guess);
    console.log(wordToGuess);
    if (guess == wordToGuess) {
        await connection.invoke("RightGuess", document.getElementById("joinGroup").value, getCookie("UserId"));
        points++;
        document.getElementById("pointsId").value = points;
        document.getElementById("guessText").value = "";
        console.log(points);
        GetPainterFinished(document.getElementById("joinGroup").value);
        connection.invoke("SendMessage", user, "guessed right");
    }
    event.preventDefault();
});

document.getElementById("skipButton").addEventListener("click", function (event) {
    connection.invoke("NextPlayer", group);
    connection.invoke("GetWord", group);
    event.preventDefault();
});

document.getElementById("startGameButton").addEventListener("click", function (event) {
    connection.invoke("StartGame", group);
    event.preventDefault();
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//signalR answers
connection.start().then(function () {
    group = document.getElementById("joinGroup").value;
    connection.invoke("JoinGroup", document.getElementById("joinGroup").value);
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveMessage", function (user, message) {
    const para = document.createElement("p");
    const node = document.createTextNode(`${user}: ${message}`);

    para.appendChild(node);
    document.getElementById("card-body").appendChild(para);
});

connection.on("RecieveWinnerOnGameOver", function (tie, winningPoints, winners) {
    if (winningPoints == points) {
        won = true;
    }
});

connection.on("RecieveRoundOver", function (roundOver) {
    if (roundOver) {
        rounds--;
        document.getElementById("numberOfrounds").value = rounds;
    }
    if (rounds <= 0) {
        GameOver();
    }
    else {
        GetRole(document.getElementById("joinGroup").value);
        erase();
    }
});

connection.on("RecieveWord", function (word) {
    wordToGuess = word;
    console.log(wordToGuess);
    document.getElementById("wordToGuess").value = wordToGuess;
});

connection.on("RecievePlayers", function (players) {
    if (players != null) {
        document.getElementById('PlayersList').value = "";
        document.getElementById('PlayersList').value = players;
    }
});

connection.on("RecieveRounds", function (round) {
    rounds = round;
    console.log(rounds);
    document.getElementById("numberOfrounds").value = rounds;
});

connection.on("GameStarted", async function () {
    await connection.invoke("GetWord", group);
    GetRole(group);
    await connection.invoke("GetGroupsPlayers", group);
    await connection.invoke("GetRounds", group);
    document.getElementById("startGameButton").disabled = true;
    document.getElementById("startGameButton").style.display = "none";
    document.getElementById("start-game-label").style.display = "none";
    document.getElementById("pointsId").value = points.toString();
});

//drawing
connection.on("updateDot", function (prevX, prevY, currX, currY, hubWidth, hubColor, fromGroup) {
    if (fromGroup) {
        ctx.beginPath();
        ctx.lineWidth = hubWidth;
        ctx.lineCap = 'round';
        ctx.strokeStyle = hubColor;
        ctx.moveTo(prevX, prevY);
        ctx.lineTo(currX, currY);
        ctx.stroke();
    }
});

connection.on("clearCanvas", function (fromGroup) {
    if (fromGroup) {
        ctx.clearRect(0, 0, w, h);
        document.getElementById("canvasimg").style.display = "none";
    }
});

function changeColor(obj) {
    if (obj == "white") {
        color = "white";
        document.getElementById("white").style.border = "2px solid cadetblue";
    }
    else {
        color = document.getElementById("colorpicker").value;
        document.getElementById("white").style.border = "2px solid black";
    }
}

function erase() {
    ctx.clearRect(0, 0, w, h);
    connection.invoke("ClearCanvas", group).catch(function (err) {
        return console.error(err.toString());
    });
}

document.getElementById("white").addEventListener("click", function (event) {
    changeColor("white");
});

document.getElementById("colorpicker").addEventListener("change", function (event) {
    changeColor(document.getElementById("colorpicker").value);
});

document.getElementById("brushSize").addEventListener("change", function (event) {
    brushSize = document.getElementById('brushSize').value;
});

document.getElementById("clearButton").addEventListener("click", function (event) {
    var m = confirm("Want to clear");
    if (m) {
        erase();
    }
    event.preventDefault();
});

function getPosition(event) {
    coord.x = event.clientX - canvas.offsetLeft;
    coord.y = event.clientY - canvas.offsetTop;
}

function startPainting(event) {
    paint = true;
    getPosition(event);
}

function stopPainting() {
    paint = false;
}

function sketch(event) {
    if (!paint) return;

    ctx.beginPath();
    ctx.lineWidth = brushSize;
    ctx.lineCap = 'round';
    ctx.strokeStyle = color;
    ctx.moveTo(coord.x, coord.y);

    var prevX = coord.x;
    var prevY = coord.y;

    getPosition(event);

    ctx.lineTo(coord.x, coord.y);
    ctx.stroke();

    connection.invoke("UpdateCanvas", group, prevX, prevY, coord.x, coord.y, parseInt(brushSize), color).catch(function (err) {
        return console.error(err.toString());
    });
}