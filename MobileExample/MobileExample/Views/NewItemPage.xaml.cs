using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MobileExample.Models;
using System.IO;
using SQLite;
using MobileExample.Tables;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            AgregarMochila();
            await Navigation.PopModalAsync();
        }

        private void AgregarMochila() {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
            var db = new SQLiteConnection(path);
            db.Insert(new Mochila { Activa = true, Descripcion = "Prueba", UUID = "10000" });
        }
    }
}