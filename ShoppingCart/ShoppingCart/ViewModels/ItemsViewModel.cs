using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using ShoppingCart.Models;
using ShoppingCart.Views;
using Xamarin.Forms;

namespace ShoppingCart.ViewModels
{
    public class ItemsViewModel : CartViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        private bool isStartup;

        public ItemsViewModel()
        {
            isStartup = true;
            Title = "Shopping Cart";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ImageView, Item>(this, "AddItem", async (obj, item) =>
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

//                var cachedImage = new CachedImage()
//                {
//                    HorizontalOptions = LayoutOptions.Center,
//                    VerticalOptions = LayoutOptions.Center,
//                    WidthRequest = 256,
//                    HeightRequest = 256,
//                    CacheDuration = TimeSpan.FromDays(30),
//                    DownsampleWidth = 256,
//                    RetryCount = 0,
//                    RetryDelay = 250,
//                    TransparencyEnabled = false,
//                    LoadingPlaceholder = "http://via.placeholder.com/256x256",
//                    ErrorPlaceholder = "http://via.placeholder.com/256x256",
//                    Source = item.ImageFilePath
//                };

            });
            
            MessagingCenter.Subscribe<ImageView, Item>(this, "EditItem", async (obj, item) =>
            {
                Debug.WriteLine(obj.ToString());
                Debug.WriteLine(item.ToString());
                int index = Items.IndexOf(item);
                Items.Remove(item);
                Items.Insert(index, item);
                await DataStore.UpdateItemAsync(item).ContinueWith(t =>
                {
                    Debug.WriteLine("Edit result : " + t.Result);
                    LoadItemsCommand.Execute(null);
                });
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
                    Debug.WriteLine("Id: " + item.Id);
                    Debug.WriteLine("Text (name) : " + (item.Text ?? "null"));
                    Debug.WriteLine("Description: " + (item.Description ?? "null"));
                    Debug.WriteLine("SourcePath: " + (item.SourcePath ?? "null"));
                    Debug.WriteLine("ImageUrl: " + (item.ImageUrl ?? "null"));
                    Debug.WriteLine("Quantity: " + item.Quantity);
                    Debug.WriteLine("ImageFilePath: " + (item.ImageFilePath ?? "null"));
                    Debug.WriteLine("Uid: " + (item.Uid??"null"));
                    Items.Add(item);
                }

                if (isStartup && items.Count == 0)
                {
                    isStartup = false;
                    MessagingCenter.Send(this, "EmptyCart", true);
                }
                else
                {
                    isStartup = false;
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