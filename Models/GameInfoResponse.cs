namespace Minesweeper.Models
{
	public class GameInfoResponse
	{
		public string game_id { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int mines_count { get; set; }
		public bool completed { get; set; } = false;
		public string[,] field { get; set; }
	}
}
