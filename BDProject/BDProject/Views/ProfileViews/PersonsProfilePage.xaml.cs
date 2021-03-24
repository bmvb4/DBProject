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

        //private void FollowProfileFunction_Click(object sender, EventArgs e)
        //{
        //    if (FollowButton.Text == "Follow")
        //    {
        //        _Globals.GlobalMainUser.AddFollowing(UsernameLabel.Text);

        //        FollowButton.Text = "Following";
        //    }
        //    else
        //    {
        //        _Globals.GlobalMainUser.RemoveFollowing(UsernameLabel.Text);

        //        FollowButton.Text = "Follow";
        //    }
        //}
    }
}