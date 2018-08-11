using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MobileExample.Tables
{
    [Table("ElementoAgregado")]
    public class ElementoAgregado
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public string UUID { get; set; }
    }
}
