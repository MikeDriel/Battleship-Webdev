using System.Text;

namespace WebApp.Models
{
    public class Game
    {
        private List<Player> _players = new List<Player>();
        public string GameId { get; set; }
        public bool IsGameOver { get; set; }

        public Player Player1 => _players[0];
        public Player Player2 => _players[1];

        public Player CurrentPlayer { get; set; }

        public int PlayerCount => _players.Count;

        public int[][] Board1 { get; set; }
        public int[][] Board2 { get; set; }

        public Game(string gameId)
        {
            GameId = gameId;
            Board1 = CreateBoard();
            Board2 = CreateBoard();
        }

        public void AddPlayer(string playerName, string connectionId)
        {
            // Create a new player object and add it to the list of players
            var player = new Player(playerName, connectionId);
            _players.Add(player);
        }

        public void StartGame()
        {
            // Randomly select a player to start the game
            var random = new Random();
            CurrentPlayer = random.Next(0, 2) == 0 ? Player1 : Player2;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
        }

        public ShotResult MakeMove(int row, int col)
        {
            int[][] board = CurrentPlayer == Player1 ? Board2 : Board1;
            ShotResult result;

            if (board[row][col] == 0)
            {
                board[row][col] = +1;
                result = ShotResult.Miss;
            }
            else if (board[row][col] > 0)
            {
                board[row][col] = +2;
                result = ShotResult.Hit;
            }
            else
            {
                result = ShotResult.Invalid;
            }

            UpdateGameState();

            return result;
        }

        private void UpdateGameState()
        {
            if (IsBoardDestroyed(Board1) || IsBoardDestroyed(Board2))
            {
                IsGameOver = true;
            }
        }

        private bool IsBoardDestroyed(int[][] board)
        {
            foreach (int[] row in board)
            {
                foreach (int cell in row)
                {
                    if (cell > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public string GetWinner()
        {
            if (IsBoardDestroyed(Board1))
            {
                return Player2.Name;
            }
            else if (IsBoardDestroyed(Board2))
            {
                return Player1.Name;
            }
            else
            {
                return null;
            }
        }

        public int[][] CreateBoard()
        {
            return new int[10][]
            {
                new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
        }

        public enum ShotResult
        {
            Invalid = 0,
            Miss = 1,
            Hit = 2
        }
    }

    public class PlayerManager
    {
        private List<Player> _players = new List<Player>();

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public Player GetPlayer(string connectionId)
        {
            return _players.FirstOrDefault(p => p.ConnectionId == connectionId);
        }
    }

    public class Player
    {
        public string Name { get; }
        public string ConnectionId { get; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Player(string name, string connectionId)
        {
            Name = name;
            ConnectionId = connectionId;
        }
    }

    //KEEPS TRACK OF RUNNING GAMES
    public class GameManager
    {
        private List<Game> _games = new List<Game>();
        private static Random _random = new Random();

        public Game GetGame(string gameId)
        {
            return _games.FirstOrDefault(g => g.GameId == gameId);
        }

        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        public void RemoveGame(Game game)
        {
            _games.Remove(game);
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[_random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}