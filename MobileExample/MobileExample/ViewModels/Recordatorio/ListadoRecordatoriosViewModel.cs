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
    public class ListadoRecordatoriosViewModel : BaseViewModel
    {
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
        private static SQLiteConnection db = new SQLiteConnection(path);

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
                    DiaSemana = 5,
                    Minuto = recordatorioViewModel.Minuto,
                    Hora = recordatorioViewModel.Hora.Hours,
                    Lunes = recordatorioViewModel.Lunes,
                    Martes = recordatorioViewModel.Martes,
                    Miercoles = recordatorioViewModel.Miercoles,
                    Jueves = recordatorioViewModel.Jueves,
                    Viernes = recordatorioViewModel.Viernes
                };

                db.Insert(recordatorio);
                Recordatorios.Add(recordatorioViewModel);
            });

            MessagingCenter.Subscribe<RecordatorioViewModel, RecordatorioViewModel>(this, "EliminarRecordatorio", (sender, recordatorioViewModel) =>
            {
                Recordatorio recordatorioAEliminar = db.Table<Recordatorio>().Where(e => e.Id.Equals(recordatorioViewModel.Id)).FirstOrDefault();
                db.Delete(recordatorioAEliminar);
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
            foreach (Recordatorio recordatorio in db.Table<Recordatorio>().ToList())
            {
                RecordatorioViewModel recordatorioViewModel = new RecordatorioViewModel();
                recordatorioViewModel.Id = recordatorio.Id;
                recordatorioViewModel.DiaSemana = recordatorio.DiaSemana;
                recordatorioViewModel.Minuto = recordatorio.Minuto;
                recordatorioViewModel.Lunes = recordatorio.Lunes;
                recordatorioViewModel.Martes = recordatorio.Martes;
                recordatorioViewModel.Miercoles = recordatorio.Miercoles;
                recordatorioViewModel.Jueves = recordatorio.Jueves;
                recordatorioViewModel.Viernes = recordatorio.Viernes;
                listadoRecordatorios.Add(recordatorioViewModel);
            }
            return listadoRecordatorios;
            
        }
    }
}
