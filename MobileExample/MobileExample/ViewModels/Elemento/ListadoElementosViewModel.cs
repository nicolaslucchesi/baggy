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

        public ObservableCollection<ElementoViewModel> Elementos { get; set; }
        public ObservableCollection<ElementoViewModel> ElementosSeleccionados { get; set; }

        public Command ComandoCargarElementos { get; set; }

        public ListadoElementosViewModel()
        {
            Title = "Mis Elementos";
            Elementos = new ObservableCollection<ElementoViewModel>();
            ElementosSeleccionados = new ObservableCollection<ElementoViewModel>();

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
                Elementos.Add(elementoViewModel);
            });

            MessagingCenter.Subscribe<ElementoViewModel, ElementoViewModel>(this, "EliminarElemento", (sender, elementoViewModel) =>
            {
                Elemento elementoAEliminar = db.Table<Elemento>().Where(e => e.Id.Equals(elementoViewModel.Id)).FirstOrDefault();
                db.Delete(elementoAEliminar);
                Elementos.Remove(elementoViewModel);
            });

            MessagingCenter.Subscribe<SeleccionarElementos, ElementoViewModel>(this, "SeleccionarElemento", (obj, elementoViewModel) =>
            {
                ElementoViewModel elemento = new ElementoViewModel
                {
                    Descripcion = elementoViewModel.Descripcion,
                    RutaIcono = elementoViewModel.RutaIcono,
                    Imprescindible = elementoViewModel.Imprescindible,
                    Vinculado = elementoViewModel.Vinculado,
                    Seleccionado = true
                };

                ElementosSeleccionados.Add(elemento);
                Elementos.Remove(elementoViewModel); 
                Elementos.OrderBy(item => item.Seleccionado);
            });

            MessagingCenter.Subscribe<SeleccionarElementos, ElementoViewModel>(this, "DeseleccionarElemento", (obj, elementoViewModel) =>
            {
                ElementoViewModel elemento = new ElementoViewModel
                {
                    Descripcion = elementoViewModel.Descripcion,
                    RutaIcono = elementoViewModel.RutaIcono,
                    Imprescindible = elementoViewModel.Imprescindible,
                    Vinculado = elementoViewModel.Vinculado,
                    Seleccionado = false
                };

                ElementosSeleccionados.Remove(elemento);
                Elementos.Remove(elementoViewModel);
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

        private List<ElementoViewModel> ObtenerElementos()
        {
            List<ElementoViewModel> listadoElementos = new List<ElementoViewModel>();
            int cantidad = 0;
            foreach (Elemento elemento in db.Table<Elemento>().ToList())
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

        public String ObtenerElementosSeleccionados()
        {
            String ElementosStr = "Seleccionados: ";
            foreach (ElementoViewModel elemento in ElementosSeleccionados.ToList())
            {
                ElementosStr = ElementosStr + elemento.Descripcion + ", " ;
  
            }

            return ElementosStr;
        }


    }
}
