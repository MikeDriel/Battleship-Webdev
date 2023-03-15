// Establish a connection to the SignalR hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/battleshipHub")
    .build();

// Listen for the "BoardUpdated" event from the hub
connection.on("BoardUpdated", (playerName, boardState) => {
    // Determine which player's board to update
    const boardId = playerName === "player1" ? "player1-board" : "player2-board";

    // Update the board state
    const board = document.querySelector(boardId);
    board.updateState(boardState);
});