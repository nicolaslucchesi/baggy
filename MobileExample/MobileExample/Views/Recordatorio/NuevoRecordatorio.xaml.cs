using MobileExample.Database;
using MobileExample.Tables;
using MobileExample.ViewModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoRecordatorio : ContentPage
    {
        public RecordatorioViewModel RecordatorioViewModel { get; set; }
        public MochilaViewModel Mochila { get; set; }

        public NuevoRecordatorio()
        {
            InitializeComponent();

            RecordatorioViewModel = new RecordatorioViewModel();

            ObtenerElementos();

            BindingContext = this;
        }

        private void ObtenerElementos()
        {
            RecordatorioViewModel.Elementos.Elementos = ObtenerListadoElementos();
        }

        private List<ElementoViewModel> ObtenerListadoElementos()
        {
            List<ElementoViewModel> listadoElementos = new List<ElementoViewModel>();
            int cantidad = 0;
            foreach (Elemento elemento in DatabaseHelper.db.Table<Elemento>().ToList())
            {
                ElementoViewModel elementoViewModel = new ElementoViewModel();
                elementoViewModel.Imprescindible = elemento.Imprescindible;
                elementoViewModel.Seleccionado = elemento.Imprescindible ? true : false;
                elementoViewModel.RutaIcono = elemento.RutaIcono;
                elementoViewModel.Descripcion = elemento.Descripcion;
                elementoViewModel.Vinculado = elemento.Vinculado;
                elementoViewModel.UUID = elemento.UUID;
                elementoViewModel.Id = elemento.Id;
                elementoViewModel.IdInterno = cantidad;
                cantidad++;
                listadoElementos.Add(elementoViewModel);
            }

            return listadoElementos.OrderBy(e => e.Imprescindible).ToList();
        }

        public async void GuardarRecordatorio(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            this.MapearDatos();

            if (!ValidarRecordatorio())
            {
                await DisplayAlert("Error de validación", "No se han completado todos los campos obligatorios.", "Aceptar");
            }
            else
            {
                MessagingCenter.Send(this, "AgregarRecordatorio", RecordatorioViewModel);
                await Navigation.PopModalAsync();
            }
        }

        async void AbrirPopupSeleccionarElementos(object sender, EventArgs e)
        {
            var PopupElementos = new SeleccionarElementosPopup(RecordatorioViewModel.Elementos);

            PopupElementos.CloseWhenBackgroundIsClicked = false;

            await PopupNavigation.PushAsync(PopupElementos);
        }

        public void ApretarBotonDia(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            switch (boton.Text)
            {
                case "Lu":
                    SwitchLunes.IsToggled = !SwitchLunes.IsToggled;
                    break;
                case "Ma":
                    SwitchMartes.IsToggled = !SwitchMartes.IsToggled;
                    break;
                case "Mi":
                    SwitchMiercoles.IsToggled = !SwitchMiercoles.IsToggled;
                    break;
                case "Ju":
                    SwitchJueves.IsToggled = !SwitchJueves.IsToggled;
                    break;
                case "Vi":
                    SwitchViernes.IsToggled = !SwitchViernes.IsToggled;
                    break;
                case "Sa":
                    SwitchSabado.IsToggled = !SwitchSabado.IsToggled;
                    break;
                case "Do":
                    SwitchDomingo.IsToggled = !SwitchDomingo.IsToggled;
                    break;
                default:
                    break;
            }
        }

        private void MapearDatos()
        {
            RecordatorioViewModel.HorarioStr = Tiempo.Time.ToString(@"hh\:mm");
        }

       


        private bool ValidarRecordatorio()
        {
            if (string.IsNullOrEmpty(RecordatorioViewModel.HorarioStr))
            {
                return false;
            }

            if (!RecordatorioViewModel.Elementos.Elementos.Any(e => e.Seleccionado))
            {
                return false;
            }
            if (!RecordatorioViewModel.Lunes
              && !RecordatorioViewModel.Martes
              && !RecordatorioViewModel.Miercoles
              && !RecordatorioViewModel.Jueves
              && !RecordatorioViewModel.Viernes
              && !RecordatorioViewModel.Sabado
              && !RecordatorioViewModel.Domingo)
            {
                return false;
            }

            return true;
        }
    }
}