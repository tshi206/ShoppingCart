using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.SystemFunctions;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Services
{
    public class AzureCosmosStore : IDataStore<Item>
    {

        public static AzureCosmosStore CosmosDbStore { get; } = new AzureCosmosStore();

        public List<Item> Items { get; private set; }

        private const string AccountUrl = @"https://xamarinshoppingcartcosmosdb.documents.azure.com:443/";
        private const string AccountKey = @"TJzZ2ajP1gAUMDMel7Wzm4rcACnmJj2apVGy4sum1VkrY96dM1EfhK0LKBK4PjFoSYLw0DhmyflSMF7QSRt6Zw==";
        private const string DatabaseId = @"ToDoList";
        private const string CollectionId = @"Items";

        private readonly Uri _collectionLink = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

        private readonly DocumentClient _client;

        private AzureCosmosStore()
        {
            _client = new DocumentClient(new System.Uri(AccountUrl), AccountKey);
        }


        public async Task<bool> AddItemAsync(Item item)
        {

            try
            {
                return await _client.CreateDocumentAsync(_collectionLink, item).ContinueWith(t =>
                {
                    Debug.WriteLine(t.Result);
                    Debug.WriteLine(t.Result.Resource.Id);
                    Debug.WriteLine(t.Result.IsNull());
                    Debug.WriteLine(t.Result.Resource.IsDefined());
                    return true;
                });
                
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return false;
            }
            
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            try
            {
                return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, item.id), item).ContinueWith(t => t.Result.GetHashCode());

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return 0;
            }
        }

        public async Task<int> DeleteItemAsync(Item item)
        {
            try
            {

                return await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, item.id)).ContinueWith(t => t.Result.GetHashCode());

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return 0;
            }
        }

        public async Task<Item> GetItemAsync(int id)
        {
            try
            {
                
                var query = _client.CreateDocumentQuery<Item>(_collectionLink, new FeedOptions { MaxItemCount = 1 })
                    .Where(item => item.Id == id)
                    .AsDocumentQuery();

                Items = new List<Item>();
                while (query.HasMoreResults)
                {
                    Items.AddRange(await query.ExecuteNextAsync<Item>());
                }


            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items.First();
        }

        public async Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                
                var query = _client.CreateDocumentQuery<Item>(_collectionLink)
                    .Where(item => item.Uid.Equals(AccountViewModel.UserID))
                    .AsDocumentQuery();

                Items = new List<Item>();
                while (query.HasMoreResults)
                {
                    Items.AddRange(await query.ExecuteNextAsync<Item>());
                }
                Debug.WriteLine(Items.Count);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items;
        }
    }
}
