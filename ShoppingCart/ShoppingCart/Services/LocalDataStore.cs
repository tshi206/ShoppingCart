using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    // local DB file : "CartSQLite.db3"
                    if (DependencyService.Get<IDBInterface>() == null)
                    {
                        Debug.WriteLine("DependencyService.Get<IDBInterface>() is null");
                    }

                    if (DependencyService.Get<IDBInterface>().GetLocalDBPath("CartSQLite", "db3") == null)
                    {
                        Debug.WriteLine("DependencyService.Get<IDBInterface>().GetLocalDBPath('CartSQLite', 'db3') is null");
                    }
                    // Debug.WriteLine(DependencyService.Get<IDBInterface>());
                    // Debug.WriteLine(DependencyService.Get<IDBInterface>().GetLocalDBPath("CartSQLite", "db3"));
                    _database = new LocalDataStore(DependencyService.Get<IDBInterface>().GetLocalDBPath("CartSQLite", "db3"));
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
            _sqLiteAsyncConnection.CreateTableAsync<Item>().ContinueWith(t => {
                if (t.IsCompleted)
                {
                    Debug.WriteLine("Table created!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Debug.WriteLine("Table Result: " + t.Result.ToString());
                }
                else
                {
                    Debug.WriteLine("TABLE CREATE : FAILED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
            }).Wait();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            Debug.WriteLine("Start adding a new item!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            return await await _sqLiteAsyncConnection.InsertAsync(item).ContinueWith(t =>
            {
                return new Task<bool>(() =>
                {
                    if (t.IsCanceled){ Debug.WriteLine("ADD ITEM FAIL : CANCELED!!!!!!!!!!!!!!!!!!!!!!"); return false; }
                    if (t.IsFaulted){ Debug.WriteLine("ADD ITEM FAIL : FAULTED!!!!!!!!!!!!!!!!!!!!!!"); return false; }
                    if (t.IsCompleted){ Debug.WriteLine("ADD ITEM SUCCEEDED!!!!!!!!!!!!!!!!!!!!!!"); return true; }
                    Debug.WriteLine("SOMETHING WRONG WHEN ADDING ITEM!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    return false;
                });
            });
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            return await _sqLiteAsyncConnection.UpdateAsync(item);
//            Debug.WriteLine("Start editting an item!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
//            return await await _sqLiteAsyncConnection.UpdateAsync(item).ContinueWith(t =>
//            {
//                return new Task<int>(() =>
//                {
//                    if (t.IsCanceled) { Debug.WriteLine("EDIT ITEM FAIL : CANCELED!!!!!!!!!!!!!!!!!!!!!!"); return t.Result; }
//                    if (t.IsFaulted) { Debug.WriteLine("EDIT ITEM FAIL : FAULTED!!!!!!!!!!!!!!!!!!!!!!"); return t.Result; }
//                    if (t.IsCompleted) { Debug.WriteLine("EDIT ITEM SUCCEEDED!!!!!!!!!!!!!!!!!!!!!!"); return t.Result; }
//                    Debug.WriteLine("SOMETHING WRONG WHEN EDITTING ITEM!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
//                    return t.Result;
//                });
//            });
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
