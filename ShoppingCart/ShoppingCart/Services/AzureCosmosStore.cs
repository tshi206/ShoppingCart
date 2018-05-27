using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class AzureCosmosStore : IDataStore<Item>
    {

        public static AzureCosmosStore CosmosDbStore { get; } = new AzureCosmosStore();

        private AzureCosmosStore()
        {

        }


        public async Task<bool> AddItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public async Task<Item> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }
    }
}
