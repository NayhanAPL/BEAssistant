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
    public partial class CrearContacto : PopupPage
    {
        public CrearContacto()
        {
            InitializeComponent();
        }

        public static string tipo = "";
        private async void ButtonCrear_Clicked(object sender, EventArgs e)
        {
            var nombre = "";
            var numero = "";
            var correo = "";
            var telefono = "";
            var direccion = "";
            var descripcion = "";
            if (!string.IsNullOrEmpty(EntryNombre.Text)) nombre = EntryNombre.Text;
            if (!string.IsNullOrEmpty(EntryNumero.Text)) numero = EntryNumero.Text;
            if (!string.IsNullOrEmpty(EntryCorreo.Text)) correo = EntryCorreo.Text;
            if (!string.IsNullOrEmpty(EntryTelefono.Text)) telefono = EntryTelefono.Text;
            if (!string.IsNullOrEmpty(EntryDireccion.Text)) direccion = EntryDireccion.Text;
            if (!string.IsNullOrEmpty(EntryDescripcion.Text)) descripcion = EntryDescripcion.Text;
            await App.Database.SaveContacts(new Contacto()
            {
                Categoria = tipo,
                Correo = correo,
                Nombre = nombre,
                Numero = numero,
                Descripcion = descripcion,
                Direccion = direccion,
                Telefono = telefono
            }) ;
            MessagingCenter.Send<CrearContacto, string>(this, "CrearContacto", "Contacto");
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ButtonCancelar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}