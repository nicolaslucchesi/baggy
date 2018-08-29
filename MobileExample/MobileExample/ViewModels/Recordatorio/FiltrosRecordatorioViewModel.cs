using MobileExample.Database;
using MobileExample.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MobileExample.ViewModels
{
    public class FiltrosRecordatorioViewModel
    {
        public ObservableCollection<string> Elementos { get; set; }
        public string ElementoSeleccionado { get; set; }
        public ObservableCollection<string> Mochilas { get; set; }
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
            Elementos = new ObservableCollection<string>();
            Elementos.Add("Todos");
            List<string> ElementosGuardados = DatabaseHelper.db.Table<Elemento>().ToList().Select(e => e.Descripcion).ToList();
            foreach (string elemento in ElementosGuardados)
            {
                Elementos.Add(elemento);
            }
            ElementoSeleccionado = "Todos";

            Mochilas = new ObservableCollection<string>();
            Mochilas.Add("Todas");
            List<string> MochilasGuardadas = DatabaseHelper.db.Table<Mochila>().ToList().Select(e => e.Descripcion).ToList();
            foreach (string mochila in MochilasGuardadas)
            {
                Mochilas.Add(mochila);
            }
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
