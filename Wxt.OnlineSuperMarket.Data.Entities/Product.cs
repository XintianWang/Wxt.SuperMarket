namespace Wxt.OnlineSuperMarket.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public override string ToString()
        {
            return $"Id = {Id} Name = {Name} Price = {Price} Category = {Category} Description = {Description}";
        }
    }
}
