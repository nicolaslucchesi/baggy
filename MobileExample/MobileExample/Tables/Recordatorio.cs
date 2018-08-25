﻿using SQLite;
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

        public string Descripcion { get; set; }

        public TimeSpan Horario { get; set; }

        public int IdMochila { get; set; }

        public bool Lunes { get; set; }

        public bool Martes { get; set; }

        public bool Miercoles { get; set; }

        public bool Jueves { get; set; }

        public bool Viernes { get; set; }

        public bool Sabado { get; set; }

        public bool Domingo { get; set; }

        [ManyToMany(typeof(ElementoRecordatorio))]
        public List<Elemento> Elementos { get; set; }
    }
}
