using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ShoppingCart.Models;
using ShoppingCart.Views;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
	{
	    
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

	    async void Edit_Clicked(object sender, EventArgs e)
	    {
		    var menuItem = sender as MenuItem;
		    if (menuItem != null)
		    {
			    var item = menuItem.CommandParameter as Item;
			    if (item != null)
			    {
				    Debug.WriteLine(item.Id);
				    Debug.WriteLine(item.Description);
				    Debug.WriteLine(item.ImageUrl);
				    Debug.WriteLine(item.Quantity.ToString());
				    await Navigation.PushModalAsync(new NavigationPage(new EditItemPage(item)));
			    }
			    
			    
			    //viewModel.LoadItemsCommand.Execute(null);
		    }
	    }

	    async void Delete_Clicked(object sender, EventArgs e)
	    {
	        Debug.WriteLine(sender.ToString());
	        Debug.WriteLine(e.ToString());
	        var menuItem = sender as MenuItem;
		    if (menuItem != null)
		    {
			    var item = menuItem.CommandParameter as Item;
			    if (item != null)
			    {
				    Debug.WriteLine(item.Id);
				    Debug.WriteLine(item.Description);
				    Debug.WriteLine(item.ImageUrl);
				    Debug.WriteLine(item.Quantity.ToString());
				    await viewModel.DataStore.DeleteItemAsync(item);
				    viewModel.LoadItemsCommand.Execute(null);
			    }
		    }
			
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}