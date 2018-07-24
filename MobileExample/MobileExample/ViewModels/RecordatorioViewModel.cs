using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.ViewModels
{
    public class RecordatorioViewModel
    {
        public int Id { get; set; }

        public int DiaSemana { get; set; }

        public int Hora { get; set; }

        public int Minuto { get; set; }

        public int IdMochila { get; set; }

        public List<int> Elementos { get; set; }
    }
}
