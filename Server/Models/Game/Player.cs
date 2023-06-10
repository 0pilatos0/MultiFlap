namespace Server.Models.Game
{
    public class Player
    {
        public string ConnectionId { get; set; }
        public int Y { get; set; }
        public int Score { get; set; }
        public string MatchId { get; set; }
        public bool IsLookingForMatch { get; set; }
    }
}
