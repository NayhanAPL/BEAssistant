using Plugin.Messaging;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEAssistant.popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;

namespace BEAssistant.popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class popupEmail : PopupPage
    {
        public popupEmail()
        {
            InitializeComponent();
        }

        private async void ButtonOk_Clicked(object sender, EventArgs e)
        {
            string email = "";
            if (EntryEmail.Text != null) email = EntryEmail.Text;
            string title = "";
            if (EntryTitle.Text != null) title = EntryTitle.Text;
            var Email = CrossMessaging.Current.EmailMessenger;
            if (Email.CanSendEmail)
            {
                if (EntryTitle.Text != null && EntryEmail.Text != null)
                {
                    Email.SendEmail(Contactos.contactoSelected.Correo, title, email);
                    popups.PopupAlert.PopupLabelTitulo = "Correcto";
                    popups.PopupAlert.PopupLabelText = $"El mensaje se envió correctamente a {Contactos.contactoSelected.Nombre}.";
                    await Navigation.PushPopupAsync(new PopupAlert());
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}