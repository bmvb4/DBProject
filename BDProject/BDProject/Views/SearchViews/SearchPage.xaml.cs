using BDProject.Helpers;
using BDProject.ViewModels.SearchViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            
            BindingContext = new SearchPageViewModel();
        }

        private async void Searcher_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            if (EmojiHandler.HasUnsoppertedCharacter(Searcher.Text))
            {
                await App.Current.MainPage.DisplayAlert("===TEST===", "Has Emoji", "OK");
            }
        }
    }
}