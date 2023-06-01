using App.ViewModels;

namespace App.Views
{
	public partial class Leaderboard : ContentPage
	{
		private readonly LeaderboardViewModel _leaderboardViewModel;

		public Leaderboard(LeaderboardViewModel leaderboardViewModel)
		{
			InitializeComponent();
			_leaderboardViewModel = leaderboardViewModel;
			BindingContext = _leaderboardViewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_ = _leaderboardViewModel.LoadLeaderboard(); 
		}
	}
}
