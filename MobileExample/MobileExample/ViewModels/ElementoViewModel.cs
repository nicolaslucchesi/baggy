using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.ViewModels
{
    public class ElementoViewModel
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string RutaIcono { get; set; }

        public bool Imprescindible { get; set; }

        public bool Vinculado { get; set; }

        public string UUID { get; set; }
    }
}
