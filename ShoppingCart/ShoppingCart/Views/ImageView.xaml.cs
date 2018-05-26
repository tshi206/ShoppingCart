using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImageView : ContentPage
	{
		public ImageView (Image image)
		{
			InitializeComponent();

		    this.Content = image;
		}
	}
}