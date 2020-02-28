using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Tokio.Models;
using Tokio.Services;
using Xamarin.Forms;
using System.Linq;

namespace Tokio.Views
{
    public partial class StorePage : ContentPage
    {
        Database db = new Database();
        public ObservableCollection<Store> Stores { get; set; }
        public Command LoadDataCommand { get; set; }
        public bool ListVisible { get { return Stores.Count > 0; } }
        public bool EmptyListVisible { get { return Stores.Count == 0; } }
        public StorePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Stores = new ObservableCollection<Store>();
            LoadDataCommand = new Command(async () =>
            {
                await LoadData();
            });
            App.MyStore = null;
            Image_Store.SetAnimation();
            Image_Logo.SetAnimation();
            BindingContext = this;
            var user = new User().LoadProfile();
            Button_AddStore.IsVisible = user.Type == UserType.Admin;
            
            Label_Description.Text = user.Type != UserType.Admin ? "Owner kamu belum mendaftarkan tokonya, coba ingetin lagi" : Label_Description.Text;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Store;
            if (item == null)
                return;
            App.MyStore = item;
            StoreListView.SelectedItem = null;
            await Navigation.PushAsync(new HomePage());
            return;
        }

        async void AddStore_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CRUDStore());
        }

        async Task LoadData()
        {
            using (UserDialogs.Instance.Loading("Mengunduh data dari server"))
            {
                if (IsBusy) return;
                IsBusy = true;

                
                try
                {
                    Stores.Clear();
                    var items = await db.Stores.GetItemsAsync();
                    items = items.OrderBy(x => x.Name);
                    foreach (var item in items)
                    {
                        Stores.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
                if (Stores.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //(Scroll_Empty.Parent as StackLayout).Children.RemoveAt(0);
                        StoreListView.IsVisible = Layout_Header.IsVisible = true;
                        Scroll_Empty.IsVisible = false;
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //(Scroll_Empty.Parent as StackLayout).Children.RemoveAt(0);
                        StoreListView.IsVisible = Layout_Header.IsVisible = false;
                        Scroll_Empty.IsVisible = true;
                    });
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadData();
        }
    }
}
