using BottomBar.XamarinForms;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileExample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : BottomBarPage
	{
		public MainPage ()
		{
            NavigationPage.SetTitleIcon(this, "BaggyLogo.jpg");
            InitializeComponent ();
		}
	}
}