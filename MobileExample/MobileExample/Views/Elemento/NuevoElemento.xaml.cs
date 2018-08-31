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
using System.Threading.Tasks;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoElemento : ContentPage
    {
        public ElementoViewModel ElementoViewModel { get; set; }

        //public String Imagen;
        //public String UUID;

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
                Descripcion = "",
                RutaIcono = "AgregarObjeto.png",
                Imprescindible = true,
                Vinculado = false
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
            if (ElementoViewModel.RutaIcono == "AgregarObjeto.png"
                || string.IsNullOrEmpty(ElementoViewModel.Descripcion))
            {
                await DisplayAlert("Error de validación", "No se han completado todos los campos obligatorios", "Aceptar");
            }
            else if (!ElementoViewModel.Vinculado)
            {
                bool acepta = await DisplayAlert("No hay vínculo",
                    "No se ha vinculado ningún elemento. ¿Desea continuar de todas formas?",
                    "Aceptar",
                    "Cancelar");
                if (acepta)
                {
                    MessagingCenter.Send(this, "AgregarElemento", ElementoViewModel);
                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                MessagingCenter.Send(this, "AgregarElemento", ElementoViewModel);
                await Navigation.PopModalAsync();
            }
        }

        async void VincularElemento_Clicked(object sender, EventArgs e)
        {
            int cantidad = 1;
            var propertiedPopup = new VincularElemento();
            propertiedPopup.CloseWhenBackgroundIsClicked = false;

            await PopupNavigation.PushAsync(propertiedPopup);
            await Task.Delay(2000);
            EsperarElemento();

            if (!ElementoViewModel.Vinculado)
            {
                while (cantidad < 3)
                {
                    bool acepta = await DisplayAlert("Aviso", "No se ha registrado ningún elemento", "Intentar de nuevo", "Cancelar");
                    if (acepta)
                    {
                        await PopupNavigation.PopAsync();
                        propertiedPopup = new VincularElemento();
                        await PopupNavigation.PushAsync(propertiedPopup);
                        await Task.Delay(2000);
                        EsperarElemento();
                        cantidad += 1;
                        if (ElementoViewModel.Vinculado)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (cantidad == 3 && !ElementoViewModel.Vinculado)
                {
                    await DisplayAlert("Aviso",
                        "No se ha registrado " +
                        "ningún elemento. Asegura que no se haya perdido la conexión" +
                        " del smartphone con la mochila y " +
                        "vuelve a intentar.",
                        "Aceptar");
                }
                if (ElementoViewModel.Vinculado)
                {
                    await DisplayAlert("Éxito", "Registro Exitoso", "Aceptar");
                }
                await PopupNavigation.PopAsync();
            }
            else
            {
                await PopupNavigation.PopAsync();
                await DisplayAlert("Éxito", "Registro Exitoso", "Aceptar");
            }

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

        /// <summary>
        /// Este método lo que hace es esperar 10 segundos, chequeando
        /// cada 2 segundos (cinco chequeos) en la base de datos, a ver si en la tabla
        /// "ElementoAgregado" hay algún registro nuevo. Cabe destacar que el servicio que chequea el bluetooth,
        /// se encarga de verificar si el elemento agregado ya existe para no agregarlo de nuevo.
        /// Habría que ver si hace falta saberlo desde acá para tirar un alert o con que no haga nada
        /// es suficiente.
        /// También se podría ver de usar un MessagingCenter.Send(... para evitar usar la base de datos
        /// como medio de comunicación. Pero por lo pronto así funciona bien.
        /// </summary>
        void EsperarElemento()
        {
            ElementoViewModel.IsBusy = true;
            DateTime now = DateTime.Now;
            DatabaseHelper.db.DeleteAll<ElementoAgregado>();
            do
            {
                if (DatabaseHelper.db.Table<ElementoAgregado>().Count() > 0)
                {
                    ElementoAgregado elementoAgregado = DatabaseHelper.db.Table<ElementoAgregado>().FirstOrDefault();
                    ElementoViewModel.UUID = elementoAgregado.UUID;
                    DatabaseHelper.db.DeleteAll<ElementoAgregado>();
                    ElementoViewModel.Vinculado = true;
                    BotonVincularElemento.IsEnabled = false;
                    break;
                }
                Task.Delay(2000);
            } while ((DateTime.Now - now).TotalSeconds < 10);
            ElementoViewModel.IsBusy = false;
        }
    }
}