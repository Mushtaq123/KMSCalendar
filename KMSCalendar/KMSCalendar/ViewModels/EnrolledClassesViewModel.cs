﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using KMSCalendar.Extensions;
using KMSCalendar.Models.Data;
using KMSCalendar.Services.Data;
using KMSCalendar.Views;

using Xamarin.Forms;

namespace KMSCalendar.ViewModels
{
	public class EnrolledClassesViewModel : BaseViewModel
	{
		//* Private Properties
		private List<Class> classes;

		//* Public Properties
		public bool ImageVisibility => Classes?.Count == 0;

		public ICommand UnsubscribeClassCommand { get; set; }

		public List<Class> Classes
		{
			get => classes;
			set
			{
				setProperty(ref classes, value);
				OnNotifyPropertyChanged(nameof(ImageVisibility));
			}
		}

		public ThemeImageSource MissingImageSource { get; }

		//* Constructors
		public EnrolledClassesViewModel()
		{
			MissingImageSource = new ThemeImageSource("missing_bag_blue.png", 
				"missing_bag_white.png", nameof(EnrolledClassesPage));

			updateData();

			UnsubscribeClassCommand = new Command<Class>(async @class => await unsubscribeClassAsync(@class));

			MessagingCenter.Subscribe<ClassSearchViewModel>(this, "UpdateClasses",
				(sender) => updateData());
		}

		//* Private Methods
		private void updateData()
		{
			App.PullEnrolledTeachers();
			Classes = App.SignedInUser.EnrolledClasses;
		}

		private async Task unsubscribeClassAsync(Class @class)
		{
			@class.UserId = App.SignedInUser.Id;
			await Task.Run(() => DataOperation.ConnectToBackend(ClassManager.UnenrollUserFromClass, @class));

			App.PullEnrolledClasses();
			updateData();

			MessagingCenter.Send(this, "LoadAssignments");

			// TODO: If there are no other users in the class, delete the class
		}
	}
}