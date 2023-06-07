using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
	public class UserSettingsDTO
	{
		public string Language { get; set; }
		public bool ReceiveNotifications { get; set; }
		public string DisplayName { get; set; }
		public bool SoundEnabled { get; set; }
		public bool ShakeEnabled { get; set; }
	}
}
