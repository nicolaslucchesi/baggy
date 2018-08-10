using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.IO;
using SQLite;
using MobileExample.Tables;
using MobileExample.ViewModels;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoElemento : ContentPage
    {
        public ElementoViewModel ElementoViewModel { get; set; }
        
        public String Imagen;
        public String UUID;

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
            
            // Este es el mensaje que llega cuando se elige una Imagen desde el pop-up.
            // Se reemplaza la ruta del icono de ElementoViewModel por la ruta del icono que se seleccionó
            // Después hago un InitializeComponent para refrescar la pagina con la nueva ruta.
            MessagingCenter.Subscribe<Popup, ImagenElementoViewModel>(this, "SeleccionarImagen", (obj, imagenelementoViewModel) =>
            {
                ElementoViewModel.RutaIcono = imagenelementoViewModel.RutaIcono;
                //ImagenImagen.Source = imagenelementoViewModel.RutaIcono; 
            });

            MessagingCenter.Subscribe<VincularElemento, ImagenElementoViewModel>(this, "VincularElemento", (obj, imagenelementoViewModel) =>
            {
               // Acá va el código 
            });


        }

        async void GuardarElemento_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            MessagingCenter.Send(this, "AgregarElemento", ElementoViewModel);
            await Navigation.PopModalAsync();
        }

        async void VincularElemento_Clicked(object sender, EventArgs e)
        {
            var propertiedPopup = new VincularElemento();

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

            propertiedPopup.Animation = scaleAnimation;
            propertiedPopup.CloseWhenBackgroundIsClicked = true;

            await PopupNavigation.PushAsync(propertiedPopup);
        }

        async void AbrirPopupImagen(object sender, EventArgs e)
        {
            bool a = true;
            var propertiedPopup = new Popup(a);

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

            propertiedPopup.Animation = scaleAnimation; 
            propertiedPopup.CloseWhenBackgroundIsClicked = true;
            
            await PopupNavigation.PushAsync(propertiedPopup);
        }

        
    }
}