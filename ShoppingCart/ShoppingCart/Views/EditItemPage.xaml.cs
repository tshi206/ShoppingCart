using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditItemPage : ContentPage
	{

	    private string OldImageFilePathVersion { get; set; }
	    private int OldImageUrlHash { get; set; }

        private string NewImageFilePathVersion { get; set; }

        public Item Item { get; set; }
		
		public EditItemPage (Item item)
		{
			InitializeComponent ();

			Item = item;
		    OldImageFilePathVersion = item.ImageFilePathVersion;
		    OldImageUrlHash = item.ImageUrl.GetHashCode();

		    NewImageFilePathVersion = item.ImageFilePathVersion;

            BindingContext = this;
		}
		
		async void Save_Clicked(object sender, EventArgs e)
		{
		    int newImageUrlHash = Item.ImageUrl.GetHashCode();
            Debug.WriteLine("old url hash code : " + OldImageUrlHash + " , new url hash code : " + newImageUrlHash);
		    Debug.WriteLine("NewImageFilePathVersion : " + NewImageFilePathVersion);
		    Debug.WriteLine("OldImageFilePathVersion : " + OldImageFilePathVersion);

            if (NewImageFilePathVersion == null)
		    {
		        NewImageFilePathVersion = "null";
		    }

		    if (OldImageFilePathVersion == null)
		    {
		        OldImageFilePathVersion = "null";
		    }

            if (newImageUrlHash != OldImageUrlHash && !NewImageFilePathVersion.Equals(OldImageFilePathVersion))
		    {
		        Item.SourcePath = Item.ImageFilePath;
            }
		    else if (newImageUrlHash == OldImageUrlHash && NewImageFilePathVersion.Equals(OldImageFilePathVersion))
		    {
		        Item.SourcePath = Item.SourcePath;
            }
		    else if (newImageUrlHash == OldImageUrlHash && !NewImageFilePathVersion.Equals(OldImageFilePathVersion))
		    {
		        Item.SourcePath = Item.ImageFilePath;
            }
		    else if (newImageUrlHash != OldImageUrlHash && NewImageFilePathVersion.Equals(OldImageFilePathVersion))
		    {
		        Item.SourcePath = Item.ImageUrl;
            }

		    Item.ImageFilePathVersion = NewImageFilePathVersion;

//		    if (string.IsNullOrEmpty(Item.ImageFilePath))
//		    {
//		        Item.SourcePath = Item.ImageUrl;
//		    }
//            MessagingCenter.Send(this, "EditItem", Item);

            await Navigation.PushAsync(new ImageView(Item, "edit"));
        }

	    async void Back_Clicked(object sender, EventArgs e)
	    {
//	        MessagingCenter.Send(this, "Back", Item);
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
	                
	                NewImageFilePathVersion = Guid.NewGuid().ToString();

	                button.IsEnabled = true;

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