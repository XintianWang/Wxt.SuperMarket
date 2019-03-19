namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Wxt.OnlineSuperMarket.Data.Entities;

    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer  { Id = 1, UserName = "wxt", Password = " ,�b�Y\a[�K\a\u0015-#Kp"},
            new Customer  { Id = 2, UserName = "lnw", Password = " ,�b�Y\a[�K\a\u0015-#Kp"}
        };

        private static readonly List<ShoppingCart> _shoppingCarts = new List<ShoppingCart>
        {
            new ShoppingCart { CustomerId = 1, ProductItems = new List<ProductItem>()},
            new ShoppingCart { CustomerId = 2, ProductItems = new List<ProductItem>()}
        };

        private static readonly object _customerIdLocker = new object();

        private static readonly object _customersLocker = new object();

        private static readonly object _shoppingCartsLocker = new object();

        public static int _customerCurrentId = 2;

#if DEBUG
        public void ReinitializeRepository()
        {
            _customers.Clear();
            _customers.AddRange(new List<Customer>
            {
                new Customer  { Id = 1, UserName = "wxt", Password = " ,�b�Y\a[�K\a\u0015-#Kp"},
                new Customer  { Id = 2, UserName = "lnw", Password = " ,�b�Y\a[�K\a\u0015-#Kp"}
            });

            _shoppingCarts.Clear();
            _shoppingCarts.AddRange(new List<ShoppingCart>
            {
                new ShoppingCart { CustomerId = 1, ProductItems = new List<ProductItem>()},
                new ShoppingCart { CustomerId = 2, ProductItems = new List<ProductItem>()}
            });
        }
#endif

#if DEBUG
        public void ReinitializeRepository()
        {
            _customers.Clear();
            _customers.AddRange(
                new List<Customer> {
                    new Customer  { Id = 1, UserName = "wxt", Password = " ,�b�Y\a[�K\a\u0015-#Kp"},
                    new Customer  { Id = 2, UserName = "lnw", Password = " ,�b�Y\a[�K\a\u0015-#Kp"}
                }
            );

            _shoppingCarts.Clear();
            _shoppingCarts.AddRange(
                new List<ShoppingCart>{
                    new ShoppingCart { CustomerId = 1, ProductItems = new List<ProductItem>()},
                    new ShoppingCart { CustomerId = 2, ProductItems = new List<ProductItem>()}
                });

            _customerCurrentId = 2;
        }
#endif

        public Customer AddCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.UserName) || string.IsNullOrWhiteSpace(customer.Password))
            {
                throw new ArgumentNullException("Cannot create new customer.");
            }
            if (_customers.Exists(c => c.UserName == customer.UserName))
            {
                throw new InvalidOperationException($"Username '{customer.UserName}' has been used.");
            }
            lock (_customerIdLocker)
            {
                customer.Id = ++_customerCurrentId;
            }
            var passwordMD5 = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(customer.Password)));
            customer.Password = passwordMD5;
            lock (_customersLocker)
            {
                _customers.Add(customer);
                lock (_shoppingCartsLocker)
                {
                    _shoppingCarts.Add(new ShoppingCart() { CustomerId = customer.Id, ProductItems = new List<ProductItem>() });
                }
            }
            return customer;
        }

        public int Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("UserName and Password cannot be null, empty or only white spaces.");
            }
            var passwordMD5 = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
            var customer = _customers.FirstOrDefault(c => c.UserName == userName && c.Password == passwordMD5);
            if (customer != null)
            {
                return customer.Id;
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot login with username and password.");
            }
        }

        public void DeleteCustomer(int customerId)
        {
            lock (_customerIdLocker)
            {
                var customer = _customers.FirstOrDefault(c => c.Id == customerId);
                if (customer != null)
                {
                    lock (_shoppingCartsLocker)
                    {
                        if (!_shoppingCarts.Remove(_shoppingCarts.FirstOrDefault(s => s.CustomerId == customerId)))
                        {
                            throw new InvalidOperationException("Unknow problem.");
                        }
                    }
                    if (!_customers.Remove(customer))
                    {
                        throw new InvalidOperationException("Unknow problem.");
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException("Customer does not exist.");
                }
            }
        }

        public void AddToCart(int customerId, int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }
            InMemorySuperMarketRepository _superMarketRepository = new InMemorySuperMarketRepository();

            var stockCount = _superMarketRepository.GetStock(productId);
            if (count > stockCount)
            {
                throw new InvalidOperationException($"Product {productId} is out of stock or not enough.");
            }
            lock (_shoppingCartsLocker)
            {
                var shoppingCart = _shoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                var item = shoppingCart.ProductItems.FirstOrDefault(i => i.ProductId == productId);
                if (item == null)
                {
                    shoppingCart.ProductItems.Add(new ProductItem() { ProductId = productId, Count = count });
                }
                else
                {
                    item.Count += count;
                }
            }
        }

        public int RemoveFromCart(int customerId, int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }
            lock (_shoppingCartsLocker)
            {
                var shoppingCart = _shoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                var item = shoppingCart.ProductItems.FirstOrDefault(i => i.ProductId == productId);
                var countInCart = item?.Count ?? 0;
                int realCount = Math.Min(count, countInCart);
                if (realCount > 0)
                {
                    item.Count -= realCount;
                    if (item.Count == 0)
                    {
                        shoppingCart.ProductItems.Remove(item);
                    }
                }
                return realCount;
            }
        }

        public void ClearCart(int customerId)
        {
            lock (_shoppingCartsLocker)
            {
                var shoppingCart = _shoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                shoppingCart.ProductItems.Clear();
            }
        }

        public string ListShoppingCart(int customerId)
        {
            var shoppingCart = _shoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
            if (shoppingCart == null)
            {
                throw new NullReferenceException("Cannot find the related shopping cart.");
            }
            if (shoppingCart.ProductItems == null || shoppingCart.ProductItems.Count <= 0)
            {
                throw new InvalidOperationException("There is nothing in the shopping cart.");
            }

            InMemorySuperMarketRepository superMarketRepository = new InMemorySuperMarketRepository();

            List<string> results = new List<string>();
            foreach (ProductItem s in shoppingCart.ProductItems)
            {
                var product = superMarketRepository.FindProduct(s.ProductId);
                results.Add($"{product?.ToString() ?? "Product Info : Unknown"} Count = {s.Count}");
            }
            return results.Count > 0
                ? string.Join(Environment.NewLine, results)
                : "";
        }

        public Receipt CheckOut(int customerId)
        {
            lock (_shoppingCartsLocker)
            {
                var shoppingCart = _shoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                if (shoppingCart.ProductItems == null || shoppingCart.ProductItems.Count <= 0)
                {
                    throw new InvalidOperationException("There is nothing in the shopping cart.");
                }

                InMemorySuperMarketRepository superMarketRepository = new InMemorySuperMarketRepository();
                var receipt = superMarketRepository.Checkout(shoppingCart.ProductItems);

                shoppingCart.ProductItems.Clear();
                return receipt;
            }
        }
    }
}
