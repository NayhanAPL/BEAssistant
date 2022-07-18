using BEAssistant.popups;
using Rg.Plugins.Popup.Extensions;
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
    public partial class TabbedOperaciones : TabbedPage
    {
        public TabbedOperaciones()
        {
            InitializeComponent();
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
    }
}