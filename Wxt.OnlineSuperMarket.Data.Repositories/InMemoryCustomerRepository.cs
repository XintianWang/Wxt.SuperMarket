using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class InMemoryCustomerRepository
    {
        private static readonly List<Customer> _customers = new List<Customer>
        {
            new Customer  { Id = 1, UserName = "Wxt"},
            new Customer  { Id = 2, UserName = "lnw"} 
        };
        private static readonly List<ShoppingCart> _shoppingCarts = new List<ShoppingCart>
        {
            new ShoppingCart { CustomerId = 1, ProductItems = new List<ProductItem>()},
            new ShoppingCart { CustomerId = 2, ProductItems = new List<ProductItem>()}
        };
    }
}
