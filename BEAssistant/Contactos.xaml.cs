using BEAssistant.popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BEAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contactos : TabbedPage
    {
        public Contactos()
        {
            InitializeComponent();
            LlenarListasContactos();
        }

        private async void LlenarListasContactos()
        {
            var contactos = await App.Database.GetContacts();

            var Clientes = contactos.FindAll(x => x.Categoria == "Clientes");
            var Proveedores = contactos.FindAll(x => x.Categoria == "Proveedores");
            var Asociados = contactos.FindAll(x => x.Categoria == "Asociados");
            var Trabajadores = contactos.FindAll(x => x.Categoria == "Trabajadores");


            listContactosClientes.ItemsSource = Clientes.OrderBy(x => x.Nombre);
            listContactosProveedores.ItemsSource = Proveedores.OrderBy(x => x.Nombre);
            listContactosAsociados.ItemsSource = Asociados.OrderBy(x => x.Nombre);
            listContactosTrabajadores.ItemsSource = Trabajadores.OrderBy(x => x.Nombre);

            if (Clientes.Count == 0) labelVacioC.IsVisible = true;
            if (Proveedores.Count == 0) labelVacioP.IsVisible = true;
            if (Asociados.Count == 0) labelVacioA.IsVisible = true;
            if (Trabajadores.Count == 0) labelVacioT.IsVisible = true;
        }

        
        public static Contacto contactoSelected = new Contacto();
        private async void listContactosClientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            contactoSelected = (Contacto)e.SelectedItem;
            await Navigation.PushPopupAsync(new OperacionesContacto());
            MessagingCenter.Subscribe<OperacionesContacto, string>(this, "BorrarContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }
        private async void listContactosProveedores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            contactoSelected = (Contacto)e.SelectedItem;
            await Navigation.PushPopupAsync(new OperacionesContacto());
            MessagingCenter.Subscribe<OperacionesContacto, string>(this, "BorrarContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }
        private async void listContactosAsociados_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            contactoSelected = (Contacto)e.SelectedItem;
            await Navigation.PushPopupAsync(new OperacionesContacto());
            MessagingCenter.Subscribe<OperacionesContacto, string>(this, "BorrarContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }
        private async void listContactosTrabajadores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            contactoSelected = (Contacto)e.SelectedItem;
            await Navigation.PushPopupAsync(new OperacionesContacto());
            MessagingCenter.Subscribe<OperacionesContacto, string>(this, "BorrarContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }

        private async void AgregarContactoProveedores_Clicked(object sender, EventArgs e)
        {
            CrearContacto.tipo = "Proveedores";
            await Navigation.PushPopupAsync(new CrearContacto());
            MessagingCenter.Subscribe<CrearContacto, string>(this, "CrearContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }
        private async void AgregarContactoAsociados_Clicked(object sender, EventArgs e)
        {
            CrearContacto.tipo = "Asociados";
            await Navigation.PushPopupAsync(new CrearContacto());
            MessagingCenter.Subscribe<CrearContacto, string>(this, "CrearContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }
        private async void AgregarContactoTrabajadores_Clicked(object sender, EventArgs e)
        {
            CrearContacto.tipo = "Trabajadores";
            await Navigation.PushPopupAsync(new CrearContacto());
            MessagingCenter.Subscribe<CrearContacto, string>(this, "CrearContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }
        private async void AgregarContactoCliente_Clicked(object sender, EventArgs e)
        {
            CrearContacto.tipo = "Clientes";
            await Navigation.PushPopupAsync(new CrearContacto());
            MessagingCenter.Subscribe<CrearContacto, string>(this, "CrearContacto", async (s, arg) =>
            {
                LlenarListasContactos();
            });
        }

        private void listContactosClientes_Scrolled(object sender, ScrolledEventArgs e)
        {
            //AgregarContactoCliente.IsVisible = false;
        }
        private void listContactosProveedores_Scrolled(object sender, ScrolledEventArgs e)
        {
            //AgregarContactoProveedores.IsVisible = false;
        }
        private void listContactosAsociados_Scrolled(object sender, ScrolledEventArgs e)
        {
            //AgregarContactoAsociados.IsVisible = false;
        }
        private void listContactosTrabajadores_Scrolled(object sender, ScrolledEventArgs e)
        {
            //AgregarContactoTrabajadores.IsVisible = false;
        }
    }
}