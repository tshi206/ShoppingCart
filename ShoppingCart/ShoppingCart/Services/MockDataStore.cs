using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Models;
using ShoppingCart.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(MockDataStore))]
namespace ShoppingCart.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Int32.Parse(Guid.NewGuid().ToString()), Text = "First item", Description="This is an item description.", ImageUrl = "http://via.placeholder.com/256x256", Quantity = 5},
                new Item { Id = Int32.Parse(Guid.NewGuid().ToString()), Text = "Second item", Description="This is an item description.", ImageUrl = "http://via.placeholder.com/256x256", Quantity = 5 },
                new Item { Id = Int32.Parse(Guid.NewGuid().ToString()), Text = "Third item", Description="This is an item description.", ImageUrl = "http://via.placeholder.com/256x256", Quantity = 5 },
                new Item { Id = Int32.Parse(Guid.NewGuid().ToString()), Text = "Fourth item", Description="This is an item description.", ImageUrl = "http://via.placeholder.com/256x256", Quantity = 5 },
                new Item { Id = Int32.Parse(Guid.NewGuid().ToString()), Text = "Fifth item", Description="This is an item description.", ImageUrl = "http://via.placeholder.com/256x256", Quantity = 5 },
                new Item { Id = Int32.Parse(Guid.NewGuid().ToString()), Text = "Sixth item", Description="This is an item description.", ImageUrl = "http://via.placeholder.com/256x256", Quantity = 5 }
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            var _item = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(1);
        }

        public async Task<int> DeleteItemAsync(Item item)
        {
            var _item = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(_item);

            return await Task.FromResult(1);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}