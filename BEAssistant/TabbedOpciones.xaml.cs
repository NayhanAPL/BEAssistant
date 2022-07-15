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
    public partial class TabbedOpciones : TabbedPage
    {
        public TabbedOpciones()
        {
            InitializeComponent();
            ByDefault();
        }

        private async void ByDefault()
        {
            var im = await App.Database.GetByNameOpciones("");
            SwitchInventarioManual.IsToggled = im[0].Activo;
        }

        private async void stockManual_Toggled(object sender, ToggledEventArgs e)
        {
            bool estado = e.Value;
            var im = await App.Database.GetByNameOpciones("");
            Opciones up = new Opciones
            {
                Id = im[0].Id,
                Activo = estado,
                Nombre = "InventarioManual"
            }; await App.Database.SaveUpOpciones(up);
        }
    }
}