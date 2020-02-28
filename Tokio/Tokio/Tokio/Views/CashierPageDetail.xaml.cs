using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tokio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashierPageDetail : ContentPage
    {
        public CashierViewModel CashierViewModel { get; set; }
        public CashierPageDetail()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            Button_Save.GestureRecognizers.Add(new TapGestureRecognizer() {
                Command = new Command(() => {
                    if (CashierViewModel.Customer == null)
                    {
                        Acr.UserDialogs.UserDialogs.Instance.Confirm(new ConfirmConfig()
                        {
                            Title = "Simpan Transaksi",
                            Message = "Data transaksi ini akan disimpan dan dapat",
                            CancelText = "Batalkan",
                            OkText = "Oke",
                            OnAction = (result) =>
                            {
                                if (result)
                                {
                                    CashierViewModel.Save(false, 0, "PENDING", () =>
                                    {
                                    });
                                    CashierViewModel.CreateNewOne();
                                }
                            }
                        });
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Konsumen belum diisi");
                    }
                })
            });
        }
    }
}
