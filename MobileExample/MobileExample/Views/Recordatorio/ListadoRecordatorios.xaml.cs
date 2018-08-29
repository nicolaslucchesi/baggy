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
using MobileExample.Database;
using SQLiteNetExtensions.Extensions;

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

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as RecordatorioViewModel;
            if (item == null)
                return;

            RecordatorioViewModel viewModel = this.ObtenerRecordatorioConElementos(item.Id);

            await Navigation.PushAsync(new VerRecordatorio(new VerRecordatorioViewModel(viewModel)));

            RecordatoriosListView.SelectedItem = null;
        }

        async void AgregarRecordatorio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NuevoRecordatorio()));
        }

        void Filtros_Clicked(object sender, EventArgs e)
        {
            Filtros.IsVisible = !Filtros.IsVisible;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Recordatorios.Count == 0)
                viewModel.ComandoCargarRecordatorios.Execute(null);
        }

        private RecordatorioViewModel ObtenerRecordatorioConElementos(int Id) {
            Recordatorio recordatorio = DatabaseHelper.db.GetWithChildren<Recordatorio>(Id);
            return (RecordatorioViewModel)recordatorio;
        }

        public void ApretarBotonDia(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            switch (boton.Text)
            {
                case "Lu":
                    SwitchLunes.IsToggled = !SwitchLunes.IsToggled;
                    break;
                case "Ma":
                    SwitchMartes.IsToggled = !SwitchMartes.IsToggled;
                    break;
                case "Mi":
                    SwitchMiercoles.IsToggled = !SwitchMiercoles.IsToggled;
                    break;
                case "Ju":
                    SwitchJueves.IsToggled = !SwitchJueves.IsToggled;
                    break;
                case "Vi":
                    SwitchViernes.IsToggled = !SwitchViernes.IsToggled;
                    break;
                case "Sa":
                    SwitchSabado.IsToggled = !SwitchSabado.IsToggled;
                    break;
                case "Do":
                    SwitchDomingo.IsToggled = !SwitchDomingo.IsToggled;
                    break;
                default:
                    break;
            }
        }

        private void AplicarFiltros_Clicked (object sender, EventArgs e)
        {
            viewModel.ObtenerRecordatoriosFiltrados();
            Filtros.IsVisible = !Filtros.IsVisible;
        }
    }
}