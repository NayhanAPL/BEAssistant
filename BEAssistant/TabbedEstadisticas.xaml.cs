using BEAssistant.popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BEAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedEstadisticas : TabbedPage
    {
        public TabbedEstadisticas()
        {
            InitializeComponent();
        }
        private async void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            List<CierresDiarios> elementos = await App.Database.GetCierreDiario();
            if(elementos.Count > 0)
            {
                double estimado = 0;
                ObservableCollection<Historial> historialGanancia = new ObservableCollection<Historial>(); 
                if (elementos.Count > 31)
                {
                    List<CierresMensual> elementosM = await App.Database.GetCierreMensual();
                    
                    for (int i = elementosM.Count - 1; i >= 0; i--)
                    {
                        estimado = 0;
                        int estimadoByDebt = 0;
                        for (int j = 0; j < i; j++)
                        {
                            estimadoByDebt = Convert.ToInt32(elementosM[j].GastoA + elementosM[j].GastoC + elementosM[j].GastoE);
                            double actual = elementosM[j].Ganancia - (elementosM[j].GastoA + elementosM[j].GastoC + elementosM[j].GastoE);
                            if (actual > estimado) estimado = actual;
                        }
                        estimado += Convert.ToInt32(estimado / 10);
                        if (estimado == 0) estimado = estimadoByDebt + Convert.ToInt32(estimadoByDebt / 10) > 0 ? estimado = estimadoByDebt + Convert.ToInt32(estimadoByDebt / 10) : 1;
                        int por = Convert.ToInt32(elementosM[i].Ganancia - (elementosM[i].GastoA + elementosM[i].GastoC + elementosM[i].GastoE));
                        por *= 100;
                        estimado = estimado > 0 ? estimado : 1;
                        por /= Convert.ToInt32(estimado);
                        if (por < 0) por = 0;
                        if (por > 100) por = 100;

                        string fecha = elementosM[i].Fecha.Month.ToString() + "/" + elementosM[i].Fecha.Year.ToString();

                        historialGanancia.Add(new Historial()
                        {
                            Ganancia = elementosM[i].Ganancia - (elementosM[i].GastoA + elementosM[i].GastoC + elementosM[i].GastoE),
                            Estimado = estimado,
                            GastoA = elementosM[i].GastoA,
                            GastoC = elementosM[i].GastoC,
                            GastoE = elementosM[i].GastoE,
                            Fecha = fecha,
                            Ingreso = elementosM[i].Ganancia,
                            Porcentaje = por
                        });
                    }                    
                }
                else
                {
                    for (int i = elementos.Count - 1; i >= 0; i--)
                    {
                        estimado = 0;
                        int estimadoByDebt = 0;
                        for (int j = 0; j < i; j++)
                        {
                            estimadoByDebt += Convert.ToInt32(elementos[j].GastoA + elementos[j].GastoC + elementos[j].GastoE);
                            double actual = elementos[j].Ingreso - (elementos[j].GastoA + elementos[j].GastoC + elementos[j].GastoE);
                            if (actual > estimado) estimado = actual;
                        }
                        estimado += estimado / 10;
                        if (estimado == 0) estimado = estimadoByDebt + Convert.ToInt32(estimadoByDebt / 10);
                        int por = Convert.ToInt32(elementos[i].Ingreso - (elementos[i].GastoA + elementos[i].GastoC + elementos[i].GastoE));
                        por *= 100;
                        estimado = estimado > 0 ? estimado : 1;
                        por /= Convert.ToInt32(estimado);
                        if (por < 0) por = 0;
                        if (por > 100) por = 100;
                        string fecha = elementos[i].Fecha.Day.ToString() + "/" + elementos[i].Fecha.Month.ToString() + "/" + elementos[i].Fecha.Year.ToString();
                        historialGanancia.Add(new Historial()
                        {
                            Ganancia = elementos[i].Ingreso - (elementos[i].GastoA + elementos[i].GastoC + elementos[i].GastoE),
                            Estimado = estimado,
                            GastoA = elementos[i].GastoA,
                            GastoC = elementos[i].GastoC,
                            GastoE = elementos[i].GastoE,
                            Fecha = fecha,
                            Ingreso = elementos[i].Ingreso,
                            Porcentaje = por
                        });
                    }                    
                }
                List<Scroll> scrollPage1 = LlenarScroll(historialGanancia.Count);
                for (int i = 0; i < historialGanancia.Count; i++)
                {
                    historialGanancia[i].labelArrowLeft = scrollPage1[i].labelArrowLeft;
                    historialGanancia[i].labelArrowRigth = scrollPage1[i].labelArrowRigth;
                    historialGanancia[i].labelPoint1Gray = scrollPage1[i].labelPoint1Gray;
                    historialGanancia[i].labelPoint1Green = scrollPage1[i].labelPoint1Green;
                    historialGanancia[i].labelPoint2Gray = scrollPage1[i].labelPoint2Gray;
                    historialGanancia[i].labelPoint2Green = scrollPage1[i].labelPoint2Green;
                    historialGanancia[i].labelPoint3Gray = scrollPage1[i].labelPoint3Gray;
                    historialGanancia[i].labelPoint3Green = scrollPage1[i].labelPoint3Green;
                }
                ViewTiempos.ItemsSource = historialGanancia;
            }
            double ganancia = 0;
            double ingresos = 0;
            double relevanciaTotal = 0;
            double relevancia = 0;
            List<TablaDenom> tablaDenom = new List<TablaDenom>();
            var ven = await App.Database.GetVenta();
            var pro = await App.Database.GetProductos();
            ven.ForEach(x => relevanciaTotal += (x.Precio * x.Unidades) - pro[pro.FindIndex(y => y.Id == x.IdPro)].Inversion);
            foreach (var item in pro)
            {
                var index = tablaDenom.FindIndex(x => x.Denominacion == item.Denominacion);
                if (index == -1) 
                {
                    var v = await App.Database.GetByDenomVenta(item.Denominacion);
                    foreach (var elem in v)
                    {
                        ingresos += elem.Precio * elem.Unidades;
                        ganancia += (elem.Precio * elem.Unidades) - pro[pro.FindIndex(x => x.Id == elem.IdPro)].Inversion;
                    }
                    if (relevanciaTotal != 0)
                    {
                        relevancia = (ganancia * 100) / relevanciaTotal;
                    }

                    tablaDenom.Add(new TablaDenom()
                    {
                        Denominacion = item.Denominacion,
                        Ingreso = ingresos,
                        Venta = v.Count,
                        Relevancia = Convert.ToInt32(relevancia),
                        Ganancia = ganancia,
                    });
                    ganancia = 0;
                    ingresos = 0;
                }
                
            }
            List<Scroll> scroll = LlenarScroll(tablaDenom.Count);
            for (int i = 0; i < tablaDenom.Count; i++)
            {
                tablaDenom[i].labelArrowLeft = scroll[i].labelArrowLeft;
                tablaDenom[i].labelArrowRigth = scroll[i].labelArrowRigth;
                tablaDenom[i].labelPoint1Gray = scroll[i].labelPoint1Gray;
                tablaDenom[i].labelPoint1Green = scroll[i].labelPoint1Green;
                tablaDenom[i].labelPoint2Gray = scroll[i].labelPoint2Gray;
                tablaDenom[i].labelPoint2Green = scroll[i].labelPoint2Green;
                tablaDenom[i].labelPoint3Gray = scroll[i].labelPoint3Gray;
                tablaDenom[i].labelPoint3Green = scroll[i].labelPoint3Green;
            }

            corouserTablaDenom.ItemsSource = tablaDenom;

            List<ListaProductos> listCategProd = new List<ListaProductos>();
            var productos = await App.Database.GetProductos();
            if (productos.Count > 0)
            {
                foreach (var item in productos)
                {
                    int index = listCategProd.FindIndex(x => x.Denominacion == item.Denominacion);
                    if (index == -1)
                    {
                        listCategProd.Add(new ListaProductos()
                        {
                            ListPro = new List<Product>()
                            {
                               item
                            },
                            Denominacion = item.Denominacion
                        });
                    }
                    else
                    {
                        listCategProd[index].ListPro.Add(item);
                    }
                }
                List<Scroll> scrollPage2 = LlenarScroll(listCategProd.Count);
                for (int i = 0; i < listCategProd.Count; i++)
                {
                    listCategProd[i].labelArrowLeft = scrollPage2[i].labelArrowLeft;
                    listCategProd[i].labelArrowRigth = scrollPage2[i].labelArrowRigth;
                    listCategProd[i].labelPoint1Gray = scrollPage2[i].labelPoint1Gray;
                    listCategProd[i].labelPoint1Green = scrollPage2[i].labelPoint1Green;
                    listCategProd[i].labelPoint2Gray = scrollPage2[i].labelPoint2Gray;
                    listCategProd[i].labelPoint2Green = scrollPage2[i].labelPoint2Green;
                    listCategProd[i].labelPoint3Gray = scrollPage2[i].labelPoint3Gray;
                    listCategProd[i].labelPoint3Green = scrollPage2[i].labelPoint3Green;
                }
                CorouselProductos.ItemsSource = listCategProd;
            }



            var ListRegE = await App.Database.GetRegExtraordinaria();
            var ListRegA = await App.Database.GetRegAcumulativa();
            var ListRegC = await App.Database.GetRegConstante();

            List<Page3Acumulativa> listInvA = new List<Page3Acumulativa>();
            var ListInvAcumulativa = await App.Database.GetInvAcumulativa();
            double sumaRelevancia = 0;
            ListRegA.ForEach(x => sumaRelevancia += x.Costo * x.Unidades);
            ListRegC.ForEach(x => sumaRelevancia += x.Costo * x.Unidades);
            ListRegE.ForEach(x => sumaRelevancia += x.Costo * x.Unidades);
            foreach (var item in ListInvAcumulativa)
            {
                double sumaTotal = 0;
                var regItemA = await App.Database.GetIdInvRegAcumulativa(item.Id);
                regItemA.ForEach(x => sumaTotal += x.Costo * x.Unidades);
                int relevanciaAcumulativa = 0;
                if (sumaTotal != 0)
                {
                    relevanciaAcumulativa = Convert.ToInt32((sumaTotal * 100) / sumaRelevancia);
                }
                listInvA.Add(new Page3Acumulativa() { 
                Descripcion = item.Descripcion,
                GastoTotal = "$ " + sumaTotal,
                Relevancia = relevanciaAcumulativa + " %",
                Nombre = item.Nombre
                });
            }
            listViewAcumulativo.ItemsSource = listInvA;

            List<Page3Constante> ListInvC = new List<Page3Constante>();
            var ListInvConstantes = await App.Database.GetInvConstante();
            foreach (var item in ListInvConstantes)
            {
                var regItem = await App.Database.GetIdInvRegConstante(item.Id);
                List<Page3Extraordinario> listConstPage3 = new List<Page3Extraordinario>();
                var init = await App.Database.GetIdFechaInicio(1);
                DateTime anterior = init.inicio;
                foreach (var elem in regItem)
                {
                    int relevanciaTotalPage3C = 0;
                    var sumaRETemporal = ListRegE.FindAll(x => (x.Fecha.DayOfYear <= elem.Fecha.DayOfYear || x.Fecha.Year <= elem.Fecha.Year) && (x.Fecha.DayOfYear >= anterior.DayOfYear || x.Fecha.Year >= anterior.Year));
                    var sumaRATemporal = ListRegA.FindAll(x => (x.Fecha.DayOfYear <= elem.Fecha.DayOfYear || x.Fecha.Year <= elem.Fecha.Year) && (x.Fecha.DayOfYear >= anterior.DayOfYear || x.Fecha.Year >= anterior.Year));
                    var sumaRCTemporal = ListRegC.FindAll(x => (x.Fecha.DayOfYear <= elem.Fecha.DayOfYear || x.Fecha.Year <= elem.Fecha.Year) && (x.Fecha.DayOfYear >= anterior.DayOfYear || x.Fecha.Year >= anterior.Year));
                    sumaRETemporal.ForEach(x => relevanciaTotalPage3C += Convert.ToInt32(x.Costo * x.Unidades));
                    sumaRATemporal.ForEach(x => relevanciaTotalPage3C += Convert.ToInt32(x.Costo * x.Unidades));
                    sumaRCTemporal.ForEach(x => relevanciaTotalPage3C += Convert.ToInt32(x.Costo * x.Unidades));
                    int relevanciaPage3 = Convert.ToInt32(((elem.Costo * elem.Unidades) * 100) / relevanciaTotalPage3C);
                    listConstPage3.Add(new Page3Extraordinario { 
                    Costo = "$ " + (elem.Costo * elem.Unidades),
                    Descripcion = elem.Fecha.Day + "/" + elem.Fecha.Month + "/" + elem.Fecha.Year,
                    Relevancia = relevanciaPage3 + " %"
                    });
                    anterior = elem.Fecha;
                }
                ListInvC.Add(new Page3Constante() { 
                Nombre = item.Nombre,
                ListConstPage3 = listConstPage3
                });
            }
            List<Scroll> scrollPage3 = LlenarScroll(ListInvC.Count);
            for (int i = 0; i < ListInvC.Count; i++)
            {
                ListInvC[i].labelArrowLeft = scrollPage3[i].labelArrowLeft;
                ListInvC[i].labelArrowRigth = scrollPage3[i].labelArrowRigth;
                ListInvC[i].labelPoint1Gray = scrollPage3[i].labelPoint1Gray;
                ListInvC[i].labelPoint1Green = scrollPage3[i].labelPoint1Green;
                ListInvC[i].labelPoint2Gray = scrollPage3[i].labelPoint2Gray;
                ListInvC[i].labelPoint2Green = scrollPage3[i].labelPoint2Green;
                ListInvC[i].labelPoint3Gray = scrollPage3[i].labelPoint3Gray;
                ListInvC[i].labelPoint3Green = scrollPage3[i].labelPoint3Green;
            }
            CarouselConstante.ItemsSource = ListInvC;

            List<Page3Extraordinario> ListInvE = new List<Page3Extraordinario>();
            foreach (var item in ListRegE)
            {
                var inv = await App.Database.GetIdInvExtraordinaria(item.IdInv);
                string costo = "$ " + (item.Costo * item.Unidades).ToString();

                var sumaRE = ListRegE.FindAll(x => x.Fecha.Month == item.Fecha.Month && x.Fecha.Year == item.Fecha.Year);
                var sumaRA = ListRegA.FindAll(x => x.Fecha.Month == item.Fecha.Month && x.Fecha.Year == item.Fecha.Year);
                var sumaRC = ListRegC.FindAll(x => x.Fecha.Month == item.Fecha.Month && x.Fecha.Year == item.Fecha.Year);
                int relevanciaTotalPage3 = 0;
                sumaRE.ForEach(x => relevanciaTotalPage3 += Convert.ToInt32(x.Costo * x.Unidades));
                sumaRC.ForEach(x => relevanciaTotalPage3 += Convert.ToInt32(x.Costo * x.Unidades));
                sumaRA.ForEach(x => relevanciaTotalPage3 += Convert.ToInt32(x.Costo * x.Unidades));
                int relevanciaPage3 = Convert.ToInt32(((item.Costo * item.Unidades) * 100) / relevanciaTotalPage3);
                ListInvE.Add(new Page3Extraordinario()
                {
                    Fecha = item.Fecha,
                    Nombre = inv.Nombre,
                    Descripcion = inv.Descripcion,
                    Costo = costo,
                    Relevancia = relevanciaPage3 + " %"
                });
            }
            listExtraordinario.ItemsSource = ListInvE;
        }
        private List<Scroll> LlenarScroll(int total)
        {
            List<Scroll> res = new List<Scroll>();
            string labelArrowLeft = "";
            string labelPoint1Gray = "";
            string labelPoint1Green = "";
            string labelPoint2Gray = "";
            string labelPoint2Green = "";
            string labelPoint3Gray = "";
            string labelPoint3Green = "";
            string labelArrowRigth = "";
            if (total > 0) labelPoint1Gray = ".";
            if (total > 1) labelPoint2Gray = ".";
            if (total > 2) labelPoint3Gray = ".";
            if (total > 3) labelArrowRigth = "+";
            for (int i = 1; i <= total; i++)
            {
                if (i == 1) 
                {
                    labelPoint1Gray = ""; labelPoint1Green = ".";
                    if (labelPoint2Green == ".") { labelPoint2Green = ""; labelPoint2Gray = "."; }
                    if (labelPoint3Green == ".") { labelPoint3Green = ""; labelPoint3Gray = "."; }
                }
                if (i == 2) 
                {
                    labelPoint2Gray = ""; labelPoint2Green = ".";
                    if (labelPoint1Green == ".") { labelPoint1Green = ""; labelPoint1Gray = "."; }
                    if (labelPoint3Green == ".") { labelPoint3Green = ""; labelPoint3Gray = "."; }
                }
                if (i == 3)
                {
                    labelPoint3Gray = ""; labelPoint3Green = ".";
                    if (labelPoint2Green == ".") { labelPoint2Green = ""; labelPoint2Gray = "."; }
                    if (labelPoint1Green == ".") { labelPoint1Green = ""; labelPoint1Gray = "."; }
                }
                if (i >= 4)
                {
                    labelArrowLeft = "-";
                }
                if (i == total && total > 3)
                {
                    labelArrowRigth = "";
                }
                res.Add(new Scroll() { 
                labelArrowLeft = labelArrowLeft,
                labelArrowRigth = labelArrowRigth,
                labelPoint1Gray = labelPoint1Gray,
                labelPoint1Green = labelPoint1Green,
                labelPoint2Gray = labelPoint2Gray,
                labelPoint2Green = labelPoint2Green,
                labelPoint3Gray = labelPoint3Gray,
                labelPoint3Green = labelPoint3Green
                });
            }
            return res;
        }
        public static int posicionCarouselView = 0;
        private void ViewTiempos_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            posicionCarouselView = (int)e.CurrentPosition;
        }
        public static List<DonusValue> donusIngresos = new List<DonusValue>();
        public static List<DonusValue> donusInversion = new List<DonusValue>();
        private async void Button_Clicked(object sender, EventArgs e)
        {
            donusIngresos = new List<DonusValue>();
            donusInversion = new List<DonusValue>();
            bool diasi = true;

            List<CierresDiarios> elementos = await App.Database.GetCierreDiario();
            var dia = elementos[elementos.Count - posicionCarouselView - 1];
            List<CierresMensual> elementosM = await App.Database.GetCierreMensual();
            var meses = elementosM[0];
            if (elementosM.Count > 1) meses = elementosM[elementosM.Count - posicionCarouselView - 1];

            if (elementosM.Count > 1)
            {
                diasi = false;
            }

            var invercionesC = await App.Database.GetRegConstante();
            foreach (var item in invercionesC)
            {
                bool sepuede = false;
                if (diasi && item.Fecha.DayOfYear == dia.Fecha.DayOfYear && item.Fecha.Year == dia.Fecha.Year) sepuede = true;
                if (diasi == false && item.Fecha.Month == meses.Fecha.Month && item.Fecha.Year == meses.Fecha.Year) sepuede = true;

                if (sepuede)
                {
                    var elem = await App.Database.GetIdInvConstante(item.IdInv);
                    int index =  donusInversion.FindIndex(x => x.Nombre == elem.Nombre);
                    if (index != -1) donusInversion[index].Value += item.Costo + item.Unidades;
                    else {
                        donusInversion.Add(new DonusValue()
                        {
                            Nombre = elem.Nombre,
                            Value = item.Costo * item.Unidades
                        });
                    }
                    
                }
            }

            var invercionesE = await App.Database.GetRegExtraordinaria();
            foreach (var item in invercionesE)
            {
                bool sepuede = false;
                if (diasi && item.Fecha.DayOfYear == dia.Fecha.DayOfYear && item.Fecha.Year == dia.Fecha.Year) sepuede = true;
                if (diasi == false && item.Fecha.Month == meses.Fecha.Month && item.Fecha.Year == meses.Fecha.Year) sepuede = true;

                if(sepuede)
                {
                    var elem = await App.Database.GetIdInvExtraordinaria(item.IdInv);
                    int index = donusInversion.FindIndex(x => x.Nombre == elem.Nombre);
                    if (index != -1) donusInversion[index].Value += item.Costo + item.Unidades;
                    else
                    {
                        donusInversion.Add(new DonusValue()
                        {
                            Nombre = elem.Nombre,
                            Value = item.Costo * item.Unidades
                        });
                    }
                        
                }  
            }

            var invercionesA = await App.Database.GetRegAcumulativa();
            foreach (var item in invercionesA)
            {
                bool sepuede = false;
                if (diasi && item.Fecha.DayOfYear == dia.Fecha.DayOfYear && item.Fecha.Year == dia.Fecha.Year) sepuede = true;
                if (diasi == false && item.Fecha.Month == meses.Fecha.Month && item.Fecha.Year == meses.Fecha.Year) sepuede = true;
                if (sepuede)
                {
                    var elem = await App.Database.GetIdInvAcumulativa(item.IdInv);
                    int index = donusInversion.FindIndex(x => x.Nombre == elem.Nombre);
                    if (index != -1) donusInversion[index].Value += item.Costo + item.Unidades;
                    else
                    {
                        donusInversion.Add(new DonusValue()
                        {
                            Nombre = elem.Nombre,
                            Value = item.Costo * item.Unidades
                        });
                    }
                        
                }
            }

            var diaSelectedV = await App.Database.GetVenta();            
            foreach (var item in diaSelectedV)
            {
                bool sepuede = false;
                if (diasi && item.Fecha.DayOfYear == dia.Fecha.DayOfYear && item.Fecha.Year == dia.Fecha.Year) sepuede = true;
                if (diasi == false && item.Fecha.Month == meses.Fecha.Month && item.Fecha.Year == meses.Fecha.Year) sepuede = true;

                if (sepuede)
                {
                    var elem = await App.Database.GetIdProductos(item.IdPro);
                    int index = donusIngresos.FindIndex(x => x.Nombre == elem.Nombre);
                    if (index != -1) donusIngresos[index].Value += item.Precio + item.Unidades;
                    else
                    {
                        donusIngresos.Add(new DonusValue()
                        {
                            Nombre = elem.Nombre,
                            Value = item.Precio * item.Unidades
                        });
                    }    
                       
                }

            }

            await Navigation.PushPopupAsync(new PopupDonus());
        }
        public static int posicionCarouselViewProductos = 0;
        private void CorouselProductos_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            posicionCarouselViewProductos = (int)e.CurrentPosition;
        }
        public static Product ProductoMostrarPopup = new Product();
        private async void ListViewproductos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ProductoMostrarPopup = (Product)e.SelectedItem;
            await Navigation.PushPopupAsync(new popupproductSelected());
        }
        public static List<Venta> denomSelectedDonus = new List<Venta>();
        public static bool cantSiGananciaNo = new bool();
        private async void SfChart_LegendItemClicked(object sender, Syncfusion.SfChart.XForms.ChartLegendItemClickedEventArgs e)
        {
            cantSiGananciaNo = false;
            var denom = e.LegendItem.Label;
            denomSelectedDonus = await App.Database.GetByDenomVenta(denom);
            await Navigation.PushPopupAsync(new popupDenomSelected());
        }
        private async void SfChart_LegendItemClicked_1(object sender, Syncfusion.SfChart.XForms.ChartLegendItemClickedEventArgs e)
        {
            cantSiGananciaNo = true;
            var denom = e.LegendItem.Label;
            denomSelectedDonus = await App.Database.GetByDenomVenta(denom);
            await Navigation.PushPopupAsync(new popupDenomSelected());
        }
        public static int posicionCarouselTablaCompleta = 0;
        private void corouserTablaDenom_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            posicionCarouselTablaCompleta = (int)e.CurrentPosition;            
        }
        public static Page3Acumulativa InvAcumMostrarPopup = new Page3Acumulativa();
        private async void listViewAcumulativo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            InvAcumMostrarPopup = (Page3Acumulativa)e.SelectedItem;
            await Navigation.PushPopupAsync(new historialInversion());
        }
    }


    public class Page3Acumulativa
    {
        public string Nombre { get; set; }
        public string GastoTotal { get; set; }
        public string Relevancia { get; set; }
        public string Descripcion { get; set; }
    }
    public class Page3Extraordinario : InExtraordinaria
    {
        public string Costo { get; set; }
        public string Relevancia { get; set; }
    }
    public class Page3Constante : Scroll
    {
        public string Nombre { get; set; }
        public List<Page3Extraordinario> ListConstPage3 { get; set; }
    }
    public class Scroll
    {
        public string labelArrowLeft { get; set; }
        public string labelPoint1Gray { get; set; }
        public string labelPoint1Green { get; set; }
        public string labelPoint2Gray { get; set; }
        public string labelPoint2Green { get; set; }
        public string labelPoint3Gray { get; set; }
        public string labelPoint3Green { get; set; }
        public string labelArrowRigth { get; set; }
    }
    public class VentasDenom
    {
        public DateTime Fecha { get; set; }
        public double Value { get; set; }
    }
    public class DonusValue
    {
        public string Nombre { get; set; }
        public double Value { get; set; }
    }   
    public class HistorialGeneral
    {
        public double Ingreso { get; set; }
        public double Gasto { get; set; }
        public double Ganancia { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class Historial : Scroll
    {
        public double Ingreso { get; set; }
        public double GastoC { get; set; }
        public double GastoA { get; set; }
        public double GastoE { get; set; }
        public string Fecha { get; set; }
        public double Ganancia { get; set; }
        public double Estimado { get; set; }
        public int Porcentaje { get; set; }
    }
    public class ListaProductos : Scroll
    {
        public List<Product> ListPro { get; set; }
        public string Denominacion { get; set; }
    }
    public class TablaDenom : TablaProducto 
    {
        public string Denominacion { get; set; }
    }
    public class VMG
    {
        
        public ObservableCollection<HistorialGeneral> historialGeneral { get; set; }    
        public VMG()
        {
            historialGeneral = new ObservableCollection<HistorialGeneral>();
            LlenarVm();

        }
        private async void LlenarVm()
        {
            var history = await App.Database.GetCierreDiario();
            if (history.Count > 0)
            {
                if (history.Count > 31)
                {
                    List<CierresMensual> historyM = await App.Database.GetCierreMensual();
                    foreach (var item in historyM)
                    {
                        historialGeneral.Add(new HistorialGeneral()
                        {
                            Fecha = item.Fecha,
                            Ingreso = item.Ganancia,
                            Gasto = item.GastoA + item.GastoC + item.GastoE,
                            Ganancia = item.Ganancia - (item.GastoA + item.GastoC + item.GastoE)
                        });                       
                        
                    }
                }
                else
                {
                    foreach (var item in history)
                    {
                        historialGeneral.Add(new HistorialGeneral()
                        {
                            Fecha = item.Fecha,
                            Ingreso = item.Ingreso,
                            Gasto = item.GastoA + item.GastoC + item.GastoE,
                            Ganancia = item.Ingreso - (item.GastoA + item.GastoC + item.GastoE),
                        });
                    }
                }
            }

            
        }
    }
    public class VMP
    {
        public ObservableCollection<DonusValue> historialDenominacion { get; set; }
        public ObservableCollection<DonusValue> historialDenominacionCant { get; set; }
        public VMP()
        {
            historialDenominacion = new ObservableCollection<DonusValue>();
            historialDenominacionCant = new ObservableCollection<DonusValue>();
            LlenarVm();
        }

        private async void LlenarVm()
        {
            
            var productos = await App.Database.GetProductos();
            foreach (var item in productos)
            {
                bool sepuede = true;
                foreach (var elem in historialDenominacion) if (elem.Nombre == item.Denominacion) sepuede = false;
                if(sepuede)
                {
                    var v = await App.Database.GetByDenomVenta(item.Denominacion);
                    double ingresos = 0;
                    double cantventas = 0;
                    foreach (var elem in v)
                    {
                        ingresos += elem.Precio * elem.Unidades; 
                        cantventas += elem.Unidades;
                    }
                    
                    historialDenominacion.Add(new DonusValue { Value = ingresos, Nombre = item.Denominacion });
                    historialDenominacionCant.Add(new DonusValue { Value = cantventas, Nombre = item.Denominacion });
                }
            }

        }
    }
}