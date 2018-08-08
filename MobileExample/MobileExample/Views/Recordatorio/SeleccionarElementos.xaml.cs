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
	public partial class SeleccionarElementos : ContentPage
	{
        ListadoElementosViewModel viewModel;
        RecordatorioViewModel recordatorioCompletado;

        public SeleccionarElementos()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListadoElementosViewModel();
        }

        public SeleccionarElementos(RecordatorioViewModel recordatorio)
        {
            InitializeComponent();
            BindingContext = viewModel = new ListadoElementosViewModel();
            recordatorioCompletado = new RecordatorioViewModel();
            recordatorioCompletado = recordatorio;
            Prueba.Text = recordatorioCompletado.HorarioStr;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ElementoViewModel;
            if (item == null)
                return;

            if (item.Seleccionado == false)
            {
                item.Seleccionado = true;
                var itemAPasar = viewModel.Elementos.Where(e => e.IdInterno == item.IdInterno).FirstOrDefault();
                viewModel.Elementos.Remove(itemAPasar);
                viewModel.ElementosSeleccionados.Add(itemAPasar);
                viewModel.ElementosSeleccionados.OrderBy(e => e.IdInterno);
            }
            else
            {
                item.Seleccionado = false;
                var itemAPasar = viewModel.ElementosSeleccionados.Where(e => e.IdInterno == item.IdInterno).FirstOrDefault();
                viewModel.ElementosSeleccionados.Remove(itemAPasar);
                viewModel.Elementos.Add(itemAPasar);
                viewModel.Elementos.OrderBy(e => e.IdInterno);
            }

            ItemsListView3.SelectedItem = null;
            
        }

        
        async void Guardar_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            RecordatorioViewModel SendRecordatorio = new RecordatorioViewModel();
            SendRecordatorio = recordatorioCompletado;

            foreach (ElementoViewModel a in viewModel.ElementosSeleccionados.ToList())
            {
                SendRecordatorio.Elementos.Add(a.Id);
            }
            //MessagingCenter.Send(this, "AgregarRecordatorio", SendRecordatorio);
            await Navigation.PushAsync(new SeleccionarMochilas(SendRecordatorio));

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Elementos.Count == 0)
                viewModel.ComandoCargarElementos.Execute(null);
        }

        
    }
}