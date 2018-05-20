using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ShoppingCart.ViewModels
{
    public class AccountViewModel : CartViewModel
    {
        public AccountViewModel()
        {
            Title = "Account";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}