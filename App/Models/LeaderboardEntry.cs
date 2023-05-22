using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
	public class LeaderboardEntry
	{
		public int Id { get; set; }
		public int Score { get; set; }
		public DateTime DateAchieved { get; set; }
		public string DisplayName { get; set; }
	}
}
