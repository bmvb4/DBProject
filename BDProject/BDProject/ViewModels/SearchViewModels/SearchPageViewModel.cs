using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
using BDProject.Views._PopUps;
using MvvmHelpers;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.SearchViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<SearchBubble> AllBubbles = new ObservableRangeCollection<SearchBubble>();
        private ObservableRangeCollection<SearchBubble> AllPeople = new ObservableRangeCollection<SearchBubble>();
        private ObservableRangeCollection<SearchBubble> AllTags = new ObservableRangeCollection<SearchBubble>();

        private void SortBubbles()
        {
            foreach(SearchBubble sb in AllBubbles)
            {
                if (sb.IsTag == true)
                {
                    AllTags.Add(sb);
                }
                else
                {
                    AllPeople.Add(sb);
                }
            }
        }

        public SearchPageViewModel()
        {
            //=============================================TEST
            for (int i=0; i<30; i++)
            {
                if(i==4 || i == 6 || i == 13 || i == 14 || i == 20 || i == 24)
                {
                    AllBubbles.Add(new SearchBubble(new Tag { TagName=$"test{i}" }));
                }
                else
                {
                    AllBubbles.Add(new SearchBubble(_Globals.GlobalMainUser));
                }
            }
            //=============================================TEST

            SortBubbles();
            SearchAllFunction();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            FilterCommand = new Command(async () => await FilterFunction());
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());

            SearchAllCommand = new Command(SearchAllFunction);
            SearchPeopleCommand = new Command(SearchPeopleFunction);
            SearchTagsCommand = new Command(SearchTagsFunction);
        }

        // Parameters
        // search bubbles collection
        private ObservableRangeCollection<SearchBubble> bubblesCollection = new ObservableRangeCollection<SearchBubble>();
        public ObservableRangeCollection<SearchBubble> BubblesCollection
        {
            get => bubblesCollection;
            set
            {
                if (value == bubblesCollection) { return; }
                bubblesCollection = value;
                OnPropertyChanged(nameof(BubblesCollection));
            }
        }

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

        // Back to post command
        public ICommand FilterCommand { get; set; }
        private async Task FilterFunction()
        {
            await PopupNavigation.Instance.PushAsync(new PostPopUp());
        }

        // show everything command
        public ICommand SearchAllCommand { get; set; }
        private void SearchAllFunction()
        {
            BubblesCollection.Clear();
            if (AllBubbles.Count >= 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    BubblesCollection.Add(AllBubbles[i]);
                }
            }
            else
            {
                foreach (SearchBubble sb in AllBubbles)
                {
                    BubblesCollection.Add(sb);
                }
            }

            everything = true;
            onlyPeople = false;
            onlyTags = false;
        }

        // show only people command
        public ICommand SearchPeopleCommand { get; set; }
        private void SearchPeopleFunction()
        {
            BubblesCollection.Clear();
            if (AllPeople.Count >= 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    BubblesCollection.Add(AllPeople[i]);
                }
            }
            else
            {
                foreach (SearchBubble sb in AllPeople)
                {
                    BubblesCollection.Add(sb);
                }
            }

            onlyPeople = true;
            onlyTags = false;
            everything = false;
        }

        // show only tags command
        public ICommand SearchTagsCommand { get; set; }
        private void SearchTagsFunction()
        {
            BubblesCollection.Clear();
            if (AllTags.Count >= 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    BubblesCollection.Add(AllTags[i]);
                }
            }
            else
            {
                foreach(SearchBubble sb in AllTags)
                {
                    BubblesCollection.Add(sb);
                }
            }

            onlyTags = true;
            onlyPeople = false;
            everything = false;
        }

        // load more command 
        private bool onlyPeople = false;
        private bool onlyTags = false;
        private bool everything = false;
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (_Globals.IsBusy) { return; }
            _Globals.IsBusy = true;

            await Task.Delay(1000);

            // add everything
            if (everything == true)
            {
                if (AllBubbles.Count - BubblesCollection.Count < 10)
                {
                    BubblesCollection.AddRange(AllBubbles.Skip(BubblesCollection.Count));
                }
                else
                {
                    BubblesCollection.AddRange(AllBubbles.Skip(BubblesCollection.Count).Take(10));
                }
            }

            // add only people everything
            if (onlyPeople == true)
            {
                if (AllPeople.Count - BubblesCollection.Count < 10)
                {
                    BubblesCollection.AddRange(AllPeople.Skip(BubblesCollection.Count));
                }
                else
                {
                    BubblesCollection.AddRange(AllPeople.Skip(BubblesCollection.Count).Take(10));
                }
            }

            // add only tags everything
            if (onlyTags == true)
            {
                if (AllTags.Count - BubblesCollection.Count < 10)
                {
                    BubblesCollection.AddRange(AllTags.Skip(BubblesCollection.Count));
                }
                else
                {
                    BubblesCollection.AddRange(AllTags.Skip(BubblesCollection.Count).Take(10));
                }
            }

            _Globals.IsBusy = false;
            return;
        }

    }
}
