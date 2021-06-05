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

        private static bool firstTime = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!firstTime)
            {
                var vm = (ProfilePageViewModel)this.BindingContext;
                vm.SetUserData();
                firstTime = false;
            }

        }
    }
}