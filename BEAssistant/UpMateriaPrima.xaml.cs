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
    public partial class UpMateriaPrima : PopupPage
    {
        public UpMateriaPrima()
        {
            InitializeComponent();
            LlenarDatos();
        }

        private async void LlenarDatos()
        {
            var materiasPrimas = await App.Database.GetByIdProMateriaPrima(TabbedProductos.page3ProSelected.Id);
            ListMatPri.ItemsSource = materiasPrimas;
        }
        public static MateriasPrimas mp = new MateriasPrimas();
        private void MateriaPrima_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            mp = (MateriasPrimas)e.SelectedItem;
            EntryPrecio.Text = mp.PrecioUnidad.ToString();
            EntryUnidades.Text = mp.CantidadPorProducto.ToString();
        }

        private async void UpMP_Clicked(object sender, EventArgs e)
        {
            int cp = 0;
            if (!String.IsNullOrEmpty(EntryPrecio.Text)) cp = Convert.ToInt32(EntryPrecio.Text);
            int pu = 0;
            if (!String.IsNullOrEmpty(EntryUnidades.Text)) pu = Convert.ToInt32(EntryUnidades.Text);
            MateriasPrimas up = new MateriasPrimas()
            {
                Id = mp.Id,
                IdProducto = mp.IdProducto,
                Nombre = mp.Nombre,
                CantidadPorProducto = cp,
                PrecioUnidad = pu
            };
            await App.Database.SaveUpMateriaPrima(up);
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}