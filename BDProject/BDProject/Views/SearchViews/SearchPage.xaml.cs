using BDProject.Helpers;
using BDProject.ViewModels.SearchViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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

            SearchAll.BackgroundColor = Color.LightGray;
        }

        private void SearchAll_Tapped(object sender, EventArgs e)
        {
            SearchAll.BackgroundColor = Color.LightGray;

            SearchPeople.BackgroundColor = Color.Black;
            SearchTags.BackgroundColor = Color.Black;
        }

        private void SearchPeople_Tapped(object sender, EventArgs e)
        {
            SearchPeople.BackgroundColor = Color.LightGray;

            SearchAll.BackgroundColor = Color.Black;
            SearchTags.BackgroundColor = Color.Black;
        }

        private void SearchTags_Tapped(object sender, EventArgs e)
        {
            SearchTags.BackgroundColor = Color.LightGray;

            SearchPeople.BackgroundColor = Color.Black;
            SearchAll.BackgroundColor = Color.Black;
        }
    }
}