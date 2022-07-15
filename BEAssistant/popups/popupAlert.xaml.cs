using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BEAssistant.popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAlert : PopupPage
    {
        public PopupAlert()
        {
            InitializeComponent();
            ByDefault();
        }

        public static string PopupLabelText = "";
        public static string PopupLabelTitulo = "";
        private void ByDefault()
        {
            
            labelTexto.Text = PopupLabelText;
            labelTitulo.Text = PopupLabelTitulo;
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}