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
        public Command ComandoCargarColores { get; set; }

        public ListadoImagenesElementosViewModel()
        {
            Title = "Elegir Imagenes";
            Imagenes = new ObservableCollection<ImagenElementoViewModel>();

                ComandoCargarImagenes = new Command(() => EjecutarComandoImagenes());
                ComandoCargarColores = new Command(() => EjecutarComandoColores()); 
        }

        private void EjecutarComandoImagenes()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Imagenes.Clear();
                bool a = true;
                var elementos = this.ObtenerImagenes(a);
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

        private void EjecutarComandoColores()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Imagenes.Clear();
                bool a = false;
                var elementos = this.ObtenerImagenes(a);
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

        private List<ImagenElementoViewModel> ObtenerImagenes(bool a)
        {
            List<ImagenElementoViewModel> listadoImagenes = new List<ImagenElementoViewModel>();
            if(a)
            { 
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
            }
            else
            {
                ImagenElementoViewModel img1 = new ImagenElementoViewModel
                {
                    Id = 0,
                    Descripcion = "Rojo",
                    RutaIcono = "Color1.png"
                };
                listadoImagenes.Add(img1);

                ImagenElementoViewModel img2 = new ImagenElementoViewModel
                {
                    Id = 1,
                    Descripcion = "Azul",
                    RutaIcono = "Color2.png"
                };

                listadoImagenes.Add(img2);

                ImagenElementoViewModel img3 = new ImagenElementoViewModel
                {
                    Id = 2,
                    Descripcion = "Amarillo",
                    RutaIcono = "Color3.png"
                };

                listadoImagenes.Add(img3);
            }

            return listadoImagenes;
        }
    }
}
