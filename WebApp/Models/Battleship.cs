using System.Text;

namespace WebApp.Models
{
    public class Game
    {
        public List<Player> _players = new List<Player>();
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
            InitializeBoards();
        }

        private void InitializeBoards()
        {
            // Initialize the boards for both players
            Board1 = GenerateInitialBoard();
            Board2 = GenerateInitialBoard();
        }

        private int[][] GenerateInitialBoard()
        {
            int[] shipSizes = new int[] { 5, 4, 3, 3, 2 }; // Sizes of the ships to place on the board
            int[][] board = new int[10][];

            // Initialize the board with empty cells
            for (int i = 0; i < 10; i++)
            {
                board[i] = new int[10];
            }

            Random random = new Random();

            foreach (int shipSize in shipSizes)
            {
                bool placed = false;

                while (!placed)
                {
                    int row = random.Next(0, 10);
                    int col = random.Next(0, 10);
                    bool isHorizontal = random.Next(0, 2) == 0;

                    if (CanPlaceShip(board, row, col, shipSize, isHorizontal))
                    {
                        PlaceShip(board, row, col, shipSize, isHorizontal);
                        placed = true;
                    }
                }
            }

            return board;
        }


        //Ik wil niet meer HOURS WASTED vanaf dit punt = 16.5
        private bool CanPlaceShip(int[][] board, int row, int col, int shipSize, bool isHorizontal)
        {
            if (isHorizontal)
            {
                if (col + shipSize > 10) return false;

                for (int i = 0; i < shipSize; i++)
                {
                    if (board[row][col + i] != 0) return false;
                }
            }
            else
            {
                if (row + shipSize > 10) return false;

                for (int i = 0; i < shipSize; i++)
                {
                    if (board[row + i][col] != 0) return false;
                }
            }

            return true;
        }


        // 2 = a ship cell
        private void PlaceShip(int[][] board, int row, int col, int shipSize, bool isHorizontal)
        {
            if (isHorizontal)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    board[row][col + i] = 2;
                }
            }
            else
            {
                for (int i = 0; i < shipSize; i++)
                {
                    board[row + i][col] = 2;
                }
            }
        }

        public (bool IsCurrentPlayer, int[][] Board) GetBoardStateForPlayer(string connectionId)
        {
            if (Player1.ConnectionId == connectionId)
            {
                return (true, Board1);
            }
            else if (Player2.ConnectionId == connectionId)
            {
                return (true, Board2);
            }
            else
            {
                return (false, null);
            }
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

        public string GetPlayerName(string connectionId)
        {
            return _players.FirstOrDefault(x => x.ConnectionId == connectionId).Name;
        }

        public (bool hit, bool gameOver) Shoot(Player shooter, int row, int col)
        {
            Player target = shooter == Player1 ? Player2 : Player1;
            int[][] targetBoard = shooter == Player1 ? Board2 : Board1;

            if (targetBoard[row][col] == 2) // 2 represents a ship cell
            {
                targetBoard[row][col] = 3; // 3 represents a hit ship cell
                bool gameOver = IsBoardDestroyed(targetBoard);

                if (gameOver)
                {
                    IsGameOver = true;
                }

                return (true, gameOver);
            }
            else if (targetBoard[row][col] == 0) // 0 represents an empty cell
            {
                targetBoard[row][col] = 1; // 1 represents a miss cell
                return (false, false);
            }
            else
            {
                return (false, false);
            }
        }

        public int[][] GetBoardState(Player player)
        {
            return player == Player1 ? Board1 : Board2;
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

    public class Ship
    {
        public string Type { get; }
        public int Length { get; }
        public string Orientation { get; set; }
        public (int row, int col) Position { get; set; }
        public int Hits { get; private set; }

        public Ship(string type, int length, string orientation, (int row, int col) position)
        {
            Type = type;
            Length = length;
            Orientation = orientation;
            Position = position;
            Hits = 0;
        }

        public bool IsSunk()
        {
            return Hits == Length;
        }

        public void Hit()
        {
            Hits++;
        }
    }
}