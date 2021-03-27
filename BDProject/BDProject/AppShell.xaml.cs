using BDProject.Views;
using BDProject.Views.MakePostViews;
using BDProject.Views.PostsViews;
using BDProject.Views.ProfileViews;
using BDProject.Views.SearchViews;
using BDProject.Views.SettingsViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // pages for posts
            Routing.RegisterRoute("PostComments", typeof(PostCommentsPage));
            Routing.RegisterRoute("EditPostPage", typeof(EditPostPage));

            // pages for settings
            Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
            Routing.RegisterRoute("ManageAccountPage", typeof(ManageAccountPage));

            // pages for making a post
            Routing.RegisterRoute("AddTagsPage", typeof(AddTagsPage));
            
            // pages for search
            Routing.RegisterRoute("SearchPage", typeof(SearchPage));

            // pages in profiles
            Routing.RegisterRoute("PersonsProfilePage", typeof(PersonsProfilePage));
            Routing.RegisterRoute("EditProfilePage", typeof(EditProfilePage));
            Routing.RegisterRoute("ProfilePostPage", typeof(ProfilePostPage));
        }
    }
}