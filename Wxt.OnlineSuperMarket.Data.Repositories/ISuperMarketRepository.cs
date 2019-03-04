namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using Wxt.OnlineSuperMarket.Data.Entities;

    public interface ISuperMarketRepository
    {
        Product AddProduct(Product product);

        bool RemoveProduct(int productId);

        bool IncreaseStock(int productId, int count);

        bool DecreaseStock(int productId, int count);
    }
}
