using System;

namespace ShoppingCart.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}