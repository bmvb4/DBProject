using BDProject.ViewModels.SettingsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views.SettingsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageAccountPage : ContentPage
    {
        public ManageAccountPage()
        {
            InitializeComponent();

            BindingContext = new ManageAccountPageViewModel();
        }
    }
}