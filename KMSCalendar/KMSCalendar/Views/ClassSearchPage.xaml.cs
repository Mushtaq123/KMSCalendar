﻿using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KMSCalendar.Models.Data;
using KMSCalendar.ViewModels;

namespace KMSCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClassSearchPage : ContentPage
	{
        private App app = (Application.Current as App);

        //* Public Properties
        public ClassSearchViewModel ViewModel;

        public MainPage RootPage => Application.Current.MainPage as MainPage;

        //* Constructors
        public ClassSearchPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new ClassSearchViewModel();

            int schoolId = app.SignedInUser.SchoolId;

            // Event Handlers for the SearchBar text changing or for the SearchButton pressing.
            ClassSearchBar.TextChanged += (sender, args) => ViewModel.FilterClasses(ClassSearchBar.Text);
            ClassSearchBar.SearchButtonPressed += (sender, args) => ViewModel.FilterClasses(ClassSearchBar.Text);
        }

        //* Public Methods

        /// <summary>
        /// This method goes to the calendar once the user has selected a class and a period
        /// </summary>
        /// <param name="periodChosen">Period that the user is in</param>
        public async Task GoToCalendarAsync(int periodChosen)
        {
            Class selectedClass = ViewModel.SelectedClass;
            
            // TODO: MATEO add this to the sample data
            
            // TODO: SUNNY add the period selected and class selected to the database.

            //Closes the page and goes to the last one on the stack
            await Navigation.PopModalAsync();
        }

        /// <summary>
        /// Swaps from the class search view to the period selection view.
        /// </summary>
        public void Swap()
        {
            SearchAreaStackLayout.IsVisible = !SearchAreaStackLayout.IsVisible;
            PopUpGrid.IsVisible = !PopUpGrid.IsVisible;
        }

        //* Event Handlers

        private void AddNewPeriodButton_Clicked(object sender, EventArgs e)
        {
            int newPeriod = int.Parse(NewPeriodLabel.Text);

            ViewModel.LoadPeriods(newPeriod);

            // I tried using these to update the list but they didn't work. Databinding issue?
            // ParentPage.ViewModel.Periods.Add(newPeriod);
            // PeriodsListView.BeginRefresh();

            // TODO: Mateo TODAY add the new Period to the listview and database


        }

        private void BackButton_Clicked(object sender, EventArgs e) =>
            Swap();

        /// <summary>
        /// Invoked when the user selects a class, then shows the selectPeriodView where
        /// the user can select a period.
        /// </summary>
        private void ClassesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectedClass = ClassesListView.SelectedItem as Class;

            ViewModel.LoadPeriods();
            Swap();
        }

        private async void DoneButton_Clicked(object sender, EventArgs e)
        {
            if (PeriodsListView.SelectedItem != null)
            {
                int periodChosen = (int) PeriodsListView.SelectedItem;

                await GoToCalendarAsync(periodChosen);
            }
        }

        private void ExitButton_Clicked(object sender, EventArgs e) =>
            Navigation.PopModalAsync();

        private void GoToNewClassButton_Clicked(object sender, EventArgs e) => 
            Navigation.PushModalAsync(new NewClassPage(this));

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedClass != null)
            {
                ViewModel.LoadPeriods();
                Swap();
            }
        }
    }
}