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

            UUID = "123456";               
        }
 
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            //Metodo para cerrar el pop-up
            await PopupNavigation.PopAsync();
        }
    }

    
}