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

            MessagingCenter.Subscribe<ElementoViewModel, ElementoViewModel>(this, "EditarElemento", (sender, elementoViewModel) =>
            {
                Navigation.PushModalAsync(new NavigationPage(new CrearEditarElemento(elementoViewModel.Id)));
            });
        }

        async void AgregarElemento_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new CrearEditarElemento()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Elementos.Count == 0)
                viewModel.ComandoCargarElementos.Execute(null);
        }
    }
}