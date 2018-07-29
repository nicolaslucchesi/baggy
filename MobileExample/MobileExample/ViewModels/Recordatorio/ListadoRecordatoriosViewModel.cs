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
                string HoraStr ;
                string MinutoStr;
                string HrStr;

                if (recordatorioViewModel.Horario.Hours < 10)
                {
                    HoraStr = "0" + recordatorioViewModel.Horario.Hours;
                }
                else
                {
                    HoraStr = "" + recordatorioViewModel.Horario.Hours;
                }

                if (recordatorioViewModel.Horario.Minutes < 10)
                {
                    MinutoStr = "0" + recordatorioViewModel.Horario.Minutes;
                }
                else
                {
                    MinutoStr = "" + recordatorioViewModel.Horario.Minutes;
                }

                HrStr = HoraStr + ":" + MinutoStr;


                
                Recordatorio recordatorio = new Recordatorio
                {
                    DiaSemana = 5,
                    Minuto = recordatorioViewModel.Horario.Minutes,
                    Hora = recordatorioViewModel.Horario.Hours,
                    Horario = recordatorioViewModel.Horario,
                    Lunes = recordatorioViewModel.Lunes,
                    Martes = recordatorioViewModel.Martes,
                    Miercoles = recordatorioViewModel.Miercoles,
                    Jueves = recordatorioViewModel.Jueves,
                    Viernes = recordatorioViewModel.Viernes,
                    HorarioStr = HrStr
                 };

                recordatorioViewModel.HorarioStr = HrStr;
                db.Insert(recordatorio);
                Recordatorios.Add(recordatorioViewModel);
            });

            MessagingCenter.Subscribe<RecordatorioViewModel, RecordatorioViewModel>(this, "EliminarRecordatorio", (sender, recordatorioViewModel) =>
            {
                Recordatorio recordatorioAEliminar = db.Table<Recordatorio>().Where(e => e.Horario.Equals(recordatorioViewModel.Horario)).FirstOrDefault();
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
            string HoraStr;
            string MinutoStr;
            string HorarioStr;

            foreach (Recordatorio recordatorio in db.Table<Recordatorio>().ToList())
            {
                RecordatorioViewModel recordatorioViewModel = new RecordatorioViewModel();
                recordatorioViewModel.Id = recordatorio.Id;
                recordatorioViewModel.DiaSemana = recordatorio.DiaSemana;
                recordatorioViewModel.Minuto = recordatorio.Minuto;
                recordatorioViewModel.Hora = recordatorio.Hora;
                recordatorioViewModel.Horario = recordatorio.Horario;
                recordatorioViewModel.Lunes = recordatorio.Lunes;
                recordatorioViewModel.Martes = recordatorio.Martes;
                recordatorioViewModel.Miercoles = recordatorio.Miercoles;
                recordatorioViewModel.Jueves = recordatorio.Jueves;
                recordatorioViewModel.Viernes = recordatorio.Viernes;

                if(recordatorio.Hora < 10)
                {
                    HoraStr = "0" + recordatorio.Hora;
                }
                else
                {
                    HoraStr = "" + recordatorio.Hora;
                }

                if (recordatorio.Minuto < 10)
                {
                    MinutoStr = "0" + recordatorio.Minuto;
                }
                else
                {
                    MinutoStr = "" + recordatorio.Minuto;
                }

                HorarioStr = HoraStr + ":" + MinutoStr;


                recordatorioViewModel.HorarioStr = HorarioStr;
                listadoRecordatorios.Add(recordatorioViewModel);
            }
            return listadoRecordatorios;
            
        }
    }
}
