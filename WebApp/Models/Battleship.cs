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

        public List<Ship> Ships1 { get; set; }
        public List<Ship> Ships2 { get; set; }


        public bool Started { get; set; }

        public Game(string gameId)
        {
            GameId = gameId;
            InitializeBoards();
        }

        private void InitializeBoards()
        {
            // Initialize the boards and ships for both players
            (Board1, Ships1) = GenerateInitialBoard();
            (Board2, Ships2) = GenerateInitialBoard();
        }

        private (int[][], List<Ship>) GenerateInitialBoard()
        {
            (string type, int length)[] shipData = new (string, int)[] {
                ("Carrier", 5),
                ("Battleship", 4),
                ("Cruiser", 3),
                ("Submarine", 3),
                ("Destroyer", 2)
             }; 

            List<Ship> ships = new List<Ship>();
            int[][] board = new int[10][];

            // Initialize the board with empty cells
            for (int i = 0; i < 10; i++)
            {
                board[i] = new int[10];
            }

            Random random = new Random();

            foreach (var shipInfo in shipData)
            {
                string shipType = shipInfo.type;
                int shipSize = shipInfo.length;

                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    int col = random.Next(10);
                    int row = random.Next(10);
                    string orientation = random.Next(2) == 0 ? "horizontal" : "vertical";

                    bool canPlace = true;

                    for (int i = 0; i < shipSize; i++)
                    {
                        int currentCol = orientation == "horizontal" ? col + i : col;
                        int currentRow = orientation == "vertical" ? row + i : row;

                        if (!IsCellEmptyAndNotAdjacentToShips(board, currentRow, currentCol))
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    if (canPlace)
                    {
                        Ship ship = new Ship(shipType, shipSize, orientation, (row, col));
                        ships.Add(ship);

                        for (int i = 0; i < shipSize; i++)
                        {
                            int currentCol = orientation == "horizontal" ? col + i : col;
                            int currentRow = orientation == "vertical" ? row + i : row;
                            board[currentRow][currentCol] = 2;
                        }
                        shipPlaced = true;
                    }
                }
            }

            return (board, ships);
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

        public MoveStates Shoot(Player shooter, int row, int col)
        {
            const int EmptyCell = 0;
            const int MissedCell = 1;
            const int ShipCell = 2;
            const int HitShipCell = 3;

            List<Ship> targetShips = shooter == Player1 ? Ships2 : Ships1;
            int[][] targetBoard = shooter == Player1 ? Board2 : Board1;
            int cellValue = targetBoard[row][col];

            if (cellValue == EmptyCell)
            {
                targetBoard[row][col] = MissedCell;

                return MoveStates.Miss;
            }
            else if (cellValue == ShipCell)
            {
                targetBoard[row][col] = HitShipCell;
                UpdateShipHitState(targetShips, row, col);
                Ship ship = targetShips.FirstOrDefault(s => s.ContainsCoordinate(row, col));

                if (ship.IsSunk())
                {
                    if (IsGameOver = targetShips.All(s => s.IsSunk()))
                    {
                        return MoveStates.GameOver;
                    }
                    return MoveStates.Sunk;
                }

                return MoveStates.Hit;
            }

            return MoveStates.Illegal;
        }

        public enum MoveStates
        {
            Hit,
            Miss,
            Sunk,
            GameOver,
            Illegal
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


        private void UpdateShipHitState(List<Ship> ships, int row, int col)
        {
            foreach (var ship in ships)
            {
                if (ship.ContainsCoordinate(row, col))
                {
                    ship.Hit();
                    break;
                }
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

        public string GetOtherPlayerConnectionId(string currentPlayerConnectionId)
        {
            if (Player1.ConnectionId == currentPlayerConnectionId)
            {
                return Player2.ConnectionId;
            }
            else if (Player2.ConnectionId == currentPlayerConnectionId)
            {
                return Player1.ConnectionId;
            }
            return null;
        }


        public string GetWinner()
        {
            bool allShipsSunkPlayer1 = Ships1.All(ship => ship.IsSunk());
            bool allShipsSunkPlayer2 = Ships2.All(ship => ship.IsSunk());

            if (allShipsSunkPlayer1)
            {
                return Player2.Name;
            }
            else if (allShipsSunkPlayer2)
            {
                return Player1.Name;
            }

            return null;
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

        public bool ContainsCoordinate(int row, int col)
        {
            for (int i = 0; i < Length; i++)
            {
                int currentRow = Orientation == "vertical" ? Position.row + i : Position.row;
                int currentCol = Orientation == "horizontal" ? Position.col + i : Position.col;

                if (row == currentRow && col == currentCol)
                {
                    return true;
                }
            }

            return false;
        }
    }

}