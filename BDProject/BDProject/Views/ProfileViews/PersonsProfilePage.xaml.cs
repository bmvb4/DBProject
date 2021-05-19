using BDProject.Helpers;
using BDProject.ViewModels.ProfileViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views.ProfileViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonsProfilePage : ContentPage
    {
        public PersonsProfilePage()
        {
            InitializeComponent();

            BindingContext = new PersonsProfilePageViewModel();
        }
    }
}