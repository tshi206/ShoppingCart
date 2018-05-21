using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using ShoppingCart.iOS;
using ShoppingCart.Services;
using SQLite;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseService))]
namespace ShoppingCart.iOS
{
    public class DatabaseService : IDBInterface
    {
        public DatabaseService()
        {
        }

        public string GetLocalDBPath(string filename, string extension)
        {
            // var sqliteFilename = "CartSQLite.db3";
            var sqliteFilename = filename + "." + extension;
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            string path = Path.Combine(libFolder, sqliteFilename);

            // This is where we copy in the pre-created database
            if (!File.Exists(path))
            {
                var existingDb = NSBundle.MainBundle.PathForResource(filename, extension);
                File.Copy(existingDb, path);
            }

            // var connection = new SQLiteConnection(path);

            // Return the database path 
            return path;
        }
    }
}