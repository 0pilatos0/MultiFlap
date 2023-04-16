using System.Collections.Concurrent;

namespace Server
{
	public class GameData
	{
		private static readonly Lazy<GameData> instance = new Lazy<GameData>(() => new GameData());

		private readonly ConcurrentDictionary<string, Player> players = new ConcurrentDictionary<string, Player>();
		private readonly ConcurrentDictionary<string, Match> matches = new ConcurrentDictionary<string, Match>();

		public static GameData Instance => instance.Value;

		public ConcurrentDictionary<string, Player> Players => players;
		public ConcurrentDictionary<string, Match> Matches => matches;

		private GameData()
		{
		}
	}
}
