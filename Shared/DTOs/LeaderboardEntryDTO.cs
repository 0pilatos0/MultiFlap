using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
	public class LeaderboardEntryDTO
	{
		public int Id { get; set; }
		public int Score { get; set; }
		public DateTime DateAchieved { get; set; }
		public string? DisplayName { get; set; }
	}
}
