namespace Wxt.OnlineSuperMarket.Business.Services
{
    using System;
    using Wxt.OnlineSuperMarket.Data.Entities;
    using Wxt.OnlineSuperMarket.Data.Repositories;

    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository = new JsonCustomerRepository();

        private int CustomerId { get; set; } = 0;

        public string AddCustomer(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Username and Password cannot be null, empty or only white spaces.");
            }

            return _customerRepository.AddCustomer(
                new Customer
                {
                    UserName = name,
                    Password = password
                }).ToString();
        }

        public void Login(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Username and Password cannot be null, empty or only white spaces.");
            }
            CustomerId = _customerRepository.Login(name, password);
        }

        public bool Logout()
        {
            bool result = false;
            if (CustomerId > 0)
            {
                CustomerId = 0;
                result = true;
            }
            return result;
        }

        public bool DeleteCustomer()
        {
            bool result = false;
            if (CustomerId > 0)
            {
                int id = CustomerId;
                CustomerId = 0;
                _customerRepository.DeleteCustomer(id);
                result = true;
            }
            return result;
        }

        public void AddToCart(int productId, int count)
        {
            if (CustomerId <= 0)
            {
                throw new InvalidOperationException("Login first.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }
            _customerRepository.AddToCart(CustomerId, productId, count);
        }

        public int RemoveFromCart(int productId, int count)
        {
            if (CustomerId <= 0)
            {
                throw new InvalidOperationException("Login first.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }
            return _customerRepository.RemoveFromCart(CustomerId, productId, count);
        }

        public string ListCart()
        {
            if (CustomerId <= 0)
            {
                throw new InvalidOperationException("Login first.");
            }
            return _customerRepository.ListShoppingCart(CustomerId);
        }

        public void ClearCart()
        {
            if (CustomerId <= 0)
            {
                throw new InvalidOperationException("Login first.");
            }
            _customerRepository.ClearCart(CustomerId);
        }

        public string CheckOut()
        {
            if (CustomerId <= 0)
            {
                throw new InvalidOperationException("Login first.");
            }
            return _customerRepository.CheckOut(CustomerId).ToString();
        }
    }
}

