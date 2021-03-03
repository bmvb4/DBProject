using BDProject.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject
{
    public partial class App : Application
    {
        public int UserID = 0;

        public App()
        {
            InitializeComponent();

            MainPage = new LogInPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
