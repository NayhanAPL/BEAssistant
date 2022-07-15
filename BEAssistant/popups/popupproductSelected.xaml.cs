using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BEAssistant.popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class popupproductSelected : PopupPage
    {
        public popupproductSelected()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }

    public class TablaProducto : Scroll
    {
        public double Ingreso { get; set; }
        public double Ganancia { get; set; }
        public double Venta { get; set; }
        public int Relevancia { get; set; }
    }
    public class VMProductos
    {
        public TablaProducto tablaProducto { get; set; }
        public ObservableCollection<VentasDenom> ventaElem { get; set; }
        public ObservableCollection<DonusValue> porcentageDenom { get; set; }
        public VMProductos()
        {
            ventaElem = new ObservableCollection<VentasDenom>();
            tablaProducto = new TablaProducto();
            porcentageDenom = new ObservableCollection<DonusValue>();
            LlenarDatos();
        }

        private void LlenarDatos()
        {
            LlenarDatosAsync();
            var ventas = totalVentasDenom.FindAll(x => x.IdPro == TabbedEstadisticas.ProductoMostrarPopup.Id);
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();

            if (TimeActual.Month > fechaini.inicio.Month && TimeActual.Day >= fechaini.inicio.Day)
            {
                int mes = fechaini.inicio.Month;
                int cont = 0;
                ventaElem.Add(new VentasDenom() { Fecha = fechaini.inicio, Value = 0 });
                foreach (var item in ventas)
                {
                    if (item.Fecha.Month == mes)
                    {
                        ventaElem[cont].Value += Convert.ToInt32(item.Unidades);
                        ventaElem[cont].Fecha = item.Fecha;
                    }
                    else
                    {
                        cont++;
                        mes = item.Fecha.Month;
                        ventaElem.Add(new VentasDenom()
                        {
                            Fecha = item.Fecha,
                            Value = Convert.ToInt32(item.Unidades)
                        });
                    }
                }
            }
            else ventas.ForEach(x => ventaElem.Add(new VentasDenom() { Fecha = x.Fecha, Value = Convert.ToInt32(x.Unidades) }));


            var otrasVentas = totalVentasDenom.FindAll(x => x.IdPro != TabbedEstadisticas.ProductoMostrarPopup.Id);
            double sumaventas = 0;
            double sumaotrasVentas = 0;
            ventas.ForEach(x => sumaventas += x.Precio * x.Unidades);
            otrasVentas.ForEach(x => sumaotrasVentas += x.Precio * x.Unidades);
            porcentageDenom.Add(new DonusValue()
            {
                Nombre = "Otros",
                Value = sumaotrasVentas
            });
            porcentageDenom.Add(new DonusValue()
            {
                Nombre = TabbedEstadisticas.ProductoMostrarPopup.Nombre,
                Value = sumaventas
            });

            double ingreso = 0;
            double ganancia = 0;
            double relevancia = 0;
            
            foreach (var item in ventas)
            {
                ingreso += item.Precio * item.Unidades;
                ganancia += (item.Precio * item.Unidades) - TabbedEstadisticas.ProductoMostrarPopup.Inversion;  
            }
            totalVentas.ForEach(y => relevancia += (y.Precio * y.Unidades) - totalProductos.Find(x => x.Id == y.IdPro).Inversion);
            if (relevancia != 0)
            {
                relevancia = (ganancia * 100) / relevancia;
            }
           
            tablaProducto = new TablaProducto()
            {
                Ganancia = ganancia,
                Ingreso = ingreso,
                Relevancia = Convert.ToInt32(relevancia),
                Venta = ventas.Count,
            };
        }

        public static List<Venta> totalVentasDenom = new List<Venta>();
        public static List<Venta> totalVentas = new List<Venta>();
        public static List<Product> totalProductos = new List<Product>();
        public static FechaInicio fechaini = new FechaInicio();
        private async void LlenarDatosAsync()
        {
            totalVentasDenom = await App.Database.GetByDenomVenta(TabbedEstadisticas.ProductoMostrarPopup.Denominacion);            
            fechaini = await App.Database.GetIdFechaInicio(1);
            totalVentas = await App.Database.GetVenta();
            totalProductos = await App.Database.GetProductos();
        }
    }
}