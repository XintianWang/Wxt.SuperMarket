using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Data.Entities;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    class JsonSuperMarketRepository : ISuperMarketRepository
    {
        private const string productFile = "products.json";

        public Product AddProduct(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name) || product.Price < 0m)
            {
                throw new ArgumentNullException("Cannot create new product.");
            }
            lock (_productIdLocker)
            {
                product.Id = ++_productCurrentId;
            }
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                product.Description = product.Name;
            }
            lock (_productsLocker)
            {
                _products.Add(product);
            }
            return product;
        }

        public void DecreaseMultipleStock(List<ProductItem> items)
        {
            throw new NotImplementedException();
        }

        public void DecreaseStock(int productId, int count)
        {
            throw new NotImplementedException();
        }

        public Product FindProduct(int id)
        {
            throw new NotImplementedException();
        }

        public int GetStock(int productId)
        {
            throw new NotImplementedException();
        }

        public void IncreaseStock(int productId, int count)
        {
            throw new NotImplementedException();
        }

        public string ListProducts()
        {
            throw new NotImplementedException();
        }

        public string ListStocks()
        {
            throw new NotImplementedException();
        }

        public void ReinitializeRepository()
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
