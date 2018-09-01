using MobileExample.Database;
using MobileExample.Tables;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace MobileExample.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ObservableCollection<RecordatorioViewModel> Recordatorios { get; set; }
        public Command ComandoCargarRecordatoriosDelDia { get; set; }

        public AboutViewModel()
        {
            Title = "About";
            Recordatorios = new ObservableCollection<RecordatorioViewModel>();

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));

            ComandoCargarRecordatoriosDelDia = new Command(() => EjecutarComandoDelDia());
        }

        public ICommand OpenWebCommand { get; }

        private void EjecutarComandoDelDia()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Recordatorios.Clear();
                var recordatorios = this.ObtenerRecordatoriosDelDia();
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

        private List<RecordatorioViewModel> ObtenerRecordatoriosDelDia()
        {
            List<Recordatorio> recordatorios = new List<Recordatorio>();
            List<RecordatorioViewModel> listadoRecordatorios = new List<RecordatorioViewModel>();

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Lunes).ToList();
                    break;
                case DayOfWeek.Thursday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Martes).ToList();
                    break;
                case DayOfWeek.Wednesday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Miercoles).ToList();
                    break;
                case DayOfWeek.Tuesday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Jueves).ToList();
                    break;
                case DayOfWeek.Friday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Viernes).ToList();
                    break;
                case DayOfWeek.Saturday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Sabado).ToList();
                    break;
                case DayOfWeek.Sunday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Domingo).ToList();
                    break;
                default:
                    break;
            }

            foreach (Recordatorio recordatorio in recordatorios)
            {
                RecordatorioViewModel recordatorioViewModel = new RecordatorioViewModel
                {
                    Id = recordatorio.Id,
                    Horario = recordatorio.Horario,
                    Lunes = recordatorio.Lunes,
                    Martes = recordatorio.Martes,
                    Miercoles = recordatorio.Miercoles,
                    Jueves = recordatorio.Jueves,
                    Viernes = recordatorio.Viernes,
                    Sabado = recordatorio.Sabado,
                    Domingo = recordatorio.Domingo,
                    Descripcion = recordatorio.Descripcion,
                    HorarioStr = recordatorio.Horario.ToString(@"hh\:mm")
                };
                listadoRecordatorios.Add(recordatorioViewModel);
            }
            return listadoRecordatorios;
        }
    }
}