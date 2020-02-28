using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Tokio.Views
{
    public partial class InputQtyPopup : PopupPage
    {
        Action<double> Callback;
        public InputQtyPopup(Action<double> callback)
        {
            InitializeComponent();
            Callback = callback;
            Button_OK.Clicked += Button_OK_Clicked;
        }

        private void Button_OK_Clicked(object sender, EventArgs e)
        {
            try
            {
                Callback(double.Parse(Entry_QTY.Text.Replace(",",".")));
            }
            catch (Exception ex)
            {

            }
            
        }
    }
}
