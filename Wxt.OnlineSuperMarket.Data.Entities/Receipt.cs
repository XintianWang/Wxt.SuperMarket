using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wxt.OnlineSuperMarket.Data.Entities
{
    public class Receipt
    {
        public int Id { get; set; }
        public DateTimeOffset TransactionTime { get; set; }
        public List<ShoppingItem> ShoppingItems { get; set; }
        public decimal TotalPrice => ShoppingItems.Sum(s => s.PriceSum);
    }
}
