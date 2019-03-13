namespace Wxt.OnlineSuperMarket.Data.Entities
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public int CustomerId { get; set; }

        public List<ProductItem> ProductItems { get; set; }
    }
}
