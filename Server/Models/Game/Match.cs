namespace Server.Models.Game
{
	public class Match
	{
		public string Id { get; set; }
		public List<PlayerMatchInfo> Players { get; set; }
	}
}
