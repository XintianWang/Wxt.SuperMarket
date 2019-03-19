namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using System.Collections.Generic;
    using Wxt.OnlineSuperMarket.Data.Entities;

    public interface ISuperMarketRepository
    {
#if DEBUG
        void ReinitializeRepository();
#endif
        Product AddProduct(Product product);

        void RemoveProduct(int productId);

        void IncreaseStock(int productId, int count);

        void DecreaseStock(int productId, int count);

        Receipt Checkout(List<ProductItem> items);

        int GetStock(int productId);

        Product FindProduct(int id);

        string ListProducts();

        string ListStocks();

        string ListReceipts();
    }
}
