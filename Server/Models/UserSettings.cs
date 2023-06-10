using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class UserSettings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Language { get; set; }
        public bool ReceiveNotifications { get; set; }
        public string DisplayName { get; set; }
        public bool SoundEnabled { get; set; }
        public bool ShakeEnabled { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
