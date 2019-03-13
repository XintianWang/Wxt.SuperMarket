namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wxt.OnlineSuperMarket.Data.Entities;

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

        public static int _productCurrentId = 3;

        private static readonly object _productIdLocker = new object();

        private static readonly object _productsLocker = new object();

        private static readonly object _stockLocker = new object();

#if DEBUG
        public void ReinitializeRepository()
        {
            _products.Clear();
            _products.AddRange(
                new List<Product> {
                    new Product { Id = 1, Name = "banana", Category = Category.Grocery, Description = "Banana from Mexico", Price = 1.67m },
                    new Product { Id = 2, Name = "apple", Category = Category.Grocery, Description = "Apple from China", Price = 2.67m },
                    new Product { Id = 3, Name = "Television", Category = Category.Electronic, Description = "Sony 65\"", Price = 1600.59m }
                }
            );
            _stocks.Clear();
            _stocks.AddRange(
                new List<ProductItem>{
                    new ProductItem { ProductId = 1, Count = 100 },
                    new ProductItem { ProductId = 2, Count = 200 },
                    new ProductItem { ProductId = 3, Count = 300 },
                });
            _receipts.Clear();
            _productCurrentId = 3;
        }
#endif

        public Product AddProduct(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name) || product.Price < 0m)
            {
                throw new ArgumentNullException("Cannot create new product.");
            }
            lock (_productIdLocker)
            {
                product.Id = ++_productCurrentId;
            }
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                product.Description = product.Name;
            }
            lock (_productsLocker)
            {
                _products.Add(product);
            }
            return product;
        }

        public Product FindProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void RemoveProduct(int productId)
        {
            lock (_productsLocker)
            {
                var product = FindProduct(productId);
                if (product != null)
                {
                    lock (_stockLocker)
                    {
                        var stock = _stocks.FirstOrDefault(s => s.ProductId == productId);
                        if (stock != null)
                        {
                            throw new InvalidOperationException("Cannot remove product which is still in stock.");
                        }
                        if (!_products.Remove(product))
                        {
                            throw new InvalidOperationException("Unknow problem.");
                        }
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException($"Product {productId} does not exist.");
                }
            }
        }

        public void IncreaseStock(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot increase {count} product to stock.");
            }
            lock (_productsLocker)
            {
                var product = FindProduct(productId);
                if (product != null)
                {
                    lock (_stockLocker)
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
                }
                else
                {
                    throw new IndexOutOfRangeException($"Product {productId} does not exist.");
                }
            }
        }

        public void DecreaseStock(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot decrease {count} product from stock.");
            }
            lock (_productsLocker)
            {
                var product = FindProduct(productId);
                if (product != null)
                {
                    lock (_stockLocker)
                    {
                        var stock = _stocks.FirstOrDefault(s => s.ProductId == productId && s.Count >= count);
                        if (stock != null)
                        {
                            stock.Count -= count;
                            if (stock.Count == 0)
                            {
                                _stocks.Remove(stock);
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException($"Product {productId} is out of stock or not enough.");
                        }
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException($"Product {productId} does not exist.");
                }
            }
        }

        public void DecreaseMultipleStock(List<ProductItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }
            lock (_productsLocker)
                lock (_stockLocker)
                {
                    if (items.Exists(
                        i => (_stocks.FirstOrDefault(
                            s => s.ProductId == i.ProductId && s.Count >= i.Count))
                            == null))
                    {
                        throw new InvalidOperationException($"Some products are out of stock or not enough.");
                    }
                    foreach (var item in items)
                    {
                        int productId = item.ProductId;
                        int count = item.Count;
                        var stock = _stocks.FirstOrDefault(s => s.ProductId == productId && s.Count >= count);
                        stock.Count -= count;
                        if (stock.Count == 0)
                        {
                            _stocks.Remove(stock);
                        }
                    }
                }
        }

        public int GetStock(int productId)
        {
            var stock = _stocks.FirstOrDefault(s => s.ProductId == productId);
            return stock?.Count ?? -1;
        }

        public string ListProducts()
        {
            List<string> results = new List<string>();
            foreach (Product p in _products)
            {
                var stock = _stocks.FirstOrDefault(s => s.ProductId == p.Id);
                results.Add(p.ToString() + $" Stock = {stock?.Count ?? 0}");
            }
            return results.Count > 0
                ? string.Join(Environment.NewLine, results)
                : "";
        }

        public string ListStocks()
        {
            List<string> results = new List<string>();
            foreach (ProductItem s in _stocks)
            {
                var product = _products.FirstOrDefault(p => p.Id == s.ProductId);
                results.Add($"{product?.ToString() ?? "Product Info : Unknown"} Stock = {s.Count}");
            }
            return results.Count > 0
                ? string.Join(Environment.NewLine, results)
                : "";
        }
    }
}
