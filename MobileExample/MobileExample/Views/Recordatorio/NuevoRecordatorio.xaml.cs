﻿using System;
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

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoRecordatorio : ContentPage
    {
        public RecordatorioViewModel recordatorioViewModel { get; set; }

        public NuevoRecordatorio()
        {
            InitializeComponent();

            recordatorioViewModel = new RecordatorioViewModel
            {
                Horario = new TimeSpan(12, 0, 0),
                Elementos = new ListadoElementosViewModel()
            };

            BindingContext = this;
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
            bool a = true;
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
            PopupElementos.CloseWhenBackgroundIsClicked = true;

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

        }
    }
}