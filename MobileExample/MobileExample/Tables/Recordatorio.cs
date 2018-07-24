using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.Tables
{
    [Table("Recordatorio")]
    public class Recordatorio
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public int DiaSemana { get; set; }

        public int Hora { get; set; }

        public int Minuto { get; set; }

        public int IdMochila { get; set; }
        
        [ManyToMany(typeof(ElementoRecordatorio))]
        public List<Elemento> Elementos { get; set; }
    }
}
