using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description.",
                ImageUrl = "https://...",
                Quantity = 1
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Item.ImageFilePath))
            {
                Item.SourcePath = Item.ImageUrl;
            }
//            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PushAsync(new ImageView(Item, "add"));
        }

        async void Back_Clicked(object sender, EventArgs e)
        {
//            MessagingCenter.Send(this, "Back", Item);
            await Navigation.PopModalAsync();
        }

        private async void PickPictureAsync(object sender, EventArgs e)
        {

            using (UserDialogs.Instance.Loading("Loading you image...\n" +
                                                "Please wait UNTIL the completion dialog shows up!",
                null, null, true, MaskType.Black))
            {

                Debug.WriteLine(sender.ToString());
                Debug.WriteLine(e.ToString());
                Button button = sender as Button;

                button.IsEnabled = false;
                Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

                if (stream != null)
                {
                    
                    Debug.WriteLine("streamCopy.CanRead = " + stream.CanRead);
                    Debug.WriteLine("streamCopy.CanWrite = " + stream.CanWrite);
                    Debug.WriteLine(DependencyService.Get<ILocalUserFolderLocator>());

                    string localPath = DependencyService.Get<ILocalUserFolderLocator>()
                        .GetPathToLocalImageDir("img_" + Guid.NewGuid(), stream);
                    Item.ImageFilePath = localPath;
                    Item.SourcePath = localPath;

                    await DisplayAlert("Done!", "Your image has been selected", "cool");
                }
                else
                {
                    button.IsEnabled = true;
                }
            }

        }
    }
}