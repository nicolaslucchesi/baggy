using MobileExample.Database;
using MobileExample.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileExample.ViewModels
{
    public class FiltrosRecordatorioViewModel
    {
        public List<string> Elementos { get; set; }
        public string ElementoSeleccionado { get; set; }
        public List<string> Mochilas { get; set; }
        public string MochilaSeleccionada { get; set; }
        public TimeSpan Horario { get; set; }
        public bool Lunes { get; set; }
        public bool Martes { get; set; }
        public bool Miercoles { get; set; }
        public bool Jueves { get; set; }
        public bool Viernes { get; set; }
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }

        public FiltrosRecordatorioViewModel()
        {
            Elementos = new List<string>();
            Elementos.Add("Todos");
            List<string> ElementosGuardados = DatabaseHelper.db.Table<Elemento>().ToList().Select(e => e.Descripcion).ToList();
            Elementos.AddRange(ElementosGuardados);
            ElementoSeleccionado = "Todos";

            Mochilas = new List<string>();
            Mochilas.Add("Todas");
            List<string> MochilasGuardadas = DatabaseHelper.db.Table<Mochila>().ToList().Select(e => e.Descripcion).ToList();
            Mochilas.AddRange(MochilasGuardadas);
            MochilaSeleccionada = "Todas";

            Lunes = true;
            Martes = true;
            Miercoles = true;
            Jueves = true;
            Viernes = true;
            Sabado = true;
            Domingo = true;
        }
    }
}
