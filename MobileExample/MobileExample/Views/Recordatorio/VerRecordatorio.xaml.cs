using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Tables;
using MobileExample.ViewModels;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerRecordatorio : ContentPage
	{
        VerRecordatorioViewModel viewModel;

        public VerRecordatorio(VerRecordatorioViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        
        public VerRecordatorio()
        {
           InitializeComponent();

            var item = new Recordatorio
            {
                Minuto = 7,
                DiaSemana = 5
            };

            viewModel = new VerRecordatorioViewModel(item);
            BindingContext = viewModel;
        }
    }
}