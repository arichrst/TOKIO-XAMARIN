using System;
using System.Collections.Generic;
using Tokio.Models;
using Xamarin.Forms;

namespace Tokio.Views
{
    public partial class CashierItemView : ContentView
    {
        public CashierItemView()
        {
            InitializeComponent();
            Content.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    //var product = (this.BindingContext as Product);
                    //App.Cashier.CashierViewModel.AddItems(product, 1);
                })
            });
        }
    }
}
