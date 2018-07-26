using System;
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
    public partial class NuevoElemento : ContentPage
    {
        public ElementoViewModel ElementoViewModel { get; set; }
        public String Imagen; 

        public NuevoElemento()
        {
            InitializeComponent();

            ElementoViewModel = new ElementoViewModel
            {
                Descripcion = "Nuevo Elemento",
                RutaIcono = "BaggyLogo.jpg",
                Imprescindible = true,
                Vinculado = true
            };
            
            BindingContext = this;
        }

        async void GuardarElemento_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            MessagingCenter.Send(this, "AgregarElemento", ElementoViewModel);
            await Navigation.PopModalAsync();
        }
        
    }
}