using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tokio.Models;
using Tokio.Services;
using Xamarin.Forms;
using System.Linq;
using System.Reactive.Linq;
using Acr.UserDialogs;

namespace Tokio.Views
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<Models.Menu> Menus { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }

        CashierViewModel cashierViewModel;
        CashierPage cashierPage;

        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            User = User.LoadProfile();
            Store = App.MyStore;
            Menus = new ObservableCollection<Models.Menu>();
            List<Models.Menu> menus = new List<Models.Menu>();
            menus.Add(new Models.Menu("Kasir", async () => {

                cashierViewModel = new CashierViewModel();
                Database Db = new Database();
                using (UserDialogs.Instance.Loading("Harap sabar menunggu"))
                {
                    bool loadFromCache = true;
                    var products = await Db.Products.GetItemsAsync(null, loadFromCache);
                    cashierViewModel.Products = products == null ? new List<Product>() : products.ToList().OrderBy(x => x.CategoryId).ToList();
                    var customers = await Db.Customers.GetItemsAsync(null, loadFromCache);
                    cashierViewModel.Customers = customers == null ? new List<Customer>() : customers.ToList();
                    var categories = await Db.Categories.GetItemsAsync(null, loadFromCache);
                    cashierViewModel.Categories = categories == null ? new List<Category>() : categories.ToList();
                }
                cashierPage = new CashierPage(cashierViewModel);
                cashierViewModel.CashierPage = cashierPage;

                await Navigation.PushAsync(cashierPage);
            }, "cashier.png", UserType.NonAdmin,User));

            menus.Add(new Models.Menu("Karyawan", () => {

            }, "employees.png", UserType.Manager, User));

            menus.Add(new Models.Menu("Order", () => {

            }, "order.png", UserType.NonAdmin, User));

            menus.Add(new Models.Menu("Scan Code", () => {

            }, "barcode.png", UserType.NonAdmin, User));

            menus.Add(new Models.Menu("Buku Kas", () => {

            }, "homework.png", UserType.Manager, User));

            menus.Add(new Models.Menu("Penutupan Shift", () => {

            }, "shift.png", UserType.NonAdmin, User));

            menus.Add(new Models.Menu("Konsumen", () => {

            }, "value.png", UserType.NonAdmin, User));

            menus.Add(new Models.Menu("Member", () => {

            }, "lock.png", UserType.NonAdmin, User));

            menus.Add(new Models.Menu("Profil",async() => {
                await Navigation.PushAsync(new RegisterPage(User));
            }, "user.png", UserType.NonAdmin, User));

            menus.Add(new Models.Menu("Produk", () => {

            }, "product.png", UserType.Admin, User));

            menus.Add(new Models.Menu("Kategori", () => {

            }, "box.png", UserType.Admin, User));

            menus.Add(new Models.Menu("Setting Toko", async () => {
                await Navigation.PushAsync(new CRUDStore(App.MyStore,true));
            }, "settings.png", UserType.Admin, User));

            menus.Add(new Models.Menu("Buku Besar", () => {

            }, "ledger.png", UserType.Admin, User));

            menus.Add(new Models.Menu("Keluar", () => {
                Acr.UserDialogs.UserDialogs.Instance.Confirm(new ConfirmConfig()
                {
                    Title = "Kamu yakin akan keluar dari aplikasi ini?",
                    Message = "Ketika kamu keluar, kamu akan diminta untuk mengisi kredensial lagi",
                    CancelText = "Batalkan",
                    OkText = "Oke",
                    OnAction = (result) =>
                    {
                        if (result)
                        {
                            User.RemoveProfile();
                            Application.Current.MainPage = new LoginPage();
                        }
                    }
                });
            }, "exit.png", UserType.NonAdmin, User));
            
            Image_Profile.SetAnimation();
            menus = menus.Where(x => x.IsVisible).ToList();
            foreach (var item in menus)
            {
                Menus.Add(item);
            }
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Image_Profile.RunAnimation(500);
            User = User.LoadProfile();
            Image_Profile.Source = User.Thumbnail;
            Store = App.MyStore;
            Label_Description.Text = Store.Name;
            Label_Name.Text = User.Name;
            //await Seeder.InitSeeder();
        }

    }
}
