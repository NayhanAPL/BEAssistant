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
            SGuardarAlStock.IsVisible = true;
            await Task.Delay(100);
            SReportarComoVenta.IsVisible = true;
            await Task.Delay(100);
            SActualizar.IsVisible = true;
            await Task.Delay(100);
            SDescripcion.IsVisible = true;
        }

        private async void GuardarAlStock_Clicked(object sender, EventArgs e)
        {
            var elem = await App.Database.GetByIdProStockProductos(TabbedProductos.procesoSelected4.IdPro);
            if(elem.Count > 0)
            {
                StockProductos stock = new StockProductos()
                {
                    Cantidad = elem[0].Cantidad + TabbedProductos.procesoSelected4.Cantidad,
                    Id = elem[0].Id,
                    IdPro = elem[0].IdPro,
                    Nombre = elem[0].Nombre
                }; await App.Database.SaveUpStockProductos(stock);
            }
            else
            {
                var pro = await App.Database.GetIdProductos(TabbedProductos.procesoSelected4.IdPro);
                await App.Database.SaveStockProductos(new StockProductos() {
                    Cantidad = TabbedProductos.procesoSelected4.Cantidad,
                    IdPro = pro.Id,
                    Nombre = pro.Nombre,
                });
            }
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
            MessagingCenter.Send<popupModificarProceso, string>(this, "popupModificarProceso", "x");
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ReportarComoVenta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupReportarVentaDesdeProceso());
            MessagingCenter.Subscribe<popupReportarVentaDesdeProceso, string>(this, "popupReportarVentaDesdeProceso", async (s, arg) =>
            {
                MessagingCenter.Send<popupModificarProceso, string>(this, "popupModificarProceso", "x");
                await PopupNavigation.Instance.PopAsync(true);
            });
        }

        private async void Actualizar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupUpdateProceso());
            MessagingCenter.Subscribe<popupUpdateProceso, string>(this, "popupUpdateProceso", async (s, arg) =>
            {
                MessagingCenter.Send<popupModificarProceso, string>(this, "popupModificarProceso", "x");
                await PopupNavigation.Instance.PopAsync(true);
            });
        }

        private async void Descripcion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupMostrarInfoProceso());
        }
    }
}