﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Autofac;

using KMSCalendar.Models;
using KMSCalendar.Models.Data;
using KMSCalendar.Services.Data;
using KMSCalendar.Services.Email;

using ModelValidation;

using Xamarin.Forms;

namespace KMSCalendar.ViewModels
{
	public class ForgotPasswordViewModel : BaseViewModel
	{
		//* Constants

		/// <summary>
		/// Determines the length of the token sent to the user. Has
		/// to be between 1 & 32.
		/// </summary>
		public const int TOKEN_SIZE = 6;

		//* Private Properties
		private bool emailVisibility;
		private bool goBackVisibility;
		private bool newPasswordVisibility;
		private bool successVisibility;
		private bool verificationVisibility;
		private bool isLoadingData;

		private readonly IEmailService emailService;

		private string code;
		private string confirmPassword;
		private string email;
		private string password;
		private string token;
		private string validationMessage;

		//* Public Properties
		public bool EmailVisibility
		{
			get => emailVisibility;
			set => setProperty(ref emailVisibility, value);
		}
		public bool GoBackVisibility
		{
			get => goBackVisibility;
			set => setProperty(ref goBackVisibility, value);
		}
		public bool NewPasswordVisibility
		{
			get => newPasswordVisibility;
			set => setProperty(ref newPasswordVisibility, value);
		}
		public bool SuccessVisibility
		{
			get => successVisibility;
			set => setProperty(ref successVisibility, value);
		}
		public bool VerificationVisibility
		{
			get => verificationVisibility;
			set => setProperty(ref verificationVisibility, value);
		}
		public bool IsLoadingData
		{
			get => isLoadingData;
			set => setProperty(ref isLoadingData, value);
		}

		public ICommand AuthenticateCodeCommand { get; }
		public ICommand AuthenticateEmailCommand { get; }
		public ICommand AuthenticateNewPasswordCommand { get; }
		public ICommand GoBackCommand { get; }

		public string Code
		{
			get => code;
			set => setProperty(ref code, value);
		}
		[PropertyValueMatch(nameof(Password),
			ErrorMessage = "The Passwords do not match!")]
		public string ConfirmPassword
		{
			get => confirmPassword;
			set => setProperty(ref confirmPassword, value);
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
		[MinimumLength(8)]
		[MaximumLength(64)]
		public string Password
		{
			get => password;
			set => setProperty(ref password, value);
		}
		public string ValidationMessage
		{
			get
			{
				if (Errors != null && Errors.Count > 0)
					return Errors[0];

				return validationMessage;
			}
			set => setProperty(ref validationMessage, value);
		}

		//* Constructors
		public ForgotPasswordViewModel() :
			this(AppContainer.Container.Resolve<IEmailService>())
		{ }

		public ForgotPasswordViewModel(IEmailService emailService)
		{
			EmailVisibility = true;
			VerificationVisibility = false;
			GoBackVisibility = true;

			this.emailService = emailService;

			token = Guid.NewGuid().ToString().Replace("-", "")
				.Substring(0, TOKEN_SIZE).ToUpper();

			PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(Errors))
					OnNotifyPropertyChanged(nameof(ValidationMessage));
			};

			AuthenticateCodeCommand = new Command(() => ExecuteAuthenticateCodeCommand());
			AuthenticateEmailCommand = new Command(async () => await ExecuteAuthenticateEmailCommandAsync());
			AuthenticateNewPasswordCommand = new Command(() => ExecuteAuthenticateNewPasswordCommand());
			GoBackCommand = new Command(() =>
				App.MainPage.Navigation.PopModalAsync());
		}

		//* Private Methods
		private void ExecuteAuthenticateCodeCommand()
		{
			if (Code?.ToUpper().Equals(token) ?? false)
				SwapViews();
			else
				ValidationMessage = "Code invalid!";
		}

		private async Task ExecuteAuthenticateEmailCommandAsync()
		{
			if (Validate() && Email != null)
			{
				IsLoadingData = true;
				ValidationMessage = "";

				await Task.Run(() =>
				{
					User user = DataOperation.ConnectToBackend(UserManager.LoadUserFromEmail, Email);
					if (user != null)
					{
						try
						{
							emailService.SendResetPasswordEmail(user, token);
							SwapViews();
						}
						catch
						{
							validationMessage = "Email failed to send";
						}
					}
					else
					ValidationMessage = "Please enter the email address for your account.";
				});

				IsLoadingData = false;
			}
			else
				ValidationMessage = "Please enter a valid email address.";
		}

		private void ExecuteAuthenticateNewPasswordCommand()
		{
			if (Validate() && Password != null)
			{
				User user = DataOperation.ConnectToBackend(UserManager.LoadUserFromEmail, Email);

				if (user != null)
				{
					user.Password = PasswordHasher.HashPassword(Password);

					//Resets the user's password in the DB
					DataOperation.ConnectToBackend(UserManager.UpdateUser, user);

					SwapViews();
				}
			}
			else
				ValidationMessage = "Please enter a password.";
		}

		private void SwapViews()
		{
			if (EmailVisibility)
			{
				EmailVisibility = false;
				VerificationVisibility = true;
			}
			else if (VerificationVisibility)
			{
				VerificationVisibility = false;
				NewPasswordVisibility = true;
			}
			else if (NewPasswordVisibility)
			{
				NewPasswordVisibility = false;
				SuccessVisibility = true;
				GoBackVisibility = false;
			}

			ValidationMessage = "";
		}
	}
}