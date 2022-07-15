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
    public partial class historialInversion : PopupPage
    {
        public historialInversion()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
    public class HistorialSelectedInvA
    {
        public ObservableCollection<VentasDenom> InvercionAcumulativa { get; set; }
        public HistorialSelectedInvA()
        {
            InvercionAcumulativa = new ObservableCollection<VentasDenom>();
            
            llenarDatos();
        }

        private async void llenarDatos()
        {
            EjecAsync();
            await Task.Delay(500);
            historia.ForEach(x => InvercionAcumulativa.Add(new VentasDenom() { Fecha = x.Fecha, Value = x.Costo * x.Unidades }));
           
        }
        public static List<ReAcumulativa> historia = new List<ReAcumulativa>();
        private async void EjecAsync()
        {
            var listHistoria = await App.Database.GetByNameInvAcumulativa(TabbedEstadisticas.InvAcumMostrarPopup.Nombre);
            historia = await App.Database.GetIdInvRegAcumulativa(listHistoria[0].Id);
        }
    }
        
}