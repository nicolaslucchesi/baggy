using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobileExample.Database
{
    public static class DatabaseHelper
    {
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");

        public static SQLiteConnection db = new SQLiteConnection(path);
    }
}
