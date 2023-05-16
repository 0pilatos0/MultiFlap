using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Models;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels
{
	public class UserSettingsViewModel : BaseViewModel
	{
		private UserSettings _userSettings;

		public UserSettings UserSettings
		{
			get => _userSettings;
			set
			{
				if (_userSettings != value)
				{
					_userSettings = value;
					OnPropertyChanged();
				}
			}
		}

		public UserSettingsViewModel()
		{
			// Initialize the UserSettings instance
			UserSettings = new UserSettings();
		}

		// Command for saving the user settings
		private RelayCommand _saveCommand;
		public RelayCommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(SaveUserSettings);
				}
				return _saveCommand;
			}
		}

		private void SaveUserSettings()
		{
			// Save the user settings logic here
			// You can access the UserSettings property to get the updated values
		}
	}
}
