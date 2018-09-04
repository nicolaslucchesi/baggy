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
    public class ListadoMochilasViewModel : BaseViewModel
    {
        public ObservableCollection<MochilaViewModel> Mochilas { get; set; }

        public Command ComandoCargarMochilas { get; set; }
        public ObservableCollection<Slide> Slides { get; }

        public ListadoMochilasViewModel()
        {
            Title = "Mis mochilas";
            Mochilas = new ObservableCollection<MochilaViewModel>();
            ComandoCargarMochilas = new Command(() => RefrescarMochilas());
 
        // Esto registra una especie de 'listener' para cuando agregamos mochilas.
        // La idea es que desde la vista de creación se envíe un mensaje con el texto
        // 'AgregarMochila' y el objeto viewModel, y de esa manera se ejecuta esta porçión de código.
        MessagingCenter.Subscribe<NuevaMochila, MochilaViewModel>(this, "AgregarMochila", (obj, mochilaViewModel) =>
            {
                Mochila mochila = new Mochila
                {
                    Activa = false,
                    Descripcion = mochilaViewModel.Descripcion,
                    UUID = mochilaViewModel.UUID,
                    RutaIcono = mochilaViewModel.RutaIcono
                };

                DatabaseHelper.db.Insert(mochila);
                Mochilas.Add(mochilaViewModel);

                MessagingCenter.Send(this, "MochilaAgregada", mochilaViewModel.Descripcion);
            });

            // Este nuevo listener escucha cuando un objeto 'mochilaViewModel' quiere ser eliminado
            // Primero lo busca por su UUID en la base de datos y luego lo elimina tanto de la BD
            // como de la lista interna para que desaparezca visualmente.
            MessagingCenter.Subscribe<MochilaViewModel, MochilaViewModel>(this, "EliminarMochila", (sender, mochilaViewModel) =>
            {
                Mochila mochilaAEliminar = DatabaseHelper.db.Table<Mochila>().Where(e => e.UUID.Equals(mochilaViewModel.UUID)).FirstOrDefault();
                DatabaseHelper.db.Delete(mochilaAEliminar);
                Mochilas.Remove(mochilaViewModel);

                // Tengo que desvincular las mochilas de los recordatorios si le doy eliminar a una
                List<Recordatorio> recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(e => e.IdMochila == mochilaAEliminar.Id).ToList();
                foreach (Recordatorio recordatorio in recordatorios) {
                    recordatorio.IdMochila = null;
                }
                DatabaseHelper.db.UpdateAll(recordatorios);

                MessagingCenter.Send(this, "MochilaEliminada", mochilaViewModel.Descripcion);
            });

            MessagingCenter.Subscribe<MochilaViewModel, MochilaViewModel>(this, "ActivarMochila", (sender, mochilaViewModel) =>
            {
                foreach (Mochila mochila in DatabaseHelper.db.Table<Mochila>().ToList())
                {
                    mochila.Activa = false;
                    if (mochila.UUID.Equals(mochilaViewModel.UUID))
                    {
                        mochila.Activa = true;
                    }
                    DatabaseHelper.db.Update(mochila);
                }

                foreach (MochilaViewModel mochila in Mochilas)
                {
                    mochila.Activa = false;
                    if (mochila.UUID.Equals(mochilaViewModel.UUID))
                    {
                        mochila.Activa = true;
                    }
                }

                this.RefrescarMochilas();
            });

            MessagingCenter.Subscribe<MochilaViewModel, MochilaViewModel>(this, "ActivarAlarma", (sender, mochilaViewModel) =>
            {
                Mochila mochilaAActivar = DatabaseHelper.db.Table<Mochila>().Where(e => e.UUID.Equals(mochilaViewModel.UUID)).FirstOrDefault();
                mochilaAActivar.EstadoAlarma = !mochilaAActivar.EstadoAlarma;
                DatabaseHelper.db.Update(mochilaAActivar);
                this.RefrescarMochilas();
                MessagingCenter.Send(this, "EnviarAlerta", mochilaAActivar.EstadoAlarma ? "La alarma ha sido activada" : "La alarma ha sido desactivada");
            });

        }

        private void RefrescarMochilas()
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
        /// <summary>
        /// Este metodo obtiene las mochilas de la base de datos y las
        /// mapea a MochilaViewModel, donde el propio constructor se encarga de
        /// asignarle el comando para eliminación.
        /// </summary>
        /// <returns></returns>
        private List<MochilaViewModel> ObtenerMochilas()
        {
            List<MochilaViewModel> listadoMochilas = new List<MochilaViewModel>();
            foreach (Mochila mochila in DatabaseHelper.db.Table<Mochila>().ToList())
            {
                listadoMochilas.Add((MochilaViewModel)mochila);
            }

            return listadoMochilas;
        }
    }
}
