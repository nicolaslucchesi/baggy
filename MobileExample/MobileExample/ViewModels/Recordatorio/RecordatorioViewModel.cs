using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace MobileExample.ViewModels
{
    public class RecordatorioViewModel: BaseViewModel
    {
        public int Id { get; set; }

        public int DiaSemana { get; set; }

        public TimeSpan Horario { get; set; }
        public string HorarioStr { get; set; }

        public int Minuto { get; set; }
        public int Hora { get; set; }

        public int IdMochila { get; set; }

        public List<int> Elementos { get; set; }



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

        public RecordatorioViewModel(RecordatorioViewModel a)
        {
            Id = a.Id;
            DiaSemana = a.DiaSemana;
            Horario = a.Horario;
            HorarioStr = a.HorarioStr;
            Minuto = a.Minuto;
            Hora = a.Hora;

            IdMochila = a.IdMochila;
            Elementos = a.Elementos;
            Lunes = a.Lunes;
            Martes = a.Martes;
            Miercoles = a.Miercoles;
            Jueves = a.Jueves;
            Viernes = a.Viernes;


            this.ComandoEliminarRecordatorio = new Command(Eliminar);
        }

        void Eliminar()
        {
            MessagingCenter.Send(this, "EliminarRecordatorio", this);
        }
        
    }
}
