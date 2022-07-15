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
    public partial class popupConstDepend : PopupPage
    {
        public popupConstDepend()
        {
            InitializeComponent();
            ButtonEstablecer.IsVisible = false;
            LlenarDatos();
        }
        private async void LlenarDatos()
        {
            List<DependenciasConstantes> listDepen = new List<DependenciasConstantes>();
            listDepen.Add(new DependenciasConstantes() { Item = "Ganancia general", Clase = "Ganancia" });
            listDepen.Add(new DependenciasConstantes() { Item = "Total de gastos", Clase = "Gasto" });
            listDepen.Add(new DependenciasConstantes() { Item = "Ingreso total", Clase = "Ingreso" });
            var prod = await App.Database.GetProductos();
            var invA = await App.Database.GetInvAcumulativa();
            var invC = await App.Database.GetInvConstante();
            var invE = await App.Database.GetInvExtraordinaria();

            invA.ForEach(x => listDepen.Add(new DependenciasConstantes() { Item = x.Nombre, Clase = "inAcumulativa", IdItem = x.Id }));
            invC.ForEach(x => listDepen.Add(new DependenciasConstantes() { Item = x.Nombre, Clase = "inConstante", IdItem = x.Id }));
            invE.ForEach(x => listDepen.Add(new DependenciasConstantes() { Item = x.Nombre, Clase = "inExtraordinaria", IdItem = x.Id }));
            prod.ForEach(x => listDepen.Add(new DependenciasConstantes() { Item = x.Nombre, Clase = "Product", IdItem = x.Id }));
            await Task.Delay(1000);
            ListItems.ItemsSource = listDepen;
        }
        private async void ButtonEstablecer_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryPorcentajeDepend.Text)) porsentaje = Convert.ToInt32(EntryPorcentajeDepend.Text);
            await PopupNavigation.Instance.PopAsync(true);
        }
        public static DependenciasConstantes itemDependenciaConst = new DependenciasConstantes();
        public static int porsentaje = 0;
        private void ListItems_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            itemDependenciaConst = (DependenciasConstantes)e.SelectedItem;
            ButtonEstablecer.IsVisible = true;
        }
        private async void volver_Clicked_1(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
        
    
}