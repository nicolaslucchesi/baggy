using MobileExample.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace MobileExample.ViewModels
{
    public class RecordatorioViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public TimeSpan Horario { get; set; }
        public MochilaViewModel MochilaSeleccionada { get; set; }
        public ListadoElementosViewModel Elementos { get; set; }
        public string HorarioStr { get; set; }
        public bool Lunes { get; set; }
        public bool Martes { get; set; }
        public bool Miercoles { get; set; }
        public bool Jueves { get; set; }
        public bool Viernes { get; set; }

        public Command ComandoEliminarRecordatorio { get; set; }

        public RecordatorioViewModel()
        {
            this.ComandoEliminarRecordatorio = new Command(Eliminar);
            Elementos = new ListadoElementosViewModel();
            MochilaSeleccionada = new MochilaViewModel();
        }

        public RecordatorioViewModel(RecordatorioViewModel viewModel)
        {
            Id = viewModel.Id;
            Horario = viewModel.Horario;
            HorarioStr = viewModel.HorarioStr;
            MochilaSeleccionada = viewModel.MochilaSeleccionada;
            Elementos = viewModel.Elementos;
            Lunes = viewModel.Lunes;
            Martes = viewModel.Martes;
            Miercoles = viewModel.Miercoles;
            Jueves = viewModel.Jueves;
            Viernes = viewModel.Viernes;
            this.ComandoEliminarRecordatorio = new Command(Eliminar);
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
            viewModel.HorarioStr = recordatorio.Horario.ToString(@"hh\:mm");

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
