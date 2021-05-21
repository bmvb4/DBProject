using BDProject.Helpers;
using BDProject.Models;
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
    public partial class MakePostPage : ContentPage
    {
        public MakePostPage()
        {
            InitializeComponent();

            descriptionEditor.Unfocus();
            tagsEditor.Unfocus();

            BindingContext = new MakePostPageViewModel();
        }
    }
}