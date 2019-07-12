﻿using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using KMSCalendar.Models.Data;
using KMSCalendar.Services.Data;

namespace KMSCalendar.ViewModels
{
    public class ClassSearchViewModel : BaseViewModel
    {
        //* Private Properties
        private Class selectedClass;

        private List<Class> classes;
        private List<Class> filteredClasses;
        private List<Class> uniqueClasses;

        private List<int> periods;

        //* Public Properties
        public Class SelectedClass
        {
            get => selectedClass;
            set => setProperty(ref selectedClass, value);
        }

        public List<Class> FilteredClasses
        {
            get => filteredClasses;
            set => setProperty(ref filteredClasses, value);
        }
        public List<int> Periods
        {
            get => periods;
            set => setProperty(ref periods, value);
        }

        //* Constructors
        public ClassSearchViewModel()
        {
            Title = "Search For Class";

            var dataStore = DependencyService.Get<IDataStore<Class>>();

            var classes =
                from _class in dataStore.GetItemsAsync().Result
                let userClasses =
                    from userClass in (Application.Current as App).SignedInUser.EnrolledClasses
                    select userClass.Id
                where !userClasses.Contains(_class.Id)
                select _class;

            this.classes = classes.ToList();
            uniqueClasses = this.classes.Distinct(new DuplicateClassNameComparer()).ToList();
            FilteredClasses = new List<Class>(uniqueClasses);
            Periods = new List<int>();
        }

        //* Public Methods
        public void FilterClasses(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                FilteredClasses = new List<Class>(uniqueClasses);
            else
            {
                var result =
                    from _class in uniqueClasses.AsParallel()
                    where _class.Name.ToLower().Contains(userInput.ToLower())
                    select _class;

                FilteredClasses = result.ToList();
            }
        }

        public void LoadPeriods()
        {
            var periods =
                from _class in classes.AsParallel()
                where _class.Name == SelectedClass.Name &&
                    _class.Teacher.Equals(SelectedClass.Teacher)
                select _class.Period;

            Periods = periods.ToList();
        }

        /// <summary>
        /// For some reason I need this method and cannot just add a new int to the periods list
        /// </summary>
        public void LoadPeriods(int newPeriod)
        {
            var periods =
                from _class in classes.AsParallel()
                where _class.Name == SelectedClass.Name &&
                    _class.Teacher.Equals(SelectedClass.Teacher)
                select _class.Period;

            Periods = periods.ToList();
            Periods.Add(newPeriod);
        }

        private class DuplicateClassNameComparer : EqualityComparer<Class>
        {
            //* Overridden Methods
            public override bool Equals(Class x, Class y) => 
                x.Name == y.Name && x.Teacher?.Name == y.Teacher?.Name;

            public override int GetHashCode(Class obj) => 
                $"{obj.Name} ({obj.Teacher?.Name ?? ""})".GetHashCode();
        }
    }
}