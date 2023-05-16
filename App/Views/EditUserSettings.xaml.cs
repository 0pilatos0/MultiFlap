using App.ViewModels;

namespace App.Views;

public partial class EditUserSettings : ContentPage
{
	public EditUserSettings(UserSettingsViewModel userSettingsViewModel)
	{
		InitializeComponent();

		BindingContext= userSettingsViewModel;
	}
}