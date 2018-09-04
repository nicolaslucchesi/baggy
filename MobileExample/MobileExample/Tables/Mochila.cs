﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.Tables
{
    [Table("Mochila")]
    public class Mochila
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string UUID { get; set; }

        public bool Activa { get; set; }

        public bool EstadoAlarma { get; set; }

        public bool Abierta { get; set; }

        public string Elementos { get; set; }

        public string RutaIcono { get; set; }
    }
}
