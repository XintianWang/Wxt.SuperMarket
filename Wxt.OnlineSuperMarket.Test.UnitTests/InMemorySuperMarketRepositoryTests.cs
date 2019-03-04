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
    public class InMemorySuperMarketRepositoryTests
    {
        [TestMethod()]
        public void AddProductTestForInvalidInput()
        {
            InMemorySuperMarketRepository inMemorySuperMarketRepository = new InMemorySuperMarketRepository();
            Assert.ThrowsException<ArgumentNullException>(() => inMemorySuperMarketRepository.AddProduct(null));

            Product product = new Product() { Name = null, Price = 1.2m };
            Assert.ThrowsException<ArgumentNullException>(() => inMemorySuperMarketRepository.AddProduct(product));

            product = new Product() { Name = string.Empty, Price = 1.2m };
            Assert.ThrowsException<ArgumentNullException>(() => inMemorySuperMarketRepository.AddProduct(product));

            product = new Product() { Name = "   ", Price = 1.2m };
            Assert.ThrowsException<ArgumentNullException>(() => inMemorySuperMarketRepository.AddProduct(product));

            product = new Product() { Name = "name", Price = -1.0m };
            Assert.ThrowsException<ArgumentNullException>(() => inMemorySuperMarketRepository.AddProduct(product));
        }

        [TestMethod()]
        public void AddProductTestForValidInput()
        {
            InMemorySuperMarketRepository inMemorySuperMarketRepository = new InMemorySuperMarketRepository();

            Product product = new Product() { Name = "banana", Price = 1.2m };
            Assert.AreEqual("Id = 4 Name = banana Price = 1.2 Category = UnClassified Description = banana",
                inMemorySuperMarketRepository.AddProduct(product).ToString());
        }

        [TestMethod()]
        public void RemoveProductTestForInvalidInput()
        {
            InMemorySuperMarketRepository inMemorySuperMarketRepository = new InMemorySuperMarketRepository();

            Assert.ThrowsException<IndexOutOfRangeException>(() => inMemorySuperMarketRepository.RemoveProduct(4));
            Assert.ThrowsException<InvalidOperationException>(() => inMemorySuperMarketRepository.RemoveProduct(3));
        }

        [TestMethod()]
        public void RemoveProductTestForValidInput()
        {
            InMemorySuperMarketRepository inMemorySuperMarketRepository = new InMemorySuperMarketRepository();
            Product product = new Product() { Name = "banana", Price = 1.2m };
            Assert.AreEqual("Id = 4 Name = banana Price = 1.2 Category = UnClassified Description = banana",
                inMemorySuperMarketRepository.AddProduct(product).ToString());
            inMemorySuperMarketRepository.RemoveProduct(4);
            Assert.ThrowsException<IndexOutOfRangeException>(() => inMemorySuperMarketRepository.RemoveProduct(4));
        }
    }
}