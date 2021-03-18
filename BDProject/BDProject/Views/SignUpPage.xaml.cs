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
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();

            firstnameEntry.Unfocus();
            lastnameEntry.Unfocus();
            usernameEntry.Unfocus();
            passwordEntry.Unfocus();
            confirmEntry.Unfocus();
            emailEntry.Unfocus();

            BindingContext = new SignUpPageViewModel();
        }
    }
}