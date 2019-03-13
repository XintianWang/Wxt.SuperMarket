using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    class JsonCustomerRepository : ICustomerRepository
    {
        public Customer AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void AddToCart(int customerId, int productId, int count)
        {
            throw new NotImplementedException();
        }

        public Receipt CheckOut(int customerId)
        {
            throw new NotImplementedException();
        }

        public void ClearCart(int customerId)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public string ListShoppingCart(int customerId)
        {
            throw new NotImplementedException();
        }

        public int Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public int RemoveFromCart(int customerId, int productId, int count)
        {
            throw new NotImplementedException();
        }
    }
}
