using MobileExample.Database;
using MobileExample.Tables;
using MobileExample.Views;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace MobileExample.ViewModels
{
    public class ListadoRecordatoriosViewModel : BaseViewModel
    {
        public ObservableCollection<RecordatorioViewModel> Recordatorios { get; set; }

        public FiltrosRecordatorioViewModel Filtros { get; set; }

        public Command ComandoCargarRecordatorios { get; set; }

        public ListadoRecordatoriosViewModel()
        {
            Title = "Mis Recordatorios";
            Recordatorios = new ObservableCollection<RecordatorioViewModel>();
            Filtros = new FiltrosRecordatorioViewModel();
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
                    Elementos = new List<Elemento>()
                };

                foreach (ElementoViewModel elementoViewModel in recordatorioViewModel.Elementos.Elementos.Where(e => e.Seleccionado))
                {
                    Elemento elemento = DatabaseHelper.db.Get<Elemento>(elementoViewModel.Id);
                    recordatorio.Elementos.Add(elemento);
                }

                if (!recordatorioViewModel.MochilaSeleccionada.Equals("Ninguna"))
                {
                    recordatorio.IdMochila = DatabaseHelper.db.Table<Mochila>().FirstOrDefault(m => m.Descripcion.Equals(recordatorioViewModel.MochilaSeleccionada)).Id;
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
                Recordatorio recordatorioAEliminar = DatabaseHelper.db.Get<Recordatorio>(recordatorioViewModel.Id);
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

        public void ObtenerRecordatoriosFiltrados()
        {
            Recordatorios.Clear();
            List<Recordatorio> recordatorios = new List<Recordatorio>();
            if (Filtros.Lunes)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Lunes));
            }
            if (Filtros.Martes)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Martes));
            }
            if (Filtros.Miercoles)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Miercoles));
            }
            if (Filtros.Jueves)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Jueves));
            }
            if (Filtros.Viernes)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Viernes));
            }
            if (Filtros.Sabado)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Sabado));
            }
            if (Filtros.Domingo)
            {
                recordatorios.AddRange(DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(r => r.Domingo));
            }

            recordatorios = recordatorios.GroupBy(e => e.Id).Select(g => g.First()).ToList();

            if (Filtros.ElementoSeleccionado != "Todos")
            {
                for (int i = recordatorios.Count - 1; i >= 0; i--)
                {
                    if (!recordatorios[i].Elementos.Select(e => e.Descripcion).Contains(Filtros.ElementoSeleccionado))
                    {
                        recordatorios.Remove(recordatorios[i]);
                    }
                }
            }

            if (Filtros.MochilaSeleccionada != "Todas")
            {
                for (int i = recordatorios.Count - 1; i >= 0; i--)
                {
                    Mochila mochilaDelFiltro = DatabaseHelper.db.Table<Mochila>().FirstOrDefault(m => m.Descripcion.Equals(Filtros.MochilaSeleccionada));
                    if (recordatorios[i].IdMochila != null && !recordatorios[i].IdMochila.Equals(mochilaDelFiltro.Id))
                    {
                        recordatorios.Remove(recordatorios[i]);
                    }
                    else if (recordatorios[i].IdMochila == null)
                    {
                        recordatorios.Remove(recordatorios[i]);
                    }
                }
            }

            foreach (Recordatorio recordatorio in recordatorios)
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
                Recordatorios.Add(recordatorioViewModel);
            }
        }

    }
}
