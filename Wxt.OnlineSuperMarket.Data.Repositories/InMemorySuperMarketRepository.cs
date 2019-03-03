using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class InMemorySuperMarketRepository
    {
        private static readonly List<Product> _products = new List<Product>();
        private static readonly List<ProductItem> _stock = new List<ProductItem>();
        private static readonly List<Receipt> _receipts = new List<Receipt>();
    }
}
