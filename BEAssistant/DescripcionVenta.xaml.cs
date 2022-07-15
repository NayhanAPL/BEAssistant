using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class DescripcionVenta : PopupPage
    {
        public DescripcionVenta()
        {
            InitializeComponent();
        }

        private async void ButtonAgregarDescripcionVenta_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntryVentaDescripcion.Text))
                TabbedProductos.descripcion = EntryVentaDescripcion.Text;
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}