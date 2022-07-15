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

        private void EntryCostoDeuda_Focused(object sender, FocusEventArgs e)
        {
            EntryCostoDeuda.FontSize = 20;
        }
        private void EntryCostoDeuda_Unfocused(object sender, FocusEventArgs e)
        {
            EntryCostoDeuda.FontSize = 17;
        }
        private void EntryFechaFinDeuda_Focused(object sender, FocusEventArgs e)
        {
            EntryFechaFinDeuda.FontSize = 20;
        }
        private void EntryFechaFinDeuda_Unfocused(object sender, FocusEventArgs e)
        {
            EntryFechaFinDeuda.FontSize = 17;
        }
        private void EntryUnidadesDeuda_Focused(object sender, FocusEventArgs e)
        {
            EntryUnidadesDeuda.FontSize = 20;
        }
        private void EntryUnidadesDeuda_Unfocused(object sender, FocusEventArgs e)
        {
            EntryUnidadesDeuda.FontSize = 17;
        }

        private async void UpDateDeudas_Clicked(object sender, EventArgs e)
        {
            var elem = await App.Database.GetIdDeuda(TabbedInversiones.deudaActual.Id);
            if (!string.IsNullOrEmpty(EntryCostoDeuda.Text) && !string.IsNullOrEmpty(EntryFechaFinDeuda.Text) && !string.IsNullOrEmpty(EntryUnidadesDeuda.Text))
            {

                Deudas deuda = new Deudas()
                {
                    Costo = Convert.ToDouble(EntryCostoDeuda.Text),
                    Unidades = Convert.ToDouble(EntryUnidadesDeuda.Text),
                    Descripcion = EntryDeudaDescripcion.Text,
                    FechaFin = Convert.ToDateTime(EntryFechaFinDeuda.Text),
                    Denominacion = elem.Denominacion,
                    FechaIcicio = TabbedInversiones.deudaActual.FechaIcicio,
                    IdInv = TabbedInversiones.deudaActual.IdInv,
                    Id = TabbedInversiones.deudaActual.Id
                };
                await App.Database.SaveUpDeuda(deuda);
                PopupAlert.PopupLabelTitulo = "ELEMENTO ACTUALIZADO";
                PopupAlert.PopupLabelText = "Los datos de su deuda fueron actualizados, revise los cambios en la página anterior.";
                await Navigation.PushPopupAsync(new PopupAlert());
                await PopupNavigation.Instance.PopAsync(true);
            }
            else {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "No puede dejar ningún dato sin llenar.";
                await PopupNavigation.Instance.PopAsync(true);
            }
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}