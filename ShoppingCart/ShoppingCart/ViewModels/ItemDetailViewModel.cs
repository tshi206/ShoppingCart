using System;

using ShoppingCart.Models;

namespace ShoppingCart.ViewModels
{
    public class ItemDetailViewModel : CartViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
