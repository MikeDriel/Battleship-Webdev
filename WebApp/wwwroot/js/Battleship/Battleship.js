// Establish a connection to the SignalR hub
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/BattleShipHub")
    .build();

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});


connection.on("GameCreated", (gameId) => {
    console.log(gameId);
    document.getElementById("lobbyscreen").remove();
    document.getElementById("playfield").classList.add("visible");
    document.getElementById("gameRoomCode").classList.add("visible");

    document.getElementById("gameRoomCode").innerHTML = `Roomcode: ${gameId}`;

    document.getElementById("player1name").innerHTML = document.getElementById("playerName").value;
});


connection.on("PlayerJoined", (playerName, gameId) => {

    document.getElementById("lobbyscreen").remove();
    document.getElementById("playfield").classList.add("visible");
    document.getElementById("gameRoomCode").classList.add("visible");
    console.log(`${playerName} has joined the game with ID: ${gameId}`);
});


connection.on("UpdateBoardState", (defenseBoard, attackBoard) => {
    const player1BoardElement = document.querySelector('player1-board');
    const player2BoardElement = document.querySelector('player2-board');

    player1BoardElement.updateBoard(defenseBoard);

    player2BoardElement.updateBoard(attackBoard);

});

connection.on("UpdateTurn", (player, activeboard, inactiveboard) => {
    const currentTurnName = document.getElementById("currentTurnName");
    currentTurnName.innerHTML = (`It is ${player}'s turn!`);

    const BoardElementActive = document.querySelector(`player${activeboard}-board`);
    BoardElementActive.classList.remove("inactiveopacity");
    BoardElementActive.classList.remove("nopointerevents");
    const BoardElementInActive = document.querySelector(`player${inactiveboard}-board`);
    BoardElementInActive.classList.add("inactiveopacity");
    BoardElementInActive.classList.add("nopointerevents");
});


connection.on("GameStarted", () => {

    console.log("Game has started");
});


connection.on("NotYourTurn", () => {

    alert("It is not your turn!");

});


connection.on("GameOver", (winnerName) => {

    console.log(`Game has ended. The winner is: ${winnerName}`);

    document.getElementById("playfield").remove();
    document.getElementById("gameRoomCode").remove();
    document.getElementById("gameOver").innerHTML = `Game Over! ${winnerName} has won!`;
    document.getElementById("gameOver").classList.add("visible");
});


connection.on("GameDataSynced", (player1Name, player2Name, gameId) => {
    document.getElementById("player1name").innerHTML = player1Name;
    document.getElementById("player2name").innerHTML = player2Name;
    document.getElementById("gameRoomCode").innerHTML = `Roomcode: ${gameId}`;
});


//adds a click event listener:
document.getElementById("joinButton").addEventListener("click", function () {
    const playerName = document.getElementById("playerName").value;
    const roomCode = document.getElementById("roomCode").value;

    if (playerName.length >= 2 && playerName.length <= 13) {
        connection.invoke("JoinGame", playerName, roomCode).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else alert("Name is too long or is empty")
});


document.getElementById("createButton").addEventListener("click", function (e) {

    const playerName = document.getElementById("playerName").value

    if (playerName.length >= 2 && playerName.length <= 13) {
        connection.invoke("CreateGame", playerName).catch(function (err) {
            return console.error(err.toString());
        });
    }

    else alert("Name is too long or is empty")
});