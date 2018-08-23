using MobileExample.ViewModels;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
        ListadoRecordatoriosViewModel viewModel;
        public AboutPage ()
		{
            NavigationPage.SetTitleIcon(this, "BaggyLogo.jpg");
            InitializeComponent ();

            BindingContext = viewModel = new ListadoRecordatoriosViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Recordatorios.Count == 0)
                viewModel.ComandoCargarRecordatoriosDelDia.Execute(null);
        }



    }
}