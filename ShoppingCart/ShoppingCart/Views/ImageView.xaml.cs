using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using ShoppingCart.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImageView : ContentPage
	{

	    private Item Item;
	    private string AddOrEdit;

        public ImageView (Item item, string addOrEdit)
		{
			InitializeComponent();

		    Item = item;
		    AddOrEdit = addOrEdit;
            
		    var cachedImage = new CachedImage()
		    {
		        HorizontalOptions = LayoutOptions.Center,
		        VerticalOptions = LayoutOptions.Center,
		        CacheDuration = TimeSpan.FromDays(30),
		        WidthRequest = 1920,
		        HeightRequest = 1080,
                DownsampleToViewSize = true,
		        RetryCount = 0,
		        RetryDelay = 250,
		        TransparencyEnabled = false,
		        LoadingPlaceholder = "http://via.placeholder.com/256x256",
		        ErrorPlaceholder = "http://via.placeholder.com/256x256",
		        Source = item.SourcePath
		    };

            this.Content = cachedImage;
		}

	    private async void Continue_Clicked(object sender, EventArgs e)
	    {
	        if (AddOrEdit == "add")
	        {
	            MessagingCenter.Send(this, "AddItem", Item);
            }
	        else if (AddOrEdit == "edit")
	        {
	            MessagingCenter.Send(this, "EditItem", Item);
            }
	        
	        await Navigation.PopModalAsync();
        }

	    private async void Back_Clicked(object sender, EventArgs e)
	    {
	        MessagingCenter.Send(this, "Back", Item);
	        await Navigation.PopModalAsync();
        }
	}
}