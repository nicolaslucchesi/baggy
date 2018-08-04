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
	public partial class SeleccionarMochilas : ContentPage
	{
        ListadoMochilasViewModel viewModel;
        RecordatorioViewModel recordatorioCompleto;

        public SeleccionarMochilas()
        {

            InitializeComponent();
            //BindingContext = viewModel = new ListadoElementosViewModel();
        }

        public SeleccionarMochilas(RecordatorioViewModel recordatorio)
        {

            InitializeComponent();
           // BindingContext = viewModel = new ListadoElementosViewModel();
            recordatorioCompleto = new RecordatorioViewModel(recordatorio);
//            Prueba.Text = recordatorioCompleto.HorarioStr;
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            RecordatorioViewModel SendRecordatorio = new RecordatorioViewModel(recordatorioCompleto);
            //SendRecordatorio.Elementos = Elementos.ToList();


            MessagingCenter.Send(this, "AgregarRecordatorio", SendRecordatorio);
            await Navigation.PopModalAsync();

            //await Navigation.PushAsync(new SeleccionarElementos(SendRecordatorio));

        }


    }
}