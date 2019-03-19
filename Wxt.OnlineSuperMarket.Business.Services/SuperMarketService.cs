namespace Wxt.OnlineSuperMarket.Business.Services
{
    using System;
    using Wxt.OnlineSuperMarket.Data.Entities;
    using Wxt.OnlineSuperMarket.Data.Repositories;

    public class SuperMarketService
    {
        private readonly ISuperMarketRepository _superMarketRepository = new JsonSuperMarketRepository();

#if DEBUG
        public void ReinitializeRepository()
        {
            _superMarketRepository.ReinitializeRepository();
        }

#endif
        public string AddProuct(string name, decimal price, string description = null, Category category = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Product name cannot be null, empty or only white spaces.");
            }
            if (price < 0m)
            {
                throw new ArgumentOutOfRangeException("Price cannot be less than 0.");
            }
            if (!Enum.IsDefined(typeof(Category), category))
            {
                throw new ArgumentOutOfRangeException("Category value is invalid.");
            }
            var product = new Product()
            {
                Name = name.Trim(),
                Category = category,
                Price = price,
                Description = description
            };
            return _superMarketRepository.AddProduct(product).ToString();
        }

        public void RemoveProduct(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentOutOfRangeException("Product's Id is always bigger than 0.");
            }
            _superMarketRepository.RemoveProduct(productId);
        }

        public void IncreaseStockt(int productId, int count)
        {
            if (productId <= 0)
            {
                throw new ArgumentOutOfRangeException("Product's Id is always bigger than 0.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot increase {count} product to stock.");
            }
            _superMarketRepository.IncreaseStock(productId, count);
        }

        public void DecreaseStockt(int productId, int count)
        {
            if (productId <= 0)
            {
                throw new ArgumentOutOfRangeException("Product's Id is always bigger than 0.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot decrease {count} product from stock.");
            }
            _superMarketRepository.DecreaseStock(productId, count);
        }

        public string ListAllProducts()
        {
            return _superMarketRepository.ListProducts();
        }

        public string ListAllStocks()
        {
            return _superMarketRepository.ListStocks();
        }
    }
}
