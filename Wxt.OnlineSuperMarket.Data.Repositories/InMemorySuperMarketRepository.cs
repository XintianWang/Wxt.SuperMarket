using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class InMemorySuperMarketRepository : ISuperMarketRepository
    {
        private static readonly List<Product> _products = new List<Product>()
        {
            new Product{ Id = 1, Name = "banana", Category = Category.Grocery, Description = "Banana from Mexico", Price = 1.67m},
            new Product{ Id = 2, Name = "apple", Category = Category.Grocery, Description = "Apple from China", Price = 2.67m},
            new Product{ Id = 3, Name = "Television", Category = Category.Electronic, Description = "Sony 65\"", Price = 1600.59m}
        };
        private static readonly List<ProductItem> _stocks = new List<ProductItem>()
        {
            new ProductItem {ProductId = 1, Count = 100},
            new ProductItem {ProductId = 2, Count = 200},
            new ProductItem {ProductId = 3, Count = 300},
        };
        private static readonly List<Receipt> _receipts = new List<Receipt>();

        private static object _productLocker = new object();

        public static int _productMaxId = 3;

        public Product AddProduct(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name) || product.Price < 0m)
            {
                throw new ArgumentNullException("Cannot create new product.");
            }
            lock(_productLocker)
            {
                product.Id = ++_productMaxId;
            }
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                product.Description = product.Name;
            }
            lock (_products)
            {
                _products.Add(product);
            }
            return product;
        }

        private Product FindProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public bool RemoveProduct(int productId)
        {
            lock (_products)
            {
                var product = FindProduct(productId);
                if (product != null)
                {
                    lock (_stocks)
                    {
                        var stock = _stocks.FirstOrDefault(s => s.ProductId == productId);
                        if (stock != null)
                        {
                            throw new ArgumentNullException("Cannot remove product which is still in stock.");
                        }
                        return _products.Remove(product);
                    }                    
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IncreaseStock(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot increase {count} product to stock.");
            }
            lock (_products)
            {          
                var product = FindProduct(productId);
                if (product != null)
                {
                    lock (_stocks)
                    {
                        var stock = _stocks.FirstOrDefault(s => s.ProductId == productId);
                        if (stock != null)
                        {
                            stock.Count += count;
                        }
                        else
                        {
                            _stocks.Add(new ProductItem() { ProductId = productId, Count = count });
                        }
                    }
                    return true;
                }
                else
                    return false;
            }                
        }

        public bool DecreaseStock(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot decrease {count} product from stock.");
            }
            lock (_products)
            {
                var product = FindProduct(productId);
                if (product != null)
                {
                    lock (_stocks)
                    {
                        var stock = _stocks.FirstOrDefault(s => s.ProductId == productId && s.Count >= count);
                        if (stock != null)
                        {
                            stock.Count -= count;
                            if (stock.Count == 0)
                            {
                                _stocks.Remove(stock);
                            }
                            return true;
                        }
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
        }
    }
}
