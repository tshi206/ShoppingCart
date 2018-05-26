using System;
using SQLite;

namespace ShoppingCart.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string SourcePath { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Uid { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFilePath { get; set; }
        public string ImageFilePathVersion { get; set; }

        public Item()
        {
            ImageFilePathVersion = Guid.NewGuid().ToString();
        }
    }
}