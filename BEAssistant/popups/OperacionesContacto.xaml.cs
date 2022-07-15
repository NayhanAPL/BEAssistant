﻿using Plugin.Messaging;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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
    public partial class OperacionesContacto : PopupPage
    {
        public OperacionesContacto()
        {
            InitializeComponent();
            Info();
        }

        private void Info()
        {
            LabelDireccion.Text = Contactos.contactoSelected.Direccion;
            LabelDescripcion.Text = Contactos.contactoSelected.Descripcion;
        }

        private async void buttonLlamar_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Contactos.contactoSelected.Numero))
            {
                var call = CrossMessaging.Current.PhoneDialer;
                if (call.CanMakePhoneCall)
                { call.MakePhoneCall(Contactos.contactoSelected.Numero); }
            }
            else
            {
                popups.PopupAlert.PopupLabelTitulo = "Ocurrió un Error";
                popups.PopupAlert.PopupLabelText = "El contacto no tiene número asignado.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
        }
        private async void buttonLlamarTelefono_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Contactos.contactoSelected.Telefono))
            {
                var call = CrossMessaging.Current.PhoneDialer;
                if (call.CanMakePhoneCall)
                { call.MakePhoneCall(Contactos.contactoSelected.Telefono); }
            }
            else
            {
                popups.PopupAlert.PopupLabelTitulo = "Ocurrió un Error";
                popups.PopupAlert.PopupLabelText = "El contacto no tiene número asignado.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
        }
        private async void buttonMensaje_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Contactos.contactoSelected.Numero))
            {
                await Navigation.PushPopupAsync(new popupSMS());
            }
            else
            {
                popups.PopupAlert.PopupLabelTitulo = "Ocurrió un Error";
                popups.PopupAlert.PopupLabelText = "El contacto no tiene número asignado.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
        }
        private async void buttonCorreo_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Contactos.contactoSelected.Numero))
            {
                await Navigation.PushPopupAsync(new popupEmail());
            }
            else
            {
                popups.PopupAlert.PopupLabelTitulo = "Ocurrió un Error";
                popups.PopupAlert.PopupLabelText = "El contacto no tiene email asignado.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
        }
        private async void buttonResumen_Clicked(object sender, EventArgs e)
        {

        }
       
    }
}