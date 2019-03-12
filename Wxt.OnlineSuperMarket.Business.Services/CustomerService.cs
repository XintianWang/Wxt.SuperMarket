using System;
using Wxt.OnlineSuperMarket.Data.Entities;
using Wxt.OnlineSuperMarket.Data.Repositories;

namespace Wxt.OnlineSuperMarket.Business.Services
{
    /// <summary>
    /// Defines the <see cref="CustomerService" />
    /// </summary>
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository = new InMemoryCustomerRepository();

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

        public void Logout()
        {
            CustomerId = 0;
        }

        public void DeleteCustomer()
        {
            if (CustomerId > 0)
            {
                int id = CustomerId;
                CustomerId = 0;
                _customerRepository.DeleteCustomer(id);
            }
        }

        public void AddToCart(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }
            _customerRepository.AddToCart(CustomerId, productId, count);
        }

        public int RemoveFromCart(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot add {count} product to cart.");
            }
            return _customerRepository.RemoveFromCart(CustomerId, productId, count);
        }

        public string CheckOut()
        {
            return _customerRepository.CheckOut(CustomerId).ToString();
        }


    }
}
