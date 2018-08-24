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
	public partial class ListadoMochilas : ContentPage
	{
        ListadoMochilasViewModel viewModel;

        public ListadoMochilas()
        {
            InitializeComponent();

            BindingContext = viewModel = new ListadoMochilasViewModel();

            MessagingCenter.Subscribe<ListadoMochilasViewModel, string>(this, "EnviarAlerta", (sender, mensaje) =>
            {
                DisplayAlert("Alarma", mensaje, "Aceptar");
            });
        }

        async void AgregarMochila_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NuevaMochila()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Mochilas.Count == 0)
                viewModel.ComandoCargarMochilas.Execute(null);
        }
    }
}