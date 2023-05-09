using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class UserSettings
    {
		[Key]
		public int Id { get; set; }
		public string DisplayName { get; set; }
        public string Language { get; set; }
    }
}
