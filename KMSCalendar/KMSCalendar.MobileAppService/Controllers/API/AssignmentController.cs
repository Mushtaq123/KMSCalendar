using KMSCalendar.MobileAppService.Models.Entities;

namespace KMSCalendar.MobileAppService.Controllers.API
{
    public class AssignmentController : BaseController<Assignment>
    {
        //* Constructors
        public AssignmentController(CalendarDbDataContext db) : base(db) { }
    }
}