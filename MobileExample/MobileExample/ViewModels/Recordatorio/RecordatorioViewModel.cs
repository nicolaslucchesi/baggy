using MobileExample.Database;
using MobileExample.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;


namespace MobileExample.ViewModels
{
    public class RecordatorioViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public TimeSpan Horario { get; set; }
        public List<string> Mochilas { get; set; }
        public string MochilaSeleccionada { get; set; }
        public ListadoElementosRecordatorioViewModel Elementos { get; set; }
        public string HorarioStr { get; set; }
        public bool Lunes { get; set; }
        public bool Martes { get; set; }
        public bool Miercoles { get; set; }
        public bool Jueves { get; set; }
        public bool Viernes { get; set; }
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }
        public string Descripcion { get; set; }

        public Command ComandoEliminarRecordatorio { get; set; }

        public RecordatorioViewModel()
        {
            this.ComandoEliminarRecordatorio = new Command(Eliminar);
            Elementos = new ListadoElementosRecordatorioViewModel();
            Horario = new TimeSpan(12, 0, 0);

            Mochilas = new List<string>();
            Mochilas.Add("Ninguna");
            List<string> MochilasGuardadas = DatabaseHelper.db.Table<Mochila>().ToList().Select(e => e.Descripcion).ToList();
            Mochilas.AddRange(MochilasGuardadas);
            MochilaSeleccionada = "Ninguna";

        }

        void Eliminar()
        {
            MessagingCenter.Send(this, "EliminarRecordatorio", this);
        }

        public static explicit operator RecordatorioViewModel(Recordatorio recordatorio)
        {
            RecordatorioViewModel viewModel = new RecordatorioViewModel();
            viewModel.Lunes = recordatorio.Lunes;
            viewModel.Martes = recordatorio.Martes;
            viewModel.Miercoles = recordatorio.Miercoles;
            viewModel.Jueves = recordatorio.Jueves;
            viewModel.Viernes = recordatorio.Viernes;
            viewModel.Sabado = recordatorio.Sabado;
            viewModel.Domingo = recordatorio.Domingo;
            viewModel.HorarioStr = recordatorio.Horario.ToString(@"hh\:mm");
            viewModel.Descripcion = recordatorio.Descripcion;


            foreach (Elemento elemento in recordatorio.Elementos)
            {
                viewModel.Elementos.Elementos.Add(new ElementoViewModel
                {
                    Descripcion = elemento.Descripcion,
                    Imprescindible = elemento.Imprescindible,
                    RutaIcono = elemento.RutaIcono,
                    Vinculado = elemento.Vinculado
                });
            }
            return viewModel;
        }

    }
}
