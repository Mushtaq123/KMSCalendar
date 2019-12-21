﻿using System.Collections.Generic;

using Xamarin.Forms;

using KMSCalendar.Models.Data;
using KMSCalendar.Services.Data;

namespace KMSCalendar.ViewModels
{
    public class NewClassViewModel : BaseViewModel
    {
        //* Private Properties
        private string period;
        App app = Application.Current as App;

        private List<Teacher> teachers;

        private string className;
        private string searchTerm;
        private string teacherName;

        private Teacher selectedTeacher;

        //* Public Properties
        public string Period
        {
            get => period;
            set => setProperty(ref period, value);
        }

        public List<Teacher> Teachers
        {
            get => teachers;
            set
            {
                foreach (Teacher t in value)
                    t.SchoolName = app.SchoolName;
                setProperty(ref teachers, value);
            }
        }

        public string ClassName
        {
            get => className;
            set => setProperty(ref className, value);
        }
        public string SearchTerm
        {
            get => searchTerm;
            set => setProperty(ref searchTerm, value);
        }
        public string TeacherName
        {
            get => teacherName;
            set => setProperty(ref teacherName, value);
        }
        public Teacher SelectedTeacher
        {
            get => selectedTeacher;
            set => setProperty(ref selectedTeacher, value);
        }
        
        //* Constructors
        public NewClassViewModel()
        {
            
            app.PullSchoolName();
            Teachers = TeacherManager.LoadAllTeachers(app.SignedInUser.SchoolId);
            SearchTerm = "";
        }
    }
}