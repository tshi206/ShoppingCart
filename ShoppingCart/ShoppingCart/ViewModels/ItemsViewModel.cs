using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using ShoppingCart.Models;
using ShoppingCart.Services;
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

                if (AccountViewModel.UserID != null)
                {
                    
                    item.Uid = AccountViewModel.UserID;
                    item.Id = (Guid.NewGuid().ToString().GetHashCode() + Guid.NewGuid().ToString().GetHashCode() + Guid.NewGuid().ToString().GetHashCode() + Guid.NewGuid().ToString().GetHashCode())/4;

                    if (!item.SourcePath.StartsWith("http")) // if source path is a local file path
                    {
                        // change the source path to a remote url behind the scene so that it can be stored in remote db
                        await BlobService.AzureBlobService.UploadFileAsync(item.SourcePath, item);
                    }

                    // just save the url to cloud DB, no need for blob storage
                    await CosmosDataStore.AddItemAsync(item);
                }
                else
                {
                    bool result = await DataStore.AddItemAsync(item);
                    if (!result)
                    {
                        Debug.WriteLine("CANNOT ADD NEW ITEM!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    }
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
                
                if (item.Uid != null) // it belongs to a authenticated user, not the local file system
                {
                    await CosmosDataStore.UpdateItemAsync(item);
                }
                else
                {
                    await DataStore.UpdateItemAsync(item).ContinueWith(t =>
                    {
                        Debug.WriteLine("Edit result : " + t.Result);
                        LoadItemsCommand.Execute(null);
                    });
                }
                
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
                
                
                if (AccountViewModel.UserID != null) // authenticated mode
                {
                    await CosmosDataStore.GetItemsAsync().ContinueWith(async t =>
                    {
                        foreach (var item in t.Result)
                        {
                            Debug.WriteLine("Id: " + item.Id);
                            Debug.WriteLine("Text (name) : " + (item.Text ?? "null"));
                            Debug.WriteLine("Description: " + (item.Description ?? "null"));
                            Debug.WriteLine("SourcePath: " + (item.SourcePath ?? "null"));
                            Debug.WriteLine("ImageUrl: " + (item.ImageUrl ?? "null"));
                            Debug.WriteLine("Quantity: " + item.Quantity);
                            Debug.WriteLine("ImageFilePath: " + (item.ImageFilePath ?? "null"));
                            Debug.WriteLine("Uid: " + (item.Uid ?? "null"));
                            Debug.WriteLine("ImageFilePathVersion: " + (item.ImageFilePathVersion ?? "null"));
                            Items.Add(item);
                        }
                        await DataStore.GetItemsAsync().ContinueWith(t1 =>
                        {
                            t1.Result.ForEach(i =>
                            {
                                Items.Add(i);
                            });
                        });
                    });
                    
                }
                else // local mode
                {
                    await DataStore.GetItemsAsync(true).ContinueWith(t =>
                    {
                        foreach (var item in t.Result)
                        {
                            Debug.WriteLine("Id: " + item.Id);
                            Debug.WriteLine("Text (name) : " + (item.Text ?? "null"));
                            Debug.WriteLine("Description: " + (item.Description ?? "null"));
                            Debug.WriteLine("SourcePath: " + (item.SourcePath ?? "null"));
                            Debug.WriteLine("ImageUrl: " + (item.ImageUrl ?? "null"));
                            Debug.WriteLine("Quantity: " + item.Quantity);
                            Debug.WriteLine("ImageFilePath: " + (item.ImageFilePath ?? "null"));
                            Debug.WriteLine("Uid: " + (item.Uid ?? "null"));
                            Debug.WriteLine("ImageFilePathVersion: " + (item.ImageFilePathVersion ?? "null"));
                            Items.Add(item);
                        }
                    });
                }
                

                // debug printlns for blobs
                await BlobService.AzureBlobService.GetAllBlobUrisAsync();

                if (isStartup && Items.Count == 0)
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