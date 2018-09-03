using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Tables;
using MobileExample.ViewModels;
using MobileExample.Database;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerMochila : ContentPage
	{
        public MochilaViewModel mochilaViewModel { get; set; }
        public VerMochila(int Id)
        {
            InitializeComponent();
            mochilaViewModel = (MochilaViewModel)DatabaseHelper.db.Get<Mochila>(Id);
            BindingContext = this.mochilaViewModel = mochilaViewModel;
        }
    }
}