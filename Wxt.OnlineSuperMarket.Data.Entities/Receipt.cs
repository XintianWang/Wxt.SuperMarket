namespace Wxt.OnlineSuperMarket.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Receipt
    {
        public int Id { get; set; }

        public DateTimeOffset TransactionTime { get; set; }

        public List<ShoppingItem> ShoppingItems { get; set; }

        public decimal TotalPrice => ShoppingItems.Sum(s => s.PriceSum);

        public override string ToString()
        {
            return $"Id = {Id} TransactionTime = {TransactionTime}{Environment.NewLine}" +
                $"TotalPrice = {TotalPrice}{Environment.NewLine}" +
                $"{string.Join<string>(Environment.NewLine, ShoppingItems.Select(s => s.ToString()))}";
        }
    }
}
