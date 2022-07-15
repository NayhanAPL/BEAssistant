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
    public partial class popupDenomSelected : PopupPage
    {
        public popupDenomSelected()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
    public class VMDenomSelected
    {
        public ObservableCollection<VentasDenom> auxVentasDenom { get; set; }
        public VMDenomSelected()
        {
            auxVentasDenom = new ObservableCollection<VentasDenom>();
            llenarDatos();
        }

        private void llenarDatos()
        {
            ObservableCollection<VentasDenom> VentasDenom = new ObservableCollection<VentasDenom>();
            ObservableCollection<VentasDenom> VentasDenomCant = new ObservableCollection<VentasDenom>();
            List<VentasDenom> listaOrdenada = new List<VentasDenom>();
            List<VentasDenom> listaOrdenadaCant = new List<VentasDenom>();
            foreach (var item in TabbedEstadisticas.denomSelectedDonus)
            {
                int index = listaOrdenada.FindIndex(x => x.Fecha.DayOfYear == item.Fecha.DayOfYear && x.Fecha.Year == item.Fecha.Year);
                if (index != -1) {listaOrdenada[index].Value += item.Precio * item.Unidades; listaOrdenadaCant[index].Value += item.Unidades; }
                else
                {
                    listaOrdenada.Add(new VentasDenom() { Fecha = item.Fecha, Value = item.Precio * item.Unidades });
                    listaOrdenadaCant.Add(new VentasDenom() { Fecha = item.Fecha, Value = item.Unidades });
                }
            }
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            listaOrdenada.OrderByDescending(x => TimeActual - x.Fecha);
            listaOrdenadaCant.OrderByDescending(x => TimeActual - x.Fecha);
            LlamarFechaInicio();

            List<VentasDenom> AuxlistaOrdenada = new List<VentasDenom>();
            if (TabbedEstadisticas.cantSiGananciaNo)
            {
                AuxlistaOrdenada = listaOrdenadaCant;
                auxVentasDenom = VentasDenomCant;
            }
            else
            {
                AuxlistaOrdenada = listaOrdenada;
                auxVentasDenom = VentasDenom;
            }
            if (TimeActual.Month > fechaini.inicio.Month && TimeActual.Day >= fechaini.inicio.Day)
            {
                int mes = fechaini.inicio.Month;
                int cont = 0;

                auxVentasDenom.Add(new VentasDenom() { Fecha = fechaini.inicio, Value = 0 });
                foreach (var item in AuxlistaOrdenada)
                {
                    if (item.Fecha.Month == mes)
                    {
                        auxVentasDenom[cont].Value += item.Value;
                        auxVentasDenom[cont].Fecha = item.Fecha;
                    }
                    else
                    {
                        cont++;
                        mes = item.Fecha.Month;
                        auxVentasDenom.Add(new VentasDenom()
                        {
                            Fecha = item.Fecha,
                            Value = item.Value
                        });
                    }
                }
            }
            else AuxlistaOrdenada.ForEach(x => auxVentasDenom.Add(x));
        }
        public static FechaInicio fechaini = new FechaInicio();
        private async void LlamarFechaInicio()
        {
            fechaini = await App.Database.GetIdFechaInicio(1);
        }
    }
}