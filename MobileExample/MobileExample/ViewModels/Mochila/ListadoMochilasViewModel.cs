﻿using MobileExample.Tables;
using MobileExample.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileExample.ViewModels
{
    public class ListadoMochilasViewModel : BaseViewModel
    {
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
        private static SQLiteConnection db = new SQLiteConnection(path);

        public ObservableCollection<Mochila> Mochilas { get; set; }

        public Command ComandoCargarMochilas { get; set; }

        public ListadoMochilasViewModel()
        {
            Title = "Mis mochilas";
            Mochilas = new ObservableCollection<Mochila>();
            ComandoCargarMochilas = new Command(() => EjecutarComando());

            // Esto registra una especie de 'listener' para cuando agregamos mochilas.
            // La idea es que desde la vista de creación se envíe un mensaje con el texto
            // 'AgregarMochila' y el objeto viewModel, y de esa manera se ejecuta esta porçión de código.
            MessagingCenter.Subscribe<NuevaMochila, MochilaViewModel>(this, "AgregarMochila", (obj, mochilaViewModel) =>
            {
                Mochila mochila = new Mochila
                {
                    Activa = false,
                    Descripcion = mochilaViewModel.Descripcion,
                    UUID = mochilaViewModel.UUID
                };

                db.Insert(mochila);
                Mochilas.Add(mochila);
            });
        }

        private void EjecutarComando()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Mochilas.Clear();
                var mochilas = this.ObtenerMochilas();
                foreach (var mochila in mochilas)
                {
                    Mochilas.Add(mochila);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private List<Mochila> ObtenerMochilas()
        {
            return db.Table<Mochila>().ToList();
        }
    }
}
