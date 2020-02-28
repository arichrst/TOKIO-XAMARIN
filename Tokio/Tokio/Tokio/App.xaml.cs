using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tokio.Services;
using Tokio.Views;
using Tokio.Models;

namespace Tokio
{
    public partial class App : Application
    {
        public static CashierPage Cashier { get; set; }
        public static Store MyStore { get; set; }
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public App()
        {
            InitializeComponent();
            if (new User().LoadProfile() == null)
                MainPage = new LoginPage();
            else
                MainPage = new NavigationPage(new StorePage()) ;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
