using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		public int Identifier { get; set; }
		public int HighScore { get; set; }
		public UserSettings? UserSettings { get; set; }
	}
}
