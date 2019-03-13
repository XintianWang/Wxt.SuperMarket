namespace Wxt.OnlineSuperMarket.Business.Services
{
    using System;
    using Wxt.OnlineSuperMarket.Data.Entities;
    using Wxt.OnlineSuperMarket.Data.Repositories;

    /// <summary>
    /// Defines the <see cref="SuperMarketService" />
    /// </summary>
    public class SuperMarketService
    {
        /// <summary>
        /// Defines the _superMarketRepository
        /// </summary>
        private readonly ISuperMarketRepository _superMarketRepository = new InMemorySuperMarketRepository();

        /// <summary>
        /// The AddProuct method checks input and adds a new product to the backend repository.
        /// </summary>
        /// <param name="name">New product's name<see cref="string"/></param>
        /// <param name="price">New product's price<see cref="decimal"/></param>
        /// <param name="description">New product's description<see cref="string"/></param>
        /// <param name="category">New product's category<see cref="Category"/></param>
        /// <returns>The whole information of new added product<see cref="string"/></returns>
        public string AddProuct(string name, decimal price, string description = null, Category category = Category.UnClassified)
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
                throw new ArgumentOutOfRangeException($"Product category {category} is not defined.");
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

        /// <summary>
        /// The RemoveProduct method removes an existing product from backend repository.
        /// </summary>
        /// <param name="productId">The Id of the product to be removed<see cref="int"/></param>
        public void RemoveProduct(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentOutOfRangeException("Product's Id is always bigger than 0.");
            }
            _superMarketRepository.RemoveProduct(productId);
        }

        /// <summary>
        /// The IncreaseStockt method increases the count of stock of one particular product to backend repository.
        /// </summary>
        /// <param name="productId">The Id of the product to be increased<see cref="int"/></param>
        /// <param name="count">The count to be increased<see cref="int"/></param>
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

        /// <summary>
        /// The DecreaseStockt method decreases the count of stock of one particular product from backend repository.
        /// </summary>
        /// <param name="productId">The Id of the product to be decreased<see cref="int"/></param>
        /// <param name="count">The count to be decreased<see cref="int"/></param>
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
    }
}
