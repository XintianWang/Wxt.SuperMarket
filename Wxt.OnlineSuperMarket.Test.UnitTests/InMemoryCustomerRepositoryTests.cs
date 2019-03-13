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
        InMemoryCustomerRepository inMemoryCustomerRepository = new InMemoryCustomerRepository();

        [TestInitialize]
        public void Preset()
        {
            inMemoryCustomerRepository.ReinitializeRepository();
        }

        [TestMethod()]
        public void AddCustomerTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(null));
            Customer customer = new Customer
            {
                UserName = null,
                Password = "dsafdsf"
            };
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = string.Empty,
                Password = "dsafdsf"
            };
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "    ",
                Password = "dsafdsf"
            };
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "sdfdsf",
                Password = null
            };
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "sdfdsf",
                Password = string.Empty
            };
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "sdfdsf",
                Password = "     "
            };
            Assert.ThrowsException<ArgumentNullException>(() => inMemoryCustomerRepository.AddCustomer(customer));
            customer = new Customer
            {
                UserName = "wxt",
                Password = "12345"
            };
            Assert.ThrowsException<InvalidOperationException>(() => inMemoryCustomerRepository.AddCustomer(customer));
        }
    }
}