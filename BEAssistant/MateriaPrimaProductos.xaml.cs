using BEAssistant.popups;
using Rg.Plugins.Popup.Extensions;
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
    public partial class MateriaPrimaProductos : ContentPage
    {
        public MateriaPrimaProductos()
        {
            InitializeComponent();
            
            ByDefault();
        }
        public static List<MatPri>matPri = new List<MatPri>();
        public static int cont = 0;
        private async void ByDefault()
        {
            EntryMatPri.IsVisible = true;
            PickerMatPri.IsVisible = true;
            EntryMatPri.Text = "";
            EntryCantUnid.Text = "";
            if (TabbedProductos.ultimoPorDenom != null)
            {
                var lista = await App.Database.GetByIdProMateriaPrima(TabbedProductos.ultimoPorDenom.Id);
                if (lista.Count < cont)
                {
                    EntryMatPri.Text = lista[cont].Nombre;
                    EntryCantUnid.Text = lista[cont].CantidadPorProducto.ToString();
                    cont++;
                }               
            }
            List<string> listaMatPri = new List<string>();
            var cons = await App.Database.GetByTipoInvConstante((int)TiposConstante.MateriaPrima);
            var acum = await App.Database.GetByTipoInvAcumulativa((int)TiposAcumulativa.MateriaPrima);

            if (cons.Count != 0)
                foreach (var item in cons) { listaMatPri.Add(item.Nombre); }
            if (acum.Count != 0)
                foreach (var item in acum) { listaMatPri.Add(item.Nombre); }
            if (listaMatPri.Count != 0)
            {
                PickerMatPri.Items.Clear();
                foreach (var item in listaMatPri) { PickerMatPri.Items.Add(item); }
            }
        }        
        private async void AgregarMatPri_Clicked(object sender, EventArgs e)
        {
            string nombre = "";
            double cantUnid = 1;
            if(!string.IsNullOrEmpty(EntryCantUnid.Text))
                cantUnid = Convert.ToDouble(EntryCantUnid.Text);
            if (!string.IsNullOrEmpty(EntryMatPri.Text)) nombre = EntryMatPri.Text;
            else nombre = PickerMatPri.SelectedItem.ToString();

            bool existe = false;
            foreach (var item in matPri)
            {
                if (item.Nombre == nombre) existe = true;
            }

            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(EntryCantUnid.Text) && !existe)
            {
                matPri.Add(new MatPri() { Nombre = nombre, Cantidad = cantUnid });
                ListViewMatPri.ItemsSource = null;
                ListViewMatPri.ItemsSource = matPri;
                PopupAlert.PopupLabelTitulo = "CORRECTO";
                PopupAlert.PopupLabelText = "La materia prima se guardó correctamente, puede volver o agregar otra materia prima para este producto";
                await Navigation.PushPopupAsync(new PopupAlert());
                if (!string.IsNullOrEmpty(EntryMatPri.Text))
                {
                    PopupAlert.PopupLabelTitulo = "ADVERTENCIA";
                    PopupAlert.PopupLabelText = "Acaba de declarar una materia prima que no se ha reportado en las inversiones. Se recomienda que la reporte cuanto antes, las sugerencias le mostrarán el elemento que acaba de crear.";
                    await Navigation.PushPopupAsync(new PopupAlert());
                }
            }
            else
            {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe llenar todos los datos que se le solicitan";
                await Navigation.PushPopupAsync(new PopupAlert());
            } 
        }
        private void VOLVER_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        private void AÑADIROTRA_Clicked(object sender, EventArgs e)
        {
            ByDefault();
        }
    }
    public class MatPri
    {
        public string Nombre { get; set; }
        public double Cantidad { get; set; }
    }
}