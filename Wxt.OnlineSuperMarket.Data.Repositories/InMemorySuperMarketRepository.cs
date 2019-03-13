namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wxt.OnlineSuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="InMemorySuperMarketRepository" />
    /// </summary>
    public class InMemorySuperMarketRepository : ISuperMarketRepository
    {
        /// <summary>
        /// Defines the _products as the inmemory database of all products and inserts some default records.
        /// </summary>
        private static readonly List<Product> _products = new List<Product>()
        {
            new Product{ Id = 1, Name = "banana", Category = Category.Grocery, Description = "Banana from Mexico", Price = 1.67m},
            new Product{ Id = 2, Name = "apple", Category = Category.Grocery, Description = "Apple from China", Price = 2.67m},
            new Product{ Id = 3, Name = "Television", Category = Category.Electronic, Description = "Sony 65\"", Price = 1600.59m}
        };

        /// <summary>
        /// Defines the _stocks as the inmemory database of all stocks and inserts some default records.
        /// </summary>
        private static readonly List<ProductItem> _stocks = new List<ProductItem>()
        {
            new ProductItem {ProductId = 1, Count = 100},
            new ProductItem {ProductId = 2, Count = 200},
            new ProductItem {ProductId = 3, Count = 300},
        };

        /// <summary>
        /// Defines the _receipts as the inmemory database of all receipts.
        /// </summary>
        private static readonly List<Receipt> _receipts = new List<Receipt>();

        /// <summary>
        /// Defines the _productCurrentId to be used when create new product, lock _productIdLocker in multi-thread.
        /// </summary>
        public static int _productCurrentId = 3;

        /// <summary>
        /// Defines the _productIdLocker as the locker for modify _productCurrentId.
        /// </summary>
        private static readonly object _productIdLocker = new object();

        /// <summary>
        /// Defines the _productsLocker as the locker for modify _products.
        /// </summary>
        private static readonly object _productsLocker = new object();

        /// <summary>
        /// Defines the _stockLocker as the locker for modify _stocks.
        /// </summary>
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
        
        /// <summary>
        /// The AddProduct method checks input and adds a new product to _products.
        /// </summary>
        /// <param name="product">The product to be added without Id<see cref="Product"/></param>
        /// <returns>The new added product with Id<see cref="Product"/></returns>
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

        /// <summary>
        /// The FindProduct returns a product with the inputed id.
        /// </summary>
        /// <param name="id">The product id<see cref="int"/></param>
        /// <returns>The product with the inputed id or null if cannot find one <see cref="Product"/></returns>
        private Product FindProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// The RemoveProduct method removes product with inputed id from _products.
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
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

        /// <summary>
        /// The IncreaseStock method increases the count of stock of one particular product to _stocks.
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
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

        /// <summary>
        /// The DecreaseStock method decreases the count of stock of one particular product from _stocks.
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
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

        /// <summary>
        /// The GetStock get count of one particular product in the stock.
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int GetStock(int productId)
        {
            var stock = _stocks.FirstOrDefault(s => s.ProductId == productId);
            return stock?.Count ?? -1;
        }
    }
}
