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
    public partial class popupAgregarProceso : PopupPage
    {
        public popupAgregarProceso()
        {
            InitializeComponent();
            LlenarPicked();
        }

        private async void LlenarPicked()
        {
            Producto = await App.Database.GetProductos();
            selectProducto.Items.Clear();
            Producto.ForEach(x => selectProducto.Items.Add(x.Nombre));
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
        public static int indexProducto = -1;
        public static List<Product> Producto = new List<Product>();
        private void selectProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexProducto = selectProducto.SelectedIndex;
        }
        private async void GuardarProceso_Clicked(object sender, EventArgs e)
        {
            int cantidad = 1;
            string descripcion = "";
            if (!string.IsNullOrEmpty(EntryCantUnidades.Text)) cantidad = Convert.ToInt32(EntryCantUnidades.Text);
            if (!string.IsNullOrEmpty(EntryDescripcion.Text)) descripcion = EntryDescripcion.Text;
            if(indexProducto == -1)
            {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe seleccionar un producto para crear el proceso.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            else
            {
                DateTime TimeActual = DateTime.Now;
                TimeActual = TimeActual.ToLocalTime();
                // algoritmo de caculo de duracion estimada
                int esperado = Producto[indexProducto].Tiempo * cantidad;
                var inventario = await App.Database.GetInventario();
                var listmat = await App.Database.GetByIdProMateriaPrima(Producto[indexProducto].Id);
                foreach (var item in listmat)
                {
                    int id = 0;
                    var invA = await App.Database.GetByNameInvAcumulativa(item.Nombre);
                    if (invA.Count == 0)
                    {
                        var invC = await App.Database.GetByNameInvConstante(item.Nombre);
                        if (invC.Count != 0)
                        {
                            var stock = await App.Database.GetIdInvCStock(invC[0].Id);
                            Stocker up = new Stocker()
                            {
                                Id = stock[0].Id,
                                Duracion = stock[0].Duracion,
                                IdInv = stock[0].IdInv,
                                TipoInv = stock[0].TipoInv,
                                CantActual = stock[0].CantActual - (item.CantidadPorProducto * cantidad),
                                Categoria = stock[0].Categoria,
                                UnidadesEstimadas = stock[0].UnidadesEstimadas
                            }; await App.Database.SaveUpStock(up);
                        }
                    }
                    else
                    {
                        var stock = await App.Database.GetIdInvAStock(invA[0].Id);
                        Stocker up = new Stocker()
                        {
                            Id = stock[0].Id,
                            Duracion = stock[0].Duracion,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual - (item.CantidadPorProducto * cantidad),
                            Categoria = stock[0].Categoria,
                            UnidadesEstimadas = stock[0].UnidadesEstimadas
                        }; await App.Database.SaveUpStock(up);
                    }
                    //inventario
                    var inv = inventario.Find(x => x.Materias == item.Nombre && x.Fecha.DayOfYear == TimeActual.DayOfYear && x.Fecha.Year == TimeActual.Year);
                    if (inv != null)
                    {
                        inv.ConsUtil += Convert.ToInt32(item.CantidadPorProducto * cantidad);
                        await App.Database.SaveUpInventario(inv);
                    }
                    else
                    {
                        await App.Database.SaveInventario(new Inventario()
                        {
                            Materias = item.Nombre,
                            ConsUtil = Convert.ToDouble(item.CantidadPorProducto * cantidad),
                            ConsCaduco = 0,
                            ConsExedente = 0,
                            Fecha = TimeActual
                        });
                    }
                }

                await App.Database.SaveProcesos(new Procesos() { 
                Cantidad = cantidad,
                Descripcion = descripcion,
                EnProceso = true,
                IdPro = Producto[indexProducto].Id,
                TimeResultado = 0,
                FechaIni = TimeActual,
                TimeEsperado = esperado
                });
            }
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}