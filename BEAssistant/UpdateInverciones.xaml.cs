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
    public partial class UpdateInverciones : PopupPage
    {
        public UpdateInverciones()
        {
            InitializeComponent();
            PrepararPage();
        }

        private async void PrepararPage()
        {
            ByDefault();
            if (TabbedInversiones.botonSeleccionado == 2)
            {
                G0costo.IsVisible = true;
                if(TabbedInversiones.pickerSeleccionado == 1)
                {
                    var registro = TabbedInversiones.page3ReObjC;
                    var elemInv = await App.Database.GetIdInvConstante(registro.IdInv);
                    if (elemInv.Tipo == TiposConstante.Empleados || elemInv.Tipo == TiposConstante.Herramientas || elemInv.Tipo == TiposConstante.MateriaPrima) G1unidades.IsVisible = true;
                    EntryCostoInversion.Text = registro.Costo.ToString();
                    EntryUnidadesInversion.Text = registro.Unidades.ToString();
                }
                if (TabbedInversiones.pickerSeleccionado == 2)
                {
                    var registro = TabbedInversiones.page3ReObjA;
                    var elemInv = await App.Database.GetIdInvAcumulativa(registro.IdInv);
                    if (elemInv.Tipo == TiposAcumulativa.Herramientas || elemInv.Tipo == TiposAcumulativa.MateriaPrima) G1unidades.IsVisible = true;
                    EntryCostoInversion.Text = registro.Costo.ToString();
                    EntryUnidadesInversion.Text = registro.Unidades.ToString();
                }
                if (TabbedInversiones.pickerSeleccionado == 3)
                {
                    var registro = TabbedInversiones.page3ReObjE;
                    EntryCostoInversion.Text = registro.Costo.ToString();
                    EntryUnidadesInversion.Text = registro.Unidades.ToString();
                }
                
            }
            if (TabbedInversiones.botonSeleccionado == 1)
            {
                G0nombre.IsVisible = true;
                G4descripcion.IsVisible = true;
                if (TabbedInversiones.pickerSeleccionado == 1)
                {
                    G1Ctiempo.IsVisible = true;
                    G2Ccateroria.IsVisible = true;
                    G3Ctipo.IsVisible = true;
                    EntryNombreInversion.Text = TabbedInversiones.page3InObjC.Nombre;
                    EntryInvDescripcion.Text = TabbedInversiones.page3InObjC.Descripcion;

                    switch (TabbedInversiones.page3InObjC.Frecuencia)
                    {
                        case ConstFrecuencia.Diaria:
                            PickerTiempoCategoria.SelectedIndex = 0;
                            break;
                        case ConstFrecuencia.Semanal:
                            PickerTiempoCategoria.SelectedIndex = 1;
                            break;
                        case ConstFrecuencia.Quincenal:
                            PickerTiempoCategoria.SelectedIndex = 2;
                            break;
                        case ConstFrecuencia.Mensual:
                            PickerTiempoCategoria.SelectedIndex = 3;
                            break;
                        case ConstFrecuencia.Semestral:
                            PickerTiempoCategoria.SelectedIndex = 4;
                            break;
                        case ConstFrecuencia.Anual:
                            PickerTiempoCategoria.SelectedIndex = 5;
                            break;
                        default:
                            break;
                    }
                    switch (TabbedInversiones.page3InObjC.Categoria)
                    {
                        case ConstCategoria.Independiente:
                            PickerConstSubCategoria.SelectedIndex = 0;
                            break;
                        case ConstCategoria.Proporcional:
                            PickerConstSubCategoria.SelectedIndex = 1;
                            break;
                        default:
                            break;
                    }
                    switch (TabbedInversiones.page3InObjC.Tipo)
                    {
                        case TiposConstante.Herramientas:
                            PickerFactorTipoConstante.SelectedIndex = 0;
                            break;
                        case TiposConstante.Renta:
                            PickerFactorTipoConstante.SelectedIndex = 1;
                            break;
                        case TiposConstante.MateriaPrima:
                            PickerFactorTipoConstante.SelectedIndex = 2;
                            break;
                        case TiposConstante.Empleados:
                            PickerFactorTipoConstante.SelectedIndex = 3;
                            break;
                        case TiposConstante.Licencias:
                            PickerFactorTipoConstante.SelectedIndex = 4;
                            break;
                        case TiposConstante.ServiciosExternos:
                            PickerFactorTipoConstante.SelectedIndex = 5;
                            break;
                        case TiposConstante.Transportacion:
                            PickerFactorTipoConstante.SelectedIndex = 6;
                            break;
                        case TiposConstante.Otros:
                            PickerFactorTipoConstante.SelectedIndex = 7;
                            break;
                        default:
                            break;
                    }
                }
                if (TabbedInversiones.pickerSeleccionado == 2)
                {
                    G1Atipo.IsVisible = true;
                    EntryNombreInversion.Text = TabbedInversiones.page3InObjA.Nombre;
                    EntryInvDescripcion.Text = TabbedInversiones.page3InObjA.Descripcion;
                    switch (TabbedInversiones.page3InObjA.Tipo)
                    {
                        case TiposAcumulativa.MateriaPrima:
                            PickerFactorTipo.SelectedIndex = 0;
                            break;
                        case TiposAcumulativa.Transportacion:
                            PickerFactorTipo.SelectedIndex = 1;
                            break;
                        case TiposAcumulativa.ServiciosExternos:
                            PickerFactorTipo.SelectedIndex = 2;
                            break;
                        case TiposAcumulativa.Otros:
                            PickerFactorTipo.SelectedIndex = 3;
                            break;
                        case TiposAcumulativa.Herramientas:
                            PickerFactorTipo.SelectedIndex = 4;
                            break;
                        default:
                            break;
                    }
                }
                if (TabbedInversiones.pickerSeleccionado == 3)
                {
                    G1Ecategoria.IsVisible = true;
                    if (TabbedInversiones.page3InObjE.Categoria == ExtraCategoria.Beneficiosas)
                    { G2Etipos1.IsVisible = true; }
                    if (TabbedInversiones.page3InObjE.Categoria == ExtraCategoria.Inprevistas)
                    { G2Etipos2.IsVisible = true; }

                    EntryNombreInversion.Text = TabbedInversiones.page3InObjE.Nombre;
                    EntryInvDescripcion.Text = TabbedInversiones.page3InObjE.Descripcion;
                    switch (TabbedInversiones.page3InObjE.Categoria)
                    {
                        case ExtraCategoria.Beneficiosas:
                            PickerExtraCategoria.SelectedIndex = 0;
                            break;
                        case ExtraCategoria.Inprevistas:
                            PickerExtraCategoria.SelectedIndex = 1;
                            break;
                        default:
                            break;
                    }
                    switch (TabbedInversiones.page3InObjE.Tipo)
                    {
                        case TiposExtra.Productividad:
                            PickerExtraTipoBenef.SelectedIndex = 0;
                            break;
                        case TiposExtra.Ahorro:
                            PickerExtraTipoBenef.SelectedIndex = 1;
                            break;
                        case TiposExtra.Calidad:
                            PickerExtraTipoBenef.SelectedIndex = 2;
                            break;
                        case TiposExtra.Rendimiento:
                            PickerExtraTipoBenef.SelectedIndex = 3;
                            break;
                        case TiposExtra.Accidente:
                            PickerExtraTipoOblig.SelectedIndex = 0;
                            break;
                        case TiposExtra.Reparacion:
                            PickerExtraTipoOblig.SelectedIndex = 1;
                            break;
                        case TiposExtra.Sustitucion:
                            PickerExtraTipoOblig.SelectedIndex = 2;
                            break;
                        case TiposExtra.Otros:
                            PickerExtraTipoOblig.SelectedIndex = 3;
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
        private async void ByDefault()
        {
            
            UpdateDependencia.IsVisible = false;
            UpdateStock.IsVisible = false;
            if (TabbedInversiones.botonSeleccionado == 1)
            {
                if(TabbedInversiones.pickerSeleccionado == 1)
                {
                    if (TabbedInversiones.page3InObjC.Categoria == ConstCategoria.Proporcional) 
                        UpdateDependencia.IsVisible = true;
                    if (TabbedInversiones.page3InObjC.Tipo == TiposConstante.MateriaPrima) 
                        UpdateStock.IsVisible = true;
                }
                if (TabbedInversiones.pickerSeleccionado == 2 
                    && TabbedInversiones.page3InObjA.Tipo == TiposAcumulativa.MateriaPrima) 
                    UpdateStock.IsVisible = true;
            }

            G0nombre.IsVisible = false;
            G4descripcion.IsVisible = false;

            G1Ctiempo.IsVisible = false;
            G2Ccateroria.IsVisible = false;
            G3Ctipo.IsVisible = false;

            G1Atipo.IsVisible = false;

            G1Ecategoria.IsVisible = false;
            G2Etipos1.IsVisible = false;
            G2Etipos2.IsVisible = false;

            G0costo.IsVisible = false;
            G1unidades.IsVisible = false;
        }
        private async void UpdateElem_Clicked(object sender, EventArgs e)
        {
            if(TabbedInversiones.botonSeleccionado == 1)
            {
                if(TabbedInversiones.pickerSeleccionado == 1)
                {
                    InConstante up = new InConstante
                    {
                        Id = TabbedInversiones.page3InObjC.Id,
                        Nombre = EntryNombreInversion.Text,
                        Descripcion = EntryInvDescripcion.Text,
                        Fecha = TabbedInversiones.page3InObjC.Fecha,
                        Frecuencia = (ConstFrecuencia)PickerTiempoCategoria.SelectedIndex,
                        Categoria = (ConstCategoria)PickerConstSubCategoria.SelectedIndex,
                        Tipo = (TiposConstante)PickerFactorTipoConstante.SelectedIndex
                    };
                    if(TabbedInversiones.page3InObjC.Categoria == ConstCategoria.Proporcional)
                    {
                        var dep = await App.Database.GetIdInvDependenciaConstante(TabbedInversiones.page3InObjC.Id);
                        var porcentaje = string.IsNullOrEmpty(popupConstDepend.porsentaje.ToString()) ? dep[0].Porcentaje : popupConstDepend.itemDependenciaConst.Porcentaje;
                        DependenciasConstantes upDepend = new DependenciasConstantes()
                        {
                            Clase = popupConstDepend.itemDependenciaConst.Clase,
                            IdItem = popupConstDepend.itemDependenciaConst.IdItem,
                            IdInv = dep[0].Id,
                            Item = popupConstDepend.itemDependenciaConst.Item,
                            Porcentaje = porcentaje
                        };
                        await App.Database.SaveUpDependenciaConstante(upDepend);
                    }
                    await App.Database.SaveUpInvConstante(up);
                    if (TabbedInversiones.page3InObjC.Tipo == TiposConstante.MateriaPrima)
                    {
                        var stock = await App.Database.GetIdInvCStock(TabbedInversiones.page3InObjC.Id);
                        var dias = string.IsNullOrEmpty(popupDuracionStock.diasStock.ToString()) ? stock[0].Duracion : popupDuracionStock.diasStock;
                        var categoria = string.IsNullOrEmpty(popupDuracionStock.CategoriaStock.ToString()) ? stock[0].Categoria : popupDuracionStock.CategoriaStock;
                        var unidades = string.IsNullOrEmpty(popupDuracionStock.UnidadesEstimadas.ToString()) ? stock[0].UnidadesEstimadas : popupDuracionStock.UnidadesEstimadas;
                        Stocker upStock = new Stocker
                        {
                            Id = stock[0].Id,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual,
                            Duracion = dias,
                            Categoria = categoria,
                            UnidadesEstimadas = unidades
                        };
                        await App.Database.SaveUpStock(upStock);
                    }
                }
                if (TabbedInversiones.pickerSeleccionado == 2)
                {
                    InAcumulativa up = new InAcumulativa
                    {
                        Id = TabbedInversiones.page3InObjA.Id,
                        Nombre = EntryNombreInversion.Text,
                        Descripcion = EntryInvDescripcion.Text,
                        Fecha = TabbedInversiones.page3InObjA.Fecha,
                        Tipo = (TiposAcumulativa)PickerFactorTipo.SelectedIndex
                    };
                    await App.Database.SaveUpInvAcumulativa(up);
                    if (TabbedInversiones.page3InObjA.Tipo == TiposAcumulativa.MateriaPrima)
                    {
                        var stock = await App.Database.GetIdInvAStock(TabbedInversiones.page3InObjA.Id);
                        var dias = string.IsNullOrEmpty(popupDuracionStock.diasStock.ToString()) ? stock[0].Duracion : popupDuracionStock.diasStock;
                        var categoria = string.IsNullOrEmpty(popupDuracionStock.CategoriaStock.ToString()) ? stock[0].Categoria : popupDuracionStock.CategoriaStock;
                        var unidades = string.IsNullOrEmpty(popupDuracionStock.UnidadesEstimadas.ToString()) ? stock[0].UnidadesEstimadas : popupDuracionStock.UnidadesEstimadas;
                        Stocker upStock = new Stocker
                        {
                            Id = stock[0].Id,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual,
                            Duracion = dias,
                            Categoria = categoria,
                            UnidadesEstimadas = unidades
                        };
                        await App.Database.SaveUpStock(upStock);
                    }
                }
                if (TabbedInversiones.pickerSeleccionado == 3)
                {
                    TiposExtra tipo = new TiposExtra();
                    if(PickerExtraCategoria.SelectedIndex == 0)
                    { tipo = (TiposExtra)PickerExtraTipoBenef.SelectedIndex + 4; }
                    if (PickerExtraCategoria.SelectedIndex == 1)
                    { tipo = (TiposExtra)PickerExtraTipoOblig.SelectedIndex; }
                    InExtraordinaria up = new InExtraordinaria
                    {
                        Id = TabbedInversiones.page3InObjE.Id,
                        Nombre = EntryNombreInversion.Text,
                        Descripcion = EntryInvDescripcion.Text,
                        Fecha = TabbedInversiones.page3InObjE.Fecha,
                        Categoria = (ExtraCategoria)PickerExtraCategoria.SelectedIndex,
                        Tipo = tipo
                    };
                    await App.Database.SaveUpInvExtraordinaria(up);
                }
            }
            if (TabbedInversiones.botonSeleccionado == 2)
            {
                if (TabbedInversiones.pickerSeleccionado == 1)
                {
                    ReConstante up = new ReConstante
                    {
                        Id = TabbedInversiones.page3ReObjC.Id,
                        IdInv = TabbedInversiones.page3ReObjC.IdInv,
                        Fecha = TabbedInversiones.page3ReObjC.Fecha,
                        Costo = Convert.ToDouble(EntryCostoInversion.Text),
                        Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                    };
                    await App.Database.SaveUpRegConstante(up);
                }
                if (TabbedInversiones.pickerSeleccionado == 2)
                {
                    ReAcumulativa up = new ReAcumulativa
                    {
                        Id = TabbedInversiones.page3ReObjA.Id,
                        IdInv = TabbedInversiones.page3ReObjA.IdInv,
                        Fecha = TabbedInversiones.page3ReObjA.Fecha,
                        Costo = Convert.ToDouble(EntryCostoInversion.Text),
                        Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                    };
                    await App.Database.SaveUpRegAcumulativa(up);
                }
                if (TabbedInversiones.pickerSeleccionado == 3)
                {
                    ReExtraordinaria up = new ReExtraordinaria
                    {
                        Id = TabbedInversiones.page3ReObjE.Id,
                        IdInv = TabbedInversiones.page3ReObjE.IdInv,
                        Fecha = TabbedInversiones.page3ReObjE.Fecha,
                        Costo = Convert.ToDouble(EntryCostoInversion.Text),
                        Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                    };
                    await App.Database.SaveUpRegExtraordinaria(up);
                }
            }
            MessagingCenter.Send<UpdateInverciones, string>(this, "UpdateInverciones", "x");
            await PopupNavigation.Instance.PopAsync(true);
        }
        private async void volver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
        private async void UpdateStock_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupDuracionStock());
        }
        private async void UpdateDependencia_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new popupConstDepend());
        }
    }
}