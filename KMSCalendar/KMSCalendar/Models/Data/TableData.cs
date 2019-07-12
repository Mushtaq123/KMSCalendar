﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KMSCalendar.Models.Data
{
    public abstract class TableData
    {
        //* Static Properties
        private static bool seedLoaded = false;

        private static List<Assignment> assignments { get; set; }
        private static List<Class> classes;
        private static List<Teacher> teachers;
        private static List<User> users;

        //* Public Properties

        /// <summary>The unique ID of the entity.</summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        //* Private Static Properties
        private static List<string> testTeachers = new List<string>
        {
            "Smith",
            "Johnson",
            "Williams",
            "Brown",
            "Davis",
            "Miller",
            "Wilson",
            "Moore",
            "Taylor"
        };
        private static List<string> testClasses = new List<string>
        {
            "Physics",
            "Calculus",
            "Biology",
            "History",
            "American Literature",
            "Chemistry",
            "Economics",
            "Computer Science",
            "Theory of Knowledge"
        };

        private static List<string> testItems = new List<string>
        {
            "Reading",
            "Complete the Assignment",
            "Finish online HW",
            "Take notes",
            "Group Project Due",
            "Comparative Essay"
        };

        //* Public Static Methods

        /// <summary>
        /// A method used to seed values if the MockDataStore is being used.
        /// </summary>
        /// <typeparam name="T">A subclass of TableData.</typeparam>
        /// <returns>An IEnumerable of the seeded instances of TableData.</returns>
        public static IEnumerable Seed<T>() where T : TableData
        {
            if (!seedLoaded)
            {
                seedListsInit();
                seedLoaded = true;
            }

            Type temp = typeof(T);

            if (temp.Equals(typeof(Assignment)))
                return assignments;
            else if (temp.Equals(typeof(Class)))
                return classes;
            else if (temp.Equals(typeof(Teacher)))
                return teachers;
            else if (temp.Equals(typeof(User)))
                return users;

            return null;
        }

        //* Private Static Methods
        private static int getAnotherNumber(int min, int max, int num)
        {
            Random random = new Random();
            int temp = num;

            while (temp == num)
                temp = random.Next(min, max);

            return temp;
        }

        private static void seedListsInit()
        {
            Random random = new Random();

            teachers = new List<Teacher>();
            for (int i = 0; i < 5; i++)
            {
                int rand = random.Next(0, 2);
                string surname = "";
                switch (rand)
                {
                    case 0: surname = "Ms. ";
                        break;
                    case 1: surname = "Mrs. ";
                        break;
                    case 2: surname = "Mr. ";
                        break;
                }
    
                teachers.Add(new Teacher
                {
                    Name = surname + testTeachers.ElementAt(i)
                });
            }

            classes = new List<Class>();
            for (int i = 0; i < 5; i++)
            {
                int num = random.Next(1, 7);
                //string className = $"Test Class {i}";
                string className = testClasses.ElementAt(random.Next(0, 8));
                Teacher teacher = teachers[i];

                classes.Add(new Class
                {
                    Name = className,
                    Period = num,
                    Teacher = teacher
                });

                classes.Add(new Class
                {
                    Name = className,
                    Period = getAnotherNumber(1, 7, num),
                    Teacher = teacher
                });
            }

            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                teachers[i].Classes = new List<Class>
                {
                    classes[count++],
                    classes[count++]
                };
            }

            assignments = new List<Assignment>();
            for (int i = 0; i < 30; i++)
            {
                assignments.Add(new Assignment
                {
                    //Name = $"Test Item {i}",
                    Name = testItems.ElementAt(getAnotherNumber(0, 5, 1)),
                    Description = $"From pages 1 - {i}   (test item)",
                    DueDate = DateTime.Today.AddDays(random.Next(-2, 3)),
                    Class = classes[random.Next(0, classes.Count)]
                });
            }

            var groupedAssignments = assignments.GroupBy(a => a.Class);
            foreach (var group in groupedAssignments)
            {
                Class _class = group.Key;
                _class.Assignments = new List<Assignment>();

                foreach (Assignment assignment in group)
                    _class.Assignments.Add(assignment);
            }

            List<Class> tempClasses = new List<Class>
            {
                classes[0],
                classes[2],
                classes[4]
            };

            users = new List<User>
            {
                new User
                {
                    Id = "1",
                    Email = "singh@king.com",
                    UserName = "SinghIsKing",
                    EnrolledClasses = tempClasses
                },
                new User
                {
                    Id = "2",
                    Email = "test@king.com",
                    UserName = "No Class Test",
                    EnrolledClasses = new List<Class>()
                },
                new User
                {
                    Id = "3",
                    Email = "mattmorgan6@gmail.com",
                    UserName = "mattmorgan6",
                    EnrolledClasses = tempClasses
                }
            };
        }
    }
}