using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopUI
{
    public class Config : IRocketPluginConfiguration
    {
        public List<Product> Products = new List<Product>();
        public void LoadDefaults()
        {
            Products = new List<Product>();
            Products.Add(new Product { Id=10, Permissions ="default",Price="0",ImageUrl = "https://unturnedhub.com/img/item/200/work-jeans.png" });
        }
    }
    public class Product
    {
        public ushort Id { get; set; }
        public string Price { get; set; }
        public string Permissions { get; set; }
        public string ImageUrl { get; set; }

    }
    public class ProductUser
    {
        public ushort Id { get; set; }
        public string Price { get; set; }
        public int index { get; set; }
        public string ImageUrl { get; set; }

    }
}