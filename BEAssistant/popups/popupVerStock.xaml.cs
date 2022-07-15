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
    public partial class popupVerStock : PopupPage
    {
        public popupVerStock()
        {
            InitializeComponent();
            LlenarLista();
        }

        private async void LlenarLista()
        {
            List<Stocker> view = new List<Stocker>();
            var stockA = await App.Database.GetByTipoInvStock("Acumulativa");
            var stockC = await App.Database.GetByTipoInvStock("Constante");
            var invA = await App.Database.GetByTipoInvAcumulativa((int)TiposAcumulativa.MateriaPrima);
            var invC = await App.Database.GetByTipoInvConstante((int)TiposConstante.MateriaPrima);
            foreach (var item in stockA)
            {
                string nombre = invA.Find(x => x.Id == item.IdInv).Nombre;
                view.Add(new Stocker() { 
                    Id = item.Id, 
                    IdInv = item.IdInv, 
                    Duracion = item.Duracion, 
                    UnidadesEstimadas = item.UnidadesEstimadas, 
                    Categoria = item.Categoria, 
                    TipoInv = nombre, 
                    CantActual = item.CantActual 
                }) ;
            }
            foreach (var item in stockC)
            {
                string nombre = invC.Find(x => x.Id == item.IdInv).Nombre;
                view.Add(new Stocker() { 
                    Id = item.Id, 
                    IdInv = item.IdInv, 
                    Duracion = item.Duracion, 
                    UnidadesEstimadas = item.UnidadesEstimadas, 
                    Categoria = item.Categoria, 
                    TipoInv = nombre, 
                    CantActual = item.CantActual 
                });
            }
            var iview = view.OrderBy(x => x.Categoria);
            int cont = 0;
            string categoria = "";
            foreach (var item in iview)
            {
                view[cont] = item;
                if (view[cont].Categoria != categoria) categoria = view[cont].Categoria;
                else view[cont].Categoria = "";
                cont++;
            }
            listStock.ItemsSource = view;
        }
        public static Stocker stockSelected = new Stocker();
        private async void listStock_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            stockSelected = (Stocker)e.SelectedItem;
            await Navigation.PushPopupAsync(new popupStockSelected());
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}