using Microsoft.AspNetCore.SignalR;
using WebApp.Models;

namespace WebApp.Hubs
{
    public class BattleShipHub : Hub
    {
        public static readonly GameManager _gameManager = new GameManager();

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
                if (!game.Started)
                {
                    // Add the player to the game
                    game.AddPlayer(playerName, Context.ConnectionId);

                    // Join the player to the game group
                    await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

                    // Get the board state for the player
                    var boardState = game.GetBoardStateForPlayer(Context.ConnectionId);

                    Context.Items["RoomCode"] = gameId;

                    int playerCount = game.PlayerCount;

                    // Notify all players in the game that a new player has joined
                    await Clients.Group(gameId).SendAsync("PlayerJoined", playerName, gameId, playerCount);

                    // Sync UI to everyone
                    await SyncGameData(gameId, game.PlayerCount);


                    if (game.PlayerCount == 2)
                    {

                        await SendBoardState(Context.ConnectionId);

                        // Start the game
                        game.StartGame();

                        // Notify all players in the game about the game start
                        await Clients.Group(gameId).SendAsync("GameStarted", game.Player1.Name, game.Player2.Name);
                        await UpdateCurrentPlayer();
                    }
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

            //await Clients.Group(gameId).SendAsync("GameDataSynced", player1Name, player2Name, gameId);
            await Clients.Client(game.Player1.ConnectionId).SendAsync("GameDataSynced", player1Name, player2Name, gameId);
            await Clients.Client(game.Player2.ConnectionId).SendAsync("GameDataSynced", player2Name, player1Name, gameId);
        }

        public async Task SendBoardState(string connectionId)
        {
            var gameId = Context.Items["RoomCode"].ToString();
            var game = _gameManager.GetGame(gameId);

            if (game != null)
            {
                var (isCurrentPlayer, defenseBoard, attackBoard) = game.GetBoardStateForPlayer(connectionId);

                int[][] safeDefenseBoard = defenseBoard.Select(row => row.Select(value => value == 2 ? 0 : value).ToArray()).ToArray();
                int[][] safeAttackBoard = attackBoard.Select(row => row.Select(value => value == 2 ? 0 : value).ToArray()).ToArray();

                if (game.Player1 == game.CurrentPlayer)
                {
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("UpdateBoardState", defenseBoard, safeAttackBoard);
                    await Clients.Client(game.Player2.ConnectionId).SendAsync("UpdateBoardState", attackBoard, safeDefenseBoard);
                }
                else
                {
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("UpdateBoardState", attackBoard, safeDefenseBoard);
                    await Clients.Client(game.Player2.ConnectionId).SendAsync("UpdateBoardState", defenseBoard, safeAttackBoard);
                }
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Game not found");
            }
        }


        public async Task Shoot(int row, int col)
        {
            // Get the game ID from the connection ID
            var gameId = Context.Items["RoomCode"].ToString();
            var game = _gameManager.GetGame(gameId);

            if (game is not null && game.CurrentPlayer is not null)
            {
                // If the game is over, notify all players
                if (!game.IsGameOver)
                {
                    // Ensure the player making the shot is the current player
                    if (game.CurrentPlayer.ConnectionId == Context.ConnectionId)
                    {

                        Player shooter = game.CurrentPlayer;

                        // Attempt to make a shot on the game board
                        Game.MoveStates moveResult = game.Shoot(shooter, row, col);

                        if (moveResult != Game.MoveStates.Illegal)
                        {
                            await SendBoardState(Context.ConnectionId);


                            if (moveResult == Game.MoveStates.Hit)
                            {
                                return;
                            }

                            game.SwitchPlayer();
                            await UpdateCurrentPlayer();
                        }

                        if (moveResult == Game.MoveStates.GameOver)
                        {
                            await Clients.Group(gameId).SendAsync("GameOver", game.GetWinner());
                        }
                    }
                    else
                    {
                        string otherPlayerConnectionId = game.GetOtherPlayerConnectionId(game.CurrentPlayer.ConnectionId);

                        await Clients.Client(otherPlayerConnectionId).SendAsync("NotYourTurn");
                    }
                }
                else
                {
                    await Clients.Group(gameId).SendAsync("GameOver", game.GetWinner());
                }
            }
        }

        public async Task UpdateCurrentPlayer()
        {
            var gameId = Context.Items["RoomCode"].ToString();
            var game = _gameManager.GetGame(gameId);
            var board1 = 1;
            var board2 = 2;
            if (game.CurrentPlayer == game.Player1)
            {
                await Clients.Client(game.Player2.ConnectionId).SendAsync("UpdateTurn", game.CurrentPlayer.Name, board1, board2);
                await Clients.Client(game.Player1.ConnectionId).SendAsync("UpdateTurn", game.CurrentPlayer.Name, board2, board1);
            }
            else
            {
                await Clients.Client(game.Player1.ConnectionId).SendAsync("UpdateTurn", game.CurrentPlayer.Name, board1, board2);
                await Clients.Client(game.Player2.ConnectionId).SendAsync("UpdateTurn", game.CurrentPlayer.Name, board2, board1);
            }
        }
    }
}