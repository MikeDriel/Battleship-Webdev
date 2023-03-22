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

// Listen for the "GameJoined" event from the hub
connection.on("PlayerJoined", (playerName, gameId) => {
    // Update the UI to show that a player has joined the game
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
    const BoardElementInActive = document.querySelector(`player${inactiveboard}-board`);
    BoardElementInActive.classList.add("inactiveopacity");
});


// Listen for the "GameStarted" event from the hub
connection.on("GameStarted", () => {
    // Update the UI to indicate the game has started
    console.log("Game has started");
});

// Listen for the "GameEnded" event from the hub
connection.on("GameOver", (winnerName) => {
    // Update the UI to show the winner of the game
    console.log(`Game has ended. The winner is: ${winnerName}`);
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
    connection.invoke("JoinGame", playerName, roomCode).catch(function (err) {
        return console.error(err.toString());
    });
});

document.getElementById("createButton").addEventListener("click", function (e) {
    connection.invoke("CreateGame", document.getElementById("playerName").value).catch(function (err) {
        return console.error(err.toString());
    });
});