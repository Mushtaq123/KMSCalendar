﻿using System;

namespace KMSCalendar.MobileAppService.Models.Entities
{
    public class Assignment : TableData
    {
        //* Public Properties
        public DateTime DueDate { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }

        //* Public Methods
        public override void Update(TableData td)
        {
            if (td is Assignment)
            {
                Assignment other = (Assignment) td;

                DueDate = other.DueDate;
                Description = other.Description;
                Name = other.Name;
            }
        }
    }
}