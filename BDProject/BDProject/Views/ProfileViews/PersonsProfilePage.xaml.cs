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
            try
            {
                InitializeComponent();

                BindingContext = new PersonsProfilePageViewModel();
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
            //InitializeComponent();

            //BindingContext = new PersonsProfilePageViewModel();
        }
    }
}