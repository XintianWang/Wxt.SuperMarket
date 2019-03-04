using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;
using Wxt.OnlineSuperMarket.Data.Repositories;

namespace Wxt.OnlineSuperMarket.Business.Services
{
    public class SuperMarketService
    {
        private readonly ISuperMarketRepository _superMarketRepository = new InMemorySuperMarketRepository();

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
            var product = new Product()
            {
                Name = name,
                Category = category,
                Price = price,
                Description = description
            };
            return _superMarketRepository.AddProduct(product).ToString();
        }

        public bool RemoveProduct(int productId)
        {
            return _superMarketRepository.RemoveProduct(productId);
        }

        public bool IncreaseStockt(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot increase {count} product to stock.");
            }
            return _superMarketRepository.IncreaseStock(productId, count);
        }

        public bool DecreaseStockt(int productId, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException($"Cannot decrease {count} product from stock.");
            }
            return _superMarketRepository.DecreaseStock(productId, count);
        }
    }
}
