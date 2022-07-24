using BEAssistant.popups;
using Rg.Plugins.Popup.Extensions;
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
    public partial class TabbedProductos : TabbedPage
    {
        public TabbedProductos()
        {
            InitializeComponent();
            ByDefault();
        }


        // ------------------------------------------page1------------------------------------------

        private async void ByDefault()
        {
            ViewNombresPro.IsVisible = false;
            rellenoListView.IsVisible = true;
            ViewPage3Ven.IsVisible = false;
            ViewPage3Pro.IsVisible = false;
            Page3producto.FontSize = 21;
            Page3producto.TextColor = Color.Black;
            Page3venta.FontSize = 21;
            Page3venta.TextColor = Color.Black;
            selectedAny = false;

            ViewNombresPro.ItemsSource = null;
            DatosReportePro.IsVisible = false;
            EntryCostoProducto.Text = "";
            EntryUnidadesproducto.Text = "";
            EntryCostoProducto.PlaceholderColor = Color.Green;
            EntryUnidadesproducto.PlaceholderColor = Color.Green;
            selectDenominacionPro.TitleColor = Color.Green;

            EntryDenomProducto.IsVisible = false;
            PickerDenominacion.IsVisible = false;
            ButtonDenomEscrbir.IsVisible = true;
            ButtonDenomSeleccionar.IsVisible = true;
            EntryDenomProducto.PlaceholderColor = Color.Green;
            EntryInversionPro.PlaceholderColor = Color.Green;
            EntryDemandaPro.PlaceholderColor = Color.Green;
            EntryDificultadPro.PlaceholderColor = Color.Green;
            EntryPrecioPro.PlaceholderColor = Color.Green;
            EntryNombreProducto.PlaceholderColor = Color.Green;
            EntryTiempoDiaPro.PlaceholderColor = Color.Green;
            EntryTiempoHorasPro.PlaceholderColor = Color.Green;
            EntryTiempoMinPro.PlaceholderColor = Color.Green;
            EntryTiempoSegPro.PlaceholderColor = Color.Green;
            Grid1parameter.IsVisible = false;
            Grid2parameter.IsVisible = false;
            Grid3parameter.IsVisible = false;
            EntryDenomProducto.Text = "";
            EntryInversionPro.Text = "";
            EntryDemandaPro.Text = "";
            EntryDificultadPro.Text = "";
            EntryPrecioPro.Text = "";
            EntryNombreProducto.Text = "";
            EntryTiempoDiaPro.Text = "";
            EntryTiempoHorasPro.Text = "";
            EntryTiempoMinPro.Text = "";
            EntryTiempoSegPro.Text = "";
            List<Product> product = await App.Database.GetProductos();
            if (product.Count != 0)
            {
                foreach (Product item in product)
                {
                    producNombres.Add(item.Nombre);
                    if (!denomExistentes.Contains(item.Denominacion))
                    {
                        denomExistentes.Add(item.Denominacion);
                    }
                }
            }

            foreach (var item in denomExistentes)
            {
                if (item != null && !selectDenominacionPro.Items.Contains(item))
                {
                    selectDenominacionPro.Items.Add(item);
                }
            }

            ByDefault4();
        }
        private async void selectDenominacionPro_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntryCostoProducto.Text = "";
            EntryUnidadesproducto.Text = "";
            DatosReportePro.IsVisible = false;
            var productos = await App.Database.GetByDenominacionProductos(selectDenominacionPro.SelectedItem.ToString());
            if (productos != null)
            {
                ViewNombresPro.IsVisible = true;
                rellenoListView.IsVisible = false;
                productos.ForEach(x => TabbedInversiones.ReducirText(x.Descripcion));
                var productosOrder = productos.OrderByDescending(x => x.Id);
                ViewNombresPro.ItemsSource = productosOrder;
            }
        }
        private async void ViewNombresPro_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DatosReportePro.IsVisible = true;
            var obj = (Product)e.SelectedItem;
            var productos = await App.Database.GetByNombreProductos(obj.Nombre);
            if (productos.Count > 0)
            {
                product = productos[0];
                Venta venta = new Venta();
                var ventas = await App.Database.GetByIdProVenta(product.Id);
                if(ventas.Count > 0)
                {
                    estandarPrecioVenta = 0;
                    foreach (var item in ventas)
                    {estandarPrecioVenta += item.Precio;}
                    estandarPrecioVenta /= ventas.Count;
                    venta = ventas[ventas.Count - 1];
                    EntryCostoProducto.Text = venta.Precio.ToString();
                    EntryUnidadesproducto.Text = venta.Unidades.ToString();
                }
                else
                {
                    estandarPrecioVenta = obj.Precio;
                    EntryCostoProducto.Text = obj.Precio.ToString();
                    EntryUnidadesproducto.Text = "1";
                }
            }
        }
        public static Product product = new Product();
        public static string descripcion = "";
        public static double estandarPrecioVenta = 0;
        private async void ButtonReportarPro_Clicked(object sender, EventArgs e)
        {
            bool sepuede = true;
            if(selectDenominacionPro.SelectedIndex == -1)
            {
                selectDenominacionPro.TitleColor = Color.Red;
                sepuede = false;
                popupSubAlert.TextSubAlert = "ERROR: Debe seleccionar una denominación y llenar los datos que se le piden";
            }
            if (product == null)
            {
                selectDenominacionPro.TitleColor = Color.Red;
                popupSubAlert.TextSubAlert = "ERROR: Debe seleccionar un elemento de la lista y llenar los datos que se le piden";
                sepuede = false;
            }
            if (string.IsNullOrEmpty(EntryCostoProducto.Text))
            {
                EntryCostoProducto.PlaceholderColor = Color.Red;
                popupSubAlert.TextSubAlert = "ERROR: Debe declarar el precio de la venta del producto.";
                sepuede = false;
            }
            if (string.IsNullOrEmpty(EntryUnidadesproducto.Text))
            {
                EntryUnidadesproducto.PlaceholderColor = Color.Red;
                popupSubAlert.TextSubAlert = "ERROR: Debe declarar la cantidad de unidades que fueron vendidas.";
                sepuede = false;
            }
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            if (sepuede)
            {
                await App.Database.SaveVenta(new Venta{ 
                IdPro = product.Id,
                Fecha = TimeActual,
                Descripcion = descripcion,
                Precio = Convert.ToDouble(EntryCostoProducto.Text),
                Unidades = Convert.ToDouble(EntryUnidadesproducto.Text),
                });
                var inventario = await App.Database.GetInventario();
                var materiaP = await App.Database.GetByIdProMateriaPrima(product.Id);
                foreach (var item in materiaP)
                {
                    var invA = await App.Database.GetByNameInvAcumulativa(item.Nombre);
                    if (invA.Count > 0)
                    {
                        var stock = await App.Database.GetIdInvAStock(invA[0].Id);
                        Stocker up = new Stocker()
                        {
                            Id = stock[0].Id,
                            Duracion = stock[0].Duracion,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual - (item.CantidadPorProducto * Convert.ToDouble(EntryUnidadesproducto.Text)),
                            Categoria = stock[0].Categoria,
                            UnidadesEstimadas = stock[0].UnidadesEstimadas
                        };
                        await App.Database.SaveUpStock(up);
                        
                    }
                    var invC = await App.Database.GetByNameInvConstante(item.Nombre);
                    if (invC.Count > 0)
                    {
                        var stock = await App.Database.GetIdInvCStock(invC[0].Id);
                        Stocker up = new Stocker()
                        {
                            Id = stock[0].Id,
                            Duracion = stock[0].Duracion,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual - (item.CantidadPorProducto * Convert.ToDouble(EntryUnidadesproducto.Text)),
                            Categoria = stock[0].Categoria,
                            UnidadesEstimadas = stock[0].UnidadesEstimadas
                        };
                        await App.Database.SaveUpStock(up);
                    }
                    //inventario
                    var inv = inventario.Find(x => x.Materias == item.Nombre && x.Fecha.DayOfYear == TimeActual.DayOfYear && x.Fecha.Year == TimeActual.Year);
                    if (inv != null)
                    {
                        inv.ConsUtil += Convert.ToInt32(item.CantidadPorProducto * Convert.ToInt32(EntryUnidadesproducto.Text));
                        await App.Database.SaveUpInventario(inv);
                    }
                    else
                    {
                        await App.Database.SaveInventario(new Inventario()
                        {
                            Materias = item.Nombre,
                            ConsUtil = Convert.ToDouble(item.CantidadPorProducto * Convert.ToInt32(EntryUnidadesproducto.Text)),
                            ConsCaduco = 0,
                            ConsExedente = 0,
                            Fecha = TimeActual
                        });
                    }
                }
                popupSubAlert.TextSubAlert = "REPORTADO CON EXITO: La venta quedó regsitrada existosament, puede ver los cambios en la página de Historial y Estadisticas.";
                await Navigation.PushPopupAsync(new popupSubAlert());
                ByDefault();
            }
            else
            {
                await Navigation.PushPopupAsync(new popupSubAlert());
            }
        }
        private async void ButtonDescripcionVeenta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new DescripcionVenta());
        }
        private void EntryCostoProducto_TextChanged(object sender, TextChangedEventArgs e) 
        {
            
            string text = EntryCostoProducto.Text;
            if (!string.IsNullOrEmpty(text))
            {                
                double num = Convert.ToDouble(text);
                string res = ColorRange(estandarPrecioVenta, (estandarPrecioVenta*20)/100, num);
                EntryCostoProducto.TextColor = Color.FromHex(res);
                if (estandarPrecioVenta > num) 
                { 
                    LavelSugerPrecioVenta.Text = "+" + Convert.ToInt32(estandarPrecioVenta - num).ToString(); 
                    LavelSugerPrecioVenta.TextColor = Color.FromHex("6f6"); 
                }
                if (estandarPrecioVenta < num) 
                {
                    LavelSugerPrecioVenta.Text = "-" + Convert.ToInt32(num - estandarPrecioVenta).ToString();
                    LavelSugerPrecioVenta.TextColor = Color.FromHex("f66");
                }
                if (estandarPrecioVenta == num) 
                {
                    LavelSugerPrecioVenta.Text = "";
                    LavelSugerPrecioVenta.TextColor = Color.White;
                }
            }
            
        }
       

        // ------------------------------------------page2------------------------------------------
        

        List<string> denomExistentes = new List<string>();
        public static List<string> producNombres = new List<string>();
        private void EntryDenomProducto_Focused(object sender, FocusEventArgs e)
        {
            EntryDenomProducto.FontSize = 20;
        }
        private void EntryDenomProducto_Unfocused(object sender, FocusEventArgs e)
        {
            EntryDenomProducto.FontSize = 17;
        }
        public bool denomPickerEntry = false;
        private void ButtonDenomEscrbir_Clicked(object sender, EventArgs e)
        {
            EntryDenomProducto.IsVisible = true;
            PickerDenominacion.IsVisible = false;
            ButtonDenomEscrbir.IsVisible = false;
            ButtonDenomSeleccionar.IsVisible = true;
            denomPickerEntry = false;
        }
        private void ButtonDenomSeleccionar_Clicked(object sender, EventArgs e)
        {
            EntryDenomProducto.IsVisible = false;
            PickerDenominacion.IsVisible = true;
            ButtonDenomEscrbir.IsVisible = true;
            ButtonDenomSeleccionar.IsVisible = false;
            denomPickerEntry = true;
            PickerDenominacion.Items.Clear();
            foreach (var item in denomExistentes)
            {
                if(item != null && !PickerDenominacion.Items.Contains(item))
                   PickerDenominacion.Items.Add(item);
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
        private async void ButtonCrearProducto_Clicked(object sender, EventArgs e)
        {
            double dimensiones = 0;
            double dimen1 = 0;
            double dimen2 = 0;
            double dimen3 = 0;

            bool sepuedeDimenciones = false;
            bool sepuede = true;
            string denominacion = "";
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

            if (!denomPickerEntry)
            {
                if (EntryDenomProducto.Text == "" || EntryDenomProducto.Text == null) 
                { 
                    sepuede = false; EntryDenomProducto.PlaceholderColor = Color.Red; 
                } 
                else denominacion = EntryDenomProducto.Text; 
            }
            else denominacion = PickerDenominacion.SelectedItem.ToString();

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

            if (EntryNombreProducto.Text != null && EntryNombreProducto.Text != "" && EntryNombreProducto.Text != " " && !producNombres.Contains(EntryNombreProducto.Text))
                nombre = EntryNombreProducto.Text;
            else { sepuede = false; EntryNombreProducto.PlaceholderColor = Color.Red; }

            if (DescripcionProducto.ProDescripcion != null)
                descripcion = DescripcionProducto.ProDescripcion;

            bool date = false;
            if (!string.IsNullOrEmpty(EntryTiempoDiaPro.Text) && EntryTiempoDiaPro.Text != "0" && !EntryTiempoDiaPro.Text.Contains("-"))
            { tiempo += Convert.ToInt32(EntryTiempoDiaPro.Text) * 60 * 60 * 24; date = true;}
            if (!string.IsNullOrEmpty(EntryTiempoHorasPro.Text) && EntryTiempoDiaPro.Text != "0" && !EntryTiempoHorasPro.Text.Contains("-"))
            { tiempo += Convert.ToInt32(EntryTiempoHorasPro.Text) * 60 * 60; date = true; }
            if (!string.IsNullOrEmpty(EntryTiempoMinPro.Text) && EntryTiempoDiaPro.Text != "0" && !EntryTiempoMinPro.Text.Contains("-"))
            { tiempo += Convert.ToInt32(EntryTiempoMinPro.Text) * 60; date = true; }
            if (!string.IsNullOrEmpty(EntryTiempoSegPro.Text) && EntryTiempoDiaPro.Text != "0" && !EntryTiempoSegPro.Text.Contains("-"))
            { tiempo += Convert.ToInt32(EntryTiempoSegPro.Text); date = true; }
             
            if (!date)
            {
                EntryTiempoDiaPro.PlaceholderColor = Color.Red;
                EntryTiempoHorasPro.PlaceholderColor = Color.Red;
                EntryTiempoMinPro.PlaceholderColor = Color.Red;
                EntryTiempoSegPro.PlaceholderColor = Color.Red;
                sepuede = false;
            }

            if (sepuede)
            {
                await App.Database.SaveProductos(new Product
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Demanda = demanda,
                    Denominacion = denominacion,
                    Dificultad = dificultad,
                    Dimensiones = dimensiones,
                    Inversion = inversion,
                    Precio = precio,
                    Tiempo = tiempo
                });
                if(MateriaPrimaProductos.matPri.Count > 0)
                {
                        
                    Product ultimo = new Product();
                    var listLastId = await App.Database.GetLastItemProductos();
                    if (listLastId.Count != 0) ultimo = listLastId[0];
                    if (ultimo != null)
                    {
                        // crear materia prima
                        foreach (var item in MateriaPrimaProductos.matPri)
                        {
                            double precioUnidad = 0;

                            var cons = await App.Database.GetByNameInvConstante(item.Nombre);
                            var acum = await App.Database.GetByNameInvAcumulativa(item.Nombre);
                            if(cons.Count != 0)
                            {
                                if (cons[0].Tipo == TiposConstante.MateriaPrima)
                                {
                                    var registro = await App.Database.GetIdInvRegConstante(cons[0].Id);
                                    if (registro.Count > 0) precioUnidad = registro[registro.Count - 1].Costo;
                                }                                
                            }
                            else if (acum.Count != 0)
                            {
                                if (acum[0].Tipo == TiposAcumulativa.MateriaPrima)
                                {
                                    var registro = await App.Database.GetIdInvRegAcumulativa(acum[0].Id);
                                    if (registro.Count > 0) precioUnidad = registro[registro.Count - 1].Costo;
                                }
                            }
                            await App.Database.SaveMateriaPrima(new MateriasPrimas
                            {
                                IdProducto = ultimo.Id,
                                Nombre = item.Nombre,
                                CantidadPorProducto = item.Cantidad,
                                PrecioUnidad = precioUnidad
                            });
                        }
                        // crear Dimenciones
                        if(sepuedeDimenciones)
                        {
                            await App.Database.SaveDimensiones(new Dimention
                            {
                                IdProducto = ultimo.Id,
                                Dimen1 = dimen1,
                                Dimen2 = dimen2,
                                Dimen3 = dimen3,
                                Denominacion = denominacion,
                            });
                        }
                    }

                }

                PopupAlert.PopupLabelTitulo = "Elemento registrado";
                PopupAlert.PopupLabelText = "Ahora puede reportar ventas con las caracteristicas que acaba de crear, para ello vaya a la página ( REPORTAR VENTAS )";
                await Navigation.PushPopupAsync(new PopupAlert());
                ByDefault();
            }
            else {
                popupSubAlert.TextSubAlert = "ERROR: Asegúrese de haber llenado todos los datos correctamentes";
                await Navigation.PushPopupAsync(new popupSubAlert());
            }
            
        }
        private async void ButtonDescripcionProducto_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new DescripcionProducto());
        }
        private void ButtonAddMateriasPrimas_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MateriaPrimaProductos()); 
        }
        private void PickerTipoDimenciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerTipoDimenciones.SelectedIndex == 0) { Grid1parameter.IsVisible = true; Grid2parameter.IsVisible = false; Grid3parameter.IsVisible = false; }
            if (PickerTipoDimenciones.SelectedIndex == 1) { Grid1parameter.IsVisible = false; Grid2parameter.IsVisible = true; Grid3parameter.IsVisible = false; }
            if (PickerTipoDimenciones.SelectedIndex == 2) { Grid1parameter.IsVisible = false; Grid2parameter.IsVisible = false; Grid3parameter.IsVisible = true; }            
        }
        public static Product ultimoPorDenom = new Product(); 
        private async void PickerDenominacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listUltimoPorDenom = await App.Database.GetLastItemByDenomProductos(PickerDenominacion.SelectedItem.ToString());
            ultimoPorDenom = listUltimoPorDenom[0];
            var listDimenciones = await App.Database.GetByIdProDimensiones(ultimoPorDenom.Id);
            if(listDimenciones.Count != 0)
            {
                var dimenciones = listDimenciones[0];
                EntryDimencionesPorUnidad.Text = dimenciones.Dimen1.ToString();
                EntryDim2Latitud.Text = dimenciones.Dimen1.ToString();
                EntryDim2Longitud.Text = dimenciones.Dimen2.ToString();
                EntryDim3Latitud.Text = dimenciones.Dimen1.ToString();
                EntryDim3Longitud.Text = dimenciones.Dimen2.ToString();
                EntryDim3Altitud.Text = dimenciones.Dimen3.ToString();
            }
           
            EntryInversionPro.Text = ultimoPorDenom.Inversion.ToString();
            EntryDemandaPro.Text = ultimoPorDenom.Demanda.ToString();
            EntryDificultadPro.Text = ultimoPorDenom.Dificultad.ToString();
            EntryPrecioPro.Text = ultimoPorDenom.Precio.ToString();
            int time = ultimoPorDenom.Tiempo;
            if(time > 0)
            {
                EntryTiempoSegPro.Text = Convert.ToString(time % 60);
                time = time / 60;
                if(time > 0)
                   EntryTiempoMinPro.Text = Convert.ToString(time % 60);
                time = time / 60;
                if (time > 0)
                    EntryTiempoHorasPro.Text = Convert.ToString(time % 24);
                time = time / 24;
                if (time > 0)
                    EntryTiempoDiaPro.Text = time.ToString();
            }  
        }

        // ------------------------------------------page3------------------------------------------

        public static int botonSeleccionadoPro = 0;
        private void Page3venta_Clicked(object sender, EventArgs e)
        {
            botonSeleccionadoPro = 2;
            Page3venta.FontSize = 22;
            Page3venta.TextColor = Color.Green;
            Page3producto.FontSize = 21;
            Page3producto.TextColor = Color.Black;
            ViewPage3Ven.IsVisible = true;
            ViewPage3Pro.IsVisible = false;
            MostrarTven();
        }
        private async void MostrarTven()
        {
            var viewVen = await App.Database.GetVenta();
            if (viewVen != null)
            {
                viewVen.ForEach(x => TabbedInversiones.ReducirText(x.Descripcion));
                var viewven = viewVen.OrderByDescending(x => x.Id);
                ViewPage3Ven.ItemsSource = viewven;
            }
        }
        private async void Page3producto_Clicked(object sender, EventArgs e)
        {
            popupSubAlert.TextSubAlert = "ADVERTENCIA: Si cambia algún parámetro de un producto cambiará la información de todos los registros de ese elemento. Se recomienda crear un nuevo elemento.";
            await Navigation.PushPopupAsync(new popupSubAlert());
            botonSeleccionadoPro = 1;
            Page3producto.FontSize = 22;
            Page3producto.TextColor = Color.Green;
            Page3venta.FontSize = 21;
            Page3venta.TextColor = Color.Black;
            ViewPage3Ven.IsVisible = false;
            ViewPage3Pro.IsVisible = true;
            MostrarTpro();
        }
        private async void MostrarTpro()
        {
            var viewPro = await App.Database.GetProductos();
            if (viewPro != null)
            {
                string denom = "";
                var viewpro = viewPro.OrderBy(x => x.Denominacion);
                int cont = 0;
                foreach (var item in viewpro)
                {
                    viewPro[cont] = item; 
                    viewPro[cont].Descripcion = TabbedInversiones.ReducirText(viewPro[cont].Descripcion);
                    if (denom == viewPro[cont].Denominacion) viewPro[cont].Denominacion = "";
                    else denom = viewPro[cont].Denominacion;
                    cont++;
                }
                ViewPage3Pro.ItemsSource = viewPro;
            }
        }
        public static bool selectedAny = false;
        private async void ButtonEliminarPro_Clicked(object sender, EventArgs e)
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();

            if (botonSeleccionadoPro != 0 && selectedAny)
            {
                if (botonSeleccionadoPro == 1)
                {
                    bool sepuede = false;
                    var ventas = await App.Database.GetByIdProVenta(page3ProSelected.Id);
                    if (ventas.Count == 0) sepuede = true;
                    else
                    {
                        if (ventas[0].Fecha.DayOfYear == TimeActual.DayOfYear && ventas[0].Fecha.Year == TimeActual.Year)
                        {
                            ventas.ForEach(async x => await App.Database.DeleteVenta(x));
                            var matPri = await App.Database.GetByIdProMateriaPrima(page3ProSelected.Id);
                            matPri.ForEach(async x => await App.Database.DeleteMateriaPrima(x));
                            var dimen = await App.Database.GetByIdProDimensiones(page3ProSelected.Id);
                            dimen.ForEach(async x => await App.Database.DeleteDimensiones(x));
                            sepuede = true;
                        }
                        else
                        {
                            popupSubAlert.TextSubAlert = "ERROR: Este producto tiene ventas guardadas, para eliminar un tipo de producto no puede tener ventas de más de un día de realizada.";
                            await Navigation.PushPopupAsync(new popupSubAlert());
                        }
                    }
                    if (sepuede)
                    {
                        await App.Database.DeleteProductos(page3ProSelected);
                        PopupAlert.PopupLabelTitulo = "PRODUCTO ELIMINADO";
                        PopupAlert.PopupLabelText = "El producto se eliminó correctamente.";
                        await Navigation.PushPopupAsync(new PopupAlert());

                        MostrarTpro();
                    }
                }
                if (botonSeleccionadoPro == 2)
                {
                    if (TimeActual.DayOfYear == page3VenSelected.Fecha.DayOfYear && TimeActual.Year == page3VenSelected.Fecha.Year)
                    {
                        await App.Database.DeleteVenta(page3VenSelected);
                        PopupAlert.PopupLabelTitulo = "VENTA ELIMINADA";
                        PopupAlert.PopupLabelText = "La venta se eliminó correctamente.";
                        await Navigation.PushPopupAsync(new PopupAlert());

                        MostrarTven();
                    }
                    else
                    {
                        popupSubAlert.TextSubAlert = "ERROR: Esta venta ya se ha guardado, para eliminar una venta debe haberse creados el mismo día.";
                        await Navigation.PushPopupAsync(new popupSubAlert());
                    }

                }
            }
            else {
                popupSubAlert.TextSubAlert = "ERROR: Debe seleccionar productos o ventas, luego seleccionar un elemento de la lista correspondiente y podrá eliminar el elemento.";
                await Navigation.PushPopupAsync(new popupSubAlert());
            }
            
        }
        private async void ButtonActualizarPro_Clicked(object sender, EventArgs e)
        {
            if (botonSeleccionadoPro != 0 && selectedAny)
            {
                bool sepuede = false;
                if (botonSeleccionadoPro == 1)
                {
                    sepuede = true;
                }
                if (botonSeleccionadoPro == 2)
                {
                    DateTime TimeActual = DateTime.Now;
                    TimeActual = TimeActual.ToLocalTime();

                    if (TimeActual.DayOfYear == page3VenSelected.Fecha.DayOfYear && TimeActual.Year == page3VenSelected.Fecha.Year)
                    {
                        sepuede = true;
                    }
                    else
                    {
                        popupSubAlert.TextSubAlert = "ERROR: Este reporte ya quedó registrado, no es posible hacer cambios sobre él. Solo se puede hacer cambios en reportes del día de hoy.";
                        await Navigation.PushPopupAsync(new popupSubAlert());
                    }                    
                }
                if (sepuede)
                {
                    await Task.Delay(1000);
                    await Navigation.PushPopupAsync(new UpdateProductos());
                    MessagingCenter.Subscribe<UpdateProductos, string>(this, "UpdateProductos", async (s, arg) =>
                    {
                        ByDefault();
                    });
                    
                }
            }
            else
            {
                popupSubAlert.TextSubAlert = "ERROR: Debe elegir entre las inversiones y los registros, y seleccionar una denominación. Podrá ver la lista corespondiente y modificar los elementos.";
                await Navigation.PushPopupAsync(new popupSubAlert());
            }
        }
        public static Product page3ProSelected = new Product();
        private async void ViewPage3Pro_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedAny = true;
            page3ProSelected = (Product)e.SelectedItem;
            var listMat = await App.Database.GetByIdProMateriaPrima(page3ProSelected.Id);
            PopupAlert.PopupLabelTitulo = "DETALLES DEL PRODUCTO";
            PopupAlert.PopupLabelText = $"Nombre: {page3ProSelected.Nombre} \nDenominación: {page3ProSelected.Denominacion} \nPrecio: {page3ProSelected.Precio} \nDificultad: {page3ProSelected.Dificultad} \nDemanda: {page3ProSelected.Demanda} \nInversión: {page3ProSelected.Inversion} \nDescripción: {page3ProSelected.Descripcion} \n";
            if(listMat.Count > 0)
            {
                PopupAlert.PopupLabelText += "Materias Primas: \n";
                listMat.ForEach(x => PopupAlert.PopupLabelText += x.Nombre + " " + x.CantidadPorProducto + "\n");
            }
            await Navigation.PushPopupAsync(new PopupAlert());
        }
        public static Venta page3VenSelected = new Venta();
        private async void ViewPage3Ven_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedAny = true;
            page3VenSelected = (Venta)e.SelectedItem;
            var obj = await App.Database.GetIdProductos(page3VenSelected.IdPro);
            PopupAlert.PopupLabelTitulo = "DETALLES DE LA VENTA";
            PopupAlert.PopupLabelText = $"Nombre del producto: {obj.Nombre} \nPrecio: {page3VenSelected.Precio} \nUnidades: {page3VenSelected.Unidades} \nFecha: {page3VenSelected.Fecha} \nDesctipción: {page3VenSelected.Descripcion}";
            await Navigation.PushPopupAsync(new PopupAlert());
        }


        // ------------------------------------------page4------------------------------------------


        private async void ByDefault4()
        {
            List<ViewProcesos> listView = new List<ViewProcesos>();
            var procesos = await App.Database.GetActivosProcesos(true);
            if (procesos.Count > 0)
            {
                string nombre = "";
                DateTime fechaIni;
                DateTime fechaFin;
                string duracion = "";
                string descripcion = "";
                int unidades = 1;
                foreach (var item in procesos)
                {
                    unidades = item.Cantidad;
                    fechaIni = item.FechaIni;
                    descripcion = item.Descripcion;
                    var producto = await App.Database.GetIdProductos(item.IdPro);
                    nombre = producto.Nombre;
                    duracion = $"{Convert.ToInt32(item.TimeEsperado / 60 / 60 / 24)} días, {Convert.ToInt32((item.TimeEsperado / 60 / 60) % 24)} horas, {Convert.ToInt32((item.TimeEsperado / 60 % 60))} min, {Convert.ToInt32(item.TimeEsperado % 60)} seg";
                    fechaFin = item.FechaIni.AddSeconds(item.TimeEsperado);
                    listView.Add(new ViewProcesos() { 
                    Descripcion = descripcion,
                    Duracion = duracion,
                    FechaFin = fechaFin,
                    FechaIni = fechaIni,
                    Nombre = nombre,
                    Unidades = unidades,
                    Id = item.Id
                    });
                }
                var ordenado = listView.OrderBy(x => x.FechaFin);
                listViewProcesos.ItemsSource = ordenado;
            }
        }
        public static Procesos procesoSelected4 = new Procesos();
        private async void listViewProcesos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var view = (ViewProcesos)e.SelectedItem;
            procesoSelected4 = await App.Database.GetIdProcesos(view.Id);
            await Navigation.PushPopupAsync(new popupModificarProceso());
            MessagingCenter.Subscribe<popupModificarProceso, string>(this, "popupModificarProceso", async (s, arg) =>
            {
                ByDefault();
            });
        }
        private async void ButtonInicioProceso_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupAgregarProceso());
            MessagingCenter.Subscribe<popupAgregarProceso, string>(this, "popupAgregarProceso", async (s, arg) =>
            {
                ByDefault();
            });
        }


        //algoritmos
        public static string ColorRange(double estandar, double range, double value)
        {
            string res = "000";
            if (value > estandar + range || value < estandar - range) return "f00";
            if (value == estandar) return "0f0";
            double dif = range / 30;
            double margen = 0;
            if (estandar > value) margen = estandar - value;
            if (estandar < value) margen = value - estandar;
            int num = Convert.ToInt32(margen / dif);
            switch (num)
            {
                case 0:
                    return "0f0";
                case 1:
                    return "1f0";
                case 2:
                    return "2f0";
                case 3:
                    return "3f0";
                case 4:
                    return "4f0";
                case 5:
                    return "5f0";
                case 6:
                    return "6f0";
                case 7:
                    return "7f0";
                case 8:
                    return "8f0";
                case 9:
                    return "9f0";
                case 10:
                    return "af0";
                case 11:
                    return "bf0";
                case 12:
                    return "cf0";
                case 13:
                    return "df0";
                case 14:
                    return "ef0";
                case 15:
                    return "ff0";
                case 16:
                    return "fe0";
                case 17:
                    return "fd0";
                case 18:
                    return "fc0";
                case 19:
                    return "fb0";
                case 20:
                    return "fa0";
                case 21:
                    return "f90";
                case 22:
                    return "f80";
                case 23:
                    return "f70";
                case 24:
                    return "f60";
                case 25:
                    return "f50";
                case 26:
                    return "f40";
                case 27:
                    return "f30";
                case 28:
                    return "f20";
                case 29:
                    return "f10";
                case 30:
                    return "f00";

                default:
                    res = "000";
                    break;
            }
            return res;
        }
        public async static Task<int> DificultadTiempo(string denominacion, int newDificultad)
        {
            var productos = await App.Database.GetByDenominacionProductos(denominacion);
            if (productos.Count > 0)
            {
                List<int> listDificultad = new List<int>();
                List<int> listTiempo = new List<int>();
                for (int i = 0; i < productos.Count; i++)
                {
                    listDificultad.Add(productos[i].Dificultad);
                    listTiempo.Add(productos[i].Tiempo);
                }
                
            }
            return -1;
        }
        public async static void TiempoPrecio()
        { }
        public async static void DemandaPrecio()
        { }
        public async static void InversionMateriaPrima()
        { }
        public async static void DimensionesMateriaPrima()
        { }
        public async static void MateriaPrimaInversion()
        { }
        public async static void InversionPrecio()
        { }

       
    }
    public class ViewProcesos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Unidades { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string Duracion { get; set; }
        public string Descripcion { get; set; }
    }
}