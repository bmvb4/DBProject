using BDProject.ViewModels.PostsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views.PostsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPostPage : ContentPage
    {
        public EditPostPage()
        {
            InitializeComponent();

            tagsEditor.Unfocus();

            BindingContext = new EditPostPageViewModel();
        }
    }
}