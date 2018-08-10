using MobileExample.Database;
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
        public ObservableCollection<ElementoViewModel> Elementos { get; set; }

        public Command ComandoCargarElementos { get; set; }
        public Command ComandoDeseleccionarElementos { get; set; }

        public ListadoElementosViewModel()
        {
            Title = "Mis Elementos";
            Elementos = new ObservableCollection<ElementoViewModel>();

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

                DatabaseHelper.db.Insert(elemento);
                Elementos.Add(elementoViewModel);
            });

            MessagingCenter.Subscribe<ElementoViewModel, ElementoViewModel>(this, "EliminarElemento", (sender, elementoViewModel) =>
            {
                Elemento elementoAEliminar = DatabaseHelper.db.Table<Elemento>().Where(e => e.Id.Equals(elementoViewModel.Id)).FirstOrDefault();
                DatabaseHelper.db.Delete(elementoAEliminar);
                Elementos.Remove(elementoViewModel);
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

        private List<ElementoViewModel> ObtenerElementos()
        {
            List<ElementoViewModel> listadoElementos = new List<ElementoViewModel>();
            int cantidad = 0;
            foreach (Elemento elemento in DatabaseHelper.db.Table<Elemento>().ToList())
            {
                ElementoViewModel elementoViewModel = new ElementoViewModel();
                elementoViewModel.Imprescindible = elemento.Imprescindible;
                elementoViewModel.RutaIcono = elemento.RutaIcono;
                elementoViewModel.Descripcion = elemento.Descripcion;
                elementoViewModel.Vinculado = elemento.Vinculado;
                elementoViewModel.UUID = elemento.UUID;
                elementoViewModel.Id = elemento.Id;
                elementoViewModel.IdInterno = cantidad;
                cantidad++;
                listadoElementos.Add(elementoViewModel);
            }

            return listadoElementos;
        }
    }
}
