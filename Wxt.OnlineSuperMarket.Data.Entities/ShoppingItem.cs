namespace Wxt.OnlineSuperMarket.Data.Entities
{
    public class ShoppingItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal PriceSum => Price * Count;

        public override string ToString()
        {
            return $"{ProductId} {ProductName} {Count} @{Price} {PriceSum}";
        }
    }
}