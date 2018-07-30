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
	public partial class ListadoRecordatorios : ContentPage
	{
        ListadoRecordatoriosViewModel viewModel;
     

        public ListadoRecordatorios()
        {

            InitializeComponent();
            BindingContext = viewModel = new ListadoRecordatoriosViewModel();
        }
        
        /// <summary>
        /// Este metodo se ejecuta cuando se selecciona un item de la lista.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as RecordatorioViewModel;
            if (item == null)
                return;

            //  await Navigation.PushAsync(new NavigationPage(new VerMochila(item)));
            await Navigation.PushAsync(new VerRecordatorio(new VerRecordatorioViewModel(item)));
            // Manually deselect item.
            ItemsListView2.SelectedItem = null;
        }

        async void AgregarRecordatorio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NuevoRecordatorio()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Recordatorios.Count == 0)
                viewModel.ComandoCargarRecordatorios.Execute(null);
        }
    }
}