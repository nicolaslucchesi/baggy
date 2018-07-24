using System;
using Xamarin.Forms;
using MobileExample.Views;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using MobileExample.Tables;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MobileExample
{
	public partial class App : Application
	{
		
		public App ()
		{
			InitializeComponent();

            IniciarDatabase();

			MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        private void IniciarDatabase()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
            var db = new SQLiteConnection(path);
            db.CreateTable<Elemento>();
            db.CreateTable<InformacionSincronizada>();
            db.CreateTable<Mochila>();
            db.CreateTable<Recordatorio>();
        }
    }
}
