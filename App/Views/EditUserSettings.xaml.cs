using App.ViewModels;

namespace App.Views;

public partial class EditUserSettings : ContentPage
{
	private readonly UserSettingsViewModel _userSettingsViewModel;

	public EditUserSettings(UserSettingsViewModel userSettingsViewModel)
	{
		InitializeComponent();
		_userSettingsViewModel = userSettingsViewModel;
		BindingContext = _userSettingsViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_ = _userSettingsViewModel.LoadUserSettings(); // Call a method to reload the data
	}
}