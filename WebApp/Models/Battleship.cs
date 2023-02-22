namespace WebApp.Models
{
	public class Battleship
	{
		Playboard Playboard;

		public Battleship(Player player, int length)
		{
			this.Playboard = new Playboard(player, length);
		}
	}

	public class Playboard
	{
		public int[,] Size { get; set; }
		public Player Player { get; set; }
		public IDictionary<int, int> Hit { get; set; }
        public IDictionary<int, int> Boats { get; set; }
        public Playboard(Player player, int length)
		{
            Size = new int[length, length];
            this.Player = Player;
            Hit = new Dictionary<int, int>();
            Boats = new Dictionary<int, int>();
        }
		
		public void FillBoats()
		{
			
		}
	}
	
	public class Player
	{
		public int PlayerId;
	}

	public class Boat
	{
		public int Id { get; set; }
		public int Size { get; set; }


		Boat(int Coordinates, int BoatId)
		{
			this.Id = Id;
			this.Size = Size;
		}

	}
}