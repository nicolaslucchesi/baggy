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
    public class ListadoImagenesElementosViewModel : BaseViewModel
    {
        public ObservableCollection<ImagenElementoViewModel> Imagenes { get; set; }

        public Command ComandoCargarImagenes { get; set; }

        public ListadoImagenesElementosViewModel()
        {
            Title = "Elegir Imagenes";
            Imagenes = new ObservableCollection<ImagenElementoViewModel>();
            ComandoCargarImagenes = new Command(() => EjecutarComando());
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
                Imagenes.Clear();
                var elementos = this.ObtenerImagenes();
                foreach (var elemento in elementos)
                {
                    Imagenes.Add(elemento);
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

        private List<ImagenElementoViewModel> ObtenerImagenes()
        {
            List<ImagenElementoViewModel> listadoImagenes = new List<ImagenElementoViewModel>();

            ImagenElementoViewModel img1 = new ImagenElementoViewModel
            {
                Id = 0,
                Descripcion = "Rojo",
                RutaIcono = "Obj1.png"
            };
            listadoImagenes.Add(img1);

            ImagenElementoViewModel img2 = new ImagenElementoViewModel
            {
                Id = 1,
                Descripcion = "Azul",
                RutaIcono = "Obj2.png"
            };

            listadoImagenes.Add(img2);

            return listadoImagenes;
        }
    }
}
