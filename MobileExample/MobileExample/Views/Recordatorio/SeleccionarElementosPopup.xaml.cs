using MobileExample.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SeleccionarElementosPopup : PopupPage
	{
        ListadoElementosViewModel popupViewModel;
		public SeleccionarElementosPopup (ListadoElementosViewModel viewModel)
		{
			InitializeComponent ();
            BindingContext = popupViewModel = viewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (popupViewModel.Elementos.Count == 0)
                popupViewModel.ComandoCargarElementos.Execute(null);
        }

        private async void CerrarPopup(object sender, System.EventArgs e)
        {
            //Metodo para cerrar el pop-up
            await PopupNavigation.PopAsync();
        }
    }
}