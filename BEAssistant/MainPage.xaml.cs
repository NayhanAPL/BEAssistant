using BEAssistant.popups;
using BEAssistant.xamlThings;
using Rg.Plugins.Popup.Extensions;
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
        {   //17.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjE4ODcwQDMxMzcyZTMyMmUzMGNvc0k5ZIJXSFhhNVoxSGVYaUJKUTR4akFTRkZRaDRRZ1NiU25kT3cxVGs9");
            //20.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjE4ODQwQDMyMzAyZTMxMmUzMGR2VzNJYmJacGtpZ0ZZckdOV1JWYUJlVVB1NlRPY201aGhiWUI0YnlLanM9");

            InitializeComponent();
            
            Iniciar();
        }

        private async void Iniciar()
        {
            CrearInventario();
            listCadusidad.Clear();
            RevisarCaducidad();
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
                // Donde se agregan las opciones 
                await App.Database.SaveOpciones(new Opciones
                {
                    Activo = false,
                    Nombre = ""
                });
                //primer inventario
                await App.Database.SaveInventario(new Inventario()
                {
                    ConsCaduco = 0,
                    ConsExedente = 0,
                    ConsUtil = 0,
                    Materias = "",
                    Fecha = TimeActual,
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
                        timeRevisar = timeRevisar.AddDays(1);
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
                                Ingreso = ganacia,
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
        private async void CrearInventario()
        {
            int dia = 0;
            DateTime fecha = new DateTime();
            var primerDia = await App.Database.GetIdFechaInicio(1);
            if (primerDia != null)
            {
                var elem = await App.Database.GetLastItemInventario();
                if (elem.Count > 0) 
                { 
                    dia = elem[0].Fecha.Year * 365 + elem[0].Fecha.DayOfYear; 
                    fecha = elem[0].Fecha; 
                }
                else
                {
                    dia = primerDia.inicio.Year * 365 + primerDia.inicio.DayOfYear;
                    fecha = primerDia.inicio;
                }

                DateTime TimeActual = DateTime.Now;
                TimeActual = TimeActual.ToLocalTime();
                int DiaHoy = TimeActual.Year * 365 + TimeActual.DayOfYear;
                dia++;
                while (dia <= DiaHoy)
                {
                    await App.Database.SaveInventario(new Inventario()
                    {
                        ConsCaduco = 0,
                        ConsExedente = 0,
                        ConsUtil = 0,
                        Materias = "",
                        Fecha = fecha,
                    });
                    dia++;
                    fecha = fecha.AddDays(1);
                }
            }
              
        }
        public static List<ViewCaducidad> listCadusidad = new List<ViewCaducidad>(); 
        private async void RevisarCaducidad()
        {
            bool Mostrarmensaje = false;
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            int diaActual = TimeActual.DayOfYear + TimeActual.Year * 365;
            var listInven = await App.Database.GetInventario();
            var stock = await App.Database.GetStock();
            var regA = await App.Database.GetByTipoReAcumulativa((int)TiposAcumulativa.MateriaPrima);
            var regC = await App.Database.GetByTipoReConstante((int)TiposConstante.MateriaPrima);            
            foreach (var item in stock)
            {
                if(item.TipoInv == "Acumulativa")
                {
                    double unidadesUtiles = 0;
                    var spesifiA = regA.FindAll(x => x.IdInv == item.IdInv);
                    for (int i = spesifiA.Count - 1; i >= 0; i--)
                    {
                        var caducidad = await App.Database.GetByIdRegAcumCaducidad(spesifiA[i].Id); if (caducidad.Count > 0)
                        {
                            int dias = (spesifiA[i].Fecha.DayOfYear + spesifiA[i].Fecha.Year * 365) + item.Duracion;
                            if (dias <= diaActual && caducidad[0].Caduco != true)
                            {
                                Caducidad up = caducidad[0]; up.Caduco = true; await App.Database.SaveUpCaducidad(up);
                                Stocker upStock = new Stocker
                                {
                                    Id = item.Id,
                                    IdInv = item.IdInv,
                                    TipoInv = item.TipoInv,
                                    CantActual = unidadesUtiles,
                                    Duracion = item.Duracion,
                                    Categoria = item.Categoria,
                                    UnidadesEstimadas = item.UnidadesEstimadas
                                };
                                await App.Database.SaveUpStock(upStock);
                                // Inventario
                                var invenDia = listInven.FindAll(x => x.Fecha.DayOfYear == TimeActual.DayOfYear && x.Fecha.Year == TimeActual.Year);
                                var inversion = await App.Database.GetIdInvAcumulativa(item.IdInv);
                                int posInven = invenDia.FindIndex(x => x.Materias == inversion.Nombre);
                                if (posInven != -1)
                                {
                                    invenDia[posInven].ConsCaduco = Convert.ToInt32(item.CantActual - unidadesUtiles);
                                    await App.Database.SaveUpInventario(invenDia[posInven]);
                                }
                                else
                                {
                                    var fecha = spesifiA[i].Fecha;
                                    fecha.AddDays(item.Duracion);
                                    await App.Database.SaveInventario(new Inventario()
                                    {
                                        Materias = inversion.Nombre,
                                        ConsUtil = 0,
                                        ConsCaduco = Convert.ToInt32(item.CantActual - unidadesUtiles),
                                        ConsExedente = 0,
                                        Fecha = fecha
                                    });
                                }
                                Mostrarmensaje = true;
                                var inv = await App.Database.GetIdInvAcumulativa(item.IdInv);
                                listCadusidad.Add( new ViewCaducidad() { Nombre = inv.Nombre , Perdida = Convert.ToInt32(spesifiA[i].Unidades - unidadesUtiles), CantidadActual = Convert.ToInt32(unidadesUtiles)});
                            }
                            else
                            {
                                unidadesUtiles += spesifiA[i].Unidades;
                                if (unidadesUtiles >= item.CantActual) break;
                            }
                        }

                    }
                }
                if (item.TipoInv == "Constante")
                {
                    double unidadesUtiles = 0;
                    var spesifiC = regC.FindAll(x => x.IdInv == item.IdInv);
                    for (int i = spesifiC.Count - 1; i >= 0; i--)
                    {
                        var caducidad = await App.Database.GetByIdRegConsCaducidad(spesifiC[i].Id); if (caducidad.Count > 0)
                        {
                            int dias = (spesifiC[i].Fecha.DayOfYear + spesifiC[i].Fecha.Year * 365) + item.Duracion;
                            if (dias <= diaActual && caducidad[0].Caduco != true)
                            {
                                Caducidad up = caducidad[0]; up.Caduco = true; await App.Database.SaveUpCaducidad(up);
                                Stocker upStock = new Stocker
                                {
                                    Id = item.Id,
                                    IdInv = item.IdInv,
                                    TipoInv = item.TipoInv,
                                    CantActual = unidadesUtiles,
                                    Duracion = item.Duracion,
                                    Categoria = item.Categoria,
                                    UnidadesEstimadas = item.UnidadesEstimadas
                                };
                                await App.Database.SaveUpStock(upStock);
                                // Inventario
                                var invenDia = listInven.FindAll(x => x.Fecha.DayOfYear == TimeActual.DayOfYear && x.Fecha.Year == TimeActual.Year);
                                var inversion = await App.Database.GetIdInvConstante(item.IdInv);
                                int posInven = invenDia.FindIndex(x => x.Materias == inversion.Nombre);
                                if (posInven != -1)
                                {
                                    invenDia[posInven].ConsCaduco = Convert.ToInt32(item.CantActual - unidadesUtiles);
                                    await App.Database.SaveUpInventario(invenDia[posInven]);
                                }
                                else
                                {
                                    var fecha = spesifiC[i].Fecha;
                                    fecha.AddDays(item.Duracion);
                                    await App.Database.SaveInventario(new Inventario()
                                    {
                                        Materias = inversion.Nombre,
                                        ConsUtil = 0,
                                        ConsCaduco = Convert.ToInt32(item.CantActual - unidadesUtiles),
                                        ConsExedente = 0,
                                        Fecha = fecha
                                    });
                                    invenDia[posInven].Materias = inversion.Nombre;
                                    invenDia[posInven].ConsUtil = 0;
                                    invenDia[posInven].ConsExedente = 0;
                                    invenDia[posInven].ConsCaduco = Convert.ToInt32(item.CantActual - unidadesUtiles);
                                }
                                Mostrarmensaje = true;
                                var inv = await App.Database.GetIdInvConstante(item.IdInv);
                                listCadusidad.Add(new ViewCaducidad() { Nombre = inv.Nombre, Perdida = Convert.ToInt32(spesifiC[i].Unidades - unidadesUtiles), CantidadActual = Convert.ToInt32(unidadesUtiles) });
                            }
                            else
                            {
                                unidadesUtiles += spesifiC[i].Unidades;
                                if (unidadesUtiles >= item.CantActual) break;
                            }
                        }
                    }
                }
            }
            if (Mostrarmensaje)
            {
                await Navigation.PushPopupAsync(new popupReporteCaducidad());
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
                    if (listaReg.Count > 0)
                    {
                        ultimoReg = listaReg[listaReg.Count - 1].Fecha;
                        ultimoReg = IncFrecuencia(elemC.Frecuencia, ultimoReg);

                        while ((ultimoReg.DayOfYear < TimeActual.DayOfYear && ultimoReg.Year == TimeActual.Year)
                            || (ultimoReg.DayOfYear < TimeActual.DayOfYear + 365 && ultimoReg.Year < TimeActual.Year))
                        {
                            if (elemC.Categoria == ConstCategoria.Proporcional)
                            {
                                int registro = 0;
                                var dependencia = await App.Database.GetIdInvDependenciaConstante(elemC.Id);
                                if (dependencia[0].Clase == "inAcumulativa")
                                {
                                    var registros = await App.Database.GetIdInvRegAcumulativa(dependencia[0].IdItem);
                                    var periodo = registros.FindAll(x => TimeActual - x.Fecha <= TimeActual - ultimoReg);
                                    double suma = 0;
                                    periodo.ForEach(x => suma += x.Costo * x.Unidades);
                                    registro = Convert.ToInt32((dependencia[0].Porcentaje * suma));
                                }
                                else if (dependencia[0].Clase == "inConstante")
                                {
                                    var registros = await App.Database.GetIdInvRegConstante(dependencia[0].IdItem);
                                    var periodo = registros.FindAll(x => TimeActual - x.Fecha <= TimeActual - ultimoReg);
                                    double suma = 0;
                                    periodo.ForEach(x => suma += x.Costo * x.Unidades);
                                    registro = Convert.ToInt32((dependencia[0].Porcentaje * suma));
                                }
                                else if (dependencia[0].Clase == "inExtraordinaria")
                                {
                                    var registros = await App.Database.GetIdInvRegExtraordinaria(dependencia[0].IdItem);
                                    var periodo = registros.FindAll(x => TimeActual - x.Fecha <= TimeActual - ultimoReg);
                                    double suma = 0;
                                    periodo.ForEach(x => suma += x.Costo * x.Unidades);
                                    registro = Convert.ToInt32((dependencia[0].Porcentaje * suma));
                                }
                                else if (dependencia[0].Clase == "Product")
                                {
                                    var registros = await App.Database.GetByIdProVenta(dependencia[0].IdItem);
                                    var periodo = registros.FindAll(x => TimeActual - x.Fecha <= TimeActual - ultimoReg);
                                    double suma = 0;
                                    periodo.ForEach(x => suma += x.Precio * x.Unidades);
                                    registro = Convert.ToInt32((dependencia[0].Porcentaje * suma));
                                }
                                else
                                {
                                    var registros = await App.Database.GetCierreDiario();
                                    var periodo = registros.FindAll(x => TimeActual - x.Fecha <= TimeActual - ultimoReg);
                                    double sumaIngreso = 0;
                                    double sumaGasto = 0;
                                    periodo.ForEach(x => sumaIngreso += x.Ingreso);
                                    periodo.ForEach(x => sumaGasto += (x.GastoA + x.GastoC + x.GastoE));
                                    
                                    if (dependencia[0].Clase == "Ganancia")
                                    {
                                        registro = Convert.ToInt32((dependencia[0].Porcentaje * (sumaIngreso - sumaGasto)));
                                    }
                                    if (dependencia[0].Clase == "Gasto")
                                    {
                                        registro = Convert.ToInt32((dependencia[0].Porcentaje * sumaGasto));
                                    }
                                    if (dependencia[0].Clase == "Ingreso")
                                    {
                                        registro = Convert.ToInt32((dependencia[0].Porcentaje * sumaIngreso));                     
                                    }
                                }
                                if (registro != 0) registro /= 100;
                                await App.Database.SaveRegConstante(new ReConstante()
                                {
                                    IdInv = elemC.Id,
                                    Costo = registro,
                                    Unidades = 1,
                                    Fecha = new DateTime(year: ultimoReg.Year, month: ultimoReg.Month, day: ultimoReg.Day)
                                });
                                var lastItem = await App.Database.GetLastItemRegConstante();
                                await App.Database.SaveCaducidad(new Caducidad()
                                {
                                    IdReg = lastItem[0].IdInv,
                                    Caduco = false,
                                    TipoInv = "C"
                                });
                            }                                    
                            if(elemC.Categoria == ConstCategoria.Independiente)
                            {
                                await App.Database.SaveRegConstante(new ReConstante()
                                {
                                    IdInv = elemC.Id,
                                    Costo = listaReg[listaReg.Count - 1].Costo,
                                    Unidades = listaReg[listaReg.Count - 1].Unidades,
                                    Fecha = new DateTime(year: ultimoReg.Year, month: ultimoReg.Month, day: ultimoReg.Day)
                                });
                                var lastItem = await App.Database.GetLastItemRegConstante();
                                await App.Database.SaveCaducidad(new Caducidad()
                                {
                                    IdReg = lastItem[0].IdInv,
                                    Caduco = false,
                                    TipoInv = "C"
                                });
                            }
                            if(elemC.Tipo == TiposConstante.MateriaPrima)
                            {
                                var stock = await App.Database.GetIdInvCStock(elemC.Id);
                                Stocker upStock = new Stocker
                                {
                                    Id = stock[0].Id,
                                    IdInv = stock[0].IdInv,
                                    TipoInv = stock[0].TipoInv,
                                    CantActual = stock[0].CantActual += Convert.ToInt32(listaReg[listaReg.Count - 1].Unidades),
                                    Duracion = stock[0].Duracion,
                                    Categoria = stock[0].Categoria,
                                    UnidadesEstimadas = stock[0].UnidadesEstimadas
                                };
                                await App.Database.SaveUpStock(upStock);
                            }
                            ultimoReg = IncFrecuencia(elemC.Frecuencia, ultimoReg);
                        }
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
            await Navigation.PushModalAsync(new TabbedOpciones());
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
            await Navigation.PushPopupAsync(new TabbedOperaciones());
        }
    }
}
