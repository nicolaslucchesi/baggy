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
    public class ListadoElementosRecordatorioViewModel
    {
        public List<ElementoViewModel> Elementos { get; set; }

        public Command ComandoCargarElementos { get; set; }

        public ListadoElementosRecordatorioViewModel()
        {
            Elementos = new List<ElementoViewModel>();

            ComandoCargarElementos = new Command(() => EjecutarComando());
        }

        private void EjecutarComando()
        {
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
