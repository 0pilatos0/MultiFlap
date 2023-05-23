using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
	public class UserSettings
	{
		public string UserId { get; set; } // Foreign key to User.Id
		public string Language { get; set; }
		public bool SoundEnabled { get; set; }
	}
}
