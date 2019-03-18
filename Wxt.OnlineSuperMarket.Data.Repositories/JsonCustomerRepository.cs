using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;
using Newtonsoft.Json;
using System.IO;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    class JsonCustomerRepository : ICustomerRepository
    {
        private const string _customerJsonFile = "customers.json";
        private const string _shoppingCartJsonFile = "shoppingcarts.json";
        private const string _receiptJsonFile = "receipts.json";


        private const string _customerAndCartLockerFile = "customerandcart.lk";

        private const string _receiptLockerFile = "receipts.lk";

        private readonly JsonHandler _jsonHandler = new JsonHandler();

        private readonly ShareFileLocker _FileLocker = new ShareFileLocker();

        public Customer AddCustomer(Customer customer)
        {
            var customers = ReadData<List<Customer>>(_customerJsonFile);
            
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
