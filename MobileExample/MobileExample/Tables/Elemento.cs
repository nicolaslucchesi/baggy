using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.Tables
{
    [Table("Elemento")]
    public class Elemento
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string RutaIcono { get; set; }

        public bool Imprescindible { get; set; }

        public bool Vinculado { get; set; }

        public string UUID { get; set; }

        [ManyToMany(typeof(ElementoRecordatorio))]
        public List<Recordatorio> Recordatorios { get; set; }
    }
}
