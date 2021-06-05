using BDProject.Helpers;
using BDProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            firstTime = true;
            BindingContext = new ProfilePageViewModel();
        }

        static bool firstTime = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_Globals.IsLeving || firstTime)
            {
                _Globals.IsLeving = false;
                firstTime = false;
            }
            else
                BindingContext = new ProfilePageViewModel();

        }
    }
}