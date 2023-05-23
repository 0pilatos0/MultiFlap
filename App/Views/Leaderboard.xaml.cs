using App.ViewModels;

namespace App.Views;

public partial class Leaderboard : ContentPage
{
	public Leaderboard(LeaderboardViewModel leaderboardViewModel)
	{
		InitializeComponent();

		BindingContext = leaderboardViewModel;
	}


}