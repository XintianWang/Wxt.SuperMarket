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

        public static int _customerMaxId = 2;

        public Customer AddCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.UserName) || string.IsNullOrWhiteSpace(customer.Password))
            {
                throw new ArgumentNullException("Cannot create new customer.");
            }
            lock (_customerIdLocker)
            {
                customer.Id = ++_customerMaxId;
            }
            lock (_customersLocker)
            {
                _customers.Add(customer);
                _shoppingCarts.Add(new ShoppingCart() { CustomerId = customer.Id, ProductItems = new List<ProductItem>()});
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

        public void AddToCart(int shoppingcartId, int productId, int count)
        {

        }
    }
}
