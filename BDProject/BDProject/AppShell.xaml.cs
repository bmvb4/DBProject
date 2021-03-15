using BDProject.Views;
using BDProject.Views.PostsViews;
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

            Routing.RegisterRoute("PostComments", typeof(PostCommentsPage));
            Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
        }
    }
}