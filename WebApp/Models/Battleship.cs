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

        public bool Started { get; set; }

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
                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    int col = random.Next(10);
                    int row = random.Next(10);

                   
                    int orientation = random.Next(2);

                    bool canPlace = true;

                    for (int i = 0; i < shipSize; i++)
                    {
                        int currentCol = orientation == 0 ? col + i : col;
                        int currentRow = orientation == 1 ? row + i : row;

                        
                        if (!IsCellEmptyAndNotAdjacentToShips(board, currentRow, currentCol))
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    if (canPlace)
                    {
                        for (int i = 0; i < shipSize; i++)
                        {
                            int currentCol = orientation == 0 ? col + i : col;
                            int currentRow = orientation == 1 ? row + i : row;
                            board[currentRow][currentCol] = 2;
                        }
                        shipPlaced = true;
                    }
                }
            }

            return board;
        }

        private bool IsCellEmptyAndNotAdjacentToShips(int[][] board, int row, int col)
        {
            if (row < 0 || col < 0 || row >= board.Length || col >= board[row].Length || board[row][col] != 0)
            {
                return false;
            }

            // This is how this part works
            // (-1, -1) (-1, 0) (-1, 1)
            // (0, -1)(row, col)(0, 1)
            // (1, -1) (1, 0) (1, 1)

            int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < rowOffsets.Length; i++)
            {
                int newRow = row + rowOffsets[i];
                int newCol = col + colOffsets[i];

                if (newRow >= 0 && newCol >= 0 && newRow < board.Length && newCol < board[newRow].Length && board[newRow][newCol] != 0)
                {
                    return false;
                }
            }

            return true;
        }




        public bool Shoot(Player shooter, int row, int col)
        {
            const int EmptyCell = 0;
            const int MissedCell = 1;
            const int ShipCell = 2;
            const int HitShipCell = 3;

            int[][] targetBoard = shooter == Player1 ? Board2 : Board1;
            int cellValue = targetBoard[row][col];

            if (cellValue == ShipCell)
            {
                targetBoard[row][col] = HitShipCell;
                bool gameOver = IsBoardDestroyed(targetBoard);

                if (gameOver)
                {
                    IsGameOver = true;
                }

                return (true);
            }
            else if (cellValue == EmptyCell)
            {
                targetBoard[row][col] = MissedCell;
            }

            return (false);
        }

        public (bool IsCurrentPlayer, int[][] DefenseBoard, int[][] AttackBoard) GetBoardStateForPlayer(string connectionId)
        {
            if (Player1.ConnectionId == connectionId)
            {
                return (true, Board1, Board2);
            }
            else if (Player2.ConnectionId == connectionId)
            {
                return (true, Board2, Board1);
            }
            else
            {
                return (false, null, null);
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

            if (random.Next(0, 2) == 0)
            {
                CurrentPlayer = Player1;
            }
            else
            {
                CurrentPlayer = Player2;
            }

            Started = true;
        }

        public void SwitchPlayer()
        {
            if (CurrentPlayer == Player1)
            {
                CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
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

        public string GetPlayerName(string connectionId)
        {
            return _players.FirstOrDefault(x => x.ConnectionId == connectionId).Name;
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