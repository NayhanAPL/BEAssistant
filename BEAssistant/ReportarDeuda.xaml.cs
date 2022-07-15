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
    public partial class ReportarDeuda : PopupPage
    {
        public ReportarDeuda()
        {
            InitializeComponent();
            ByDefault();          
        }
        private void ByDefault()
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            EntryTiempoAnnoDeuda.Text = TimeActual.Year.ToString();
            EntryTiempoMesDeuda.Text = TimeActual.Month.ToString();
            EntryTiempoDiaDeuda.Text = TimeActual.Day.ToString();
            EntryTiempoAnnoDeuda.PlaceholderColor = Color.Green;
            EntryTiempoMesDeuda.PlaceholderColor = Color.Green;
            EntryTiempoDiaDeuda.PlaceholderColor = Color.Green;
        }
        private async void ButtonAgregarDeuda_Clicked(object sender, EventArgs e)
        {
            bool sepuede = true;
            int idObj = 0;
            string denominacion = "";
            if (TabbedInversiones.DenomSelected == 0) { denominacion = "C"; idObj = TabbedInversiones.objC.Id; }
            if (TabbedInversiones.DenomSelected == 1) { denominacion = "A"; idObj = TabbedInversiones.objA.Id; }
            if (TabbedInversiones.DenomSelected == 2) { denominacion = "E"; idObj = TabbedInversiones.objE.Id; }

            if (idObj == 0)
            {
                sepuede = false;
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Al parecer no se ha seleccionado correctamente un tipo de inverción en la página anterior.";
                await Navigation.PushPopupAsync(new PopupAlert());

            }
            if (string.IsNullOrEmpty(EntryTiempoAnnoDeuda.Text) || EntryTiempoAnnoDeuda.Text.Length != 4)
            {
                EntryTiempoAnnoDeuda.PlaceholderColor = Color.Green;
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe especificar el año de entrega con 4 dígitos.";
                await Navigation.PushPopupAsync(new PopupAlert());

                sepuede = false;
            }
            if (string.IsNullOrEmpty(EntryTiempoMesDeuda.Text))
            {
                EntryTiempoMesDeuda.PlaceholderColor = Color.Green;
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe especificar el mes límite.";
                await Navigation.PushPopupAsync(new PopupAlert());
                sepuede = false;
            }
            if (string.IsNullOrEmpty(EntryTiempoDiaDeuda.Text))
            {
                EntryTiempoDiaDeuda.PlaceholderColor = Color.Green;
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe especificar el día límite.";
                await Navigation.PushPopupAsync(new PopupAlert());
                sepuede = false;
            }

            if (sepuede)
            {
                TabbedInversiones.UpDateCostoMatPri();
                DateTime TimeActual = DateTime.Now;
                TimeActual = TimeActual.ToLocalTime();
                DateTime TimeFin = new DateTime(year: Convert.ToInt32(EntryTiempoAnnoDeuda.Text), month: Convert.ToInt32(EntryTiempoMesDeuda.Text), day: Convert.ToInt32(EntryTiempoDiaDeuda.Text)) ;
                await App.Database.SaveDeuda(new Deudas{
                Costo = TabbedInversiones.costoDeuda,
                Unidades = TabbedInversiones.unidadesDeuda,
                Denominacion = denominacion,
                Descripcion = EntryDeudaDescripcion.Text,
                FechaIcicio = TimeActual,
                FechaFin = TimeFin,
                IdInv = idObj
                });
                if (TabbedInversiones.DenomSelected == 0)
                {
                    var invC = await App.Database.GetIdInvConstante(idObj);
                    if (invC.Tipo == TiposConstante.MateriaPrima)
                    {
                        var stock = await App.Database.GetIdInvCStock(idObj);
                        SaveUpStock(stock[0]);
                    }
                }            
                if (TabbedInversiones.DenomSelected == 1)
                {
                    var invA = await App.Database.GetIdInvAcumulativa(idObj);
                    if (invA.Tipo == TiposAcumulativa.MateriaPrima)
                    {
                        var stock = await App.Database.GetIdInvAStock(idObj);
                        SaveUpStock(stock[0]);
                    }
                }
                ByDefault();
                await PopupNavigation.Instance.PopAsync(true);
            }
        }

        private async void SaveUpStock(Stocker stock)
        {
            Stocker upStock = new Stocker
            {
                Id = stock.Id,
                IdInv = stock.IdInv,
                TipoInv = stock.TipoInv,
                CantActual = stock.CantActual += TabbedInversiones.unidadesDeuda,
                Duracion = stock.Duracion,
                Categoria = stock.Categoria,
                UnidadesEstimadas = stock.UnidadesEstimadas
            };
            await App.Database.SaveUpStock(upStock);
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}