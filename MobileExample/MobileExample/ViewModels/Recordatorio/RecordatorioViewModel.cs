using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace MobileExample.ViewModels
{
    public class RecordatorioViewModel: BaseViewModel
    {
        public int Id { get; set; }

        public TimeSpan Horario { get; set; }
        public int IdMochila { get; set; }
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
        }

        public RecordatorioViewModel(RecordatorioViewModel viewModel)
        {
            Id = viewModel.Id;
            Horario = viewModel.Horario;
            HorarioStr = viewModel.HorarioStr;
            IdMochila = viewModel.IdMochila;
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
        
    }
}
