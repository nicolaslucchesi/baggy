using MobileExample.Sincronizacion;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileExample.Tables
{
    [Table("InformacionSincronizada")]
    public class InformacionSincronizada
    {
        public InformacionSincronizada(ModeloRespuesta modeloRespuesta)
        {
            this.Data = modeloRespuesta.Data;
            this.Fecha = DateTime.Now;
        }

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public string Data { get; set; }

        public DateTime Fecha { get; set; }
    }
}
