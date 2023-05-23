using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
	public class User
	{
		public int Id { get; set; }
		public string ?Auth0Identifier { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		public bool SoundEnabled { get; set; }

		public virtual ICollection<LeaderboardEntry> LeaderboardEntries { get; set; }
		public virtual ICollection<PowerUpItem> PowerUpItems { get; set; }
		public virtual ICollection<Achievement> Achievements { get; set; }
	}
}
