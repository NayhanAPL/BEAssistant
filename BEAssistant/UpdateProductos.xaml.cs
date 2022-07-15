using BEAssistant.popups;
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

namespace BEAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateProductos : PopupPage
    {
        public UpdateProductos()
        {
            InitializeComponent();
            ByDefault();
        }

        private async void ByDefault()
        {
            InfoPro.IsVisible = false;
            InfoVen.IsVisible = false;
            if (TabbedProductos.botonSeleccionadoPro == 1)
            {
                InfoPro.IsVisible = true;
                InfoVen.IsVisible = false;
                Grid1parameter.IsVisible = false; 
                Grid2parameter.IsVisible = false; 
                Grid3parameter.IsVisible = false; 
                titlePageUpPro.Text = "Actualizar Producto";
                EntryNombreProducto.Text = TabbedProductos.page3ProSelected.Nombre;
                EntryTiempoSegPro.Text = (TabbedProductos.page3ProSelected.Tiempo % 60).ToString();
                EntryTiempoMinPro.Text = ((TabbedProductos.page3ProSelected.Tiempo / 60) % 60).ToString();
                EntryTiempoHorasPro.Text = (((TabbedProductos.page3ProSelected.Tiempo / 60) / 60) % 24).ToString();
                EntryTiempoDiaPro.Text = (((TabbedProductos.page3ProSelected.Tiempo / 60) / 60) / 24).ToString();
                EntryInversionPro.Text = TabbedProductos.page3ProSelected.Inversion.ToString();
                EntryDemandaPro.Text = TabbedProductos.page3ProSelected.Demanda.ToString();
                EntryDificultadPro.Text = TabbedProductos.page3ProSelected.Dificultad.ToString();
                EntryPrecioPro.Text = TabbedProductos.page3ProSelected.Precio.ToString();
                EntryProDescripcion.Text = TabbedProductos.page3ProSelected.Descripcion.ToString();
            }
            if (TabbedProductos.botonSeleccionadoPro == 2)
            {
                ButtonActualizarMP.IsVisible = false;
                InfoVen.IsVisible = true;
                InfoPro.IsVisible = false;
                var prod = await App.Database.GetIdProductos(TabbedProductos.page3VenSelected.IdPro);
                NombreVenta.Text = prod.Nombre;
                titlePageUpPro.Text = "Actualizar Venta";
                EntryVenta.Text = TabbedProductos.page3VenSelected.Precio.ToString();
                EntryUnidades.Text = TabbedProductos.page3VenSelected.Unidades.ToString();
                EntryVenDescripcion.Text = TabbedProductos.page3VenSelected.Descripcion.ToString();
            }          
        }
        private void EntryNombreProducto_Focused(object sender, FocusEventArgs e)
        {
            EntryNombreProducto.FontSize = 20;
        }
        private void EntryNombreProducto_Unfocused(object sender, FocusEventArgs e)
        {
            EntryNombreProducto.FontSize = 17;
        }
        private async void ButtonActualizarPro_Clicked(object sender, EventArgs e)
        {
            if (TabbedProductos.botonSeleccionadoPro == 1)
            {
                double dimensiones = 0;
                double dimen1 = 0;
                double dimen2 = 0;
                double dimen3 = 0;

                bool sepuedeDimenciones = false;
                bool sepuede = true;
                double inversion = 0;
                int demanda = 0;
                int dificultad = 0;
                double precio = 0;
                string nombre = "";
                string descripcion = "";
                int tiempo = 1;
                if (PickerTipoDimenciones.SelectedIndex == 0 && !string.IsNullOrEmpty(EntryDimencionesPorUnidad.Text))
                {
                    dimen1 = Convert.ToDouble(EntryDimencionesPorUnidad.Text);
                    dimensiones = dimen1; sepuedeDimenciones = true;
                }
                if (PickerTipoDimenciones.SelectedIndex == 1 && !string.IsNullOrEmpty(EntryDim2Longitud.Text) && !string.IsNullOrEmpty(EntryDim2Latitud.Text))
                {
                    dimen1 = Convert.ToDouble(EntryDim2Longitud.Text);
                    dimen2 = Convert.ToDouble(EntryDim2Latitud.Text);
                    dimensiones = dimen1 * dimen2; sepuedeDimenciones = true;
                }
                if (PickerTipoDimenciones.SelectedIndex == 2 && !string.IsNullOrEmpty(EntryDim3Longitud.Text) && !string.IsNullOrEmpty(EntryDim3Latitud.Text) && !string.IsNullOrEmpty(EntryDim3Altitud.Text))
                {
                    dimen1 = Convert.ToDouble(EntryDim3Longitud.Text);
                    dimen2 = Convert.ToDouble(EntryDim3Latitud.Text);
                    dimen3 = Convert.ToDouble(EntryDim3Altitud.Text);
                    dimensiones = dimen1 * dimen2 * dimen3; sepuedeDimenciones = true;
                }

                if (EntryInversionPro.Text != null && EntryInversionPro.Text != "" && !EntryInversionPro.Text.Contains("-"))
                    inversion = Convert.ToDouble(EntryInversionPro.Text);
                else { sepuede = false; EntryInversionPro.PlaceholderColor = Color.Red; }

                if (EntryDemandaPro.Text != null && EntryDemandaPro.Text != "" && !EntryDemandaPro.Text.Contains("-") && !EntryDificultadPro.Text.Contains("."))
                    demanda = Convert.ToInt32(EntryDemandaPro.Text);
                else { sepuede = false; EntryDemandaPro.PlaceholderColor = Color.Red; }

                if (EntryDificultadPro.Text != null && EntryDificultadPro.Text != "" && !EntryDificultadPro.Text.Contains("-") && !EntryDificultadPro.Text.Contains(".") && Convert.ToInt32(EntryDificultadPro.Text) <= 10)
                    dificultad = Convert.ToInt32(EntryDificultadPro.Text);
                else { sepuede = false; EntryDificultadPro.PlaceholderColor = Color.Red; }

                if (EntryPrecioPro.Text != null && EntryPrecioPro.Text != "" && !EntryPrecioPro.Text.Contains("-"))
                    precio = Convert.ToInt32(EntryPrecioPro.Text);
                else { sepuede = false; EntryPrecioPro.PlaceholderColor = Color.Red; }

                if (EntryNombreProducto.Text != null && EntryNombreProducto.Text != "" && EntryNombreProducto.Text != " " )
                    nombre = EntryNombreProducto.Text;
                else { sepuede = false; EntryNombreProducto.PlaceholderColor = Color.Red; }

                if (!string.IsNullOrEmpty(EntryProDescripcion.Text))
                    descripcion = EntryProDescripcion.Text;
                else descripcion = TabbedProductos.page3ProSelected.Descripcion;

                bool date = false;
                if (!string.IsNullOrEmpty(EntryTiempoDiaPro.Text) && EntryTiempoDiaPro.Text != "0" && !EntryTiempoDiaPro.Text.Contains("-"))
                { tiempo += Convert.ToInt32(EntryTiempoDiaPro.Text) * 60 * 60 * 24; date = true; }
                if (!string.IsNullOrEmpty(EntryTiempoHorasPro.Text) && EntryTiempoHorasPro.Text != "0" && !EntryTiempoHorasPro.Text.Contains("-"))
                { tiempo += Convert.ToInt32(EntryTiempoHorasPro.Text) * 60 * 60; date = true; }
                if (!string.IsNullOrEmpty(EntryTiempoMinPro.Text) && EntryTiempoMinPro.Text != "0" && !EntryTiempoMinPro.Text.Contains("-"))
                { tiempo += Convert.ToInt32(EntryTiempoMinPro.Text) * 60; date = true; }
                if (!string.IsNullOrEmpty(EntryTiempoSegPro.Text) && EntryTiempoSegPro.Text != "0" && !EntryTiempoSegPro.Text.Contains("-"))
                { tiempo += Convert.ToInt32(EntryTiempoSegPro.Text); date = true; }

                if (!date)
                {
                    EntryTiempoDiaPro.PlaceholderColor = Color.Red;
                    EntryTiempoHorasPro.PlaceholderColor = Color.Red;
                    EntryTiempoMinPro.PlaceholderColor = Color.Red;
                    EntryTiempoSegPro.PlaceholderColor = Color.Red;
                    sepuede = false;
                }
                if (!sepuedeDimenciones)
                    dimensiones = TabbedProductos.page3ProSelected.Dimensiones;
                if (sepuede)
                {
                    Product p = new Product
                    {
                        Id = TabbedProductos.page3ProSelected.Id,
                        Nombre = nombre,
                        Descripcion = descripcion,
                        Demanda = demanda,
                        Denominacion = TabbedProductos.page3ProSelected.Denominacion,
                        Dificultad = dificultad,
                        Dimensiones = dimensiones,
                        Inversion = inversion,
                        Precio = precio,
                        Tiempo = tiempo
                    }; await App.Database.SaveUpProductos(p);
                    ByDefault();
                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    PopupAlert.PopupLabelTitulo = "ERROR";
                    PopupAlert.PopupLabelText = "Asegúrese de no dejar ningún parámetro vacio";
                    await Navigation.PushPopupAsync(new PopupAlert());
                }
                

            }
            if (TabbedProductos.botonSeleccionadoPro == 2)
            {
                double precio = 0;
                double unidades = 0;
                string descripcion = "";
                if (!string.IsNullOrEmpty(EntryVenta.Text))
                { precio = Convert.ToDouble(EntryVenta.Text); }
                else precio = Convert.ToDouble(TabbedProductos.page3VenSelected.Precio);
                if (!string.IsNullOrEmpty(EntryUnidades.Text))
                { unidades = Convert.ToDouble(EntryUnidades.Text); }
                else unidades = Convert.ToDouble(TabbedProductos.page3VenSelected.Unidades);
                if (!string.IsNullOrEmpty(EntryVenDescripcion.Text))
                { descripcion = EntryVenDescripcion.Text; }
                else descripcion = TabbedProductos.page3VenSelected.Descripcion.ToString();

                Venta v = new Venta
                {
                    Id = TabbedProductos.page3VenSelected.Id,
                    Descripcion = descripcion,
                    Unidades = unidades,
                    Precio = precio,
                    Fecha = TabbedProductos.page3VenSelected.Fecha,
                    IdPro = TabbedProductos.page3VenSelected.IdPro
                }; await App.Database.SaveUpVenta(v);
                ByDefault();
                await PopupNavigation.Instance.PopAsync(true);
            }           
        }
        private void PickerTipoDimenciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerTipoDimenciones.SelectedIndex == 0) { Grid1parameter.IsVisible = true; Grid2parameter.IsVisible = false; Grid3parameter.IsVisible = false; }
            if (PickerTipoDimenciones.SelectedIndex == 1) { Grid1parameter.IsVisible = false; Grid2parameter.IsVisible = true; Grid3parameter.IsVisible = false; }
            if (PickerTipoDimenciones.SelectedIndex == 2) { Grid1parameter.IsVisible = false; Grid2parameter.IsVisible = false; Grid3parameter.IsVisible = true; }
        }

        private async void ButtonActualizarMP_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new UpMateriaPrima());
        }

        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}