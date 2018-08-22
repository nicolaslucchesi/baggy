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
            string mensaje = ValidarMochila();
            if (!string.IsNullOrEmpty(mensaje))
            {
                await DisplayAlert("Error de validación", mensaje, "Aceptar");
            }
            else
            {
                if (string.IsNullOrEmpty(MochilaViewModel.UUID))
                {
                    bool acepta = await DisplayAlert("Error de validación",
                                        "No has completado el código de la mochila. ¿Deseas guardar la mochila de todas formas?",
                                        "Aceptar",
                                        "Cancelar");
                    if (acepta)
                    {
                        MessagingCenter.Send(this, "AgregarMochila", MochilaViewModel);
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    MessagingCenter.Send(this, "AgregarMochila", MochilaViewModel);
                    await Navigation.PopModalAsync();
                }
            }
        }

        string ValidarMochila()
        {
            if (string.IsNullOrEmpty(MochilaViewModel.Descripcion))
            {
                return "La descripción de la mochila no fue completada.";
            }
            return string.Empty;
        }

    }
}