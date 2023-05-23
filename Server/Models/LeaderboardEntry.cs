using System.Text.Json.Serialization;

namespace Server.Models
{
	public class LeaderboardEntry
	{
		public int Id { get; set; }
		public int Score { get; set; }
		public DateTime DateAchieved { get; set; }

		public int UserId { get; set; }
		[JsonIgnore]
		public virtual User User { get; set; }
	}
}
