﻿using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels.SearchViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {
        private ObservableRangeCollection<SearchBubble> AllBubbles = new ObservableRangeCollection<SearchBubble>();
        private ObservableRangeCollection<SearchBubble> AllPeople = new ObservableRangeCollection<SearchBubble>();
        private ObservableRangeCollection<SearchBubble> AllTags = new ObservableRangeCollection<SearchBubble>();
        private int choice = 0;

        private async void SetAllCollection(string value)
        {
            AllBubbles.Clear();
            BubblesCollection.Clear();
            var success = await ServerServices.SendPostRequestAsync($"search/tag/{value}", new JObject());
            await Task.Delay(1000);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var listTags = JsonConvert.DeserializeObject<List<string>>(earthquakesJson);

                foreach (string s in listTags)
                    AllBubbles.Add(new SearchBubble(s) { IsTag = true });
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                return;
            }

            success = await ServerServices.SendPostRequestAsync($"search/user/{value}", new JObject());
            await Task.Delay(1000);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var listUsers = JsonConvert.DeserializeObject<List<UserDB>>(earthquakesJson);

                foreach (UserDB u in listUsers)
                {
                    if (u.Photo == null) AllBubbles.Add(new SearchBubble(u) { IsTag = false, ImageBytes = _Globals.Base64Bytes });
                    else AllBubbles.Add(new SearchBubble(u) { IsTag = false });
                }
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                return;
            }

            AllBubbles.OrderBy(x => string.Compare(x.Name, Search));
            if (AllBubbles.Count >= 20) BubblesCollection.AddRange(AllBubbles.Take(20));
            else BubblesCollection.AddRange(AllBubbles);
        }

        private async void SetPeopleCollection(string value)
        {
            AllPeople.Clear();
            BubblesCollection.Clear();
            var success = await ServerServices.SendPostRequestAsync($"search/user/{value}", new JObject());
            await Task.Delay(1000);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var listUsers = JsonConvert.DeserializeObject<List<UserDB>>(earthquakesJson);

                if (listUsers.Count >= 20)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (listUsers[i].Photo == null) AllPeople.Add(new SearchBubble(listUsers[i]) { IsTag = false, ImageBytes = _Globals.Base64Bytes });
                        else AllPeople.Add(new SearchBubble(listUsers[i]) { IsTag = false });
                    }
                    AllPeople.OrderBy(x => string.Compare(x.Name, Search));
                    BubblesCollection.AddRange(AllPeople.Take(20));
                    return;
                }
                else
                {
                    for (int i = 0; i < listUsers.Count; i++)
                    {
                        if (listUsers[i].Photo == null) AllPeople.Add(new SearchBubble(listUsers[i]) { IsTag = false, ImageBytes = _Globals.Base64Bytes });
                        else AllPeople.Add(new SearchBubble(listUsers[i]) { IsTag = false });
                    }
                    AllPeople.OrderBy(x => string.Compare(x.Name, Search));
                    BubblesCollection.AddRange(AllPeople);
                    return;
                }
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                return;
            }
        }

        private async void SetTagsCollection(string value)
        {
            AllTags.Clear();
            BubblesCollection.Clear();
            var success = await ServerServices.SendPostRequestAsync($"search/tag/{value}", new JObject());
            await Task.Delay(1000);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var listTags = JsonConvert.DeserializeObject<List<string>>(earthquakesJson);

                if (listTags.Count >= 20)
                {
                    for (int i = 0; i < 20; i++)
                        AllTags.Add(new SearchBubble(listTags[i]) { IsTag = true });
                    AllTags.OrderBy(x => string.Compare(x.Name, Search));
                    BubblesCollection.AddRange(AllTags.Take(20));
                    return;
                }
                else
                {
                    for (int i = 0; i < listTags.Count; i++)
                        AllTags.Add(new SearchBubble(listTags[i]) { IsTag = true });
                    AllTags.OrderBy(x => string.Compare(x.Name, Search));
                    BubblesCollection.AddRange(AllTags);
                    return;
                }
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                return;
            }
        }

        //private async void SetCollcetions(string value)
        //{
        //    await Task.Delay(1000);

        //    AllBubbles.Clear();
        //    AllPeople.Clear();
        //    AllTags.Clear();

        //    var success = await ServerServices.SendPostRequestAsync($"search/tag/{value}", new JObject());

        //    if (success.IsSuccessStatusCode)
        //    {
        //        var earthquakesJson = success.Content.ReadAsStringAsync().Result;
        //        var listTags = JsonConvert.DeserializeObject<List<string>>(earthquakesJson);

        //        foreach (string s in listTags)
        //            AllBubbles.Add(new SearchBubble(s) { IsTag = true });

        //        foreach (string s in listTags)
        //            AllTags.Add(new SearchBubble(s) { IsTag = true });
        //        AllTags.OrderBy(x => string.Compare(x.Name, Search));
        //    }
        //    else if (success.StatusCode == HttpStatusCode.Unauthorized)
        //    {
        //        await ServerServices.RefreshTokenAsync();
        //    }

        //    success = await ServerServices.SendPostRequestAsync($"search/user/{value}", new JObject());

        //    if (success.IsSuccessStatusCode)
        //    {
        //        var earthquakesJson = success.Content.ReadAsStringAsync().Result;
        //        var listUsers = JsonConvert.DeserializeObject<List<UserDB>>(earthquakesJson);

        //        foreach (UserDB u in listUsers)
        //        {
        //            if (u.Photo == null)
        //                AllBubbles.Add(new SearchBubble(u) { IsTag = false, ImageBytes = _Globals.Base64Bytes });
        //            else
        //                AllBubbles.Add(new SearchBubble(u) { IsTag = false });
        //        }

        //        foreach (UserDB u in listUsers)
        //        {
        //            if (u.Photo == null)
        //                AllPeople.Add(new SearchBubble(u) { IsTag = false, ImageBytes = _Globals.Base64Bytes });
        //            else
        //                AllPeople.Add(new SearchBubble(u) { IsTag = false });
        //        }
        //        AllPeople.OrderBy(x => string.Compare(x.Name, Search));
        //    }
        //    else if (success.StatusCode == HttpStatusCode.Unauthorized)
        //    {
        //        await ServerServices.RefreshTokenAsync();
        //    }

        //    AllBubbles.OrderBy(x => string.Compare(x.Name, Search));

        //    switch (choice)
        //    {
        //        case 1:
        //            BubblesCollection.Clear();

        //            if (AllBubbles.Count >= 20)
        //                BubblesCollection.AddRange(AllBubbles.Take(20));
        //            else
        //                BubblesCollection.AddRange(AllBubbles);
        //            break;

        //        case 2:
        //            BubblesCollection.Clear();

        //            if (AllPeople.Count >= 20)
        //                BubblesCollection.AddRange(AllPeople.Take(20));
        //            else
        //                BubblesCollection.AddRange(AllPeople);
        //            break;

        //        case 3:
        //            BubblesCollection.Clear();

        //            if (AllTags.Count >= 20)
        //                BubblesCollection.AddRange(AllTags.Take(20));
        //            else
        //                BubblesCollection.AddRange(AllTags);
        //            break;

        //        default: break;
        //    }
        //}

        public SearchPageViewModel()
        {
            SearchAllFunction();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());

            SearchAllCommand = new Command(SearchAllFunction);
            SearchPeopleCommand = new Command(SearchPeopleFunction);
            SearchTagsCommand = new Command(SearchTagsFunctionAsync);
            OpenProfileCommand = new Command<SearchBubble>(OpenProfileFunctionAsync);
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
                OnPropertyChanged();
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

                SetAllCollection(value);

                OnPropertyChanged();
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // show everything command
        public ICommand SearchAllCommand { get; set; }
        private void SearchAllFunction()
        {
            choice = 1;
            SetAllCollection(Search);
        }

        // show only people command
        public ICommand SearchPeopleCommand { get; set; }
        private void SearchPeopleFunction()
        {
            choice = 2;
            SetPeopleCollection(Search);
        }

        // show only tags command
        public ICommand SearchTagsCommand { get; set; }
        private void SearchTagsFunctionAsync()
        {
            choice = 3;
            SetTagsCollection(Search);
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (IsBusy) return;

            IsBusy = true;
            await Task.Delay(1000);

            if(choice == 1)
            {
                if (AllBubbles.Count - BubblesCollection.Count <= 10) BubblesCollection.AddRange(AllBubbles.Skip(BubblesCollection.Count));
                else BubblesCollection.AddRange(AllBubbles.Skip(BubblesCollection.Count).Take(10));
                IsBusy = false;
                return;
            }
            if(choice == 2)
            {
                if (AllPeople.Count - BubblesCollection.Count <= 10) BubblesCollection.AddRange(AllPeople.Skip(BubblesCollection.Count));
                else BubblesCollection.AddRange(AllPeople.Skip(BubblesCollection.Count).Take(10));
                IsBusy = false;
                return;
            }
            if(choice == 3)
            {
                if (AllTags.Count - BubblesCollection.Count <= 10) BubblesCollection.AddRange(AllTags.Skip(BubblesCollection.Count));
                else BubblesCollection.AddRange(AllTags.Skip(BubblesCollection.Count).Take(10));
                IsBusy = false;
                return;
            }

            IsBusy = false;
            return;
        }

        // open profile command
        public ICommand OpenProfileCommand { get; set; }
        private async void OpenProfileFunctionAsync(SearchBubble sb)
        {
            if (sb.Name != _Globals.GlobalMainUser.Username)
            {
                _Globals.UsernameTemp = sb.Name;
                await Shell.Current.GoToAsync("PersonsProfilePage");
            }
            else
                await Shell.Current.GoToAsync("//ProfilePage");
        }

    }
}
