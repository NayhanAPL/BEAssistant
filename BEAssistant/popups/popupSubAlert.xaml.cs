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
    public partial class popupSubAlert : PopupPage
    {
        public popupSubAlert()
        {
            InitializeComponent();
            ByDefault();
        }
        public static string TextSubAlert = "";
        private async void ByDefault()
        {
            textSubAlert.Text = TextSubAlert;
            await Task.Delay(30);
            if (!salio)
            {
                await PopupNavigation.Instance.PopAsync(true);
            }
        }
        public static bool salio = false;

        private async void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            salio = true;
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}