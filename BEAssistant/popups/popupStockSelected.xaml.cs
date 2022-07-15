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
    public partial class popupStockSelected : PopupPage
    {
        public popupStockSelected()
        {
            InitializeComponent();
            LlenarInfo();
        }
        public static string tipo = "";
        public static double unidActualizado = 0;
        private async void LlenarInfo()
        {
            double costo = 0;
            var stockA = await App.Database.GetByTipoInvStock("Acumulativa");
            var stockC = await App.Database.GetByTipoInvStock("Constante");
            for (int i = 0; i < stockA.Count; i++)
            { if (stockA[i].Id == popupVerStock.stockSelected.Id)
                { tipo = "Acumulativa"; break; } }
            for (int i = 0; i < stockC.Count; i++)
            { if (stockC[i].Id == popupVerStock.stockSelected.Id) 
                { tipo = "Constante"; break; } }

            if (tipo == "Acumulativa")
            {
                var reg = await App.Database.GetIdInvRegAcumulativa(popupVerStock.stockSelected.IdInv);
                if (reg.Count > 0) costo = reg[reg.Count - 1].Costo;
                else costo = 1;
            }
            if (tipo == "Constante")
            {
                var reg = await App.Database.GetIdInvRegConstante(popupVerStock.stockSelected.IdInv);
                if (reg.Count > 0) costo = reg[reg.Count - 1].Costo;
                else costo = 1;
            }
            double unidadesFaltante = popupVerStock.stockSelected.UnidadesEstimadas - popupVerStock.stockSelected.CantActual;
            double dineroFaltante = unidadesFaltante * costo;
            labelTitle.Text = popupVerStock.stockSelected.TipoInv;
            labelfondoTotal.Text = (popupVerStock.stockSelected.UnidadesEstimadas * costo).ToString();
            labelfondoInvertido.Text = (popupVerStock.stockSelected.CantActual * costo).ToString();
            labelfondoFaltante.Text = dineroFaltante.ToString();
            labelUnidEsperadas.Text = popupVerStock.stockSelected.UnidadesEstimadas.ToString();
            labelUnidActuales.Text = popupVerStock.stockSelected.CantActual.ToString();
            labelUnidFaltantes.Text = unidadesFaltante.ToString();
            var materiasPrimas = await App.Database.GetByNameMateriaPrima(popupVerStock.stockSelected.TipoInv);
            List<Product> productos = new List<Product>();
            foreach (var item in materiasPrimas)
            {
                var x = await App.Database.GetIdProductos(item.IdProducto);
                productos.Add(x);
            }
            foreach (var item in productos)
            {
                labelProductosEmpleados.Text += item.Nombre + ", ";
            }
            double fondoActualizado = 0;
            double DemandaActualizado = 0;
            unidActualizado = 0;
            for (int i = 0; i < materiasPrimas.Count; i++)
            {
                if (materiasPrimas[i].PrecioUnidad > 0)
                {
                    DemandaActualizado += productos[i].Demanda;
                    unidActualizado += materiasPrimas[i].CantidadPorProducto * productos[i].Demanda;
                    fondoActualizado += materiasPrimas[i].PrecioUnidad * materiasPrimas[i].CantidadPorProducto * productos[i].Demanda;
                }
            }
            labelOfertaDemanda.Text = $"La demanda actual de los productos que emplean esta materia prima es de {DemandaActualizado} al mes," +
                $" esto implica un total de {unidActualizado} unidades de {popupVerStock.stockSelected.Categoria}, ${fondoActualizado} es el fondo utilizado," +
                $" le recomendamos agregar un 10% más para un total de ${Convert.ToInt32(fondoActualizado + fondoActualizado / 10)}.";
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ActualizarFondo_Clicked(object sender, EventArgs e)
        {
            Stocker up = new Stocker() {
                Id = popupVerStock.stockSelected.Id,
                IdInv = popupVerStock.stockSelected.IdInv,
                CantActual = popupVerStock.stockSelected.CantActual,
                Categoria = popupVerStock.stockSelected.Categoria,
                Duracion = popupVerStock.stockSelected.Duracion,
                TipoInv = tipo,
                UnidadesEstimadas = Convert.ToInt32(unidActualizado)
                };await App.Database.SaveUpStock(up);
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}