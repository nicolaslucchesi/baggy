using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.Tables
{
    [Table("Configuracion")]
    public class Configuracion
    {
        public bool Vinculando { get; set; }
    }
}
