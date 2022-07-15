using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BEAssistant.popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class popupModificarProceso : PopupPage
    {
        public popupModificarProceso()
        {
            InitializeComponent();
            Animacion();
        }

        private async void Animacion()
        {
            await Task.Delay(100);
            GuardarAlStock.IsVisible = true;
            await Task.Delay(200);
            ReportarComoVenta.IsVisible = true;
            await Task.Delay(200);
            Actualizar.IsVisible = true;
        }

        private async void GuardarAlStock_Clicked(object sender, EventArgs e)
        {
            Procesos up = new Procesos() { 
            Id = TabbedProductos.procesoSelected4.Id,
            Cantidad = TabbedProductos.procesoSelected4.Cantidad,
            Descripcion = TabbedProductos.procesoSelected4.Descripcion,
            EnProceso = false,
            FechaIni = TabbedProductos.procesoSelected4.FechaIni,
            IdPro = TabbedProductos.procesoSelected4.IdPro,
            TimeEsperado = TabbedProductos.procesoSelected4.TimeEsperado,
            TimeResultado = TabbedProductos.procesoSelected4.TimeResultado
            };await App.Database.SaveUpProcesos(up);

            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ReportarComoVenta_Clicked(object sender, EventArgs e)
        {
            await Task.Delay(100);
            GuardarAlStock.IsVisible = false;
            await Task.Delay(200);
            ReportarComoVenta.IsVisible = false;
            await Task.Delay(200);
            Actualizar.IsVisible = false;
            await Task.Delay(100);
            lavel.IsVisible = true;
            await Task.Delay(200);
            EntryCosto.IsVisible = true;
            await Task.Delay(200);
            reportar.IsVisible = true;
        }

        private async void Actualizar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupUpdateProceso());
        }

        private async void reportar_Clicked(object sender, EventArgs e)
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            var time = TimeActual - TabbedProductos.procesoSelected4.FechaIni;
            int result = Convert.ToInt32(time.TotalDays);
            Procesos up = new Procesos()
            {
                Id = TabbedProductos.procesoSelected4.Id,
                Cantidad = TabbedProductos.procesoSelected4.Cantidad,
                Descripcion = TabbedProductos.procesoSelected4.Descripcion,
                EnProceso = false,
                FechaIni = TabbedProductos.procesoSelected4.FechaIni,
                IdPro = TabbedProductos.procesoSelected4.IdPro,
                TimeEsperado = TabbedProductos.procesoSelected4.TimeEsperado,
                TimeResultado = result
            }; await App.Database.SaveUpProcesos(up);

            await App.Database.SaveVenta(new Venta()
            {
                Descripcion = TabbedProductos.procesoSelected4.Descripcion,
                Fecha = TimeActual,
                IdPro = TabbedProductos.procesoSelected4.IdPro,
                Precio = Convert.ToDouble(EntryCosto.Text),
                Unidades = TabbedProductos.procesoSelected4.Cantidad
            });
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}