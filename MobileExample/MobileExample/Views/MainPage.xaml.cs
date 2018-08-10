using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
		public MainPage ()
		{
            NavigationPage.SetTitleIcon(this, "BaggyLogo.jpg");
            InitializeComponent ();
		}
	}
}