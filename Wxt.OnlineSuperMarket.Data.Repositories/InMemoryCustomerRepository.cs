using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class InMemoryCustomerRepository
    {
        private static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer  { Id = 1, UserName = "Wxt", Password = "1�\u001bt�h]>�+� ��w�"},
            new Customer  { Id = 2, UserName = "lnw", Password = ",6s�@���\u000e\a�e�enN"} 
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

        private readonly ISuperMarketRepository _superMarketRepository = new InMemorySuperMarketRepository();

        public Customer AddCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.UserName) || string.IsNullOrWhiteSpace(customer.Password))
            {
                throw new ArgumentNullException("Cannot create new customer.");
            }
            lock (_customerIdLocker)
            {
                customer.Id = ++_customerCurrentId;
            }
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
                    throw new InvalidOperationException("Cannot find the relative shopping cart.");
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
                    throw new InvalidOperationException("Cannot find the relative shopping cart.");
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
    }
}
