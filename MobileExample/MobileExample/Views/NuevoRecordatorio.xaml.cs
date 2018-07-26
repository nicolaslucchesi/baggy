using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Models;
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
                Hora = 2,
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

    }
}