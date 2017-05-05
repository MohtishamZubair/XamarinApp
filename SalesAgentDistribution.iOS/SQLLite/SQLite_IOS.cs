using SalesAgentDistribution.Helper;
using SalesAgentDistribution.iOS.SQLLite;
using SalesAgentDistribution.SQLLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: Xamarin.Forms.Dependency (typeof (SQLite_iOS))]
namespace SalesAgentDistribution.iOS.SQLLite
{
    
// ...
public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = AppHelper.DB_FILE_NAME;
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}
