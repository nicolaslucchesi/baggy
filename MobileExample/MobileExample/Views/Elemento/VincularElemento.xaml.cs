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
	public partial class VincularElemento : PopupPage
	{
        ListadoImagenesElementosViewModel viewModel;
        public string UUID;

        public VincularElemento ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new ListadoImagenesElementosViewModel();

            
            Libro.TranslateTo(0, 100, 3000); 

            UUID = "123456";               
        }
 
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            //Metodo para cerrar el pop-up
            await PopupNavigation.PopAsync();
        }

        async void Cancelar_OnClicked (object sender, SelectedItemChangedEventArgs args)
        {
            MessagingCenter.Send(this, "VincularElemento", false);
            await PopupNavigation.PopAsync();
        }

        async void Aceptar_OnClicked(object sender, SelectedItemChangedEventArgs args)
        {
            MessagingCenter.Send(this, "VincularElemento", true);
            await PopupNavigation.PopAsync();
        }
    }

    
}