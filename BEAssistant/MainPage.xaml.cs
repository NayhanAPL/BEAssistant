using BEAssistant.xamlThings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BEAssistant
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Iniciar();
        }

        private async void Iniciar()
        {           
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            var FechaInicio = await App.Database.GetIdFechaInicio(1);
            if(FechaInicio == null)
            {
                await App.Database.SaveFechaInicio(new FechaInicio
                {
                    inicio = TimeActual
                });
            }
            else
            {
                if(FechaInicio.inicio.DayOfYear != TimeActual.DayOfYear || FechaInicio.inicio.Year != TimeActual.Year)
                {
                    DateTime timeRevisar = new DateTime();
                    bool actualizar = false;
                    DateTime limiteDeRevicion = new DateTime();
                    var lastDate = await App.Database.GetLastItemCierreDiario();
                    if (lastDate.Count == 0) 
                    { 
                        actualizar = true;
                        timeRevisar = FechaInicio.inicio;
                    }
                    if (lastDate.Count > 0)
                    {
                        if (lastDate[0].Fecha.DayOfYear < TimeActual.DayOfYear - 1 && lastDate[0].Fecha.Year == TimeActual.Year) actualizar = true;
                        if (lastDate[0].Fecha.DayOfYear < TimeActual.DayOfYear - 1 + 365 && lastDate[0].Fecha.Year < TimeActual.Year) actualizar = true;
                        timeRevisar = lastDate[0].Fecha;
                        timeRevisar.AddDays(1);
                    }
                    limiteDeRevicion = TimeActual;
                    if (actualizar)
                    {
                        

                        var cons = await App.Database.GetRegConstante();
                        var acum = await App.Database.GetRegAcumulativa();
                        var extra = await App.Database.GetRegExtraordinaria();
                        var venta = await App.Database.GetVenta();
                        while (timeRevisar.Year != limiteDeRevicion.Year || timeRevisar.Month != limiteDeRevicion.Month || timeRevisar.Day != limiteDeRevicion.Day)
                        {
                            double gasto = 0;
                            double ganacia = 0;

                            
                            if (cons.Count > 0) 
                            { var elems = cons.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems)  { gasto += (item.Costo * item.Unidades); } }
                            
                            if (acum.Count > 0)
                            { var elems = acum.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { gasto += (item.Costo * item.Unidades); } }
                            
                            if (extra.Count > 0)
                            { var elems = extra.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { gasto += (item.Costo * item.Unidades); } }

                            
                            if (venta.Count > 0) 
                            { var elems = venta.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { ganacia += (item.Precio * item.Unidades); } }

                            await App.Database.SaveCierreDiario(new CierresDiario()
                            {
                                Fecha = new DateTime(year: timeRevisar.Year, month: timeRevisar.Month, day: timeRevisar.Day),
                                Ganancia = ganacia,
                                Gasto = gasto
                            });

                            timeRevisar = timeRevisar.AddDays(1);
                        }

                    }
                }
            }                     
        }

        private async void ButtonEstadisticas_Clicked(object sender, EventArgs e)
        {           
            await Navigation.PushModalAsync(new TabbedEstadisticas());
        }

        private async void ButtonElementos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedProductos());
        }

        private async void ButtonOperaciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedConsultas());
        }

        private async void ButtonInverciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedInversiones());
        }

        private async void ButtonPropuestas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedPropuestas());
        }

        private async void ButtonRespuestasR_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedRespRapida());
        }
    }
}
