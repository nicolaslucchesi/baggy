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
using MobileExample.Database;

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

            var indicator = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy", BindingMode.OneWay);
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy", BindingMode.OneWay);

            ElementoViewModel = new ElementoViewModel
            {
                Descripcion = "Nuevo Elemento",
                RutaIcono = "AgregarObjeto.png",
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
                ImagenElemento.Source = imagenelementoViewModel.RutaIcono;
            });

            MessagingCenter.Subscribe<VincularElemento, bool>(this, "VincularElemento", (obj, vinculado) =>
            {
                // Acá va el código 
                ElementoViewModel.Vinculado = vinculado;
            });


        }

        async void GuardarElemento_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            if (ElementoViewModel.RutaIcono == "AgregarObjeto.png")
            {
                //Mensaje de que hay que seleccionar una imagen
            }
            else
            {
                MessagingCenter.Send(this, "AgregarElemento", ElementoViewModel);
                await Navigation.PopModalAsync();
            }
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
            propertiedPopup.CloseWhenBackgroundIsClicked = false;

            // MANDAR SEÑAL A LA MOCHILA PARA QUE MANDE NUEVOS ELEMENTOS

            await PopupNavigation.PushAsync(propertiedPopup);
            ElementoViewModel.IsBusy = true;
            DateTime now = DateTime.Now;
            DatabaseHelper.db.DeleteAll<ElementoAgregado>();

            System.Threading.Thread.Sleep(100);

            do
            {
                if (DatabaseHelper.db.Table<ElementoAgregado>().Count() > 0)
                {
                    ElementoAgregado elementoAgregado = DatabaseHelper.db.Table<ElementoAgregado>().FirstOrDefault();
                    ElementoViewModel.UUID = elementoAgregado.UUID;
                    DatabaseHelper.db.DeleteAll<ElementoAgregado>();
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            } while ((DateTime.Now - now).TotalSeconds < 10);
            ElementoViewModel.IsBusy = false;
            
            // MANDAR SEÑAL A LA MOCHILA PARA QUE DEJE DE MANDAR NUEVOS ELEMENTOS

            await PopupNavigation.PopAsync();
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