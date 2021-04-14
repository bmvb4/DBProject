using BDProject.Helpers;
using BDProject.ModelWrappers;
using BDProject.Views._PopUps;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.SearchViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {
        private ObservableCollection<SearchBubble> AllBubbles = new ObservableCollection<SearchBubble>();
        private ObservableCollection<SearchBubble> AllPeople = new ObservableCollection<SearchBubble>();
        private ObservableCollection<SearchBubble> AllTags = new ObservableCollection<SearchBubble>();

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
            LoadMoreCommand = new Command(LoadMoreFunction);

            SearchAllCommand = new Command(SearchAllFunction);
            SearchPeopleCommand = new Command(SearchPeopleFunction);
            SearchTagsCommand = new Command(SearchTagsFunction);
        }

        // Parameters
        // search bubbles collection
        private ObservableCollection<SearchBubble> bubblesCollection = new ObservableCollection<SearchBubble>();
        public ObservableCollection<SearchBubble> BubblesCollection
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
        private void LoadMoreFunction()
        {
            if (BubblesCollection.Count == AllBubbles.Count) { return; }
            int start = BubblesCollection.Count;
            int amount = BubblesCollection.Count + 10;
            
            // add everything
            if (everything == true)
            {
                if(amount < AllBubbles.Count)
                {
                    for (int i = start; i < amount; i++)
                    {
                        if (BubblesCollection.Count == AllBubbles.Count) { break; }

                        BubblesCollection.Add(AllBubbles[i]);
                    }
                }
                else
                {
                    for (int i = start; i < AllBubbles.Count; i++)
                    {
                        if (BubblesCollection.Count == AllBubbles.Count) { break; }

                        BubblesCollection.Add(AllBubbles[i]);
                    }
                }

                return;
            }

            // add only people everything
            if (onlyPeople == true)
            {
                if (amount < AllPeople.Count)
                {
                    for (int i = start; i < amount; i++)
                    {
                        if (BubblesCollection.Count == AllPeople.Count) { break; }

                        BubblesCollection.Add(AllPeople[i]);
                    }
                }
                else
                {
                    for (int i = start; i < AllPeople.Count; i++)
                    {
                        if (BubblesCollection.Count == AllPeople.Count) { break; }

                        BubblesCollection.Add(AllPeople[i]);
                    }
                }

                return;
            }

            // add only tags everything
            if (onlyTags == true)
            {
                if (amount < AllTags.Count)
                {
                    for (int i = start; i < amount; i++)
                    {
                        if (BubblesCollection.Count == AllTags.Count) { break; }

                        BubblesCollection.Add(AllTags[i]);
                    }
                }
                else
                {
                    for (int i = start; i < AllTags.Count; i++)
                    {
                        if (BubblesCollection.Count == AllTags.Count) { break; }

                        BubblesCollection.Add(AllTags[i]);
                    }
                }

                return;
            }
        }

    }
}
