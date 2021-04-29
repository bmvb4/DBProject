using BDProject.Helpers;
using BDProject.Models;
using BDProject.ViewModels;
using BDProject.ViewModels.SearchViewModels;
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
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            BindingContext = new HomePageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_Globals.Refresh == true)
            {
                var vm = (HomePageViewModel)this.BindingContext;
                vm.SetCollection();
                _Globals.Refresh = false;
            }
        }
    }
}