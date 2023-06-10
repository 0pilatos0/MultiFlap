namespace Server.Models
{
	public class Achievement
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int UserId { get; set; }
		public virtual User User { get; set; }
	}
}
