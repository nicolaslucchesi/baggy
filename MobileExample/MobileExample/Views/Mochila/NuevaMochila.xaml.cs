using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                Descripcion = "",
                UUID = ""
            };

            BindingContext = this;
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            if (String.IsNullOrEmpty(MochilaViewModel.Descripcion))
            {
                await DisplayAlert("Error de validación", "El campo 'Descripción' es requerido.", "Aceptar");
            }
            else
            {
                MessagingCenter.Send(this, "AgregarMochila", MochilaViewModel);
                await Navigation.PopModalAsync();
            }
        }

    }
}