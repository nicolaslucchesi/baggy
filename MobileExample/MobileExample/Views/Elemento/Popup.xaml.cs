using MobileExample.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Popup : PopupPage
	{
        ListadoImagenesElementosViewModel viewModel;
        public bool b;

        public Popup (bool a)
		{
			InitializeComponent ();
            BindingContext = viewModel = new ListadoImagenesElementosViewModel();

            if(a)
                b = true;
             else
                b = false;            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Imagenes.Count == 0 && b == true)
                viewModel.ComandoCargarImagenes.Execute(null);
            else if (viewModel.Imagenes.Count == 0 && b == false)
                viewModel.ComandoCargarColores.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ImagenElementoViewModel;
            // Mando un ImagenElementoViewModel pero pienso que se podria mandar un String directamente
            string mensaje;
            if (b)
            {
                mensaje = "SeleccionarImagen";
            }
            else
            {
                mensaje = "SeleccionarColor";
            }
            MessagingCenter.Send(this, "SeleccionarImagen", item);
            await PopupNavigation.PopAsync();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            //Metodo para cerrar el pop-up
            await PopupNavigation.PopAsync();
        }
    }

    
}