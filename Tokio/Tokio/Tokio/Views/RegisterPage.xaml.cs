using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Tokio.Models;
using Tokio.Services;
using System.Linq;
using Acr.UserDialogs;
using System.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media.Abstractions;
using Plugin.Media;

namespace Tokio.Views
{
    public partial class RegisterPage : ContentPage
    {
        Database db = new Database();
        public User User { get; set; }
        public MediaFile Stream { get; set; }
        public bool IsEdit { get; set; }
        public RegisterPage(User user = null)
        {
            InitializeComponent();
            User = user == null ? new User() : user;
            IsEdit = user != null;
            BindingContext = this;
            App.MyStore = IsEdit ? App.MyStore : null;

            if (IsEdit)
            {
                Label_Login.IsVisible = false;
                Entry_Username.IsEnabled = false;
                Image_Profile.Source = User.Thumbnail;
            }


            NavigationPage.SetHasNavigationBar(this, false);
            Label_Login.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new LoginPage();
                })
            });
            Image_Logo.SetAnimation();
            Image_Profile.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async() => {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                    if (status != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                        {
                           
                        }

                        var x = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera , Permission.Photos , Permission.MediaLibrary);
                        
                    }

                    await ImagePicker.PickImage((stream) => {
                        Stream = stream;
                        if (Stream != null)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                Image_Profile.Source = ImageSource.FromStream(() => { return File.Open(Stream.Path, FileMode.Open); });
                            });
                        }
                    });
                    
                })
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Image_Logo.RunAnimation(500);
        }

        async void Submit_Clicked(object sender, EventArgs e)
        {
            Button_Submit.IsEnabled = false;
            if (User.Name.IsEmpty())
            {
                UserDialogs.Instance.Toast("Nama lengkap tidak boleh kosong");
                Button_Submit.IsEnabled = true;
                return;
            }
            if (User.Phone.IsEmpty())
            {
                UserDialogs.Instance.Toast("Nomor telepon tidak boleh kosong");
                Button_Submit.IsEnabled = true;
                return;
            }
            if (User.Username.IsEmpty())
            {
                UserDialogs.Instance.Toast("Nama pengguna tidak boleh kosong");
                Button_Submit.IsEnabled = true;
                return;
            }
            if (User.Password.IsEmpty())
            {
                UserDialogs.Instance.Toast("Kata sandi tidak boleh kosong");
                Button_Submit.IsEnabled = true;
                return;
            }
            using (UserDialogs.Instance.Loading("Harap sabar menunggu"))
            {
                if (IsEdit)
                {
                    if (Stream != null)
                    {
                        User.ImageUrl = await db.Users.Upload(Stream.GetStream(), User.Id);
                    }
                    if (await db.Users.UpdateItemAsync(User))
                    {
                        User.SaveAsProfile();
                        UserDialogs.Instance.Toast("Profil berhasil diubah");
                        Button_Submit.IsEnabled = true;

                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Ada kesalahan dalam mengubah profil");
                        Button_Submit.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    var result = (await db.Users.GetItemsAsync(x => x.Username == User.Username.Replace("**", "").Replace("##", ""))).FirstOrDefault();
                    if (result == null)
                    {
                        if (Stream != null)
                        {
                            User.ImageUrl = await db.Users.Upload(Stream.GetStream(), User.Id);
                        }
                        if (User.Username.Contains("**"))
                            User.Type = UserType.Admin;
                        else if (User.Username.Contains("##"))
                            User.Type = UserType.Manager;
                        else
                            User.Type = UserType.NonAdmin;
                        User.Username = User.Username.Replace("**", "").Replace("##", "");
                        if (await db.Users.AddItemAsync(User))
                        {
                            User.SaveAsProfile();
                            UserDialogs.Instance.Toast("Selamat datang " + User.Name);
                            Button_Submit.IsEnabled = true;

                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Ada kesalahan dalam registrasi");
                            Button_Submit.IsEnabled = true;
                            return;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Nama pengguna sudah pernah digunakan");
                        Button_Submit.IsEnabled = true;
                        return;
                    }
                }
            }
            if (IsEdit)
            {
                await this.Navigation.PopAsync();
            }
            else
                Application.Current.MainPage = new NavigationPage(new StorePage());
        }
    }
}
