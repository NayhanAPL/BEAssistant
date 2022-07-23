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

namespace BEAssistant.popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class popupInventario : PopupPage
    {
        public popupInventario()
        {
            InitializeComponent();
            ConstruirTabla();
        }
        public static List<MateriasPrimas> listMateria = new List<MateriasPrimas>();
        public static List<CorouserInventario> listCoro = new List<CorouserInventario>();
        private async void ConstruirTabla()
        {
            listMateria = new List<MateriasPrimas>();
            listCoro = new List<CorouserInventario>();
            GridUpdate.IsVisible = false;
            List<Inventario> listaInventario = new List<Inventario>();
            var inventario = await App.Database.GetInventario();
            var inventarioOrdenado = inventario.OrderByDescending(x => x.Fecha);
            DateTime fechaAux = inventarioOrdenado.First().Fecha;
            foreach (var item in inventarioOrdenado)
            {
                if (fechaAux.DayOfYear != item.Fecha.DayOfYear)
                {    
                    listCoro.Add(new CorouserInventario()
                    {
                        Fecha = fechaAux.Day + "/" + fechaAux.Month + "/" + fechaAux.Year,
                        ListInventario = new List<Inventario>(listaInventario)
                    }) ;
                    fechaAux = item.Fecha;
                    listaInventario.Clear();
                }
                else
                {
                    if(item.Materias != "")
                    {
                        listaInventario.Add(new Inventario()
                        {
                            ConsCaduco = item.ConsCaduco,
                            ConsUtil = item.ConsUtil,
                            ConsExedente = item.ConsExedente,
                            Fecha = item.Fecha,
                            Id = item.Id,
                            Materias = item.Materias,
                        });
                    }
                    
                }
            }
            listCoro.Add(new CorouserInventario()
            {
                Fecha = fechaAux.Day + "/" + fechaAux.Month + "/" + fechaAux.Year,
                ListInventario = listaInventario
            });
            CarouselInventarioFechas.ItemsSource = listCoro;

            List<MateriasPrimas> matPrim = await App.Database.GetMateriaPrima();
            List<MateriasPrimas> listaSinRepetir = new List<MateriasPrimas>();
            listaSinRepetir.Add(matPrim[0]);
            foreach (var item in matPrim)
            {
                int index = listaSinRepetir.FindIndex(x => x.Nombre == item.Nombre);
                if (index == -1){ listaSinRepetir.Add(item); }
            }
            listMateria.Clear();
            listMateria.AddRange(listaSinRepetir);
            listMateria.ForEach(x => selectMateria.Items.Add(x.Nombre));
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
        private void ModificarInventario_Clicked(object sender, EventArgs e)
        {
            GridUpdate.IsVisible = true;
        }
        public static int indexMateria = -1;
        private void selectMateria_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexMateria = selectMateria.SelectedIndex;
        }
        private async void GuardarCambiosInventario_Clicked(object sender, EventArgs e)
        {
            double util = 0;
            double caduco = 0;
            double perdido = 0;
            if (!string.IsNullOrEmpty(EntryUtil.Text)) util = Convert.ToDouble(EntryUtil.Text);
            if (!string.IsNullOrEmpty(EntryCaduco.Text)) caduco = Convert.ToDouble(EntryCaduco.Text);
            if (!string.IsNullOrEmpty(EntryExedente.Text)) perdido = Convert.ToDouble(EntryExedente.Text);
            if (indexMateria == -1)
            {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe seleccionar una materia prima para guardar en el inventario.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            else
            {
                Inventario elem = listCoro[movCoro].ListInventario.Find(x => x.Materias == listMateria[indexMateria].Nombre);
                if(elem != null)
                {
                    Inventario up = new Inventario(){
                        Id = elem.Id,
                        ConsCaduco = caduco,
                        ConsExedente = perdido,
                        ConsUtil = util,
                        Fecha = elem.Fecha,
                        Materias = elem.Materias
                    }; await App.Database.SaveUpInventario(up);
                }
                else
                {
                    DateTime fecha;
                    if (listCoro[movCoro].ListInventario.Count != 0) fecha = listCoro[movCoro].ListInventario[0].Fecha;
                    else
                    {
                        string num = ""; int dia = 0; int mes = 0; int anno = 0; int cont = 0;
                        foreach (var item in listCoro[movCoro].Fecha)
                        { if (item == '/' && cont == 0) { dia = Convert.ToInt32(num); num = ""; cont++; }
                          else if (item == '/' && cont == 1) { mes = Convert.ToInt32(num); num = ""; }
                          else num += item;} anno = Convert.ToInt32(num);
                        fecha = new DateTime(day: dia, month: mes, year: anno);

                    }

                    await App.Database.SaveInventario(new Inventario()
                    {
                        Materias = listMateria[indexMateria].Nombre,
                        ConsCaduco = caduco,
                        ConsExedente = perdido,
                        ConsUtil = util,
                        Fecha = fecha
                    });
                }
            }

            CarouselInventarioFechas.ItemsSource = new List<string>();
            ConstruirTabla();
            GridUpdate.IsVisible = false;
            PopupAlert.PopupLabelTitulo = "CORRECTO";
            PopupAlert.PopupLabelText = "El inventario ha sido actualizado.";
            await Navigation.PushPopupAsync(new PopupAlert());
        }
        public static int movCoro = 0;
        private void CarouselInventarioFechas_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            movCoro = (int)e.CurrentPosition;
        }
    }
    public class CorouserInventario
    {
        public List<Inventario> ListInventario { get; set; }
        public string Fecha { get; set; }
    }
}