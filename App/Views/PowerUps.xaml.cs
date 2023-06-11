using App.ViewModels;

namespace App.Views;

public partial class PowerUps : ContentPage
{
    private PowerUpsViewModel _powerUpsViewModel;

    public PowerUps(PowerUpsViewModel powerUpsViewModel)
    {
        InitializeComponent();
        _powerUpsViewModel = powerUpsViewModel;

        BindingContext = _powerUpsViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _powerUpsViewModel.LoadPowerUps(); // Call a method to reload the data
    }
}
