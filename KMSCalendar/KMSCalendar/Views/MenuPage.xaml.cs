﻿using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KMSCalendar.Models;

namespace KMSCalendar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        //* Public Properties
        MainPage RootPage => Application.Current.MainPage as MainPage;

        //* Private Properties
        private List<HomeMenuItem> menuItems;

        //* Constructors
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem
                {
                    Id = MenuItemType.Calendar,
                    Title ="Calendar"
                },
                new HomeMenuItem
                {
                    Id = MenuItemType.About,
                    Title ="About"
                },
                new HomeMenuItem
                {
                    Id = MenuItemType.Settings,
                    Title = "Settings"
                }

            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                int id = (int)((HomeMenuItem) e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}