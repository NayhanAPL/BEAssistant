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
    public partial class PagarDeuda : PopupPage
    {
        public PagarDeuda()
        {
            InitializeComponent();
            byDefault();
        }

        private void byDefault()
        {
            totalDeuda.Text = TabbedInversiones.deudaActual.Costo.ToString();
            EntryCanidadPago.Text = "1";
        }
        private async void ButtonPagarDeuda_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(EntryCanidadPago.Text))
            {
                var deuda = await App.Database.GetIdDeuda(TabbedInversiones.deudaActual.Id);
                double newTotal = deuda.Costo * deuda.Unidades - Convert.ToInt32(EntryCanidadPago.Text);
                double newUnidades = newTotal / deuda.Costo;
                if (deuda.Costo * deuda.Unidades == Convert.ToInt32(EntryCanidadPago.Text))
                {
                    await App.Database.DeleteDeuda(deuda);
                }
                else
                {                    
                    Deudas Deuda = new Deudas()
                    {
                        Costo = deuda.Costo,
                        Unidades = newUnidades,
                        Descripcion = deuda.Descripcion,
                        FechaFin = deuda.FechaFin,
                        Denominacion = deuda.Denominacion,
                        FechaIcicio = deuda.FechaIcicio,
                        IdInv = deuda.IdInv,
                        Id = deuda.Id
                    };
                    await App.Database.SaveUpDeuda(Deuda);                                      
                    string por = Convert.ToInt32((Convert.ToInt32(EntryCanidadPago.Text) * 100) / (deuda.Costo * deuda.Unidades)).ToString();
                }
                DateTime TimeActual = DateTime.Now;
                TimeActual = TimeActual.ToLocalTime();
                if (deuda.Denominacion == "A")
                {
                    await App.Database.SaveRegAcumulativa(new ReAcumulativa
                    {
                        Fecha = TimeActual,
                        Costo = deuda.Costo,
                        Unidades = deuda.Unidades - newUnidades,
                        IdInv = deuda.IdInv
                    });
                }
                if (deuda.Denominacion == "C")
                {
                    await App.Database.SaveRegConstante(new ReConstante
                    {
                        Fecha = TimeActual,
                        Costo = deuda.Costo,
                        Unidades = deuda.Unidades - newUnidades,
                        IdInv = deuda.IdInv
                    }); 
                }
                if (deuda.Denominacion == "E")
                {
                    await App.Database.SaveRegExtraordinaria(new ReExtraordinaria
                    {
                        Fecha = TimeActual,
                        Costo = deuda.Costo,
                        Unidades = deuda.Unidades - newUnidades,
                        IdInv = deuda.IdInv
                    });
                }
                MessagingCenter.Send<PagarDeuda, string>(this, "PagarDeuda", "Pagar");
                await PopupNavigation.Instance.PopAsync(true);
            }
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}