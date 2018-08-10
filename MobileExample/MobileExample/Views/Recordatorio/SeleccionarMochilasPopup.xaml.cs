using MobileExample.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SeleccionarMochilasPopup : PopupPage
	{
        ListadoMochilasViewModel popupViewModel;
        MochilaViewModel mochilaSeleccionada;
 
		public SeleccionarMochilasPopup (MochilaViewModel mochilaViewModel)
		{
			InitializeComponent ();
            popupViewModel = new ListadoMochilasViewModel();
            mochilaSeleccionada = mochilaViewModel;
            BindingContext = popupViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (popupViewModel.Mochilas.Count == 0)
                popupViewModel.ComandoCargarMochilas.Execute(null);
        }

        private async void CerrarPopup(object sender, System.EventArgs e)
        {
            //Metodo para cerrar el pop-up
            await PopupNavigation.PopAsync();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as MochilaViewModel;
            if (item == null)
                return;

            MessagingCenter.Send(this, "SeleccionarMochila", item);

            await PopupNavigation.PopAsync();
        }
    }
}