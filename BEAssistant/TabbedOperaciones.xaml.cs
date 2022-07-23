using BEAssistant.popups;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BEAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedOperaciones : PopupPage
    {
        public TabbedOperaciones()
        {
            InitializeComponent();
            Animacion();
        }

        private async void Animacion()
        {
            await Task.Delay(100);
            Encargos.IsVisible = true;
            await Task.Delay(100);
            StockProductos.IsVisible = true;
            await Task.Delay(100);
            Inventario.IsVisible = true;
            await Task.Delay(100);
            VerStock.IsVisible = true;
            await Task.Delay(100);
            Contactos.IsVisible = true;
        }

        private async void Contactos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Contactos());
        }

        private async void VerStock_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupVerStock());
        }

        private async void Inventario_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupInventario());
        }

        private async void StockProductos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupStockProductos());
        }

        private void Encargos_Clicked(object sender, EventArgs e)
        {

        }
    }
}