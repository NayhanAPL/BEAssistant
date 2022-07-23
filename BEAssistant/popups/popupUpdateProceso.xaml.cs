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
    
    public partial class popupUpdateProceso : PopupPage
    {
        public popupUpdateProceso()
        {
            InitializeComponent();
            ByDefault();
        }

        private void ByDefault()
        {
            EntryCantUnidades.Text = (TabbedProductos.procesoSelected4.Cantidad).ToString();
            EntryTiempoDiaPro.Text = (TabbedProductos.procesoSelected4.TimeEsperado / (60 * 60 * 24)).ToString();
            EntryTiempoHorasPro.Text = (TabbedProductos.procesoSelected4.TimeEsperado % (60 * 60 * 24)).ToString();
            EntryTiempoMinPro.Text = (TabbedProductos.procesoSelected4.TimeEsperado % (60 * 60)).ToString();
            EntryTiempoSegPro.Text = (TabbedProductos.procesoSelected4.TimeEsperado % 60).ToString();
            EntryDescripcion.Text = (TabbedProductos.procesoSelected4.Descripcion).ToString();
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            Procesos up = new Procesos() { 
            Id = TabbedProductos.procesoSelected4.Id,
            Cantidad = Convert.ToInt32(EntryCantUnidades.Text),
            Descripcion = EntryDescripcion.Text,
            EnProceso = TabbedProductos.procesoSelected4.EnProceso,
            FechaIni = TabbedProductos.procesoSelected4.FechaIni,
            IdPro = TabbedProductos.procesoSelected4.IdPro,
            TimeResultado = TabbedProductos.procesoSelected4.TimeResultado,
            TimeEsperado = Convert.ToInt32(EntryTiempoSegPro.Text)
            + Convert.ToInt32(EntryTiempoMinPro.Text) * 60 
            + Convert.ToInt32(EntryTiempoHorasPro.Text) * 60 * 60
            + Convert.ToInt32(EntryTiempoDiaPro.Text) * 60 * 60 * 24
            }; await App.Database.SaveUpProcesos(up);
            MessagingCenter.Send<popupUpdateProceso, string>(this, "popupUpdateProceso", "x");
            await PopupNavigation.Instance.PopAsync(true);
        }
        
    }
}