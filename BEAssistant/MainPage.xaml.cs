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
            RegistroConstante();
            var d = await App.Database.GetCierreDiario();
            var m = await App.Database.GetCierreMensual();

            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            var FechaInicio = await App.Database.GetIdFechaInicio(1);
            if (FechaInicio == null)
            {
                await App.Database.SaveFechaInicio(new FechaInicio
                {
                    inicio = TimeActual
                });
            }
            else
            {
                if (FechaInicio.inicio.DayOfYear != TimeActual.DayOfYear || FechaInicio.inicio.Year != TimeActual.Year)
                {
                    DateTime timeRevisar = new DateTime();
                    bool actualizar = false;
                    DateTime limiteDeRevicion = new DateTime();
                    var lastDate = await App.Database.GetLastItemCierreDiario();
                    var lastRegMes = await App.Database.GetLastItemCierreMensual();
                    if (lastRegMes.Count == 0)
                    {
                        await App.Database.SaveCierreMensual(new CierresMensual()
                        {
                            GastoE = 0,
                            GastoA = 0,
                            GastoC = 0,
                            Ganancia = 0,
                            Fecha = FechaInicio.inicio
                        });
                        lastRegMes = await App.Database.GetLastItemCierreMensual();
                    }
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
                            double gastoC = 0;
                            double gastoA = 0;
                            double gastoE = 0;
                            double ganacia = 0;


                            if (cons.Count > 0)
                            { var elems = cons.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { gastoC += (item.Costo * item.Unidades); } }

                            if (acum.Count > 0)
                            { var elems = acum.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { gastoA += (item.Costo * item.Unidades); } }

                            if (extra.Count > 0)
                            { var elems = extra.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { gastoE += (item.Costo * item.Unidades); } }


                            if (venta.Count > 0)
                            { var elems = venta.FindAll(x => x.Fecha.DayOfYear == timeRevisar.DayOfYear && x.Fecha.Year == timeRevisar.Year);
                                foreach (var item in elems) { ganacia += (item.Precio * item.Unidades); } }

                            await App.Database.SaveCierreDiario(new CierresDiarios()
                            {
                                Fecha = new DateTime(year: timeRevisar.Year, month: timeRevisar.Month, day: timeRevisar.Day),
                                Ganancia = ganacia,
                                GastoC = gastoC,
                                GastoA = gastoA,
                                GastoE = gastoE
                            });

                            lastRegMes = await App.Database.GetLastItemCierreMensual();
                            if (lastRegMes[0].Fecha.Month != timeRevisar.Month) 
                            {
                                await App.Database.SaveCierreMensual( new CierresMensual()
                                {
                                    GastoE = gastoE,
                                    GastoA = gastoA,
                                    GastoC = gastoC,
                                    Ganancia = ganacia,
                                    Fecha = new DateTime(day:1, year: timeRevisar.Year, month: timeRevisar.Month)
                                }); 
                            }
                            else 
                            {
                                CierresMensual mensual = new CierresMensual()
                                {
                                    Id = lastRegMes[0].Id,
                                    GastoE = lastRegMes[0].GastoE + gastoE,
                                    GastoA = lastRegMes[0].GastoA + gastoA,
                                    GastoC = lastRegMes[0].GastoC + gastoC,
                                    Ganancia = lastRegMes[0].Ganancia + ganacia,
                                    Fecha = lastRegMes[0].Fecha
                                }; await App.Database.SaveUpCierreMensual(mensual);
                            }

                            timeRevisar = timeRevisar.AddDays(1);
                        }

                    }
                }

                
            }            
        }

        private async void RegistroConstante()
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            var listaConstante = await App.Database.GetInvConstante();
            if(listaConstante.Count > 0)
            {
                foreach (var elemC in listaConstante)
                {
                    DateTime ultimoReg = new DateTime();
                    var listaReg = await App.Database.GetIdInvRegConstante(elemC.Id);
                    if (listaReg.Count == 0) ultimoReg = elemC.Fecha;
                    if (listaReg.Count > 0) ultimoReg = listaReg[listaReg.Count - 1].Fecha;

                    ultimoReg = IncFrecuencia(elemC.Frecuencia, ultimoReg);
                    
                    while (ultimoReg.DayOfYear < TimeActual.DayOfYear - 1 && ultimoReg.Year == TimeActual.Year 
                        || ultimoReg.DayOfYear < TimeActual.DayOfYear - 1 + 365 && ultimoReg.Year < TimeActual.Year)
                    {
                        await App.Database.SaveRegConstante(new ReConstante() { 
                        IdInv = elemC.Id,
                        Costo = listaReg[listaReg.Count - 1].Costo,
                        Unidades = listaReg[listaReg.Count - 1].Unidades,
                        Fecha = new DateTime(year: ultimoReg.Year, month: ultimoReg.Month, day: ultimoReg.Day)
                        });
                        IncFrecuencia(elemC.Frecuencia, ultimoReg);
                    }
                }
            }
              
        }

        private DateTime IncFrecuencia(ConstFrecuencia frecuencia, DateTime ultimoReg)
        {
            switch (frecuencia)
            {
                case ConstFrecuencia.Diaria:
                    ultimoReg = ultimoReg.AddDays(1);
                    break;
                case ConstFrecuencia.Semanal:
                    ultimoReg = ultimoReg.AddDays(7);
                    break;
                case ConstFrecuencia.Quincenal:
                    ultimoReg = ultimoReg.AddDays(15);
                    break;
                case ConstFrecuencia.Mensual:
                    ultimoReg = ultimoReg.AddMonths(1);
                    break;
                case ConstFrecuencia.Semestral:
                    ultimoReg = ultimoReg.AddMonths(6);
                    break;
                case ConstFrecuencia.Anual:
                    ultimoReg = ultimoReg.AddYears(1);
                    break;
                default:
                    break;
            }
            return ultimoReg;
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
