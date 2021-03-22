using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.SearchViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {

        public SearchPageViewModel()
        {

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
        }

        // Parameters
        // Search parameter
        private string search = "";
        public string Search
        {
            get => search;
            set
            {
                if (value == search) { return; }
                search = value;
                OnPropertyChanged(nameof(Search));
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
