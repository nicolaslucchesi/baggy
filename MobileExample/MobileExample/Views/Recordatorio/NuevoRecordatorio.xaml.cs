﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using System.IO;
using SQLite;
using MobileExample.Tables;
using MobileExample.ViewModels;



namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoRecordatorio : ContentPage
    {
        public RecordatorioViewModel RecordatorioViewModel { get; set; }

       
        public NuevoRecordatorio()
        {
            InitializeComponent();

            RecordatorioViewModel = new RecordatorioViewModel
            {
                DiaSemana = 1,
                Horario = new TimeSpan(12,0,0),
                Hora = 0,
                Minuto = 3,
            };

            BindingContext = this;
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            MessagingCenter.Send(this, "AgregarRecordatorio", RecordatorioViewModel);
            await Navigation.PopModalAsync();
        }


        async void ApretarBotonLunes(object sender, EventArgs e)
        {
            if (SwitchLunes.IsToggled)
            {
                SwitchLunes.IsToggled = false;
            }
            else
            {
                SwitchLunes.IsToggled = true;
            }
        }

        async void ApretarBotonMartes(object sender, EventArgs e)
        {
            if (SwitchMartes.IsToggled)
            {
                SwitchMartes.IsToggled = false;
            }
            else
            {
                SwitchMartes.IsToggled = true;
            }
        }

        async void ApretarBotonMiercoles(object sender, EventArgs e)
        {
            if (SwitchMiercoles.IsToggled)
            {
                SwitchMiercoles.IsToggled = false;
            }
            else
            {
                SwitchMiercoles.IsToggled = true;
            }
        }
        async void ApretarBotonJueves(object sender, EventArgs e)
        {
            if (SwitchJueves.IsToggled)
            {
                SwitchJueves.IsToggled = false;
            }
            else
            {
                SwitchJueves.IsToggled = true;
            }
        }
        async void ApretarBotonViernes(object sender, EventArgs e)
        {
            if (SwitchViernes.IsToggled)
            {
                SwitchViernes.IsToggled = false;
            }
            else
            {
                SwitchViernes.IsToggled = true;
            }
        }

    }
}