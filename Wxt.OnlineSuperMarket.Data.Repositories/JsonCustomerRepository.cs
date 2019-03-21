namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Wxt.OnlineSuperMarket.Data.Entities;

    public class JsonCustomerRepository : ICustomerRepository
    {
        private const string _customerJsonFile = "customers.json";

        private const string _shoppingCartJsonFile = "shoppingcarts.json";

        private const string _customerAndCartLockerFile = "customerandcart.lk";

        private const string _receiptLockerFile = "receipts.lk";

        private readonly JsonHandler _jsonHandler = new JsonHandler();

        private readonly ShareFileLocker _FileLocker = new ShareFileLocker();

        private CustomerRecords Customers { get; set; }

        private List<ShoppingCart> ShoppingCarts { get; set; }

        class CustomerRecords
        {
            public int CustomerCurrentId { get; set; }

            public List<Customer> CustomerList { get; set; }
        }

#if DEBUG
        public void ReinitializeRepository()
#else
        private void ReinitializeRepository()
#endif
        {
            Customers = new CustomerRecords
            {
                CustomerList = new List<Customer>
                {
                    new Customer  { Id = 1, UserName = "wxt", Password = " ,�b�Y\a[�K\a\u0015-#Kp"},
                    new Customer  { Id = 2, UserName = "lnw", Password = " ,�b�Y\a[�K\a\u0015-#Kp"}
                },
                CustomerCurrentId = 2
            };

            ShoppingCarts = new List<ShoppingCart>
            {
                new ShoppingCart { CustomerId = 1, ProductItems = new List<ProductItem>()},
                new ShoppingCart { CustomerId = 2, ProductItems = new List<ProductItem>()}
            };

            _jsonHandler.SaveRecords(_customerJsonFile, Customers);
            _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);
        }

        private void GetCustomers()
        {
            Customers = _jsonHandler.GetRecords<CustomerRecords>(_customerJsonFile);
            if (Customers == null)
            {
                ReinitializeRepository();
            }
        }

        private void GetShoppingCarts()
        {
            ShoppingCarts = _jsonHandler.GetRecords<List<ShoppingCart>>(_shoppingCartJsonFile);
            if (ShoppingCarts == null)
            {
                ReinitializeRepository();
            }
        }

        public Customer AddCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.UserName) || string.IsNullOrWhiteSpace(customer.Password))
            {
                throw new ArgumentNullException("Cannot create new customer.");
            }

            FileStream locker = _FileLocker.LockObj(_customerAndCartLockerFile);

            try
            {
                GetCustomers();
                GetShoppingCarts();

                if (Customers.CustomerList.Exists(c => c.UserName == customer.UserName))
                {
                    throw new InvalidOperationException($"Username '{customer.UserName}' has been used.");
                }

                customer.Id = ++Customers.CustomerCurrentId;

                var passwordMD5 = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(customer.Password)));
                customer.Password = passwordMD5;

                Customers.CustomerList.Add(customer);
                ShoppingCarts.Add(new ShoppingCart() { CustomerId = customer.Id, ProductItems = new List<ProductItem>() });

                _jsonHandler.SaveRecords(_customerJsonFile, Customers);
                _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);

                return customer;
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public void DeleteCustomer(int customerId)
        {
            FileStream locker = _FileLocker.LockObj(_customerAndCartLockerFile);

            try
            {
                GetCustomers();
                GetShoppingCarts();

                var customer = Customers.CustomerList.FirstOrDefault(c => c.Id == customerId);
                if (customer != null)
                {
                    if (!ShoppingCarts.Remove(ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId)))
                    {
                        throw new InvalidOperationException("Unknow problem.");
                    }
                    if (!Customers.CustomerList.Remove(customer))
                    {
                        throw new InvalidOperationException("Unknow problem.");
                    }
                    _jsonHandler.SaveRecords(_customerJsonFile, Customers);
                    _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);
                }
                else
                {
                    throw new IndexOutOfRangeException("Customer does not exist.");
                }
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public int Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("UserName and Password cannot be null, empty or only white spaces.");
            }

            FileStream locker = _FileLocker.LockObj(_customerAndCartLockerFile);

            try
            {
                GetCustomers();

                var passwordMD5 = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));

                var customer = Customers.CustomerList.FirstOrDefault(c => c.UserName == userName && c.Password == passwordMD5);

                if (customer != null)
                {
                    return customer.Id;
                }
                else
                {
                    throw new IndexOutOfRangeException("Cannot login with username and password.");
                }
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public void ClearCart(int customerId)
        {
            var locker = _FileLocker.LockObj(_customerAndCartLockerFile);
            try
            {
                GetShoppingCarts();
                var shoppingCart = ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                shoppingCart.ProductItems.Clear();
                _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public void AddToCart(int customerId, int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }

            JsonSuperMarketRepository jsonSuperMarketRepository = new JsonSuperMarketRepository();

            var stockCount = jsonSuperMarketRepository.GetStock(productId);
            if (count > stockCount)
            {
                throw new InvalidOperationException($"Product {productId} is out of stock or not enough.");
            }

            FileStream locker = _FileLocker.LockObj(_customerAndCartLockerFile);

            try
            {
                GetShoppingCarts();
                var shoppingCart = ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
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
                _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public string ListShoppingCart(int customerId)
        {
            FileStream locker = _FileLocker.LockObj(_customerAndCartLockerFile);

            try
            {
                GetShoppingCarts();
                var shoppingCart = ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                if (shoppingCart.ProductItems == null || shoppingCart.ProductItems.Count <= 0)
                {
                    throw new InvalidOperationException("There is nothing in the shopping cart.");
                }

                JsonSuperMarketRepository superMarketRepository = new JsonSuperMarketRepository();

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
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public int RemoveFromCart(int customerId, int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }

            var locker = _FileLocker.LockObj(_customerAndCartLockerFile);

            try
            {
                GetShoppingCarts();

                var shoppingCart = ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
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
                _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);
                return realCount;
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }

        public Receipt CheckOut(int customerId)
        {
            var locker = _FileLocker.LockObj(_customerAndCartLockerFile);
            try
            {
                GetShoppingCarts();
                var shoppingCart = ShoppingCarts.FirstOrDefault(s => s.CustomerId == customerId);
                if (shoppingCart == null)
                {
                    throw new InvalidOperationException("Cannot find the related shopping cart.");
                }
                if (shoppingCart.ProductItems == null || shoppingCart.ProductItems.Count <= 0)
                {
                    throw new InvalidOperationException("There is nothing in the shopping cart.");
                }

                JsonSuperMarketRepository superMarketRepository = new JsonSuperMarketRepository();

                var receipt = superMarketRepository.Checkout(shoppingCart.ProductItems);

                shoppingCart.ProductItems.Clear();

                _jsonHandler.SaveRecords(_shoppingCartJsonFile, ShoppingCarts);

                return receipt;
            }
            finally
            {
                _FileLocker.UnlockObj(locker);
            }
        }
    }
}
