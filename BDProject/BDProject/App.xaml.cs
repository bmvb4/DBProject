using BDProject.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            SetColors();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //Preferences.Set("LastPage", Shell.Current.CurrentPage.Title);
        }

        protected override void OnResume()
        {
            //Shell.Current.GoToAsync(Preferences.Get("LastPage", string.Empty));
        }

        private void SetColors()
        {
            string IconsColor = Preferences.Get("IconsColor", string.Empty);
            string TextColor = Preferences.Get("TextColor", string.Empty);
            string BackgroundColor = Preferences.Get("BackgroundColor", string.Empty);

            if (IconsColor == string.Empty || TextColor == string.Empty || BackgroundColor == string.Empty)
            {
                Application.Current.Resources["IconsColor"] = Color.Black;
                Application.Current.Resources["TextColor"] = Color.Black;
                Application.Current.Resources["BackgroundColor"] = Color.White;
            }
            else
            {
                Application.Current.Resources["IconsColor"] = Color.FromHex(IconsColor);
                Application.Current.Resources["TextColor"] = Color.FromHex(TextColor);
                Application.Current.Resources["BackgroundColor"] = Color.FromHex(BackgroundColor);
            }
        }
    }
}