using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Tables;
using MobileExample.ViewModels;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerMochila : ContentPage
	{
        VerMochilaViewModel viewModel;

        public VerMochila(VerMochilaViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        
        public VerMochila()
        {
           InitializeComponent();

            var item = new Mochila
            {
                UUID = "Item 1",
                Descripcion = "This is an item description."
            };

            viewModel = new VerMochilaViewModel(item);
            BindingContext = viewModel;
        }
    }
}