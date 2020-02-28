using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Tokio.Models;
using Tokio.Models;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;

namespace Tokio.Views
{
    public partial class ThumbProductItemView : ContentView
    {
        public ThumbProductItemView()
        {
            InitializeComponent();

            Content.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await PopupNavigation.PushAsync(new InputQtyPopup(async (qty)=> {
                        var product = (this.BindingContext as Product);
                        App.Cashier.CashierViewModel.AddItems(product, qty);
                        UserDialogs.Instance.Toast(product.Name + " ditambahkan sebanyak " + qty.ToString() + " " + product.Denomination);
                        await PopupNavigation.PopAsync();
                    }), true);
                    
                })
            });
        }
    }
}
