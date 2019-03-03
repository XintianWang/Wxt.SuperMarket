namespace Wxt.OnlineSuperMarket.Data.Entities
{
    public class ShoppingItem
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal PriceSum => Price * Count;
    }
}