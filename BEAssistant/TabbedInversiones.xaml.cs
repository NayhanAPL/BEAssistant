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
    public partial class TabbedInversiones : TabbedPage
    {
        public TabbedInversiones()
        {
            InitializeComponent();
            ByDefaultPage1();
            ByDefaultPage2();
            ByDefaultPage3();
            ByDefaultPage4();
        }


        //----------------------------------Page 1--------------------------------------
        public static InConstante objC = new InConstante();
        public static InAcumulativa objA = new InAcumulativa();
        public static InExtraordinaria objE = new InExtraordinaria();
        public static int DenomSelected = 3;
        private void ByDefaultPage1()
        {
            labelNoHayPage1.IsVisible = false;
            ViewNombresInv.IsVisible = false;
            rellenoListView.IsVisible = true;
            EntryCostoInversion.Text = "1";
            EntryUnidadesInversion.Text = "1";
            LabelDeCosto.IsVisible = false;
            EntryCostoInversion.IsVisible = false;
            labelCostoInversionUnidad.IsVisible = false;
            LabelCantUnid.IsVisible = false;
            EntryUnidadesInversion.IsVisible = false;
        }       
        private async void ViewNombresInv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (selectDenominacion.SelectedIndex == 0)
            {
                DenomSelected = 0;
                objC = (InConstante)e.SelectedItem;
                var listaLastvalue = await App.Database.GetIdInvRegConstante(objC.Id);
                if (listaLastvalue.Count != 0)
                {
                    var Lastvalue = listaLastvalue[listaLastvalue.Count - 1];
                    EntryCostoInversion.Text = Lastvalue.Costo.ToString();
                    EntryUnidadesInversion.Text = Lastvalue.Unidades.ToString();                   
                }
                EntryCostoInversion.IsVisible = true;
                LabelDeCosto.IsVisible = true;
                if ((objC.Tipo == TiposConstante.Herramientas || objC.Tipo == TiposConstante.Empleados || objC.Tipo == TiposConstante.MateriaPrima) && objC.Categoria != ConstCategoria.Proporcional)
                {
                    labelCostoInversionUnidad.IsVisible = true;                
                    LabelCantUnid.IsVisible = true;
                    EntryUnidadesInversion.IsVisible = true;
                }
                else
                {
                    labelCostoInversionUnidad.IsVisible = false;
                    LabelCantUnid.IsVisible = false;
                    EntryUnidadesInversion.IsVisible = false;
                }
                
            }                            
            if (selectDenominacion.SelectedIndex == 1)
            {
                DenomSelected = 1;
                objA = (InAcumulativa)e.SelectedItem;
                var listaLastvalue = await App.Database.GetIdInvRegAcumulativa(objA.Id);
                if (listaLastvalue.Count != 0)
                {
                    var Lastvalue = listaLastvalue[listaLastvalue.Count - 1];
                    EntryCostoInversion.Text = Lastvalue.Costo.ToString();
                    EntryUnidadesInversion.Text = Lastvalue.Unidades.ToString();
                }
                EntryCostoInversion.IsVisible = true;
                LabelDeCosto.IsVisible = true;

                if (objA.Tipo == TiposAcumulativa.MateriaPrima || objA.Tipo == TiposAcumulativa.Herramientas)
                {
                    labelCostoInversionUnidad.IsVisible = true;
                    LabelCantUnid.IsVisible = true;
                    EntryUnidadesInversion.IsVisible = true;
                }
                else
                {
                    labelCostoInversionUnidad.IsVisible = false;
                    LabelCantUnid.IsVisible = false;
                    EntryUnidadesInversion.IsVisible = false;
                }
            }
            if (selectDenominacion.SelectedIndex == 2)
            {
                DenomSelected = 2;
                objE = (InExtraordinaria)e.SelectedItem;
                var listaLastvalue = await App.Database.GetIdInvRegExtraordinaria(objE.Id);
                if (listaLastvalue.Count != 0)
                {
                    var Lastvalue = listaLastvalue[listaLastvalue.Count - 1];
                    EntryCostoInversion.Text = Lastvalue.Costo.ToString();
                    EntryUnidadesInversion.Text = Lastvalue.Unidades.ToString();
                }
                EntryCostoInversion.IsVisible = true;
                LabelDeCosto.IsVisible = true;
            }             
        }
        private async void ButtonReportarInv_Clicked(object sender, EventArgs e)
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            if (ViewNombresInv.SelectedItem == null) {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe seleccionar un elemento de la lista de inversiones";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            else
            {
                if (string.IsNullOrEmpty(EntryUnidadesInversion.Text)) EntryUnidadesInversion.Text = 1.ToString();
                if (string.IsNullOrEmpty(EntryCostoInversion.Text)) EntryCostoInversion.Text = 1.ToString();
                if (DenomSelected == 0)
                {
                    ReConstante existeC = new ReConstante();
                    var lis = await App.Database.GetIdInvRegConstante(objC.Id); 
                    if(lis.Count != 0) existeC = lis[0];
                    if (existeC.Id != 0)
                    {
                        var res = await DisplayAlert("ESTA INVERSIÓN YA ESTÁ DECLARADA", "Ya se ha reportado una inversión constante de este elemento. ¿Desea sobreescribirlo o agregar otra inversión del mismo tipo?", "SOBREESCRIBIR", "OTRA INVERSIÓN");
                        if(res)
                        {
                            ReConstante up = new ReConstante
                            {
                                Id = existeC.Id,
                                Fecha = TimeActual,
                                Costo = Convert.ToDouble(EntryCostoInversion.Text),
                                Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                                IdInv = objC.Id
                            };
                            await App.Database.SaveUpRegConstante(up);
                        }
                        else
                        {
                            await App.Database.SaveRegConstante(new ReConstante
                            {
                                Fecha = TimeActual,
                                Costo = Convert.ToDouble(EntryCostoInversion.Text),
                                Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                                IdInv = objC.Id
                            });
                            var lastItem = await App.Database.GetLastItemRegConstante();
                            await App.Database.SaveCaducidad(new Caducidad()
                            {
                                IdReg = lastItem[0].IdInv,
                                Caduco = false,
                                TipoInv = "C"
                            });
                            PopupAlert.PopupLabelTitulo = "Inversión registrada";
                            PopupAlert.PopupLabelText = "Se ha guardado correctamente este nuevo reporte de inversión Constante. Usted puede estar al tanto de los efectos que esta haya causado en su negocio en la página (Historial y Estadísticas)";
                            await Navigation.PushPopupAsync(new PopupAlert());
                        }
                    }   

                    if(lis.Count == 0)
                    {
                        
                        await App.Database.SaveRegConstante(new ReConstante
                        {
                            Fecha = TimeActual,
                            Costo = Convert.ToDouble(EntryCostoInversion.Text),
                            Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                            IdInv = objC.Id
                        });
                        var lastItem = await App.Database.GetLastItemRegConstante();
                        await App.Database.SaveCaducidad(new Caducidad()
                        {
                            IdReg = lastItem[0].IdInv,
                            Caduco = false,
                            TipoInv = "C"
                        });
                    }
                    UpDateCostoMatPri();
                    if (objC.Tipo == TiposConstante.MateriaPrima)
                    {
                        var stock = await App.Database.GetIdInvCStock(objC.Id);
                        Stocker upStock = new Stocker
                        {
                            Id = stock[0].Id,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual += Convert.ToInt32(EntryUnidadesInversion.Text),
                            Duracion = stock[0].Duracion,
                            Categoria = stock[0].Categoria,
                            UnidadesEstimadas = stock[0].UnidadesEstimadas
                        };
                        await App.Database.SaveUpStock(upStock);
                    }
                }
                if (DenomSelected == 1)
                {
                    await App.Database.SaveRegAcumulativa(new ReAcumulativa
                    {
                        Fecha = TimeActual,
                        Costo = Convert.ToDouble(EntryCostoInversion.Text),
                        Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                        IdInv = objA.Id
                    });
                    var lastItem = await App.Database.GetLastItemRegAcumulativa();
                    await App.Database.SaveCaducidad(new Caducidad()
                    {
                        IdReg = lastItem[0].IdInv,
                        Caduco = false,
                        TipoInv = "A"
                    });
                    if (objA.Tipo == TiposAcumulativa.MateriaPrima)
                    {
                        costoDeuda = Convert.ToDouble(EntryCostoInversion.Text);
                        UpDateCostoMatPri();
                        var stock = await App.Database.GetIdInvAStock(objA.Id);
                        Stocker upStock = new Stocker
                        {
                            Id = stock[0].Id,
                            IdInv = stock[0].IdInv,
                            TipoInv = stock[0].TipoInv,
                            CantActual = stock[0].CantActual += Convert.ToInt32(EntryUnidadesInversion.Text),
                            Duracion = stock[0].Duracion,
                            Categoria = stock[0].Categoria,
                            UnidadesEstimadas = stock[0].UnidadesEstimadas
                        };
                        await App.Database.SaveUpStock(upStock);
                    }
                }
                if (DenomSelected == 2)
                {
                    await App.Database.SaveRegExtraordinaria(new ReExtraordinaria
                    {
                        Fecha = TimeActual,
                        Costo = Convert.ToDouble(EntryCostoInversion.Text),
                        Unidades = Convert.ToDouble(EntryUnidadesInversion.Text),
                        IdInv = objE.Id
                    });
                }
                PopupAlert.PopupLabelTitulo = "Inversión registrada";
                PopupAlert.PopupLabelText = "Se ha guardado correctamente este nuevo reporte de inversión Constante. Usted puede estar al tanto de los efectos que esta haya causado en su negocio en la página (Historial y Estadísticas)";
                await Navigation.PushPopupAsync(new PopupAlert());
                ByDefaultPage1();    
            }
            
        }
        private async void SelectDenominacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ByDefaultPage1();
            labelNoHayPage1.IsVisible = false;
            ViewNombresInv.IsVisible = true;
            rellenoListView.IsVisible = false;
            if (selectDenominacion.SelectedIndex == 0)
            {
                var nombreInversionC = await App.Database.GetInvConstante();
                if (nombreInversionC != null)
                {
                    nombreInversionC.ForEach(x => x.Descripcion = ReducirText(x.Descripcion));
                    var nombreInversionc = nombreInversionC.OrderByDescending(x => x.Fecha);
                    ViewNombresInv.ItemsSource = nombreInversionc;
                }
                if(nombreInversionC.Count == 0)
                {
                    ViewNombresInv.IsVisible = false;
                    labelNoHayPage1.Text = "Aún no hay inversiones Constantes.";
                    labelNoHayPage1.IsVisible = true;
                }
            }
            if (selectDenominacion.SelectedIndex == 1)
            {
                var nombreInversionA = await App.Database.GetInvAcumulativa();
                if (nombreInversionA != null)
                {
                    nombreInversionA.ForEach(x => x.Descripcion = ReducirText(x.Descripcion));
                    var nombreInversiona = nombreInversionA.OrderByDescending(x => x.Fecha);
                    ViewNombresInv.ItemsSource = nombreInversiona;
                }
                if (nombreInversionA.Count == 0)
                {
                    ViewNombresInv.IsVisible = false;
                    labelNoHayPage1.Text = "Aún no hay inversiones Acumulativas.";
                    labelNoHayPage1.IsVisible = true; 
                }
            }           
            if (selectDenominacion.SelectedIndex == 2)
            {
                var nombreInversionE = await App.Database.GetInvExtraordinaria();
                if (nombreInversionE != null)
                {
                    nombreInversionE.ForEach(x => x.Descripcion = ReducirText(x.Descripcion));
                    var nombreInversione = nombreInversionE.OrderByDescending(x => x.Fecha);
                    ViewNombresInv.ItemsSource = nombreInversione;
                }
                if (nombreInversionE.Count == 0)
                {
                    ViewNombresInv.IsVisible = false;
                    labelNoHayPage1.Text = "Aún no hay inversiones Extraordinarias.";
                    labelNoHayPage1.IsVisible = true;
                }
            } 
        }

        public static double costoDeuda = 0;
        public static int unidadesDeuda = 0;
        private async void ButtonComoDeuda_Clicked(object sender, EventArgs e)
        {
            bool sepuede = true;
            PopupAlert.PopupLabelTitulo = "ERROR";
            if (selectDenominacion.SelectedIndex == -1)
            {
                sepuede = false;
                PopupAlert.PopupLabelText = "Debe seleccionar una denominación y llenar los datos que se le piden";
            }
            if (objA == null && objC == null && objE == null)
            {
                PopupAlert.PopupLabelText = "Debe seleccionar un elemento de la lista y llenar los datos que se le piden";
                sepuede = false;
            }
            if (string.IsNullOrEmpty(EntryCostoInversion.Text))
            {
                PopupAlert.PopupLabelText = "Debe declarar el precio de la cada unidad que compró.";
                sepuede = false;
            }
            if (string.IsNullOrEmpty(EntryUnidadesInversion.Text))
            {                
                PopupAlert.PopupLabelText = "Debe declarar la cantidad de unidades en las que invirtió.";               
                sepuede = false;
            }
            if (sepuede)
            {
                costoDeuda = Convert.ToDouble(EntryCostoInversion.Text);
                unidadesDeuda = Convert.ToInt32(EntryUnidadesInversion.Text);
                await Navigation.PushPopupAsync(new ReportarDeuda());
                ByDefaultPage4();
            }
            else await Navigation.PushPopupAsync(new PopupAlert());
        }
        public async static void UpDateCostoMatPri()
        {
            var matPri = await App.Database.GetByPrecUniVacioMateriaPrima();
            if (matPri.Count != 0)
            {
                foreach (var item in matPri)
                {
                    if (item.Nombre == objA.Nombre)
                    {
                        MateriasPrimas mp = new MateriasPrimas()
                        {
                            Id = item.Id,
                            Nombre = item.Nombre,
                            IdProducto = item.IdProducto,
                            PrecioUnidad = costoDeuda,
                            CantidadPorProducto = item.CantidadPorProducto
                        };
                        await App.Database.SaveUpMateriaPrima(mp);
                    }
                }
            }
        }

        //----------------------------------Page 2--------------------------------------

        public static bool boolMatPriByPicker = false;
        private void ByDefaultPage2()
        {
            EntryNombreInversion.IsVisible = true;
            EntryNombreInversion.Text = "";
            StackByDefault.IsVisible = true;
            PickerMatPriSujerencias.IsVisible = false;
            flechita.IsVisible = false;
            StackConstante.IsVisible = false;
            StackAcumulativa.IsVisible = false;
            StackExtraordinaria.IsVisible = false;

            StackByDefault2.IsVisible = true;
            StackSubConstante.IsVisible = false;
            StackExtraBeneficiosa.IsVisible = false;
            StackExtraObligatoria.IsVisible = false;

            StackTipoConstante.IsVisible = false;            
        }
        private async void ButtonCrearInversion_Clicked(object sender, EventArgs e)
        {
            if (denominacion.SelectedIndex != -1 && !string.IsNullOrEmpty(EntryNombreInversion.ToString()))
            {

                if (denominacion.SelectedIndex == 0)
                {
                    if (PickerTiempoCategoria.SelectedIndex != -1)
                    {
                        if (PickerConstSubCategoria.SelectedIndex != -1)
                        {
                            bool depende = false;
                            if (PickerFactorTipoConstante.SelectedIndex != -1)
                            {
                                string nombre = "";
                                if (PickerMatPriSujerencias.SelectedIndex != -1) nombre = PickerMatPriSujerencias.SelectedItem.ToString();
                                else nombre = EntryNombreInversion.Text;
                                if (!string.IsNullOrEmpty(nombre))
                                {
                                    depende = true;
                                    DateTime TimeActual = DateTime.Now;
                                    TimeActual = TimeActual.ToLocalTime();
                                    await App.Database.SaveInvConstante(new InConstante
                                    {
                                        Fecha = TimeActual,
                                        Nombre = nombre,
                                        Descripcion = DescripcionInversion.InvDescripcion,
                                        Categoria = (ConstCategoria)PickerConstSubCategoria.SelectedIndex,
                                        Frecuencia = (ConstFrecuencia)PickerTiempoCategoria.SelectedIndex,
                                        Tipo = (TiposConstante)PickerFactorTipoConstante.SelectedIndex
                                    });
                                    if (PickerFactorTipoConstante.SelectedIndex == 2)
                                    {
                                        var item = await App.Database.GetLastItemInvConstante();
                                        await App.Database.SaveStock(new Stocker()
                                        {
                                            IdInv = item[0].Id,
                                            CantActual = 0,
                                            TipoInv = "Constante",
                                            Duracion = popupDuracionStock.diasStock,
                                            Categoria = popupDuracionStock.CategoriaStock,
                                            UnidadesEstimadas = popupDuracionStock.UnidadesEstimadas
                                        });
                                    }
                                    PopupAlert.PopupLabelTitulo = "Inversión registrada";
                                    PopupAlert.PopupLabelText = "Ahora puede reportar inversiones con las caracteristicas que acaba de crear, para ello vaya a la página ( REPORTAR INVERSIÓN )";
                                    await Navigation.PushPopupAsync(new PopupAlert());

                                    ByDefaultPage2();
                                }
                                else
                                {
                                    PopupAlert.PopupLabelTitulo = "ERROR";
                                    PopupAlert.PopupLabelText = "Debe darle un nombre a la inversión";
                                    await Navigation.PushPopupAsync(new PopupAlert());
                                }
                            }
                            else
                            {
                                PopupAlert.PopupLabelTitulo = "ERROR";
                                PopupAlert.PopupLabelText = "Asegurese de llenar correctamente todo el cuestionario";
                                await Navigation.PushPopupAsync(new PopupAlert());
                            }
                            if (PickerConstSubCategoria.SelectedIndex == 1 && depende)
                            {
                                var lastInv = await App.Database.GetLastItemInvConstante();
                                await App.Database.SaveDependenciaConstante(new DependenciasConstantes()
                                {
                                    Clase = popupConstDepend.itemDependenciaConst.Clase,
                                    IdItem = popupConstDepend.itemDependenciaConst.IdItem,
                                    IdInv = lastInv[0].Id,
                                    Item = popupConstDepend.itemDependenciaConst.Item,
                                    Porcentaje = Convert.ToInt32(popupConstDepend.porsentaje)
                                });
                            }
                        }
                        else
                        {
                            PopupAlert.PopupLabelTitulo = "ERROR";
                            PopupAlert.PopupLabelText = "Asegurese de llenar correctamente todo el cuestionario";
                            await Navigation.PushPopupAsync(new PopupAlert());
                        }
                    }
                    else
                    {
                        PopupAlert.PopupLabelTitulo = "ERROR";
                        PopupAlert.PopupLabelText = "Asegurese de llenar correctamente todo el cuestionario";
                        await Navigation.PushPopupAsync(new PopupAlert());
                    }
                }
                else if (denominacion.SelectedIndex == 1)
                {
                    if (PickerFactorTipo.SelectedIndex != -1)
                    {
                        
                        string nombre = "";
                        if (PickerMatPriSujerencias.SelectedIndex != -1) 
                            nombre = PickerMatPriSujerencias.SelectedItem.ToString();
                        else nombre = EntryNombreInversion.Text;

                        if (!String.IsNullOrEmpty(nombre))
                        {
                            DateTime TimeActual = DateTime.Now;
                            TimeActual = TimeActual.ToLocalTime();
                            await App.Database.SaveInvAcumulativa(new InAcumulativa
                            {
                                Fecha = TimeActual,
                                Nombre = nombre,
                                Descripcion = DescripcionInversion.InvDescripcion,
                                Tipo = (TiposAcumulativa)PickerFactorTipo.SelectedIndex
                            });
                            if (PickerFactorTipo.SelectedIndex == 0)
                            {
                                var item = await App.Database.GetLastItemInvAcumulativa();
                                await App.Database.SaveStock(new Stocker()
                                {
                                    IdInv = item[0].Id,
                                    CantActual = 0,
                                    TipoInv = "Acumulativa",
                                    Duracion = popupDuracionStock.diasStock,
                                    Categoria = popupDuracionStock.CategoriaStock,
                                    UnidadesEstimadas = popupDuracionStock.UnidadesEstimadas
                                });
                            }
                            PopupAlert.PopupLabelTitulo = "Elemento registrado";
                            PopupAlert.PopupLabelText = "Ahora puede reportar inversiones con las caracteristicas que acaba de crear, para ello vaya a la página ( REPORTAR INVERSIÓN )";
                            await Navigation.PushPopupAsync(new PopupAlert());
                            ByDefaultPage2();
                        }                        
                        else
                        {
                            PopupAlert.PopupLabelTitulo = "ERROR";
                            PopupAlert.PopupLabelText = "Debe darle un nombre a la inversión";
                            await Navigation.PushPopupAsync(new PopupAlert());
                        }
                    }
                    else
                    {
                        PopupAlert.PopupLabelTitulo = "ERROR";
                        PopupAlert.PopupLabelText = "Asegurese de llenar correctamente todo el cuestionario";
                        await Navigation.PushPopupAsync(new PopupAlert());
                    }
                }
                else if (denominacion.SelectedIndex == 2)
                {
                    if (PickerExtraCategoria.SelectedIndex != -1)
                    {
                        if (PickerExtraCategoria.SelectedIndex == 0)
                        {
                            if (PickerExtraTipoBenef.SelectedIndex != -1)
                            {
                                if(!String.IsNullOrEmpty(EntryNombreInversion.Text))
                                {
                                    DateTime TimeActual = DateTime.Now;
                                    TimeActual = TimeActual.ToLocalTime();
                                    await App.Database.SaveInvExtraordinaria(new InExtraordinaria
                                    {
                                        Fecha = TimeActual,
                                        Nombre = EntryNombreInversion.Text,
                                        Descripcion = DescripcionInversion.InvDescripcion,
                                        Categoria = ExtraCategoria.Beneficiosas,
                                        Tipo = (TiposExtra)PickerExtraTipoBenef.SelectedIndex + 4
                                    });
                                    PopupAlert.PopupLabelTitulo = "Elemento registrado";
                                    PopupAlert.PopupLabelText = "Ahora puede reportar inversiones con las caracteristicas que acaba de crear, para ello vaya a la página ( REPORTAR INVERSIÓN )";
                                    await Navigation.PushPopupAsync(new PopupAlert());
                                    ByDefaultPage2();
                                }
                                else
                                {
                                    PopupAlert.PopupLabelTitulo = "ERROR";
                                    PopupAlert.PopupLabelText = "Debe darle un nombre a la inversión";
                                    await Navigation.PushPopupAsync(new PopupAlert());
                                }
                            }
                        }
                        if (PickerExtraCategoria.SelectedIndex == 1)
                        {
                            if (PickerExtraTipoOblig.SelectedIndex != -1)
                            {
                                if (String.IsNullOrEmpty(EntryNombreInversion.Text))
                                {
                                    DateTime TimeActual = DateTime.Now;
                                    TimeActual = TimeActual.ToLocalTime();
                                    await App.Database.SaveInvExtraordinaria(new InExtraordinaria
                                    {
                                        Fecha = TimeActual,
                                        Nombre = EntryNombreInversion.Text,
                                        Descripcion = DescripcionInversion.InvDescripcion,
                                        Categoria = ExtraCategoria.Beneficiosas,
                                        Tipo = (TiposExtra)PickerExtraTipoOblig.SelectedIndex
                                    });
                                    PopupAlert.PopupLabelTitulo = "Elemento registrado";
                                    PopupAlert.PopupLabelText = "Ahora puede reportar inversiones con las caracteristicas que acaba de crear, para ello vaya a la página ( REPORTAR INVERSIÓN )";
                                    await Navigation.PushPopupAsync(new PopupAlert());
                                    ByDefaultPage2();
                                }
                                else
                                {
                                    PopupAlert.PopupLabelTitulo = "ERROR";
                                    PopupAlert.PopupLabelText = "Debe darle un nombre a la inversión";
                                    await Navigation.PushPopupAsync(new PopupAlert());
                                }
                            }
                        }
                    }
                    else
                    {
                        PopupAlert.PopupLabelTitulo = "ERROR";
                        PopupAlert.PopupLabelText = "Asegurese de llenar correctamente todo el cuestionario";
                        await Navigation.PushPopupAsync(new PopupAlert());
                    }
                }
                else
                {
                    PopupAlert.PopupLabelTitulo = "ERROR";
                    PopupAlert.PopupLabelText = "Asegurese de llenar correctamente todo el cuestionario";
                    await Navigation.PushPopupAsync(new PopupAlert());
                }
            }
        }
        private void denominacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (denominacion.SelectedIndex != -1)
            {
                string name = EntryNombreInversion.Text;
                ByDefaultPage2();
                StackByDefault.IsVisible = false;
                StackByDefault2.IsVisible = false;
                if (denominacion.SelectedIndex == 0)
                {
                    StackConstante.IsVisible = true;
                    StackSubConstante.IsVisible = true;
                    StackTipoConstante.IsVisible = true;
                }
                if (denominacion.SelectedIndex == 1)
                {
                    StackAcumulativa.IsVisible = true;
                }
                if (denominacion.SelectedIndex == 2)
                {
                    StackExtraordinaria.IsVisible = true;
                    StackByDefault2.IsVisible = true;
                }
                EntryNombreInversion.Text = name;
            }
        }
        private async void ButtonDescripcionInversion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new DescripcionInversion());
        }
        private void PickerExtraCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerExtraCategoria.SelectedIndex != -1)
            {
                StackByDefault2.IsVisible = false;
                if (PickerExtraCategoria.SelectedIndex == 0)
                {
                    StackExtraObligatoria.IsVisible = false;
                    StackExtraBeneficiosa.IsVisible = true;
                }
                if (PickerExtraCategoria.SelectedIndex == 1)
                {
                    StackExtraBeneficiosa.IsVisible = false;
                    StackExtraObligatoria.IsVisible = true;
                }
            }
        }
        private async void PickerFactorTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerFactorTipo.SelectedIndex == 0)
            {
                MateriasPrimasSelected();
                await Navigation.PushPopupAsync(new popupDuracionStock());
            }
            else
            {
                EntryNombreInversion.IsVisible = true;
                PickerMatPriSujerencias.IsVisible = false;
                flechita.IsVisible = false;
            }
        }
        private async void MateriasPrimasSelected()
        {   
            EntryNombreInversion.IsVisible = true;
            var matPri = await App.Database.GetByPrecUniVacioMateriaPrima();
            PickerMatPriSujerencias.Items.Clear();
            matPri.ForEach(x => PickerMatPriSujerencias.Items.Add(x.Nombre));
            if(PickerMatPriSujerencias.Items.Count > 0)
            {
                PickerMatPriSujerencias.IsVisible = true;
                flechita.IsVisible = true;
            }           
        }
        private async void PickerFactorTipoConstante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PickerFactorTipoConstante.SelectedIndex == 2)
            {
                await Navigation.PushPopupAsync(new popupDuracionStock());
                MateriasPrimasSelected();
            }
            else
            {
                EntryNombreInversion.IsVisible = true;
                PickerMatPriSujerencias.IsVisible = false;
                flechita.IsVisible = false;
            }
        }
        private async void PickerConstSubCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(PickerConstSubCategoria.SelectedIndex == 1)
            {
                await Navigation.PushPopupAsync(new popupConstDepend());
            }
        }

        //----------------------------------Page 3--------------------------------------


        private void ByDefaultPage3()
        {
            labelNoHayPage3.IsVisible = false;
        }
        public static int botonSeleccionado = 1;
        public static int pickerSeleccionado = 0;
        private async void Page3Inversion_Clicked(object sender, EventArgs e)
        {
            ViewPage3Inv.IsVisible = true;
            ViewPage3Reg.IsVisible = false;
            if (PickerPage3Deno.SelectedIndex == 0) MostrarTablaPage3("InvC");
            if (PickerPage3Deno.SelectedIndex == 1) MostrarTablaPage3("InvA");
            if (PickerPage3Deno.SelectedIndex == 2) MostrarTablaPage3("InvE");
            botonSeleccionado = 1;
            Page3Inversion.TextColor = Color.Green;
            Page3Registro.TextColor = Color.Black;
            popupSubAlert.TextSubAlert = "Advertencia: Si cambia algún parámetro de un tipo de inversión cambiará la información de todos los registros de inversiones de ese elemento. Se recomienda crear un nuevo elemento.";
            await Navigation.PushPopupAsync(new popupSubAlert());
        }
        private void Page3Registro_Clicked(object sender, EventArgs e)
        {
            ViewPage3Inv.IsVisible = false;
            ViewPage3Reg.IsVisible = true;
            if (PickerPage3Deno.SelectedIndex == 0) MostrarTablaPage3("RegC");
            if (PickerPage3Deno.SelectedIndex == 1) MostrarTablaPage3("RegA");
            if (PickerPage3Deno.SelectedIndex == 2) MostrarTablaPage3("RegE");
            botonSeleccionado = 2;
            Page3Inversion.TextColor = Color.Black;
            Page3Registro.TextColor = Color.Green;
        }
        private void PickerPage3Deno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewPage3Inv.IsVisible = false;
            ViewPage3Reg.IsVisible = false;
            if (botonSeleccionado == 1)
            {
                ViewPage3Inv.IsVisible = true;
                ViewPage3Reg.IsVisible = false;
                if (PickerPage3Deno.SelectedIndex == 0) MostrarTablaPage3("InvC");
                if (PickerPage3Deno.SelectedIndex == 1) MostrarTablaPage3("InvA");
                if (PickerPage3Deno.SelectedIndex == 2) MostrarTablaPage3("InvE");
            }
            if (botonSeleccionado == 2)
            {
                ViewPage3Inv.IsVisible = false;
                ViewPage3Reg.IsVisible = true;                
                if (PickerPage3Deno.SelectedIndex == 0) MostrarTablaPage3("RegC");
                if (PickerPage3Deno.SelectedIndex == 1) MostrarTablaPage3("RegA");
                if (PickerPage3Deno.SelectedIndex == 2) MostrarTablaPage3("RegE");
            }
        }
        private async void MostrarTablaPage3(string tabla)
        {
            if(tabla == "InvC")
            {
                var viewInversionC = await App.Database.GetInvConstante();
                if (viewInversionC != null)
                {
                    labelNoHayPage3.IsVisible = false;
                    viewInversionC.ForEach(x => x.Descripcion = ReducirText(x.Descripcion));
                    var viewInversionc = viewInversionC.OrderByDescending(x => x.Fecha);
                    ViewPage3Inv.ItemsSource = viewInversionc;
                }
                if (viewInversionC.Count == 0)
                {
                    ViewPage3Inv.IsVisible = false;
                    labelNoHayPage3.Text = "No ha creado inversiones constantes";
                    labelNoHayPage3.IsVisible = true;
                }
            }            
            if (tabla == "InvA")
            {
                var viewInversionA = await App.Database.GetInvAcumulativa();
                if (viewInversionA != null)
                {
                    labelNoHayPage3.IsVisible = false;
                    viewInversionA.ForEach(x => x.Descripcion = ReducirText(x.Descripcion));
                    var viewInversiona = viewInversionA.OrderByDescending(x => x.Fecha);
                    ViewPage3Inv.ItemsSource = viewInversiona;
                }
                if (viewInversionA.Count == 0)
                {
                    ViewPage3Inv.IsVisible = false;
                    labelNoHayPage3.Text = "No ha creado inversiones acumulativas";
                    labelNoHayPage3.IsVisible = true;
                }
            }
            if (tabla == "InvE")
            {
                var viewInversionE = await App.Database.GetInvExtraordinaria();
                if (viewInversionE != null)
                {
                    labelNoHayPage3.IsVisible = false;
                    viewInversionE.ForEach(x => x.Descripcion = ReducirText(x.Descripcion));
                    var viewInversione = viewInversionE.OrderByDescending(x => x.Fecha);
                    ViewPage3Inv.ItemsSource = viewInversione;
                }
                if(viewInversionE.Count == 0)
                {
                    ViewPage3Inv.IsVisible = false;
                    labelNoHayPage3.Text = "No ha creado inversiones extraordinarias";
                    labelNoHayPage3.IsVisible = true;
                }
            }
            if (tabla == "RegC")
            {
                var regC = await App.Database.GetRegConstante();
                if(regC != null)
                {
                    labelNoHayPage3.IsVisible = false;
                    var regc = regC.OrderByDescending(x => x.Fecha);
                    ViewPage3Reg.ItemsSource = regc;
                }
                if (regC.Count == 0)
                {
                    ViewPage3Reg.IsVisible = false;
                    labelNoHayPage3.Text = "No ha declarado reportes de inversiones";
                    labelNoHayPage3.IsVisible = true;
                }
            }
            if (tabla == "RegA")
            {
                labelNoHayPage3.IsVisible = false;
                var regA = await App.Database.GetRegAcumulativa();
                var rega = regA.OrderByDescending(x => x.Fecha);
                ViewPage3Reg.ItemsSource = rega;
                if (regA.Count == 0)
                {
                    ViewPage3Reg.IsVisible = false;
                    labelNoHayPage3.Text = "No ha declarado reportes de inversiones";
                    labelNoHayPage3.IsVisible = true;
                }
            }
            if (tabla == "RegE")
            {
                labelNoHayPage3.IsVisible = false;
                var regE = await App.Database.GetRegExtraordinaria();
                var rege = regE.OrderByDescending(x => x.Fecha);
                ViewPage3Reg.ItemsSource = rege;
                if (regE.Count == 0)
                {
                    ViewPage3Reg.IsVisible = false;
                    labelNoHayPage3.Text = "No ha declarado reportes de inversiones";
                    labelNoHayPage3.IsVisible = true;
                }
            }
        }
        public static string ReducirText(string text)
        {
            if (text != null)
            {
                text = text.Substring(0, text.Length > 0 && text.Length > 60 ? 60 : text.Length);
                text = text.Length == 60 ? text + "..." : text.Length == 0 ? "..." : text;
                return text;
            }
            else return "";
        }
        private async void ButtonEliminarInv_Clicked(object sender, EventArgs e)
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();

            if (PickerPage3Deno.SelectedIndex != -1 && botonSeleccionado != 0 && selectedAny)
            {
                bool hecho = false;
                if (botonSeleccionado == 1)
                {
                    if (PickerPage3Deno.SelectedIndex == 0)
                    {
                        var res = await DisplayAlert("ELIMINAR INVERSIÓN", "¿Está seguro de que desea eliminar este tipo de inversión?.", "ACEPTAR", "CANCELAR");
                        if (res)
                        {
                            if (page3InObjC.Tipo == TiposConstante.MateriaPrima)
                            {
                                var elemBorrar = await App.Database.GetIdInvCStock(page3InObjC.Id);
                                await App.Database.DeleteStock(elemBorrar[0]);
                            }
                            await App.Database.DeleteInvConstante(page3InObjC);
                            hecho = true;
                            MostrarTablaPage3("InvC");
                        }
                    }
                    if (PickerPage3Deno.SelectedIndex == 1)
                    {
                        bool sepuede = false;
                        var registros = await App.Database.GetIdInvRegAcumulativa(page3InObjA.Id);
                        if (registros.Count == 0) sepuede = true;
                        else
                        {
                            if (registros[0].Fecha.DayOfYear == TimeActual.DayOfYear && registros[0].Fecha.Year == TimeActual.Year) sepuede = true;
                            else
                            {
                                PopupAlert.PopupLabelTitulo = "ERROR";
                                PopupAlert.PopupLabelText = "Esta inversión tiene registros guardados, para eliminar un tipo de inversión no puede tener registros de más de un día de realizado.";
                                await Navigation.PushPopupAsync(new PopupAlert());
                            }  
                        }
                        if (sepuede)
                        {
                            var res = await DisplayAlert("ELIMINAR INVERSIÓN", "¿Está seguro de que desea eliminar este tipo de inversión?.", "ACEPTAR", "CANCELAR");
                            if (res)
                            {
                                foreach (var item in registros)
                                {
                                    var del = await App.Database.GetByIdRegAcumCaducidad(item.Id);
                                    await App.Database.DeleteCaducidad(del[0]);
                                    await App.Database.DeleteRegAcumulativa(item);
                                }
                                if (page3InObjA.Tipo == TiposAcumulativa.MateriaPrima)
                                {
                                    var elemBorrar = await App.Database.GetIdInvAStock(page3InObjA.Id);
                                    await App.Database.DeleteStock(elemBorrar[0]);
                                }
                                await App.Database.DeleteInvAcumulativa(page3InObjA);
                                hecho = true;
                                MostrarTablaPage3("InvA");
                            }
                        }
                    }
                    if (PickerPage3Deno.SelectedIndex == 2)
                    {
                        bool sepuede = false;
                        var registros = await App.Database.GetIdInvRegExtraordinaria(page3InObjE.Id);
                        if (registros.Count == 0) sepuede = true;
                        else
                        {
                            if (registros[0].Fecha.DayOfYear == TimeActual.DayOfYear && registros[0].Fecha.Year == TimeActual.Year) sepuede = true;
                            else 
                            {
                                PopupAlert.PopupLabelTitulo = "ERROR";
                                PopupAlert.PopupLabelText = "Esta inversión tiene registros guardados, para eliminar un tipo de inversión no puede tener registros de más de un día de realizado.";
                                await Navigation.PushPopupAsync(new PopupAlert());
                            }
                        }
                        if (sepuede)
                        {
                            var res = await DisplayAlert("ELIMINAR INVERSIÓN", "¿Está seguro de que desea eliminar este tipo de inversión?.", "ACEPTAR", "CANCELAR");
                            if (res)
                            {
                                registros.ForEach(async x => await App.Database.DeleteRegExtraordinaria(x));
                                await App.Database.DeleteInvExtraordinaria(page3InObjE);
                                hecho = true;
                                MostrarTablaPage3("InvE");
                            }
                        }
                    }
                }
                if (botonSeleccionado == 2)
                {                 
                    if (PickerPage3Deno.SelectedIndex == 0 && TimeActual.DayOfYear == page3ReObjC.Fecha.DayOfYear && TimeActual.Year == page3ReObjC.Fecha.Year)
                    {
                        var res = await DisplayAlert("ELIMINAR INVERSIÓN", "¿Está seguro de que desea eliminar este tipo de inversión?.", "ACEPTAR", "CANCELAR");
                        if (res)
                        { 
                            await App.Database.DeleteRegConstante(page3ReObjC);
                            var del = await App.Database.GetByIdRegConsCaducidad(page3ReObjC.Id);
                            await App.Database.DeleteCaducidad(del[0]);
                            hecho = true;
                            MostrarTablaPage3("RegC");
                        }
                    }
                    else if (PickerPage3Deno.SelectedIndex == 1 && TimeActual.DayOfYear == page3ReObjA.Fecha.DayOfYear && TimeActual.Year == page3ReObjA.Fecha.Year)
                    {
                        var res = await DisplayAlert("ELIMINAR INVERSIÓN", "¿Está seguro de que desea eliminar este tipo de inversión?.", "ACEPTAR", "CANCELAR");
                        if (res)
                        {
                            await App.Database.DeleteRegAcumulativa(page3ReObjA);
                            var del = await App.Database.GetByIdRegAcumCaducidad(page3ReObjA.Id);
                            await App.Database.DeleteCaducidad(del[0]);
                            hecho = true;
                            MostrarTablaPage3("RegA");
                        }
                    }
                    else if (PickerPage3Deno.SelectedIndex == 2 && TimeActual.DayOfYear == page3ReObjE.Fecha.DayOfYear && TimeActual.Year == page3ReObjE.Fecha.Year)
                    {
                        var res = await DisplayAlert("ELIMINAR INVERSIÓN", "¿Está seguro de que desea eliminar este tipo de inversión?.", "ACEPTAR", "CANCELAR");
                        if (res)
                        {
                            await App.Database.DeleteRegExtraordinaria(page3ReObjE);
                            hecho = true;
                            MostrarTablaPage3("RegE");
                        }
                    }
                    else
                    {
                        PopupAlert.PopupLabelTitulo = "ERROR";
                        PopupAlert.PopupLabelText = "Este registro ya se ha guardado, para eliminar un registros debe haberse creados hoy.";
                        await Navigation.PushPopupAsync(new PopupAlert());
                    }                   
                }
                if (hecho)
                {
                    PopupAlert.PopupLabelTitulo = "ELEMENTO ELIMINADO";
                    PopupAlert.PopupLabelText = "El elemento se eliminó correctamente.";
                    await Navigation.PushPopupAsync(new PopupAlert());
                }
            }
            else {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe elegir entre las inversiones y los registros, y seleccionar una denominación. Podrá ver la lista correspondiente y eliminar el elemento que desee.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
        }
        private async void ButtonActualizarInv_Clicked(object sender, EventArgs e)
        {
            if (PickerPage3Deno.SelectedIndex != -1 && botonSeleccionado != 0 && selectedAny)
            {
                bool sepuede = false;
                if (botonSeleccionado == 1)
                {
                    if (PickerPage3Deno.SelectedIndex == 0) pickerSeleccionado = 1;
                    if (PickerPage3Deno.SelectedIndex == 1) pickerSeleccionado = 2;
                    if (PickerPage3Deno.SelectedIndex == 2) pickerSeleccionado = 3;
                    
                    sepuede = true;
                }
                if (botonSeleccionado == 2)
                {
                    DateTime TimeActual = DateTime.Now;
                    TimeActual = TimeActual.ToLocalTime();
                    if (PickerPage3Deno.SelectedIndex == 0 && TimeActual.DayOfYear == page3ReObjC.Fecha.DayOfYear && TimeActual.Year == page3ReObjC.Fecha.Year)
                    { pickerSeleccionado = 1; sepuede = true;}
                    else if (PickerPage3Deno.SelectedIndex == 1 && TimeActual.DayOfYear == page3ReObjA.Fecha.DayOfYear && TimeActual.Year == page3ReObjA.Fecha.Year)
                    {pickerSeleccionado = 2; sepuede = true; }
                    else if (PickerPage3Deno.SelectedIndex == 2 && TimeActual.DayOfYear == page3ReObjE.Fecha.DayOfYear && TimeActual.Year == page3ReObjE.Fecha.Year)
                    { pickerSeleccionado = 3; sepuede = true;}
                    else
                    {
                        PopupAlert.PopupLabelTitulo = "ERROR";
                        PopupAlert.PopupLabelText = "Este reporte ya quedó registrado, no es posible hacer cambios sobre él. Solo se puede hacer cambios en reportes del día de hoy.";
                        await Navigation.PushPopupAsync(new PopupAlert());
                    }
                }
                if (sepuede)
                { 
                    await Navigation.PushPopupAsync(new UpdateInverciones());
                    MessagingCenter.Subscribe<UpdateInverciones, string>(this, "UpdateInverciones", async (s, arg) =>
                    {
                        if (botonSeleccionado == 1)
                        {
                            ViewPage3Inv.IsVisible = true;
                            ViewPage3Reg.IsVisible = false;
                            if (PickerPage3Deno.SelectedIndex == 0) MostrarTablaPage3("InvC");
                            if (PickerPage3Deno.SelectedIndex == 1) MostrarTablaPage3("InvA");
                            if (PickerPage3Deno.SelectedIndex == 2) MostrarTablaPage3("InvE");
                        }
                        if (botonSeleccionado == 2)
                        {
                            ViewPage3Inv.IsVisible = false;
                            ViewPage3Reg.IsVisible = true;
                            if (PickerPage3Deno.SelectedIndex == 0) MostrarTablaPage3("RegC");
                            if (PickerPage3Deno.SelectedIndex == 1) MostrarTablaPage3("RegA");
                            if (PickerPage3Deno.SelectedIndex == 2) MostrarTablaPage3("RegE");
                        }
                    });
                }
            }
            else {
                PopupAlert.PopupLabelTitulo = "ERROR";
                PopupAlert.PopupLabelText = "Debe elegir entre las inversiones y los registros, y seleccionar una denominación. Podrá ver la lista corespondiente y modificar los elementos.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
        }
        public static InConstante page3InObjC = new InConstante();
        public static InAcumulativa page3InObjA = new InAcumulativa();
        public static InExtraordinaria page3InObjE = new InExtraordinaria();
        public static ReConstante page3ReObjC = new ReConstante();
        public static ReAcumulativa page3ReObjA = new ReAcumulativa();
        public static ReExtraordinaria page3ReObjE = new ReExtraordinaria();
        public static bool selectedAny = false;
        private async void ViewPage3Inv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(PickerPage3Deno.SelectedIndex == 0)
            {
                page3InObjC = (InConstante)e.SelectedItem;
                popupMostrarInfoInv.tipeSelected = 1;
                await Navigation.PushPopupAsync(new popupMostrarInfoInv());
                
            }
            if (PickerPage3Deno.SelectedIndex == 1)
            {
                page3InObjA = (InAcumulativa)e.SelectedItem;
                popupMostrarInfoInv.tipeSelected = 2;
                PopupAlert.PopupLabelTitulo = "Detalles de la inversión Acumulativa";
                PopupAlert.PopupLabelText = $"TIPO: {page3InObjA.Tipo} \n";
                if (!string.IsNullOrEmpty(page3InObjA.Descripcion)) PopupAlert.PopupLabelText += $"Descripción:{page3InObjA.Descripcion}";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            if (PickerPage3Deno.SelectedIndex == 2)
            {
                page3InObjE = (InExtraordinaria)e.SelectedItem;
                popupMostrarInfoInv.tipeSelected = 3;
                PopupAlert.PopupLabelTitulo = "Detalles de la inversión Extraordinaria";
                PopupAlert.PopupLabelText = $"CATEGORÍA: {page3InObjE.Categoria} \nTIPO: {page3InObjE.Tipo} \n";
                if (!string.IsNullOrEmpty(page3InObjE.Descripcion)) PopupAlert.PopupLabelText += $"Descripción:{page3InObjE.Descripcion}";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            selectedAny = true;
        }
        private async void ViewPage3Reg_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (PickerPage3Deno.SelectedIndex == 0)
            {
                page3ReObjC = (ReConstante)e.SelectedItem;
                var obj = await App.Database.GetIdInvConstante(page3ReObjC.IdInv);
                PopupAlert.PopupLabelTitulo = "DETALLES DE LA INVERSIÓN CONSTANTE";
                PopupAlert.PopupLabelText = $"NOMBRE: {obj.Nombre} \nFRECUENCIA: {obj.Frecuencia} \nCATEGORÍA: {obj.Categoria} \nTIPO: {obj.Tipo} \nDESCRIPCIÓN: {obj.Descripcion}";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            if (PickerPage3Deno.SelectedIndex == 1)
            {
                page3ReObjA = (ReAcumulativa)e.SelectedItem;
                var obj = await App.Database.GetIdInvAcumulativa(page3ReObjA.IdInv);
                PopupAlert.PopupLabelTitulo = "DETALLES DE LA INVERSIÓN ACUMULATIVA";
                PopupAlert.PopupLabelText = $"NOMBRE: {obj.Nombre} \nTIPO: {obj.Tipo} \nDESCRIPCIÓN: {obj.Descripcion}";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            if (PickerPage3Deno.SelectedIndex == 2)
            {
                page3ReObjE = (ReExtraordinaria)e.SelectedItem;
                var obj = await App.Database.GetIdInvExtraordinaria(page3ReObjE.IdInv);
                PopupAlert.PopupLabelTitulo = "DETALLES DEL TIPO DE INVERSIÓN";
                PopupAlert.PopupLabelText = $"NOMBRE: {obj.Nombre} \nCATEGORÍA: {obj.Categoria} \nTIPO: {obj.Tipo} \nDESCRIPCIÓN: {obj.Descripcion}";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            selectedAny = true;
        }


        //----------------------------------Page 4--------------------------------------
        
        private async void ByDefaultPage4()
        {
            double deudaTotal = 0;
            ButtonActualizarDeuda.IsVisible = false;
            ButtonBorrarDeuda.IsVisible = false;
            ButtonPagarDeuda.IsVisible = false;
            var deudas = await App.Database.GetDeuda();
            foreach (var item in deudas)
            {
                if (item.Denominacion == "A")
                {InAcumulativa elem = await App.Database.GetIdInvAcumulativa(item.IdInv);
                item.Denominacion = elem.Nombre;}
                if (item.Denominacion == "C")
                {InConstante elem = await App.Database.GetIdInvConstante(item.IdInv);
                item.Denominacion = elem.Nombre;}
                if (item.Denominacion == "E")
                {InExtraordinaria elem = await App.Database.GetIdInvExtraordinaria(item.IdInv);
                item.Denominacion = elem.Nombre;}

                string restante = DiferenciaDeTiempo(item);
                if (item.Descripcion != null)
                {
                    item.Descripcion = item.Descripcion.Substring(0, item.Descripcion.Length > 0 && item.Descripcion.Length > 100 ? 100 : item.Descripcion.Length);
                    item.Descripcion = item.Descripcion.Length == 100 ? item.Descripcion + "..." : item.Descripcion;
                    item.Descripcion += $"\nTiempo restante: {restante}";
                } 
                else item.Descripcion = $"Tiempo restante: {restante}";
                item.Costo = item.Costo * item.Unidades;
                deudaTotal += item.Costo;
            }
            var deudasOrder = deudas.OrderBy(x => x.FechaFin);
            ListDeudas.ItemsSource = deudasOrder;
            DeudaTotal.Text = deudaTotal.ToString();
        }
        private string DiferenciaDeTiempo(Deudas item)
        {
            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            var timeSpan = item.FechaFin - TimeActual;
            if (timeSpan.Days > 0)
            {
                int dif = 0;
                string restante = item.FechaFin.Day > TimeActual.Day ? (item.FechaFin.Day - TimeActual.Day) > 1 ? Convert.ToString(item.FechaFin.Day - TimeActual.Day) + " días, " : "1 día" : TimeActual.Day > item.FechaFin.Day ? Convert.ToString(item.FechaFin.Day + 30 - TimeActual.Day) + " días, " : "";
                if (TimeActual.Day > item.FechaFin.Day) dif = 1;
                restante += item.FechaFin.Month - dif > TimeActual.Month ? (item.FechaFin.Month - dif - TimeActual.Month) > 1 ? Convert.ToString(item.FechaFin.Month - dif - TimeActual.Month) + " meses, " : "1 mes" : TimeActual.Month > item.FechaFin.Month - dif ? Convert.ToString(item.FechaFin.Month - dif + 12 - TimeActual.Month) + " meses, " : "";
                if (TimeActual.Month <= item.FechaFin.Month - dif) dif = 0;
                else dif = 1;
                restante += item.FechaFin.Year - dif > TimeActual.Year ? (item.FechaFin.Year - dif - TimeActual.Year) > 1 ? Convert.ToString(item.FechaFin.Year - dif - TimeActual.Year) + " años, " : "1 año" : "";
                return restante;
            }
            else return $"El plazo se venció hace {System.Math.Abs(timeSpan.Days)} días";
            
        }
        public static Deudas deudaActual = new Deudas();
        private async void ListDeudas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            deudaActual = (Deudas)e.SelectedItem;
            string restante = DiferenciaDeTiempo(deudaActual);

            DateTime TimeActual = DateTime.Now;
            TimeActual = TimeActual.ToLocalTime();
            string unidadtime = "día";
            int cantidadCadaTime = 0;
            int dias = deudaActual.FechaFin.Day + (30 * (deudaActual.FechaFin.Month + (12 * deudaActual.FechaFin.Year))) - (TimeActual.Day + (30 * (TimeActual.Month + (12 * TimeActual.Year))));
            int meses = 0;
            if (dias > 30)
            {
                meses = dias / 30;
                unidadtime = "mes";
                cantidadCadaTime = Convert.ToInt32(deudaActual.Costo / meses);
            }
            else if (dias > 0) cantidadCadaTime = Convert.ToInt32(deudaActual.Costo / dias);
            else cantidadCadaTime = Convert.ToInt32(deudaActual.Costo);

            var timeSpan = deudaActual.FechaFin - TimeActual;
            if (timeSpan.Days <= 0)
            {
                PopupAlert.PopupLabelTitulo = "SUGERENCIAS";
                PopupAlert.PopupLabelText = $"Esta deuda terminó su plazo de pago, se recomienda pagar cuanto antes.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            else
            {
                PopupAlert.PopupLabelTitulo = "SUGERENCIAS";
                PopupAlert.PopupLabelText = $"Para pagar esta deuda tiene un plazo de {restante}. Se le recomienda pagar {cantidadCadaTime}$ cada {unidadtime}.";
                await Navigation.PushPopupAsync(new PopupAlert());
            }
            ButtonActualizarDeuda.IsVisible = true;
            ButtonBorrarDeuda.IsVisible = true;
            ButtonPagarDeuda.IsVisible = true;
        }
        private async void ButtonActualizarDeuda_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new UpDateDeuda());
            MessagingCenter.Subscribe<UpDateDeuda, string>(this, "UpDateDeuda", async (s, arg) =>
            {
                ByDefaultPage4();
            });
        }
        private async void ButtonBorrarDeuda_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("BORRAR REGISTRO DE DEUDA","¿Está seguro de que desea eliminar este registro?","ACEPTAR","CANCELAR");
            if (res)
            {
                await App.Database.DeleteDeuda(deudaActual);
            }
            ByDefaultPage4();
        }
        private async void ButtonPagarDeuda_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new PagarDeuda());
            MessagingCenter.Subscribe<PagarDeuda, string>(this, "PagarDeuda", async (s, arg) =>
            {
                ByDefaultPage4();
            });
        }
        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            ByDefaultPage4();
            ByDefaultPage1();
            ByDefaultPage2();
        }  
    }
}