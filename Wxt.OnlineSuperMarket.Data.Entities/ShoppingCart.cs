using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wxt.OnlineSuperMarket.Data.Entities
{
    public class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<ProductItem> ProductItems { get; set; }
    }
}
