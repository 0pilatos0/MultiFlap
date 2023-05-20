using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
	public class UserSettings
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Language { get; set; }
		public bool ReceiveNotifications { get; set; }

		public virtual User User { get; set; }
	}
}
