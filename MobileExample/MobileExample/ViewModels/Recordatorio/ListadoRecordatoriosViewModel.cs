using MobileExample.Database;
using MobileExample.Tables;
using MobileExample.Views;
using SQLite;
using SQLiteNetExtensions.Extensions;
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
    public class ListadoRecordatoriosViewModel : BaseViewModel
    {
        public ObservableCollection<RecordatorioViewModel> Recordatorios { get; set; }

        public Command ComandoCargarRecordatorios { get; set; }

        public ListadoRecordatoriosViewModel()
        {
            Title = "Mis Recordatorios";
            Recordatorios = new ObservableCollection<RecordatorioViewModel>();
            ComandoCargarRecordatorios = new Command(() => EjecutarComando());

            // Esto registra una especie de 'listener' para cuando agregamos mochilas.
            // La idea es que desde la vista de creación se envíe un mensaje con el texto
            // 'AgregarMochila' y el objeto viewModel, y de esa manera se ejecuta esta porçión de código.
            MessagingCenter.Subscribe<NuevoRecordatorio, RecordatorioViewModel>(this, "AgregarRecordatorio", (obj, recordatorioViewModel) =>
            {
                Recordatorio recordatorio = new Recordatorio
                {
                    Horario = recordatorioViewModel.Horario,
                    Lunes = recordatorioViewModel.Lunes,
                    Martes = recordatorioViewModel.Martes,
                    Miercoles = recordatorioViewModel.Miercoles,
                    Jueves = recordatorioViewModel.Jueves,
                    Viernes = recordatorioViewModel.Viernes,
                    Sabado = recordatorioViewModel.Sabado,
                    Domingo = recordatorioViewModel.Domingo,
                    Elementos = new List<Elemento>(),
                    IdMochila = recordatorioViewModel.MochilaSeleccionada.Id
                };

                foreach (ElementoViewModel elementoViewModel in recordatorioViewModel.Elementos.Elementos.Where(e => e.Seleccionado))
                {
                    Elemento elemento = DatabaseHelper.db.Table<Elemento>().Where(e => e.Id == elementoViewModel.Id).FirstOrDefault();
                    recordatorio.Elementos.Add(elemento);
                }

                DatabaseHelper.db.InsertWithChildren(recordatorio);
                // Se agrega el ID que se obtiene despues de insertarlo en la BD,
                // para así poder traer los datos de los elementos para el 'ver' sin
                // tener que refrescar la lista.
                recordatorioViewModel.Id = recordatorio.Id;
                Recordatorios.Add(recordatorioViewModel);
            });

            MessagingCenter.Subscribe<RecordatorioViewModel, RecordatorioViewModel>(this, "EliminarRecordatorio", (sender, recordatorioViewModel) =>
            {
                Recordatorio recordatorioAEliminar = DatabaseHelper.db.Table<Recordatorio>().Where(e => e.Id.Equals(recordatorioViewModel.Id)).FirstOrDefault();
                DatabaseHelper.db.Delete(recordatorioAEliminar);
                Recordatorios.Remove(recordatorioViewModel);
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
                Recordatorios.Clear();
                var recordatorios = this.ObtenerRecordatorios();
                foreach (var recordatorio in recordatorios)
                {
                    Recordatorios.Add(recordatorio);
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

        private List<RecordatorioViewModel> ObtenerRecordatorios()
        {
            List<RecordatorioViewModel> listadoRecordatorios = new List<RecordatorioViewModel>();

            foreach (Recordatorio recordatorio in DatabaseHelper.db.Table<Recordatorio>().ToList())
            {
                RecordatorioViewModel recordatorioViewModel = new RecordatorioViewModel();
                recordatorioViewModel.Id = recordatorio.Id;
                recordatorioViewModel.Horario = recordatorio.Horario;
                recordatorioViewModel.Lunes = recordatorio.Lunes;
                recordatorioViewModel.Martes = recordatorio.Martes;
                recordatorioViewModel.Miercoles = recordatorio.Miercoles;
                recordatorioViewModel.Jueves = recordatorio.Jueves;
                recordatorioViewModel.Viernes = recordatorio.Viernes;
                recordatorioViewModel.Sabado = recordatorio.Sabado;
                recordatorioViewModel.Domingo = recordatorio.Domingo;
                recordatorioViewModel.HorarioStr = recordatorio.Horario.ToString(@"hh\:mm");
                listadoRecordatorios.Add(recordatorioViewModel);
            }

            return listadoRecordatorios;
        }
    }
}
