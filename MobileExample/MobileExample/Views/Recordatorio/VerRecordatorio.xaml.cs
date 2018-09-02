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

            var item = new RecordatorioViewModel();

            viewModel = new VerRecordatorioViewModel(item);
            BindingContext = viewModel;
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
    }
}