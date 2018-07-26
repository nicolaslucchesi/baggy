using MobileExample.Tables;
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
    public class ListadoElementosViewModel : BaseViewModel
    {
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
        private static SQLiteConnection db = new SQLiteConnection(path);

        public ObservableCollection<Elemento> Elementos { get; set; }

        public Command ComandoCargarElementos { get; set; }

        public ListadoElementosViewModel()
        {
            Title = "Mis Elementos";
            Elementos = new ObservableCollection<Elemento>();
            ComandoCargarElementos = new Command(() => EjecutarComando());

            // Esto registra una especie de 'listener' para cuando agregamos mochilas.
            // La idea es que desde la vista de creación se envíe un mensaje con el texto
            // 'AgregarMochila' y el objeto viewModel, y de esa manera se ejecuta esta porçión de código.
            MessagingCenter.Subscribe<NuevoElemento, ElementoViewModel>(this, "AgregarElemento", (obj, elementoViewModel) =>
            {
                Elemento elemento = new Elemento
                {
                    Descripcion = elementoViewModel.Descripcion ,
                    RutaIcono = elementoViewModel.RutaIcono,
                    Imprescindible = elementoViewModel.Imprescindible,
                    Vinculado = elementoViewModel.Vinculado
                };

                db.Insert(elemento);
                Elementos.Add(elemento);
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
                Elementos.Clear();
                var elementos = this.ObtenerElementos();
                foreach (var elemento in elementos)
                {
                    Elementos.Add(elemento);
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

        private List<Elemento> ObtenerElementos()
        {
            return db.Table<Elemento>().ToList();
        }
    }
}
