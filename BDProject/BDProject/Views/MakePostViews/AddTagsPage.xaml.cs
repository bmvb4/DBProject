using BDProject.ViewModels.MakePostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views.MakePostViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTagsPage : ContentPage
    {
        public AddTagsPage()
        {
            InitializeComponent();

            BindingContext = new AddTagsPageViewModel();
        }
    }
}