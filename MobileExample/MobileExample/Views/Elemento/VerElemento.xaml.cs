using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Tables;
using MobileExample.ViewModels;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerElemento : ContentPage
	{
        VerElementoViewModel viewModel;

        public VerElemento(VerElementoViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        
        public VerElemento()
        {
           InitializeComponent();

            var item = new Elemento
            {
                UUID = "Item 1",
                Descripcion = "This is an item description."
            };

            viewModel = new VerElementoViewModel(item);
            BindingContext = viewModel;
        }
        
    }
}