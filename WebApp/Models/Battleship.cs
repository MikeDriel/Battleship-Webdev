namespace WebApp.Models
{
    public class Game
    {
        private List<Player> _players = new List<Player>();
        public string GameId { get; }
        public bool IsGameOver { get; internal set; }

        public Game(string gameId)
        {
            GameId = gameId;
        }

        public void AddPlayer(string playerName, string connectionId)
        {
            // Create a new player object and add it to the list of players
            var player = new Player(playerName, connectionId);
            _players.Add(player);
        }

        //TODO
        internal object GetPlayerName(string connectionId)
        {
            throw new NotImplementedException();
        }
        //TODO
        internal object Shoot(object playerName, int cellId)
        {
            throw new NotImplementedException();
        }
        //TODO
        internal CancellationToken GetWinner()
        {
            throw new NotImplementedException();
        }

        internal void UpdateBoard(string playerName, int[][] boardState)
        {
            throw new NotImplementedException();
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


    public class GameManager
    {
        private List<Game> _games = new List<Game>();

        public Game GetGame(string gameId)
        {
            return _games.FirstOrDefault(g => g.GameId == gameId);
        }

        public void AddGame(Game game)
        {
            _games.Add(game);
        }
    }
}