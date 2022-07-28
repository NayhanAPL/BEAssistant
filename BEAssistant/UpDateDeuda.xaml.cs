using BEAssistant.popups;
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

namespace BEAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpDateDeuda : PopupPage
    {
        public UpDateDeuda()
        {
            InitializeComponent();
            ByDefault();
        }

        private async void ByDefault()
        {
            var elem = await App.Database.GetIdDeuda(TabbedInversiones.deudaActual.Id);
            fechaInicialDeuda.Text = TabbedInversiones.deudaActual.FechaIcicio.ToString();
            EntryCostoDeuda.Text = (TabbedInversiones.deudaActual.Costo / TabbedInversiones.deudaActual.Unidades).ToString();
            EntryUnidadesDeuda.Text = TabbedInversiones.deudaActual.Unidades.ToString();
            EntryDeudaDescripcion.Text = elem.Descripcion;
            EntryFechaFinDeuda.Text = TabbedInversiones.deudaActual.FechaFin.ToString();
            nameDeuda.Text = TabbedInversiones.deudaActual.Denominacion;

        }

        private async void UpDateDeudas_Clicked(object sender, EventArgs e)
        {
            DateTime fecha = new DateTime(); 
            try
            {
                fecha = Convert.ToDateTime(EntryFechaFinDeuda.Text);
            }
            catch (Exception)
            {
                fecha = TabbedInversiones.deudaActual.FechaFin;
            }
            var elem = await App.Database.GetIdDeuda(TabbedInversiones.deudaActual.Id);
            if (!string.IsNullOrEmpty(EntryCostoDeuda.Text) && !string.IsNullOrEmpty(EntryFechaFinDeuda.Text) && !string.IsNullOrEmpty(EntryUnidadesDeuda.Text))
            {

                Deudas deuda = new Deudas()
                {
                    Costo = Convert.ToDouble(EntryCostoDeuda.Text),
                    Unidades = Convert.ToDouble(EntryUnidadesDeuda.Text),
                    Descripcion = EntryDeudaDescripcion.Text,
                    FechaFin = fecha,
                    Denominacion = elem.Denominacion,
                    FechaIcicio = TabbedInversiones.deudaActual.FechaIcicio,
                    IdInv = TabbedInversiones.deudaActual.IdInv,
                    Id = TabbedInversiones.deudaActual.Id
                };
                await App.Database.SaveUpDeuda(deuda);
                MessagingCenter.Send<UpDateDeuda, string>(this, "UpDateDeuda", "UpDeuda");
                await PopupNavigation.Instance.PopAsync(true);
            }
            else {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "No puede dejar ningún dato sin llenar.";
                await Navigation.PushPopupAsync(new PopupAlert());
            } 
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}