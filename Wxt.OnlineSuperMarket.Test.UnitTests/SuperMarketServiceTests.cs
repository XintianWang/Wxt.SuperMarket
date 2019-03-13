using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wxt.OnlineSuperMarket.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Test.UnitTests
{
    [TestClass()]
    public class SuperMarketServiceTests
    {
        private SuperMarketService _superMarketService = new SuperMarketService();

        [TestInitialize]
        public void Preset()
        {
            _superMarketService.ReinitializeRepository();
        }

        [TestMethod()]
        public void AddProuctTestForInvalidInput()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _superMarketService.AddProuct(null, 1.23m));
            Assert.ThrowsException<ArgumentNullException>(() => _superMarketService.AddProuct(string.Empty, 1.23m));
            Assert.ThrowsException<ArgumentNullException>(() => _superMarketService.AddProuct("  ", 1.23m));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _superMarketService.AddProuct("peach", -1.23m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _superMarketService.AddProuct("peach", 1.23m, null, (Category)(-11.09)));
        }

        [TestMethod()]
        public void AddProuctTestForValidInput()
        {
            Assert.AreEqual("Id = 4 Name = peach Price = 1.23 Category = UnClassified Description = peach", 
                _superMarketService.AddProuct("peach", 1.23m));

            Assert.AreEqual("Id = 5 Name = CDPlayer Price = 165 Category = Electronic Description = CDPlayer",
                _superMarketService.AddProuct("CDPlayer", 165m, null, Category.Electronic));
            Assert.AreEqual("Id = 6 Name = CDPlayer2 Price = 165 Category = Electronic Description = CDPlayer2",
                _superMarketService.AddProuct("CDPlayer2", 165m, string.Empty, Category.Electronic));
            Assert.AreEqual("Id = 7 Name = CDPlayer3 Price = 165 Category = Electronic Description = CDPlayer3",
                _superMarketService.AddProuct("CDPlayer3", 165m, "  ", Category.Electronic));

            Assert.AreEqual("Id = 8 Name = CDPlayer4 Price = 165 Category = Electronic Description = CDPlayer4",
                _superMarketService.AddProuct("CDPlayer4", 165m, category : Category.Electronic));

            Assert.AreEqual("Id = 9 Name = CDPlayer5 Price = 165 Category = Electronic Description = The last type of CDPlayer",
                _superMarketService.AddProuct("CDPlayer5", 165m, "The last type of CDPlayer", Category.Electronic));
        }
    }
}