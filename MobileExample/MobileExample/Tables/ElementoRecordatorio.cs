using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.Tables
{
    public class ElementoRecordatorio
    {
        [ForeignKey(typeof(Elemento))]
        public int IdElemento { get; set; }

        [ForeignKey(typeof(Recordatorio))]
        public int IdRecordatorio { get; set; }
    }
}
