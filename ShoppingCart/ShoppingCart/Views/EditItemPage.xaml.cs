using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditItemPage : ContentPage
	{
		public Item Item { get; set; }
		
		public EditItemPage (Item item)
		{
			InitializeComponent ();

			Item = item;
			
			BindingContext = this;
		}
		
		async void Save_Clicked(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "EditItem", Item);
			await Navigation.PopModalAsync();
		}
	}
}