using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SalesAgentDistribution.SQLLite;
using SalesAgentDistribution.Droid.SQLLite;
using System.IO;
using SalesAgentDistribution.Helper;

[assembly: Xamarin.Forms.Dependency(typeof(SQLite_Android))]
namespace SalesAgentDistribution.Droid.SQLLite
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = AppHelper.DB_FILE_NAME;
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}