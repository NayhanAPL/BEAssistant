using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BEAssistant
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ButtonEstadisticas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedEstadisticas());
        }

        private async void ButtonElementos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedProductos());
        }

        private async void ButtonOperaciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedConsultas());
        }

        private async void ButtonInverciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedInversiones());
        }

        private async void ButtonPropuestas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedPropuestas());
        }

        private async void ButtonRespuestasR_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedRespRapida());
        }
    }
}
