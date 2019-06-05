using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using KMSCalendar.Models.Entities;
using KMSCalendar.Services;

namespace KMSCalendar.ViewModels
{
    public class AssignmentDetailViewModel : BaseViewModel
    {
        //* Public Properties
        public Assignment Assignment { get; set; }

        public ICommand DeleteAssignmentCommand { get; set; }

        //* Constructors
        public AssignmentDetailViewModel(Assignment assignment)
        {
            Title = assignment?.Name;
            Assignment = assignment;

            DeleteAssignmentCommand = new Command(async () =>
                await ExecuteDeleteAssignmentCommandAssignment());
        }

        //* Private Methods
        public async Task ExecuteDeleteAssignmentCommandAssignment()
        {
            IDataStore<Assignment> dataStore = DependencyService.Get<IDataStore<Assignment>>();
            await dataStore.DeleteItemAsync(Assignment.Id);
        }
    }
}