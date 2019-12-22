﻿using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using ModelValidation;

using Xamarin.Forms;

using KMSCalendar.Models;
using KMSCalendar.Models.Data;
using KMSCalendar.Models.Settings;
using KMSCalendar.Services.Data;
using KMSCalendar.Views;
using System;

namespace KMSCalendar.ViewModels
{
    public class SignUpViewModel : LogInViewModel
    {
        //* Private Properties
        private string confirmPassword;
        private string userName;

        private string zipCode;

        private bool signUpVisibility;
        private bool schoolEnrollmentVisibility;

        //* Public Properties
        public ICommand AlreadyUserCommand { get; set; }
        public new ICommand AuthenticateUserCommand { get; set; }

        [PropertyValueMatch(nameof(Password), 
            ErrorMessage = "The Passwords do not match!")]
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => setProperty(ref confirmPassword, value);
        }
        [MinimumLength(2)]
        [MaximumLength(64)]
        public string UserName
        {
            get => userName;
            set => setProperty(ref userName, value);
        }

        public string ZipCode
        {
            get => zipCode;
            set => setProperty(ref zipCode, value);
        }

        public bool SchoolEnrollmentVisibility
        {
            get => schoolEnrollmentVisibility;
            set => setProperty(ref schoolEnrollmentVisibility, value);
        }
        public bool SignUpVisibility
        {
            get => signUpVisibility;
            set => setProperty(ref signUpVisibility, value);
        }


        //* Constructors
        public SignUpViewModel() : base()
        {
            Title = "Sign Up";

            SignUpVisibility = true;
            SchoolEnrollmentVisibility = false;

            ConfirmPassword = string.Empty;
            UserName = string.Empty;

            AlreadyUserCommand = new Command(() => ExecuteAlreadyUserCommand());
            AuthenticateUserCommand = new Command(async () => await ExecuteAuthenticateUserCommand());
        }

        //* Public Methods
        public void ExecuteAlreadyUserCommand() =>
            (Application.Current as App).MainPage = new LogInPage();

        public new async Task ExecuteAuthenticateUserCommand()
        {
            if (Validate())
            {
                bool alreadyEmail = UserManager.CheckForUser(Email.Trim());

                if (alreadyEmail)
                    LoginValidationMessage = "User already exists! Please log in instead.";
                else
                {
                    SwapViews();   
                }
            }
        }

        private void SignUpUser()
        {
            string hashedPassword = PasswordHasher.HashPassword(Password);

            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = Email.Trim(),
                UserName = UserName.Trim(),
                Password = hashedPassword,
                SchoolId = new Guid("6e67224a - e398 - 430a - b7b6 - b3e0c0c8c4ae")
            };

            UserManager.PutInUser(user);

            App app = Application.Current as App;

            app.SignedInUser = user;
            userSettings.SignedInUserId = user.Id;

            app.MainPage = new MainPage();
        }

        private void SwapViews()
        {
            if(SignUpVisibility)
            {
                SignUpVisibility = false;
                SchoolEnrollmentVisibility = true;
            }
            else
            {
                SchoolEnrollmentVisibility = false;
                SignUpVisibility = true;
            }
        }
    }
}