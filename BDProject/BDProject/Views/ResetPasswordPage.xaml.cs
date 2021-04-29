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
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage()
        {
            InitializeComponent();

            codeEntry.Unfocus();
            passwordEntry.Unfocus();
            confirmPasswordEntry.Unfocus();

            BindingContext = new ResetPasswordPageViewModel();
        }
    }
}