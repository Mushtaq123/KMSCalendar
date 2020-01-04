﻿using System.Threading.Tasks;
using System.Windows.Input;

using Autofac;

using KMSCalendar.Models;
using KMSCalendar.Models.Data;
using KMSCalendar.Models.Settings;
using KMSCalendar.Services.Data;
using KMSCalendar.Views;

using ModelValidation;

using Xamarin.Forms;

namespace KMSCalendar.ViewModels
{
	public class LogInViewModel : BaseViewModel
	{
		//* Private Properties
		private int logInAttempts;

		private string email;
		private string loginValidationMessage;
		private string password;

		//* Protected Properties
		protected readonly UserSettings UserSettings;

		//* Public Properties
		public ICommand AuthenticateUserCommand { get; set; }
		public ICommand ForgotPasswordCommand { get; set; }
		public ICommand NewUserCommand { get; set; }

		public int LogInAttempts
		{
			get => logInAttempts;
			set => setProperty(ref logInAttempts, value);
		}

		[ContainsCharacter('@')]
		[DoesNotContainCharacter(' ')]
		[MinimumLength(5)]
		[MaximumLength(254)]
		public string Email
		{
			get => email;
			set => setProperty(ref email, value);
		}
		public string LoginValidationMessage
		{
			get
			{
				if (Errors != null && Errors.Count > 0)
					return Errors[0];

				return loginValidationMessage;
			}
			set => setProperty(ref loginValidationMessage, value);
		}
		[MinimumLength(8)]
		[MaximumLength(64)]
		public string Password
		{
			get => password;
			set => setProperty(ref password, value);
		}

		//* Constructor
		public LogInViewModel() :
			this(AppContainer.Container.Resolve<UserSettings>())
		{ }

		public LogInViewModel(UserSettings userSettings)
		{
			UserSettings = userSettings;

			Email = string.Empty;
			Password = string.Empty;

			PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(Errors))
					OnNotifyPropertyChanged(nameof(LoginValidationMessage));
			};

			AuthenticateUserCommand = new Command(async () => await authenticateUser());
			ForgotPasswordCommand = new Command(async () => await
				App.MainPage.Navigation.PushModalAsync(new ForgotPasswordPage()));
			NewUserCommand = new Command(() => App.MainPage = new SignUpPage());
		}

		//* Private Method
		private async Task authenticateUser()
		{
			IsBusy = true;

			await Task.Run(() =>
			{
				if (Validate())
				{
					User signedInUser = DataOperation.ConnectToBackend(UserManager.LoadUserFromEmail, Email);

					System.Diagnostics.Debug.WriteLine("Made it Here!");

					if (signedInUser == null)
						LoginValidationMessage = "This email does not have an account, please sign up for an account";
					else
					{
						if (PasswordHasher.ValidatePassword(Password, signedInUser.Password))
						{
							App.SignedInUser = signedInUser;

							UserSettings.SignedInUserId = signedInUser.Id;

							Device.BeginInvokeOnMainThread(() => App.MainPage = new MainPage());
						}
						else
							LoginValidationMessage = "Invalid Password";
					}
				}
			});

			IsBusy = false;
		}
	}
}