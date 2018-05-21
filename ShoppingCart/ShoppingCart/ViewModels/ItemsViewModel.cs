using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ShoppingCart.Models;
using ShoppingCart.Views;
using Xamarin.Forms;

namespace ShoppingCart.ViewModels
{
    public class ItemsViewModel : CartViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Shopping Cart";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                if (item == null)
                {
                    Debug.WriteLine("new item is null!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }

                if (obj == null)
                {
                    Debug.WriteLine("obj is null!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }

                Debug.WriteLine(obj.ToString());
                Debug.WriteLine(item.ToString());

                Items.Add(item);
                bool result = await DataStore.AddItemAsync(item);
                if (!result)
                {
                    Debug.WriteLine("CANNOT ADD NEW ITEM!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
            });
            
            MessagingCenter.Subscribe<NewItemPage, Item>(this, "EditItem", async (obj, item) =>
            {
                await DataStore.UpdateItemAsync(item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}