using App.ViewModels;

namespace App.Views;

public partial class Achievements : ContentPage
{
    private readonly AchievementsViewModel _achievementsViewModel;

    public Achievements(AchievementsViewModel achievementsViewModel)
    {
        InitializeComponent();
        _achievementsViewModel = achievementsViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _achievementsViewModel.LoadAchievements(); // Call a method to reload the data
    }
}
