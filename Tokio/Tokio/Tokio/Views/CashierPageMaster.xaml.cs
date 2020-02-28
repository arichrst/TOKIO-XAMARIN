using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tokio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashierPageMaster : ContentPage
    {
        public CashierViewModel CashierViewModel { get; set; }
        public CashierPageMaster()
        {
            InitializeComponent();
            
            BindingContext = CashierViewModel;
        }

        public void UpdateTransactionUI()
        {
            Device.BeginInvokeOnMainThread(() => {
                Text_Total.Text = CashierViewModel.TotalPriceFormatted;
                Text_Tax.Text = CashierViewModel.TaxFormatted;
                Text_Service.Text = CashierViewModel.ServiceFormatted;
                Text_GrandTotal.Text = CashierViewModel.GrandTotalFormatted;
            });
        }
        
    }
}
