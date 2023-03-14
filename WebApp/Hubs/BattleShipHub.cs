using Microsoft.AspNetCore.SignalR;
using WebApp.Models;
using static Azure.Core.HttpHeader;

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
            string gameId = Guid.NewGuid().ToString();

            // Create a new game object
            var game = new Game(gameId);

            // Add the player to the game
            game.AddPlayer(playerName, Context.ConnectionId);

            // Add the game to the list of active games
            _gameManager.AddGame(game);

            // Join the player to the game group
            await JoinGame(gameId, playerName);

            // Send a message to the player with the game ID
            await Clients.Caller.SendAsync("GameCreated", gameId);

            return gameId;
        }


        public async Task JoinGame(string gameId, string playerName)
        {
            // Get the game from the list of active games
            var game = _gameManager.GetGame(gameId);

            if (game != null)
            {
                // Add the player to the game
                game.AddPlayer(playerName, Context.ConnectionId);

                // Join the player to the game group
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

                // Notify all players in the game that a new player has joined
                await Clients.Group(gameId).SendAsync("PlayerJoined", playerName);
            }
            else
            {
                await Clients.Caller.SendAsync("JoinGameError", "Game not found");
            }
        }

        public async Task Shoot(string gameId, int cellId)
        {
            // Get the game from the list of active games
            var game = _gameManager.GetGame(gameId);

            if (game != null)
            {
                // Get the current player's name
                var playerName = game.GetPlayerName(Context.ConnectionId);

                if (playerName != null)
                {

                    // Attempt to make a shot on the game board
                    var result = game.Shoot(playerName, cellId);

                    // Notify all players of the result of the shot
                    await Clients.Group(gameId).SendAsync("ShotResult", playerName, cellId, result);

                    // If the game is over, notify all players
                    if (game.IsGameOver)
                    {
                        await Clients.Group(gameId).SendAsync("GameOver", game.GetWinner());
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("ShootError", "Player not found in game");
                }
            }
            else
            {
                await Clients.Caller.SendAsync("ShootError", "Game not found");
            }
        }
    }
}