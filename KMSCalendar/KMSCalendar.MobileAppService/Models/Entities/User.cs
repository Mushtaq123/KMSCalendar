using System.Collections.Generic;

namespace KMSCalendar.MobileAppService.Models.Entities
{
    public class User : TableData
    {
        //* Public Properties
        public List<Class> EnrolledClasses { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        //* Overridden Methods
        public override void Update(TableData td)
        {
            if (td is User)
            {
                User other = (User) td;

                EnrolledClasses = other.EnrolledClasses;
                Password = other.Password;
            }
        }
    }
}