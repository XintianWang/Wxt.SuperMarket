using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wxt.OnlineSuperMarket.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Test.UnitTests
{
    [TestClass()]
    public class InMemoryCustomerRepositoryTests
    {
        private InMemoryCustomerRepository _inMemoryCustomerRepository = new InMemoryCustomerRepository();

        [TestInitialize]
        public void Preset()
        {
            _inMemoryCustomerRepository.ReinitializeRepository();
        }

        [TestMethod()]
        public void AddCustomerTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(null));
            Customer customer = new Customer
            {
                UserName = null,
                Password = "dsafdsf"
            };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = string.Empty,
                Password = "dsafdsf"
            };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "    ",
                Password = "dsafdsf"
            };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "sdfdsf",
                Password = null
            };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "sdfdsf",
                Password = string.Empty
            };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "sdfdsf",
                Password = "     "
            };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "wxt",
                Password = "12345"
            };
            Assert.ThrowsException<InvalidOperationException>(() => _inMemoryCustomerRepository.AddCustomer(customer));
        }

        [TestMethod()]
        public void AddCustomerTestForValidInput()
        {
            Customer customer = new Customer
            {
                UserName = "test",
                Password = "123"
            };
            Assert.ThrowsException<NullReferenceException>(() => _inMemoryCustomerRepository.ListShoppingCart(3));
            Assert.AreEqual("Id = 3 UserName = test EmailAddress =  TelephoneNumber = ", 
                _inMemoryCustomerRepository.AddCustomer(customer).ToString());
            Assert.AreEqual(" ,�b�Y\a[�K\a\u0015-#Kp", customer.Password);
            Assert.ThrowsException<InvalidOperationException>(() => _inMemoryCustomerRepository.ListShoppingCart(3));
        }

        [TestMethod()]
        public void LoginTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.Login(null, "123"));

            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.Login(string.Empty, "123"));

            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.Login("  ", "123"));

            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.Login("wxt", null));

            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.Login("wxt", string.Empty));

            Assert.ThrowsException<ArgumentNullException>(() => _inMemoryCustomerRepository.Login("wxt", "  "));

            Assert.ThrowsException<IndexOutOfRangeException>(() => _inMemoryCustomerRepository.Login("wxt", "456"));

            Assert.ThrowsException<IndexOutOfRangeException>(() => _inMemoryCustomerRepository.Login("wxt1", "123"));
        }

        [TestMethod()]
        public void LoginTestForValidInput()
        {
            Assert.AreEqual(1, _inMemoryCustomerRepository.Login("wxt", "123"));
            Assert.AreEqual(2, _inMemoryCustomerRepository.Login("lnw", "123"));
            Customer customer = new Customer
            {
                UserName = "test",
                Password = "1234"
            };
            _inMemoryCustomerRepository.AddCustomer(customer);
            Assert.AreEqual(3, _inMemoryCustomerRepository.Login("test", "1234"));
        }

        [TestMethod()]
        public void DeleteCustomerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddToCartTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveFromCartTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ClearCartTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListShoppingCartTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckOutTest()
        {
            Assert.Fail();
        }


    }
}