using Microsoft.AspNetCore.SignalR;
using WebApp.Models;

namespace WebApp.Hubs
{
    public class BattleShipHub : Hub
    {

        private readonly GameManager _gameManager;

        public BattleShipHub(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        public async Task<string> CreateGame(string playerName)
        {
            // Generate a unique game ID
            string gameId = _gameManager.GenerateRandomString(5);

            // Create a new game object
            var game = new Game(gameId);

            // Add the game to the list of active games
            _gameManager.AddGame(game);

            // Join the player to the game group
            await JoinGame(playerName, gameId);

            // Send a message to the player with the game ID
            await Clients.Group(gameId).SendAsync("GameCreated", gameId);

            return gameId;
        }

        public async Task JoinGame(string playerName, string gameId)
        {
            // Get the game from the list of active games
            var game = _gameManager.GetGame(gameId);

            if (game != null)
            {
                // Add the player to the game
                game.AddPlayer(playerName, Context.ConnectionId);

                // Join the player to the game group
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

                int playerCount = game.PlayerCount;

                // Notify all players in the game that a new player has joined
                await Clients.Group(gameId).SendAsync("PlayerJoined", playerName, gameId, playerCount);

                await SyncGameData(gameId, game.PlayerCount);


                if (game.PlayerCount == 2)
                {
                    // Start the game
                    game.StartGame();
                    // Notify all players in the game about the game start
                    await Clients.Group(gameId).SendAsync("GameStarted", game.Player1.Name, game.Player2.Name);
                }
            }
            else
            {
                await Clients.Caller.SendAsync("JoinGameError", "Game not found");
            }
        }

        public async Task SyncGameData(string gameId, int whoToSync)
        {
            var game = _gameManager.GetGame(gameId);

            string player1Name = "";
            string player2Name = "";


            if (whoToSync == 1)
            {
                player1Name = game.Player1.Name;
                player2Name = "waiting for player";
            }
            if (whoToSync == 2)
            {
                player1Name = game.Player1.Name;
                player2Name = game.Player2.Name;
            }

            await Clients.Group(gameId).SendAsync("GameDataSynced", player1Name, player2Name, gameId);

        }


        public async Task Shoot(string gameId, int row, int col, string playerName)
        {
            // Get the game from the list of active games
            var game = _gameManager.GetGame(gameId);

            if (game != null)
            {
                // Check if the player is authorized to make a move
                if (game.CurrentPlayer.Name == playerName)
                {
                    // Make the move and get the result
                    var result = game.MakeMove(row, col);

                    // Notify all players in the game about the move result
                    await Clients.Group(gameId).SendAsync("MoveResult", playerName, row, col, result);

                    // Check if the game is over
                    if (game.IsGameOver)
                    {
                        // Notify all players in the game about the game result
                        var winnerName = game.GetWinner();
                        await Clients.Group(gameId).SendAsync("GameOver", winnerName);

                        // Remove the game from the list of active games
                        _gameManager.RemoveGame(game);
                    }
                    else
                    {
                        // Switch the current player
                        game.SwitchPlayer();
                    }
                }
            }
        }
    }
}