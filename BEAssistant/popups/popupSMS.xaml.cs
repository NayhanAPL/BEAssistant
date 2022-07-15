using Plugin.Messaging;
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
    public partial class popupSMS : PopupPage
    {
        public popupSMS()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            string text = "";
            if (EntryMensaje.Text != null) text = EntryMensaje.Text;
            var sms = CrossMessaging.Current.SmsMessenger;
            if (sms.CanSendSms)
            { sms.SendSms(Contactos.contactoSelected.Numero, text); }
            popups.PopupAlert.PopupLabelTitulo = "Correcto";
            popups.PopupAlert.PopupLabelText = $"El mensaje se envió correctamente a {Contactos.contactoSelected.Nombre}.";
            await Navigation.PushPopupAsync(new PopupAlert());
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}