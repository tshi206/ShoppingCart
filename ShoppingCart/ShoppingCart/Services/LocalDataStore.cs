using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models;
using SQLite;
using Xamarin.Forms;

namespace ShoppingCart.Services
{
    public class LocalDataStore : IDataStore<Item>
    {
        readonly SQLiteAsyncConnection _sqLiteAsyncConnection;

        static LocalDataStore _database;

        public static LocalDataStore Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new LocalDataStore(DependencyService.Get<IFileHelper>().GetLocalFilePath("CartSQLite.db3"));
                    Debug.WriteLine("New DB connection ESTABLISHED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                else
                {
                    Debug.WriteLine("Using existing DB instance!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                return _database;
            }
        }

        private LocalDataStore(String dbPath)
        {
            _sqLiteAsyncConnection = new SQLiteAsyncConnection(dbPath);
            _sqLiteAsyncConnection.CreateTableAsync<Item>().Wait();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            return await _database.AddItemAsync(item);
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            return await _sqLiteAsyncConnection.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(Item item)
        {
            return await _sqLiteAsyncConnection.DeleteAsync(item);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await _sqLiteAsyncConnection.Table<Item>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await _sqLiteAsyncConnection.Table<Item>().ToListAsync();
        }
    }
}
