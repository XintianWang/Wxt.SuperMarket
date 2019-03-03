using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class InMemorySuperMarketRepository
    {
        private static readonly List<Product> _products = new List<Product>()
        {
            new Product{ Id = 1, Name = "banana", Category = Category.Grocery, Description = "Banana from Mexico", Price = 1.67m},
            new Product{ Id = 2, Name = "apple", Category = Category.Grocery, Description = "Apple from China", Price = 2.67m},
            new Product{ Id = 3, Name = "TV", Category = Category.Electronic, Description = "Sony 65\"", Price = 1600.59m}
        };
        private static readonly List<ProductItem> _stock = new List<ProductItem>()
        {
            new ProductItem {ProductId = 1, Count = 100},
            new ProductItem {ProductId = 2, Count = 200},
            new ProductItem {ProductId = 3, Count = 300},
        };
        private static readonly List<Receipt> _receipts = new List<Receipt>();
    }
}
