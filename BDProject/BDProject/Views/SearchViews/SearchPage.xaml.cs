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
        }

        private void Searcher_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Searcher.Text)) { return; }

            UTF8Encoding utf8 = new UTF8Encoding(true, true);
            Byte[] encodedBytes = utf8.GetBytes(Searcher.Text);

            string encodedBytesString = "";
            for(int i=0; i<encodedBytes.Length; i++)
            {
                if (i == 0) { encodedBytesString = encodedBytesString + encodedBytes[i]; }

                encodedBytesString = encodedBytesString + " " + encodedBytes[i];
            }

            emojicode.Text = encodedBytesString;

            String decodedString = utf8.GetString(encodedBytes);
            emojiicon.Text = decodedString;
        }
    }
}