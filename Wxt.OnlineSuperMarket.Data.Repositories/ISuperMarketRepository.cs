namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using Wxt.OnlineSuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="ISuperMarketRepository" />
    /// </summary>
    public interface ISuperMarketRepository
    {
        Product AddProduct(Product product);

        void RemoveProduct(int productId);

        void IncreaseStock(int productId, int count);

        void DecreaseStock(int productId, int count);

        int GetStock(int productId);

#if DEBUG
        void ReinitializeRepository();
#endif
    }
}
