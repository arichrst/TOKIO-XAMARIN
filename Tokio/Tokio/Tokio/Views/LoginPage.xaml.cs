using System;
using System.Collections.Generic;
using Tokio.Models;
using Tokio.Services;
using Xamarin.Forms;
using System.Linq;
using Acr.UserDialogs;

namespace Tokio.Views
{
    public partial class LoginPage : ContentPage
    {
        Database db = new Database();
        public User User { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            User = new User();
            BindingContext = this;
            App.MyStore = null;
            Label_Register.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new RegisterPage();
                })
            });
            Image_Logo.SetAnimation();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Image_Logo.RunAnimation(500);
        }

        async void Login_Clicked(object sender, EventArgs e)
        {
            Button_Login.IsEnabled = false;
            if (User.Username.IsEmpty())
            {
                UserDialogs.Instance.Toast("Nama pengguna tidak boleh kosong");
                Button_Login.IsEnabled = true;
                return;
            }
            if (User.Password.IsEmpty())
            {
                UserDialogs.Instance.Toast("Kata sandi tidak boleh kosong");
                Button_Login.IsEnabled = true;
                return;
            }
            using (UserDialogs.Instance.Loading("Harap sabar menunggu"))
            {
                var result = (await db.Users.GetItemsAsync(x => x.Username == User.Username && x.Password == User.Password)).FirstOrDefault();

                if (result == null)
                {
                    UserDialogs.Instance.Toast("Data tidak ditemukan, silahkan periksa kembali");
                    Button_Login.IsEnabled = true;
                    
                    return;
                }
                else
                {
                    UserDialogs.Instance.Toast("Selamat datang " + result.Name);
                    Button_Login.IsEnabled = true;
                    result.SaveAsProfile();
                }
            }
            Application.Current.MainPage = new NavigationPage( new StorePage());
        }
    }
}
