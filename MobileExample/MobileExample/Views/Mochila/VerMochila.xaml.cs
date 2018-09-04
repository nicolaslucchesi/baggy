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
        public int tapCount;

        public VerMochila(int Id)
        {
            InitializeComponent();
            mochilaViewModel = (MochilaViewModel)DatabaseHelper.db.Get<Mochila>(Id);
            BindingContext = this.mochilaViewModel = mochilaViewModel;

            tapCount = 0;


            var topLeft = new Image { Source = "Libro.png" };
            var topRight = new Image { Source = "Editar1.png" };
            var bottomLeft = new Image { Source = "mochila.png" };
            var bottomRight = new Image { Source = "Libro.png" };

            GridElementos.Children.Add(topLeft, 0, 0);
            GridElementos.Children.Add(topRight, 1, 0);
            GridElementos.Children.Add(bottomLeft, 0, 1);
            GridElementos.Children.Add(bottomRight, 1, 1);
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            tapCount++;
            var imageSender = (Image)sender;
            // watch the monkey go from color to black&white!
            if (tapCount % 2 == 0)
            {
                imageSender.IsVisible = false;
                Blurr.IsVisible = true;
                GridElementos.IsVisible = true;
            } 
        }

        void CerrarMochila(object sender, EventArgs args)
        {
            tapCount++;
            if (tapCount % 2 == 0)
            {
                MochilaCerrada.IsVisible = true;
                Blurr.IsVisible = false;
                GridElementos.IsVisible = false;
            }
        }
    }
}