using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using System.IO;
using SQLite;
using MobileExample.Tables;
using MobileExample.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using MobileExample.Database;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoRecordatorio : ContentPage
    {
        public RecordatorioViewModel recordatorioViewModel { get; set; }
        public MochilaViewModel Mochila { get; set; }

        public NuevoRecordatorio()
        {
            InitializeComponent();

            recordatorioViewModel = new RecordatorioViewModel
            {
                Horario = new TimeSpan(12, 0, 0),
                Elementos = new ListadoElementosRecordatorioViewModel(),
                MochilaSeleccionada = new MochilaViewModel()
            };

            ObtenerElementos();
            //ListadoMochila = new ListadoMochilasViewModel();

            BindingContext = this;

            MessagingCenter.Subscribe<SeleccionarMochilasPopup, MochilaViewModel>(this, "SeleccionarMochila", (sender, mochilaViewModel) =>
            {
                recordatorioViewModel.MochilaSeleccionada = Mochila = mochilaViewModel;
            });
        }

        private void ObtenerElementos()
        {
            recordatorioViewModel.Elementos.Elementos = ObtenerListadoElementos();
        }

        private List<ElementoViewModel> ObtenerListadoElementos()
        {
            List<ElementoViewModel> listadoElementos = new List<ElementoViewModel>();
            int cantidad = 0;
            foreach (Elemento elemento in DatabaseHelper.db.Table<Elemento>().ToList())
            {
                ElementoViewModel elementoViewModel = new ElementoViewModel();
                elementoViewModel.Imprescindible = elemento.Imprescindible;
                elementoViewModel.RutaIcono = elemento.RutaIcono;
                elementoViewModel.Descripcion = elemento.Descripcion;
                elementoViewModel.Vinculado = elemento.Vinculado;
                elementoViewModel.UUID = elemento.UUID;
                elementoViewModel.Id = elemento.Id;
                elementoViewModel.IdInterno = cantidad;
                cantidad++;
                listadoElementos.Add(elementoViewModel);
            }

            return listadoElementos;
        }

        public async void GuardarRecordatorio(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.

            // TODO: Revisar porqué no se mapea el timeSpan directamente.
            this.MapearDatos();

            MessagingCenter.Send(this, "AgregarRecordatorio", recordatorioViewModel);
            await Navigation.PopModalAsync();
        }

        async void AbrirPopupSeleccionarElementos(object sender, EventArgs e)
        {
            var PopupElementos = new SeleccionarElementosPopup(recordatorioViewModel.Elementos);

            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Top,
                PositionOut = MoveAnimationOptions.Bottom,
                ScaleIn = 1.2,
                ScaleOut = 0.8,
                DurationIn = 400,
                DurationOut = 800,
                EasingIn = Easing.BounceIn,
                EasingOut = Easing.CubicOut,
                HasBackgroundAnimation = true
            };

            PopupElementos.Animation = scaleAnimation;
            PopupElementos.CloseWhenBackgroundIsClicked = false;

            await PopupNavigation.PushAsync(PopupElementos);
        }

        async void AbrirPopupSeleccionarMochila(object sender, EventArgs e)
        {
            bool a = true;
            var PopupMochilas = new SeleccionarMochilasPopup(recordatorioViewModel.MochilaSeleccionada);

            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Top,
                PositionOut = MoveAnimationOptions.Bottom,
                ScaleIn = 1.2,
                ScaleOut = 0.8,
                DurationIn = 400,
                DurationOut = 800,
                EasingIn = Easing.BounceIn,
                EasingOut = Easing.CubicOut,
                HasBackgroundAnimation = true
            };

            PopupMochilas.Animation = scaleAnimation;
            PopupMochilas.CloseWhenBackgroundIsClicked = false;

            await PopupNavigation.PushAsync(PopupMochilas);
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
            recordatorioViewModel.Horario = Tiempo.Time;
            recordatorioViewModel.HorarioStr = Tiempo.Time.ToString(@"hh\:mm");
            recordatorioViewModel.Lunes = SwitchLunes.IsToggled;
            recordatorioViewModel.Martes = SwitchMartes.IsToggled;
            recordatorioViewModel.Miercoles = SwitchMiercoles.IsToggled;
            recordatorioViewModel.Jueves = SwitchJueves.IsToggled;
            recordatorioViewModel.Viernes = SwitchViernes.IsToggled;
            recordatorioViewModel.Sabado = SwitchSabado.IsToggled;
            recordatorioViewModel.Domingo = SwitchDomingo.IsToggled;
        }
    }
}