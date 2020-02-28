using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Tokio.Models;
using Tokio.Services;

using Xamarin.Forms;

namespace Tokio.Views
{
    public partial class CRUDStore : ContentPage
    {
        Database db = new Database();
        public Store Store { get; set; }
        public bool IsEdit { get; set; }
        public bool SetAsDefault { get; set; }
        public string ButtonText { get { return IsEdit ? "Submit" : "Ubah"; } }
        public CRUDStore(Store store = null , bool setAsDefault = true)
        {
            InitializeComponent();
            SetAsDefault = setAsDefault;
            IsEdit = store != null;
            if (!IsEdit) Store = new Store();
            else Store = store;
            NavigationPage.SetHasNavigationBar(this, false);

            Image_Dana.SetAnimation();
            Image_GoPay.SetAnimation();
            Image_OVO.SetAnimation();
            Image_LinkAja.SetAnimation();
            BindingContext = this;
#if __ANDROID__
            // Initialize the scanner first so it can track the current context
            MobileBarcodeScanner.Initialize(Application);
#endif

            CheckPermission();

            Image_Dana.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async() => {
                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                    var result = await scanner.Scan();
                    Entry_Dana.Text = result == null ? "" : result.Text;
                })
            });

            Image_GoPay.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () => {
                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                    var result = await scanner.Scan();
                    Entry_GoPay.Text = result == null ? "" : result.Text;
                })
            });

            Image_LinkAja.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () => {
                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                    var result = await scanner.Scan();
                    Entry_LinkAja.Text = result == null ? "" : result.Text;
                })
            });

            Image_OVO.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () => {
                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                    var result = await scanner.Scan();
                    Entry_OVO.Text = result == null ? "" : result.Text;
                })
            });
            Button_Submit.Text = ButtonText;
        }

        async void CheckPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                {

                }

                var x = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera, Permission.Photos, Permission.MediaLibrary);

            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Image_OVO.RunAnimation(500);
            await Image_GoPay.RunAnimation(500);
            await Image_LinkAja.RunAnimation(500);
            await Image_Dana.RunAnimation(500);

        }

        async void Submit_Clicked(object sender, EventArgs e)
        {
            Button_Submit.IsEnabled = false;
            if (Store.Name.IsEmpty())
            {
                UserDialogs.Instance.Toast("Nama toko tidak boleh kosong");
                Button_Submit.IsEnabled = true;
                return;
            }
            if (Store.Address.IsEmpty())
            {
                UserDialogs.Instance.Toast("Alamat tidak boleh kosong");
                Button_Submit.IsEnabled = true;
                return;
            }
            using (UserDialogs.Instance.Loading("Harap sabar menunggu"))
            {
                if (IsEdit)
                {
                    if (await db.Stores.UpdateItemAsync(Store))
                    {
                        if(SetAsDefault)
                            App.MyStore = Store;
                        UserDialogs.Instance.Alert("Toko " + Store.Name + " berhasil diubah", "Selamat!");
                        Button_Submit.IsEnabled = true;
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Mengubah toko mengalami kendala, silahkan coba lagi atau cek kembali data yang dimasukkan");
                        Button_Submit.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    var result = (await db.Stores.GetItemsAsync(x => x.Name == Store.Name)).FirstOrDefault();

                    if (result == null)
                    {
                        Store.UserId = new User().LoadProfile().Id;
                        if (await db.Stores.AddItemAsync(Store))
                        {
                            if (SetAsDefault)
                                App.MyStore = Store;
                            UserDialogs.Instance.Alert("Toko " + Store.Name + " berhasil dibuat", "Selamat!");
                            Button_Submit.IsEnabled = true;
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Menambah toko mengalami kendala, silahkan coba lagi atau cek kembali data yang dimasukkan");
                            Button_Submit.IsEnabled = true;
                            return;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Nama toko sudah dipakai");
                        Button_Submit.IsEnabled = true;
                        return;
                    }
                }
            }
            await Navigation.PopAsync();
        }
    }
}

