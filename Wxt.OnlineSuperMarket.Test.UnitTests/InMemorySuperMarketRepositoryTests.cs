namespace Wxt.OnlineSuperMarket.Test.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Wxt.OnlineSuperMarket.Data.Repositories;
    using System;
    using Wxt.OnlineSuperMarket.Data.Entities;

    [TestClass()]
    public class InMemorySuperMarketRepositoryTests
    {
        InMemorySuperMarketRepository _inMemorySuperMarketRepository = new InMemorySuperMarketRepository();

        [TestInitialize]
        public void Preset()
        {
            _inMemorySuperMarketRepository.ReinitializeRepository();
        }

        [TestMethod()]
        public void AddProductTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _inMemorySuperMarketRepository.AddProduct(null));

            Product product = new Product() { Name = null, Price = 1.2m };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemorySuperMarketRepository.AddProduct(product));

            product = new Product() { Name = string.Empty, Price = 1.2m };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemorySuperMarketRepository.AddProduct(product));

            product = new Product() { Name = "   ", Price = 1.2m };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemorySuperMarketRepository.AddProduct(product));

            product = new Product() { Name = "name", Price = -1.0m };
            Assert.ThrowsException<ArgumentNullException>(() => _inMemorySuperMarketRepository.AddProduct(product));
        }

        [TestMethod()]
        public void AddProductTestForValidInput()
        {
            Product product = new Product() { Name = "banana", Price = 1.2m };
            Assert.AreEqual("Id = 4 Name = banana Price = 1.2 Category = UnClassified Description = banana",
                _inMemorySuperMarketRepository.AddProduct(product).ToString());
        }

        [TestMethod()]
        public void RemoveProductTestForInvalidInput()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => _inMemorySuperMarketRepository.RemoveProduct(4));
            Assert.ThrowsException<InvalidOperationException>(() => _inMemorySuperMarketRepository.RemoveProduct(3));
        }

        [TestMethod()]
        public void RemoveProductTestForValidInput()
        {
            Product product = new Product() { Name = "banana", Price = 1.2m };
            Assert.AreEqual("Id = 4 Name = banana Price = 1.2 Category = UnClassified Description = banana",
                _inMemorySuperMarketRepository.AddProduct(product).ToString());
            _inMemorySuperMarketRepository.RemoveProduct(4);
            Assert.ThrowsException<IndexOutOfRangeException>(() => _inMemorySuperMarketRepository.RemoveProduct(4));
        }

        [TestMethod()]
        public void IncreaseStockTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _inMemorySuperMarketRepository.IncreaseStock(3, -10));
            Assert.ThrowsException<IndexOutOfRangeException>(() => _inMemorySuperMarketRepository.IncreaseStock(4, 10));
        }

        [TestMethod()]
        public void IncreaseStockTestForValidInput()
        {
            Assert.AreEqual(300, _inMemorySuperMarketRepository.GetStockInner(3));
            _inMemorySuperMarketRepository.IncreaseStock(3, 10);
            Assert.AreEqual(310, _inMemorySuperMarketRepository.GetStockInner(3));

            Assert.AreEqual(-1, _inMemorySuperMarketRepository.GetStockInner(4));

            Product product = new Product() { Name = "banana", Price = 1.2m };
            Assert.AreEqual("Id = 4 Name = banana Price = 1.2 Category = UnClassified Description = banana",
                _inMemorySuperMarketRepository.AddProduct(product).ToString());

            _inMemorySuperMarketRepository.IncreaseStock(4, 10);
            Assert.AreEqual(10, _inMemorySuperMarketRepository.GetStockInner(4));
        }

        [TestMethod()]
        public void DecreaseStockTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _inMemorySuperMarketRepository.DecreaseStock(3, -10));
            Assert.ThrowsException<IndexOutOfRangeException>(() => _inMemorySuperMarketRepository.DecreaseStock(4, 10));

            Product product = new Product() { Name = "banana", Price = 1.2m };
            Assert.AreEqual("Id = 4 Name = banana Price = 1.2 Category = UnClassified Description = banana",
                _inMemorySuperMarketRepository.AddProduct(product).ToString());
            Assert.ThrowsException<InvalidOperationException>(() => _inMemorySuperMarketRepository.DecreaseStock(4, 10));
            Assert.ThrowsException<InvalidOperationException>(() => _inMemorySuperMarketRepository.DecreaseStock(3, 400));
        }

        [TestMethod()]
        public void DecreaseStockTestForValidInput()
        {
            Assert.AreEqual(300, _inMemorySuperMarketRepository.GetStockInner(3));
            _inMemorySuperMarketRepository.DecreaseStock(3, 10);
            Assert.AreEqual(290, _inMemorySuperMarketRepository.GetStockInner(3));
            _inMemorySuperMarketRepository.DecreaseStock(3, 290);
            Assert.AreEqual(-1, _inMemorySuperMarketRepository.GetStockInner(3));
        }
    }
}