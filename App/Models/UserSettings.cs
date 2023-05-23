using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
	public class UserSettings
	{
		public string Language { get; set; }
		public bool ReceiveNotifications { get; set; }
		public string DisplayName { get; set; }
		public bool SoundEnabled { get; set; }
	}
}
