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
    public partial class PopupDonus : PopupPage
    {
        public PopupDonus()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
    public class DonusClass
    {    
        public  ObservableCollection<DonusValue> ing { get; set; }
        public ObservableCollection<DonusValue> inv { get; set; }

        public DonusClass()
        {
            ing = new ObservableCollection<DonusValue>();
            inv = new ObservableCollection<DonusValue>();
            LlenarElementos();
           
        }

        private void LlenarElementos()
        {
            TabbedEstadisticas.donusIngresos.ForEach(x => ing.Add(new DonusValue() { Value = x.Value, Nombre = x.Nombre }));
            TabbedEstadisticas.donusInversion.ForEach(x => inv.Add(new DonusValue() { Value = x.Value, Nombre = x.Nombre }));
        }
    }
}