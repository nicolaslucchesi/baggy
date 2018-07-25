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
    public partial class NuevaMochila : ContentPage
    {
        public MochilaViewModel MochilaViewModel { get; set; }

        public NuevaMochila()
        {
            InitializeComponent();

            MochilaViewModel = new MochilaViewModel
            {
                Descripcion = "Descripción de la mochila.",
                UUID = "Código único de la mochila."
            };

            BindingContext = this;
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            MessagingCenter.Send(this, "AgregarMochila", MochilaViewModel);
            await Navigation.PopModalAsync();
        }

    }
}