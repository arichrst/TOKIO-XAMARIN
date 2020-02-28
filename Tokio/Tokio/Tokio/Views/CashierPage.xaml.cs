using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tokio.Models;
using Tokio.Services;
using System.Collections.ObjectModel;
using Acr.UserDialogs;

namespace Tokio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashierPage : MasterDetailPage
    {
        public Database Db { get; set; }
        public CashierViewModel CashierViewModel { get; set; }
        public CashierPage(CashierViewModel cashierViewModel)
        {
            InitializeComponent();
            FlowDirection = FlowDirection.LeftToRight;
            Db = new Database();
            CashierViewModel = cashierViewModel;
            BindingContext = this;
            MasterPage.CashierViewModel = CashierViewModel;
            DetailPage.CashierViewModel = CashierViewModel;
            MasterBehavior = MasterBehavior.SplitOnLandscape;
            App.Cashier = this;
            this.IsPresented = true;
            NavigationPage.SetHasNavigationBar(this, false);
            DetailPage.BindingContext = CashierViewModel;
            MasterPage.BindingContext = CashierViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this,"allowLandScapePortrait");
        }
        //during page close setting back to portrait
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "preventLandScape");
        }

        protected override bool OnBackButtonPressed()
        {
            bool exit = true;
            Acr.UserDialogs.UserDialogs.Instance.Confirm(new ConfirmConfig()
            {
                Title = "Kamu yakin akan keluar dari menu kasir?",
                Message = "Ketika kamu keluar, transaksi yang belum diproses tidak akan tersimpan",
                CancelText = "Batalkan",
                OkText = "Oke",
                OnAction = async (result) =>
                {
                    if (result)
                    {
                        await this.Navigation.PopAsync();
                    }
                }
            });
            return exit;
        }
    }

    

    public class CashierViewModel
    {
        public Database Database { get; set; }
        public Store Store { get; set; }
        public User User { get; set; }
        public List<Product> Products { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Category> Categories { get; set; }
        public Transaction Order { get; set; }
        public Customer Customer { get; set; }
        public CashierPage CashierPage { get; set; }
        public string TaxInfo { get { return "PAJAK " + Store.Tax.ToString() + "%"; } }
        public double Tax { get { return (TotalPrice * Store.Tax / 100); } }
        public string TaxFormatted { get { return Tax.ToRupiah(); } }
        public string ServiceInfo { get { return "LAYANAN " + Store.ServiceCharge.ToString() + "%"; } }
        public double Service { get { return (TotalPrice * Store.ServiceCharge / 100); } }
        public string ServiceFormatted { get { return Service.ToRupiah(); } }
        public double TotalPrice { get {
                try
                {
                    return Order.TransactionDetails.Sum(x => x.Total);
                }
                catch { return 0; }
            } }
        public string TotalPriceFormatted { get { return TotalPrice.ToRupiah(); } }
        public double GrandTotal
        {
            get
            {
                try
                {
                    return TotalPrice + Tax + Service;
                }
                catch { return 0; }
            }
        }
        public string GrandTotalFormatted
        {
            get
            {
                return GrandTotal.ToRupiah();
            }
        }
        

        public CashierViewModel()
        {
            Store = App.MyStore;
            User = new User().LoadProfile();
            Database = new Database();
            CreateNewOne();
        }

        public async void Load(Transaction order)
        {
            Order = order;
            try
            {
                Order.TransactionDetails = new ObservableCollection<TransactionDetail>(await Database.TransactionDetails.GetItemsAsync(x => x.TransactionId == order.Id));
            }
            catch
            {
                Order.TransactionDetails = new ObservableCollection<TransactionDetail>();
            }
        }

        public void CreateNewOne()
        {
            Order = new Transaction();
            Customer = new Customer();
            
            Order.TransactionDetails = new ObservableCollection<TransactionDetail>();
            Order.Created = DateTime.Now;
            Order.User = User;
            Order.UserId = User.Id;
            Order.Store = Store;
            Order.StoreId = Store.Id;
            ClearAllItems();
        }

        public void ClearAllItems()
        {
            foreach (var item in Order.TransactionDetails)
            {
                Order.TransactionDetails.Remove(item);
            }
        }

        public void AddItems(Product product, double qty)
        {
            if (Order.TransactionDetails.Any(x => x.ProductId == product.Id))
                EditItems(Order.TransactionDetails.FirstOrDefault(x => x.ProductId == product.Id), qty);
            else
            {
                Order.TransactionDetails.Add(new TransactionDetail()
                {
                    IsMemberTransaction = Customer.Member == CustomerType.Member,
                    Price = Customer.Member == CustomerType.Member ? product.MemberPrice : product.Price,
                    Product = product,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Qty = qty,
                    Total = Customer.Member == CustomerType.Member ? product.MemberPrice * qty : product.Price * qty,
                    Transaction = Order,
                    TransactionId = Order.Id,
                    Denomination = product.Denomination,
                    ImageUrl = product.ImageUrl
                });
            }
            UpdateUI();
        }

        private void EditItems(TransactionDetail item, double qty)
        {
            Order.TransactionDetails.Remove(item);
            item.Qty += qty;
            item.Total = Customer.Member == CustomerType.Member ? item.Product.MemberPrice * item.Qty : item.Product.Price * item.Qty;
            Order.TransactionDetails.Add(item);
        }

        private void UpdateUI()
        {
            var master = CashierPage.Master as CashierPageMaster;
            master.UpdateTransactionUI();
        }

        public async Task Save(bool isPaid,double paymentAmount, string paymentMethod , Action callback = null)
        {
            Order.Created = DateTime.Now;
            Order.Customer = Customer;
            Order.CustomerId = Customer.Id;
            Order.IsMemberTransaction = Customer.Member == CustomerType.Member;
            Order.IsReceived = true;
            Order.PaymentMethod = paymentMethod;
            Order.IsPaid = isPaid;
            Order.GrandTotal = this.GrandTotal;
            Order.Tax = this.Tax;
            Order.Discount = 0;
            Order.IsFinished = false;
            Order.IsOnProgress = false;
            Order.IsReceived = true;
            if (isPaid)
                Order.PaidDate = DateTime.Now;
            Order.PaymentAmount = paymentAmount;
            Order.Return = paymentAmount - GrandTotal;
            Order.ServiceCharge = this.Service;
            Order.Store = this.Store;
            Order.StoreId = this.Store.Id;
            Order.Total = this.TotalPrice;
            Order.User = this.User;
            Order.UserId = this.User.Id;
            
            if (await CashierPage.Db.Transactions.AddItemAsync(Order))
            {
                foreach (var item in Order.TransactionDetails)
                {
                    await Database.TransactionDetails.AddItemAsync(item);
                }
                UserDialogs.Instance.Toast("Order nomor " + Order.Code + " berhasil disimpan");
                CreateNewOne();
                if(callback != null)callback();
            }
            else
            {
                UserDialogs.Instance.Toast("Terjadi kesalahan dalam menyimpan transaksi, silahkan coba kembali");
            }
        }
    }
}
