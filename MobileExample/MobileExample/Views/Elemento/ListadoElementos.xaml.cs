using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Tables;
using MobileExample.Views;
using MobileExample.ViewModels;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListadoElementos : ContentPage
	{
        ListadoElementosViewModel viewModel;

        public ListadoElementos()
        {

            InitializeComponent();
            BindingContext = viewModel = new ListadoElementosViewModel();
        }
        

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Elemento;
            if (item == null)
                return;

            //  await Navigation.PushAsync(new NavigationPage(new VerMochila(item)));

            await Navigation.PushAsync(new VerElemento(new VerElementoViewModel(item)));
            // Manually deselect item.
            ItemsListView3.SelectedItem = null;
        }

        async void AgregarElemento_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NuevoElemento()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Elementos.Count == 0)
                viewModel.ComandoCargarElementos.Execute(null);
        }
    }
}