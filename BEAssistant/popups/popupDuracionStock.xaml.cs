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
    public partial class popupDuracionStock : PopupPage
    {
        public popupDuracionStock()
        {
            InitializeComponent();
            LlenarCategorias();
        }

        private async void LlenarCategorias()
        {
            var stock = await App.Database.GetStock();
            List<string> categorias = new List<string>();
            foreach (var item in stock)
            {
                bool exist = categorias.Contains(item.Categoria);
                if (!exist) categorias.Add(item.Categoria);
            }
            categorias.ForEach(x => PickerCategoria.Items.Add(x));
        }

        public static int diasStock = 0;
        public static string CategoriaStock = "";
        public static int UnidadesEstimadas = 0;
        private void indefinido_Clicked(object sender, EventArgs e)
        {
            if (indefinido.TextColor == Color.Black)
            {
                indefinido.TextColor = Color.Green;
                indefinido.FontSize = 19;
                diasStock = 36500;
            }
            else
            {
                indefinido.TextColor = Color.Black;
                indefinido.FontSize = 18;
                diasStock = 0;
            }
            
        }

        private async void guardar_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(EntryTiempoDiaPro.Text)) diasStock += Convert.ToInt32(EntryTiempoDiaPro.Text);
            if (!String.IsNullOrEmpty(EntryTiempoMesPro.Text)) diasStock += 30 * Convert.ToInt32(EntryTiempoMesPro.Text);
            if (!String.IsNullOrEmpty(EntryTiempoAnnoPro.Text)) diasStock += 365 * Convert.ToInt32(EntryTiempoAnnoPro.Text);
            if (PickerCategoria.SelectedIndex != -1) CategoriaStock = PickerCategoria.SelectedItem.ToString();
            else if (PickerCategoria.SelectedIndex == -1 && !string.IsNullOrEmpty(EntryCategoria.Text)) CategoriaStock = EntryCategoria.Text;
            else if (PickerCategoria.Items.Count > 0 && string.IsNullOrEmpty(EntryCategoria.Text)) CategoriaStock = PickerCategoria.Items[0];
            else if (PickerCategoria.Items.Count == 0 && string.IsNullOrEmpty(EntryCategoria.Text)) CategoriaStock = "";
            if(!string.IsNullOrEmpty(EntryUnidadesEstimadas.Text)) UnidadesEstimadas = Convert.ToInt32(EntryUnidadesEstimadas.Text);
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}